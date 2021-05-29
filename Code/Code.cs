using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Management;
using System.Threading;

namespace stealth_copy.Code
{
    // Singleton Class
    sealed class Finder
	{
		// private constructor
		private Finder() { }
		//Constructor will be fired.
		private static readonly Finder finderObj = new Finder();
		public static Finder Get { get { return finderObj; } }
		
		public static Thread threadCopyQueue, threadCurrentCopy;
		List<string> extentions = new List<string>();
		List<string> except = new List<string>();
		
		public void caller()
		{
			if( findNewRemovableDisks() == false )
				return;
			Common.log_AddHeader();		////
			//Common.log("caller():new drives found","Count", Globals.tblNewDisks.Rows.Count );////
			initialization();
			loadExtentionsAndExceptions();
			runThread();			//must be before searching function
			penDriveSearching();
			threadCopyQueue.Join();		//wait untill current copy prcess ends
			createLogFiles();
			termination();
			saveDiskRecord();
		}

		private void termination()
		{
			Globals.trayIcon.Text = Globals.tblMatched.Rows.Count +
									" - " + Globals.tblMatched.Rows.Count;
		}

		private bool findNewRemovableDisks()
		{
			bool status = false;
			string driveLetter = "";

			Globals.tblNewDisks.Rows.Clear();		// alas! will be executed all the times.
			
			// HACK: what a mistake (forgetness). its possible to do easily using DriveInfo class
			//		 even with properties that can be used to separate different device
			ManagementObjectSearcher searcher = new ManagementObjectSearcher( "select * from " + Globals.key );
			foreach( ManagementObject MgtObj in searcher.Get() )
			{
				foreach( PropertyData prop in MgtObj.Properties )
				{
					if( prop.Value == null || prop.Value.ToString() == "" )
						continue;

					if( ( prop.Name == "Name" && prop.Value.ToString().IndexOf( ':' ) > -1 )
						|| ( prop.Name == "Caption" && prop.Value.ToString().IndexOf( ':' ) > -1 )
						|| ( prop.Name == "DeviceID" && prop.Value.ToString().IndexOf( ':' ) > -1 ) )
					{
						driveLetter = prop.Value.ToString();
					}
					else if( prop.Name == "Description" &&
							prop.Value.ToString() != "Local Fixed Disk" )
					{
						if( findDiskCopyRecords( MgtObj ) == true )
							status |= false;		// if there are two disks, one may be found 
						// in reocrd, but another may be new
						else
						{
							status = true;
							int v = Globals.tblNewDisks.Rows.Count;
							Globals.tblNewDisks.Rows.Add( "" );
							Globals.tblNewDisks.Rows[v][Globals.colDriveLetter] = driveLetter;
							Globals.tblNewDisks.Rows[v][Globals.colRecordIndex] =
																Globals.tblDiskInfo.Rows.Count;
							saveDiskRecord_Instant( MgtObj );
						}
					}
					else if( prop.Name == "Description" &&
							prop.Value.ToString() == "Local Fixed Disk" )
						break;
				}
			}

			return status;
		}

		private void saveDiskRecord_Instant( ManagementObject MgtObj )
		{
			DataRow row = Globals.tblDiskInfo.NewRow();
			foreach( PropertyData prop in MgtObj.Properties )
			{
				if( prop.Name == Globals.col1 )
					row[Globals.col1] = prop.Value == null ? "" : prop.Value;
				else if( prop.Name == Globals.col2 )
					row[Globals.col2] = prop.Value == null ? "" : prop.Value;
				else if( prop.Name == Globals.col3 )
					row[Globals.col3] = prop.Value == null ? "" : prop.Value;
				else if( prop.Name == Globals.col4 )
					row[Globals.col4] = prop.Value == null ? "" : prop.Value;
			}
			row[Globals.colExt] = Globals.selected;
			Globals.tblDiskInfo.Rows.Add( row );
		}

		private void initialization()
		{
			Globals.ended = false;
			Globals.totalSize = 0;
			//Globals.tblNewDisks		--will be cleared in findNewRemovableDisks()
			Globals.mgtObj.Clear();		// currently not in use. even though, dont comment
			Globals.tblMatched.Rows.Clear();
			Globals.tblErrors.Rows.Clear();
			Globals.omittedFiles.Clear();
		}

