/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-7-18
 * 时间: 11:07
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using WatchCilent.dao;
using WatchCilent.pojo;
using System.Collections.Generic;
using System.IO;

namespace WatchCilent.UI.Pack
{
	/// <summary>
	/// Description of SaveView.
	/// </summary>
	public partial class SaveView : Form
	{
		public SaveView()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			List<TestUnit> tulist = TestUnitDao.getAlltestUnit();
			
			RichTextBox rtf = new RichTextBox(); 
			int i=1;
			foreach(TestUnit tu in tulist)
			{
				int startlength =this.richTextBox1.Text.Length;
				string title = i.ToString()+"、"+tu.Testtitle+"\n";
				this.richTextBox1.AppendText(title);
				this.richTextBox1.SelectionStart = startlength;
				this.richTextBox1.SelectionLength = this.richTextBox1.Text.Length-startlength;
				
				this.richTextBox1.SelectionFont = new Font("宋体", 14 ,FontStyle.Bold);
				this.richTextBox1.SelectionColor = Color.Red;
				this.richTextBox1.SelectionStart = this.richTextBox1.Rtf.Length;
				rtf.LoadFile(new MemoryStream(tu.Testcontent),RichTextBoxStreamType.RichText);
				rtf.SelectAll();
				this.richTextBox1.SelectedRtf = rtf.SelectedRtf;
				this.richTextBox1.SelectionStart = this.richTextBox1.Rtf.Length;
				this.richTextBox1.AppendText("\n");
				i++;
			}
			rtf.Dispose();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
