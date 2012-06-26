/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/17
 * 时间: 8:56
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;
using System.Collections.Generic;
using WatchCilent.UI;

namespace WatchCilent.UI.UICheck
{
	/// <summary>
	/// Description of UICheckList.
	/// </summary>
	public partial class UICheckList : UserControl,MainPlug
	{
		
		private int currentpage=0;
		private int count = 0;
		private int pagesize = 20;
		
		private string currentstr = "当前第{0}页";
		private string countstr ="共{0}页/共{1}条";
		private string pagestr ="每页{0}条";
		
		
		public CommonConst.UIShowSytle getSytle()
		{
			return CommonConst.UIShowSytle.UserControl;
		}
		public string getAuthorCode()
		{
			return "2,3,4,5";
		}
		
		public string[] getPlugName()
		{
			return new string[]{"测试","界面检查"};
		}
		public UICheckList()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			
			System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
				
			List<PersonInfo> datasource_person = PersonDao.getAllPersonInfo();
			PersonInfo person = new PersonInfo();
			person.Fullname = "全部责任人";
			person.Id = 0;
			datasource_person.Insert(0,person);
			this.comboBox2.DataSource = datasource_person;
			this.comboBox2.DisplayMember = "Fullname";
			this.comboBox2.ValueMember = "Id";
		
			
			List<ModuleInfo> datasource_module = ModuleDao.getAllModuleInfo();
			ModuleInfo all = new ModuleInfo();
			all.Fullname ="全部模块";
			all.Id=0;
			datasource_module.Insert(0,all);
			this.comboBox1.DataSource = datasource_module;
			this.comboBox1.DisplayMember ="Fullname";
			this.comboBox1.ValueMember = "Id";
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.comboBox3.Items.Add("全部状态");
			this.comboBox3.Items.AddRange(Enum.GetNames(typeof(CommonConst.CheckState)));
			this.comboBox3.SelectedIndex = 0;
			
			this.comboBox3.SelectedIndexChanged+=new EventHandler(conditionChanged);
			this.comboBox1.SelectedIndexChanged+=new EventHandler(conditionChanged);
			this.comboBox2.SelectedIndexChanged+=new EventHandler(conditionChanged);
					
			
			this.listView1.DoubleClick += new EventHandler(ListVew_DoubleClick); 
			this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
			this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			
			this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,(count%pagesize==0)?count/pagesize:count/pagesize+1,this.count);
			
