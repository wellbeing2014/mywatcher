/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2011-5-10
 * Time: 14:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WatchService.pojo
{
	/// <summary>
	/// Description of ProjectInfo.
	/// </summary>
	public class ProjectInfo
	{
		public ProjectInfo()
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
		private string projectname;
		
		public string Projectname {
			get { return projectname; }
			set { projectname = value; }
		}
		private string projectpath;
		
		public string Projectpath {
			get { return projectpath; }
			set { projectpath = value; }
		}
		private string url;
		
		public string Url {
			get { return url; }
			set { url = value; }
		}
		
		public override  bool Equals(object o)
		{
			ProjectInfo gb =(ProjectInfo)o;
			bool isequal = true;
			if(this.Projectname != gb.Projectname)
			{
				isequal = false;
			}
			if(this.Projectpath != gb.Projectpath)
			{
				isequal = false;
			}
			
			return isequal;
		}
	}
}
