/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/6/15
 * 时间: 11:17
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;

namespace WatchCilent.UI.WIMS
{
	/// <summary>
	/// Description of PackGuide.
	/// </summary>
	public partial class PackGuide : Form,UI.MainPlug
	{
		public CommonConst.UIShowSytle getSytle()
		{
			return CommonConst.UIShowSytle.Form;
		}
		public string getAuthorCode()
		{
			return "1,2,3,4";
		}
		
		public string[] getPlugName()
		{
			return new string[]{"工具","打包辅助"};
		}
		
		public PackGuide()
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
			this.openFileDialog1.ShowDialog();
		}
	}
}
