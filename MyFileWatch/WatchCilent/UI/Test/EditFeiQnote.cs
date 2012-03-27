/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2012/2/23
 * 时间: 19:14
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WatchCilent.UI.Test
{
	/// <summary>
	/// Description of EditFeiQnote.
	/// </summary>
	public partial class EditFeiQnote : Form
	{
		public string ip="";
		public bool adminip = false;
		public EditFeiQnote()
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
			ip = this.textBox1.Text;
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(this.checkBox1.Checked)
				adminip = true;
			else
				adminip = false;
		}
	}
}
