/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/26
 * Time: 11:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WatchCilent
{
	/// <summary>
	/// Description of PackageInfo.
	/// </summary>
	public class PackageInfo
	{
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
		private string packagename;
		
		public string Packagename {
			get { return packagename; }
			set { packagename = value; }
		}
		private string packagepath;
		
		public string Packagepath {
			get { return packagepath; }
			set { packagepath = value; }
		}
		private string packtime;
		
		public string Packtime {
			get { return packtime; }
			set { packtime = value; }
		}
		private string testtime;
		
		public string Testtime {
			get { return testtime; }
			set { testtime = value; }
		}
		private string publishtime;
		
		public string Publishtime {
			get { return publishtime; }
			set { publishtime = value; }
		}
		private int moduleid;
		
		public int Moduleid {
			get { return moduleid; }
			set { moduleid = value; }
		}
		private string state;
		
		public string State {
			get { return state; }
			set { state = value; }
		}
		
		private int managerid;
		
		public int Managerid {
			get { return managerid; }
			set { managerid = value; }
		}
		public PackageInfo()
		{
		}
	}
}
