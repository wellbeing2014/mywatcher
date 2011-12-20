/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/6/21
 * Time: 23:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;


namespace WatchCore.dao
{
	/// <summary>
	/// Description of TestUnitDao.
	/// </summary>
	public class TestUnitDao
	{
		public TestUnitDao()
		{
		}
		
		/// <summary>
		///	统计BUG概率
		/// </summary>
		/// <param name="begintime"></param>
		/// <param name="endtime"></param>
		/// <returns></returns>
		static public DataTable getRePortBugRate(string begintime ,string endtime)
		{
			DataTable numtable = new DataTable("numdt");
			numtable.Columns.Add("");
			//------------------查姓名--------------------------------------------
			string sqlname ="SELECT distinct adminname FROM testunit "+
					"where cast(testtime as datetime)>=cast('"+begintime+"' as datetime) and cast(testtime as datetime)<=cast('"+endtime+"' as datetime) ";
			DataSet dataname = SqlDBUtil.ExecuteQuery(sqlname);
			DataRowCollection drsname = dataname.Tables["ds"].Rows;
			List<string> adminname = new List<string>();
			for (int j = 0; j < drsname.Count; j++) {
				adminname.Add(drsname[j][0].ToString());
				numtable.Columns.Add(drsname[j][0].ToString(),Type.GetType("System.Double"));
			}
			string sql ="SELECT sum(packageInfo.testrate/100) FROM testunit INNER JOIN packageInfo ON testunit.packageid = packageInfo.ID "+
						"where cast(testunit.testtime as datetime)>=cast('"+begintime+"' as datetime) and  cast(testunit.testtime as datetime)<=cast('"+endtime+"' as datetime) and testunit.adminname='{0}'";
			string sqlnum ="select count(*) from (select distinct testunit.packageid FROM testunit INNER JOIN packageInfo ON testunit.packageid = packageInfo.ID "+
						"where cast(testunit.testtime as datetime)>=cast('"+begintime+"' as datetime) and  cast(testunit.testtime as datetime)<=cast('"+endtime+"' as datetime) and testunit.adminname='{0}') a";
			string sqlv="";
			DataRow dr = numtable.NewRow();
			for (int i = 0; i < adminname.Count; i++) {
				sqlv = string.Format(sql,adminname[i]);
				double num1 = SqlDBUtil.ExecuteSUM(sqlv,null);
				sqlv = string.Format(sqlnum,adminname[i]);
				double num2 = SqlDBUtil.ExecuteSUM(sqlv,null);
				if(num2==0.00)
					dr[i+1]=0;
				else
					dr[i+1]=num1/num2;
			}
			numtable.Rows.Add(dr);
			return numtable;
		}
		
		/// <summary>
		///	统计BUG类型
		/// </summary>
		/// <param name="begintime"></param>
		/// <param name="endtime"></param>
		/// <returns></returns>
		static public DataTable getRePortBugType(string begintime ,string endtime)
		{
			DataTable numtable = new DataTable("numdt");
			numtable.Columns.Add("",Type.GetType("System.String"));
			numtable.Columns.Add(CommonConst.BUGTYPE_JieMian,Type.GetType("System.Int32"));
			numtable.Columns.Add(CommonConst.BUGTYPE_GongNeng,Type.GetType("System.Int32"));
			numtable.Columns.Add(CommonConst.BUGTYPE_JianYi,Type.GetType("System.Int32"));
			numtable.Columns.Add(CommonConst.BUGTYPE_BianMa,Type.GetType("System.Int32"));
			numtable.Columns.Add(CommonConst.BUGTYPE_BianYi,Type.GetType("System.Int32"));
			//------------------查姓名--------------------------------------------
			string sqlname ="SELECT distinct adminname FROM testunit "+
					"where cast(testtime as datetime)>=cast('"+begintime+"' as datetime) and cast(testtime as datetime)<=cast('"+endtime+"' as datetime) ";
			DataSet dataname = SqlDBUtil.ExecuteQuery(sqlname);
			DataRowCollection drsname = dataname.Tables["ds"].Rows;
			List<string> adminname = new List<string>();
			for (int j = 0; j < drsname.Count; j++) {
				adminname.Add(drsname[j][0].ToString());
				numtable.Columns.Add(drsname[j][0].ToString(),Type.GetType("System.Double"));
			}
			
			string sql ="select count(*) from testunit where bugtype = '{0}' and adminname='{1}'"+
						"and cast(testtime as datetime)>=cast('"+begintime+"' as datetime) and "+
				" cast(testtime as datetime)<=cast('"+endtime+"' as datetime)";
			
			for (int i = 0; i < adminname.Count; i++) {
				DataRow dr = numtable.NewRow();
				dr[0]=adminname[i];
				for (int p = 0; p < CommonConst.BUGTYPE.Length; p++) {
					
					dr[p+1] =SqlDBUtil.ExecuteScalar(string.Format(sql,CommonConst.BUGTYPE[p],adminname[i]));
				}
				numtable.Rows.Add(dr);
			}
			
			return numtable;
		}
		
