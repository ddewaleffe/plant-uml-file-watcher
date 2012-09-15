namespace WatchFile
{
	partial class PlantUMLViewer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.lblFileName = new System.Windows.Forms.Label();
			this.btnWatchFile = new System.Windows.Forms.Button();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.relativeSize = new System.Windows.Forms.NumericUpDown();
			this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnSave = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLatestChange = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.relativeSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblFileName
			// 
			this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.lblFileName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblFileName.Location = new System.Drawing.Point(3, 33);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(994, 23);
			this.lblFileName.TabIndex = 0;
			this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnWatchFile
			// 
			this.btnWatchFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWatchFile.Location = new System.Drawing.Point(1012, 33);
			this.btnWatchFile.Name = "btnWatchFile";
			this.btnWatchFile.Size = new System.Drawing.Size(81, 23);
			this.btnWatchFile.TabIndex = 1;
			this.btnWatchFile.Text = "PlantUML";
			this.btnWatchFile.UseVisualStyleBackColor = true;
			this.btnWatchFile.Click += new System.EventHandler(this.btnWatchFile_Click);
			// 
			// pictureBox
			// 
			this.pictureBox.BackColor = System.Drawing.SystemColors.HighlightText;
			this.pictureBox.Location = new System.Drawing.Point(0, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(294, 328);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox.TabIndex = 2;
			this.pictureBox.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel1.Controls.Add(this.pictureBox);
			this.panel1.Location = new System.Drawing.Point(3, 59);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1199, 509);
			this.panel1.TabIndex = 3;
			// 
			// relativeSize
			// 
			this.relativeSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.relativeSize.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.relativeSize.Location = new System.Drawing.Point(1111, 573);
			this.relativeSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.relativeSize.Name = "relativeSize";
			this.relativeSize.Size = new System.Drawing.Size(91, 20);
			this.relativeSize.TabIndex = 4;
			this.relativeSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.relativeSize.ValueChanged += new System.EventHandler(this.relativeSize_ValueChanged);
			// 
			// fileSystemWatcher1
			// 
			this.fileSystemWatcher1.EnableRaisingEvents = true;
			this.fileSystemWatcher1.NotifyFilter = System.IO.NotifyFilters.LastWrite;
			this.fileSystemWatcher1.SynchronizingObject = this;
			this.fileSystemWatcher1.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
			// 
			// timer1
			// 
			this.timer1.Interval = 250;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(1116, 33);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(81, 23);
			this.btnSave.TabIndex = 6;
			this.btnSave.Text = "Gem";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLatestChange});
			this.statusStrip1.Location = new System.Drawing.Point(0, 571);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1202, 22);
			this.statusStrip1.TabIndex = 7;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLatestChange
			// 
			this.toolStripStatusLatestChange.AutoSize = false;
			this.toolStripStatusLatestChange.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
							| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolStripStatusLatestChange.Name = "toolStripStatusLatestChange";
			this.toolStripStatusLatestChange.Size = new System.Drawing.Size(230, 17);
			this.toolStripStatusLatestChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PlantUMLViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1202, 593);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.relativeSize);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnWatchFile);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.statusStrip1);
			this.Name = "PlantUMLViewer";
			this.Text = "PlantUML : Source File Watcher";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.relativeSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFileName;
		private System.Windows.Forms.Button btnWatchFile;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.NumericUpDown relativeSize;
		private System.IO.FileSystemWatcher fileSystemWatcher1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLatestChange;
	}
}

