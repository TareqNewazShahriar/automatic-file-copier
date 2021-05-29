using System.Windows.Forms;

namespace stealth_copy.Code
{
    class ControlsAndEvents
	{
		public static Panel pnlHiddenControls;
		static string pss = "";

		public static void press( object sender, KeyPressEventArgs e )
		{
			if( e.KeyChar == (char)Keys.Back && pss.Length > 0 )  // '\b' - backspace
			{
				pss = pss.Remove( pss.Length - 1 );
				//txt.Text = pss; ////
				return;
			}
			if( char.IsLetterOrDigit( e.KeyChar ) == true )
				pss += e.KeyChar;
			if( pss == Globals.ps + Globals.ps2 )
			{
				pnlHiddenControls.Visible = !pnlHiddenControls.Visible;
				pss = "";
			}
			//txt.Text = pss; ////
		}

	}
}
