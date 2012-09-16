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
		DateTime _lastWriteDateTime = DateTime.MinValue;

		ResentFileHandler _resentFileHandler = null;

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

			panel1.Click += new EventHandler(panel1_Click);

			_resentFileHandler = new ResentFileHandler(recentToolStripMenuItem, new ResentFileHandler.OpenRecentFileHandler(OnResentFile)); 			
		}

		private void OnResentFile(string filename)
		{
			loadFile(filename);
		}

		private void panel1_Click(object sender, EventArgs e)
		{
			panel1.Focus();
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


		private void Form1_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta == 0 || pictureBox.Image == null)
				return;

			int ticks =  (e.Delta / -120);			
			int  newValue = (int) panel1.VerticalScroll.Value + (ticks * panel1.VerticalScroll.LargeChange);
			if (newValue >= panel1.VerticalScroll.Minimum && newValue <= panel1.VerticalScroll.Maximum)
				panel1.VerticalScroll.Value = newValue;
			else if (newValue < panel1.VerticalScroll.Minimum)
				panel1.VerticalScroll.Value = panel1.VerticalScroll.Minimum;
			else
				panel1.VerticalScroll.Value = panel1.VerticalScroll.Maximum;
		}

		private void btnWatchFile_Click(object sender, EventArgs e)
		{
			openToolStripMenuItem.PerformClick();
		}


		private bool loadFile(string fullPathToFile)
		{
			Cursor oldCursor = Cursor.Current;
			try
			{
				if (!File.Exists(fullPathToFile))
				{
					MessageBox.Show("File not found!\nFile requested: " + fullPathToFile, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					_resentFileHandler.Remove(fullPathToFile);
					pictureBox.Image = null;
					toolStripStatusAutoUpdate.Text = String.Empty;
					return false;
				}

				_resentFileHandler.Add(fullPathToFile);
				lblFileName.Text = fullPathToFile;

				fileSystemWatcher1.Path = Path.GetDirectoryName(fullPathToFile);
				fileSystemWatcher1.Filter = Path.GetFileName(fullPathToFile);
				fileSystemWatcher1.EnableRaisingEvents = true;	

				Cursor.Current = Cursors.WaitCursor;
				System.Diagnostics.Process process = new System.Diagnostics.Process();
				System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
				startInfo.FileName = "cmd.exe";
				startInfo.Arguments = String.Format("/C type \"{0}\" | \"{1}\" -jar \"{2}\"  -nbthread {4}  -pipe > {3}", lblFileName.Text, _javaExe, _plantUmlJarFIle, _tempFilePath + _tempFileName, Environment.ProcessorCount);

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

				toolStripStatusLatestChange.Text = String.Format("Last edit: {0}", File.GetLastWriteTime(lblFileName.Text));

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
			if (pictureBox.Image == null)
				return;

			pictureBox.Width = (int)(pictureBox.Image.Width * ((relativeSize.Value) / 100));
			pictureBox.Height = (int)(pictureBox.Image.Height * ((relativeSize.Value) / 100));
		}

		private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
			if (e.ChangeType == System.IO.WatcherChangeTypes.Changed)
			{
				if (File.GetLastWriteTime(lblFileName.Text) != _lastWriteDateTime)
				{
					_lastWriteDateTime = File.GetLastWriteTime(lblFileName.Text);
					fileSystemWatcher1.EnableRaisingEvents = false;
					_refreshImage = true;
				}
			}
			else if (e.ChangeType == System.IO.WatcherChangeTypes.Deleted || e.ChangeType == System.IO.WatcherChangeTypes.Renamed)
			{
				fileSystemWatcher1.EnableRaisingEvents = false;
				pictureBox.Image = null;
				pictureBox.Size = new Size(0, 0);
				toolStripStatusLatestChange.Text = String.Empty;
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (!_refreshImage || String.IsNullOrEmpty(lblFileName.Text))
				return;

			timer1.Enabled = false; // stop the ticker while working...

			if (loadFile(lblFileName.Text))
			{
				fileSystemWatcher1.EnableRaisingEvents = true;
				_refreshImage = false;
			}

			timer1.Enabled = updateToolStripMenuItem.Checked;
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
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

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
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

			loadFile(fd.FileName);
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (updateToolStripMenuItem.Checked)
			{
				updateToolStripMenuItem.Checked = false;
				toolStripStatusAutoUpdate.Text = "Auto Update Off";				
			}
			else
			{
				updateToolStripMenuItem.Checked = true;
				toolStripStatusAutoUpdate.Text = "Auto Update On";
			}

			timer1.Enabled = updateToolStripMenuItem.Checked;
		}

		private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
		{			
			relativeSize.UpButton();
		}

		private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			relativeSize.DownButton();
		}


		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			relativeSize.Value = 100;
		}
	}
}
