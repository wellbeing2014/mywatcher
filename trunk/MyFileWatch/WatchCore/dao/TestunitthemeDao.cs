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
		
		/// <summary>
		/// 删除主题测试单元关联表
		/// </summary>
		/// <param name="unitid">测试单元ID数组，如果传入NULL则删除全部关联</param>
		/// <param name="themeid">主题ID</param>
		/// <returns></returns>
		public static bool DelGuanLianUnit(string[] unitid,string themeid)
		{
			string sql="delete from testunittheme where themeid ='"+themeid+"'";
			string unitstr ="";
			if(unitid==null)
				unitstr = "0";
			else
			{
				for (int i = 0; i < unitid.Length; i++) {
					
					unitstr="'"+unitid[i]+"',"+unitstr;
				}
				unitstr=unitstr.Substring(0,unitstr.Length-1);
				sql+=" and unitid in ("+unitstr+") ";
			}
			try {
				SqlDBUtil.ExecuteNonQuery(sql);
				return true;	
			} catch (Exception) {
				throw new Exception("删除id数据出现异常");
				return false;
			}
			
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
