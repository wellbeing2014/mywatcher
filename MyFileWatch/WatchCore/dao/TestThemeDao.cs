﻿/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-12-23
 * 时间: 10:41
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using WatchCore.pojo;
using System.Collections.Generic;

namespace WatchCore.dao
{
	/// <summary>
	/// Description of TestThemeDao.
	/// </summary>
	public class TestThemeDao
	{
		public TestThemeDao()
		{
		}
		
		static public List<TestTheme> getAllTestThemeByPersonname(string personname )
		{
			string sql = "SELECT * FROM TestTheme  " +
				"where  personname='"+personname+"'";
			DataSet data = SqlDBUtil.ExecuteQuery(sql);
			List<TestTheme> ls = new List<TestTheme>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2TestTheme(row));
			}
			return ls;
		}
		
		
		private static TestTheme  Row2TestTheme(DataRow row)
		{
			TestTheme testtheme = new TestTheme();
			testtheme.Id = Int32.Parse(row["id"].ToString());
			testtheme.Parentid = Int32.Parse(row["parentid"].ToString());
			testtheme.Personid =Int32.Parse( row["personid"].ToString());
			testtheme.Unitid = Int32.Parse(row["unitid"].ToString());
			testtheme.Favname = row["favname"].ToString();
			testtheme.Favcontent = row["favcontent"].ToString();
			testtheme.Personname = row["personname"].ToString();
			return testtheme;
		}
	}
}