/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2011/4/22
 * Time: 8:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;

namespace WatchCilent.dao
{
	/// <summary>
	/// Description of PersonDao.
	/// </summary>
	public class PersonDao
	{
		public PersonDao()
		{
		}
		static public List<PersonInfo> getAllPersonInfo()
		{
			string sql = "select * from PersonInfo";
			DataSet data = AccessDBUtil.ExecuteQuery(sql);
			List<PersonInfo> ls = new List<PersonInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2PersonInfo(row));
			}
			return ls;
		}
		
		static public PersonInfo getPersonInfoByModuleid(int id)
		{
			string sql = "select PersonInfo.* from PersonInfo,moduleInfo where PersonInfo.id = moduleInfo.managerid and moduleInfo.id="+id.ToString();
			DataSet data = AccessDBUtil.ExecuteQuery(sql);
			PersonInfo ls = new PersonInfo();
			if(data.Tables["ds"].Rows.Count>0)
			{
				ls=Row2PersonInfo(data.Tables["ds"].Rows[0]);
			}
			return ls;
		}
		private static PersonInfo Row2PersonInfo(DataRow row)
		{
			PersonInfo person = new PersonInfo();
			person.Id = Int32.Parse(row["id"].ToString());
			person.Fullname = row["fullname"].ToString();
			person.Ip = row["ip"].ToString();
			return person;
		}
	}
}
