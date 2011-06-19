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
			webBrowser1.DocumentText = string.Empty;
			webBrowser1.Document.ExecCommand("EditMode", false, null);
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
	}
}
