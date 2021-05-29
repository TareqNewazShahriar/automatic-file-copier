using System.Collections.Generic;
using System.Data;
using System.Management;
using System.Windows.Forms;

namespace stealth_copy.Code
{
    class Globals
	{
		public const string key = "Win32_LogicalDisk";
		public static NotifyIcon trayIcon;
		public static ToolStripLabel lblMsg;
		public static bool ended = false;			// thread synchronization var
		public static string selected = "";
		public static string ps = "tar";
		public static string ps2 = "123";
		public static int currentCopyIndex = -1;

		public static DataTable tblNewDisks = new DataTable();
		public static List<ManagementObject> mgtObj = new List<ManagementObject>();
		public static DataTable tblMatched = new DataTable();
		public static List<string> omittedFiles = new List<string>();
		public static DataTable tblErrors = new DataTable();
		public static Dictionary<string, string> searchCategory = new Dictionary<string, string>();

		public static double totalSize = 0;

		// stored in varialbles to increase flexibility and avoid misspelling
		#region acronyms / short names
		// if any new search criteria need to added, then code need to 
		// add in 4 places. here add 2 variables, here in loadExtCode(), 
		// and in Code.cs -> loadExtentionsAndExceptions()
		public const string code = "code";
		public const string executables = "executables";
		public const string compressed = "compressed";
		public const string images = "images";
		public const string video = "video";
		public const string audio = "audio";
		
		public const string codeCode = "cc";
		public const string codeExe = "ex";
		public const string codeCompressed = "zp";
		public const string codeImages = "im";
		public const string codeVideo = "vd";
		public const string codeAudio = "ad";
		public static string defaultSearch;

		// for various tables columns
		public const string colExt = "ext";
		public const string col1 = "MaximumComponentLength";
		public const string col2 = "Size";
		public const string col3 = "VolumeName";
		public const string col4 = "VolumeSerialNumber";
		public const string colSourcePath = "src";
		public const string colDestPath = "dest";
		public const string colSize = "size";
		public const string colDriveLetter = "d";
		public const string colRecordIndex = "p";
		public const string colMatchedStart = "matcS";
		public const string colMatchedEnd = "matcE";
		public const string colOmittedStart = "omitS";
		public const string colOmittedEnd = "omitE";
		public const string colError = "err";
		
		#endregion

		public static string[] extCode = { ".c", ".cpp", ".cc", ".cs", ".h" };
		public static string[] extExe = { ".exe", ".msi" };
		public static string[] extZip = { ".zip", ".rar", ".7z", ".iso" };
		public static string[] extImg = { ".bmp", ".jpg", ".gif", ".png", ".tif" };
		public static string[] extVid = { ".avi", ".mpg", ".mp4", ".flv", ".mov", ".mpeg", ".wmv", 
											".dat", ".3gp", ".xvid", ".divx", ".webm", ".webp",
											".rm" };
		public static string[] extAud = { ".mp3", ".wav", ".mid", ".amr", ".wma" };

		//public static string extVidPattern = "*.avi, *.mpg, *.mp4, *.flv, *.mov, *.mpeg, *.wmv, *.dat, *.3gp, *.xvid, *.divx, *.webm, *.webp, *.rm";

		public static string[] exceptVid = { "java", "c#", "c++", " c ", "sql", "asp.net", ".net", 
											"javascript", "php", "server", "visual basic", "ajax",
											"visual studio", "framework", "programming" };
		public static string[] exceptEmpty = { };

		public static string destMainPath = @"C:\Users Data\DRM\Cache";
			//@"I:\Tariq's Documents\Web\Miscellaneous\ww\01.   2011.05.19  11.23pm\Crystal17 my replies_files\gets ad data";
		public const string destDirPrefix = "post-adds-00";
		public const string destDirPostfix = "-dta";
		public static int createFolderSL = 0;
		public static string destDir = "";
		public const string LogFile = "log.txt";

		public static DataTable tblDiskInfo = new DataTable();

		// constructor
		public Globals( ComboBox ddl )
		{
			defaultSearch = codeVideo;  // assign code name as default search
			initializeNewDisksTbl();
			initializeMatchedTable();
			initializeErrorTable();
			initializeCompletedDiskInfoTable();

			loadExtCode();
			LoadDDL( ddl );
		}

		private void initializeErrorTable()
		{
			tblErrors.Columns.Add( colError );
			tblErrors.Columns.Add( colDriveLetter );
		}

		private void initializeNewDisksTbl()
		{
			tblNewDisks.Columns.Add( colDriveLetter, typeof(string) );
			tblNewDisks.Columns.Add( colRecordIndex, typeof( int ) );
			tblNewDisks.Columns.Add( colDestPath, typeof( string ) );
			tblNewDisks.Columns.Add( colMatchedStart, typeof( int ) );
			tblNewDisks.Columns.Add( colMatchedEnd, typeof( int ) );
			tblNewDisks.Columns.Add( colOmittedStart, typeof( int ) );
			tblNewDisks.Columns.Add( colOmittedEnd, typeof( int ) );
		}

		private void initializeMatchedTable()
		{
			tblMatched.Columns.Add(colSourcePath, typeof(string) );
			tblMatched.Columns.Add( colDestPath, typeof( string ) );
			tblMatched.Columns.Add( colSize );
			tblMatched.Columns.Add( colDriveLetter );
		}

		private void initializeCompletedDiskInfoTable()
		{
			tblDiskInfo.Columns.Add( col1 );
			tblDiskInfo.Columns.Add( col2 );
			tblDiskInfo.Columns.Add( col3 );
			tblDiskInfo.Columns.Add( col4 );
			tblDiskInfo.Columns.Add( colExt );
		}

		/// <summary>
		/// Just to ensure that the properties are in a sorted way, as they
		/// appear in the Mgt object. it will help greatly to omit lot of 
		/// checkings at later processing.		/// 
		/// </summary>
		private void initializeCompletedDiskInfoTable__noNeed()
		{
			ManagementObjectSearcher searcher = new ManagementObjectSearcher( "select * from " + key );
			foreach( ManagementObject MgtObj in searcher.Get() )
			{
				if( MgtObj.Properties.Count <= 0 )
					return;

				foreach( PropertyData prop in MgtObj.Properties )
				{
					if( prop.Name == "MaximumComponentLength" )
						tblDiskInfo.Columns.Add( "MaximumComponentLength" );
					else if( prop.Name == "Size" )
						tblDiskInfo.Columns.Add( "Size" );
					else if( prop.Name == "VolumeName" )
						tblDiskInfo.Columns.Add( "VolumeName" );
					else if( prop.Name == "VolumeSerialNumber" )
						tblDiskInfo.Columns.Add( "VolumeSerialNumber" );
				}
				break;
			}
		}

		private void loadExtCode()
		{
			searchCategory[codeCode] = code;
			searchCategory[codeExe] = executables;
			searchCategory[codeVideo] = video;
			searchCategory[codeCompressed] = compressed;
			searchCategory[codeImages] = images;
			searchCategory[codeAudio] = audio;
		}

		private void LoadDDL( ComboBox ddl )
		{
			foreach( KeyValuePair<string, string> item in searchCategory )
			{
				ddl.Items.Add( item.Key );
			}
			ddl.SelectedIndex = ddl.FindString( defaultSearch );
			selected = defaultSearch;
		}

	}

}