		private void loadExtentionsAndExceptions()
		{
			extentions.Clear();
			except.Clear();

			if( Globals.searchCategory[Globals.selected] == Globals.code )
				fillExtAndExceptWords( Globals.extCode, Globals.exceptEmpty );
			else if( Globals.searchCategory[Globals.selected] == Globals.compressed )
				fillExtAndExceptWords( Globals.extZip, Globals.exceptEmpty );
			else if( Globals.searchCategory[Globals.selected] == Globals.executables )
				fillExtAndExceptWords( Globals.extExe, Globals.exceptEmpty );
			else if( Globals.searchCategory[Globals.selected] == Globals.images )
				fillExtAndExceptWords( Globals.extImg, Globals.exceptEmpty );
			else if( Globals.searchCategory[Globals.selected] == Globals.video )
				fillExtAndExceptWords( Globals.extVid, Globals.exceptVid );
			else if( Globals.searchCategory[Globals.selected] == Globals.audio )
				fillExtAndExceptWords( Globals.extAud, Globals.exceptEmpty );
		}

		private void runThread()
		{
			threadCopyQueue = new Thread( new ThreadStart( this.copyProcess ) );
			threadCopyQueue.Start();			// must be before file searching thread
		}

		private void penDriveSearching()
		{
			int i, j, u;

			for( i = 0; i < Globals.tblNewDisks.Rows.Count; i++ )
			{
				createDestinationFolder();
				Globals.tblNewDisks.Rows[i][Globals.colDestPath] = Globals.destMainPath +
																	@"\" + Globals.destDir;
				Globals.tblNewDisks.Rows[i][Globals.colMatchedStart] = Globals.tblMatched.Rows.Count;
				Globals.tblNewDisks.Rows[i][Globals.colOmittedStart] = Globals.omittedFiles.Count;

				DirectoryInfo dir = new DirectoryInfo(
									Globals.tblNewDisks.Rows[i][Globals.colDriveLetter].ToString() );

				try		// when drive is ejected, it will be kept shown, but not accessible
				{
					FileInfo[] allFiles = dir.GetFiles( "*", SearchOption.AllDirectories );
					Common.log( "searching()", "allFiles.Length ", allFiles.Length ); ////

					foreach( FileInfo f in allFiles )	// also: dir.GetFiles("*.exe")
					{
						// find desired files
						for( j = 0;
								j < extentions.Count && f.Extension.ToLower() != extentions[j];
										j++ ) ;

						if( j >= extentions.Count )
							continue;		// file not matched. so... 'next'

						// check for exclusion words; if not found then add in queue
						if( exceptWordFound( f.FullName ) == false )
						{
							DataRow row = Globals.tblMatched.NewRow();
							row[Globals.colSourcePath] = f.FullName;
							row[Globals.colDestPath] = Globals.destMainPath + @"\" + 
														Globals.destDir + @"\" + f.Name;
							row[Globals.colSize] = f.Length / 1024.0 / 1024.0;
							Globals.totalSize += f.Length / 1024.0 / 1024.0;
							row[Globals.colDriveLetter] =
											Globals.tblNewDisks.Rows[i][Globals.colDriveLetter];
							Globals.tblMatched.Rows.Add( row );			// safe add, with data row

							Common.log( "searching() : row added",
										"Globals.tblMatched.Rows[v][Globals.colSource] ",
											row[Globals.colSourcePath],
										"Globals.tblMatched.Rows[v][Globals.colDestPath] ",
											row[Globals.colDestPath],
										"Globals.tblMatched.Rows.Count",
											Globals.tblMatched.Rows.Count ); ////
						}
						else Globals.omittedFiles.Add( f.FullName + " [ " + f.Length + " ]" );
					}
					Common.LogAllCounts("searching()"); ////
					Globals.tblNewDisks.Rows[i][Globals.colMatchedEnd] =
														Globals.tblMatched.Rows.Count;
					Globals.tblNewDisks.Rows[i][Globals.colOmittedEnd] =
														Globals.omittedFiles.Count;
				}
				catch( Exception ex )
				{
					u = Globals.tblErrors.Rows.Count;
					Globals.tblErrors.Rows.Add( "" );
					Globals.tblErrors.Rows[u][Globals.colError] = ex.Message;
					Globals.tblErrors.Rows[u][Globals.colDriveLetter] = i;
				}
			}
			Globals.ended = true;
		}

