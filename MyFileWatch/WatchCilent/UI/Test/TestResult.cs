/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/6/19
 * Time: 13:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;
using System.Collections.Generic;
using MFCComboBox;
namespace WatchCilent.UI.Test
{
	/// <summary>
	/// Description of TestResult.
	/// </summary>
	public partial class TestResult : Form
	{
		
		private TestListParameter tp;
		
		public TestListParameter Tp {
			get { return tp; }
			set { tp = value; }
		}
		private TestUnit tu = new TestUnit();
		//缺陷列表的HTML路径
		string unitHTMLpath = FunctionUtils.AutoCreateFolder(System.Configuration.ConfigurationManager.AppSettings["UnitHtmlPath"]);
		//浏览缺陷地址
		string HtmlUrl = System.Configuration.ConfigurationManager.AppSettings["HtmlUrl"];
		//监控系统主机IP
		string WisofServiceHost = System.Configuration.ConfigurationManager.AppSettings["WisofServiceHost"];
		//缺陷列表的DOC路径
		string unitDOCpath = FunctionUtils.AutoCreateFolder(System.Configuration.ConfigurationManager.AppSettings["UnitDocPath"]);
		//默认路径
		string defaultpath =System.Environment.CurrentDirectory;
		//临时文件路径
		string temppath =System.IO.Path.GetTempPath();
		
		DataTable Source_Person = PersonDao.getPersonTable();
		DataTable Source_Module = ModuleDao.getAllModuleTable();
		DataTable Source_Project = ProjectInfoDao.getAllProjectTable();
		bool isSaved = false;
		
		/// <summary>
		/// 新建窗口
		/// </summary>
		public TestResult()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.Text ="缺陷新建";
			this.button5.Dispose();
			this.button6.Dispose();
			this.checkBox2.Dispose();
			
			this.comboBox1.DataSource = Source_Person;
			this.comboBox1.DisplayMember = "fullname";
			this.comboBox1.ValueMember = "id";
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.textBox1.Text =TestUnitDao.getNewUnitNO();
			
			this.comboBox2.DataSource = Source_Module;
			this.comboBox2.DisplayMember = "fullname";
			this.comboBox2.ValueMember = "id";
			this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox3.DataSource =Source_Project;
			this.comboBox3.DisplayMember = "projectname";
			this.comboBox3.ValueMember = "id";
			this.comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox5.DataSource = CommonConst.BUGLEVEL;
			this.comboBox5.DropDownStyle= ComboBoxStyle.DropDownList;
			
			this.comboBox6.DataSource = CommonConst.BUGTYPE;
			this.comboBox6.DropDownStyle= ComboBoxStyle.DropDownList;
			
			this.multiColumnFilterComboBox1.DataSource = PackageDao.getAllUnTestPack();
			
