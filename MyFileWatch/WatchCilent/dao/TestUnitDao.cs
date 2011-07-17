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

using WatchCilent.Common;
using WatchCilent.dao;
using WatchCilent.pojo;


namespace WatchCilent.dao
{
	/// <summary>
	/// Description of TestUnitDao.
	/// </summary>
	public class TestUnitDao
	{
		public TestUnitDao()
		{
		}
		
		
		static public List<TestUnit> getAlltestUnit()
		{
			DataSet data=AccessDBUtil.ExecuteQuery("select * from testunit");
			List<TestUnit> ls = new List<TestUnit>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2TestUnit(row));
			}
			return ls;
		}
		static public bool insert(TestUnit tu)
		{
			OleDbParameter tt = new OleDbParameter();
			tt.OleDbType=OleDbType.VarBinary;
			tt.Value=tu.Testcontent;
			int i=AccessDBUtil.ExecuteInsert("insert into testunit (testcontent) values(tt)",new OleDbParameter[]{tt});
			if(i!=0)
			{
				return true;
			}
			else return false;
		}
		static public TestUnit gettestUnitById(int id)
		{
			DataSet data=AccessDBUtil.ExecuteQuery("select top 1 * from testunit where id="+id.ToString());
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
			test.Adminid =Int32.Parse((row["adminid"].Equals(""))?row["adminid"].ToString():"0");
			test.Moduleid =Int32.Parse((row["moduleid"].Equals(""))?row["moduleid"].ToString():"0");
			test.Packageid =Int32.Parse((row["packageid"].Equals(""))?row["packageid"].ToString():"0");
			test.Projectid =Int32.Parse((row["projectid"].Equals(""))?row["projectid"].ToString():"0");
			test.Testorid =Int32.Parse((row["testorid"].Equals(""))?row["testorid"].ToString():"0");
			test.Adminname = row["adminname"].ToString();
			test.Buglevel = row["buglevel"].ToString();
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
		
		
	}
}
