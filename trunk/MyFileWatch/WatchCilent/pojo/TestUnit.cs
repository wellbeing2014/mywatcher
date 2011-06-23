/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/6/21
 * Time: 22:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WatchCilent.pojo
{
	/// <summary>
	/// Description of TestUnit.
	/// </summary>
	public class TestUnit
	{
		public TestUnit()
		{
		}
		private  string px = "Id";
		
		public string Px {
			get { return px; }
			set { px = value; }
		}
		private int id;
		
		public int Id {
			get { return id; }
			set { id = value; }
		}
		private byte[] testcontent;
		
		public byte[] Testcontent {
			get { return testcontent; }
			set { testcontent = value; }
		}
		private string unitno;
		
		public string Unitno {
			get { return unitno; }
			set { unitno = value; }
		}
		private string adminname;
		
		public string Adminname {
			get { return adminname; }
			set { adminname = value; }
		}
		private int adminid;
		
		public int Adminid {
			get { return adminid; }
			set { adminid = value; }
		}
		private string modulename;
		
		public string Modulename {
			get { return modulename; }
			set { modulename = value; }
		}
		private int moduleid;
		
		public int Moduleid {
			get { return moduleid; }
			set { moduleid = value; }
		}
		private string packagename;
		
		public string Packagename {
			get { return packagename; }
			set { packagename = value; }
		}
		private int packageid;
		
		public int Packageid {
			get { return packageid; }
			set { packageid = value; }
		}
		private string projectname;
		
		public string Projectname {
			get { return projectname; }
			set { projectname = value; }
		}
		private int projectid;
		
		public int Projectid {
			get { return projectid; }
			set { projectid = value; }
		}
		private string testtime;
		
		public string Testtime {
			get { return testtime; }
			set { testtime = value; }
		}
		private string testorname;
		
		public string Testorname {
			get { return testorname; }
			set { testorname = value; }
		}
		private int testorid;
		
		public int Testorid {
			get { return testorid; }
			set { testorid = value; }
		}
		private string buglevel;
		
		public string Buglevel {
			get { return buglevel; }
			set { buglevel = value; }
		}
		private string testtitle;
		
		public string Testtitle {
			get { return testtitle; }
			set { testtitle = value; }
		}
	}
}
