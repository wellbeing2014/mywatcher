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
using WatchCilent.UI.Pack;
using WatchCilent.UI;

namespace WatchCilent.UI
{
	/// <summary>
	/// Description of Main.
	/// </summary>
	public partial class Main : Form
	{
		public Main()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
			Microsoft.Win32.RegistryKey dbc = key.OpenSubKey("software\\WisoftWatchClient");  
			if(dbc==null|| dbc.GetValue("dbpath")==null)
			{
				Dbconfig db = new Dbconfig();
				if(db.ShowDialog()!= DialogResult.OK)
				{
					
					this.Load += new EventHandler(main_Load);
				}
				else InitializeComponent();
			}
			else InitializeComponent();
			
			
			
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
		void 测试列表ToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			//TestListUI testlistui = new TestListUI();
			changeForm(new TestListUI());
		}
		
		void 更新包列表ToolStripMenuItemClick(object sender, EventArgs e)
		{
			changeForm(new PackageUI());
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
