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
			string begin=this.dateTimePicker1.Value.ToShortDateString()+" 00:00:00";
			string end =this.dateTimePicker2.Value.ToShortDateString()+" 23:59:59";
			Thread th = new Thread(new ParameterizedThreadStart(ThreadFunc));
			ReportPara  rp=  new ReportPara();
			rp.Path = this.textBox1.Text;
			rp.Begintime = begin;
			rp.Endtime = end;
			th.Start(rp);
			
		}
		private delegate void del_do_changetxt(string text,int pgbvalue);
		
		void do_changetxt(string text,int pgbvalue)
		{
			this.label3.Text = text;
			this.progressBar1.Value=pgbvalue;
		}
		
		void ThreadFunc(object obj)
		{
			ReportPara rp = (ReportPara)obj;
			string begin = rp.Begintime;
			string end = rp.Endtime;
			string path = rp.Path;
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
				//查询数据库
				this.BeginInvoke(delchangetxt,new object[]{"正在查询数据库",30});
				DataTable table1 =PackageDao.getRePortPack(begin,end);
				DataTable table2 =TestUnitDao.getRePortTest(begin,end);
				DataTable packdt = PackageDao.getRePortPackNUM(begin,end);
				DataTable testdt =TestUnitDao.getRePortBugLevel(begin,end);
				//插入标签
				this.BeginInvoke(delchangetxt,new object[]{"正在插入数据",60});
				wm.WriteIntoMarkBook("测试报告名称","权力运行许可平台");
				wm.WriteIntoMarkBook("测试时间起",begin);
				wm.WriteIntoMarkBook("测试时间至",end);
				wm.WriteIntoMarkBook("更新包个数",table1.Rows.Count.ToString());
				wm.insertTable("成功率表格",packdt);
				wm.WriteIntoMarkBook("BUG个数",table2.Rows.Count.ToString());
				DataTable testAllBug = TestUnitDao.getRePortBugLevelAll(begin,end);
				this.BeginInvoke(delchangetxt,new object[]{"正在绘制图表和表格",80});
				wm.WriteChartFromBK("BUG等级饼图",testAllBug);
				wm.insertTable("BUG等级表格",testdt);
				wm.insertTableForPack("测试对象表格",table1);
				wm.insertTableForTest("测试缺陷表格",table2);
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
	class ReportPara
	{
		private string path;
		
		public string Path {
			get { return path; }
			set { path = value; }
		}
		private string begintime;
		
		public string Begintime {
			get { return begintime; }
			set { begintime = value; }
		}
		private string endtime;
		
		public string Endtime {
			get { return endtime; }
			set { endtime = value; }
		}
	}
}
