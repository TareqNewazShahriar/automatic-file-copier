using stealth_copy.Code;
using System;
using System.Threading;
using System.Windows.Forms;

namespace stealth_copy
{
    public partial class MainForm : Form
    {
        // TODO: logs were enabled for alpha period

        Thread threadMain;

        public MainForm()
        {
            InitializeComponent();

            Globals.trayIcon = notifyIcon1;
            Globals.lblMsg = this.lblMsg;
            ControlsAndEvents.pnlHiddenControls = this.pnlControls;
        }

		private void Form1_Load(object sender, EventArgs e)
		{
			this.Hide();
			this.SendToBack();
			this.WindowState = FormWindowState.Minimized;
			notifyIcon1.Visible = true;
			textBox2.KeyPress += ControlsAndEvents.press;

			// windows form controls is not safe to access from a thread
			// it requires a different way. see c# doc.
			Globals nouseObj = new Globals(this.ddlExtentions);

			// infinite loop in a thread to keep the form away from 'always busy'
			threadMain = new Thread(new ThreadStart(mainThread));
			threadMain.Start();
		}

		void mainThread()
		{
			Finder pathFinder = Finder.Get;
			while (true)
			{
				pathFinder.caller();
				System.Threading.Thread.Sleep(15000);
			}
		}

		private void ddlExtentions_SelectedIndexChanged(object sender, EventArgs e)
		{
			Globals.selected = ddlExtentions.SelectedItem.ToString();

		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			closeF();
		}

		private void menuX_Click(object sender, EventArgs e)
		{
			closeF();
		}

		private void closeF()
		{
			//Finder.threadCurrentCopy.Abort();
			Finder.threadCopyQueue.Abort();
			threadMain.Abort();
			//Application.Exit();
			this.Close();
		}

		int v = 0, u = 0;
		private void notifyIcon1_Click(object sender, EventArgs e)
		{
			v++;
			showWindDow();
		}
		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			u++;
			showWindDow();
		}
		void showWindDow()
		{
			if ((v > 0 && u > 1) || (v > 1 && u > 0))
			{
				this.Show();
				this.WindowState = FormWindowState.Normal;
				this.ShowInTaskbar = true;
				v = 0; u = 0;
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			if (Finder.threadCopyQueue.IsAlive == true)
				Finder.threadCopyQueue.Abort();
			threadMain.Abort();

			threadMain = new Thread(new ThreadStart(mainThread));
			threadMain.Start();

			notifyIcon1.Text = "! " + notifyIcon1.Text;
			txtCurrentFile.Text = "";
		}

		private void btnMinimize_Click(object sender, EventArgs e)
		{
			this.ShowInTaskbar = false;
			this.Hide();
			this.WindowState = FormWindowState.Minimized;
			this.Refresh();

		}

		private void btnDestinationForlder_Click(object sender, EventArgs e)
		{
			if (folderDestination.ShowDialog() == DialogResult.OK)
			{
				Globals.destMainPath = folderDestination.SelectedPath;
			}
		}

		private void btnShowFileName_Click(object sender, EventArgs e)
		{
			if (txtCurrentFile.Text.Length > 0)
			{
				txtCurrentFile.Text = "";
				return;
			}

			if (Globals.currentCopyIndex == -1 || Globals.tblMatched.Rows.Count == 0)
				return;

			int queueIndex = Globals.currentCopyIndex, j;
			string fullPath = Globals.tblMatched.Rows[queueIndex][Globals.colDestPath].ToString();

			int lastSlash = fullPath.LastIndexOf('\\');
			if (lastSlash < 0) return;

			int lastDot = fullPath.LastIndexOf('.');
			if (lastDot == -1) lastDot = fullPath.Length;

			string name = fullPath.Substring(lastSlash + 1,
								fullPath.Length - lastSlash - (fullPath.Length - lastDot + 1));
			string ext = fullPath.Substring(lastDot);

			name = name.ToLower();
			ext = ext.ToLower();
			txtCurrentFile.Text = "";

			for (j = name.Length - 1; j >= 0; j--)
				txtCurrentFile.Text += name[j];
			txtCurrentFile.Text += "@@";
			for (j = ext.Length - 1; j >= 0; j--)
				txtCurrentFile.Text += ext[j];
		}

		// Current copy could not be cancelled even thread used. 
		// probably the file copy is a thread itself and runs separately.
		private void btnSkip_Click(object sender, EventArgs e)
		{
			if (Globals.currentCopyIndex == -1 || Globals.tblMatched.Rows.Count == 0)
				return;

			/*////
			 Common.log( "btnSkip_Click() : current file",
						"source",
						Globals.tblMatched.Rows[Globals.currentCopyIndex][Globals.colSourcePath],
						"queue index", Globals.currentCopyIndex );
			//*/
			if (Finder.threadCurrentCopy.IsAlive == true)
				Finder.threadCurrentCopy.Abort();

			//Thread.Sleep( 500 );	////
			/*////
			Common.log( "btnSkip_Click() : current file",
						"source: ",
						Globals.tblMatched.Rows[Globals.currentCopyIndex][Globals.colSourcePath],
						"queue index", Globals.currentCopyIndex );
			//*/
		}
	}
}