		private void LocalDriveSearching()
		{
			int i, j, u;

			for( i = 0; i < Globals.tblNewDisks.Rows.Count; i++ )
			{
				createDestinationFolder();

				Globals.tblNewDisks.Rows[i][Globals.colDestPath] = Globals.destMainPath +
																	@"\" + Globals.destDir;
				Globals.tblNewDisks.Rows[i][Globals.colMatchedStart] = Globals.tblMatched.Rows.Count;
				Globals.tblNewDisks.Rows[i][Globals.colOmittedStart] = Globals.omittedFiles.Count;

				DirectoryInfo dir = new DirectoryInfo(
									Globals.tblNewDisks.Rows[i][Globals.colDriveLetter].ToString() );

				// when drive is 'Ejected', it will be kept
				// shown, but not accessible; so try catch used.
				try		
				{
					FileInfo[] allFiles = dir.GetFiles( "*", SearchOption.AllDirectories );
					Common.log( "searching()", "allFiles.Length ", allFiles.Length ); ////

					foreach( FileInfo f in allFiles )	// also: dir.GetFiles("*.exe")
					{
						// find desired files
						for( j = 0;
								j < extentions.Count && f.Extension.ToLower() != extentions[j];
										j++ ) ;

						if( j >= extentions.Count )
							continue;		// file not matched. so... 'next'
						FileInfo ff = new FileInfo( "" );
						DirectoryInfo dd = new DirectoryInfo( "" );
						
						// check for exclusion words; if not found then add in queue
						if( exceptWordFound( f.FullName ) == false )
						{
							DataRow row = Globals.tblMatched.NewRow();
							row[Globals.colSourcePath] = f.FullName;
							row[Globals.colDestPath] = Globals.destMainPath + @"\" +
														Globals.destDir + @"\" + f.Name;
							row[Globals.colSize] = f.Length / 1024.0 / 1024.0;
							Globals.totalSize += f.Length / 1024.0 / 1024.0;
							row[Globals.colDriveLetter] =
											Globals.tblNewDisks.Rows[i][Globals.colDriveLetter];
							Globals.tblMatched.Rows.Add( row );			// safe add, with data row

							Common.log( "searching() : row added",
										"Globals.tblMatched.Rows[v][Globals.colSource] ",
											row[Globals.colSourcePath],
										"Globals.tblMatched.Rows[v][Globals.colDestPath] ",
											row[Globals.colDestPath],
										"Globals.tblMatched.Rows.Count",
											Globals.tblMatched.Rows.Count ); ////
						}
						else Globals.omittedFiles.Add( f.FullName + " [ " + f.Length + " ]" );
					}
					Common.LogAllCounts( "searching()" ); ////
					Globals.tblNewDisks.Rows[i][Globals.colMatchedEnd] =
														Globals.tblMatched.Rows.Count;
					Globals.tblNewDisks.Rows[i][Globals.colOmittedEnd] =
														Globals.omittedFiles.Count;
				}
				catch( Exception ex )
				{
					u = Globals.tblErrors.Rows.Count;
					Globals.tblErrors.Rows.Add( "" );
					Globals.tblErrors.Rows[u][Globals.colError] = ex.Message;
					Globals.tblErrors.Rows[u][Globals.colDriveLetter] = i;
				}
			}
			Globals.ended = true;
		}
		
		private void createLogFiles()
		{
			int i;
			for( i = 0; i < Globals.tblNewDisks.Rows.Count; i++ )
			{
				writeInformation( Globals.tblNewDisks.Rows[i], Convert.ToInt32( Globals.tblNewDisks.Rows[i][Globals.colRecordIndex] ) );
			}
		}
		
		private bool findDiskCopyRecords( ManagementObject MgtObj )
		{
			int matched;
			string val = "";
			foreach( DataRow row in Globals.tblDiskInfo.Rows )
			{
				matched = 0;

				if( row[Globals.colExt].ToString() == Globals.selected )
					matched++;
				foreach( PropertyData prop in MgtObj.Properties )
				{
					val = prop.Value == null ? "" : prop.Value.ToString();

					if( prop.Name == Globals.col1 &&
							val == row[Globals.col1].ToString() )
						matched++;
					else if( prop.Name == Globals.col2 && val == row[Globals.col2].ToString() )
						matched++;
					else if( prop.Name == Globals.col3 && val == row[Globals.col3].ToString() )
						matched++;
					else if( prop.Name == Globals.col4 &&
								 val == row[Globals.col4].ToString() )
						matched++;
				}

				if( matched == Globals.tblDiskInfo.Columns.Count )
					return true;
			}
			return false;
		}

