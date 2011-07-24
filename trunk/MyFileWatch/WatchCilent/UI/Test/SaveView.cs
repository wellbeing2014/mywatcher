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

namespace WatchCilent.UI.Test
{
	/// <summary>
	/// Description of SaveView.
	/// </summary>
	public partial class SaveView : Form
	{
		private List<TestUnit> tulist = new List<TestUnit>();
		//缺陷列表的HTML路径
		string unitHTMLpath = FunctionUtils.AutoCreateFolder(System.Configuration.ConfigurationManager.AppSettings["UnitHtmlPath"]);
		//缺陷列表的DOC路径
		string unitDOCpath = FunctionUtils.AutoCreateFolder(System.Configuration.ConfigurationManager.AppSettings["UnitDocPath"]);
		//默认路径
		string defaultpath =System.Environment.CurrentDirectory;
		public SaveView()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			InitializeComponent();
			this.CenterToScreen();
			tulist = TestUnitDao.getAlltestUnit();
			RichTextBox rtftemp = new RichTextBox(); //临时控件
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
				//缓存到临时控件中
				rtftemp.LoadFile(new MemoryStream(tu.Testcontent),RichTextBoxStreamType.RichText);
				//rtftemp.SaveFile(unitDOCpath+@"\"+tu.Testtitle+".doc");
				rtftemp.SelectAll();
				this.richTextBox1.SelectedRtf = rtftemp.SelectedRtf;
				this.richTextBox1.SelectionStart = this.richTextBox1.Rtf.Length;
				this.richTextBox1.AppendText("\n");
				i++;
			}
			rtftemp.Dispose();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			WordDocumentMerger wm = new WordDocumentMerger();
			try {
				if(unitDOCpath==null||"".Equals(unitDOCpath))
				{
					unitDOCpath = this.defaultpath;
				}
				//打开模版
				wm.Open(defaultpath+@"\temp\TestReport.doc");
				//插入标签
				wm.WriteIntoMarkBook("Atitle","权力运行许可平台");
				//缺陷循环插入
				foreach(TestUnit tu in tulist)
				{
					wm.AppendText(tu.Testtitle,"标题 2");
					wm.InsertMerge(new string[]{unitDOCpath+@"\"+tu.Unitno+".doc"},null);
				}
				//保存
				wm.SaveAs();
				MessageBox.Show("保存成功","提示");
			} catch (Exception e1) {
				MessageBox.Show(e1.ToString(),"提示", MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			finally
			{
				wm.Quit();
			}
		}
	}
}
