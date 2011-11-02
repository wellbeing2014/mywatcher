/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/5/20
 * Time: 22:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;

namespace WatchCilent
{
	/// <summary>
	/// Description of ConfigForm.
	/// </summary>
	public partial class ConfigForm : Form
	{
		public ConfigForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			getAllModuleInfo();
			getAllProjectInfo();
			getAllPersonInfo();
			getAllModuleProject();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	
/**************************版本信息开始*******************************************************************/
		private void getAllModuleInfo()
		{
			datasource_person = PersonDao.getAllPersonInfo();
			PersonInfo person = new PersonInfo();
			person.Fullname = "选择责任人";
			person.Id = 0;
			datasource_person.Insert(0,person);
			
			comboBox1.DataSource = datasource_person;
			comboBox1.DisplayMember ="Fullname";
			comboBox1.ValueMember = "Id";
			this.textBox1.Text =null;
			this.textBox2.Text=null;
			modulelist=ModuleDao.getAllModuleInfo();
			this.listView1.Items.Clear();
			foreach(ModuleInfo module in modulelist)
			{
				ListViewBingForModule(module);
			}
			
		}
		
		private void ListViewBingForModule(ModuleInfo module)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text=module.Fullname;
			lvi.SubItems.Add(module.Code);
			lvi.SubItems.Add(module.Managername);
			lvi.SubItems.Add(module.Managerid.ToString());
			lvi.SubItems.Add(module.Lastversion);
			lvi.SubItems.Add(module.Createtime);
			lvi.SubItems.Add(module.Id.ToString());
			//lvi.SubItems.Add();
			this.listView1.Items.Add(lvi);
		}
		
		//新增
		void Button1Click(object sender, EventArgs e)
		{
			if(ValidateModuleEdit())
			{
				ModuleInfo newmodule= new ModuleInfo();
				newmodule.Code=textBox2.Text;
				newmodule.Createtime =DateTime.Now.ToLocalTime().ToString();
				newmodule.Fullname = textBox1.Text;
				newmodule.Managerid = Int32.Parse(comboBox1.SelectedValue.ToString());
				newmodule.Managername=comboBox1.Text;
				newmodule.Id=0;
				bool isinsert=SqlDBUtil.insert(newmodule);
				if(isinsert) getAllModuleInfo();
			}
		}
		//选择列表记录
		void listView1_Click(object sender, EventArgs e)
		{
			ModuleInfo selectmodule=ListView1_Select(this.listView1.SelectedItems[0]);
			this.textBox1.Text = selectmodule.Fullname;
			this.textBox2.Text = selectmodule.Code;
			if(selectmodule.Managerid>0)
			{
				int index_manager;
				for (index_manager=0;index_manager<datasource_person.Count ;index_manager++ ) 
				{
					if(selectmodule.Managerid==datasource_person[index_manager].Id)
					{
						this.comboBox1.SelectedItem=this.comboBox1.Items[index_manager];
						break;
					}
				}
			}
		}
		//选择转换
		private ModuleInfo  ListView1_Select(ListViewItem lvi)
		{
			ModuleInfo module = new ModuleInfo();
			module.Fullname = lvi.SubItems[0].Text;
			module.Code = lvi.SubItems[1].Text;
			module.Managername = lvi.SubItems[2].Text;
			module.Managerid = Int32.Parse(lvi.SubItems[3].Text);
			module.Lastversion = lvi.SubItems[4].Text;
			module.Createtime = lvi.SubItems[5].Text;
			module.Id = Int32.Parse(lvi.SubItems[6].Text);
			return module;
		}
	
		//修改
		void Button2Click(object sender, EventArgs e)
		{
			if(this.listView1.SelectedItems.Count!=1)
			{
				MessageBox.Show("请选择一条记录","提示");
				return;
			}
			if(ValidateModuleEdit())
			{
				ModuleInfo module=ListView1_Select(this.listView1.SelectedItems[0]);
				module.Fullname = this.textBox1.Text;
				module.Code = this.textBox2.Text;
				module.Managerid=Int32.Parse(this.comboBox1.SelectedValue.ToString());
				module.Managername=this.comboBox1.Text;
				SqlDBUtil.update(module);
				this.getAllModuleInfo();
			}
		}
		//验证
		bool ValidateModuleEdit()
		{
			bool isvalidated = true;
			string message ="";
			if(textBox1.Text.Trim()=="")
			{
				isvalidated = false;
				message += "名称,";
			}
			if(textBox2.Text.Trim()=="")
			{
				isvalidated = false;
				message += "代码,";
			}
			if(comboBox1.SelectedValue.ToString()=="0")
			{
				isvalidated = false;
				message += "责任人,";
			}
			if(!isvalidated)
			{
				message=message.Substring(0,message.Length-1);
				MessageBox.Show("请在编辑处输入"+message,"提示");
			}
			return isvalidated;
		}
		
