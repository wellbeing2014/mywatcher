/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011-8-7
 * 时间: 21:54
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WatchCilent.pojo;
using WatchCilent.dao;
using EXControls;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.IO;

namespace WatchCilent.UI.Pub
{
	/// <summary>
	/// Description of PublishUI.
	/// </summary>
	public partial class PublishUI : UserControl
	{
		private string ftphost = System.Configuration.ConfigurationManager.AppSettings["FTPHOST"];
		private string username = System.Configuration.ConfigurationManager.AppSettings["FTPID"];
		private string password = System.Configuration.ConfigurationManager.AppSettings["FTPPWD"];
		public PublishUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.exListView1.MySortBrush = SystemBrushes.ControlLight;
			this.exListView1.MyHighlightBrush = Brushes.Goldenrod;
			this.exListView1.GridLines = true;
			this.exListView1.ControlPadding = 4;
			
			ImageList colimglst = new ImageList();
			colimglst.ImageSize = new Size(1, 25); // this will affect the row height
			this.exListView1.SmallImageList = colimglst;
			
			this.exListView1.Columns.Add(new EXColumnHeader("更新包名称", 200));
			this.exListView1.Columns.Add(new EXEditableColumnHeader("上传路径", 180));
			this.exListView1.Columns.Add(new EXColumnHeader("进度", 120));
			this.exListView1.Columns.Add(new EXColumnHeader("状态", 60));
			this.exListView1.Columns.Add(new EXColumnHeader("操作", 80));
			
			List<PackageInfo> ls =PackageDao.queryPackageInfo("0","0","已发布",null,null);
			
			
			this.exListView1.BeginUpdate();
			this.exListView1.Items.Clear();
			foreach(PackageInfo pack in ls)
			{
				ListViewBing(pack);
			}
			this.exListView1.EndUpdate();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private void ListViewBing(PackageInfo packinfo)
		{
			//movie
				EXListViewItem item = new EXListViewItem(packinfo.Packagename);
				//添加第二列控件    上传路径
				EXControls.EXListViewSubItem serverpath=new EXControls.EXListViewSubItem();
				item.SubItems.Add(serverpath);
				//添加第三列控件   进度
				EXControlListViewSubItem cs = new EXControlListViewSubItem();
				ProgressBar b = new ProgressBar();
				b.Tag = item;
				b.Maximum =100;
				b.Value =100;
				item.SubItems.Add(cs);
				this.exListView1.AddControlToSubItem(b, cs);
				//添加第四列控件   状态
				EXControls.EXListViewSubItem status=new EXControls.EXListViewSubItem();
				item.SubItems.Add(status);
				//添加第五列控件   操作
				EXControlListViewSubItem cs1 = new EXControlListViewSubItem();
				LinkLabel llbl = new LinkLabel();
				llbl.Text = "上传";
				llbl.Tag = cs;
				llbl.LinkClicked += new LinkLabelLinkClickedEventHandler(llbl_LinkClicked);
				item.SubItems.Add(cs1);
				this.exListView1.AddControlToSubItem(llbl, cs1);
			
				//conclusion
				//item.SubItems.Add(new EXBoolListViewSubItem(true));
				this.exListView1.Items.Add(item);
		}
		
		private void llbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			
			LinkLabel l = (LinkLabel) sender;
			EXControlListViewSubItem subitem = l.Tag as EXControlListViewSubItem;
			ProgressBar p = subitem.MyControl as ProgressBar;
			ListViewItem item = (ListViewItem) p.Tag;
			string serverpath =item.SubItems[1].Text;
			if(string.IsNullOrEmpty(serverpath))
			{
				SelectPath sp = new SelectPath();
				sp.StartPosition=FormStartPosition.CenterParent;
				if(sp.ShowDialog()==DialogResult.OK)
				{
					serverpath =sp.selpath;
					
					item.SubItems[1].Text =serverpath;
					sp.Dispose();
				}
				else
				{
					sp.Dispose();
					return ;
				}
			}
			
			if (l.Text == "正在上传") return;
			Thread th = new Thread(new ParameterizedThreadStart(UpdateProgressBarMethod));
			th.IsBackground = true;
			
