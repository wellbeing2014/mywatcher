/*
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


namespace WatchCilent.UI.Theme
{
	/// <summary>
	/// Description of ThemeListUI.
	/// </summary>
	public partial class ThemeListUI : UserControl
	{
		private TreeNode themeTree = new TreeNode("默认主题");
		public ThemeListUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			getThemeTree();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void getThemeTree()
		{
			List<TestTheme> ttlist = TestThemeDao.getAllTestThemeByPersonname("朱新培");
			//List<TreeNode> maintreelist = new List<TreeNode>();
			TreeNode main =new TreeNode();
			//默认主题
			TreeNode tmp =new TreeNode("默认主题");
			TestTheme default_tt = new TestTheme();
			default_tt.Id=99999;
			default_tt.Personid=0;
			default_tt.Personname="朱新培";
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
				
			}
		}
	}
}