		private void saveDiskRecord()	// not in use
		{
			DataRow row = Globals.tblDiskInfo.NewRow();
			foreach( ManagementObject mgtObjItem in Globals.mgtObj )
			{
				
				foreach( PropertyData prop in mgtObjItem.Properties )
				{
					if( prop.Name == Globals.col1 )
						row[Globals.col1] = prop.Value == null ? "" : prop.Value;
					else if( prop.Name == Globals.col2 )
						row[Globals.col2] = prop.Value == null ? "" : prop.Value;
					else if( prop.Name == Globals.col3 )
						row[Globals.col3] = prop.Value == null ? "" : prop.Value;
					else if( prop.Name == Globals.col4 )
						row[Globals.col4] = prop.Value == null ? "" : prop.Value;
				}
				row[Globals.colExt] = Globals.selected;
				Globals.tblDiskInfo.Rows.Add( row );
			}
		}

		private void fillExtAndExceptWords( string[] ext, string[] exceptWord )
		{
			foreach( string item in ext )
				extentions.Add( item );
			foreach( string item in exceptWord )
				except.Add( item );
		}

		private void copyProcess()
		{
			int i = 0, u, x = 0;
			while( Globals.ended == false || i < Globals.tblMatched.Rows.Count )
			{
				if( i >= Globals.tblMatched.Rows.Count )	// if all copied
					continue;
				
				try
				{	x++;

					Globals.trayIcon.Text = progressNotification(i);
					Globals.lblMsg.Text = Globals.trayIcon.Text;

					resolveDuplicateFileName( i );
					
					threadCurrentCopy = new Thread( Finder.CurrentCopy );
					threadCurrentCopy.Start( i );
					threadCurrentCopy.Join();

					Common.log( "copyProcess()",
								"tblMatched.Rows[i][Globals.colSource]",
									Globals.tblMatched.Rows[i][Globals.colSourcePath],
								"tblMatched.Rows[i][Globals.colDestPath]",
									Globals.tblMatched.Rows[i][Globals.colDestPath],
								"i", i,
								"tblMatched.Rows.Count ", Globals.tblMatched.Rows.Count ); ////

					i++;	// it should be increamented even when exception occurred
					x = 0;
				}
				catch( Exception ex )
				{
					// try five times to copy, to see - if problem resolves
					if( x < 5 ) continue;

					//MessageBox.Show( ex.Message + "  |  " + Globals.tblMatched.Rows[i][Globals.colSource] + " [ " + Globals.tblMatched.Rows[i][Globals.colSize] + " ]" );	////
					u = Globals.tblErrors.Rows.Count;
					Globals.tblErrors.Rows.Add( "" );
					Globals.tblErrors.Rows[u][Globals.colError] = ex.Message + "  |  " + Globals.tblMatched.Rows[i][Globals.colSourcePath] + " [" + Globals.tblMatched.Rows[i][Globals.colSize] + "]";

					Globals.tblErrors.Rows[u][Globals.colDriveLetter] =
													Globals.tblMatched.Rows[i][Globals.colDriveLetter];
					i++;
				}
				Common.LogAllCounts( "copyProcess()" ); ////
			}
		}

		private static void CurrentCopy(object index)
		{	// though was possible to do without parameterized thread

			int i = Common.GetInt( index );
			if( i == -1 ) return;

			Globals.currentCopyIndex = i;
			File.Copy( Globals.tblMatched.Rows[i][Globals.colSourcePath].ToString(),
									Globals.tblMatched.Rows[i][Globals.colDestPath].ToString() );
			
		}

