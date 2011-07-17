/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/5/23
 * Time: 23:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;

using WatchCilent.Common;
using WatchCilent.dao;
using WatchCilent.pojo;


namespace WatchCilent.dao
{
	/// <summary>
	/// Description of ModuleProjectDao.
	/// </summary>
	public class ModuleProjectDao
	{
		public ModuleProjectDao()
		{
		}
		
		static public List<ModuleProject> getAllMPByPrjIDAndMdlID(string pid,string mid)
		{
			string sql = "SELECT * from ModuleProject where moduleid="+mid+" and projectid="+pid;
			DataSet data = AccessDBUtil.ExecuteQuery(sql);
			List<ModuleProject> ls = new List<ModuleProject>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2ModuleProject(row));
			}
			return ls;
		}
		private static ModuleProject Row2ModuleProject(DataRow row)
		{
			ModuleProject mp = new ModuleProject();
			mp.Id = Int32.Parse(row["id"].ToString());
			mp.Moduleid = Int32.Parse(row["moduleid"].ToString());
			mp.Projectid = Int32.Parse(row["projectid"].ToString());
			
			return mp;
		}
	}
}
