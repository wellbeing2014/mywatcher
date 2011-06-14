/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/6/11
 * Time: 22:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WatchCilent
{
	/// <summary>
	/// Description of EditBugReport.
	/// </summary>
	public partial class EditBugReport : Form
	{
		public EditBugReport()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			EditMode();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void EditMode()
		{
		    if (this.webBrowser1.Document != null)
		    {
		        MSHTML.IHTMLDocument2 doc = this.webBrowser1.Document.DomDocument as MSHTML.IHTMLDocument2;
		        if (doc != null)
		        {
		            doc.designMode = "on";
		        }
		    }
		}

	}
}
