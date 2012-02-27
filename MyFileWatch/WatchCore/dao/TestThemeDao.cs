/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-12-23
 * 时间: 10:41
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
	/// Description of TestThemeDao.
	/// </summary>
	public class TestThemeDao
	{
		public TestThemeDao()
		{
		}
		
		public static bool DeleteTheme(string themeid)
		{
			string sql="delete from testtheme where id='"+themeid+"' or parentid ='"+themeid+"'";
			try {
				SqlDBUtil.ExecuteNonQuery(sql);
				return true;	
			} catch (Exception) {
				throw new Exception("删除出现异常");
				return false;
			}
			
		}
		
		static public List<TestTheme> getAllTestThemeByPersonname(string personname )
		{
			string sql = "SELECT * FROM TestTheme  ";
			if(string.IsNullOrEmpty(personname))
			{
				sql +="where  personname is null";
			}
			else
			 	sql +="where  personname='"+personname+"'";
			DataSet data = SqlDBUtil.ExecuteQuery(sql);
			List<TestTheme> ls = new List<TestTheme>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2TestTheme(row));
			}
			return ls;
		}
		
		/// <summary>
		/// 根据关联的unitid获取主题
		/// </summary>
		/// <param name="unitid"></param>
		/// <returns></returns>
		static public List<TestTheme> getTestThemeByUnitid(string unitid )
		{
			
			List<TestTheme> ls = new List<TestTheme>();
			string sql1 ="select count(*) from testunittheme where themeid = 99999 and unitid="+unitid;
			int count=SqlDBUtil.ExecuteScalar(sql1);
			if(count>0)
			{
				string personid = System.Configuration.ConfigurationManager.AppSettings["UserId"];
				//string personname = System.Configuration.ConfigurationManager.AppSettings["Username"];
				TestTheme default_tt = new TestTheme();
				default_tt.Id=99999;
				default_tt.Personid=((personid==null)?0:Int32.Parse(personid));
				default_tt.Personname=System.Configuration.ConfigurationManager.AppSettings["Username"];
				ls.Add(default_tt);
			}
			
			string sql = "SELECT * FROM TestTheme  " +
				"where id in (select themeid from testunittheme where unitid="+unitid+")";
			DataSet data = SqlDBUtil.ExecuteQuery(sql);
			
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
