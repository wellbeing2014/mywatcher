/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-8-5
 * 时间: 14:32
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;

namespace WatchCilent.UI.Pub
{
	/// <summary>
	/// Description of CreateNameForm.
	/// </summary>
	public partial class CreateNameForm : Form
	{
		public string name="";
		public CreateNameForm()
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
			string temp = this.textBox1.Text;
			if(string.IsNullOrEmpty(temp))
			{
				MessageBox.Show("文件夹名称为空或有非法字符","提示");
				return;
			}
			else
			{
				this.name=temp;
			}
		}
	}
}
