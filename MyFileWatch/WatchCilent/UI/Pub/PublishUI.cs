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
	public partial class PublishUI : UserControl,UI.MainPlug
	{
		private string ftphost = GlobalParams.FTPHOST;
		private string username = GlobalParams.FTPID;
		private string password = GlobalParams.FTPPWD;
		string WisofServiceHost = GlobalParams.WisofServiceHost;
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
			return "3,4,5";
		}
		
		public string[] getPlugName()
		{
			return new string[]{"更新包","更新包发布"};
		}
		
		public PublishUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
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
			
			//----------------------------------
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
			this.treeView1.Nodes.Add("今日发布");
			this.treeView1.Nodes.Add(root);
			this.treeView1.Nodes[1].Expand();
			this.treeView1.AfterExpand+=new TreeViewEventHandler(treeView1_AfterExpand);
			this.treeView1.NodeMouseClick+= new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
			this.treeView1.Leave+=new EventHandler(treeView1_Leave);
			this.treeView1.BeforeSelect+=new TreeViewCancelEventHandler(treeView1_BeforeSelect);
			treeView1.SelectedNode=treeView1.Nodes[0];
			//让选中项背景色呈现蓝色
            treeView1.SelectedNode.BackColor = Color.SteelBlue;
            //前景色为白色
            treeView1.SelectedNode.ForeColor = Color.White;
			
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
			this.exListView1.Columns.Add(new EXColumnHeader("状态", 150));
			this.exListView1.Columns.Add(new EXColumnHeader("操作", 45));
			
			this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,(count%pagesize==0)?count/pagesize:count/pagesize+1,this.count);
			
			System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
			
			
			getPublishPackageList();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private void getPublishPackageList()
		{
			string _moduleid="0" ;
			string _managerid="0";
			
			string _pubpath=null;
			string _begin = null;
			string _end = null;
			string _keyword=null;
			
			if(!this.dateTimePicker1.IsDisposed&&!this.dateTimePicker2.IsDisposed)
			{
				_begin = this.dateTimePicker1.Value.ToShortDateString()+" 00:00:00";
				_end = this.dateTimePicker2.Value.ToShortDateString()+" 23:59:59";
			}
			
			if(this.treeView1.SelectedNode!=null)
			{
					_pubpath=this.treeView1.SelectedNode.FullPath.Replace('\\','/');
					if(!ftphost.Equals(_pubpath))
						_pubpath = _pubpath.Replace(ftphost+"/","");
					else _pubpath = null;
			}
			else
			{
				MessageBox.Show("请选择分类！");
				return;
			}
			
			_moduleid = this.comboBox1.SelectedValue.ToString();
			_managerid = this.comboBox2.SelectedValue.ToString();
			if(!string.IsNullOrEmpty(this.textBox1.Text)&&!"请输入关键字...".Equals(this.textBox1.Text))
			{
				_keyword=textBox1.Text;
			}
			this.count = PackageDao.queryPubPackageInfoCount(_moduleid,_managerid,_pubpath,_keyword,_begin,_end);
			int countpage = (count%pagesize==0)?count/pagesize:count/pagesize+1;
			if(this.currentpage>countpage) this.currentpage=1;
			this.label3.Text=string.Format(currentstr,this.currentpage);
			this.label5.Text = string.Format(pagestr,this.pagesize);
			this.label4.Text = string.Format(countstr,countpage,this.count);
			List<PackageInfo> ls =PackageDao.queryPubPackageInfo(_moduleid,_managerid,_pubpath,_keyword,_begin,_end,
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
			item.Tag="true";//默认是 确认上传
			this.exListView1.AddControlToSubItem(b, cs);
			this.exListView1.AddControlToSubItem(llbl, cs1);
			this.exListView1.Items.Add(item);
		}
		
		private void llbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			
			LinkLabel l = (LinkLabel) sender;
			EXControlListViewSubItem subitem = l.Tag as EXControlListViewSubItem;
			ProgressBar p = subitem.MyControl as ProgressBar;
			ListViewItem item = (ListViewItem) p.Tag;
			string serverpath =item.SubItems[1].Text;
			
			if (!l.Text.Equals("上传")&&!l.Text.Equals("重传")) 
			{
				item.Tag = "false";
			}
			else 
			{
				item.Tag = "true";
				if(string.IsNullOrEmpty(serverpath))
				{
					System.DateTime today = System.DateTime.Now;
					string todaystr  = today.ToString("yyyyMMdd");
					DialogResult mr = MessageBox.Show("是否使用当前日期“"+todaystr+"“作为文件夹?","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
					if(DialogResult.Yes==mr)
					{
						item.SubItems[1].Text =todaystr;
						serverpath = todaystr;
					}
					else return;
				}
				
				Thread th = new Thread(new ParameterizedThreadStart(UpdateProgressBarMethod));
				th.IsBackground = true;
				
				UploadParam up =new UploadParam();
				up.PackPath=item.SubItems[6].Text;//@"D:\111.txt";
				up.Bar= p;
				up.ServerPath=serverpath;
				th.Start(up);
				l.Text = "取消";
			}
					
		}
		
		private delegate void del_do_update(ProgressBar pb,int pvalue,int pmax,int curnum,int sum,string prjname);
		private delegate void del_do_changetxt(LinkLabel l, string text);
		private void UpdateProgressBarMethod(object param) {
			UploadParam up =(UploadParam)param ;
			
			ProgressBar pp =  (ProgressBar)up.Bar;
			string packfile = up.PackPath;
			string serverpath = up.ServerPath+"/";
			this.Upload1(packfile,serverpath,pp);
			
		}
		/// <summary>
		/// 上传时同步UI进度条方法
		/// </summary>
		/// <param name="p">进度条控件对象</param>
		/// <param name="pvalue">进度条当前值</param>
		/// <param name="pmax">进度条最大值</param>
		/// <param name="curnum">当前上传的第几个项目</param>
		/// <param name="sum">总的项目个数</param>
		/// <param name="prjname">项目名称</param>
		private void do_update(ProgressBar p,int pvalue,int pmax,int curnum,int sum,string prjname) {
			p.Minimum=0;
			p.Maximum=pmax*sum;
			p.Value=pmax*(curnum-1)+pvalue;
			 ListViewItem item = (ListViewItem) p.Tag;
			 if(p.Value!=p.Maximum)
			 {
			 	item.SubItems[3].Text = string.Format("正在上传至{0}({1}/{2})",prjname,curnum,sum);
			 }
			 else{
			 	item.SubItems[3].Text=string.Format("上传完成({0}/{1})",curnum,sum);
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
		
		/// <summary>
		/// 上传具体方法，根据关联的项目数循环上传至FTP
		/// </summary>
		/// <param name="filename">被上传的文件路径</param>
		/// <param name="serverpath">上传的项目所在的相对路径</param>
		/// <param name="pp">进度条控件</param>
		private void Upload1(string filename,string serverpath,ProgressBar pp)
		{
			//读取区间大小
			int buffLength = 2048;
			byte[] buffer;
			FtpWebRequest reqFTP;
			FtpWebResponse	uploadResponse=null ;
			//上传数据流
			Stream strm = null;
			//本地文件流
			FileStream fs = null;
			
			//获取文件信息
		    FileInfo fileInf = new FileInfo(filename);
			try {
				 if(!fileInf.Exists)
			    {
				 	MessageBox.Show("文件不存在","提示");
				 	return;
			    }
			} catch (Exception e1) {
				
		    	MessageBox.Show(e1.ToString(),"提示");
		    	return ;
			}		   
		   
		   
		    ListViewItem item = (ListViewItem) pp.Tag;
		    
			LinkLabel l = ((LinkLabel) ((EXControlListViewSubItem) item.SubItems[4]).MyControl);
			del_do_update delupdate = new del_do_update(do_update);
			del_do_changetxt delchangetxt = new del_do_changetxt(ChangeTextMethod);
			
			//查询要上传至服务器的项目数
			List<ProjectInfo> _prjlist = new List<ProjectInfo>();
			if(!string.IsNullOrEmpty(item.SubItems[5].Text))
			{
				_prjlist =ProjectInfoDao.getAllProjectInfoByPackID(new string[]{item.SubItems[5].Text});
			}
			
			for (int i = 0; i < _prjlist.Count; i++) {
				
				if(!bool.Parse(item.Tag as string))//中断
				{
					break;
				}
				try {
					this.MakeDirectory(ftphost+_prjlist[i].Ftppath+serverpath);
					
				} catch (Exception) {
					MessageBox.Show("创建文件夹错误，可能目标文件夹已经存在","提示");
				}
				
				string uri =ftphost+_prjlist[i].Ftppath+serverpath+fileInf.Name;
				try {
					// 根据uri创建FtpWebRequest对象 
				    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri)); 
				    // ftp用户名和密码
				    reqFTP.Credentials = new NetworkCredential(this.username, this.password);
				    reqFTP.Proxy = GlobalProxySelection.GetEmptyWebProxy();
				    // 默认为true，连接不会被关闭
				    // 在一个命令之后被执行
				    reqFTP.KeepAlive = false; 
				    // 指定执行什么命令
				    reqFTP.Method = WebRequestMethods.Ftp.UploadFile; 
				    // 指定数据传输类型
				    reqFTP.UseBinary = true; 
				    // 上传文件时通知服务器文件的大小
				    reqFTP.ContentLength = fileInf.Length;
				    // 缓冲大小设置为2kb
				    buffer = new byte[buffLength];
				    // 打开一个文件流 (System.IO.FileStream) 去读上传的文件
				    fs = fileInf.OpenRead();
				    strm = reqFTP.GetRequestStream(); 
				    int bytesRead;
					int hasread=0;
					pp.BeginInvoke(delupdate,new object[]{pp,hasread,(int)fileInf.Length,i+1,_prjlist.Count,_prjlist[i].Projectname});					
					while (bool.Parse(item.Tag as string))//中断
					{ 
						//Thread.Sleep(300);
						bytesRead = fs.Read(buffer, 0, buffer.Length);
						hasread+=bytesRead;
						if (bytesRead == 0) 
							break; 
						strm.Write(buffer, 0, bytesRead);
						pp.BeginInvoke(delupdate,new object[]{pp,hasread,(int)fileInf.Length,i+1,_prjlist.Count,_prjlist[i].Projectname});
					} 
					fs.Close();
					strm.Close();
					uploadResponse = (FtpWebResponse)reqFTP.GetResponse(); 
				} catch (Exception e) {
					MessageBox.Show("上传到"+uri+"发生错误:"+e.ToString(),"提示");
					pp.BeginInvoke(delupdate,new object[]{pp,(int)fileInf.Length,(int)fileInf.Length,i+1,_prjlist.Count,_prjlist[i].Projectname});
				}
			}
			l.BeginInvoke(delchangetxt, new object[] {l, "重传"});
		    
	    	if (uploadResponse!= null) 
			uploadResponse.Close(); 
			if (fs != null) 
			fs.Close(); 
			if (strm != null) 
			strm.Close(); 
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
		
		
		//复制文件
		private bool CopyFile(string uristring,string target)
		{
			try {
				Uri uri = new Uri ( uristring );
				FtpWebRequest listRequest = ( FtpWebRequest ) WebRequest.Create ( uri );
				listRequest.Credentials = new NetworkCredential ( username , password );
				listRequest.Method = WebRequestMethods.Ftp.Rename;
				listRequest.Proxy = GlobalProxySelection.GetEmptyWebProxy();
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
		
		//新建目录
		private bool MakeDirectory(string uristring)
		{
			try {
				Uri uri = new Uri ( uristring );
				FtpWebRequest listRequest = ( FtpWebRequest ) WebRequest.Create ( uri );
				listRequest.Credentials = new NetworkCredential ( username , password );
				listRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
				listRequest.Proxy = GlobalProxySelection.GetEmptyWebProxy();
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
				listRequest.Proxy = GlobalProxySelection.GetEmptyWebProxy();
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
				listRequest.Proxy = GlobalProxySelection.GetEmptyWebProxy();
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
			} catch (Exception e) {
				
				MessageBox.Show(e.ToString(),"FTP访问错误");
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
			this.treeView1.SelectedNode=e.Node;
			getPublishPackageList();
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
				string[] packidarray = new string[this.exListView1.SelectedItems.Count];
				for (int i = 0; i < this.exListView1.SelectedItems.Count; i++) {
					string name = this.exListView1.SelectedItems[i].SubItems[0].Text;
					string path = this.exListView1.SelectedItems[i].SubItems[1].Text;
					msg+=(i+1).ToString()+"、";
					msg+=name+"\n";
					packidarray[i] = this.exListView1.SelectedItems[i].SubItems[5].Text;
				}
				msg+="需要更新的项目包括:\n";
				List<ProjectInfo> prolist = ProjectInfoDao.getAllProjectInfoByPackID(packidarray);
				for(int j = 0;j<prolist.Count;j++)
				{
					msg+= (j+1).ToString()+"、";
					msg+=prolist[j].Projectname+":"+ftphost.Insert(6,username+":"+password+"@")+"/"+prolist[j].Ftppath+"\n";
				}
				//MessageBox.Show(msg);
				Clipboard.SetDataObject(msg,true);
				MessageBox.Show("发布信息成功复制到剪贴板","提示");
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
		
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			Point cb1p=new Point();
			cb1p=this.comboBox1.Location;
			Point cb2p=new Point();
			cb2p=this.comboBox2.Location;
			Point cb3p=new Point();
			cb3p=this.textBox1.Location;
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
				this.textBox1.Location=cb3p;
			}
			else
			{
			
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.dateTimePicker2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(91, 3);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(106, 21);
			System.DateTime dt =System.DateTime.Now; 
				dateTimePicker1.Value=dt.AddDays(-7);
			this.dateTimePicker1.TabIndex = 45;
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.Location = new System.Drawing.Point(226, 3);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(107, 21);
			this.dateTimePicker2.TabIndex = 46;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(66, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(19, 18);
			this.label1.TabIndex = 47;
			this.label1.Text = "起";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(203, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(17, 18);
			this.label2.TabIndex = 48;
			this.label2.Text = "止";
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(476, 3);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(89, 20);
			this.comboBox2.TabIndex = 44;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(349, 3);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 20);
			this.comboBox1.TabIndex = 43;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(574, 4);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(160, 21);
			this.textBox1.TabIndex = 53;
			this.textBox1.Text = "请输入关键字...";
			cb1p.X=cb1p.X+250;
			cb2p.X=cb2p.X+250;
			cb3p.X=cb3p.X+250;
			this.comboBox1.Location=cb1p;
			this.comboBox2.Location=cb2p;
			this.textBox1.Location=cb3p;
			this.textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
			this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
			this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			}
			getPublishPackageList();
		}
		
		private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                SendKeys.Send("{TAB}");            
            }
        }
		
		/// <summary>
		/// 所有条件变化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void conditionChanged(object sender, EventArgs e)
		{
			getPublishPackageList();
		}
		
		/// <summary>
		/// 同步WIMS
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button4Click(object sender, EventArgs e)
		{
			if(this.exListView1.SelectedItems.Count>0)
			{
				List<string> versions = new List<string>();
				for (int i = 0; i < this.exListView1.SelectedItems.Count; i++) {
					string name = this.exListView1.SelectedItems[i].SubItems[0].Text;
					versions.Add(name);
				}
				UpdateWims uw = new UpdateWims(versions.ToArray());
				uw.StartPosition = FormStartPosition.CenterParent;
				uw.ShowDialog();
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