		//删除
		void Button3Click(object sender, EventArgs e)
		{
			if(this.listView1.SelectedItems.Count!=1)
			{
				MessageBox.Show("请选择一条记录","提示");
				return;
			}
			ModuleInfo module=ListView1_Select(this.listView1.SelectedItems[0]);
			SqlDBUtil.delete(module);
			this.getAllModuleInfo();
		}
/**************************版本信息结束*******************************************************************/		
/**************************项目信息开始*******************************************************************/	
		private void getAllProjectInfo()
		{
			
			this.textBox4.Text =null;
			this.textBox3.Text=null;
			this.textBox7.Text=null;
			projectlist=ProjectInfoDao.getAllProjectInfo();
			this.listView2.Items.Clear();
			foreach(ProjectInfo project in projectlist)
			{
				ListViewBingForProject(project);
			}
		}
		private void ListViewBingForProject(ProjectInfo project)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text=project.Projectname;
			lvi.SubItems.Add(project.Projectpath);
			lvi.SubItems.Add(project.Url);
			lvi.SubItems.Add(project.Id.ToString());
			//lvi.SubItems.Add();
			this.listView2.Items.Add(lvi);
		}

		
		private ProjectInfo  ListView2_Select(ListViewItem lvi)
		{
			ProjectInfo project = new ProjectInfo();
			project.Projectname = lvi.SubItems[0].Text;
			project.Projectpath = lvi.SubItems[1].Text;
			project.Url = lvi.SubItems[2].Text;
			project.Id = Int32.Parse(lvi.SubItems[3].Text);
			return project;
		}
		//新增
		void Button7Click(object sender, EventArgs e)
		{
			if(ValidateProjectEdit())
			{
				ProjectInfo project= new ProjectInfo();
				project.Projectname=textBox4.Text;
				project.Projectpath=textBox3.Text;
				project.Url=textBox7.Text;
				SqlDBUtil.insert(project);
				getAllProjectInfo();
			}
		}
		
		bool ValidateProjectEdit()
		{
			bool isvalidated = true;
			string message ="";
			if(textBox4.Text.Trim()=="")
			{
				isvalidated = false;
				message += "项目名称,";
			}
			if(textBox3.Text.Trim()=="")
			{
				isvalidated = false;
				message += "测试路径,";
			}
			if(textBox7.Text.Trim()=="")
			{
				isvalidated = false;
				message += "Url";
			}
			if(!isvalidated)
			{
				message=message.Substring(0,message.Length-1);
				MessageBox.Show("请在编辑处输入"+message,"提示");
			}
			return isvalidated;
		}
		
		void ListView2Click(object sender, EventArgs e)
		{
			ProjectInfo selectmodule=ListView2_Select(this.listView2.SelectedItems[0]);
			this.textBox4.Text = selectmodule.Projectname;
			this.textBox3.Text = selectmodule.Projectpath;
			this.textBox7.Text = selectmodule.Url;
			
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			if(this.listView2.SelectedItems.Count!=1)
			{
				MessageBox.Show("请选择一条记录","提示");
				return;
			}
			if(ValidateProjectEdit())
			{
				ProjectInfo project=ListView2_Select(this.listView2.SelectedItems[0]);
				project.Projectname = this.textBox4.Text;
				project.Projectpath = this.textBox3.Text;
				project.Url=this.textBox7.Text;
				SqlDBUtil.update(project);
				this.getAllProjectInfo();
			}
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			if(this.listView2.SelectedItems.Count!=1)
			{
				MessageBox.Show("请选择一条记录","提示");
				return;
			}
			ProjectInfo project=ListView2_Select(this.listView2.SelectedItems[0]);
			SqlDBUtil.delete(project);
			this.getAllProjectInfo();
		}
/**************************项目信息结束*******************************************************************/	

