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
			feiq.LISTENED_WRITING = printline1;
			feiq.LISTENED_ONLINE = printline2;
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
		private void printline1(string ip)
		{
			
			//MessageBox.Show(ip+"正在输入日日日");
		}
		private void printline2(string ip)
		{
			DataRow[] dr = this.dt.Select("ip='"+ip+"'");
    		foreach (var element in dr) {
				feiq.SendMsgToSomeIP(element[1].ToString(),ip);
    		}
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//feiq.SendMsgToSomeIP("ahahhaha","192.10.110.206");
			//feiq.BroadcastOnLine();
			var msg = this.textBox1.Text;
			var ip = this.textBox2.Text;
			string msgid= feiq.SendMsgToSomeIP(msg,ip);
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
