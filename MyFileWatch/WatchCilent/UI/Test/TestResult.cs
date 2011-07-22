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
			this.textBox1.Text = AccessDBUtil.CalcBUGNO();
			
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
			this.textBox1.Text = AccessDBUtil.CalcBUGNO();
			
			this.comboBox2.Items.Add(tu.Modulename);
			this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox3.DataSource = ProjectInfoDao.getAllProjectTable();;
			this.comboBox3.DisplayMember = "projectname";
			this.comboBox3.ValueMember = "id";
			this.comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.textBox3.Text = tu.Packagename;
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
			tu.Packagename =this.textBox3.Text;
			tu.Projectname ="fadsadsfads" ;
			tu.State = "未修订";
			tu.Testorname ="朱新培" ;
			tu.Testtime =DateTime.Now.ToString() ;
			tu.Testtitle =this.textBox9.Text ;
			tu.Unitno =AccessDBUtil.CalcBUGNO();
			return true;
		}
		public   void   InsertImage() 
		{ 
//			bool   b   =   richTextBox1.ReadOnly; 
//			Image   img   =   Image.FromFile( "C:/a.bmp "); 
//			if (img != null) {
//				Clipboard.SetDataObject(img); 
//				richTextBox1.ReadOnly   =   false; 
//				richTextBox1.Paste(DataFormats.GetFormat(DataFormats.Bitmap)); 
//				richTextBox1.ReadOnly   =   b; 
//			}
			
		}
		
		void Button1Click(object sender, System.EventArgs e)
		{
			if(TestuiSave())
			{
				if(AccessDBUtil.insert(tu))
				{
					try {
						if(unitDOCpath==null)
						{
							unitDOCpath = this.defaultpath;
						}
						this.richTextBox1.SaveFile(unitDOCpath+@"\"+tu.Unitno+".doc");
						if(unitHTMLpath!=null)
						{
							var fullHtmlPath =unitHTMLpath+@"\"+tu.Unitno+".html";
							WordDocumentMerger.WordToHtmlFile(unitDOCpath+@"\"+tu.Unitno+".doc",fullHtmlPath);
							if(this.checkBox1.Checked)
							{
								string content ="您好："+tu.Adminname+"!\n  您提交测试组测试的《"+tu.Packagename+
									"》有一项内容为『"+tu.Testtitle+"』的缺陷,被列为『"+tu.Buglevel+"』等级。\n请访问:"+HtmlUrl+"查看详细并确认。";
								PersonInfo person =PersonDao.getPersonInfoByid(tu.Adminid);
								string[] iplist = person.Ip.Split(';');
								foreach(string ip in iplist)
								{
									Communication.TCPManage.SendMessage(WisofServiceHost,content+"##"+ip);
								}
							}
						}
					} catch (Exception) {
						
						MessageBox.Show("通知主管失败！");
					}
					
					
					MessageBox.Show("保存成功");
				}
				
			}
		}
		void read()
		{
			TestUnit tu=TestUnitDao.gettestUnitById(3);
			if(tu.Testcontent!=null)
			{
				MemoryStream stream = new MemoryStream(tu.Testcontent);
				this.richTextBox1.LoadFile(stream, RichTextBoxStreamType.RichText);
			}
			
		}
		
		
	}
}
