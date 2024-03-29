﻿/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-12-22
 * 时间: 11:09
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.dao;
using WatchCore.pojo;
using System.Collections.Generic;
using WatchCore.Common;
using WatchCilent.UI.Test;

namespace WatchCilent.UI.Theme
{
	/// <summary>
	/// Description of ThemeListUI.
	/// </summary>
	public partial class ThemeListUI : UserControl,UI.MainPlug
	{
		private TreeNode themeTree = new TreeNode("默认主题");
		
		
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
			return new string[]{"测试","缺陷监控"};
		}
		
		public ThemeListUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			getThemeTree();
			this.treeView1.BeforeSelect += new TreeViewCancelEventHandler(treeView1_BeforeSelect);
			this.treeView1.Leave += new EventHandler(treeView1_Leave);
			this.treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
			this.listView1.DoubleClick += new EventHandler(ListVew_DoubleClick); 
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
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
		
        
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs  e)
		{
			this.treeView1.SelectedNode=e.Node;
			getGuanlianUnitList();
		}
		 
		void getThemeTree()
		{
			List<TestTheme> ttlist = TestThemeDao.getAllTestThemeByPersonname(System.Configuration.ConfigurationManager.AppSettings["username"]);
			this.treeView1.Nodes.Clear();
			//List<TreeNode> maintreelist = new List<TreeNode>();
			TreeNode main =new TreeNode();
			//默认主题
			TreeNode tmp =new TreeNode("默认主题");
			TestTheme default_tt = new TestTheme();
			default_tt.Id=99999;
			string personid = GlobalParams.UserId;
			default_tt.Personid=((personid==null)?0:Int32.Parse(personid));
			default_tt.Personname=GlobalParams.Username;
			tmp.Tag = default_tt;
			main.Nodes.Add(tmp);
            
			foreach (var element in ttlist) {
				TreeNode tmp1 = null;
				tmp1 = new TreeNode(element.Favname);
				tmp1.Tag = element;
				if(element.Parentid == 0)
				{
					main.Nodes.Add(tmp1);
				}
				else
				{
					creatTree(tmp1,main);
				}
			}
			TreeNode[] tn = new TreeNode[main.Nodes.Count];
			main.Nodes.CopyTo(tn,0);
			this.treeView1.Nodes.AddRange(tn);
			
			this.treeView1.SelectedNode = treeView1.Nodes[0];
			//让选中项背景色呈现蓝色
            treeView1.SelectedNode.BackColor = Color.SteelBlue;
            //前景色为白色
            treeView1.SelectedNode.ForeColor = Color.White;
		}
		
		void creatTree(TreeNode childtn,TreeNode parenttn)
		{
			if(parenttn.Tag!=null)
			{
				TestTheme child = childtn.Tag as TestTheme;
				TestTheme parent = parenttn.Tag as TestTheme;
				if(child.Parentid == parent.Id)
				{
					parenttn.Nodes.Add(childtn);
					return ;
				} 
			}

			for (int i = 0; i < parenttn.Nodes.Count; i++) {
				 creatTree(childtn,parenttn.Nodes[i]);
//				parenttn.Nodes.RemoveAt(i);
//				parenttn.Nodes.Insert(i,tn);
			}
			return ;
		}
		

		
		//新增缺陷关联
		void Button3Click(object sender, EventArgs e)
		{
			SelectUnit su = new SelectUnit();
			su.StartPosition = FormStartPosition.CenterParent;
			DialogResult dr = su.ShowDialog();
			if(dr==DialogResult.OK)
			{
				TestTheme theme = this.treeView1.SelectedNode.Tag as TestTheme;
				foreach (var element in su.select_tu) {
					Testunittheme tt = new Testunittheme();
					tt.Themeid = theme.Id;
					tt.Unitid = element.Id;
					SqlDBUtil.insert(tt);
				}
				getGuanlianUnitList();
			}
		}
		
		/// <summary>
		/// 删除缺陷关联
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button4Click(object sender, EventArgs e)
		{
			int selnum = this.listView1.SelectedItems.Count;
			if(selnum>0)
			{
				string[] unitidSZ = new string[selnum];
				for (int i = 0; i < selnum; i++) {
					unitidSZ[i] = listView1.SelectedItems[i].SubItems[4].Text;
				}
				DialogResult a = MessageBox.Show("您正准备删除"+selnum+"条缺陷……","删除",MessageBoxButtons.OKCancel);
				if(DialogResult.OK==a)
				{
					TestunitthemeDao.DelGuanLianUnit(unitidSZ,((TestTheme)(this.treeView1.SelectedNode.Tag)).Id.ToString());
					MessageBox.Show("删除成功","删除");
				}
				getGuanlianUnitList();
			}
			else
				MessageBox.Show("请选择要删除的缺陷","删除");
			
			
		}
				/// <summary>
		/// 查询关联的测试单元列表
		/// </summary>
		void getGuanlianUnitList()
		{
			TestTheme theme= this.treeView1.SelectedNode.Tag as TestTheme;
			string themeid =theme.Id.ToString();
			List<TestUnit> tulist =TestUnitDao.getGuanLianUnitList(themeid);
			this.listView1.Items.Clear();
			foreach (TestUnit tu in tulist) {
				ListViewBing(tu);
			}
			if(string.IsNullOrEmpty(theme.Favcontent))
				this.textBox1.Text = "请输入描述……";
			else
				this.textBox1.Text = theme.Favcontent;
			
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
			//lvi.SubItems.Add(tu.Buglevel);
			lvi.SubItems.Add(tu.Testtitle);
			//lvi.SubItems.Add(tu.Testtime);
			lvi.SubItems.Add(tu.Adminname);//
			//lvi.SubItems.Add(tu.State);
			lvi.SubItems.Add(tu.Id.ToString());//
			this.listView1.Items.Add(lvi);
			
		}
		private TestUnit  ListViewSelect(ListViewItem lvi)
		{
			TestUnit tu = new TestUnit();
			tu.Unitno = lvi.Text;
			tu.Packagename = lvi.SubItems[1].Text;
			//tu.Buglevel = lvi.SubItems[2].Text;
			tu.Testtitle = lvi.SubItems[2].Text;
			//tu.Testtime = lvi.SubItems[4].Text;
			tu.Adminname = lvi.SubItems[3].Text;
			//tu.State = lvi.SubItems[6].Text;
			tu.Id = Int32.Parse(lvi.SubItems[4].Text);
			return tu;
		}
		
			
		void ListVew_DoubleClick(object sender, EventArgs e)
		{
			if(this.listView1.SelectedItems.Count==1)
			{
				TestUnit tu = ListViewSelect(this.listView1.SelectedItems[0]);
				TestResult tr = null;
				tr = new TestResult(TestUnitDao.gettestUnitById(tu.Id),null);
				tr.ShowDialog();
			}
			else{
MessageBox.Show("请选择一条记录","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}			
		}			
		
		//新增主题
		void Button1Click(object sender, EventArgs e)
		{
			if(this.treeView1.SelectedNode==null)
			{
				MessageBox.Show("请选择主题");
				return;
			}
			TestTheme theme1= this.treeView1.SelectedNode.Tag as TestTheme;
			CreateThemeDialog ct = new CreateThemeDialog();
			ct.StartPosition = FormStartPosition.CenterParent;
			DialogResult dr = ct.ShowDialog();
			if(dr==DialogResult.OK)
			{
				TestTheme theme2 = new TestTheme();
				//theme2.Id=theme1.Id;
				if(ct.isTreeRoot)
				{
					theme2.Parentid=0;
					theme2.Personid=0;
					
				}
				else
				{
					theme2.Parentid=theme1.Id;
					theme2.Personid=theme1.Personid;
				}
				theme2.Personname=theme1.Personname;
				theme2.Favname=ct.fname;
				SqlDBUtil.insert(theme2);
				MessageBox.Show("创建成功","提示");
				getThemeTree();
			}
		}
		
		
		//删除主题
		void Button2Click(object sender, EventArgs e)
		{
			if(this.treeView1.SelectedNode==null)
			{
				MessageBox.Show("请选择要删除的主题");
				return;
			}
			else
			{
				
				TestTheme theme= this.treeView1.SelectedNode.Tag as TestTheme;
				if(theme.Id ==99999)
				{
					MessageBox.Show("默认主题不能删除","提示");
				}
				else
				{
					DialogResult a = MessageBox.Show("您正准备删除主题，与之关联的缺陷将一并删除","删除",MessageBoxButtons.OKCancel);
					if(DialogResult.OK==a)
					{
						TestunitthemeDao.DelGuanLianUnit(null,theme.Id.ToString());
						TestThemeDao.DeleteTheme(theme.Id.ToString());
						this.listView1.Items.Clear();
						MessageBox.Show("删除成功","提示");
						getThemeTree();
					}
				}
			}
		}
		
		
		//保存描述的方法
		void Button5Click(object sender, EventArgs e)
		{
			TestTheme theme= this.treeView1.SelectedNode.Tag as TestTheme;
			theme.Favcontent=this.textBox1.Text;
			SqlDBUtil.update(theme);
			MessageBox.Show("保存成功","提示");
		}
		
		void textBox1_TextChanged(object sender, EventArgs e)
		{
			MessageBox.Show("您正在离开编辑内容描述，是否保存","提示",MessageBoxButtons.OKCancel);
		}
	}
}
