/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/6/14
 * 时间: 16:39
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;
namespace WatchCilent.UI.Help
{
	/// <summary>
	/// Description of HelpWnd.
	/// </summary>
	public partial class HelpWnd : Form,MainPlug
	{
		
		public CommonConst.UIShowSytle getSytle()
		{
			return CommonConst.UIShowSytle.Form;
		}
		
		public string getAuthorCode()
		{
			return "0,1,2,3,4,5";
		}
		
		public string[] getPlugName()
		{
			return new string[]{"帮助","关于"};
		}
		
		public HelpWnd()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("notepad.exe",System.Environment.CurrentDirectory+"\\releasenote.txt");
		}
	}
}
