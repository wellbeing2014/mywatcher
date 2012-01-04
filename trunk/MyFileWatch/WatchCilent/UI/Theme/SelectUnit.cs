/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-12-26
 * 时间: 18:26
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;
using System.Collections.Generic;

namespace WatchCilent.UI.Theme
{
	/// <summary>
	/// Description of SelectUnit.
	/// </summary>
	public partial class SelectUnit : Form
	{
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private int currentpage=0;
		private int count = 0;
		private int pagesize = 20;
		
		private string currentstr = "当前第{0}页";
		private string countstr ="共{0}页/共{1}条";
		private string pagestr ="每页{0}条";
		
		public List<TestUnit> select_tu = new List<TestUnit>();
		public SelectUnit()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		/// <summary>
		/// 获取所有缺陷单元
		/// </summary>
		void getTestUnitList()
		{
			string moduleid = this.comboBox1.SelectedValue.ToString();
			string manageid = this.comboBox2.SelectedValue.ToString();
			string level = this.comboBox3.Text;
			string state = "全部";
			
			string begin=this.dateTimePicker1.Value.ToShortDateString()+" 00:00:00";
			string end =this.dateTimePicker2.Value.ToShortDateString()+" 23:59:59";
			if(this.dateTimePicker1.IsDisposed&&this.dateTimePicker2.IsDisposed)
			{
				begin=null;
				end =null;
			}
			
			this.count = TestUnitDao.QueryTestUnitCount(moduleid,manageid,level,state,begin,end);
			int countpage = (count%pagesize==0)?count/pagesize:count/pagesize+1;
			if(this.currentpage>countpage) this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,countpage,this.count);
			
			this.listView1.Items.Clear();
			List<TestUnit>alltu=TestUnitDao.QueryTestUnit(moduleid,manageid,level,state,begin,end,
			                                             (currentpage>1)?((this.currentpage-1)*pagesize):0
			                                                  ,pagesize);
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
		/// 所有条件变化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void conditionChanged(object sender, EventArgs e)
		{
			getTestUnitList();
		}
		
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			Point cb1p=new Point();
			cb1p=this.comboBox1.Location;
			Point cb2p=new Point();
			cb2p=this.comboBox2.Location;
			Point cb3p=new Point();
			cb3p=this.comboBox3.Location;
			if(!this.checkBox2.Checked)
			{
				this.dateTimePicker1.Dispose();
				this.dateTimePicker2.Dispose();
				this.label1.Dispose();
				this.label2.Dispose();
				cb1p.X=cb1p.X-250;
				cb2p.X=cb2p.X-250;
				cb3p.X=cb3p.X-250;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
				this.comboBox3.Location=cb3p;
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
			this.dateTimePicker1.Location = new System.Drawing.Point(95, 4);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(106, 21);
			System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
			this.dateTimePicker1.TabIndex = 71;
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.Location = new System.Drawing.Point(236, 4);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(107, 21);
			this.dateTimePicker2.TabIndex = 72;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(70, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(19, 18);
			this.label1.TabIndex = 73;
			this.label1.Text = "起";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(213, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(17, 18);
			this.label2.TabIndex = 74;
			this.label2.Text = "止";
			cb1p.X=cb1p.X+250;
			cb2p.X=cb2p.X+250;
			cb3p.X=cb3p.X+250;
			this.comboBox1.Location=cb1p;
			this.comboBox2.Location=cb2p;
			this.comboBox3.Location=cb3p;
			this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
			this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			}
			getTestUnitList();
		}
		
		void LinkLabel3LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentpage=1;
			getTestUnitList();
		}
		
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(this.currentpage>1)
			{
				this.currentpage--;
			}
			getTestUnitList();
		}
		
		void LinkLabel2LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(this.currentpage<((count%pagesize==0)?count/pagesize:count/pagesize+1))
			{
				this.currentpage++;
			}
			getTestUnitList();
		}
		
		void LinkLabel4LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentpage=(count%pagesize==0)?count/pagesize:count/pagesize+1;
			getTestUnitList();
		}
		
		//确定
		void Button1Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count>0)
			{
				foreach (ListViewItem lvi in this.listView1.CheckedItems) {
					TestUnit tu = ListViewSelect(lvi);
					select_tu.Add(tu);
				}
			}
			else
			{
				MessageBox.Show("请至少选择一条记录！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}
	}
}