		/// <summary>
		/// 统计BUG趋势图
		/// </summary>
		/// <param name="begintime"></param>
		/// <param name="endtime"></param>
		/// <returns></returns>
		static public DataTable getRePortBugNum(string begintime ,string endtime)
		{
			DataTable numtable = new DataTable("numdt");
			numtable.Columns.Add("name",Type.GetType("System.String"));
			//------------------查时间点--------------------------------
			string sqlcol ="SELECT distinct(CONVERT(varchar(10), cast(testtime as datetime), 23) ) FROM testunit "+
					"where cast(testtime as datetime)>=cast('"+begintime+"' as datetime) and  cast(testtime as datetime)<=cast('"+endtime+"' as datetime) "+
					"order by CONVERT(varchar(10), cast(testtime as datetime), 23)  asc";
			DataSet datacol = SqlDBUtil.ExecuteQuery(sqlcol);
			DataRowCollection drs = datacol.Tables["ds"].Rows;
			List<string> timecol = new List<string>();
			for (int i = 0; i < drs.Count; i++) {
				timecol.Add(drs[i][0].ToString());
				numtable.Columns.Add(drs[i][0].ToString(),Type.GetType("System.Int32"));
			}
			//------------------查姓名--------------------------------------------
			string sqlname ="SELECT distinct(adminname) FROM testunit "+
					"where cast(testtime as datetime)>=cast('"+begintime+"' as datetime) and  cast(testtime as datetime)<=cast('"+endtime+"' as datetime) ";
			DataSet dataname = SqlDBUtil.ExecuteQuery(sqlname);
			DataRowCollection drsname = dataname.Tables["ds"].Rows;
			List<string> adminname = new List<string>();
			for (int j = 0; j < drsname.Count; j++) {
				adminname.Add(drsname[j][0].ToString());
			}
			
			string sql="SELECT count(*) FROM testunit where cast(testtime as datetime)>=" +
				"cast('"+begintime+"' as datetime) and  " +
				"cast(testtime as datetime)<=cast('"+endtime+"' as datetime) and " +
				"adminname='{0}' and CONVERT(varchar(10), cast(testtime as datetime), 23) =CONVERT(varchar(10), cast('{1}' as datetime), 23)";
			string sqlv ="";
			for (int a = 0; a < adminname.Count; a++) {
				DataRow dr = numtable.NewRow();
				dr[0]=adminname[a];
				for(int b = 0;b<timecol.Count;b++)
				{
					sqlv = string.Format(sql,adminname[a],timecol[b]);
					int num = SqlDBUtil.ExecuteScalar(sqlv);
					dr[b+1] = num;
				}
				numtable.Rows.Add(dr);
			}
		
			return numtable;
		}
		
