/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/4/21
 * Time: 22:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
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
	/// Description of ModuleDao.
	/// </summary>
	public class ModuleDao
	{
		public ModuleDao()
		{
		}
		
		static public DataTable getAllModuleTable()
		{
			string sql = "select * from ModuleInfo order by fullname";
			DataSet data = SqlDBUtil.ExecuteQuery(sql,null);
			DataRow dr = data.Tables["ds"].NewRow();
			dr["fullname"] = "全部平台";
			dr["id"] = 0;
			data.Tables["ds"].Rows.InsertAt(dr,0);
			return data.Tables["ds"];
		}
		
		static public List<ModuleInfo> getAllModuleInfo()
		{
			string sql = "select * from ModuleInfo order by fullname";
			DataSet data = SqlDBUtil.ExecuteQuery(sql,null);
			List<ModuleInfo> ls = new List<ModuleInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2ModuleInfo(row));
			}
			return ls;
		}
		static public List<ModuleInfo> getAllModuleInfoByProjectID(string id)
		{
			string sql = "SELECT * from moduleInfo where id in (select moduleid from moduleproject where projectid="+id+")";
			DataSet data = SqlDBUtil.ExecuteQuery(sql);
			List<ModuleInfo> ls = new List<ModuleInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2ModuleInfo(row));
			}
			return ls;
		}
		
		static public List<ModuleInfo> getAllModuleInfoLikename(string mname ,string mcode)
		{
			string sql = "SELECT * FROM moduleInfo  " +
				"where  moduleInfo.fullname like '%"+mname+"%' and code = '"+mcode+"'";
			DataSet data = SqlDBUtil.ExecuteQuery(sql);
			List<ModuleInfo> ls = new List<ModuleInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2ModuleInfo(row));
			}
			return ls;
		}
		
		/// <summary>
		/// 检查模块代码是否存在
		/// </summary>
		/// <param name="code">模块代码</param>
		/// <returns>true 存在，false 不存在</returns>
		static public bool getModuleByCode(string code)
		{
			string sql = "select count(*) from ModuleInfo where code = '"+code+"'";
			int data = SqlDBUtil.ExecuteScalar(sql);
			if(data > 0)
				return true;
			else
				return false;
		}
		
		private static ModuleInfo Row2ModuleInfo(DataRow row)
		{
			ModuleInfo module = new ModuleInfo();
			module.Id = Int32.Parse(row["id"].ToString());
			module.Fullname = row["fullname"].ToString();
			module.Code = row["code"].ToString();
			module.Createtime = row["createtime"].ToString();
			module.Lastversion = row["lastversion"].ToString();
			module.Managerid =Int32.Parse( row["managerid"].ToString());
			module.Managername = row["Managername"].ToString();
			return module;
		}
	}
}
