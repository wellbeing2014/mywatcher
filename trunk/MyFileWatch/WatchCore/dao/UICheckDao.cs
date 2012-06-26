/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/21
 * 时间: 15:22
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using WatchCore.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

using WatchCore.pojo;

namespace WatchCore.dao
{
	/// <summary>
	/// Description of UICheckDao.
	/// </summary>
	public class UICheckDao
	{
		public UICheckDao()
		{
		}
		

		//根据当前日期配合数据库查询，生成新编号		
		static public string getNewCheckNO()
		{
			string returnstring ="";
			DateTime dt = System.DateTime.Now;
        	string dtno = dt.Year.ToString()+
        		((dt.Month<10)?"0"+dt.Month.ToString():dt.Month.ToString())+
        		((dt.Day<10)?"0"+dt.Day.ToString():dt.Day.ToString());
        	dtno="CHECK"+dtno;//得到时间头
        	returnstring = dtno;
        	int no;
			DataSet data=SqlDBUtil.ExecuteQuery("SELECT top 1 checkno from UIcheckinfo where checkno like '"+dtno+"%' order by checkno desc");
			if(data.Tables["ds"].Rows.Count!=0)
			{
				
				try {
					no = Int32.Parse(data.Tables["ds"].Rows[0]["checkno"].ToString().Replace(dtno,""));
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

		static public bool DelUICheckViewByNO(string checkno)
		{
			string sql = "delete from UICheckView where checkno='"+checkno+"'";
			try {
				SqlDBUtil.ExecuteNonQuery(sql);
				return true;	
			} catch (Exception) {
				throw new Exception("删除出现异常");
				return false;
			}
		}
		
		static public bool DelUICheckViewByImageno(string checkno,string imageno)
		{
			string sql = "delete from UICheckView where checkno='"+checkno+
			                                   "' and imageno='"+imageno+"'";
			try {
				SqlDBUtil.ExecuteNonQuery(sql);
				return true;	
			} catch (Exception e) {
				throw new Exception("删除出现异常"+e.ToString());
				return false;
			}
		}
		
		static public List<UICheckView> getUICheckViewByNO(string checkno)
		{
			DataSet data=SqlDBUtil.ExecuteQuery("select * from UICheckView where checkno='"+checkno+"'");
			List<UICheckView> ls = new List<UICheckView>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2UICheckview(row));
			}
			return ls;
		}	
		
		static public UIcheckinfo getUIcheckInfoById(int id)
		{
			DataSet data=SqlDBUtil.ExecuteQuery("select top 1 * from UIcheckinfo where id="+id.ToString());
			if(data.Tables["ds"].Rows.Count>0)
			{
			UIcheckinfo ls=Row2UIcheckinfo(data.Tables["ds"].Rows[0]);
			return ls;
			}
			else
				return new UIcheckinfo();
		}	
		
		static public UIcheckinfo getUIcheckInfoByPackId(int id)
		{
			DataSet data=SqlDBUtil.ExecuteQuery("select top 1 * from UIcheckinfo where packageid="+id.ToString());
			if(data.Tables["ds"].Rows.Count>0)
			{
			UIcheckinfo ls=Row2UIcheckinfo(data.Tables["ds"].Rows[0]);
			return ls;
			}
			else
				return new UIcheckinfo();
		}

		private static UICheckView Row2UICheckview(DataRow row)
		{
			UICheckView test = new UICheckView();
			test.Id = Int32.Parse(row["id"].ToString());
			test.Checkedimage = row["checkedimage"] as byte[];
			test.Checkno = row["checkno"].ToString();
			test.Checkmark = row["checkmark"].ToString();
			test.Imageno =row["imageno"].ToString() ;
			test.Srcimage = row["srcimage"] as byte[];
			
			return test;
		}	
		
		private static UIcheckinfo Row2UIcheckinfo(DataRow row)
		{
			UIcheckinfo test = new UIcheckinfo();
			test.Id = Int32.Parse(row["id"].ToString());
			//test.Testcontent = row["testcontent"] as byte[];
			test.Adminid =Int32.Parse(!(row["adminid"].ToString().Equals(""))?row["adminid"].ToString():"0");
			test.Adminname = row["adminname"].ToString();
			test.Moduleid =Int32.Parse(!(row["moduleid"].ToString().Equals(""))?row["moduleid"].ToString():"0");
			test.Modulename = row["modulename"].ToString();
			test.Packageid =Int32.Parse(!(row["packageid"].ToString().Equals(""))?row["packageid"].ToString():"0");
			test.Packagename = row["packagename"].ToString();
			test.Projectid =Int32.Parse(!(row["projectid"].ToString().Equals(""))?row["projectid"].ToString():"0");
			test.Projectname = row["projectname"].ToString();
			test.Checkno = row["checkno"].ToString();
			test.Checkerid = Int32.Parse(!(row["checkerid"].ToString().Equals(""))?row["moduleid"].ToString():"0");
			test.Checkername = row["checkername"].ToString();
			test.Checkedtime =row["checkedtime"].ToString();
			test.Createtime = row["createtime"].ToString();
			test.State = row["state"].ToString();
			return test;
		}	
		
		

		static public int QueryUIcheckCount(string moduleid,string managerid,string state,string begintime, string endtime)
		{
			string sql = "select count(*) from uicheckinfo where "
			+"(0="+moduleid+" or moduleid="+moduleid+")"
				+" and (0="+managerid+" or adminid="+managerid+")"
				+" and ('全部状态'='"+state+"' or state='"+state+"')";
			if(begintime!=null)
			{
				sql+=" and  cast(createtime as datetime)>=cast('"+begintime+"' as datetime)";
			}
			if(endtime!=null)
			{
				sql+=" and  cast(createtime as datetime)<=cast('"+endtime+"' as datetime)";
			}	
			return SqlDBUtil.ExecuteScalar(sql);
			
		}
		
		static public List<UIcheckinfo> QueryUIcheckinfo(string moduleid,string managerid,string state,string begintime, string endtime,
		                                          int startnum,int pagesize)
		{
			string sql = "select * from uicheckinfo where "
			+"(0="+moduleid+" or moduleid="+moduleid+")"
				+" and (0="+managerid+" or adminid="+managerid+")"
				+" and ('全部状态'='"+state+"' or state='"+state+"')";
			if(begintime!=null)
			{
				sql+=" and  cast(createtime as datetime)>=cast('"+begintime+"' as datetime)";
			}
			if(endtime!=null)
			{
				sql+=" and  cast(createtime as datetime)<=cast('"+endtime+"' as datetime)";
			}	
			sql+= " order by cast(createtime as datetime) desc";
			
			DataSet data=SqlDBUtil.ExecuteQuery(sql,startnum,pagesize);
			List<UIcheckinfo> ls = new List<UIcheckinfo>();
			foreach(DataRow row in data.Tables["ds"].Rows)
			{
				ls.Add(Row2UIcheckinfo(row));
			}
			return ls;
		}
			
		static public bool UpdateState(string state,string checkedtime,string checkername,string checkerid,string id)
		{
			bool isSuccess = false;
			string sql = "update uicheckinfo set state='"+state+"',checkedtime='"+checkedtime+"',checkername='"+checkername+"',checkerid="+checkerid+" where id="+id;
			try {
				SqlDBUtil.ExecuteNonQuery(sql);
				isSuccess = true;
			} catch (Exception) {
				
				isSuccess = false;
			}
			return isSuccess;
		}
	}
}
