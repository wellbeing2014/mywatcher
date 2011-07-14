﻿/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011/6/28
 * 时间: 21:39
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WatchCilent.dao;
using WatchCilent.pojo;
using System.Collections.Generic;

namespace WatchCilent
{
	/// <summary>
	/// Description of TestListUI.
	/// </summary>
	public partial class TestListUI : UserControl
	{
		public TestListUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			getAll();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void getAll()
		{
			List<TestUnit>alltu=TestUnitDao.getAlltestUnit();
			foreach (TestUnit tu in alltu) {
				ListViewBing(tu);
			}
		}
		
		private void ListViewBing(TestUnit tu)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text=tu.Unitno;//
			lvi.Checked=false;
			lvi.SubItems.Add(tu.Packagename);
			lvi.SubItems.Add(tu.Buglevel);
			lvi.SubItems.Add(tu.Testtitle);
			lvi.SubItems.Add(tu.Testtime);
			lvi.SubItems.Add(tu.Adminname);//
			lvi.SubItems.Add(tu.State);
			lvi.SubItems.Add(tu.Id.ToString());//
			this.listView1.Items.Add(lvi);
			
		}
		
		private TestUnit  ListViewSelect(ListViewItem lvi)
		{
			TestUnit tu = new TestUnit();
			
			tu.Unitno = lvi.Text;
			tu.Packagename = lvi.SubItems[1].Text;
			tu.Buglevel = lvi.SubItems[2].Text;
			tu.Testtitle = lvi.SubItems[3].Text;
			tu.Testtime = lvi.SubItems[4].Text;
			tu.Adminname = lvi.SubItems[5].Text;
			tu.State = lvi.SubItems[6].Text;
			tu.Id = Int32.Parse(lvi.SubItems[7].Text);
			return tu;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			TestResult tr = new TestResult();
			
			tr.ShowDialog();
		}
	}
}