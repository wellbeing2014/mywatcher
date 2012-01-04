/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-12-26
 * 时间: 16:52
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace WatchCore.pojo
{
	/// <summary>
	/// Description of Testunittheme.
	/// </summary>
	public class Testunittheme
	{
		public Testunittheme()
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
		private int themeid;
		
		public int Themeid {
			get { return themeid; }
			set { themeid = value; }
		}
	}
}
