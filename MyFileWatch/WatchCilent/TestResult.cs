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
using System.Windows.Forms;

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
			InsertImage();
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
		
	}
}
