namespace stealth_copy
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ddlExtentions = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuX = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.folderDestination = new System.Windows.Forms.FolderBrowserDialog();
            this.btnDestinationForlder = new System.Windows.Forms.Button();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.txtCurrentFile = new System.Windows.Forms.TextBox();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnShowFileName = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddlExtentions
            // 
            this.ddlExtentions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ddlExtentions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ddlExtentions.FormattingEnabled = true;
            this.ddlExtentions.Location = new System.Drawing.Point(3, -18);
            this.ddlExtentions.Name = "ddlExtentions";
            this.ddlExtentions.Size = new System.Drawing.Size(164, 28);
            this.ddlExtentions.TabIndex = 1;
            this.ddlExtentions.SelectedIndexChanged += new System.EventHandler(this.ddlExtentions_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.GhostWhite;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 82);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(281, 26);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblMsg
            // 
            this.lblMsg.ForeColor = System.Drawing.Color.Black;
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(15, 20);
            this.lblMsg.Text = "-";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "------";
            this.notifyIcon1.BalloonTipTitle = "......";
            this.notifyIcon1.Text = "0 -0";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.DimGray;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.Silver;
            this.btnClose.Location = new System.Drawing.Point(263, 35);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(14, 15);
            this.btnClose.TabIndex = 6;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuX});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(86, 28);
            // 
            // menuX
            // 
            this.menuX.Name = "menuX";
            this.menuX.Size = new System.Drawing.Size(85, 24);
            this.menuX.Text = "x";
            this.menuX.Click += new System.EventHandler(this.menuX_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox2.Location = new System.Drawing.Point(0, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(164, 27);
            this.textBox2.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.BackColor = System.Drawing.Color.Sienna;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.ForeColor = System.Drawing.Color.Silver;
            this.btnStop.Location = new System.Drawing.Point(242, 35);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(14, 15);
            this.btnStop.TabIndex = 5;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.White;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.ForeColor = System.Drawing.Color.DarkGray;
            this.btnMinimize.Location = new System.Drawing.Point(259, 1);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(20, 21);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.Text = "X";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnDestinationForlder
            // 
            this.btnDestinationForlder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDestinationForlder.BackColor = System.Drawing.Color.White;
            this.btnDestinationForlder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDestinationForlder.ForeColor = System.Drawing.Color.Silver;
            this.btnDestinationForlder.Location = new System.Drawing.Point(222, 35);
            this.btnDestinationForlder.Name = "btnDestinationForlder";
            this.btnDestinationForlder.Size = new System.Drawing.Size(14, 15);
            this.btnDestinationForlder.TabIndex = 4;
            this.btnDestinationForlder.UseVisualStyleBackColor = false;
            this.btnDestinationForlder.Click += new System.EventHandler(this.btnDestinationForlder_Click);
            // 
            // pnlControls
            // 
            this.pnlControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlControls.Controls.Add(this.txtCurrentFile);
            this.pnlControls.Controls.Add(this.ddlExtentions);
            this.pnlControls.Controls.Add(this.btnClose);
            this.pnlControls.Controls.Add(this.btnSkip);
            this.pnlControls.Controls.Add(this.btnShowFileName);
            this.pnlControls.Controls.Add(this.btnDestinationForlder);
            this.pnlControls.Controls.Add(this.btnStop);
            this.pnlControls.Location = new System.Drawing.Point(0, 30);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(280, 53);
            this.pnlControls.TabIndex = 2;
            this.pnlControls.Visible = false;
            // 
            // txtCurrentFile
            // 
            this.txtCurrentFile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtCurrentFile.Location = new System.Drawing.Point(0, 3);
            this.txtCurrentFile.Name = "txtCurrentFile";
            this.txtCurrentFile.ReadOnly = true;
            this.txtCurrentFile.Size = new System.Drawing.Size(167, 27);
            this.txtCurrentFile.TabIndex = 0;
            // 
            // btnSkip
            // 
            this.btnSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSkip.BackColor = System.Drawing.Color.Silver;
            this.btnSkip.Enabled = false;
            this.btnSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkip.ForeColor = System.Drawing.Color.Silver;
            this.btnSkip.Location = new System.Drawing.Point(202, 35);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(14, 15);
            this.btnSkip.TabIndex = 3;
            this.btnSkip.UseVisualStyleBackColor = false;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // btnShowFileName
            // 
            this.btnShowFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowFileName.BackColor = System.Drawing.Color.SkyBlue;
            this.btnShowFileName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowFileName.ForeColor = System.Drawing.Color.Silver;
            this.btnShowFileName.Location = new System.Drawing.Point(182, 35);
            this.btnShowFileName.Name = "btnShowFileName";
            this.btnShowFileName.Size = new System.Drawing.Size(14, 15);
            this.btnShowFileName.TabIndex = 2;
            this.btnShowFileName.UseVisualStyleBackColor = false;
            this.btnShowFileName.Click += new System.EventHandler(this.btnShowFileName_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(281, 108);
            this.ControlBox = false;
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.textBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Opacity = 0.6D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Automatic Copier";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlExtentions;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblMsg;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuX;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.FolderBrowserDialog folderDestination;
        private System.Windows.Forms.Button btnDestinationForlder;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.TextBox txtCurrentFile;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnShowFileName;
    }
}

