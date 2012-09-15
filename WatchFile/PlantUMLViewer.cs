using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Drawing.Imaging;

namespace WatchFile
{
	public partial class PlantUMLViewer : Form
	{
		// Change cursor when building png-file

		string _plantUmlJarFIle = "";
		string _javaExe = "";
		string _tempFilePath = "";
		string _tempFileName = "";

		bool _refreshImage = false;

		public PlantUMLViewer()
		{
			InitializeComponent();

			_plantUmlJarFIle = ConfigurationManager.AppSettings["PlantUML"];
			_javaExe = ConfigurationManager.AppSettings["JavaExecutable"];
			_tempFilePath = ConfigurationManager.AppSettings["TempFileDir"].TrimEnd('\\').TrimEnd('/') + "\\";

			_tempFileName = Guid.NewGuid().ToString() + ".png";

			pictureBox.Size = new Size(0, 0);

			this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
			timer1.Enabled = true;

			this.FormClosed += new FormClosedEventHandler(PlantUMLViewer_FormClosed);
			this.FormClosing += new FormClosingEventHandler(PlantUMLViewer_FormClosing);
		}

		private void PlantUMLViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			timer1.Enabled = false; 
			fileSystemWatcher1.EnableRaisingEvents = false;			
		}

		private void PlantUMLViewer_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{				
				if (File.Exists(_tempFilePath + _tempFileName))
				{
					File.Delete(_tempFilePath + _tempFileName);
				}
			}
			catch { }
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.S | Keys.Control))
			{
				btnSave.PerformClick();
				return true;
			}
			else if (keyData == (Keys.F | Keys.Control))
			{
				btnWatchFile.PerformClick();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Add))
			{
				relativeSize.UpButton();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Subtract))
			{
				relativeSize.DownButton();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Home))
			{
				relativeSize.Value = 100;
				return true;
			}	

			
			return base.ProcessCmdKey(ref msg, keyData);
		}


		private void Form1_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta == 0 || pictureBox.Image == null)
				return;

			decimal ticks =  (e.Delta / 120);
			decimal newValue = relativeSize.Value + (ticks * relativeSize.Increment);
			if (newValue >= relativeSize.Minimum && newValue <= relativeSize.Maximum)
				relativeSize.Value += (ticks * relativeSize.Increment);
			else if (newValue < relativeSize.Minimum)
				relativeSize.Value = relativeSize.Minimum;
			else
				relativeSize.Value = relativeSize.Maximum;
		}

		private void btnWatchFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog();
			fd.CheckFileExists = true;
			fd.CheckPathExists = true;
			fd.Multiselect = false;		
			fd.Filter = "*.uml (UML)|*.uml|*.txt (TXT)| *.txt";		
			if (fd.ShowDialog() != DialogResult.OK)
			{
				lblFileName.Text = String.Empty;
				fileSystemWatcher1.EnableRaisingEvents = false;				
				return;
			}

			lblFileName.Text = fd.FileName;
			_refreshImage = true;
			relativeSize.Value = 100;
			fileSystemWatcher1.Path = Path.GetDirectoryName(fd.FileName);
			fileSystemWatcher1.Filter = Path.GetFileName(fd.SafeFileName);			
			fileSystemWatcher1.EnableRaisingEvents = true;			
		}


		private bool loadFile(string fullPathToFile)
		{
			Cursor oldCursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				System.Diagnostics.Process process = new System.Diagnostics.Process();
				System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
				startInfo.FileName = "cmd.exe";
				startInfo.Arguments = String.Format("/C type \"{0}\" | \"{1}\" -jar \"{2}\" -pipe > {3}", lblFileName.Text, _javaExe, _plantUmlJarFIle, _tempFilePath + _tempFileName);

				process.StartInfo = startInfo;

				process.Start();
				process.WaitForExit();
				if (process.ExitCode != 0)
				{
					MessageBox.Show("ExitCode: " + process.ExitCode.ToString());
					return false;
				}

				pictureBox.Load(_tempFilePath + _tempFileName);
				relativeSize_ValueChanged(null, null);

				lblLatestEdit.Text = String.Format("Last edit (plantuml-file): {0}", File.GetLastWriteTime(lblFileName.Text));

				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				return false;
			}
			finally
			{
				Cursor.Current = oldCursor;
			}
		
		}

		private void relativeSize_ValueChanged(object sender, EventArgs e)
		{			
			pictureBox.Width = (int)(pictureBox.Image.Width * ((relativeSize.Value) / 100));
			pictureBox.Height = (int)(pictureBox.Image.Height * ((relativeSize.Value) / 100));
		}

		private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
			if (e.ChangeType == System.IO.WatcherChangeTypes.Changed)
			{
				fileSystemWatcher1.EnableRaisingEvents = false;
				_refreshImage = true; 
			}
			else if (e.ChangeType == System.IO.WatcherChangeTypes.Deleted || e.ChangeType == System.IO.WatcherChangeTypes.Renamed)
			{
				fileSystemWatcher1.EnableRaisingEvents = false;
				pictureBox.Image = null;
				pictureBox.Size = new Size(0, 0);					
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (!_refreshImage || String.IsNullOrEmpty(lblFileName.Text))
				return;

			if (loadFile(lblFileName.Text))
			{
				_refreshImage = false;
				fileSystemWatcher1.EnableRaisingEvents = true;
			}						
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (pictureBox.Image != null)
			{
				SaveFileDialog fd = new SaveFileDialog();
				fd.CheckPathExists = true;
				fd.Filter = "*.png (PNG)|*.png";
				fd.Title = "Gem som...";
				fd.OverwritePrompt = true;

				if (fd.ShowDialog() != DialogResult.OK)				
					return;
				try 
				{
					Stream stream = fd.OpenFile();					
					pictureBox.Image.Save(stream, ImageFormat.Png);
					stream.Close();
					MessageBox.Show("Filen er gemt som:\n" + fd.FileName);
				}
				catch (Exception _e)
				{
					MessageBox.Show("Kunne ikke gemme! Fejlen er:\n" + _e.Message);
				}				
			}
		}
	}
}
