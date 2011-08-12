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
			this.textBox1.ReadOnly=true;
			this.CenterToScreen();
			
			//
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if(this.textBox1.Text==null||this.textBox1.Text.Equals(""))
			{
				MessageBox.Show("请选择保存路径","提示");
				return ;
			}
			Thread th = new Thread(new ParameterizedThreadStart(ThreadFunc));
			th.Start(this.textBox1.Text);
			
		}
		private delegate void del_do_changetxt(string text,int pgbvalue);
		
		void do_changetxt(string text,int pgbvalue)
		{
			this.label3.Text = text;
			for (int i = this.progressBar1.Value; i < pgbvalue; i++) {
				this.progressBar1.Value=i;
			}
			
		}
		
		void ThreadFunc(object path)
		{
			del_do_changetxt delchangetxt = new del_do_changetxt(do_changetxt);
			this.BeginInvoke(delchangetxt,new object[]{"正在启动WORD",10});
			WordDocumentMerger wm = new WordDocumentMerger();
			try {
				if(unitDOCpath==null||"".Equals(unitDOCpath))
				{
					unitDOCpath = this.defaultpath;
				}
				this.BeginInvoke(delchangetxt,new object[]{"正在打开模版",20});
				//打开模版
				wm.Open(defaultpath+@"\temp\TestReport.doc");
				//插入标签
				wm.WriteIntoMarkBook("Atitle","权力运行许可平台");
				string begintime = "2011-06-01";
				string endtime = "2011-08-31";
				wm.WriteChartFromBK("BUGLevel");
//				this.BeginInvoke(delchangetxt,new object[]{"正在导入更新包数据",30});
//				DataTable table1 =PackageDao.getRePortPack();
//				this.BeginInvoke(delchangetxt,new object[]{"正在分析更新包数据",40});
//				wm.insertTableForPack(1,table1);
//				this.BeginInvoke(delchangetxt,new object[]{"正在导入测试数据",60});
//				DataTable table2 =TestUnitDao.getRePortTest(begintime,endtime);
//				this.BeginInvoke(delchangetxt,new object[]{"正在分析测试数据",80});
//				wm.insertTableForTest(2,table2);
				this.BeginInvoke(delchangetxt,new object[]{"完成",100});
				wm.SaveAs(path.ToString());
				MessageBox.Show("保存成功","提示");
			} catch (Exception e1) {
				MessageBox.Show(e1.ToString(),"提示", MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			finally
			{
				wm.Quit();
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			saveFileDialog1.Filter = "Word Document(*.doc)|*.doc";  
			saveFileDialog1.DefaultExt = "Word Document(*.doc)|*.doc";  
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				this.textBox1.Text=this.saveFileDialog1.FileName;
			}
		}
	}
}
