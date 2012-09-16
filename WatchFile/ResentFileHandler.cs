using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WatchFile
{
	class ResentFileHandler
	{
		ToolStripMenuItem _resentMenuItem = null;
		OpenRecentFileHandler _callback = null;

		Dictionary<String, DateTime> _resentFiles = new Dictionary<string, DateTime>();
		Object _lock = new Object();

		public delegate void OpenRecentFileHandler(string pathToFile);
		public ResentFileHandler(ToolStripMenuItem menuItem, OpenRecentFileHandler handler )
		{
			_resentMenuItem = menuItem;			
			_callback = handler;
			DeserializeResentFiles();
			RefreshMenu();
		}

		~ResentFileHandler()
		{
			lock (_lock)
			{
				_resentMenuItem = null;
				_callback = null;
				_resentFiles.Clear();
				_resentFiles = null;
				_lock = null;
			}
		}

		public void RefreshMenu()
		{
			lock (_lock)
			{
				_resentMenuItem.DropDownItems.Clear();

				bool hasItems = false;
				foreach(KeyValuePair<String, DateTime> pair in _resentFiles.OrderByDescending(key => key.Value))
				{
					hasItems = true;
					ToolStripMenuItem tsmi = new ToolStripMenuItem();	
					tsmi.Click += new EventHandler(tsmi_Click);
					tsmi.Text = pair.Key;
					tsmi.Tag = "ResentFile";
					_resentMenuItem.DropDownItems.Add(tsmi);
				}
				_resentMenuItem.Enabled = hasItems;
			}
		}

		private void tsmi_Click(object sender, EventArgs e)
		{
			if (!(sender is ToolStripMenuItem))
				return;
			ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;

			if (tsmi.Text == null || !(tsmi.Tag is String) || ((String) tsmi.Tag) != "ResentFile")
				return;

			_callback(tsmi.Text);
		}

		public void Add(String filename)
		{
			lock (_lock)
			{
				if (_resentFiles.ContainsKey(filename))
					_resentFiles[filename] = DateTime.Now;
				else
				{
					// can we add it, or should we remove some?
					if (_resentFiles.Count == Properties.Settings.Default.NumberOfRecentFiles)
						RemoveOldest();
							
					// add the new file
					_resentFiles.Add(filename, DateTime.Now);
				}
				SerializeResentFiles();
				RefreshMenu();
			}			
		}

		public bool Remove(string file)
		{
			lock (_lock)
			{
				if (!_resentFiles.ContainsKey(file))
					return false;

				if (!_resentFiles.Remove(file))
					return false;

				SerializeResentFiles();
				RefreshMenu();
				return true;
			}
		}


		/// <summary>
		/// Must hold lock when calling this method!
		/// </summary>
		private void RemoveOldest()
		{
			DateTime oldestDateTime = DateTime.MaxValue;
			String oldestKey = "";

			foreach (string key in _resentFiles.Keys)
			{
				if (_resentFiles[key] < oldestDateTime)
				{
					oldestDateTime = _resentFiles[key];
					oldestKey = key;
				}
			}

			if (String.IsNullOrEmpty(oldestKey))
				return;

			_resentFiles.Remove(oldestKey);
		}

		private  void SerializeResentFiles()
		{
			lock (_lock)
			{
				StringBuilder sb = new StringBuilder();
				foreach (String key in _resentFiles.Keys)
				{
					sb.AppendLine(String.Format("{0}|{1}", key, _resentFiles[key]));
				}
				Properties.Settings.Default.RecentFiles = sb.ToString();
				Properties.Settings.Default.Save();
			}
		}

		private void DeserializeResentFiles()
		{
			lock (_lock)
			{
				_resentFiles.Clear();
				foreach (String keyValue in Properties.Settings.Default.RecentFiles.Split('\n'))
				{
					if (String.IsNullOrEmpty(keyValue))
						continue;

					String[] split = keyValue.Split('|');
					_resentFiles.Add(split[0], DateTime.Parse(split[1]));
				}
			}
		}
	}
}
