using System;
using System.Collections;
using SQLite.NET;
using MediaPortal.Util;
using MediaPortal.GUI.Library;
using Programs.Utils;


namespace ProgramsDatabase
{
	/// <summary>
	/// Summary description for DBPrograms.
	/// </summary>
	public class ProgramDatabase
	{
		public static SQLiteClient m_db = null;
		static Applist mAppList = null;

		// singleton. Dont allow any instance of this class
		private ProgramDatabase()
		{
		}

		static ProgramDatabase()
		{
			try 
			{
				// Open database
				try
				{
					System.IO.Directory.CreateDirectory("database");
				}
				catch(Exception){}
				m_db = new SQLiteClient(@"database\ProgramDatabaseV2.db");
				// make sure the DB-structure is complete
				CreateObjects();
				// dirty hack: propagate the m_db to the singleton objects...
				ProgramSettings.m_db = m_db;
			} 
			catch (SQLiteException ex) 
			{
				Log.Write("programdatabase exception err:{0} stack:{1}", ex.Message,ex.StackTrace);
			}
			mAppList = new Applist(m_db, new AppItem.FilelinkLaunchEventHandler(LaunchFilelink));
			//			mAppList = new Applist(m_db);
			//			mAppList.OnLaunchFilelink += new AppItem.FilelinkLaunchEventHandler(LaunchFilelink);

		}

		static void LaunchFilelink(FilelinkItem curLink, bool MPGUIMode)
		{
			AppItem targetApp = mAppList.GetAppByID(curLink.TargetAppID);
			if (targetApp != null)
			{
				targetApp.LaunchFile(curLink, MPGUIMode); 
			}
			}


		static bool ObjectExists(string strName, string strType)
		{
			SQLiteResultSet results;
			bool res = true;
			results = m_db.Execute("SELECT name FROM sqlite_master WHERE name='"+strName+"' and type='"+strType+"' UNION ALL SELECT name FROM sqlite_temp_master WHERE type='"+strType+"' ORDER BY name");
			if (results!=null&& results.Rows.Count>0) 
			{
				if (results.Rows.Count==1) 
				{
					ArrayList arr = (ArrayList)results.Rows[0];
					if (arr.Count==1)
					{
						if ( (string)arr[0] == strName) 
						{
							res = false;
						}
					}
				}
			}
			return res;
		}


		static bool AddObject( string strName, string strType, string strSQL)
		// checks if object exists and returns true if it newly added the object
		{
			if (m_db==null) return false;
			if (!ObjectExists(strName, strType)) return false;
			try 
			{
				m_db.Execute(strSQL);
			}
			catch (SQLiteException ex) 
			{
				Log.Write("programdatabase exception err:{0} stack:{1}", ex.Message,ex.StackTrace);
			}
			return true;
		}


		static void DoV1_V2Migration()
		{
			try 
			{
				Log.Write("programdatabase migration: started");
				m_db.Execute("attach database \"database\\ProgramDatabase.db\" as progV1;\n");
				Log.Write("programdatabase migration: attached v1 database");
				m_db.Execute("insert into application (appid, title, shorttitle, filename, arguments, windowstyle, startupdir, useshellexecute, usequotes, source_type, source, imagefile, filedirectory, imagedirectory, validextensions, importvalidimagesonly, position, enabled) select appid, title, shorttitle, filename, arguments, windowstyle, startupdir, useshellexecute, usequotes, source_type, source, imagefile, filedirectory, imagedirectory, validextensions, importvalidimagesonly, position, enabled from progV1.application;\n");
				Log.Write("programdatabase migration: table APPLICATION migrated");
				m_db.Execute("insert into file (fileid, appid, title, filename, imagefile, genre, country, manufacturer, year, rating, overview, system, manualfilename, lastTimeLaunched, launchcount, isfolder, external_id) select fileid, appid, title, filename, imagefile, genre, country, manufacturer, year, rating, overview, system, manualfilename, lastTimeLaunched, launchcount, isfolder, external_id from progV1.file;");
				Log.Write("programdatabase migration: table FILE migrated");
				m_db.Execute("detach database progV1;\n");
				Log.Write("programdatabase migration: detached v1 database.");
				m_db.Execute("update application set fatherID = -1 where fatherID is null;\n");
				m_db.Execute("update application set enableGUIRefresh = 'T' where source_type in ('DIR_CACHE', 'MY_FILE_INI', 'MY_FILE_MEEDIO');\n");
				m_db.Execute("update application set enableGUIRefresh = 'F' where enableGUIRefresh is null;\n");
				m_db.Execute("update application set GUIRefreshPossible = 'T' where enableGUIRefresh = 'T';\n");
				m_db.Execute("update application set GUIRefreshPossible = 'F' where GUIRefreshPossible is null;\n");
				m_db.Execute("update file set uppertitle = upper(title) where uppertitle is null;\n");
				Log.Write("programdatabase migration: complete");
			}
			catch (SQLiteException ex) 
			{
				Log.Write("programdatabase migration exception err:{0} stack:{1}", ex.Message,ex.StackTrace);
			}
		}

		static bool CreateObjects()
		{
			bool bTryMigration = false;
			if (m_db==null) return false;
			bTryMigration = AddObject("application", "table", "CREATE TABLE application (appid integer primary key, fatherID integer, title text, shorttitle text, filename text, arguments text, windowstyle text, startupdir text, useshellexecute text, usequotes text, source_type text, source text, imagefile text, filedirectory text, imagedirectory text, validextensions text, enabled text, importvalidimagesonly text, position integer, enableGUIRefresh text, GUIRefreshPossible text, pincode integer);\n");
			AddObject("file", "table", "CREATE TABLE file (fileid integer primary key, appid integer, title text, filename text, filepath text, imagefile text, genre text, genre2 text, genre3 text, genre4 text, genre5 text, country text, manufacturer text, year integer, rating integer, overview text, system text, import_flag integer, manualfilename text, lastTimeLaunched text, launchcount integer, isfolder text, external_id integer, uppertitle text);\n");
			AddObject("filterItem", "table", "CREATE TABLE filterItem (appid integer, grouperAppID integer, fileID integer, filename text, tag integer);\n");
			AddObject("setting", "table", "CREATE TABLE setting (settingid integer primary key, key text, value text);\n");
			AddObject("idxFile1", "index", "CREATE INDEX idxFile1 ON file(appid);\n");
			AddObject("idxFile2", "index", "CREATE INDEX idxFile2 ON file(filepath, uppertitle);\n");
			AddObject("idxApp1", "index", "CREATE INDEX idxApp1 ON application(fatherID);\n");
			AddObject("idxFilterItem1", "index", "CREATE UNIQUE INDEX idxFilterItem1 ON filterItem(appID, fileID, grouperAppID);\n");
			AddObject("td_application", "trigger", "CREATE TRIGGER td_application BEFORE DELETE ON application \n  BEGIN\n    DELETE FROM filterItem WHERE appID = old.appID OR grouperAppID = old.appID;\n    DELETE FROM file WHERE appID = old.appID;\n  END;\n");
			if (bTryMigration)
			{
				DoV1_V2Migration();
			}
			return true;
		}


		static public Applist AppList
		{
			get{ return mAppList; }
		}

	}
}
