/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/5/23
 * Time: 23:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WatchCilent.pojo
{
	/// <summary>
	/// Description of ModuleProject.
	/// </summary>
	public class ModuleProject
	{
		public ModuleProject()
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
		private int moduleid;
		
		public int Moduleid {
			get { return moduleid; }
			set { moduleid = value; }
		}
		private int projectid;
		
		public int Projectid {
			get { return projectid; }
			set { projectid = value; }
		}
	}
}
