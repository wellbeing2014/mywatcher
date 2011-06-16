/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/4/21
 * Time: 22:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WatchService.pojo
{
	/// <summary>
	/// Description of ModuleInfo.
	/// </summary>
	public class ModuleInfo
	{
		public ModuleInfo()
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
		private string fullname;
		
		public string Fullname {
			get { return fullname; }
			set { fullname = value; }
		}
		private string code;
		
		public string Code {
			get { return code; }
			set { code = value; }
		}
		private string createtime;
		
		public string Createtime {
			get { return createtime; }
			set { createtime = value; }
		}
		private string lastversion;
		
		public string Lastversion {
			get { return lastversion; }
			set { lastversion = value; }
		}
		private int managerid;
		
		public int Managerid {
			get { return managerid; }
			set { managerid = value; }
		}
		
		private string managername;
		
		public string Managername {
			get { return managername; }
			set { managername = value; }
		}
		public override  bool Equals(object o)
		{
			ModuleInfo gb =(ModuleInfo)o;
			bool isequal = true;
			if(this.Code != gb.Code)
			{
				isequal = false;
			}
			if(this.Createtime != gb.Createtime)
			{
				isequal = false;
			}
			if(this.Fullname != gb.Fullname)
			{
				isequal = false;
			}
			if(this.Id != gb.Id)
			{
				isequal = false;
			}
			if(this.Lastversion != gb.Lastversion)
			{
				isequal = false;
			}
			if(this.Managerid != gb.Managerid)
			{
				isequal = false;
			}
			if(this.managername != gb.managername)
			{
				isequal = false;
			}
			return isequal;
		}
	}
}