		static public DataTable getRePortBugLevelAll(string begintime ,string endtime)
		{
			DataTable numtable = new DataTable("numdt");
			numtable.Columns.Add("",Type.GetType("System.String"));
			numtable.Columns.Add("轻微",Type.GetType("System.Int32"));
			numtable.Columns.Add("一般",Type.GetType("System.Int32"));
			numtable.Columns.Add("紧急",Type.GetType("System.Int32"));
			numtable.Columns.Add("严重",Type.GetType("System.Int32"));
			
			List<PersonInfo> personlist = PersonDao.getAllPersonInfo();
			string sql="SELECT count(*) from testunit where "+
				"cast(testtime as datetime)>=cast('"+begintime+"' as datetime) "+
				"and cast(testtime as datetime)<=cast('"+endtime+"' as datetime) "+"and buglevel='{0}'";
			
				string sqltemp = string.Format(sql,CommonConst.BUGLEVEL_QinWei);
				int qinwei =SqlDBUtil.ExecuteScalar(sqltemp);
				
				sqltemp = string.Format(sql,CommonConst.BUGLEVEL_YiBan);
				int yiban =SqlDBUtil.ExecuteScalar(sqltemp);
				
				sqltemp = string.Format(sql,CommonConst.BUGLEVEL_YanZhong);
				int yanzhong =SqlDBUtil.ExecuteScalar(sqltemp);
				
				sqltemp = string.Format(sql,CommonConst.BUGLEVEL_JinJI);
				int jinji =SqlDBUtil.ExecuteScalar(sqltemp);
			
				numtable.Rows.Add(null,qinwei,yiban,jinji,yanzhong);
		
			return numtable;
		}
		static public DataTable getRePortBugLevel(string begintime ,string endtime)
		{
			DataTable numtable = new DataTable("numdt");
			numtable.Columns.Add("personname",Type.GetType("System.String"));
			numtable.Columns.Add("轻微",Type.GetType("System.Int32"));
			numtable.Columns.Add("一般",Type.GetType("System.Int32"));
			numtable.Columns.Add("紧急",Type.GetType("System.Int32"));
			numtable.Columns.Add("严重",Type.GetType("System.Int32"));
			numtable.Columns.Add("总计",Type.GetType("System.Int32"));
			List<PersonInfo> personlist = PersonDao.getAllPersonInfo();
			string sql="SELECT count(*) from testunit where adminid={0} "+
				"and cast(testtime as datetime)>=cast('"+begintime+"' as datetime) "+
				"and cast(testtime as datetime)<=cast('"+endtime+"' as datetime) "+"and buglevel='{1}'";
			
			
			int 轻微 = 0;
			int 一般 = 0;
			int 紧急 = 0;
			int 严重 = 0;
			
			foreach (PersonInfo ps in personlist) {
				string sqltemp = string.Format(sql,ps.Id,CommonConst.BUGLEVEL_QinWei);
				int qinwei =SqlDBUtil.ExecuteScalar(sqltemp);
				轻微+=qinwei;//合计数
				sqltemp = string.Format(sql,ps.Id,CommonConst.BUGLEVEL_YiBan);
				int yiban =SqlDBUtil.ExecuteScalar(sqltemp);
				一般+=yiban;//合计数
				sqltemp = string.Format(sql,ps.Id,CommonConst.BUGLEVEL_YanZhong);
				int yanzhong =SqlDBUtil.ExecuteScalar(sqltemp);
				严重+=yanzhong;//合计数
				sqltemp = string.Format(sql,ps.Id,CommonConst.BUGLEVEL_JinJI);
				int jinji =SqlDBUtil.ExecuteScalar(sqltemp);
				紧急+=jinji;//合计数
				int zongji=jinji+yanzhong+yiban+qinwei;
				numtable.Rows.Add(ps.Fullname,qinwei,yiban,jinji,yanzhong,zongji);
			}
				int 总计=紧急+严重+一般+轻微;
				numtable.Rows.Add("合计",轻微,一般,紧急,严重,总计);
			return numtable;
		}
		
