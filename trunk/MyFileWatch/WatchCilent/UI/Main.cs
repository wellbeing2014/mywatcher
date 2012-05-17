/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011/6/26
 * 时间: 0:36
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using System.Configuration;
using System.Collections;
using WatchCilent.UI.Test;
using WatchCilent.UI;
using WatchCilent.UI.Pub;
using WatchCore.dao;
using WatchCore.pojo;
using WatchCore.Common;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace WatchCilent.UI
{
	/// <summary>
	/// Description of Main.
	/// </summary>
	public partial class Main : Form
	{
		//注册热键的api 
		[DllImport("user32")]    
    		public static extern bool RegisterHotKey(IntPtr hWnd,int id,uint control,Keys vk );   
	       
	    [DllImport("user32")]    
	    public static extern bool UnregisterHotKey(IntPtr hWnd,    int id); 
	    public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }
	       
		public Main()
		{
			//MessageBox.Show("数据库异常");
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
			Microsoft.Win32.RegistryKey dbc = key.OpenSubKey("software\\WisoftWatchClient",true);  
			//注册热键(窗体句柄,热键ID,辅助键,实键)   
			RegisterHotKey(this.Handle,888,(int)KeyModifiers.Ctrl , Keys.N);
			RegisterHotKey(this.Handle,999, ((int)KeyModifiers.Ctrl+(int)KeyModifiers.Shift), Keys.A);
        	string username=ConfigurationManager.AppSettings["Username"];
        	string password=ConfigurationManager.AppSettings["Password"];
        	bool checkpass = true;
        	string errorstr ="";
        	
        	if(dbc==null|| dbc.GetValue("Username")==null)
        	{
        		checkpass = false;
        		errorstr = "对不起，您的设置可能有问题，请重新设置！";
        	}
        	else if(string.IsNullOrEmpty(username))
        	{
        		checkpass = false;
        		errorstr = "您好像是第一次登录系统，请设置用户名密码";
        	}
        	else if(PersonDao.getAllPersonInfo(username,password).Count!=1)
        	{
        		checkpass = false;
        		errorstr = "您的用户名密码好像不正确，请设置用户名密码";
        	}
        	else if(dbc.GetValue("Version").ToString()!="0.4.0")
        	{
        		System.Diagnostics.Process.Start("notepad.exe",System.Environment.CurrentDirectory+"\\releasenote.txt");
        		dbc.SetValue("Version","0.4.0");
        	}
        	
        	if(!checkpass)
			{
        		MessageBox.Show(errorstr);
				Dbconfig db = new Dbconfig();
				if(db.ShowDialog()!= DialogResult.OK)
				{
					
					//this.Load += new EventHandler(main_Load);
					this.Close();
					System.Windows.Forms.Application.Exit();
					//return;
				}
				else
				{
					if(dbc.GetValue("Version").ToString()!="0.4.0")
		        	{
		        		System.Diagnostics.Process.Start("notepad.exe",System.Environment.CurrentDirectory+"\\releasenote.txt");
		        		dbc.SetValue("Version","0.4.0");
		        	}
					InitializeComponent();
					username=ConfigurationManager.AppSettings["Username"];
					if(string.IsNullOrEmpty(username))
					{
						this.Text = this.Text+"--未登录";
					}
					else this.Text = this.Text+"--"+username;
				}
			}
			else 
			{
				
				try {
					SqlDBUtil.CheckDBState();
				} catch (Exception) {
					
					MessageBox.Show("数据库异常","DBERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.Close();
					System.Windows.Forms.Application.Exit();
				}
				InitializeComponent();
				this.Text = this.Text+"--"+username;
			}
			
			this.notifyIcon1.Visible=true;
			this.notifyIcon1.MouseClick+= new MouseEventHandler(notifyIcon1_Click);
			this.SizeChanged+= new EventHandler(Main_MinimumSizeChanged);
			this.Closing+= new CancelEventHandler(Main_Closing);
			//LoadModules();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		void LoadModules()
		{
			IDictionary IDTest2 = (IDictionary)ConfigurationSettings.GetConfig("modules"); 
			string[] keys=new string[IDTest2.Keys.Count]; 
			string[] values=new string[IDTest2.Keys.Count]; 
			IDTest2.Keys.CopyTo(keys,0); 
			IDTest2.Values.CopyTo(values,0);
			MessageBox.Show(keys[0]+" "+values[0]);;
		}
		
		private void main_Load(object sender, EventArgs e)
		{
			this.Close();
			System.Windows.Forms.Application.Exit();
		}
	
		/// <summary>
		/// 切换主界面
		/// </summary>
		/// <param name="ctrl"></param>
		void changeForm(Control ctrl)
		{
			ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
			if(this.panel2.Controls.Count>0)
			{
				foreach (Control element in this.panel2.Controls) {
					if(element==ctrl) 
						element.Visible=true;
					else 
						element.Visible=false;
				}
				
			}
		}
	
	
		
		
		//最小化隐藏窗口到通知图标
		private void Main_MinimumSizeChanged( object sender,EventArgs e)
		{
			if(this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState =  FormWindowState.Minimized;
				this.Visible = false;
				this.notifyIcon1.BalloonTipText="正在监控中……";
				this.notifyIcon1.ShowBalloonTip(0);
			}
		}
		//通知图标点击还原窗口
		private void notifyIcon1_Click(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				if(this.Visible&&this.WindowState==FormWindowState.Normal)
				{
					this.Visible = true;
					this.WindowState = FormWindowState.Minimized;
				}
				else
				{
					this.Visible = true;
					this.WindowState = FormWindowState.Normal;
				}
				
			}
		}		
		private void Main_Closing(object sender, CancelEventArgs e)
		{
			var close =MessageBox.Show("您真的要退出系统吗？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
			
			if(close==DialogResult.Yes)
			{
				Application.Exit();
			}
			else
			{
				e.Cancel=true;
	    		//注消热键(句柄,热键ID)
        		UnregisterHotKey(this.Handle, 888);  
			}
		}
		
		       	
	     protected override void WndProc(ref Message m)   
	     {   
	         switch (m.Msg)   
	         {   
	             case 0x0312:    //这个是window消息定义的   注册的热键消息    
	                 if (m.WParam.ToString().Equals("888"))  //如果是我们注册的那个热键Ctrl+N    
	         		{
	         			TestResult tr = new TestResult();
						tr.ShowDialog();
	         		}
	         		 if (m.WParam.ToString().Equals("999"))  //如果是我们注册的那个热键Ctrl+Shift+A    
	         		{
	         			if(this.Visible&&this.WindowState==FormWindowState.Normal)
						{
							this.Visible = true;
							this.WindowState = FormWindowState.Minimized;
						}
						else
						{
							this.Visible = true;
							this.WindowState = FormWindowState.Normal;
						}
	         		}
	                 break;   
	         }   
	         base.WndProc(ref m);   
	     }  
		
	    #region 系统托盘右键菜单
	   
		void 退出ToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Close();
			System.Windows.Forms.Application.Exit();
		}
		
		#endregion
		
		void 新建缺陷单元ToolStripMenuItemClick(object sender, EventArgs e)
		{
			TestResult tr = new TestResult();
			tr.ShowDialog();
		}
		
		void 发布ToolStripMenuItemClick(object sender, EventArgs e)
		{
			changeForm(this.publishUI1);
		}
		
		void 更新包ToolStripMenuItemClick(object sender, EventArgs e)
		{
			changeForm(this.packageUI1);
		}
		
		void 配置ToolStripMenuItemClick(object sender, EventArgs e)
		{
			ConfigForm cm = new ConfigForm();
			cm.StartPosition = FormStartPosition.CenterParent;
			cm.ShowDialog();
			this.panel2.Controls[0].Refresh();
		}
		//测试
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			changeForm(this.testlistUI1);
		}
		
		void 主题ToolStripMenuItemClick(object sender, EventArgs e)
		{
			changeForm(this.themelistUI1);
		}
		void 界面检查ToolStripMenuItemClick(object sender, EventArgs e)
		{
			changeForm(this.uichecklist1);
		}
		
	}
}
