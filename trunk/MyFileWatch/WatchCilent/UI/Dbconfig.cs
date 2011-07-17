/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/5/26
 * Time: 21:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WatchCilent
{
	/// <summary>
	/// Description of Dbconfig.
	/// </summary>
	public partial class Dbconfig : Form
	{
		public Dbconfig()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.CenterToScreen();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
			Microsoft.Win32.RegistryKey reg = key.CreateSubKey("software\\WisoftWatchClient");  
   			reg.SetValue("dbpath", this.textBox1.Text); 
		}
	}
}
