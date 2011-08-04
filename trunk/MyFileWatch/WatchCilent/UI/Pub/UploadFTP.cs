/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-8-3
 * 时间: 10:16
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using WatchCilent.Common;
using System.Collections.Generic;
using EXControls;
using System.Threading;
using System.Diagnostics;   

namespace WatchCilent.UI.Pub
{
	/// <summary>
	/// Description of UploadFTP.
	/// </summary>
	public partial class UploadFTP : Form
	{
		
		
		private string ftphost = System.Configuration.ConfigurationManager.AppSettings["FTPHOST"];
		private string username = System.Configuration.ConfigurationManager.AppSettings["FTPID"];
		private string password = System.Configuration.ConfigurationManager.AppSettings["FTPPWD"];
		
		public UploadFTP()
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
			this.exListView1.Columns.Add(new EXColumnHeader("状态", 80));
			
			this.exListView1.BeginUpdate();
			for (int i = 0; i < 100; i++) {
				//movie
				EXListViewItem item = new EXListViewItem(i.ToString());
				//添加第二列控件    上传路径
				item.SubItems.Add(new EXControls.EXListViewSubItem("服务器路径"+i.ToString()));
				EXControlListViewSubItem cs = new EXControlListViewSubItem();
				ProgressBar b = new ProgressBar();
				b.Tag = item;
//				b.Minimum = 0;
//				b.Maximum = 1000;
//				b.Step = 1;
				//添加第三列控件   进度
				item.SubItems.Add(cs);
				this.exListView1.AddControlToSubItem(b, cs);
				
				EXControlListViewSubItem cs1 = new EXControlListViewSubItem();
				LinkLabel llbl = new LinkLabel();
				llbl.Text = "上传";
				llbl.Tag = cs;
				llbl.LinkClicked += new LinkLabelLinkClickedEventHandler(llbl_LinkClicked);
				item.SubItems.Add(cs1);
				this.exListView1.AddControlToSubItem(llbl, cs1);
				//conclusion
				item.SubItems.Add(new EXBoolListViewSubItem(true));
				this.exListView1.Items.Add(item);
			}
			this.exListView1.EndUpdate();
			
			//ListDirectory("aaaabbb");
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		private void llbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {

			SelectPath sp = new SelectPath();
			sp.ShowDialog();
//			LinkLabel l = (LinkLabel) sender;
//			if (l.Text == "正在上传") return;
//			EXControlListViewSubItem subitem = l.Tag as EXControlListViewSubItem;
//			ProgressBar p = subitem.MyControl as ProgressBar;
//			Thread th = new Thread(new ParameterizedThreadStart(UpdateProgressBarMethod));
//			th.IsBackground = true;
//			th.Start(p);
//			((LinkLabel) sender).Text = "正在上传";
			
			
		}
		
		private delegate void del_do_update(ProgressBar pb,int pvalue,int pmax);
		private delegate void del_do_changetxt(LinkLabel l, string text);
		private void UpdateProgressBarMethod(object pb) {
			ProgressBar pp = (ProgressBar) pb;
			string localfile =@"D:\111.txt";
			string serverpath="qlyg/";
			this.Upload1(localfile,serverpath,pp);
			
		}
	
		private void do_update(ProgressBar p,int pvalue,int pmax) {
			p.Minimum=0;
			p.Maximum=pmax;
			p.Value=pvalue;
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
		    string uri = this.ftphost+serverpath+fileInf.Name;
		    
		    ListViewItem item = (ListViewItem) pp.Tag;
			LinkLabel l = ((LinkLabel) ((EXControlListViewSubItem) item.SubItems[3]).MyControl);
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
				l.BeginInvoke(delchangetxt, new object[] {l, "上传完成"});
			} catch (Exception) {
				l.BeginInvoke(delchangetxt, new object[] {l, "上传出错"});
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
		
		
		#region 操作FTP
		//新建目录
		private bool MakeDirectory(string uristring)
		{
			try {
				Uri uri = new Uri ( ftphost+uristring );
				FtpWebRequest listRequest = ( FtpWebRequest ) WebRequest.Create ( uri );
				listRequest.Credentials = new NetworkCredential ( username , password );
				listRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
				FtpWebResponse listResponse = ( FtpWebResponse )listRequest.GetResponse();
				if(listResponse.StatusCode== FtpStatusCode.PathnameCreated)
				{
					listResponse.Close();
					return true;
				}
				else
				{
					listResponse.Close();
					return false;
				}
			} catch (Exception) {
				
				return false;
			}
			
		}
		//列出目录
		private string[] ListDirectory(string uristring)
		{
			Uri uri = new Uri ( ftphost+uristring );
			FtpWebRequest listRequest = ( FtpWebRequest ) WebRequest.Create ( uri );
			listRequest.Credentials = new NetworkCredential ( username , password );
			listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
			FtpWebResponse listResponse = ( FtpWebResponse )listRequest.GetResponse();
			Stream responseStream = listResponse.GetResponseStream ( );
			StreamReader readStream = new StreamReader ( responseStream , System.Text.Encoding.Default );
			if ( readStream != null )
			{
    			DirectoryListParser parser = new DirectoryListParser ( readStream.ReadToEnd() );
				FileStruct[] fs = parser.FullListing;
				List<string> returns = new List<string>();
				foreach (FileStruct element in fs) {
					if(element.IsDirectory){
						returns.Add(element.Name);
					}
				}
				if(returns.Count>0)
				{
					return returns.ToArray();
				}
				else return null;
			}
			listResponse.Close();
			responseStream.Close();
			readStream.Close();
			return null;
		}
		

		#endregion 操作FTP
		
		//上传按钮
		void Button1Click(object sender, EventArgs e)
		{
		//	MakeDirectory("aaaaaa/bbbb/cccc");
		//	this.Upload(@"C:\Documents and Settings\Administrator\My Documents\滨湖网上政务大厅(WBS_BH2.4).rar","qlyg/");
		}


		//关闭按钮
		void Button2Click(object sender, EventArgs e)
		{
			this.Close();
			this.Dispose();
		}
	}
}
