/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2012-1-10
 * 时间: 13:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WatchCilent.UI.Theme
{
	/// <summary>
	/// Description of CreateThemeDialog.
	/// </summary>
	public partial class CreateThemeDialog : Form
	{
		public string fname = "";
		public CreateThemeDialog()
		{
			InitializeComponent();
		}

		
		void Button1Click(object sender, EventArgs e)
		{
			string temp = this.textBox1.Text;
			if(string.IsNullOrEmpty(temp))
			{
				MessageBox.Show("文件夹名称为空或有非法字符","提示");
				return;
			}
			else
			{
				this.fname=temp;
			}
		}
	}
}
