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
using System.Windows.Forms;
using WatchCilent.Common;
using WatchCilent.dao;
using WatchCilent.pojo;


namespace WatchCilent.dao
{
	/// <summary>
	/// Description of PackageDao.
	/// </summary>
	public class PackageDao
	{
		public PackageDao()
		{
		}
	
		
		static public DataTable getRePortPack(string begintime ,string endtime)
		{
			string sql = "SELECT packageInfo.packagename, packageInfo.packtime,personinfo.fullname FROM personinfo right JOIN packageInfo ON personinfo.ID = packageInfo.managerid " +
						"where cdate(packageInfo.packtime) >=cdate('"+begintime+"')"+
						" and cdate(packageInfo.packtime) <=cdate('"+endtime+"')"+" order by cdate(packageInfo.packtime) asc";
			DataSet data = AccessDBUtil.ExecuteQuery(sql);
			return data.Tables["ds"];
		}
		
		static public DataTable getRePortPackNUM(string begintime ,string endtime)
		{
			DataTable numtable = new DataTable("numdt");
			numtable.Columns.Add("personname",Type.GetType("System.String"));
			numtable.Columns.Add("pubnum",Type.GetType("System.Int32"));
			numtable.Columns.Add("fznum",Type.GetType("System.Int32"));
			numtable.Columns.Add("totalnum",Type.GetType("System.Int32"));
			numtable.Columns.Add("rate",Type.GetType("System.String"));
			List<PersonInfo> personlist = PersonDao.getAllPersonInfo();
			string sql="SELECT count(*) from packageinfo a where "+
				"a.managerid ={0} "+
				"and cdate(packtime)>=cdate('"+begintime+"') "+
				"and cdate(packtime)<=cdate('"+endtime+"') "+"and state='{1}'";
			string totalsql="SELECT count(*) from packageinfo a where "+
				"a.managerid ={0} "+
				"and cdate(packtime)>=cdate('"+begintime+"') "+
				"and cdate(packtime)<=cdate('"+endtime+"') ";
			System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();
			provider.PercentDecimalDigits = 2;//小数点保留几位数. 
			provider.PercentPositivePattern = 1;//百分号出现在何处. 
			int pub = 0;
			int fz = 0;
			int total =0;
			foreach (PersonInfo ps in personlist) {
				string sqltemp = string.Format(sql,ps.Id,CommonConst.PACKSTATE_YiFaBu);
				int pubnum =AccessDBUtil.ExecuteScalar(sqltemp);
				pub+=pubnum;//合计数
				sqltemp = string.Format(sql,ps.Id,CommonConst.PACKSTATE_YiFeiZhi);
				int fznum =AccessDBUtil.ExecuteScalar(sqltemp);
				fz+=fznum;//合计数
				sqltemp = string.Format(totalsql,ps.Id);
				int totalnum =AccessDBUtil.ExecuteScalar(sqltemp);
				total+=totalnum;//合计数
				double rate = (double)pubnum/(fznum+pubnum);//一定要用double类型.
				numtable.Rows.Add(ps.Fullname,pubnum,fznum,totalnum,rate.ToString("P", provider));
			}
			double totalrate = (double)pub/(fz+pub);//一定要用double类型.
				numtable.Rows.Add("合计",pub,fz,total,totalrate.ToString("P", provider));
			return numtable;
		}
		
		/// <summary>
		/// 所有没有测试的更新包除了已发布，已废止的。
		/// </summary>
		/// <returns></returns>
		static public DataTable getAllUnTestPack()
		{
			string sql = "SELECT packageInfo.*,moduleInfo.id as realmoduleid, moduleInfo.code FROM moduleInfo right JOIN packageInfo ON moduleInfo.ID = packageInfo.moduleid  order by cdate(packageInfo.packtime) desc";
			DataSet data = AccessDBUtil.ExecuteQuery(sql);
			return data.Tables["ds"];
		}
		
		static public List<PackageInfo> queryPackageInfo(string moduleid,string managerid,string state,string begintime,string endtime)
		{
			string sql = "select * from packageinfo where "
				+"(0="+moduleid+" or moduleid="+moduleid+")"
				+" and (0="+managerid+" or managerid="+managerid+")"
				+" and ('全部'='"+state+"' or state='"+state+"')";
			if(begintime!=null)
			{
				sql+=" and  cdate(packtime)>=cdate('"+begintime+"')";
			}
			if(endtime!=null)
			{
				sql+=" and  cdate(packtime)<=cdate('"+endtime+"')";
			}	
			sql+=" order by cdate(packtime) desc";
			DataSet data = AccessDBUtil.ExecuteQuery(sql,null);
			List<PackageInfo> ls = new List<PackageInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2PackageInfo(row));
			}
			return ls;
		}
		
		public static bool updateForPub(string id,string pubpath)
		{
			string time = System.DateTime.Now.ToLocalTime().ToString();
			string sql = "update packageinfo set pubpath = '"+pubpath+"',publishtime ='"+time+"',state ='已发布' where id="+id;
			try {
				int i = AccessDBUtil.ExecuteNonQuery(sql);
				if(i!=0)
					return true;
				else
					return false;
			} catch (Exception) {
				
				return false;
			}
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
		static public List<PackageInfo> getPackageInfoBypath(string path)
		{
			string sql = "select * from packageinfo where packagepath='"+path+"'";
			DataSet data = AccessDBUtil.ExecuteQuery(sql,null);
			List<PackageInfo> ls = new List<PackageInfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2PackageInfo(row));
			}
			return ls;
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
			page.TestRate =Int32.Parse(row["testrate"].ToString());
			page.PubPath = row["pubpath"].ToString();
			return page;
		}
	}
}
