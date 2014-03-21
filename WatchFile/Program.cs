using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PlantUMLFileWatcher
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			PlantUMLViewer pv = new PlantUMLViewer();

			// Load file from command line
			if (args.Length > 0)
				pv.LoadFile(args[0]);

			Application.Run(pv);
		}
	}
}
