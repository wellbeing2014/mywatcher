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


namespace WatchTest
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private Communication.UDPManage udp = new Communication.UDPManage();
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			udp.StartListen(this,printline);
			this.Closing+= new CancelEventHandler(MainForm_Closing);
			
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private void printline(string msg)
		{
			string[] msgtou = msg.Split(':');
			string feiqtou = msgtou[0];
			string feiqtou = msgtou[0];
			string feiqtou = msgtou[0];
			
			string msgbody = msg.Substring(string.);
			
			MessageBox.Show(msgbody);
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Communication.UDPManage.BroadcastToFQ("ahahhaha","192.10.110.206");
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			udp.StopListen();
		}
		
		void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
			udp.StopListen();
		}
		void Button3Click(object sender, EventArgs e)
		{
			udp.StartListen(this,printline);
		}
	}
}
