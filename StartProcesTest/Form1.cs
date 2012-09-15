using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace StartProcesTest
{
	public partial class Form1 : Form
	{

		string _plantUmlJarFIle = "";
		string _javaExe = "";
		string _tempFile = "";

		public Form1()
		{
			InitializeComponent();

			_plantUmlJarFIle = ConfigurationManager.AppSettings["PlantUML"];
			_javaExe = ConfigurationManager.AppSettings["JavaExecutable"];
			_tempFile = ConfigurationManager.AppSettings["TempFileDir"].TrimEnd('\\').TrimEnd('/') + "\\";
		}

		private void button1_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			//startInfo.Arguments = String.Format("/C type \"{0}\" | \"{1}\" -jar \"{2}\" -pipe > {3}", basePath + "adayindebormanagement.txt", _javaExe, _plantUmlJarFIle, _tempFile + "temp.png"); 

			process.StartInfo = startInfo;
			
			process.Start();			
			process.WaitForExit();

			if (process.ExitCode != 0)
			{
				MessageBox.Show("ExitCode: " + process.ExitCode.ToString());
				return;
			}

			//pictureBox1.Load(basePath + "temp.png");
		}
	}
}
