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

namespace WatchCilent
{
	/// <summary>
	/// Description of PackageUI.
	/// </summary>
	public partial class PackageUI : UserControl
	{
		public PackageUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			TreeNode tmp =new TreeNode("全部");
			tmp.Nodes.Add("已接收");
			tmp.Nodes.Add("已测试");
			tmp.Nodes.Add("已发布");
			tmp.Nodes.Add("已完成");
			tmp.Nodes.Add("已作废");
			treeView1.Nodes.Add(tmp);
			treeView1.ExpandAll();
			
			
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
			string state="全部";
			List<PackageInfo> ls =PackageDao.queryPackageInfo(moduleid,manageid,state);
			this.listView1.Items.Clear();
			foreach(PackageInfo pack in ls)
			{
				ListViewBing(pack);
			}
		}
		private void ListViewBing(PackageInfo pack)
		{
			ListViewItem lvi = new ListViewItem();
			//lvi.Text=pack.Packagename;
			lvi.Checked=false;
			lvi.SubItems.Add(pack.Packagename);
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
			pack.Packagename = lvi.SubItems[0].Text;
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
	}
}
