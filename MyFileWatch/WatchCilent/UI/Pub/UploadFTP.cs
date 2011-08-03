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

namespace WatchCilent.UI.Pub
{
	/// <summary>
	/// Description of UploadFTP.
	/// </summary>
	public partial class UploadFTP : Form
	{
		//文件大小
		private int  totalSize= 0;
		//已读取的位置
		private int  position = 0;
		private FtpWebRequest reqFTP;
		//上传数据流
		private Stream strm = null;
		//本地文件流
		private FileStream fs = null;
		//读取区间大小
		private int buffLength = 2048;
		private byte[] buffer;
		private string ftphost = System.Configuration.ConfigurationManager.AppSettings["FTPHOST"];
		private string username = System.Configuration.ConfigurationManager.AppSettings["FTPID"];
		private string password = System.Configuration.ConfigurationManager.AppSettings["FTPPWD"];
		
		public UploadFTP()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
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
		//上传
		private void Upload(string filename,string serverpath)
		{
		    FileInfo fileInf = new FileInfo(filename);
		    string uri = this.ftphost+serverpath+fileInf.Name;
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
		    this.totalSize = (int)fileInf.Length;
		    // 缓冲大小设置为2kb
		    buffer = new byte[buffLength];
		    // 打开一个文件流 (System.IO.FileStream) 去读上传的文件
		    fs = fileInf.OpenRead();
		    strm = reqFTP.GetRequestStream(); 
			fs.BeginRead(buffer, 0, buffLength, new AsyncCallback( AsyncCopyFile ), null);
		}
		
		 /// <summary>
        ///异步复制文件
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncCopyFile(IAsyncResult ar)
        {
        	//Invoke(new System.DeleSynchProgressBar);
            int readedLength;
            // 更新progressBar1
	        MethodInvoker m = SynchProgressBar;
            try {
            	  //判断stream是否可读（是否已被关掉）
	            if (fs.CanRead)
	            { 
	                // 锁定 FileStream
	                lock (fs)
	                {
	                    readedLength = fs.EndRead(ar); // 等到挂起的异步读取完成
	                }
	            }
	            else
	            {
	            	m.BeginInvoke(null, null);
	                return;
	            }
	            strm.Write(buffer, 0, buffer.Length);
	            // 当前位置 
	            position += readedLength;
	            m.BeginInvoke(null,null);
	            if (position >= totalSize) // 读取完毕
	            {
	                strm.Close(); //关闭
	                fs.Close();
	                return;
	            }
	            if (fs.CanRead)   //
	            {
	                lock (fs)
	                {
	                    int leftSize = totalSize - position;
	                    if (leftSize < buffLength)
	                        buffer = new byte[leftSize];
	                    fs.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(AsyncCopyFile), null);
	                }
	            }
	        }
          	catch(Exception e)
          	{
          		MessageBox.Show(e.ToString(),"ERROR");
          	}
	    }
		private delegate void SetStatus();
		private void SynchProgressBar()
        {
        	if (progressBar1.InvokeRequired)
            {
                SetStatus s = SynchProgressBar;
                Invoke(s);
            }
        	else
        	{
        		progressBar1.Maximum = totalSize;
            	progressBar1.Value = position;
        	}
         }
		#endregion 操作FTP
		
		//上传按钮
		void Button1Click(object sender, EventArgs e)
		{
		//	MakeDirectory("aaaaaa/bbbb/cccc");
			this.Upload(@"C:\Documents and Settings\Administrator\My Documents\滨湖网上政务大厅(WBS_BH2.4).rar","qlyg/");
		}


		//关闭按钮
		void Button2Click(object sender, EventArgs e)
		{
			this.Close();
			this.Dispose();
		}
	}
}
