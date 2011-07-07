/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/6
 * Time: 0:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;

using WatchCilent.dao;
using WatchCilent.pojo;

namespace WatchCilent
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		[DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_VER_POSITIVE = 0x0004;
        const int AW_VER_NEGATIVE = 0x0008;
        const int AW_CENTER = 0x0010;
        const int AW_HIDE = 0x10000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_SLIDE = 0x40000;
        const int AW_BLEND = 0x80000;
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(
            IntPtr hwnd,
            int hWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            int wFlags
        );


		private Communication.UDPManage udp;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			InitializeComponent();
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
			Microsoft.Win32.RegistryKey dbc = key.OpenSubKey("software\\WisoftWatchClient");  
			if(dbc==null|| dbc.GetValue("dbpath")==null)
			{
				Dbconfig db = new Dbconfig();
				db.ShowDialog();
			}
			
			udp = new Communication.UDPManage();
			udp.StartListen(this,printline);
			this.Closing += new CancelEventHandler(Form1_Closing);
			this.notifyIcon1.Visible=true;
			this.notifyIcon1.MouseClick+= new MouseEventHandler(notifyIcon1_Click);
			this.SizeChanged+= new EventHandler(MainForm_MinimumSizeChanged);
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			//ListViewBing(null);
			
			List<PersonInfo> datasource_person = PersonDao.getAllPersonInfo();
			PersonInfo person = new PersonInfo();
			person.Fullname = "全部责任人";
			person.Id = 0;
			datasource_person.Insert(0,person);
			this.comboBox2.DataSource = datasource_person;
			this.comboBox2.DisplayMember = "Fullname";
			this.comboBox2.ValueMember = "Id";
			
			List<ModuleInfo> datasource_module = ModuleDao.getAllModuleInfo();
			ModuleInfo all = new ModuleInfo();
			all.Fullname ="全部模块";
			all.Id=0;
			datasource_module.Insert(0,all);
			this.comboBox1.DataSource = datasource_module;
			this.comboBox1.DisplayMember ="Fullname";
			this.comboBox1.ValueMember = "Id";
			getAllPackInList();
			this.checkBox1.Checked = true;
			this.CenterToScreen();
			
		}
		//通知图标点击还原窗口
		private void notifyIcon1_Click(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				this.Visible = true;
				this.WindowState = FormWindowState.Normal;
			}
		}
		//最小化隐藏窗口到通知图标
		private void MainForm_MinimumSizeChanged( object sender,EventArgs e)
		{
			if(this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState =  FormWindowState.Minimized;
				this.Visible = false;
				this.notifyIcon1.BalloonTipText="正在监控中……";
				this.notifyIcon1.ShowBalloonTip(0);
			}
		}
		//退出程序时关闭监听
		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
	    	udp.StopListen();
		}
		//监听响应的方法。
		void printline(String udpstr)
		{
			getAllPackInList();
			MsgForm mf =new MsgForm();
			mf.ShowInTaskbar=false;
			mf.msgstr = udpstr;
			SetWindowPos(mf.Handle, 1, Screen.PrimaryScreen.Bounds.Width - mf.Width, Screen.PrimaryScreen.Bounds.Height - mf.Height - 30, 50, 50, 1);
			mf.ShowDialog();
		}
		//查询按钮的方法			
		void Button5Click(object sender, EventArgs e)
		{
			getAllPackInList();
		}
		
		//手动处理的按钮方法
		void Button4Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count==1)
			{
				PackageInfo pack = new PackageInfo();
				pack = ListViewSelect(listView1.SelectedItems[0]);
				BussinessForm bf = new BussinessForm(pack);
			 	bf.StartPosition = FormStartPosition.CenterParent;
				bf.ShowDialog();
				getAllPackInList();
			}
			else
			{
				MessageBox.Show("请选择一个更新包","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			
		}
		
		//隐藏按钮的方法
		
		void Button2Click(object sender, EventArgs e)
		{
			this.WindowState =  FormWindowState.Minimized;
			this.Visible = false;
			this.notifyIcon1.BalloonTipText="正在监控中……";
			this.notifyIcon1.ShowBalloonTip(1);
		}
		
		private void getAllPackInList()
		{
			string moduleid = this.comboBox1.SelectedValue.ToString();
			string manageid = this.comboBox2.SelectedValue.ToString();
			string state = this.comboBox3.SelectedItem.ToString();
			List<PackageInfo> ls =PackageDao.queryPackageInfo(moduleid,manageid,state,null,null);
			this.listView1.Items.Clear();
			foreach(PackageInfo pack in ls)
			{
				ListViewBing(pack);
			}
		}
		private void ListViewBing(PackageInfo pack)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text=pack.Packagename;
			lvi.SubItems.Add(pack.Packagepath);
			lvi.SubItems.Add(pack.Packtime);
			lvi.SubItems.Add(pack.Testtime);
			lvi.SubItems.Add(pack.Publishtime);
			lvi.SubItems.Add(pack.State);
			lvi.SubItems.Add(pack.Moduleid.ToString());
			lvi.SubItems.Add(pack.Managerid.ToString());
			lvi.SubItems.Add(pack.Id.ToString());
			//lvi.SubItems.Add();
			this.listView1.Items.Add(lvi);
		}
		
		private PackageInfo  ListViewSelect(ListViewItem lvi)
		{
			PackageInfo pack = new PackageInfo();
			pack.Packagename = lvi.SubItems[0].Text;
			pack.Packagepath = lvi.SubItems[1].Text;
			pack.Packtime = lvi.SubItems[2].Text;
			pack.Publishtime = lvi.SubItems[4].Text;
			pack.Testtime = lvi.SubItems[3].Text;
			pack.State = lvi.SubItems[5].Text;
			pack.Moduleid = Int32.Parse(lvi.SubItems[6].Text);
			pack.Managerid =Int32.Parse(lvi.SubItems[7].Text);
			pack.Id = Int32.Parse(lvi.SubItems[8].Text);
			return pack;
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			ConfigForm cm = new ConfigForm();
			cm.StartPosition = FormStartPosition.CenterParent;
			cm.ShowDialog();
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0)
			{
				foreach(ListViewItem lt in listView1.SelectedItems)
				{
					PackageInfo pack = new PackageInfo();
					pack = ListViewSelect(lt);
					AccessDBUtil.delete(pack);
				}
			}
			else
			{
				MessageBox.Show("请选择更新包","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			getAllPackInList();
		}
		
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0&&!this.checkBox1.Checked)
			{
				this.button6.Enabled=true;
				this.button7.Enabled=true;
				this.button8.Enabled = true;
				this.button9.Enabled = true;
			}
			else 
			{
				this.button6.Enabled=false;
				this.button7.Enabled=false;
				this.button8.Enabled = false;
				this.button9.Enabled = false;
			}
			
			
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0)
			{
				PackageInfo pack = new PackageInfo();
				pack = ListViewSelect(listView1.SelectedItems[0]);
				pack.State="已测试";
				pack.Testtime=System.DateTime.Now.ToLocalTime().ToString();
				AccessDBUtil.update(pack);
			}
			getAllPackInList();
		}
		void Button8Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0)
			{
				PackageInfo pack = new PackageInfo();
				pack = ListViewSelect(listView1.SelectedItems[0]);
				pack.State="已作废";
				AccessDBUtil.update(pack);
			}
			getAllPackInList();
		}
		void Button9Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0)
			{
				PackageInfo pack = new PackageInfo();
				pack = ListViewSelect(listView1.SelectedItems[0]);
				pack.State="已发布";
				pack.Publishtime=System.DateTime.Now.ToLocalTime().ToString();
				AccessDBUtil.update(pack);
			}
			getAllPackInList();
		}
	}
}
