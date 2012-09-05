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
using System.Reflection;


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
					if(dbc.GetValue("Version").ToString()!="0.4.1")
		        	{
		        		System.Diagnostics.Process.Start("notepad.exe",System.Environment.CurrentDirectory+"\\releasenote.txt");
		        		dbc.SetValue("Version","0.4.1");
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
				GlobalParams.User = PersonDao.getAllPersonInfo(username,password)[0];
				InitializeComponent();
				this.Text = this.Text+"--"+username;
			}
			
			this.notifyIcon1.Visible=true;
			this.notifyIcon1.MouseClick+= new MouseEventHandler(notifyIcon1_Click);
			this.SizeChanged+= new EventHandler(Main_MinimumSizeChanged);
			this.Closing+= new CancelEventHandler(Main_Closing);
			LoadModules();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		void LoadModules()
		{
			Assembly asm = Assembly.GetExecutingAssembly(); 
			Type[] moudules = asm.GetTypes();
			for (int i = 0; i < moudules.Length; i++) {
				Type mymoudle = moudules[i];
				if(mymoudle.GetInterface("MainPlug")!=null)
				{
					object obj = Activator.CreateInstance(mymoudle);
					//获取权限信息
					MethodInfo   getauthor   =   mymoudle.GetMethod("getAuthorCode");
					string   authorlist   =   (string)getauthor.Invoke(obj,null);
					string[] author = GlobalParams.User.Role.Split(';');
					bool isAuthor = false;
					for (int b = 0; b < author.Length; b++) {
						if(authorlist.Contains(author[b]))
						{
							isAuthor = true;
							break;
						}
					}
					if(!isAuthor)
						continue;
					
					
					//获取菜单注册信息
					MethodInfo   method   =   mymoudle.GetMethod("getPlugName");
					string[]   menuinfo   =   (string[])method.Invoke(obj,null);
					int len = menuinfo.Length;
					//定义一个根菜单、
					ToolStripMenuItem tempitem = null;
					//目前只是写死 菜单 为两级菜单 所以 menuinfo 长度为2
					//从根菜单中查找
					ToolStripItemCollection findoutitem =this.menuStrip1.Items;
					bool isnew =true;
					for (int a = 0; a < findoutitem.Count; a++) {
						if(menuinfo[0].Equals(findoutitem[a].Text))
						{
							//如果找到，就用定义的取代掉，并先从根菜单中删除，等结束时重新加根菜单
							tempitem = (ToolStripMenuItem)(findoutitem[a]);
							this.menuStrip1.Items.Remove(findoutitem[a]);
							isnew = false;
						}
					}
					if(isnew)
					{
						//如果没有，就创建一个新的
						tempitem = new ToolStripMenuItem();
						tempitem.Text = menuinfo[0];
					}
					//定义子菜单
					ToolStripMenuItem realmenu = new ToolStripMenuItem();
					realmenu.Text = menuinfo[1];
					realmenu.Click += new EventHandler(MenuItemClick);
					
					//根据不同的展示方式，转换成不同类
					MethodInfo   getsytle   =   mymoudle.GetMethod("getSytle");
					CommonConst.UIShowSytle  _sytle = (CommonConst.UIShowSytle)getsytle.Invoke(obj,null);
					switch (_sytle) {
						case CommonConst.UIShowSytle.Form:
							Form obj2 = (Form)Activator.CreateInstance(mymoudle);
							realmenu.Tag = obj2;
							break;
						case CommonConst.UIShowSytle.UserControl:
							UserControl obj1 = (UserControl)obj;
							obj1.Dock = System.Windows.Forms.DockStyle.Fill;
							this.panel2.Controls.Add(obj1);
							realmenu.Tag = obj1;
							break;
						case CommonConst.UIShowSytle.MessageBox:
							
							break;
						default:
							throw new Exception("Invalid value for UIShowSytle");
					}
					
					tempitem.DropDownItems.Add(realmenu);
					this.menuStrip1.Items.Add(tempitem);
				}
			}
			
			//this.menuStrip1.Items.Add(this.配置ToolStripMenuItem);
		}
		
		
		private void MenuItemClick(object sender, EventArgs e)
		{
			Object obj =((ToolStripMenuItem)sender).Tag;
			//获取权限信息
			MethodInfo   getsytle   =   obj.GetType().GetMethod("getSytle");
			CommonConst.UIShowSytle   _sytle   =   (CommonConst.UIShowSytle)getsytle.Invoke(obj,null);
			
			
			switch (_sytle) {
						case CommonConst.UIShowSytle.Form:
							Form objform = obj as Form;
							objform.StartPosition = FormStartPosition.CenterParent;
							objform.ShowDialog();
							if(this.panel2.Controls.Count>0)
								this.panel2.Controls[0].Refresh();
							break;
						case CommonConst.UIShowSytle.UserControl:
							Control a = obj as Control;
								changeForm(a);
							break;
						case CommonConst.UIShowSytle.MessageBox:
							
							break;
						default:
							throw new Exception("Invalid value for UIShowSytle");
					}
			
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
		
		
		
		void 配置ToolStripMenuItemClick(object sender, EventArgs e)
		{
			ConfigForm cm = new ConfigForm();
			cm.StartPosition = FormStartPosition.CenterParent;
			cm.ShowDialog();
			this.panel2.Controls[0].Refresh();
		}
		
		
	}
}
