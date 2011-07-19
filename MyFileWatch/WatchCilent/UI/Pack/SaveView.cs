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
using WatchCilent.Common;
using System.Collections.Generic;
using System.IO;

namespace WatchCilent.UI.Pack
{
	/// <summary>
	/// Description of SaveView.
	/// </summary>
	public partial class SaveView : Form
	{
		private List<TestUnit> tulist = new List<TestUnit>();
		string tempPath = Path.GetTempPath();
		public SaveView()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.CenterToScreen();
			InitializeComponent();
			tulist = TestUnitDao.getAlltestUnit();
			
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
				
				
				rtf.SaveFile(tempPath+@"\"+tu.Unitno+".doc");
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
		
		void Button2Click(object sender, EventArgs e)
		{
		//	FunctionUtils.OpenDocument(@"E:\SVN目录\MyFileWatch\测试报告模版.doc");
		//	FunctionUtils.WriteIntoDocument("Atitle","权力运行");
			//FunctionUtils.SaveAndClose(@"E:\SVN目录\MyFileWatch\测试报告2.doc");
			WordDocumentMerger wm = new WordDocumentMerger();
			wm.Open(@"E:\SVN\mywatcher\MyFileWatch\测试报告模版.doc");
			
			wm.WriteIntoMarkBook("Atitle","权力运行2131231231");
			foreach(TestUnit tu in tulist)
			{
				wm.AppendText(tu.Testtitle,"标题 2");
				wm.InsertMerge(new string[]{tempPath+@"\"+tu.Unitno+".doc"});
			}
			wm.SaveAs();
			
		}
	}
}
