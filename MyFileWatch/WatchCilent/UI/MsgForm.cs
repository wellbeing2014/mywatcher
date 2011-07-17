/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/23
 * Time: 21:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WatchCilent.Common;
using WatchCilent.dao;
using WatchCilent.pojo;

namespace WatchCilent
{
	/// <summary>
	/// Description of MsgForm.
	/// </summary>
	/// 
	
	public partial class MsgForm : Form
	{
		public String msgstr = "";
		public MsgForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.Load+= MsgForm_Load;
			InitializeComponent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	
		private void MsgForm_Load(object sender, EventArgs e)
        {
         	this.label1.Text="您有一个的新更新包需要处理:\n"+this.msgstr;
        }
		
//处理提醒框 
//效果：当一个人更新了 另一个人不能通过提醒框进入修改
	
		void Button1Click(object sender, EventArgs e)
		{
			//查数据库
			//select * from PackageInfo where packpath='msgstr'
			//有记录的提示并返回，没记录就
			//insert packpath,packtime  into PackageInfo values(msgstr,datetime)
			PackageInfo pack= PackageDao.getPackageInfoBypath(msgstr)[0];
			BussinessForm bf = new BussinessForm(pack);
			bf.StartPosition = FormStartPosition.CenterParent;
			this.Close();
			this.Dispose();
			bf.ShowDialog();
		}
		void Button2Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.Dispose();
		}
		
	}
}
