/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/6/12
 * 时间: 14:52
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;

namespace WatchCilent.UI
{
	/// <summary>
	/// Description of Test.
	/// </summary>
	public partial class Test1 : UserControl,MainPlug
	{
		public Test1()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public CommonConst.UIShowSytle getSytle()
		{
			return CommonConst.UIShowSytle.UserControl;
		}
		
		public string getAuthorCode()
		{
			//可怜的普通人
			return "0";
		}
		
		public string[] getPlugName()
		{
			return new string[]{"测试","测试菜单"};
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			MessageBox.Show("您不具备系统权限，给个按钮您点着玩！");
		}
	}
}
