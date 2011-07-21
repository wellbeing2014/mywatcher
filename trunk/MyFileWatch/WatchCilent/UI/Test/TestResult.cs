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
			bool   b   =   richTextBox1.ReadOnly; 
			Image   img   =   Image.FromFile( "C:/a.bmp "); 
			if (img != null) {
				Clipboard.SetDataObject(img); 
				richTextBox1.ReadOnly   =   false; 
				richTextBox1.Paste(DataFormats.GetFormat(DataFormats.Bitmap)); 
				richTextBox1.ReadOnly   =   b; 
			}
			
		}
		
		void Button1Click(object sender, System.EventArgs e)
		{
			if(TestuiSave())
			{
				if(this.checkBox1.Checked)
				{
					Communication.TCPManage.SendMessage("127.0.0.1","afsjfasjfa##127.0.0.1");
				}
				if(AccessDBUtil.insert(tu))
				{
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
