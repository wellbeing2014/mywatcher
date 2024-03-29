﻿/*
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
	public partial class ConfigForm : Form,UI.MainPlug
	{
		public CommonConst.UIShowSytle getSytle()
		{
			return CommonConst.UIShowSytle.Form;
		}
		
		public string getAuthorCode()
		{
			//配置、经理、测试
			return "3,4,5";
		}
		
		public string[] getPlugName()
		{
			return new string[]{"工具","配置"};
		}
		
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
			getAllModuleProject2();
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
			modulelist2=ModuleDao.getAllModuleInfo();
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
			lvi.SubItems.Add(module.Lastversion);
			lvi.SubItems.Add(module.Managerid.ToString());
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
				newmodule.Lastversion=textBox9.Text;
				newmodule.Id=0;
				if(ModuleDao.getModuleByCode(newmodule.Code))
				{
					MessageBox.Show("模块代码已存在，不能重复！","提示：");
					return;
				}
				else
				{
					bool isinsert=SqlDBUtil.insert(newmodule);
				if(isinsert) getAllModuleInfo();
				}
			}
		}
		//选择列表记录
		void listView1_Click(object sender, EventArgs e)
		{
			ModuleInfo selectmodule=ListView1_Select(this.listView1.SelectedItems[0]);
			this.textBox1.Text = selectmodule.Fullname;
			this.textBox2.Text = selectmodule.Code;
			this.textBox9.Text = selectmodule.Lastversion;
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
			module.Lastversion = lvi.SubItems[3].Text;
			module.Managerid = Int32.Parse(lvi.SubItems[4].Text);
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
				module.Lastversion = this.textBox9.Text ;
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
			if(textBox9.Text.Trim()=="")
			{
				isvalidated = false;
				message += "最新版本,";
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
			this.textBox8.Text = null;
			projectlist=ProjectInfoDao.getAllProjectInfo();
			projectlist2=ProjectInfoDao.getAllProjectInfo();
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
			lvi.SubItems.Add(project.Ftppath);
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
			project.Ftppath = lvi.SubItems[3].Text;
			project.Id = Int32.Parse(lvi.SubItems[4].Text);
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
				project.Ftppath=textBox8.Text;
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
				message += "Url,";
			}
			if(textBox8.Text.Trim()=="")
			{
				isvalidated = false;
				message += "FTP路径";
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
			this.textBox8.Text = selectmodule.Ftppath;
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
				project.Ftppath=this.textBox8.Text;
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
			this.textBox10.Text =null;
			this.textBox11.Text=null;
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
			lvi.SubItems.Add(person.Password);
			lvi.SubItems.Add(person.Role);
			//lvi.SubItems.Add();
			lvi.SubItems.Add(person.Id.ToString());
			this.listView3.Items.Add(lvi);
		}

		
		private PersonInfo  ListView3_Select(ListViewItem lvi)
		{
			PersonInfo person = new PersonInfo();
			person.Fullname = lvi.SubItems[0].Text;
			person.Ip = lvi.SubItems[1].Text;
			person.Password = lvi.SubItems[2].Text ;
			person.Role = lvi.SubItems[3].Text ;
			person.Id = Int32.Parse(lvi.SubItems[4].Text);
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
				person.Password = textBox10.Text ;
				person.Role = string.IsNullOrEmpty(textBox11.Text.Trim())?"0":textBox11.Text.Trim() ;
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
			this.textBox10.Text = selectperson.Password;
			this.textBox11.Text = selectperson.Role;
			
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
				person.Password = textBox10.Text ;
				person.Role = textBox11.Text ;
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
/**************************项目关联开始*******************************************************************/	
		private void getAllModuleProject()
		{
			ProjectInfo project = new ProjectInfo();
			project.Projectname = "选择项目名称";
			project.Id = 0;
			projectlist.Insert(0,project);
			
			comboBox2.DataSource = projectlist;
			comboBox2.DisplayMember ="Projectname";
			comboBox2.ValueMember = "Id";
			comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
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
			this.listBox1.SelectedItem =null;
			
			this.listBox2.DataSource = listBox2data;
			this.listBox2.DisplayMember = "Fullname";
			this.listBox2.ValueMember ="Id";
			this.listBox2.SelectedItem =null;
			
		}
		
	