			UploadParam up =new UploadParam();
			up.PackPath=@"D:\111.txt";
			up.Bar= p;
			up.ServerPath=serverpath;
			th.Start(up);
			((LinkLabel) sender).Text = "正在上传";
		}
		
		private delegate void del_do_update(ProgressBar pb,int pvalue,int pmax);
		private delegate void del_do_changetxt(LinkLabel l, string text);
		private void UpdateProgressBarMethod(object param) {
			UploadParam up =(UploadParam)param ;
			
			ProgressBar pp =  (ProgressBar)up.Bar;
			string packfile = up.PackPath;
			string serverpath = up.ServerPath+"/";
			this.Upload1(packfile,serverpath,pp);
			
		}
	
		private void do_update(ProgressBar p,int pvalue,int pmax) {
			p.Minimum=0;
			p.Maximum=pmax;
			p.Value=pvalue;
			 ListViewItem item = (ListViewItem) p.Tag;
			 item.SubItems[3].Text=pvalue+"/"+pmax;
		}
		private void ChangeTextMethod(LinkLabel l, string text) {
			l.Text = text;
		}
		
		private void Upload1(string filename,string serverpath,ProgressBar pp)
		{
			//读取区间大小
			int buffLength = 2048;
			byte[] buffer;
			del_do_update delupdate = new del_do_update(do_update);
			FtpWebRequest reqFTP;
			FtpWebResponse	uploadResponse=null ;
			//上传数据流
			Stream strm = null;
			//本地文件流
			FileStream fs = null;
		    FileInfo fileInf = new FileInfo(filename);
		    string uri = serverpath+fileInf.Name;
		    
		    ListViewItem item = (ListViewItem) pp.Tag;
			LinkLabel l = ((LinkLabel) ((EXControlListViewSubItem) item.SubItems[4]).MyControl);
			del_do_changetxt delchangetxt = new del_do_changetxt(ChangeTextMethod);
			
			try {
				// 根据uri创建FtpWebRequest对象 
			    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri)); 
			    // ftp用户名和密码
			    reqFTP.Credentials = new NetworkCredential(this.username, this.password); 
			    // 默认为true，连接不会被关闭
			    // 在一个命令之后被执行
			    reqFTP.KeepAlive = false; 
			    // 指定执行什么命令
			    reqFTP.Method = WebRequestMethods.Ftp.UploadFile; 
			    // 指定数据传输类型
			    reqFTP.UseBinary = true; 
			    // 上传文件时通知服务器文件的大小
			    reqFTP.ContentLength = fileInf.Length;
				//pp.Value=0;		    
			    //pp.Maximum = (int)fileInf.Length;
			    // 缓冲大小设置为2kb
			    buffer = new byte[buffLength];
			    // 打开一个文件流 (System.IO.FileStream) 去读上传的文件
			    fs = fileInf.OpenRead();
			    strm = reqFTP.GetRequestStream(); 
			    int bytesRead;
				int hasread=0;		    
				while (true) 
				{ 
					bytesRead = fs.Read(buffer, 0, buffer.Length);
					hasread+=bytesRead;
					if (bytesRead == 0) 
						break; 
					strm.Write(buffer, 0, bytesRead);
					pp.BeginInvoke(delupdate,new object[]{pp,hasread,(int)fileInf.Length});
				} 
				fs.Close();
				strm.Close();
				uploadResponse = (FtpWebResponse)reqFTP.GetResponse(); 
				l.BeginInvoke(delchangetxt, new object[] {l, "重传"});
			} catch (Exception) {
				l.BeginInvoke(delchangetxt, new object[] {l, "重传"});
			}
		    finally
		    {
		    	if (uploadResponse!= null) 
				uploadResponse.Close(); 
				if (fs != null) 
				fs.Close(); 
				if (strm != null) 
				strm.Close(); 
		    }
		}
		
	}
	class UploadParam{
	private ProgressBar bar;
	public ProgressBar Bar {
		get { return bar; }
		set { bar = value; }
	}
	private string serverPath; 
	public string ServerPath {
		get { return serverPath; }
		set { serverPath = value; }
	}
	private string packPath;
	public string PackPath {
		get { return packPath; }
		set { packPath = value; }
	}
	}
}