			this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("packagename",200,true));
            this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("packtime", 60, true));
            this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("code", 60, true));
            this.multiColumnFilterComboBox1.DisplayMember = "packagename";
            this.multiColumnFilterComboBox1.ValueMember = "id";
            this.multiColumnFilterComboBox1.Validated+= new EventHandler(Package_SelectedValueChanged);
				
			this.CenterToParent();
			//InsertImage();
			//read();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		/// <summary>
		/// 编辑窗口
		/// </summary>
		/// <param name="tuint"></param>
		public TestResult(TestUnit tuint,TestListParameter tp)
		{
			this.tu=tuint;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			if(tp==null)
			{
				this.Text ="缺陷编辑";
				this.button3.Enabled = false;
				this.button5.Dispose();
				this.button6.Dispose();
				this.checkBox2.Dispose();
			}
			else
			{
				this.Text ="缺陷浏览";
				this.Tp = tp;
				this.button5.Location = this.button1.Location;
				this.button6.Location = this.button2.Location;
				this.checkBox2.Location = this.checkBox1.Location;
				this.button1.Dispose();
				this.button2.Dispose();
				this.button3.Dispose();
				this.button4.Dispose();
				this.checkBox1.Dispose();
				int sumtu = TestUnitDao.QueryTestUnitCount(tp.Moduleid,tp.Manageid,tp.Level,tp.State,
				                                    tp.Begintime,tp.Endtime);
				if(tp.Startindex-1 < 0 )
					this.button5.Enabled = false;
				else 
					this.button5.Enabled = true;
				
				if(tp.Startindex+2 > sumtu )
					this.button6.Enabled = false;
				else 
					this.button6.Enabled = true;
			}
		
			TestUnitBingDS();
			TestUnitBingData();
			
			this.CenterToParent();
			
			
			
		}
		
		/// <summary>
		/// 绑定数据源到控件
		/// </summary>
		void TestUnitBingDS()
		{
			this.comboBox1.DataSource = this.Source_Person;
			this.comboBox1.DisplayMember = "fullname";
			this.comboBox1.ValueMember = "id";
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox2.DataSource = this.Source_Module;
			this.comboBox2.DisplayMember = "fullname";
			this.comboBox2.ValueMember = "id";
			this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			//绑定项目
			this.comboBox3.DataSource = this.Source_Project;
			this.comboBox3.DisplayMember = "projectname";
			this.comboBox3.ValueMember = "id";
			this.comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			//绑定BUG等级
			if(!this.comboBox5.IsDisposed)
			{
				this.comboBox5.Items.AddRange(CommonConst.BUGLEVEL);
			}
			
			//绑定BUG类型
			if(!this.comboBox6.IsDisposed)
			{
				this.comboBox6.Items.AddRange(CommonConst.BUGTYPE);
			}
			
		}
		
		/// <summary>
		/// 绑定数据到控件
		/// </summary>
		void TestUnitBingData()
		{
			this.multiColumnFilterComboBox1.Items.Clear();
			this.multiColumnFilterComboBox1.Items.Add(this.tu);
			this.multiColumnFilterComboBox1.DisplayMember = "Packagename";
            this.multiColumnFilterComboBox1.ValueMember = "Packageid";
			this.multiColumnFilterComboBox1.SelectedIndex = 0;
			this.multiColumnFilterComboBox1.Enabled = false;
			
			//绑定主管
			foreach (DataRow element in this.Source_Person.Rows) {
				string tempid = element["id"].ToString();
				if(Int32.Parse(tempid)== tu.Adminid)
				{
					int sel = this.Source_Person.Rows.IndexOf(element);
					this.comboBox1.SelectedIndex = sel;
					break;
				}
			}
			
			//绑定模块（平台）
			
			foreach (DataRow element in this.Source_Module.Rows) {
				string tempid = element["id"].ToString();
				if(Int32.Parse(tempid)== tu.Moduleid)
				{
					int sel = this.Source_Module.Rows.IndexOf(element);
					this.comboBox2.SelectedIndex = sel;
					break;
				}
			}
			
			//绑定项目
			foreach (DataRow element in this.Source_Project.Rows) {
				string tempid = element["id"].ToString();
				if(Int32.Parse(tempid)== tu.Projectid)
				{
					int sel = this.Source_Project.Rows.IndexOf(element);
					this.comboBox3.SelectedIndex = sel;
					break;
				}
			}
			//绑定BUG等级
			foreach (var element in CommonConst.BUGLEVEL) {
				if(element.Equals(tu.Buglevel))
				{
					this.comboBox5.SelectedItem = element;
					break;
				}
			}
			//绑定BUG类型
			foreach (var element in CommonConst.BUGTYPE) {
				if(element.Equals(tu.Bugtype))
				{
					this.comboBox6.SelectedItem = element;
					break;
				}
			}
			
			this.textBox9.Text = tu.Testtitle;
			this.textBox1.Text = tu.Unitno;
			this.textBox1.ReadOnly = true;

			MemoryStream stream = new MemoryStream(tu.Testcontent);
			this.richTextBox1.LoadFile(stream, RichTextBoxStreamType.RichText);
		}
		
		bool TestuiSave()
		{
			MemoryStream stream = new MemoryStream();
			richTextBox1.SaveFile(stream, RichTextBoxStreamType.RichText);
			//richTextBox1.SaveFile(@"D:\tttt.rtf");
			tu.Testcontent=stream.ToArray();
			if(this.comboBox1.SelectedItem!=null&&this.comboBox1.SelectedIndex!=0)
			{
				tu.Adminname = this.comboBox1.Text;
				tu.Adminid = (int)this.comboBox1.SelectedValue;
			}
			else
			{
				MessageBox.Show("请选择责任人","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return false ;
			}
			if(this.comboBox2.SelectedItem!=null&&this.comboBox2.SelectedIndex!=0)
			{
				tu.Modulename= this.comboBox2.Text;
				tu.Moduleid = (int)this.comboBox2.SelectedValue;
			}
			else
			{
				MessageBox.Show("请选择责任人","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return false;
			}
					
			tu.Buglevel =this.comboBox5.Text;
			tu.Bugtype = this.comboBox6.Text;
			tu.Packagename =this.multiColumnFilterComboBox1.Text;//his.textBox3.Text;
			if(this.multiColumnFilterComboBox1.SelectedValue!=null)
			{
				tu.Packageid = (int)this.multiColumnFilterComboBox1.SelectedValue;
			}
			tu.Projectname =this.comboBox3.Text ;
			tu.State = Enum.GetName(typeof(CommonConst.TestState),CommonConst.TestState.已确认);
			//Enum.GetNames(typeof(CommonConst.TestState)))
			tu.Testorname = comboBox4.Text;
			tu.Testtime =DateTime.Now.ToString() ;
			if(tu.Id==0&&!isSaved)
			{
				if(!TestUnitDao.getNewUnitNO().Equals(this.textBox1.Text))
				{
					tu.Unitno = TestUnitDao.getNewUnitNO();
					MessageBox.Show("缺陷编号被占用，系统重新分配的编号为："+tu.Unitno,"提示");
				}
				else tu.Unitno =this.textBox1.Text;
					
			}
			else tu.Unitno =this.textBox1.Text;
			tu.Testtitle =this.textBox9.Text ;
			return true;
		}
		/// <summary>
		/// 给主管发送飞秋消息。
		/// </summary>
		void SendMessageToManager()
		{
			try {
				string content ="您好："+tu.Adminname+"!\n  您提交测试组测试的《"+tu.Packagename+
					"》有一项内容为『"+tu.Testtitle+"』、类型为『"+tu.Bugtype+"』的缺陷,被列为『"+tu.Buglevel+"』等级。\n请访问:"+HtmlUrl+tu.Unitno+".html"+"查看详细并确认。";
				PersonInfo person =PersonDao.getPersonInfoByid(tu.Adminid);
				string[] iplist = person.Ip.Split(';');
				foreach(string ip in iplist)
				{
					Communication.TCPManage.SendMessage(WisofServiceHost,content+"##"+ip);
				}
			} catch (Exception e) {
				MessageBox.Show("通知主管失败！:"+e.ToString());
			}
		}
		void Button1Click(object sender, System.EventArgs e)
		{
			if(TestuiSave())
			{
				bool isSave = false;
				
				if(tu.Id!=0)
					isSave = SqlDBUtil.update(tu);
				else
				{
					tu.Id = SqlDBUtil.insertReturnID(tu);
					isSave = true;
				}
				if(isSave)
				{
					
					//创建文档
					if(unitDOCpath==null)
					{
						unitDOCpath = this.defaultpath;
					}
					this.richTextBox1.SaveFile(temppath+@"\"+tu.Unitno+".doc");
					CreateTestUnit(defaultpath+@"\temp\TestUnit.doc",temppath+@"\"+tu.Unitno+".doc",unitDOCpath+@"\"+tu.Unitno+".doc");
					//创建HTML
					if(unitHTMLpath!=null)
					{
						var fullHtmlPath =unitHTMLpath+@"\"+tu.Unitno+".html";
						WordDocumentMerger.WordToHtmlFile(unitDOCpath+@"\"+tu.Unitno+".doc",fullHtmlPath);
					}
					//勾选自动发送
					if(this.checkBox1.Checked)
					{
						SendMessageToManager();
					}
				
					MessageBox.Show("保存成功！");
				}
				else
					MessageBox.Show("保存失败！");	
				
			}
		}
		
		void CreateTestUnit(string unitdocTpl,string tempdocpath,string unitdocpath)
		{
			PackageInfo pi = PackageDao.getPackageInfoByID(tu.Packageid.ToString());
			WordDocumentMerger wm = new WordDocumentMerger();
			try {
				
				wm.Open(unitdocTpl);
				wm.WriteIntoMarkBook("AdminName",tu.Adminname);
				wm.WriteIntoMarkBook("BUGLevel",tu.Buglevel);
				wm.WriteIntoMarkBook("BUGType",tu.Bugtype);
				wm.WriteIntoMarkBook("UnitNO",tu.Unitno);
				wm.WriteIntoMarkBook("ModuleName",tu.Modulename);
				wm.WriteIntoMarkBook("NO",tu.Unitno);
				wm.WriteIntoMarkBook("PackageName",pi.Packagepath);
				wm.WriteIntoMarkBook("ProjectName",tu.Projectname);
				wm.WriteIntoMarkBook("TestTime",tu.Testtime);
				wm.WriteIntoMarkBook("Title",tu.Testtitle);
				wm.InsertMerge(new string[]{tempdocpath},"Content");
				wm.Save(unitdocpath);
			} catch (Exception e) {
				
				MessageBox.Show("生成测试单元文档失败！");
			}
			finally
			{
				wm.Quit();
			}
		}
		
		
		/// <summary>
		/// 关闭按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		
		void Button2Click(object sender, EventArgs e)
		{
			this.Close();
			this.Dispose();
		}
		
		/// <summary>
		/// 继续新建按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button3Click(object sender, EventArgs e)
		{
			//标题置空
			this.textBox9.Text ="";
			//详细信息置空
			this.richTextBox1.Clear();
			//重新设置BUG编号
			this.textBox1.Text = TestUnitDao.getNewUnitNO();
			this.tu.Id = 0;
		}
		
		/// <summary>
		/// 通知主管按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button4Click(object sender, EventArgs e)
		{
			if(TestuiSave())
			{
				SendMessageToManager();
			}
		}
		
		/// <summary>
		/// 选择更新包时自动绑定其他信息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Package_SelectedValueChanged(object sender, EventArgs e)
		{
			if(this.multiColumnFilterComboBox1.SelectedItem==null)
			{
				return ;
			}
			DataRowView package =(DataRowView) this.multiColumnFilterComboBox1.SelectedItem;
			string  moduleid =  package["realmoduleid"].ToString();
			string  managerid =  package["managerid"].ToString();
			//绑定主管
			foreach (DataRow element in this.Source_Person.Rows) {
				string tempid = element["id"].ToString();
				if(tempid.Equals(managerid))
				{
					int sel = this.Source_Person.Rows.IndexOf(element);
					this.comboBox1.SelectedIndex = sel;
					break;
				}
			}
			
			//绑定模块（平台）
			foreach (DataRow element in this.Source_Module.Rows) {
				string tempid = element["id"].ToString();
				if(tempid.Equals(moduleid))
				{
					int sel = this.Source_Module.Rows.IndexOf(element);
					this.comboBox2.SelectedIndex = sel;
					break;
				}
			}
		}
		//浏览模式 上一个 按钮
		void Button5Click(object sender, EventArgs e)
		{
			if(tp!=null)
			{
				int sumtu = TestUnitDao.QueryTestUnitCount(tp.Moduleid,tp.Manageid,tp.Level,tp.State,
				                                    tp.Begintime,tp.Endtime);
				List<TestUnit> tulist = TestUnitDao.QueryTestUnit(tp.Moduleid,tp.Manageid,tp.Level,tp.State,
				                                    tp.Begintime,tp.Endtime,tp.Startindex-1,1);
				if(tulist.Count>0)
				{
					this.tu = TestUnitDao.gettestUnitById(tulist[0].Id);
					TestUnitBingData();
					tp.Startindex = tp.Startindex-1;
					if(tp.Startindex-1 < 0 )
						this.button5.Enabled = false;
					else 
						this.button5.Enabled = true;
					
					if(tp.Startindex+2 > sumtu )
						this.button6.Enabled = false;
					else 
						this.button6.Enabled = true;
				}
				else
				{
					MessageBox.Show("没有找到上一条数据","出错");
				}
			}
		}
		//浏览模式 下一个 按钮
		void Button6Click(object sender, EventArgs e)
		{
			if(tp!=null)
			{
				int sumtu = TestUnitDao.QueryTestUnitCount(tp.Moduleid,tp.Manageid,tp.Level,tp.State,
				                                    tp.Begintime,tp.Endtime);
				List<TestUnit> tulist = TestUnitDao.QueryTestUnit(tp.Moduleid,tp.Manageid,tp.Level,tp.State,
				                                    tp.Begintime,tp.Endtime,tp.Startindex+1,1);
				if(tulist.Count>0)
				{
					this.tu = TestUnitDao.gettestUnitById(tulist[0].Id);
					TestUnitBingData();
					tp.Startindex = tp.Startindex+1;
					if(tp.Startindex-1 < 0 )
						this.button5.Enabled = false;
					else 
						this.button5.Enabled = true;
					
					if(tp.Startindex+2 > sumtu )
						this.button6.Enabled = false;
					else 
						this.button6.Enabled = true;
					
				}
				else
				{
					MessageBox.Show("没有找到下一条数据","出错");
				}
			}
		}
		
		
		//关注
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			if(this.checkBox2.Checked)
			{
				Theme.ChoseThemeDialog cd = new WatchCilent.UI.Theme.ChoseThemeDialog();
				cd.StartPosition = FormStartPosition.CenterParent;
				DialogResult a = cd.ShowDialog();
				if (DialogResult.OK == a) {
					if(cd.selthem==null)
					{
						MessageBox.Show("选择关注主题出错，请重新选择","提示");
						return;
					}
					Testunittheme tt = new Testunittheme();
					tt.Themeid = cd.selthem.Id;
					tt.Unitid = this.tu.Id;
					SqlDBUtil.insert(tt);
				}
			}
			else
			{
				DialogResult a =MessageBox.Show("是否取消关注主题","提示",MessageBoxButtons.OKCancel);
				if(DialogResult.OK ==a)
				{
					this.checkBox2.Text="未关注";
				}
				else
					this.checkBox2.Checked = true;
			}
		}
	}
	public class TestListParameter
	{
		private string moduleid;
		
		public string Moduleid {
			get { return moduleid; }
			set { moduleid = value; }
		}
		private string manageid;
		
		public string Manageid {
			get { return manageid; }
			set { manageid = value; }
		}
		private string level;
		
		public string Level {
			get { return level; }
			set { level = value; }
		}
		private string state;
		
		public string State {
			get { return state; }
			set { state = value; }
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
		private int startindex;
		
		public int Startindex {
			get { return startindex; }
			set { startindex = value; }
		}
		private int pagesize;
		
		public int Pagesize {
			get { return pagesize; }
			set { pagesize = value; }
		}
	}
}
