/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-8-19
 * 时间: 9:45
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using WatchCore.Common;


namespace WatchTest
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private FeiQIM feiq = new FeiQIM(2425);
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			feiq.StartListen(this,printline);
			this.Closing+= new CancelEventHandler(MainForm_Closing);
			
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private void printline(string ip,string msg)
		{
			
			MessageBox.Show(ip+"say:"+msg);
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			feiq.SendMsgToSomeIP("ahahhaha","192.10.110.206");
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			feiq.StopListen();
		}
		
		void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
			feiq.StopListen();
		}
		void Button3Click(object sender, EventArgs e)
		{
			feiq.StartListen(this,printline);
		}
	}
}
