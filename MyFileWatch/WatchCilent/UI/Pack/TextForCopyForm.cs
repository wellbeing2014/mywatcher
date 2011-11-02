/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2011-5-10
 * Time: 14:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;

namespace WatchCilent
{
	/// <summary>
	/// Description of TextForCopyForm.
	/// </summary>
	public partial class TextForCopyForm : Form
	{
		public string path;
		public TextForCopyForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			InitializeComponent();
			this.CenterToParent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		
		
		
		void Button1Click(object sender, EventArgs e)
		{
			path = this.textBox1.Text;
			
		}
	}
	
}
