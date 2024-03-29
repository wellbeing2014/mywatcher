﻿/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011/6/25
 * 时间: 20:38
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;
namespace WatchCilent
{
	/// <summary>
	/// Description of PackageUI.
	/// </summary>
	public partial class PackageUI : UserControl,UI.MainPlug
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
			return new string[]{"更新包","更新包列表"};
		}
		
		public PackageUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.listView1.DoubleClick+= new EventHandler(Button4Click);
			TreeNode tmp =new TreeNode("全部");
			tmp.Nodes.Add(CommonConst.PACKSTATE_YiJieShou);//已接收
			tmp.Nodes.Add(CommonConst.PACKSTATE_YiChuLi);//已处理
			tmp.Nodes.Add(CommonConst.PACKSTATE_YiCeShi);//已测试
			tmp.Nodes.Add(CommonConst.PACKSTATE_YiFaBu);//已发布
			tmp.Nodes.Add(CommonConst.PACKSTATE_YiFeiZhi);//已废止
			treeView1.Nodes.Add(tmp);
			
			treeView1.SelectedNode=treeView1.Nodes[0].Nodes[0];
		  	//让选中项背景色呈现蓝色
            treeView1.SelectedNode.BackColor = Color.SteelBlue;
            //前景色为白色
            treeView1.SelectedNode.ForeColor = Color.White;
			treeView1.ExpandAll();
			treeView1.NodeMouseClick+= new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
			treeView1.Leave+=new EventHandler(treeView1_Leave);
			treeView1.BeforeSelect+=new TreeViewCancelEventHandler(treeView1_BeforeSelect);
			
			
			List<PersonInfo> datasource_person = PersonDao.getAllPersonInfo();
			PersonInfo person = new PersonInfo();
			person.Fullname = "全部责任人";
			person.Id = 0;
			datasource_person.Insert(0,person);
			this.comboBox2.DataSource = datasource_person;
			this.comboBox2.DisplayMember = "Fullname";
			this.comboBox2.ValueMember = "Id";
			this.comboBox2.SelectedIndexChanged+=new EventHandler(conditionChanged);
			
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
			
			this.comboBox1.SelectedIndexChanged+=new EventHandler(conditionChanged);
			
			System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
			this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
			this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			
			
			this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,(count%pagesize==0)?count/pagesize:count/pagesize+1,this.count);
			getAllPackInList();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
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
				pack.PubPath="";
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
		

		private void getAllPackInList()
		{
			string moduleid = this.comboBox1.SelectedValue.ToString();
			string manageid = this.comboBox2.SelectedValue.ToString();
			string state = this.treeView1.SelectedNode.Text;
			
			string begin=this.dateTimePicker1.Value.ToShortDateString()+" 00:00:00";
			string end =this.dateTimePicker2.Value.ToShortDateString()+" 23:59:59";
			
			if(this.dateTimePicker1.IsDisposed&&this.dateTimePicker2.IsDisposed)
			{
				begin=null;
				end =null;
			}
			
			this.count = PackageDao.queryPackageInfoCount(moduleid,manageid,state,begin,end);
			int countpage = (count%pagesize==0)?count/pagesize:count/pagesize+1;
			if(this.currentpage>countpage) this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,countpage,this.count);
			List<PackageInfo> ls =PackageDao.queryPackageInfo(moduleid,manageid,state,begin,end,
			                                                  (currentpage>1)?((this.currentpage-1)*pagesize):0
			                                                  ,pagesize);
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
			lvi.Checked=false;
			lvi.SubItems.Add(pack.Packagepath);
			lvi.SubItems.Add(pack.Packtime);
			lvi.SubItems.Add(pack.Testtime);
			lvi.SubItems.Add(pack.Publishtime);
			lvi.SubItems.Add(pack.State);
			UIcheckinfo ui = UICheckDao.getUIcheckInfoByPackId(pack.Id);
			if(ui.State==null)
			{
				lvi.SubItems.Add("未生成检查");
			}
			else
				lvi.SubItems.Add(ui.State);
			lvi.SubItems.Add(pack.Moduleid.ToString());
			lvi.SubItems.Add(pack.Managerid.ToString());
			lvi.SubItems.Add(pack.Id.ToString());
			lvi.SubItems.Add(pack.TestRate.ToString());
			this.listView1.Items.Add(lvi);
		}
		
		private PackageInfo  ListViewSelect(ListViewItem lvi)
		{
			PackageInfo pack = new PackageInfo();
			pack.Packagename = lvi.Text;
			pack.Packagepath = lvi.SubItems[1].Text;
			pack.Packtime = lvi.SubItems[2].Text;
			pack.Publishtime = lvi.SubItems[4].Text;
			pack.Testtime = lvi.SubItems[3].Text;
			pack.State = lvi.SubItems[5].Text;
			pack.Moduleid = Int32.Parse(lvi.SubItems[7].Text);
			pack.Managerid =Int32.Parse(lvi.SubItems[8].Text);
			pack.Id = Int32.Parse(lvi.SubItems[9].Text);
			pack.TestRate = Int32.Parse(lvi.SubItems[10].Text);
			return pack;
		}
		
		
		
		//按钮     已删除
		void Button6Click(object sender, EventArgs e)
		{
			if(listView1.CheckedItems.Count!=0)
			{
				foreach(ListViewItem lt in listView1.CheckedItems)
				{
					PackageInfo pack = new PackageInfo();
					pack = ListViewSelect(lt);
					SqlDBUtil.delete(pack);
				}
			}
			else
			{
				MessageBox.Show("请选择更新包","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			getAllPackInList();
		}
		
		//按钮     已测试
		void Button7Click(object sender, EventArgs e)
		{
			if(listView1.CheckedItems.Count!=0)
			{
				foreach(ListViewItem lt in listView1.CheckedItems)
				{
					PackageInfo pack = new PackageInfo();
					pack = ListViewSelect(lt);
					if(pack.State==CommonConst.PACKSTATE_YiJieShou)
					{
						MessageBox.Show(pack.Packagename+"尚未处理，不能标记为已测试","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
						continue;
					}
					pack.State=CommonConst.PACKSTATE_YiCeShi;
					pack.Testtime=System.DateTime.Now.ToLocalTime().ToString();
					SqlDBUtil.update(pack);
				}
			}
			getAllPackInList();
		}
		
		//按钮     已废止
		void Button8Click(object sender, EventArgs e)
		{
			if(listView1.CheckedItems.Count!=0)
			{
				foreach(ListViewItem lt in listView1.CheckedItems)
				{
					PackageInfo pack = new PackageInfo();
					pack = ListViewSelect(lt);
					if(pack.State==CommonConst.PACKSTATE_YiJieShou)
					{
						MessageBox.Show(pack.Packagename+"尚未处理，不能标记为已废止","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
						continue;
					}
					pack.State=CommonConst.PACKSTATE_YiFeiZhi;
					//pack.Publishtime=System.DateTime.Now.ToLocalTime().ToString();
					SqlDBUtil.update(pack);
				}
			}
			getAllPackInList();
		}
		
		//按钮     已发布
		void Button9Click(object sender, EventArgs e)
		{
			if(listView1.CheckedItems.Count!=0)
			{
				foreach(ListViewItem lt in listView1.CheckedItems)
				{
					PackageInfo pack = new PackageInfo();
					pack = ListViewSelect(lt);
					if(pack.State==CommonConst.PACKSTATE_YiJieShou)
					{
						MessageBox.Show(pack.Packagename+"尚未处理，不能标记为已发布","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
						continue;
					}
					pack.State=CommonConst.PACKSTATE_YiFaBu;
					pack.Publishtime=System.DateTime.Now.ToLocalTime().ToString();
					SqlDBUtil.update(pack);
				}
			}
			getAllPackInList();
		}
		
		
		/// <summary>
		/// 时间段显示
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
				// label2
				// 
				this.label2.Location = new System.Drawing.Point(360, 17);
				this.label2.Name = "label2";
				this.label2.Size = new System.Drawing.Size(19, 18);
				this.label2.TabIndex = 10;
				this.label2.Text = "至";
				// 
				// label1
				// 
				this.label1.Location = new System.Drawing.Point(242, 17);
				this.label1.Name = "label1";
				this.label1.Size = new System.Drawing.Size(19, 18);
				this.label1.TabIndex = 9;
				this.label1.Text = "起";
				
				this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
				this.dateTimePicker1.Location = new System.Drawing.Point(267, 13);
				this.dateTimePicker1.Name = "dateTimePicker1";
				this.dateTimePicker1.Size = new System.Drawing.Size(87, 21);
				System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
				this.dateTimePicker1.TabIndex = 7;
				
				this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
				this.dateTimePicker2.Location = new System.Drawing.Point(385, 13);
				this.dateTimePicker2.Name = "dateTimePicker2";
				this.dateTimePicker2.Size = new System.Drawing.Size(84, 21);
				this.dateTimePicker2.TabIndex = 8;
				cb1p.X=cb1p.X+234;
				cb2p.X=cb2p.X+234;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
				
				this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
				this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			}
			getAllPackInList();
		}
		
		/// <summary>
		/// 所有条件变化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void conditionChanged(object sender, EventArgs e)
		{
			getAllPackInList();
		}
		
		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs  e)
		{
			this.treeView1.SelectedNode=e.Node;
			getAllPackInList();
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


		
		
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(this.currentpage>1)
			{
				this.currentpage--;
			}
			getAllPackInList();
		}
		
		void LinkLabel2LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(this.currentpage<((count%pagesize==0)?count/pagesize:count/pagesize+1))
			{
				this.currentpage++;
			}
			getAllPackInList();
		}
		
		void LinkLabel3LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentpage=1;
			getAllPackInList();
		}
		
		void LinkLabel4LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentpage=(count%pagesize==0)?count/pagesize:count/pagesize+1;
			getAllPackInList();
		}
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			bool isallcheck=this.checkBox1.Checked;
				foreach (ListViewItem element in this.listView1.Items) {
					element.Checked=isallcheck;
				}
			
		}
	}
}
