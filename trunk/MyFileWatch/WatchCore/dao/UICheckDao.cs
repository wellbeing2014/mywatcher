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
		
		static public string getNewCheckNO()
		{
			string returnstring ="";
			DateTime dt = System.DateTime.Now;
        	string dtno = dt.Year.ToString()+
        		((dt.Month<10)?"0"+dt.Month.ToString():dt.Month.ToString())+
        		((dt.Day<10)?"0"+dt.Day.ToString():dt.Day.ToString());
        	dtno="CHECK"+dtno;
        	returnstring = dtno;
        	int no;
			DataSet data=SqlDBUtil.ExecuteQuery("SELECT top 1 checkno from UIcheckinfo where checkno like '"+dtno+"%' order by checkno desc");
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
	}
}
