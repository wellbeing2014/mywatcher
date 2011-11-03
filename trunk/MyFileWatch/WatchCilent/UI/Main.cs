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
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
			Microsoft.Win32.RegistryKey dbc = key.OpenSubKey("software\\WisoftWatchClient");  
			//注册热键(窗体句柄,热键ID,辅助键,实键)   
			RegisterHotKey(this.Handle,888,(int)KeyModifiers.Ctrl , Keys.N);
			RegisterHotKey(this.Handle,999, ((int)KeyModifiers.Ctrl+(int)KeyModifiers.Shift), Keys.A);
        	
			if(dbc==null|| dbc.GetValue("dbpath")==null)
			//if(!FunctionUtils.ConfigRight())
			{
				Dbconfig db = new Dbconfig();
				if(db.ShowDialog()!= DialogResult.OK)
				{
					
					this.Load += new EventHandler(main_Load);
				}
				else InitializeComponent();
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
			}
			
			this.notifyIcon1.Visible=true;
			this.notifyIcon1.MouseClick+= new MouseEventHandler(notifyIcon1_Click);
			this.SizeChanged+= new EventHandler(Main_MinimumSizeChanged);
			this.Closing+= new CancelEventHandler(Main_Closing);
			
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
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
				this.panel2.Controls.Clear();
				this.panel2.Controls.Add(ctrl);
			}
		}
	
	
		
		void 配置ToolStripMenuItemClick(object sender, EventArgs e)
		{
			ConfigForm cm = new ConfigForm();
			cm.StartPosition = FormStartPosition.CenterParent;
			cm.ShowDialog();
			this.panel2.Controls[0].Refresh();
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
			changeForm(new PublishUI());
		}
		
		void 更新包ToolStripMenuItemClick(object sender, EventArgs e)
		{
			changeForm(new PackageUI());
		}
		
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			changeForm(new TestListUI());
		}
	}
}
