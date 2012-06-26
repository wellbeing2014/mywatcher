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
using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;
using System.Collections.Generic;
namespace WatchCilent.UI.Test
{
	/// <summary>
	/// Description of TestListUI.
	/// </summary>
	public partial class TestListUI : UserControl,MainPlug
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
		
		
		public CommonConst.UIShowSytle getSytle()
		{
			return CommonConst.UIShowSytle.UserControl;
		}
		public string getAuthorCode()
		{
			return "3,4,5";
		}
		
		public string[] getPlugName()
		{
			return new string[]{"测试","缺陷列表"};
		}
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
			treeView1.ExpandAll();
			treeView1.SelectedNode=treeView1.Nodes[0].Nodes[0];
			//让选中项背景色呈现蓝色
            treeView1.SelectedNode.BackColor = Color.SteelBlue;
            //前景色为白色
            treeView1.SelectedNode.ForeColor = Color.White;
			System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
				
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
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox3.Items.Add("全部等级");
			this.comboBox3.Items.AddRange(CommonConst.BUGLEVEL);
			this.comboBox3.SelectedIndex = 0;
			
			this.comboBox3.SelectedIndexChanged+=new EventHandler(conditionChanged);
			this.comboBox1.SelectedIndexChanged+=new EventHandler(conditionChanged);
			this.comboBox2.SelectedIndexChanged+=new EventHandler(conditionChanged);
			
			treeView1.NodeMouseClick+= new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
			treeView1.Leave+=new EventHandler(treeView1_Leave);
			treeView1.BeforeSelect+=new TreeViewCancelEventHandler(treeView1_BeforeSelect);			
			
			this.listView1.DoubleClick += new EventHandler(ListVew_DoubleClick); 
			this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
			this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			
			this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,(count%pagesize==0)?count/pagesize:count/pagesize+1,this.count);
			
			getTestUnitList();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		#region 通用方法
		/// <summary>
		/// 获取所有缺陷单元
		/// </summary>
		void getTestUnitList()
		{
			string moduleid = this.comboBox1.SelectedValue.ToString();
			string manageid = this.comboBox2.SelectedValue.ToString();
			string level = this.comboBox3.Text;
			string state = this.treeView1.SelectedNode.Text;
			
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
		
		
		
		#endregion 
		
		#region 按钮等 响应方法
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
			getTestUnitList();
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
				TestResult tr = new TestResult(TestUnitDao.gettestUnitById(tu.Id),null);
				tr.ShowDialog();
			}
			else
			{
				MessageBox.Show("请选择一条记录","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			getTestUnitList();
		}
		
		void ListVew_DoubleClick(object sender, EventArgs e)
		{
			if(this.listView1.SelectedItems.Count==1)
			{
				TestUnit tu = ListViewSelect(this.listView1.SelectedItems[0]);
				TestResult tr = null;
				TestListParameter tp = new TestListParameter();
				
				if(this.dateTimePicker1.IsDisposed&&this.dateTimePicker2.IsDisposed)
				{
					tp.Begintime=null;
					tp.Endtime =null;
				}
				else
				{
					tp.Begintime = this.dateTimePicker1.Value.ToShortDateString()+" 00:00:00";
					tp.Endtime = this.dateTimePicker2.Value.ToShortDateString()+" 23:59:59";;
				}
				tp.Level = this.comboBox3.Text;
				tp.Manageid = this.comboBox2.SelectedValue.ToString();
				tp.Moduleid = this.comboBox1.SelectedValue.ToString();
				
				tp.Pagesize = pagesize ;
				tp.Startindex = (currentpage-1)*pagesize+this.listView1.SelectedItems[0].Index;
				tp.State = this.treeView1.SelectedNode.Text;
				//tr.Tp = tp;
				tr = new TestResult(TestUnitDao.gettestUnitById(tu.Id),tp);
				tr.ShowDialog();
			}
			else
			{
				MessageBox.Show("请选择一条记录","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			getTestUnitList();
			
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
				    SqlDBUtil.delete(tu);
				}
				MessageBox.Show("删除成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
				this.listView1.Items.Clear();
				getTestUnitList();
			}
			else
			{
				MessageBox.Show("请至少选择一条记录！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}
		/// <summary>
		/// 生成测试报告
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button4Click(object sender, EventArgs e)
		{
			SaveView sv = new SaveView();
			sv.ShowDialog();
		}
		/// <summary>
		/// 已确认
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button5Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count>0)
			{
				foreach (ListViewItem lvi in this.listView1.CheckedItems) {
					string id =lvi.SubItems[7].Text;
					string state=Enum.GetName(typeof(CommonConst.TestState),CommonConst.TestState.已确认);
					TestUnitDao.UpdateState(state,id);
				}
				getTestUnitList();
			}
			else
			{
				MessageBox.Show("请至少选择一条记录！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}
		/// <summary>
		/// 已废止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button8Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count>0)
			{
				foreach (ListViewItem lvi in this.listView1.CheckedItems) {
					string id =lvi.SubItems[7].Text;
					string state=Enum.GetName(typeof(CommonConst.TestState),CommonConst.TestState.已废止);
					TestUnitDao.UpdateState(state,id);
				}
				getTestUnitList();
			}
			else
			{
				MessageBox.Show("请至少选择一条记录！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}
		/// <summary>
		/// 已修订
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button6Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count>0)
			{
				foreach (ListViewItem lvi in this.listView1.CheckedItems) {
					//TestUnit oldtu = ListViewSelect(lvi);
					string id =lvi.SubItems[7].Text;
					string state=Enum.GetName(typeof(CommonConst.TestState),CommonConst.TestState.已修订);
					TestUnitDao.UpdateState(state,id);
				}
				getTestUnitList();
			}
			else
			{
				MessageBox.Show("请至少选择一条记录！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
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
			this.dateTimePicker1.Location = new System.Drawing.Point(224, 17);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(103, 21);
			System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
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
		#endregion
		
		#region 状态树的事件
		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs  e)
		{
			this.treeView1.SelectedNode=e.Node;
			getTestUnitList();
		}
		 
		//将要选中新节点之前发生
        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView1.SelectedNode.BackColor = Color.Empty;
                //还原前景色
                treeView1.SelectedNode.ForeColor = Color.Black;
            }
        }
        
        //失去焦点时
        private void treeView1_Leave(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode!=null)
            {
                //让选中项背景色呈现蓝色
                treeView1.SelectedNode.BackColor = Color.SteelBlue;
                //前景色为白色
                treeView1.SelectedNode.ForeColor = Color.White;
            }
        }
		#endregion 
		
		
		
		
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			bool isallcheck=this.checkBox1.Checked;
				foreach (ListViewItem element in this.listView1.Items) {
					element.Checked=isallcheck;
				}
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
	}
}
