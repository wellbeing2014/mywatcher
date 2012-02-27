/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2012/2/9
 * 时间: 17:18
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using WatchCore.dao;
using WatchCore.pojo;
using WatchCore.Common;

namespace WatchCilent.UI.Theme
{
	/// <summary>
	/// Description of ChoseThemeDialog.
	/// </summary>
	public partial class ChoseThemeDialog : Form
	{
		public List<TestTheme> selthem = new List<TestTheme>();
		private string unitid;
		
		public string Unitid {
			set { unitid = value; }
		}
		public ChoseThemeDialog(string unitid)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			//unitid = "161";
			this.unitid=unitid;
			
				
				
			InitializeComponent();
			this.treeView1.AfterCheck += new TreeViewEventHandler(treeView1_AfterCheck);
			getThemeTree();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		
		void getThemeTree()
		{
			if(unitid!=null)
				selthem = TestThemeDao.getTestThemeByUnitid(unitid);
			List<TestTheme> ttlist = TestThemeDao.getAllTestThemeByPersonname(System.Configuration.ConfigurationManager.AppSettings["username"]);
			this.treeView1.Nodes.Clear();
			//List<TreeNode> maintreelist = new List<TreeNode>();
			TreeNode main =new TreeNode();
			//默认主题
			TreeNode tmp =new TreeNode("默认主题");
			TestTheme default_tt = new TestTheme();
			default_tt.Id=99999;
			default_tt.Personid=0;
			default_tt.Personname="朱新培";
			tmp.Tag = default_tt;
			
			setTreeNodeCheck(tmp);
			
			main.Nodes.Add(tmp);
            
			
			foreach (var element in ttlist) {
				TreeNode tmp1 = null;
				tmp1 = new TreeNode(element.Favname);
				tmp1.Tag = element;
				
				
				if(element.Parentid == 0)
				{
					main.Nodes.Add(tmp1);
					setTreeNodeCheck(tmp1);
					
				}
				else
				{
					creatTree(tmp1,main);
				}
			}
			TreeNode[] tn = new TreeNode[main.Nodes.Count];
			main.Nodes.CopyTo(tn,0);
			this.treeView1.Nodes.AddRange(tn);
			this.treeView1.ExpandAll();
			this.treeView1.SelectedNode = treeView1.Nodes[0];
			
		}
		
		void setTreeNodeCheck(TreeNode tn)
		{
			TestTheme tt = tn.Tag as TestTheme;
			foreach (var element in selthem) {
				if(tt.Id==element.Id)
				{
					tn.Checked = true;
					//selthem.Remove(element);
					//TreeViewCheck.ExpandParent(tn);
				}
			}
		}
		
		
		
		void creatTree(TreeNode childtn,TreeNode parenttn)
		{
			if(parenttn.Tag!=null)
			{
				TestTheme child = childtn.Tag as TestTheme;
				TestTheme parent = parenttn.Tag as TestTheme;
				setTreeNodeCheck(parenttn);
				
				if(child.Parentid == parent.Id)
				{
					parenttn.Nodes.Add(childtn);
					setTreeNodeCheck(childtn);
					
					return ;
				} 
			}

			for (int i = 0; i < parenttn.Nodes.Count; i++) {
				 creatTree(childtn,parenttn.Nodes[i]);
			}
			return ;
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
						MessageBox.Show("删除成功","提示");
						getThemeTree();
					}
				}
			}
		}
		
		//确定 选中
		void Button3Click(object sender, EventArgs e)
		{
//			if(this.treeView1.SelectedNode == null)
//			{
//				MessageBox.Show("请选择关注主题");
//			}
//			else this.selthem.Add(this.treeView1.SelectedNode.Tag as TestTheme);
			List<TreeNode> sel_temp = new List<TreeNode>();
			TreeViewCheck.GetSelectedTreeNode(this.treeView1.Nodes,sel_temp);
			selthem.Clear();
			foreach (var element in sel_temp) {
				selthem.Add(element.Tag as TestTheme);
			}
			
		}
		
		private void treeView1_AfterCheck(object sender, TreeViewEventArgs e) 
		{ 
		    //TreeViewCheck.CheckControl(e);
		     if (e.Node != null && !Convert.IsDBNull(e.Node))
            {
                if (e.Node.Nodes.Count > 0)
                {
                	e.Node.ExpandAll();
                }
            }
		}
	}
}
