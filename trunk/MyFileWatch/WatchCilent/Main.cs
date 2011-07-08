﻿/*
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

namespace WatchCilent
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
	}
}