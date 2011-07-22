/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011/6/25
 * 时间: 20:38
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using WatchCilent.dao;
using WatchCilent.pojo;
using WatchCilent.Common;

namespace WatchCilent
{
	/// <summary>
	/// Description of PackageUI.
	/// </summary>
	public partial class PackageUI : UserControl
	{
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		
		public PackageUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.listView1.DoubleClick+= new EventHandler(Button4Click);
			TreeNode tmp =new TreeNode("全部");
			tmp.Nodes.Add("已接收");
			tmp.Nodes.Add("已测试");
			tmp.Nodes.Add("已发布");
			tmp.Nodes.Add("已完成");
			tmp.Nodes.Add("已作废");
			treeView1.Nodes.Add(tmp);
			treeView1.SelectedNode=treeView1.Nodes[0].Nodes[0];
			treeView1.ExpandAll();
			treeView1.NodeMouseClick+= new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
			treeView1.Leave+=new EventHandler(treeView1_Leave);
			treeView1.BeforeSelect+=new TreeViewCancelEventHandler(treeView1_BeforeSelect);
			
			
			List<PersonInfo> datasource_person = PersonDao.getAllPersonInfo();
			PersonInfo person = new PersonInfo();
			person.Fullname = "全部责任人";
			person.Id = 0;
			datasource_person.Insert(0,person);
			this.comboBox2.DataSource = datasource_person;
			this.comboBox2.DisplayMember = "Fullname";
			this.comboBox2.ValueMember = "Id";
			this.comboBox2.SelectedIndexChanged+=new EventHandler(conditionChanged);
			
			List<ModuleInfo> datasource_module = ModuleDao.getAllModuleInfo();
			ModuleInfo all = new ModuleInfo();
			all.Fullname ="全部模块";
			all.Id=0;
			datasource_module.Insert(0,all);
			this.comboBox1.DataSource = datasource_module;
			this.comboBox1.DisplayMember ="Fullname";
			this.comboBox1.ValueMember = "Id";
			
			this.comboBox1.SelectedIndexChanged+=new EventHandler(conditionChanged);
			this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
			this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			getAllPackInList();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		//查询按钮的方法			
		void Button5Click(object sender, EventArgs e)
		{
			getAllPackInList();
		}
		
		//手动处理的按钮方法
		void Button4Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count==1)
			{
				PackageInfo pack = new PackageInfo();
				pack = ListViewSelect(listView1.SelectedItems[0]);
				BussinessForm bf = new BussinessForm(pack);
			 	bf.StartPosition = FormStartPosition.CenterParent;
				bf.ShowDialog();
				getAllPackInList();
			}
			else
			{
				MessageBox.Show("请选择一个更新包","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			
		}
		
		//隐藏按钮的方法
		
//		void Button2Click(object sender, EventArgs e)
//		{
//			this.WindowState =  FormWindowState.Minimized;
//			this.Visible = false;
//			this.notifyIcon1.BalloonTipText="正在监控中……";
//			this.notifyIcon1.ShowBalloonTip(1);
//		}
//		
		private void getAllPackInList()
		{
			string moduleid = this.comboBox1.SelectedValue.ToString();
			string manageid = this.comboBox2.SelectedValue.ToString();
			string state = this.treeView1.SelectedNode.Text;
			
			string begin=this.dateTimePicker1.Value.ToShortDateString()+" 00:00:00";
			string end =this.dateTimePicker2.Value.ToShortDateString()+" 23:59:59";
			if(this.dateTimePicker1.IsDisposed&&this.dateTimePicker2.IsDisposed)
			{
				begin=null;
				end =null;
			}
			List<PackageInfo> ls =PackageDao.queryPackageInfo(moduleid,manageid,state,begin,end);
			this.listView1.Items.Clear();
			foreach(PackageInfo pack in ls)
			{
				ListViewBing(pack);
			}
		}
		private void ListViewBing(PackageInfo pack)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text=pack.Packagename;
			lvi.Checked=false;
			lvi.SubItems.Add(pack.Packagepath);
			lvi.SubItems.Add(pack.Packtime);
			lvi.SubItems.Add(pack.Testtime);
			lvi.SubItems.Add(pack.Publishtime);
			lvi.SubItems.Add(pack.State);
			lvi.SubItems.Add(pack.Moduleid.ToString());
			lvi.SubItems.Add(pack.Managerid.ToString());
			lvi.SubItems.Add(pack.Id.ToString());
			//lvi.SubItems.Add();
			this.listView1.Items.Add(lvi);
		}
		