		private void resolveDuplicateFileName( int queueIndex )
		{
			string fullPath = Globals.tblMatched.Rows[queueIndex][Globals.colDestPath].ToString();
			if( File.Exists( fullPath ) == false)
				return;
			
			int lastSlash = fullPath.LastIndexOf('\\');
			if(lastSlash < 0 ) return;
			
			int lastDot = fullPath.LastIndexOf('.');
			if(lastDot == -1) lastDot = fullPath.Length;
			
			string path = fullPath.Substring(0, lastSlash+1);
			string name = fullPath.Substring( lastSlash+1, 
								fullPath.Length - lastSlash - (fullPath.Length-lastDot+1));
			string ext = fullPath.Substring(lastDot);

			int k;
			for( k = 2; File.Exists( path + name + "_" + k + ext ) == true; k++ ) ;
			Globals.tblMatched.Rows[queueIndex][Globals.colDestPath] =
													path + name + "_" + k + ext;
			DataRow row = Globals.tblErrors.NewRow();
			row[Globals.colError] = "Duplicate file resolved. " +
									"New Name: " + name + "_" + k + ext +
									"  Source: " + fullPath;
			row[Globals.colDriveLetter] = Globals.tblMatched.Rows[queueIndex][Globals.colDriveLetter];
			Globals.tblErrors.Rows.Add( row );			
		}

		private string progressNotification(int i)
		{
			return ( i + 1 ) + " - " + Globals.tblMatched.Rows.Count + " [" +
				Convert.ToDouble( Globals.tblMatched.Rows[i][Globals.colSize]).ToString( "0.00" ) 
				+ " / " + (Convert.ToDouble( Globals.totalSize )).ToString("0.00") + "]";
		}

		private void createDestinationFolder()
		{
			DirectoryInfo dirs = new DirectoryInfo( Globals.destMainPath );
			Globals.destDir = Globals.destDirPrefix + Globals.createFolderSL + "-" + 
								Globals.selected;
			while( dirs.GetDirectories( Globals.destDir, SearchOption.TopDirectoryOnly ).Length > 0 )
			{
				Globals.createFolderSL++;
				Globals.destDir = Globals.destDirPrefix + Globals.createFolderSL + "-" + 
									Globals.selected;
			}
			dirs.CreateSubdirectory( Globals.destDir );
		}

		private bool exceptWordFound( string filePath )
		{
			int i;
			filePath = filePath.ToLower();

			// Check for exceptional words
			for( i = 0; i < except.Count && filePath.IndexOf( except[i] ) == -1; i++ ) ;

			if( i < except.Count )
				return true;	// except word found

			return false;		// ok, exception not found
		}
		
		private void writeInformation(DataRow row, int recordIndex)
		{
			int i, matchStart, matchEnd, omitStart, omitEnd;
			matchStart = Common.GetInt( row[Globals.colMatchedStart] );
			matchEnd = Common.GetInt( row[Globals.colMatchedEnd] );
			omitStart = Common.GetInt( row[Globals.colOmittedStart] );
			omitEnd = Common.GetInt( row[Globals.colOmittedEnd] );
			
			using( StreamWriter sw = File.CreateText( row[Globals.colDestPath].ToString()+ @"\" + Globals.LogFile ) )
			{
				sw.WriteLine( "\n" + DateTime.Now.ToString() );

				// write disk information
				sw.WriteLine( "\n\n" + "disk inf\n" + "====================================" );	
				for (i = 0; i < Globals.tblDiskInfo.Columns.Count; i++)
				{
					sw.WriteLine( Globals.tblDiskInfo.Columns[i].ColumnName + " : " + 
									Globals.tblDiskInfo.Rows[recordIndex][i].ToString() );
				}
				sw.WriteLine( "Matched : " + ( matchEnd - matchStart ) );
				sw.WriteLine( "Omitted : " + ( omitEnd - omitStart ) );
				
				// write matched file paths
				sw.WriteLine( "\n\n" + "matched\n" + "====================================" );
				for( i = matchStart; i < matchEnd; i++ )
				{
					sw.WriteLine( Globals.tblMatched.Rows[i][Globals.colSourcePath] + " [ " +
									Globals.tblMatched.Rows[i][Globals.colSize] + " ]" );
				}

				// write errors
				sw.WriteLine( "\n\n" + "errs\n" + "====================================" );
				object val = row[Globals.colDriveLetter];
				if( Globals.tblErrors.Rows.Count > 0)
					for( i = 0; i < Globals.tblErrors.Rows.Count
								&& Globals.tblErrors.Rows[i][Globals.colDriveLetter] == val; 
									i++ )
					{
						sw.WriteLine( Globals.tblErrors.Rows[i][Globals.colError] );
					}

				// write omitted files
				sw.WriteLine( "\n\n" + "omttd\n" + "====================================" );
				for( i = omitStart; i < omitEnd; i++ )
				{
					sw.WriteLine( Globals.omittedFiles[i] );
				}
				sw.Close();
			}
		}
		
	}//class Finder
	
}
