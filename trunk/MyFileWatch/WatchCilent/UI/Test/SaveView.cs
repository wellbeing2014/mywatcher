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
using System.Data;
using System.Threading;

namespace WatchCilent.UI.Test
{
	/// <summary>
	/// Description of SaveView.
	/// </summary>
	public partial class SaveView : Form
	{
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
			
			//
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Thread th = new Thread(new ParameterizedThreadStart(ThreadFunc));
			th.Start(this.label3);
			
		}
		private delegate void del_do_changetxt(Label lab,string text);
		
		void do_changetxt(Label lab,string text)
		{
			lab.Text = text;
		}
		
		void ThreadFunc(object label1)
		{
			Label label =(Label)label1;
			del_do_changetxt delchangetxt = new del_do_changetxt(do_changetxt);
			string msg = "正在启动";
			label.BeginInvoke(delchangetxt,new object[]{label,msg});
			WordDocumentMerger wm = new WordDocumentMerger();
			try {
				if(unitDOCpath==null||"".Equals(unitDOCpath))
				{
					unitDOCpath = this.defaultpath;
				}
				//打开模版
				wm.Open(defaultpath+@"\temp\TestReport.doc");
				
				label.BeginInvoke(delchangetxt,new object[]{label,"打开模版"});
				//插入标签
				wm.WriteIntoMarkBook("Atitle","权力运行许可平台");
				string begintime = "2011-06-01";
				string endtime = "2011-08-31";
				label.BeginInvoke(delchangetxt,new object[]{label,"正在导入更新包"});
				DataTable table1 =PackageDao.getRePortPack();
				wm.insertTableForPack(1,table1);
				label.BeginInvoke(delchangetxt,new object[]{label,"正在导入测试单元"});
				DataTable table2 =TestUnitDao.getRePortTest(begintime,endtime);
				wm.insertTableForTest(2,table2);
				label.BeginInvoke(delchangetxt,new object[]{label,"完成"});
				
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
