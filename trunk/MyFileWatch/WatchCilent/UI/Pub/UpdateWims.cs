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
		public UpdateWims()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			try {
				string wimsurl = System.Configuration.ConfigurationManager.AppSettings["WimsUrl"];
				WimsToTestManager tm = new WimsToTestManager(wimsurl);
				trackReturn tr = tm.findFinishWimsTrackByVer("南京玄武框架(nj_wif)1.0.0beta1");
				wimsSingleIssueTracking[] wt = tr.value;
				
			} catch (Exception e ) {
				
				MessageBox.Show("链接WIMS服务器超时");
			}
			
		
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
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
			lvi.SubItems.Add(tu.sqpersonid);//申请人
			lvi.SubItems.Add(tu.ver);//版本信息
			lvi.SubItems.Add(tu.sqdate);//申请日期
			lvi.SubItems.Add(tu.zrpersonid);//责任人
			lvi.SubItems.Add(tu.Status);//状态
			lvi.SubItems.Add(tu.Id.ToString());//
			this.listView1.Items.Add(lvi);
		}
	}
}
