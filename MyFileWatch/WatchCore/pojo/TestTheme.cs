/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-12-23
 * 时间: 10:36
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace WatchCore.pojo
{
	/// <summary>
	/// Description of TestTheme.
	/// </summary>
	public class TestTheme
	{
		public TestTheme()
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
		private int unitid;
		
		public int Unitid {
			get { return unitid; }
			set { unitid = value; }
		}
		private int parentid;
		
		public int Parentid {
			get { return parentid; }
			set { parentid = value; }
		}
		private string favname;
		
		public string Favname {
			get { return favname; }
			set { favname = value; }
		}
		private int personid;
		
		public int Personid {
			get { return personid; }
			set { personid = value; }
		}
		private string personname;
		
		public string Personname {
			get { return personname; }
			set { personname = value; }
		}
		private string favcontent;
		
		public string Favcontent {
			get { return favcontent; }
			set { favcontent = value; }
		}
	}
}
