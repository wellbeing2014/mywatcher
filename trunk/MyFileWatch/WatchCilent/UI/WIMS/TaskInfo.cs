/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/6/8
 * 时间: 10:44
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;
using WatchCilent.WimsManager;
using WatchCilent;
using System.Configuration;

namespace WatchCilent.UI.WIMS
{
	/// <summary>
	/// Description of TaskInfo.
	/// </summary>
	public partial class TaskInfo : Form,UI.MainPlug
	{
		
		public CommonConst.UIShowSytle getSytle()
		{
			return CommonConst.UIShowSytle.Form;
		}
		public string getAuthorCode()
		{
			return "1,2,3,4";
		}
		
		public string[] getPlugName()
		{
			return new string[]{"WIMS","任务填写"};
		}
		
		private TrankingManager tm = null;
		
		//任务状态 0未开始，1进行中，2已完成
		private string _taskstate = "0";
		//项目阶段
		private string _xmjd = "";
		//WimsProInfoSectionSettings obj = System.Configuration.ConfigurationManager.GetSection("WimsProInfos") as WimsProInfoSectionSettings;
		List<Object> prolist = new List<Object>();
		public TaskInfo()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			
			InitializeComponent();
			string wimsurl = "http://58.214.246.37:8120/wisoftintegrateframe/services/WimsManager";
			tm = new TrankingManager(wimsurl);
			
			this.textBox1.ReadOnly = true;
			this.textBox1.Text = tm.findNewTaskNumber();
			
//			//从配置文件中获取项目信息
//			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//			WimsProInfoSectionSettings proConfigSection = config.GetSection("WimsProInfos") as WimsProInfoSectionSettings;
//			WimsProInfoCollection proConfigCollection = proConfigSection.WimsProInfos;
//			if(proConfigCollection.Count>0)
//			{
//				foreach (WimsProInfoElement element in proConfigCollection) {
//					Object newobj = new {Name=element.Name,Id=element.Id};
//					prolist.Add(newobj);
//				}
//			}
//			else//如果没有取得项目信息，则从WIMS中加载
//			{
//				resultReturnByArray allpro = tm.findAllWimsProInfo();
//				Object[] proobj = allpro.arrayobj;
//				foreach (var element in proobj) {
//					wimsProInfo a = element as wimsProInfo;
//					WimsProInfoElement procfg = new WimsProInfoElement();
//					procfg.Id = a.id;
//					procfg.Name =a.proname;
//					proConfigCollection.Add(procfg);
//					Object newobj = new {Name=a.proname,Id=a.id};
//					prolist.Add(newobj);
//				}
//				config.Save();
//			}
			
			
			
			resultReturnByArray allpro = tm.findAllWimsProInfo();
			Object[] proobj = allpro.arrayobj;
			foreach (var element in proobj) {
				wimsProInfo a = element as wimsProInfo;
//				WimsProInfoElement procfg = new WimsProInfoElement();
//				procfg.Id = a.id;
//				procfg.Name =a.proname;
//				proConfigCollection.Add(procfg);
				Object newobj = new {Name=a.proname,Id=a.id,Xmjd = a.xmjd};
				prolist.Add(newobj);
			}
			
			//项目
			this.comboBox1.DataSource = prolist;
			this.comboBox1.DisplayMember ="Name";
			this.comboBox1.ValueMember = "Id";
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			//优先级
			List<Object> aa=new List<object>{
				new {Name="重要且紧急",Id=0},
			    new {Name="紧急不重要",Id=1},
			    new {Name="重要不紧急",Id=2},
				new {Name="不重要不紧急",Id=3}
			           };
			this.comboBox2.DataSource = aa;
			this.comboBox2.DisplayMember ="Name";
			this.comboBox2.ValueMember = "Id";
			
			//难易程度
			List<Object> bb=new List<object>{
				new {Name="高",Id=0},
			    new {Name="中",Id=1},
			    new {Name="低",Id=2}
			           };
			this.comboBox3.DataSource = bb;
			this.comboBox3.DisplayMember ="Name";
			this.comboBox3.ValueMember = "Id";
			
