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
using WatchCore.Common;
using System.Configuration;
using WatchCore.dao;
using WatchCore.pojo;

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
			
			this.textBox1.Text = GlobalParams.SqlDB;
			this.textBox2.Text = GlobalParams.UnitDOCpath;
			this.textBox3.Text = GlobalParams.UnitHTMLpath;
			this.textBox4.Text = GlobalParams.HtmlUrl;
			this.textBox5.Text = GlobalParams.WisofServiceHost;
			this.textBox1.Enabled = false;
			this.textBox2.Enabled = false;
			this.textBox3.Enabled = false;
			this.textBox4.Enabled = false;
			this.textBox5.Enabled = false;
			this.textBox6.Text = ConfigurationManager.AppSettings["Username"];
			this.textBox7.Text = ConfigurationManager.AppSettings["Password"];
//			if(string.IsNullOrEmpty(ConfigurationManager.AppSettings["Username"]))
//			{
//				MessageBox.Show("您是第一次登录，请设置用户名密码。\n若用户名不存在请联系管理员","提示");
//				this.TabIndex=this.textBox6.TabIndex;
//			}
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		
		void Button1Click(object sender, EventArgs e)
		{
			string username = this.textBox6.Text;
			string password = this.textBox7.Text;
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
			Microsoft.Win32.RegistryKey reg = key.CreateSubKey("software\\WisoftWatchClient");  
			reg.SetValue("Version", this.textBox1.Text);
			reg.SetValue("SqlDB", this.textBox1.Text);
   			reg.SetValue("UnitDocPath", this.textBox2.Text); 
   			reg.SetValue("UnitHtmlPath", this.textBox3.Text); 
   			reg.SetValue("HtmlUrl", this.textBox4.Text); 
   			reg.SetValue("WisofServiceHost", this.textBox5.Text);
   			
   			if(PersonDao.getAllPersonInfo(username).Count==1)
   			{
   				PersonInfo ps = PersonDao.getAllPersonInfo(username)[0];
   				ps.Fullname = username;
   				password = MD5Common.GetMd5Hash(password);
   				ps.Password = password;
   				GlobalParams.User = ps;
				reg.SetValue("Username", username);
				reg.SetValue("Password", password);	
				reg.SetValue("UserId", ps.Id);				
	   			FunctionUtils.UpdateAppConfig("Username",username);
	   			FunctionUtils.UpdateAppConfig("Password",password);
	   			FunctionUtils.UpdateAppConfig("UserId",ps.Id.ToString());
	   			SqlDBUtil.update(ps);
   			}
   			else
   			{
   				MessageBox.Show("数据库没有该用户!,请联系管理员");
   			}
		}
	}
}
