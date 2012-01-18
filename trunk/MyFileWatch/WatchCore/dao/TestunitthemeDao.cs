/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-12-26
 * 时间: 16:58
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Data;

using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;

namespace WatchCore.dao
{
	/// <summary>
	/// Description of TestunitthemeDao.
	/// </summary>
	public class TestunitthemeDao
	{
		public TestunitthemeDao()
		{
		}
		
		public static bool DelGuanLianUnit(string[] unitid,string themeid)
		{
			string unitstr ="";
			for (int i = 0; i < unitid; i++) {
				
			}
			return true;
		}
		
		
		private static Testunittheme Row2unitTheme(DataRow row)
		{
			Testunittheme mp = new Testunittheme();
			mp.Id = Int32.Parse(row["id"].ToString());
			mp.Unitid = Int32.Parse(row["unitid"].ToString());
			mp.Themeid = Int32.Parse(row["themeid"].ToString());
			return mp;
		}
	}
}