//		
//		void comboBox2SelectedIndexChanged(object sender, EventArgs e)
//		{
//			if(comboBox2.SelectedValue!=null)
//			{
//				getlistboxdata();
//			}
//		}
		
		
		
		//选择
		void Button4Click(object sender, EventArgs e)
		{
			if(null!=comboBox2.SelectedValue)
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
			else MessageBox.Show("请选择相应的模块或项目","提示");
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
		
//		void TabChangeRefresh(object sender, EventArgs e)
//		{
//			getAllModuleInfo();
//			getAllProjectInfo();
//			getAllPersonInfo();
//			getAllModuleProject();
//			getAllModuleProject2();
//		}
		
		
/**************************项目关联结束*******************************************************************/	
/**************************版本关联开始*******************************************************************/	
		private void getAllModuleProject2()
		{
			ModuleInfo mi = new ModuleInfo();
			mi.Fullname = "选择版本名称";
			mi.Id = 0;
			modulelist2.Insert(0,mi);
			
			ProjectInfo project2 = new ProjectInfo();
//			project2.Projectname = "";
//			project2.Id = 0;
//			projectlist2.Insert(0,project2);
			
			comboBox3.DataSource = modulelist2;
			comboBox3.DisplayMember ="fullname";
			comboBox3.ValueMember = "id";
			comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
			comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			getlistboxdata2();			
		}					
		
		void getlistboxdata2()
		{
			
			
			List<ProjectInfo> selproject =ProjectInfoDao.getAllProjectInfoByModuleid(Int32.Parse(comboBox3.SelectedValue.ToString()));
			List<ProjectInfo> listBox4data = new List<ProjectInfo>();
			List<ProjectInfo> listBox3data = new List<ProjectInfo>();
			foreach(ProjectInfo project2 in projectlist2 )
			{
				bool t = true;
				foreach(ProjectInfo sel in selproject)
				{
					if(sel.Equals(project2))
					{
						t = false;
						break;
					}
				}
				if(t)
					listBox4data.Add(project2);
				else 
					listBox3data.Add(project2);
			}
			
			
			this.listBox4.DataSource = listBox4data;
			this.listBox4.DisplayMember = "projectname";
			this.listBox4.ValueMember ="id";
			this.listBox4.SelectedItem =null;
			
			this.listBox3.DataSource = listBox3data;
			this.listBox3.DisplayMember = "projectname";
			this.listBox3.ValueMember ="id";	
			this.listBox3.SelectedItem =null;			
		}
		

		
		//选择
		void Button13Click(object sender, EventArgs e)
		{
			if(null!=comboBox3.SelectedValue)
			{
			int count = this.listBox4.SelectedItems.Count;
			if(count>0&&(int)comboBox3.SelectedValue!=0)
			{
				int i ;
				for(i=0;i<count;i++)
				{
					ModuleProject mp = new ModuleProject();
					mp.Projectid=((ProjectInfo)this.listBox4.SelectedItems[i]).Id;
					//mp.Moduleid = (int)comboBox3.SelectedValue;
					mp.Moduleid = Int32.Parse(comboBox3.SelectedValue.ToString());
					SqlDBUtil.insert(mp);
				}
			}
			else 
			{
				MessageBox.Show("请选择相应的模块或项目","提示");
			}
			getlistboxdata2();
			}
			else MessageBox.Show("请选择相应的模块或项目","提示");
		}
		
		//撤选
		void Button12Click(object sender, EventArgs e)
		{
			int count = this.listBox3.SelectedItems.Count;
			if(count>0&&(int)comboBox3.SelectedValue!=0)
			{
				int i ;
				for(i=0;i<count;i++)
				{
					string mid =comboBox3.SelectedValue.ToString();
					string pid =((ProjectInfo)this.listBox3.SelectedItems[i]).Id.ToString();
					List<ModuleProject> mplist = 
						ModuleProjectDao.getAllMPByPrjIDAndMdlID(pid,mid);
					SqlDBUtil.delete(mplist[0]);
				}
			}
			else 
			{
				MessageBox.Show("请选择相应的模块或项目","提示");
			}
			getlistboxdata2();
		}
		
		void TabChangeRefresh(object sender, EventArgs e)
		{
			getAllModuleInfo();
			getAllProjectInfo();
			getAllPersonInfo();
			getAllModuleProject();
			getAllModuleProject2();
		}
		
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox3.SelectedValue!=null)
			{
				getlistboxdata2();
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox2.SelectedValue!=null)
			{
				getlistboxdata();
			}
		}
/**************************版本关联结束*******************************************************************/
	}
}
