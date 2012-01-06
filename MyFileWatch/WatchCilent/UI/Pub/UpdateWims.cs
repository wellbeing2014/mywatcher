/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2012-1-4
 * 时间: 10:05
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using WatchCilent.WimsToTest;

namespace WatchCilent.UI.Pub
{
	/// <summary>
	/// Description of UpdateWims.
	/// </summary>
	public partial class UpdateWims : Form
	{
		private WimsToTestManager tm;
		private string[] vers;
		private bool isEditQuery = false;
		private string wimsLoginId="";
		public UpdateWims(string[] versions)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.vers = versions;
			this.wimsLoginId = System.Configuration.ConfigurationManager.AppSettings["WimsLoginId"];
			InitializeComponent();
			try {
				string wimsurl = System.Configuration.ConfigurationManager.AppSettings["WimsUrl"];
				tm = new WimsToTestManager(wimsurl);
				getWimsTrack(vers);
			} catch (Exception e ) {
				MessageBox.Show("访问WIMS服务出错");
			}
			
		
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		/// <summary>
		/// 获取问题单
		/// </summary>
		private void getWimsTrack(string[] ver)
		{
			this.listView1.Items.Clear();
			for (int i = 0; i < ver.Length; i++) {
				trackReturn tr = tm.findFinishWimsTrackByVer(ver[i]);
			    wimsSingleIssueTracking[] wt = tr.value;
			    if(wt!=null)
			    {
			    	foreach (var element in wt) {
			    		ListViewBing(element);
			    	}
			    }
			}
			
		}
		/// <summary>
		/// LIstVieW 绑定数据。
		/// </summary>
		/// <param name="tu"></param>
		private void ListViewBing(wimsSingleIssueTracking tu)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text=tu.lsh;//序号
			lvi.Checked=false;
			lvi.SubItems.Add(tu.proid);//项目名称
			lvi.SubItems.Add(tu.ver);//版本信息
			lvi.SubItems.Add(tu.sqpersonid);//申请人
			lvi.SubItems.Add(tu.sqdate.ToString("yyyy-MM-dd"));//申请日期
			lvi.SubItems.Add(tu.finishtime.ToString("yyyy-MM-dd"));//实际日
			lvi.SubItems.Add(tu.content);//内容
			string status = string.Empty;
			switch (tu.status) {
				case "0":
					status = "未完成";
					break;
				case "1":
					status = "已完成";
					break;
				case "2":
					status = "已发布";
					break;
			}
			lvi.SubItems.Add(status);//状态
			lvi.SubItems.Add(tu.id);//
			
			this.listView1.Items.Add(lvi);
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			tm.Dispose();
			this.Close();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(this.listView1.CheckedItems.Count>0)
			{
				bool sucess=true;
				string message ="";
				for (int i = 0; i < this.listView1.CheckedItems.Count; i++) {
					string trackid = this.listView1.CheckedItems[i].SubItems[8].Text;
					string newstatus ="1";
					baseReturn br = tm.updateWimsTrackStatus(trackid,this.wimsLoginId,newstatus);
					if(!br.isSuccued)
					{
						message+="序号：【"+this.listView1.CheckedItems[i].Text+"】"+br.message+"\n";
						sucess = false;
					}
				}
				if(sucess)
				{
					MessageBox.Show("更新成功","提示");
				}
				else
					MessageBox.Show(message,"有错误");
				if(isEditQuery)
				{
					getWimsTrack(new string[]{this.textBox1.Text});
				}
				else getWimsTrack(vers);
			}
			else
				MessageBox.Show("请选择要发布的问题单");
		}
		
		void textBox1_Click(object sender, EventArgs e)
		{
			this.textBox1.Text = "";
		}
		
		
		void textBox1_TabIndexChanged(object sender, EventArgs e)
		{
			isEditQuery = true;
			if(!string.IsNullOrEmpty(this.textBox1.Text)&&this.textBox1.Text!="输入版本号……")
			{
				getWimsTrack(new string[]{this.textBox1.Text});
			}
		}
	}
}