		static public DataTable getRePortTest(string begintime,string endtime)
		{
			string sql = "SELECT unitno,buglevel,packagename,testtitle FROM testunit " +
"where cast(testtime as datetime)>=cast('"+begintime+"' as datetime) and  cast(testtime as datetime)<=cast('"+endtime+"' as datetime) order by unitno asc";
			DataSet data = SqlDBUtil.ExecuteQuery(sql);
			return data.Tables["ds"];
		}
		
		
		static public string getNewUnitNO()
		{
			string returnstring ="";
			DateTime dt = System.DateTime.Now;
        	string dtno = dt.Year.ToString()+
        		((dt.Month<10)?"0"+dt.Month.ToString():dt.Month.ToString())+
        		((dt.Day<10)?"0"+dt.Day.ToString():dt.Day.ToString());
        	dtno="BUG"+dtno;
        	returnstring = dtno;
        	int no;
			DataSet data=SqlDBUtil.ExecuteQuery("SELECT top 1 unitno from testunit where unitno like '"+dtno+"%' order by unitno desc");
			if(data.Tables["ds"].Rows.Count!=0)
			{
				
				try {
					no = Int32.Parse(data.Tables["ds"].Rows[0]["unitno"].ToString().Replace(dtno,""));
					no++;
				} catch (Exception) {
					
					no=1;
				}
			}
			else
				no=1;
			if(no<10)
			{
				returnstring=returnstring+"0"+no;
			}
			else returnstring =returnstring+no;
			return returnstring;
		}	
			
