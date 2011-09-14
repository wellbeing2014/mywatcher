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
using System.Data;
using System.Threading;


namespace WatchTest
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private FeiQIM feiq = new FeiQIM(2425);
		private delegate void ddddd(string msg,string ip);
		private DataTable dt;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.textBox2.Text="192.10.110.58";
			feiq.StartListen();
			feiq.LISTENED_MSG=printline;
			feiq.LISTENED_SRCEENSHAKE = LISTENED_SRCEENSHAKE;
			feiq.LISTENED_ONLINE = printline2;
			feiq.AutoResend = true;
			dt = feiq.msgdt;
			this.Closing+= new CancelEventHandler(MainForm_Closing);
			
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private void printline(string ip,string msg)
		{
			
			if(textBox4.InvokeRequired)
			{
				ddddd d = dddd1;
				textBox4.Invoke(d,new string[]{msg,ip});
			}
			else
				dddd1(msg,ip);
		}
		private void dddd1(string msg,string ip)
		{
			string date = DateTime.Now.ToLocalTime().ToString();
				this.textBox4.AppendText("\r\n"+date+":\r\n"+"IP地址："+ip+"\r\n"+msg);
		}
		private void LISTENED_SRCEENSHAKE(string ip)
		{
			feiq.SendMsgToSomeIP("你抖我，我也抖你",ip);
			feiq.SendScreenShakeToSomeIP(ip);
		}
		private void printline2(string ip)
		{
			if(ip.Equals("192.10.110.206"))
			{
				feiq.SendOnLineToSomeIP(ip);
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//feiq.SendMsgToSomeIP("ahahhaha","192.10.110.206");
			//feiq.BroadcastOnLine();
			var msg = this.textBox1.Text;
			var msg1 = "你好~，很高兴为您服务！\n1、查询BUG单，请回复“BUGNO#BUG单号”\n"
+"2、确认非BUG，请回复“NO#BUG单号#原因”\n"
+"3、确认BUG，请回复“YES#BUG单号#原因”\n";
			var ip = this.textBox2.Text;
			string msgid= feiq.SendMsgToSomeIP(msg1,ip);
			this.textBox3.Text = msgid;
			
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
			feiq.StartListen();
		}
		//刷新
		void Button4Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
			
			for (int i = 0; i < this.dt.Rows.Count; i++) {
				ListViewItem li = new ListViewItem();
				li.Text = dt.Rows[i][0].ToString();
				li.SubItems.Add(dt.Rows[i][1].ToString());
				li.SubItems.Add(dt.Rows[i][2].ToString());
				this.listView1.Items.Add(li);
			}
		}
	}
}
