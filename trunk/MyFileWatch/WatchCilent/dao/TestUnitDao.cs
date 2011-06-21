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
		static public bool insert(TestUnit tu)
		{
			OleDbParameter tt = new OleDbParameter();
			tt.OleDbType=OleDbType.VarBinary;
			tt.Value=tu.Testcontent;
			int i=AccessDBUtil.ExecuteInsert("insert into testunit (test_content) values(tt)",new OleDbParameter[]{tt});
			if(i!=0)
			{
				return true;
			}
			else return false;
		}
		static public TestUnit gettestUnitById(int id)
		{
			DataSet data=AccessDBUtil.ExecuteQuery("select top 1 * from testunit where id="+id.ToString());
			
			TestUnit ls=Row2TestUnit(data.Tables["ds"].Rows[0]);
			return ls;
		}
		private static TestUnit Row2TestUnit(DataRow row)
		{
			TestUnit test = new TestUnit();
			test.Id = Int32.Parse(row["id"].ToString());
			test.Testcontent = row["test_content"] as byte[];
			return test;
		}
		
		
	}
}
