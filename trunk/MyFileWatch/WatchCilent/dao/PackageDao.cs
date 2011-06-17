﻿/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/26
 * Time: 23:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace WatchCilent
{
	/// <summary>
	/// Description of PackageDao.
	/// </summary>
	public class PackageDao
	{
		public PackageDao()
		{
		}
		static public List<PackageInfo> queryPackageInfo(string moduleid,string managerid,string state)
		{
			string sql = "select * from packageinfo where "
				+"(0="+moduleid+" or moduleid="+moduleid+")"
				+" and (0="+managerid+" or managerid="+managerid+")"
				+" and ('全部状态'='"+state+"' or state='"+state+"')";
			DataSet data = AccessDBUtil.ExecuteQuery(sql,null);
			List<PackageInfo> ls = new List<PackageInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2PackageInfo(row));
			}
			return ls;
		}
		
		
		
		
		
		static public List<PackageInfo> getAllPackageInfo()
		{
			string sql = "select * from packageinfo";
			DataSet data = AccessDBUtil.ExecuteQuery(sql);
			List<PackageInfo> ls = new List<PackageInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2PackageInfo(row));
			}
			return ls;
		}
////////	
		static public int getPackageInfoCountBypath(string path)
		{
			string sql = "select * from packageinfo where packagepath='"+path+"'";
			DataSet data = AccessDBUtil.ExecuteQuery(sql,null);
			return  data.Tables["ds"].Rows.Count;
		}
// /// 
		
		private static PackageInfo Row2PackageInfo(DataRow row)
		{
			PackageInfo page = new PackageInfo();
			page.Id = Int32.Parse(row["id"].ToString());
			page.Packagename = row["packagename"].ToString();
			page.Moduleid = Int32.Parse(row["moduleid"].ToString());
			page.Packagepath = row["packagepath"].ToString();
			page.Packtime = row["packtime"].ToString();
			page.Publishtime = row["publishtime"].ToString();
			page.Testtime = row["testtime"].ToString();
			page.Managerid =Int32.Parse(row["managerid"].ToString());
			//page.Managerid = null;
			page.State = row["state"].ToString();
			return page;
		}
	}
}