/**************************责任人信息开始*******************************************************************/	

	
		private void getAllPersonInfo()
		{
			
			this.textBox6.Text =null;
			this.textBox5.Text=null;
			List<PersonInfo> personlist=PersonDao.getAllPersonInfo();
			this.listView3.Items.Clear();
			foreach(PersonInfo person in personlist)
			{
				ListViewBingForPerson(person);
			}
		}
		private void ListViewBingForPerson(PersonInfo person)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text=person.Fullname;
			lvi.SubItems.Add(person.Ip);
			lvi.SubItems.Add(person.Id.ToString());
			//lvi.SubItems.Add();
			this.listView3.Items.Add(lvi);
		}

		
		private PersonInfo  ListView3_Select(ListViewItem lvi)
		{
			PersonInfo person = new PersonInfo();
			person.Fullname = lvi.SubItems[0].Text;
			person.Ip = lvi.SubItems[1].Text;
			person.Id = Int32.Parse(lvi.SubItems[2].Text);
			return person;
		}
		//新增
		void Button10Click(object sender, EventArgs e)
		{
			if(ValidatePersonEdit())
			{
				PersonInfo person= new PersonInfo();
				person.Fullname=textBox6.Text;
				person.Ip=textBox5.Text;
				SqlDBUtil.insert(person);
				getAllPersonInfo();
			}
		}
		
		bool ValidatePersonEdit()
		{
			bool isvalidated = true;
			string message ="";
			if(textBox6.Text.Trim()=="")
			{
				isvalidated = false;
				message += "责任人姓名,";
			}
			if(textBox5.Text.Trim()=="")
			{
				isvalidated = false;
				message += "IP地址,";
			}
			
			if(!isvalidated)
			{
				message=message.Substring(0,message.Length-1);
				MessageBox.Show("请在编辑处输入"+message,"提示");
			}
			return isvalidated;
		}
		
		void ListView3Click(object sender, EventArgs e)
		{
			PersonInfo selectperson=ListView3_Select(this.listView3.SelectedItems[0]);
			this.textBox6.Text = selectperson.Fullname;
			this.textBox5.Text = selectperson.Ip;
			
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			if(this.listView3.SelectedItems.Count!=1)
			{
				MessageBox.Show("请选择一条记录","提示");
				return;
			}
			if(ValidatePersonEdit())
			{
				PersonInfo person=ListView3_Select(this.listView3.SelectedItems[0]);
				person.Fullname = this.textBox6.Text;
				person.Ip = this.textBox5.Text;
				SqlDBUtil.update(person);
				this.getAllPersonInfo();
			}
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			if(this.listView3.SelectedItems.Count!=1)
			{
				MessageBox.Show("请选择一条记录","提示");
				return;
			}
			PersonInfo person=ListView3_Select(this.listView3.SelectedItems[0]);
			SqlDBUtil.delete(person);
			this.getAllPersonInfo();
		}
/**************************责任人信息结束*******************************************************************/	
/**************************关联开始*******************************************************************/	
		private void getAllModuleProject()
		{
			ProjectInfo project = new ProjectInfo();
			project.Projectname = "选择项目名称";
			project.Id = 0;
			projectlist.Insert(0,project);
			
			comboBox2.DataSource = projectlist;
			comboBox2.DisplayMember ="Projectname";
			comboBox2.ValueMember = "Id";
			
			getlistboxdata();
			
			
		}
		
		void getlistboxdata()
		{
			List<ModuleInfo> selmodule =ModuleDao.getAllModuleInfoByProjectID(comboBox2.SelectedValue.ToString());
			List<ModuleInfo> listBox1data = new List<ModuleInfo>();
			List<ModuleInfo> listBox2data = new List<ModuleInfo>();
			foreach(ModuleInfo module in modulelist )
			{
				bool t = true;
				foreach(ModuleInfo sel in selmodule)
				{
					if(sel.Equals(module))
					{
						t = false;
						break;
					}
				}
				if(t)
					listBox1data.Add(module);
				else 
					listBox2data.Add(module);
			}
			
			
			this.listBox1.DataSource = listBox1data;
			this.listBox1.DisplayMember = "Fullname";
			this.listBox1.ValueMember ="Id";
			
			this.listBox2.DataSource = listBox2data;
			this.listBox2.DisplayMember = "Fullname";
			this.listBox2.ValueMember ="Id";
			
		}
		
	
		
		void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox2.SelectedValue!=null)
			{
				getlistboxdata();
			}
		}
		
		
		
		//选择
		void Button4Click(object sender, EventArgs e)
		{
			int count = this.listBox1.SelectedItems.Count;
			if(count>0&&(int)comboBox2.SelectedValue!=0)
			{
				int i ;
				for(i=0;i<count;i++)
				{
					ModuleProject mp = new ModuleProject();
					mp.Moduleid=((ModuleInfo)this.listBox1.SelectedItems[i]).Id;
					mp.Projectid = (int)comboBox2.SelectedValue;
					SqlDBUtil.insert(mp);
				}
			}
			else 
			{
				MessageBox.Show("请选择相应的模块或项目","提示");
			}
			getlistboxdata();
		}
		void Button5Click(object sender, EventArgs e)
		{
			int count = this.listBox2.SelectedItems.Count;
			if(count>0&&(int)comboBox2.SelectedValue!=0)
			{
				int i ;
				for(i=0;i<count;i++)
				{
					string pid =comboBox2.SelectedValue.ToString();
					string mid =((ModuleInfo)this.listBox2.SelectedItems[i]).Id.ToString();
					List<ModuleProject> mplist = 
						ModuleProjectDao.getAllMPByPrjIDAndMdlID(pid,mid);
					SqlDBUtil.delete(mplist[0]);
				}
			}
			else 
			{
				MessageBox.Show("请选择相应的模块或项目","提示");
			}
			getlistboxdata();
		}
		
		void TabChangeRefresh(object sender, EventArgs e)
		{
			getAllModuleInfo();
			getAllProjectInfo();
			getAllPersonInfo();
			getAllModuleProject();
		}
	}
}
