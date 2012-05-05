/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/3
 * 时间: 11:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace WatchCilent.UI.UICheck
{
	/// <summary>
	/// Description of UICheck.
	/// </summary>
	public partial class UICheck : Form
	{
		
		public UICheck()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			 
            Bitmap b = new Bitmap(@"E:\1.jpg");
            this.pictureBox1.Image=b;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button4Click(object sender, System.EventArgs e)
		{
			//this.pictureBox1.Image.Save(@"C:\Users\wellbeing.wellbeing-PC\Desktop\aa.jpg");
			
			Bitmap b = this.pictureBox1.GetImg();
			b.Save("E:\\1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg); 
		}
	}
}
