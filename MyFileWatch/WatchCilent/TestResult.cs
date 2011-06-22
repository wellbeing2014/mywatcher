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

using WatchCilent.dao;
using WatchCilent.pojo;

namespace WatchCilent
{
	/// <summary>
	/// Description of TestResult.
	/// </summary>
	public partial class TestResult : Form
	{
		public TestResult()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//InsertImage();
			read();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
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
			MemoryStream stream = new MemoryStream();
			richTextBox1.SaveFile(stream, RichTextBoxStreamType.RichText);
			TestUnit tu =new TestUnit();
			tu.Testcontent=stream.ToArray();
			if(TestUnitDao.insert(tu))
			{
				MessageBox.Show("保存成功");
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
