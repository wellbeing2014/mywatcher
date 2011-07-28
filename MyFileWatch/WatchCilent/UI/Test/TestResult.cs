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
using WatchCilent.dao;
using WatchCilent.pojo;
using WatchCilent.Common;
using System.Collections.Generic;
using MFCComboBox;
namespace WatchCilent.UI.Test
{
	/// <summary>
	/// Description of TestResult.
	/// </summary>
	public partial class TestResult : Form
	{
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
		public TestResult()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.comboBox1.DataSource = PersonDao.getPersonTable();;
			this.comboBox1.DisplayMember = "fullname";
			this.comboBox1.ValueMember = "id";
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.textBox1.Text =TestUnitDao.getNewUnitNO();
			
			this.comboBox2.DataSource = ModuleDao.getAllModuleTable();;
			this.comboBox2.DisplayMember = "fullname";
			this.comboBox2.ValueMember = "id";
			this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox3.DataSource = ProjectInfoDao.getAllProjectTable();;
			this.comboBox3.DisplayMember = "projectname";
			this.comboBox3.ValueMember = "id";
			this.comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox5.DataSource = CommonConst.BUGLEVEL;
			this.comboBox5.DropDownStyle= ComboBoxStyle.DropDownList;
			
			this.multiColumnFilterComboBox1.DataSource = PackageDao.getAllUnTestPack();
			
			this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("packagename",200,true));
            this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("packtime", 60, true));
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
		public TestResult(TestUnit tu)
		{
			this.tu=tu;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//this.comboBox1.DataSource = PersonDao.getPersonTable();;
			this.comboBox1.Text=tu.Adminname;
			this.comboBox1.Items.Add(tu.Adminname);
			//this.comboBox1.DisplayMember = "fullname";
			//this.comboBox1.ValueMember = "id";
			
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.textBox1.Text = TestUnitDao.getNewUnitNO();
			
			this.comboBox2.Items.Add(tu.Modulename);
			this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox3.DataSource = ProjectInfoDao.getAllProjectTable();;
			this.comboBox3.DisplayMember = "projectname";
			this.comboBox3.ValueMember = "id";
			this.comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.textBox9.Text = tu.Testtitle;
			this.comboBox5.Text = tu.Buglevel;

			MemoryStream stream = new MemoryStream(tu.Testcontent);
			this.richTextBox1.LoadFile(stream, RichTextBoxStreamType.RichText);
			
			this.CenterToParent();
			
			
			//InsertImage();
			//read();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		bool TestuiSave()
		{
			MemoryStream stream = new MemoryStream();
			richTextBox1.SaveFile(stream, RichTextBoxStreamType.RichText);
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
			tu.Packageid =0;
			tu.Projectid =0;
			tu.Testorid =0;
			
			tu.Buglevel =this.comboBox5.Text;
			tu.Packagename =this.multiColumnFilterComboBox1.Text;//his.textBox3.Text;
			if(this.multiColumnFilterComboBox1.SelectedValue!=null)
			{
				tu.Packageid = (int)this.multiColumnFilterComboBox1.SelectedValue;
			}
			tu.Projectname =this.comboBox3.Text ;
			tu.State = "未修订";
			tu.Testorname ="朱新培" ;
			tu.Testtime =DateTime.Now.ToString() ;
			if(!TestUnitDao.getNewUnitNO().Equals(this.textBox1.Text))
			{
				tu.Unitno = TestUnitDao.getNewUnitNO();
				MessageBox.Show("缺陷编号被占用，系统重新分配的编号为："+tu.Unitno,"提示");
			}
			else tu.Unitno =this.textBox1.Text;
			tu.Testtitle =this.textBox9.Text ;
			return true;
		}
		
		void Button1Click(object sender, System.EventArgs e)
		{
			if(TestuiSave())
			{
				if(AccessDBUtil.insert(tu))
				{
					if(unitDOCpath==null)
					{
						unitDOCpath = this.defaultpath;
					}
					this.richTextBox1.SaveFile(temppath+@"\"+tu.Unitno+".doc");
					CreateTestUnit(defaultpath+@"\temp\TestUnit.doc",temppath+@"\"+tu.Unitno+".doc",unitDOCpath+@"\"+tu.Unitno+".doc");
					if(unitHTMLpath!=null)
					{
						var fullHtmlPath =unitHTMLpath+@"\"+tu.Unitno+".html";
						WordDocumentMerger.WordToHtmlFile(unitDOCpath+@"\"+tu.Unitno+".doc",fullHtmlPath);
					}
					try {
						if(this.checkBox1.Checked)
						{
							string content ="您好："+tu.Adminname+"!\n  您提交测试组测试的《"+tu.Packagename+
								"》有一项内容为『"+tu.Testtitle+"』的缺陷,被列为『"+tu.Buglevel+"』等级。\n请访问:"+HtmlUrl+@"\"+tu.Unitno+".html"+"查看详细并确认。";
							PersonInfo person =PersonDao.getPersonInfoByid(tu.Adminid);
							string[] iplist = person.Ip.Split(';');
							foreach(string ip in iplist)
							{
								Communication.TCPManage.SendMessage(WisofServiceHost,content+"##"+ip);
							}
						}
					} catch (Exception) {
						
						MessageBox.Show("通知主管失败！");
					}
					
					
					MessageBox.Show("保存成功");
				}
				
			}
		}
		
		void CreateTestUnit(string unitdocTpl,string tempdocpath,string unitdocpath)
		{
			WordDocumentMerger wm = new WordDocumentMerger();
			try {
				
				wm.Open(unitdocTpl);
				wm.WriteIntoMarkBook("AdminName",tu.Adminname);
				wm.WriteIntoMarkBook("BUGLevel",tu.Buglevel);
				wm.WriteIntoMarkBook("UnitNO",tu.Unitno);
				wm.WriteIntoMarkBook("ModuleName",tu.Modulename);
				wm.WriteIntoMarkBook("NO",tu.Unitno);
				wm.WriteIntoMarkBook("PackageName",tu.Packagename);
				wm.WriteIntoMarkBook("ProjectName",tu.Projectname);
				wm.WriteIntoMarkBook("TestTime",tu.Testtime);
				wm.WriteIntoMarkBook("Title",tu.Testtitle);
				wm.InsertMerge(new string[]{tempdocpath},"Content");
				wm.Save(unitdocpath);
			} catch (Exception) {
				
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
				try {
					string content ="您好："+tu.Adminname+"!\n  您提交测试组测试的《"+tu.Packagename+
						"》有一项内容为『"+tu.Testtitle+"』的缺陷,被列为『"+tu.Buglevel+"』等级。\n请访问:"+HtmlUrl+@"\"+tu.Unitno+".html"+"查看详细并确认。";
					PersonInfo person =PersonDao.getPersonInfoByid(tu.Adminid);
					string[] iplist = person.Ip.Split(';');
					foreach(string ip in iplist)
					{
						Communication.TCPManage.SendMessage(WisofServiceHost,content+"##"+ip);
					}
				} catch (Exception) {
					
					MessageBox.Show("通知主管失败！");
				}
			}
		}
		
		/// <summary>
		/// 选择更新包时自动绑定其他信息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Package_SelectedValueChanged(object sender, EventArgs e)
		{
			MessageBox.Show("aaaa");
		}
		
	}
}