			//任务来源
			List<Object> cc=new List<object>{
					   new {Name="自我安排",Id=3},
			           new {Name="领导分配",Id=1},
			           new {Name="现场支援",Id=2},
					   new {Name="需求沟通",Id=4},
					   new {Name="界面审改",Id=5}
			           };
			this.comboBox4.DataSource = cc;
			this.comboBox4.DisplayMember ="Name";
			this.comboBox4.ValueMember = "Id";
			
			//任务类型
			List<Object> dd=new List<object>{
					   new {Name="策划",Id=1},
			           new {Name="调研",Id=2},
			           new {Name="设计开发",Id=3},
					   new {Name="系统测试",Id=4},
					   new {Name="实施",Id=5},
					   new {Name="维护",Id=6},
			           new {Name="管理",Id=10},
			           new {Name="支持",Id=11}
			           };
			this.comboBox5.DataSource = dd;
			this.comboBox5.DisplayMember ="Name";
			this.comboBox5.ValueMember = "Id";
			
			this.numericUpDown1.Value= 4.0M;
			this.numericUpDown2.Value = 4.0M;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}	
		

		
		//保存按钮
		void Button1Click(object sender, EventArgs e)
		{
			wimsTaskmgr newwtm = new wimsTaskmgr();
			
			//项目信息
			newwtm.proinfo_id = this.comboBox1.SelectedValue.ToString();
			newwtm.proinfo_name =this.comboBox1.Text;
			newwtm.task_begin_time = this.dateTimePicker1.Value;
			newwtm.task_begin_timeSpecified=true;
			newwtm.task_create_person = GlobalParams.WimsLoginId;
			newwtm.task_create_time = DateTime.Now;
			newwtm.task_create_timeSpecified =true;
			newwtm.task_difficulty = this.comboBox3.SelectedValue.ToString();
			newwtm.task_end_time = this.dateTimePicker2.Value;
			newwtm.task_end_timeSpecified = true ;
			newwtm.task_gjys = (float)this.numericUpDown1.Value;
			//newwtm.task_id = ;
			newwtm.task_isdelete = 0;
			newwtm.task_name = this.textBox2.Text;
			if(tm.findNewTaskNumber().Equals(this.textBox1.Text))
			{
				newwtm.task_number = this.textBox1.Text ;
			}
			else
			{
				newwtm.task_number = tm.findNewTaskNumber() ;
				this.textBox1.Text = tm.findNewTaskNumber() ;
			}
			newwtm.task_origin = this.comboBox4.SelectedValue.ToString();
		//	newwtm.t
			newwtm.task_owner = GlobalParams.WimsLoginId;
			
			newwtm.task_priority = this.comboBox2.SelectedValue.ToString();
			newwtm.task_remark = this.textBox3.Text;
			if(newwtm.task_begin_time!=null)
			{
				this._taskstate = "1";
			}
			if(newwtm.task_end_time!=null)
			{
				this._taskstate = "2";
			}
			newwtm.task_state = this._taskstate;
			newwtm.task_workload = (float)this.numericUpDown2.Value;;
			//newwtm.track_id = ;
//			Type ab =(this.comboBox1.SelectedItem).GetType();
//			Object xmjdobj = ab.GetProperty("Xmjd").GetValue(this.comboBox1.SelectedItem,null);
			newwtm.xmjd = this.comboBox5.SelectedValue.ToString();
			
			tm.saveTask(newwtm);
			MessageBox.Show("保存成功","提示");
			//tm.u
		}
		
		void NumericUpDown2ValueChanged(object sender, EventArgs e)
		{
//			DateTime dt = this.dateTimePicker1.Value;
//			double minute =(dt.Minute>=30)?60:0;
//			dt.AddMinutes((-dt.Minute+minute));
//			this.dateTimePicker1.Value = dt;
			double realhour =(double)this.numericUpDown2.Value;
			this.dateTimePicker2.Value = this.dateTimePicker1.Value.AddHours(realhour);
		}
		
		
		
	}
}