		private PackageInfo  ListViewSelect(ListViewItem lvi)
		{
			PackageInfo pack = new PackageInfo();
			pack.Packagename = lvi.Text;
			pack.Packagepath = lvi.SubItems[1].Text;
			pack.Packtime = lvi.SubItems[2].Text;
			pack.Publishtime = lvi.SubItems[4].Text;
			pack.Testtime = lvi.SubItems[3].Text;
			pack.State = lvi.SubItems[5].Text;
			pack.Moduleid = Int32.Parse(lvi.SubItems[6].Text);
			pack.Managerid =Int32.Parse(lvi.SubItems[7].Text);
			pack.Id = Int32.Parse(lvi.SubItems[8].Text);
			return pack;
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			ConfigForm cm = new ConfigForm();
			cm.StartPosition = FormStartPosition.CenterParent;
			cm.ShowDialog();
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0)
			{
				foreach(ListViewItem lt in listView1.SelectedItems)
				{
					PackageInfo pack = new PackageInfo();
					pack = ListViewSelect(lt);
					AccessDBUtil.delete(pack);
				}
			}
			else
			{
				MessageBox.Show("请选择更新包","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			getAllPackInList();
		}
		
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0&&!this.checkBox1.Checked)
			{
				this.button6.Enabled=true;
				this.button7.Enabled=true;
				this.button8.Enabled = true;
				this.button9.Enabled = true;
			}
			else 
			{
				this.button6.Enabled=false;
				this.button7.Enabled=false;
				this.button8.Enabled = false;
				this.button9.Enabled = false;
			}
			
			
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0)
			{
				PackageInfo pack = new PackageInfo();
				pack = ListViewSelect(listView1.SelectedItems[0]);
				pack.State="已测试";
				pack.Testtime=System.DateTime.Now.ToLocalTime().ToString();
				AccessDBUtil.update(pack);
			}
			getAllPackInList();
		}
		void Button8Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0)
			{
				PackageInfo pack = new PackageInfo();
				pack = ListViewSelect(listView1.SelectedItems[0]);
				pack.State="已作废";
				AccessDBUtil.update(pack);
			}
			getAllPackInList();
		}
		void Button9Click(object sender, EventArgs e)
		{
			if(listView1.SelectedItems.Count!=0)
			{
				PackageInfo pack = new PackageInfo();
				pack = ListViewSelect(listView1.SelectedItems[0]);
				pack.State="已发布";
				pack.Publishtime=System.DateTime.Now.ToLocalTime().ToString();
				AccessDBUtil.update(pack);
			}
			getAllPackInList();
		}
		
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			Point cb1p=new Point();
			cb1p=this.comboBox1.Location;
			Point cb2p=new Point();
			cb2p=this.comboBox2.Location;
			if(!this.checkBox2.Checked)
			{
				this.dateTimePicker1.Dispose();
				this.dateTimePicker2.Dispose();
				
				this.label1.Dispose();
				this.label2.Dispose();
				cb1p.X=cb1p.X-234;
				cb2p.X=cb2p.X-234;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
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
				// label2
				// 
				this.label2.Location = new System.Drawing.Point(360, 17);
				this.label2.Name = "label2";
				this.label2.Size = new System.Drawing.Size(19, 18);
				this.label2.TabIndex = 10;
				this.label2.Text = "至";
				// 
				// label1
				// 
				this.label1.Location = new System.Drawing.Point(242, 17);
				this.label1.Name = "label1";
				this.label1.Size = new System.Drawing.Size(19, 18);
				this.label1.TabIndex = 9;
				this.label1.Text = "起";
				
				
				
				this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
				this.dateTimePicker1.Location = new System.Drawing.Point(267, 13);
				this.dateTimePicker1.Name = "dateTimePicker1";
				this.dateTimePicker1.Size = new System.Drawing.Size(87, 21);
				DateTime dt =DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
				this.dateTimePicker1.TabIndex = 7;
				
				this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
				this.dateTimePicker2.Location = new System.Drawing.Point(385, 13);
				this.dateTimePicker2.Name = "dateTimePicker2";
				this.dateTimePicker2.Size = new System.Drawing.Size(84, 21);
				this.dateTimePicker2.TabIndex = 8;
				cb1p.X=cb1p.X+234;
				cb2p.X=cb2p.X+234;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
			}
			getAllPackInList();
		}
		private void conditionChanged(object sender, EventArgs e)
		{
			getAllPackInList();
		}
		
		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs  e)
		{
			this.treeView1.SelectedNode=e.Node;
			
			getAllPackInList();
		}
		 //将要选中新节点之前发生
        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView1.SelectedNode.BackColor = Color.Empty;
                //还原前景色
                treeView1.SelectedNode.ForeColor = Color.Black;
            }
        }
        //失去焦点时
        private void treeView1_Leave(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode!=null)
            {
                //让选中项背景色呈现蓝色
                treeView1.SelectedNode.BackColor = Color.SteelBlue;
                //前景色为白色
                treeView1.SelectedNode.ForeColor = Color.White;
            }
        }


		
	}
}
