using System;
using System.IO;
using System.Windows.Forms;

namespace stealth_copy.Code
{
	class Common
	{
		public static bool LogOnOff = true;
		private static Object thisLock = new Object();
		private static string commonLogPath = Globals.destMainPath + @"\DubuggerLog.txt";

		public static void log_AddHeader()
		{
			StreamWriter sw = File.AppendText( commonLogPath );
			try
			{
				sw.WriteLine( "\n\n\n\n=================================================\n\n\n" );
			}
			catch { }
			sw.Close();
		}

		public static void log( string title, params object[] values )
		{			
			// critical section. mutex used.
			lock( thisLock )
			{
				StreamWriter sw = File.AppendText( commonLogPath );
				try
				{
					sw.WriteLine( "\n{0:hh:MM:ss ffff} :: {1}\n-------------------------------------",
									DateTime.Now, 
									title );
					for( int i = 0; i < values.Length; i += 2 )
					{
						sw.WriteLine( "{0,-50} : {1}", values[i], values[i + 1] );
					}
				}
				catch
				{ log( title, values ); }

				sw.Close();
			}
		}

		/// <summary>
		/// if null then returns zero, if exception to convert then returns -1
		/// </summary>
		public static int GetInt(object val)
		{
			if( val == null || val == DBNull.Value )
			{	return 0;	}
			try
			{	return Convert.ToInt32( val );	}
			catch
			{	return -1;	}
		}

		public static string GetString( object val )
		{
			return val == null || val == DBNull.Value ? "" : val.ToString();
		}

		internal static void LogAllCounts(string funcName)
		{
			Common.log( funcName + " : all counts",
										"tblDiskInfo", Globals.tblDiskInfo.Rows.Count,
										"tblMatched", Globals.tblMatched.Rows.Count,
										"tblNewDisks", Globals.tblNewDisks.Rows.Count,
										"tblErrors", Globals.tblErrors.Rows.Count,
										"omittedFiles", Globals.omittedFiles.Count );
		}

		

	}
}
