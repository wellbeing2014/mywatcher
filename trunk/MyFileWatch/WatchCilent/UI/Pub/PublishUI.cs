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
using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;
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
		string WisofServiceHost = System.Configuration.ConfigurationManager.AppSettings["WisofServiceHost"];
		private int currentpage=0;
		private int count = 0;
		private int pagesize = 20;
		
		private string currentstr = "当前第{0}页";
		private string countstr ="共{0}页/共{1}条";
		private string pagestr ="每页{0}条";
		
		public PublishUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			TreeNode root1 = new TreeNode();
			root1.Text = "未发布";
			TreeNode root = new TreeNode();
			root.Text = ftphost;
			string[] list=this.ListDirectory("/");
			if(list!=null)
			{
				foreach (var element in list) {
					TreeNode temp = new TreeNode(element);
					temp.Nodes.Add(new TreeNode());
					root.Nodes.Add(temp);
				}
			}
			this.treeView1.Nodes.Add(root1);
			this.treeView1.Nodes.Add(root);
			this.treeView1.Nodes[1].Expand();
			this.treeView1.AfterExpand+=new TreeViewEventHandler(treeView1_AfterExpand);
			this.treeView1.NodeMouseClick+= new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
			this.treeView1.Leave+=new EventHandler(treeView1_Leave);
			this.treeView1.BeforeSelect+=new TreeViewCancelEventHandler(treeView1_BeforeSelect);
			
			
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
			
			this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,(count%pagesize==0)?count/pagesize:count/pagesize+1,this.count);
			getPublishPackageList();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private void getPublishPackageList()
		{
			this.count = PackageDao.queryPackageInfoCount("0","0","已发布",null,null);
			int countpage = (count%pagesize==0)?count/pagesize:count/pagesize+1;
			if(this.currentpage>countpage) this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,countpage,this.count);
			List<PackageInfo> ls =PackageDao.queryPackageInfo("0","0","已发布",null,null,
			                                                  (currentpage>1)?((this.currentpage-1)*pagesize):0
			                                                  ,pagesize);		
			this.exListView1.BeginUpdate();
			this.exListView1.Items.Clear();
			foreach(PackageInfo pack in ls)
			{
				ListViewBing(pack);
			}
			this.exListView1.EndUpdate();
		}
		
		private void ListViewBing(PackageInfo packinfo)
		{
			//添加第二列控件    更新包名称     item[0]
			EXListViewItem item = new EXListViewItem(packinfo.Packagename);
			//添加第二列控件    上传路径       item[1]
			EXControls.EXListViewSubItem serverpath=new EXControls.EXListViewSubItem();
			//添加第三列控件   进度            item[2]
			EXControlListViewSubItem cs = new EXControlListViewSubItem();
			ProgressBar b = new ProgressBar();
			b.Tag = item;
			//添加第四列控件   状态            item[3]
			EXControls.EXListViewSubItem status=new EXControls.EXListViewSubItem();
			//添加第五列控件   操作             item[4]
			EXControlListViewSubItem cs1 = new EXControlListViewSubItem();
			LinkLabel llbl = new LinkLabel();
			llbl.Tag = cs;
			llbl.LinkClicked += new LinkLabelLinkClickedEventHandler(llbl_LinkClicked);
			
			if(!string.IsNullOrEmpty(packinfo.PubPath)&&CommonConst.PACKSTATE_YiFaBu.Equals(packinfo.State))
			{
				serverpath.Text=packinfo.PubPath;
				status.Text="已上传";
				b.Maximum =100;
				b.Value =100;
				llbl.Text = "重传";
			}
			else
			{
				status.Text="未上传";
				llbl.Text = "上传";
			}
			item.SubItems.Add(serverpath);
			item.SubItems.Add(cs);
			item.SubItems.Add(status);
			item.SubItems.Add(cs1);
			item.SubItems.Add(packinfo.Id.ToString());
			item.SubItems.Add(packinfo.Packagepath);
			this.exListView1.AddControlToSubItem(b, cs);
			this.exListView1.AddControlToSubItem(llbl, cs1);
			this.exListView1.Items.Add(item);
		}
		
		private void llbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			
			LinkLabel l = (LinkLabel) sender;
			if (!l.Enabled) return;
			EXControlListViewSubItem subitem = l.Tag as EXControlListViewSubItem;
			ProgressBar p = subitem.MyControl as ProgressBar;
			ListViewItem item = (ListViewItem) p.Tag;
			string serverpath =item.SubItems[1].Text;
			if(string.IsNullOrEmpty(serverpath))
			{
				if(this.treeView1.SelectedNode!=null&&this.treeView1.SelectedNode!=this.treeView1.Nodes[0])
				{
					serverpath = this.treeView1.SelectedNode.FullPath.Replace('\\','/');
					item.SubItems[1].Text =serverpath;
				}
				else
				{
					MessageBox.Show("请选择服务器路径","提示");
					return;
				}
			}
			
			Thread th = new Thread(new ParameterizedThreadStart(UpdateProgressBarMethod));
			th.IsBackground = true;
			
			UploadParam up =new UploadParam();
			up.PackPath=item.SubItems[6].Text;//@"D:\111.txt";
			up.Bar= p;
			up.ServerPath=serverpath;
			th.Start(up);
			l.Enabled = false;
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
			 if(pvalue!=pmax)
			 {
			 	item.SubItems[3].Text = "正在上传";
			 }
			 else{
			 	item.SubItems[3].Text="上传完成";
			 	SaveInList(item);
			 }
		}
		private void SaveInList(ListViewItem item)
		{
			string id = item.SubItems[5].Text.ToString();
			string path = item.SubItems[1].Text.ToString();
			PackageDao.updateForPub(id,path);
		}
		
		private void ChangeTextMethod(LinkLabel l, string text) {
			if(text.Equals("重传"))
			{
				l.Enabled=true;
			}
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
		
		private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode atn =e.Node;
			string fullpath = System.Text.RegularExpressions.Regex.Replace(atn.FullPath,ftphost,"");
			if(atn.Nodes.Count==1&&string.IsNullOrEmpty(atn.Nodes[0].Text))
			{
				atn.Nodes.RemoveAt(0);
				string[] list=this.ListDirectory(fullpath);
				if(list!=null)
				{
					foreach (var element in list) {
					TreeNode temp = new TreeNode(element);
					temp.Nodes.Add(new TreeNode());
					atn.Nodes.Add(temp);
					}
				}else{
				}
			}
		}
		
		//新建目录
		private bool MakeDirectory(string uristring)
		{
			try {
				Uri uri = new Uri ( uristring );
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
		
		//删除目录
		private bool DeleteDirectory(string uristring)
		{
			try {
				Uri uri = new Uri ( uristring );
				FtpWebRequest listRequest = ( FtpWebRequest ) WebRequest.Create ( uri );
				listRequest.Credentials = new NetworkCredential ( username , password );
				listRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
				FtpWebResponse listResponse = ( FtpWebResponse )listRequest.GetResponse();
				if(listResponse.StatusCode== FtpStatusCode.FileActionOK)
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
			try {
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
					listResponse.Close();
					responseStream.Close();
					readStream.Close();
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
			} catch (Exception) {
				
				return new string[]{""};
			}
			
			
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
		
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs  e)
		{
			
		}
		 
        
		void Button1Click(object sender, EventArgs e)
		{
			if(this.treeView1.SelectedNode==null)
			{
				MessageBox.Show("请选择FTP目录");
				return;
			}
			string newpath =this.treeView1.SelectedNode.FullPath.Replace('\\','/');
			
			CreateNameForm cn = new CreateNameForm();
			cn.StartPosition=FormStartPosition.CenterParent;
			if(cn.ShowDialog()== DialogResult.OK)
			{
				newpath=newpath+"/"+cn.name;
				if(this.MakeDirectory(newpath))
				{
					TreeNode tn = new TreeNode();
					tn.Text = cn.name;
					this.treeView1.SelectedNode.Nodes.Add(tn);
					MessageBox.Show("创建文件夹成功！");
				}
				else
					MessageBox.Show("创建文件夹失败！");
			}
			else
				return;
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if(this.treeView1.SelectedNode==null)
			{
				MessageBox.Show("请选择FTP目录");
				return;
			}
			string newpath =this.treeView1.SelectedNode.FullPath.Replace('\\','/');
			if(this.DeleteDirectory(newpath))
			{
				this.treeView1.SelectedNode.Remove();
				MessageBox.Show("删除文件夹成功！");
			}
			else
				MessageBox.Show("删除文件夹失败！");
			
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			IPHostEntry MyEntry=Dns.GetHostByName(Dns.GetHostName());
           	IPAddress MyAddress=new IPAddress(MyEntry.AddressList[0].Address);
           	string ip = MyAddress.ToString();
			string msg ="今日发布\n";
			if(this.exListView1.SelectedItems.Count>0)
			{
				for (int i = 0; i < this.exListView1.SelectedItems.Count; i++) {
					string name = this.exListView1.SelectedItems[i].SubItems[0].Text;
					string path = this.exListView1.SelectedItems[i].SubItems[1].Text;
					msg+=(i+1).ToString()+"、";
					msg+=name+"\n";
					msg+="下载地址："+path.Insert(6,username+":"+password+"@")+"/"+name+".rar\n";
				}
				Communication.TCPManage.SendMessage(WisofServiceHost,msg+"##"+ip);
			}
			
			
		}
		
			
		
		void LinkLabel3LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentpage=1;
			getPublishPackageList();
		}
		
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(this.currentpage>1)
			{
				this.currentpage--;
			}
			getPublishPackageList();
		}
		
		void LinkLabel2LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(this.currentpage<((count%pagesize==0)?count/pagesize:count/pagesize+1))
			{
				this.currentpage++;
			}
			getPublishPackageList();
		}
		
		void LinkLabel4LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentpage=(count%pagesize==0)?count/pagesize:count/pagesize+1;
			getPublishPackageList();
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
