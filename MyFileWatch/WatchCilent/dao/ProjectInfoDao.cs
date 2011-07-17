/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2011-5-10
 * Time: 14:04
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
	/// Description of ProjectInfoDao.
	/// </summary>
	public class ProjectInfoDao
	{
		public ProjectInfoDao()
		{
		}
		
		static public DataTable getAllProjectTable()
		{
			string sql = "select * from projectinfo";
			DataSet data = AccessDBUtil.ExecuteQuery(sql,null);
			DataRow dr = data.Tables["ds"].NewRow();
			dr["projectname"] = "全部项目";
			dr["id"] = 0;
			data.Tables["ds"].Rows.InsertAt(dr,0);
			return	data.Tables["ds"];
			
		}
		static public List<ProjectInfo> getAllProjectInfo()
		{
			string sql = "select * from projectinfo";
			DataSet data = AccessDBUtil.ExecuteQuery(sql,null);
			List<ProjectInfo> ls = new List<ProjectInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2ProjectInfo(row));
			}
			return ls;
		}
		
		static public List<ProjectInfo> getAllProjectInfoByModulename(string mname ,string mcode)
		{
			string sql = "select * from ProjectInfo where id in (SELECT moduleproject.projectid FROM moduleInfo , moduleproject " +
				"where moduleInfo.ID = moduleproject.moduleid and moduleInfo.fullname like '"+mname+"%' and code = '"+mcode+"')";
			DataSet data = AccessDBUtil.ExecuteQuery(sql);
			List<ProjectInfo> ls = new List<ProjectInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2ProjectInfo(row));
			}
			return ls;
		}
		
		static public List<ProjectInfo> getAllProjectInfoByModuleid(int id)
		{
			string sql = "select * from ProjectInfo where id in (SELECT moduleproject.projectid FROM  moduleproject " +
				"where moduleproject.moduleid = "+id+")";
			DataSet data = AccessDBUtil.ExecuteQuery(sql);
			List<ProjectInfo> ls = new List<ProjectInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2ProjectInfo(row));
			}
			return ls;
		}
		private static ProjectInfo Row2ProjectInfo(DataRow row)
		{
			ProjectInfo project = new ProjectInfo();
			project.Id = Int32.Parse(row["id"].ToString());
			project.Projectname = row["projectname"].ToString();
			project.Projectpath = row["projectpath"].ToString();
			project.Url = row["url"].ToString();
			return project;
		}
	}
}
