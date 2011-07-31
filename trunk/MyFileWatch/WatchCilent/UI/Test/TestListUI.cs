/*
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
using WatchCilent.Common;
using System.Collections.Generic;

namespace WatchCilent.UI.Test
{
	/// <summary>
	/// Description of TestListUI.
	/// </summary>
	public partial class TestListUI : UserControl
	{
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		public TestListUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			
			
			InitializeComponent();
			
			TreeNode tmp =new TreeNode("全部");
			foreach (var element in Enum.GetNames(typeof(CommonConst.TestState))) {
				tmp.Nodes.Add(element);
			}
			treeView1.Nodes.Add(tmp);
			getAll();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		/// <summary>
		/// 获取所有缺陷单元
		/// </summary>
		void getAll()
		{
			List<TestUnit>alltu=TestUnitDao.getAlltestUnit();
			foreach (TestUnit tu in alltu) {
				ListViewBing(tu);
			}
		}
		
		/// <summary>
		/// LIstVieW 绑定数据。
		/// </summary>
		/// <param name="tu"></param>
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
		/// <summary>
		/// 新增
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button1Click(object sender, EventArgs e)
		{
			TestResult tr = new TestResult();
			tr.ShowDialog();
			this.listView1.Items.Clear();
			getAll();
		}
		/// <summary>
		/// 修改
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button2Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count==1)
			{
				TestUnit tu = ListViewSelect(this.listView1.CheckedItems[0]);
				TestResult tr = new TestResult(TestUnitDao.gettestUnitById(tu.Id));
				tr.ShowDialog();
			}
			else
			{
				MessageBox.Show("请选择一条记录","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}
		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button3Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count>0)
			{
				foreach (ListViewItem lvi in this.listView1.CheckedItems) {
					TestUnit tu = ListViewSelect(lvi);
				    AccessDBUtil.delete(tu);
				}
				MessageBox.Show("删除成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
				this.listView1.Items.Clear();
				getAll();
			}
			else
			{
				MessageBox.Show("请至少选择一条记录！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			SaveView sv = new SaveView();
			sv.ShowDialog();
		}
		
		/// <summary>
		/// 所有条件变化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void conditionChanged(object sender, EventArgs e)
		{
			getAll();
		}
		
		/// <summary>
		/// 时间段是否显示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			
			Point cb1p=new Point();
			cb1p=this.comboBox1.Location;
			Point cb2p=new Point();
			cb2p=this.comboBox2.Location;
			if(!this.checkBox2.Checked)
			{
				this.dateTimePicker1.Dispose();
				this.dateTimePicker2.Dispose();
				
				this.label1.Dispose();
				this.label2.Dispose();
				cb1p.X=cb1p.X-234;
				cb2p.X=cb2p.X-234;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
			}
			else
			{
			
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			
			this.panel1.Controls.Add(this.dateTimePicker1);
			this.panel1.Controls.Add(this.dateTimePicker2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.label2);
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(224, 17);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(103, 21);
			this.dateTimePicker1.TabIndex = 38;
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.Location = new System.Drawing.Point(356, 17);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(103, 21);
			this.dateTimePicker2.TabIndex = 39;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(199, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(19, 18);
			this.label1.TabIndex = 40;
			this.label1.Text = "起";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(333, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(17, 18);
			this.label2.TabIndex = 41;
			this.label2.Text = "止";
			cb1p.X=cb1p.X+234;
			cb2p.X=cb2p.X+234;
			this.comboBox1.Location=cb1p;
			this.comboBox2.Location=cb2p;
			}
		}
		
	
	}
}