		static public List<TestUnit> getAlltestUnit()
		{
			DataSet data=SqlDBUtil.ExecuteQuery("select Unitno,Packagename,Buglevel,Testtitle,Testtime,Adminname,State,Id from testunit order by Unitno desc");
			List<TestUnit> ls = new List<TestUnit>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2Tu(row));
			}
			return ls;
		}
		
		static public List<TestUnit> QueryTestUnit(string moduleid,string managerid,string level,string state,string begintime, string endtime)
		{
			string sql = "select Unitno,Packagename,Buglevel,Testtitle,Testtime,Adminname,State,Id from testunit where "
			+"(0="+moduleid+" or moduleid="+moduleid+")"
				+" and (0="+managerid+" or adminid="+managerid+")"
				+" and ('全部等级'='"+level+"' or buglevel='"+level+"')"
				+" and ('全部'='"+state+"' or state='"+state+"')";
			if(begintime!=null)
			{
				sql+=" and  cast(testtime as datetime)>=cast('"+begintime+"' as datetime)";
			}
			if(endtime!=null)
			{
				sql+=" and  cast(testtime as datetime)<=cast('"+endtime+"' as datetime)";
			}	
			sql+= " order by cast(testtime as datetime) desc";
			
			DataSet data=SqlDBUtil.ExecuteQuery(sql);
			List<TestUnit> ls = new List<TestUnit>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2Tu(row));
			}
			return ls;
		}
		
		static public int QueryTestUnitCount(string moduleid,string managerid,string level,string state,string begintime, string endtime)
		{
			string sql = "select count(*) from testunit where "
			+"(0="+moduleid+" or moduleid="+moduleid+")"
				+" and (0="+managerid+" or adminid="+managerid+")"
				+" and ('全部等级'='"+level+"' or buglevel='"+level+"')"
				+" and ('全部'='"+state+"' or state='"+state+"')";
			if(begintime!=null)
			{
				sql+=" and  cast(testtime as datetime)>=cast('"+begintime+"' as datetime)";
			}
			if(endtime!=null)
			{
				sql+=" and  cast(testtime as datetime)<=cast('"+endtime+"' as datetime)";
			}	
			
			
			return SqlDBUtil.ExecuteScalar(sql);
			
		}
	
		
		static public List<TestUnit> QueryTestUnit(string moduleid,string managerid,string level,string state,string begintime, string endtime,
		                                          int startnum,int pagesize)
		{
			string sql = "select Unitno,Packagename,Buglevel,Testtitle,Testtime,Adminname,State,Id from testunit where "
			+"(0="+moduleid+" or moduleid="+moduleid+")"
				+" and (0="+managerid+" or adminid="+managerid+")"
				+" and ('全部等级'='"+level+"' or buglevel='"+level+"')"
				+" and ('全部'='"+state+"' or state='"+state+"')";
			if(begintime!=null)
			{
				sql+=" and  cast(testtime as datetime)>=cast('"+begintime+"' as datetime)";
			}
			if(endtime!=null)
			{
				sql+=" and  cast(testtime as datetime)<=cast('"+endtime+"' as datetime)";
			}	
			sql+= " order by cast(testtime as datetime) desc";
			
			DataSet data=SqlDBUtil.ExecuteQuery(sql,startnum,pagesize);
			List<TestUnit> ls = new List<TestUnit>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2Tu(row));
			}
			return ls;
		}
		
		static public bool UpdateState(string state,string id)
		{
			bool isSuccess = false;
			string sql = "update testunit set state='"+state+"' where id="+id;
			try {
				SqlDBUtil.ExecuteNonQuery(sql);
				isSuccess = true;
			} catch (Exception) {
				
				isSuccess = false;
			}
			return isSuccess;
		}
		
		static public TestUnit gettestUnitById(int id)
		{
			DataSet data=SqlDBUtil.ExecuteQuery("select top 1 * from testunit where id="+id.ToString());
			if(data.Tables["ds"].Rows.Count>0)
			{
			TestUnit ls=Row2TestUnit(data.Tables["ds"].Rows[0]);
			return ls;
			}
			else
				return new TestUnit();
		}
		private static TestUnit Row2TestUnit(DataRow row)
		{
			TestUnit test = new TestUnit();
			test.Id = Int32.Parse(row["id"].ToString());
			test.Testcontent = row["testcontent"] as byte[];
			test.Adminid =Int32.Parse(!(row["adminid"].ToString().Equals(""))?row["adminid"].ToString():"0");
			test.Moduleid =Int32.Parse(!(row["moduleid"].ToString().Equals(""))?row["moduleid"].ToString():"0");
			test.Packageid =Int32.Parse(!(row["packageid"].ToString().Equals(""))?row["packageid"].ToString():"0");
			test.Projectid =Int32.Parse(!(row["projectid"].ToString().Equals(""))?row["projectid"].ToString():"0");
			test.Testorid =Int32.Parse(!(row["testorid"].ToString().Equals(""))?row["testorid"].ToString():"0");
			test.Adminname = row["adminname"].ToString();
			test.Buglevel = row["buglevel"].ToString();
			test.Bugtype = row["bugtype"].ToString();
			test.Modulename = row["modulename"].ToString();
			test.Packagename = row["packagename"].ToString();
			test.Projectname = row["projectname"].ToString();
			test.State = row["state"].ToString();
			test.Testorname = row["testorname"].ToString();
			test.Testtime = row["testtime"].ToString();
			test.Testtitle = row["testtitle"].ToString();
			test.Unitno = row["unitno"].ToString();
			return test;
		}
		
		private static TestUnit Row2Tu(DataRow row)
		{
			TestUnit test = new TestUnit();
			test.Id = Int32.Parse(row["id"].ToString());
			test.Adminname = row["adminname"].ToString();
			test.Buglevel = row["buglevel"].ToString();
			test.Packagename = row["packagename"].ToString();
			test.State = row["state"].ToString();
			test.Testtitle = row["testtitle"].ToString();
			test.Testtime = row["testtime"].ToString();
			test.Unitno = row["unitno"].ToString();
			return test;
		}
		
		
	}
}
