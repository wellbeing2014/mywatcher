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
		public ThemeListUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void getThemeTree()
		{
			List<TestTheme> ttlist = TestThemeDao.getAllTestThemeByPersonname("朱新培");
			List<TreeNode> maintreelist = new List<TreeNode>();
			TreeNode tmp =new TreeNode("默认主题");
			foreach (var element in ttlist) {
				TreeNode tmp1 = null;
				if(element.Parentid == 0)
				{
					tmp1 = new TreeNode();
				}
			}
		}
	}
}
