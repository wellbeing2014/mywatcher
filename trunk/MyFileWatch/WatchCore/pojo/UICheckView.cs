/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/22
 * 时间: 14:41
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace WatchCore.pojo
{
	/// <summary>
	/// Description of UICheckView.
	/// </summary>
	public class UICheckView
	{
		public UICheckView()
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
		
		private string checkno;
		
		public string Checkno {
			get { return checkno; }
			set { checkno = value; }
		}
		
		private string checkmark;
		
		public string Checkmark {
			get { return checkmark; }
			set { checkmark = value; }
		}
		
		private byte[] checkedimage;
		
		public byte[] Checkedimage {
			get { return checkedimage; }
			set { checkedimage = value; }
		}
		
		private byte[] srcimage;
		
		public byte[] Srcimage {
			get { return srcimage; }
			set { srcimage = value; }
		}
		
		private string imageno;
		
		public string Imageno {
			get { return imageno; }
			set { imageno = value; }
		}
	}
}
