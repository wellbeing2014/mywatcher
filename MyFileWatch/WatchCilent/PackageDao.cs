/*
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
		static public List<PackageInfo> getAllPackageInfo()
		{
			string sql = "select * from packageinfo";
			DataSet data = AccessDBUtil.ExecuteQuery(sql,null);
			List<PackageInfo> ls = new List<PackageInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2PackageInfo(row));
			}
			return ls;
		}
		public static int insert(PackageInfo pack)
		{
			string sql="insert into PackageInfo(packagename,packagepath,packtime,testtime,publishtime,moduleid,state,managerid)" +
				"values(@packagename,@packagepath,@packtime,@testtime,@publishtime,@moduleid,@state,@managerid);";
			OleDbParameter[] parameters = new OleDbParameter[8];
			parameters[0]=new OleDbParameter("@packagename",OleDbType.VarChar,255);
			parameters[0].Value=pack.Packagename;
			parameters[1]=new OleDbParameter("@packagepath",OleDbType.VarChar,255);
			parameters[1].Value=pack.Packagepath;
			parameters[2]=new OleDbParameter("@packtime",OleDbType.VarChar,255);
			parameters[2].Value=pack.Packtime;
			parameters[3]=new OleDbParameter("@testtime",OleDbType.VarChar,255);
			parameters[3].Value=null;
			parameters[3].IsNullable=true;
			parameters[4]=new OleDbParameter("@publishtime",OleDbType.VarChar,255);
			parameters[4].Value=pack.Publishtime;
			parameters[5] = new OleDbParameter("@moduleid",OleDbType.Integer);
			parameters[5].Value=pack.Moduleid;
			parameters[6] = new OleDbParameter("@state",OleDbType.VarChar,255);
			parameters[6].Value=pack.State;
			parameters[7] = new OleDbParameter("@managerid",OleDbType.Integer);
			parameters[7].Value=pack.Managerid;
		
			return AccessDBUtil.ExecuteNonQuery(sql,parameters);
		}
		public static int update(PackageInfo pack)
		{
			string sql="update PackageInfo set packagename=?,packagepath=?,packtime=?,testtime=?,publishtime=?,moduleid=?,state=?,managerid=? where id=?";
			OleDbParameter[] parameters = new OleDbParameter[9];
			parameters[0]=new OleDbParameter("@packagename",OleDbType.VarChar,255);
			parameters[0].Value=pack.Packagename;
			parameters[1]=new OleDbParameter("@packagepath",OleDbType.VarChar,255);
			parameters[1].Value=pack.Packagepath;
			parameters[2]=new OleDbParameter("@packtime",OleDbType.VarChar,255);
			parameters[2].Value=pack.Packtime;
			parameters[3]=new OleDbParameter("@testtime",OleDbType.VarChar,255);
			parameters[3].Value=pack.Testtime;
			parameters[4]=new OleDbParameter("@publishtime",OleDbType.VarChar,255);
			parameters[4].Value=pack.Publishtime;
			parameters[5] = new OleDbParameter("@moduleid",OleDbType.Integer);
			parameters[5].Value=pack.Moduleid;
			parameters[6] = new OleDbParameter("@state",OleDbType.VarChar,255);
			parameters[6].Value=pack.State;
			parameters[7] = new OleDbParameter("@managerid",OleDbType.Integer);
			parameters[7].Value=pack.Managerid;
			parameters[8] = new OleDbParameter("@id",OleDbType.VarChar,255);
			parameters[8].Value=pack.Id;
			return AccessDBUtil.ExecuteNonQuery(sql,parameters);
		}
		private static PackageInfo Row2PackageInfo(DataRow row)
		{
			PackageInfo page = new PackageInfo();
			page.Id = Int32.Parse(row["id"].ToString());
			page.Packagename = row["packagename"].ToString();
			page.Moduleid = row["moduleid"].ToString();
			page.Packagepath = row["packagepath"].ToString();
			page.Packtime = row["packtime"].ToString();
			page.Publishtime = row["publishtime"].ToString();
			page.Testtime = row["testtime"].ToString();
			page.State = row["state"].ToString();
			
			return page;
		}
	}
}