			getUIcheckList();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		/// <summary>
		/// 时间段显示、隐藏
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			Point cb1p=new Point();
			cb1p=this.comboBox1.Location;
			Point cb2p=new Point();
			cb2p=this.comboBox2.Location;
			Point cb3p=new Point();
			cb3p=this.comboBox3.Location;
			if(!this.checkBox2.Checked)
			{
				this.dateTimePicker1.Dispose();
				this.dateTimePicker2.Dispose();
				this.label1.Dispose();
				this.label2.Dispose();
				cb1p.X=cb1p.X-250;
				cb2p.X=cb2p.X-250;
				cb3p.X=cb3p.X-250;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
				this.comboBox3.Location=cb3p;
			}
			else
			{
				this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
				this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
				this.label2 = new System.Windows.Forms.Label();
				this.label1 = new System.Windows.Forms.Label();
				
				this.panel1.Controls.Add(this.dateTimePicker1);
				this.panel1.Controls.Add(this.dateTimePicker2);
				this.panel1.Controls.Add(this.label1);
				this.panel1.Controls.Add(this.label2);
				
				// 
				// dateTimePicker1
				// 
				this.dateTimePicker1.Location = new System.Drawing.Point(86, 5);
				this.dateTimePicker1.Name = "dateTimePicker1";
				this.dateTimePicker1.Size = new System.Drawing.Size(106, 21);
				this.dateTimePicker1.TabIndex = 72;
				// 
				// dateTimePicker2
				// 
				this.dateTimePicker2.Location = new System.Drawing.Point(221, 5);
				this.dateTimePicker2.Name = "dateTimePicker2";
				this.dateTimePicker2.Size = new System.Drawing.Size(107, 21);
				this.dateTimePicker2.TabIndex = 73;
				// 
				// label1
				// 
				this.label1.Location = new System.Drawing.Point(61, 8);
				this.label1.Name = "label1";
				this.label1.Size = new System.Drawing.Size(19, 18);
				this.label1.TabIndex = 74;
				this.label1.Text = "起";
				// 
				// label2
				// 
				this.label2.Location = new System.Drawing.Point(198, 8);
				this.label2.Name = "label2";
				this.label2.Size = new System.Drawing.Size(17, 18);
				this.label2.TabIndex = 75;
				this.label2.Text = "止";
				cb1p.X=cb1p.X+250;
				cb2p.X=cb2p.X+250;
				cb3p.X=cb3p.X+250;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
				this.comboBox3.Location=cb3p;
				System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
				this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
				this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			}
			getUIcheckList();
		}
		
		
		
		
		
		/// <summary>
		/// 获取检查列表
		/// </summary>
		void getUIcheckList()
		{
			string moduleid = this.comboBox1.SelectedValue.ToString();
			string manageid = this.comboBox2.SelectedValue.ToString();
			string state = this.comboBox3.Text;
			string begin=this.dateTimePicker1.Value.ToShortDateString()+" 00:00:00";
			string end =this.dateTimePicker2.Value.ToShortDateString()+" 23:59:59";
			if(this.dateTimePicker1.IsDisposed&&this.dateTimePicker2.IsDisposed)
			{
				begin=null;
				end =null;
			}
			
			this.count = UICheckDao.QueryUIcheckCount(moduleid,manageid,state,begin,end);
			int countpage = (count%pagesize==0)?count/pagesize:count/pagesize+1;
			if(this.currentpage>countpage) this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,countpage,this.count);
			
			this.listView1.Items.Clear();
			List<UIcheckinfo> allui=UICheckDao.QueryUIcheckinfo(moduleid,manageid,state,begin,end,
			                                             (currentpage>1)?((this.currentpage-1)*pagesize):0
			                                                  ,pagesize);
			foreach (UIcheckinfo tu in allui) {
				ListViewBing(tu);
			}
		}
		
		/// <summary>
		/// LIstVieW 绑定数据。
		/// </summary>
		/// <param name="tu"></param>
		private void ListViewBing(UIcheckinfo tu)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text=tu.Checkno;//
			lvi.Checked=false;
			lvi.SubItems.Add(tu.Packagename);
			lvi.SubItems.Add(tu.Createtime);
			lvi.SubItems.Add(tu.Checkedtime);
			lvi.SubItems.Add(tu.Checkername);//
			lvi.SubItems.Add(tu.State);
			lvi.SubItems.Add(tu.Id.ToString());//
			this.listView1.Items.Add(lvi);
			
		}
		
		private UIcheckinfo  ListViewSelect(ListViewItem lvi)
		{
			UIcheckinfo tu = new UIcheckinfo();
			tu.Checkno = lvi.Text;
			tu.Packagename = lvi.SubItems[1].Text;
			tu.Createtime = lvi.SubItems[2].Text;
			tu.Checkedtime = lvi.SubItems[3].Text;
			tu.Checkername = lvi.SubItems[4].Text;
			tu.State = lvi.SubItems[5].Text;
			tu.Id = Int32.Parse(lvi.SubItems[6].Text);
			return tu;
		}
		
		/// <summary>
		/// 所有条件变化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void conditionChanged(object sender, EventArgs e)
		{
			getUIcheckList();
		}
		
		//首页
		void LinkLabel3LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentpage=1;
			getUIcheckList();
		}
		
		//上一页
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(this.currentpage>1)
			{
				this.currentpage--;
			}
			getUIcheckList();
		}
		
		
		//下一页
		void LinkLabel2LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(this.currentpage<((count%pagesize==0)?count/pagesize:count/pagesize+1))
			{
				this.currentpage++;
			}
			getUIcheckList();
		}
		
		//末页
		void LinkLabel4LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentpage=(count%pagesize==0)?count/pagesize:count/pagesize+1;
			getUIcheckList();
		}
		
		void ListVew_DoubleClick(object sender, EventArgs e)
		{
			if(this.listView1.SelectedItems.Count==1)
			{
				UIcheckinfo tu = ListViewSelect(this.listView1.SelectedItems[0]);
				
				UICheck uc = new UICheck(tu.Id,false);
				uc.ShowDialog();
			}
			else
			{
				MessageBox.Show("请选择一条记录","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			getUIcheckList();
			
		}
		
		// 新增
		void Button1Click(object sender, EventArgs e)
		{
			UICheck uc = new UICheck();
			uc.ShowDialog();
			getUIcheckList();
		}
		
		//修改
		void Button2Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count==1)
			{
				UIcheckinfo tu = ListViewSelect(this.listView1.CheckedItems[0]);
				
				UICheck uc = new UICheck(tu.Id,true);
				uc.ShowDialog();
			}
			else
			{
				MessageBox.Show("请选择一条记录","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			getUIcheckList();
		}
		
		//删除
		void Button3Click(object sender, EventArgs e)
		{
			
			if(this.listView1.CheckedItems.Count>0)
			{
				foreach (ListViewItem lvi in this.listView1.CheckedItems) {
					UIcheckinfo tu = ListViewSelect(lvi);
					SqlDBUtil.delete(tu);
					UICheckDao.DelUICheckViewByNO(tu.Checkno);
				}
				MessageBox.Show("删除成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
				getUIcheckList();
			}
			else
			{
				MessageBox.Show("请至少选择一条记录","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			bool isallcheck=this.checkBox1.Checked;
			foreach (ListViewItem element in this.listView1.Items) {
				element.Checked=isallcheck;
			}
		}
		
		//设置未检查,未通过，选择性通过，通过
		void Button5Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count>0)
			{
				string state="";
				string bt = ((Button)sender).Text;
				string checkedtime = "";
				string msgcontent ="{0},您好！\n您发布的{1}版本，";
				if("待检查".Equals(bt))
					state =Enum.GetName(typeof(CommonConst.CheckState),CommonConst.CheckState.未检查);
				if("未通过".Equals(bt))
				{
					state =Enum.GetName(typeof(CommonConst.CheckState),CommonConst.CheckState.未通过);
					checkedtime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
					msgcontent +="未通过界面检查。请登录以下网址查看界面检查详细:\n http://192.10.110.206:9001/CmdCtrlService/jsp/UIcheckView.jsp?checkno={2}";
				}
				if("选择性通过".Equals(bt))
				{
					state =Enum.GetName(typeof(CommonConst.CheckState),CommonConst.CheckState.选择性通过);
					checkedtime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
					msgcontent +="选择性通过界面检查。请登录以下网址查看界面检查详细:\n http://192.10.110.206:9001/CmdCtrlService/jsp/UIcheckView.jsp?checkno={2}";
				}
				if("已通过".Equals(bt))
				{
					state =Enum.GetName(typeof(CommonConst.CheckState),CommonConst.CheckState.已通过);
					checkedtime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
					msgcontent +="检查单号为{2}，已通过界面检查。";
				}
				foreach (ListViewItem lvi in this.listView1.CheckedItems) {
					string id =lvi.SubItems[6].Text;
					UIcheckinfo uiinfo = UICheckDao.getUIcheckInfoById(Int32.Parse(id));
					uiinfo.Checkedtime = checkedtime;
					uiinfo.State = state;
					uiinfo.Checkername =WatchCilent.GlobalParams.User.Fullname;
					uiinfo.Checkerid = WatchCilent.GlobalParams.User.Id;
					
					SqlDBUtil.update(uiinfo);
					SendMessageToManager(string.Format(msgcontent,uiinfo.Adminname,uiinfo.Packagename,uiinfo.Checkno),uiinfo.Adminid);
				}
				getUIcheckList();
			}
			else
			{
				MessageBox.Show("请至少选择一条记录！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}
		
		
		/// <summary>
		/// 给主管发送飞秋消息。
		/// </summary>
		void SendMessageToManager(string content,int personid)
		{
			PersonInfo p =PersonDao.getPersonInfoByid(personid);
			//List<PersonInfo> meigonglist = PersonDao.getPersonByRole(2);
			try {
				//string content ="您好：{0},\n"+uiinfo.Packagename+"的检查列表，已在"+uiinfo.Createtime+"被创建,请登录查看详细并检查。";
				//foreach (var element in meigonglist) {
					string[] iplist1 = p.Ip.Split(';');
					foreach(string ip1 in iplist1)
					{
						Communication.TCPManage.SendMessage(GlobalParams.WisofServiceHost,content+"##"+ip1);
					}
				//}
				
			} catch (Exception e) {
				MessageBox.Show("通知主管失败！:"+e.ToString());
			}
		}
	
	}
}
