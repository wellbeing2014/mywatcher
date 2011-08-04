/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-8-4
 * 时间: 16:15
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Net;
using System.IO;
using WatchCilent.Common;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WatchCilent.UI.Pub
{
	/// <summary>
	/// Description of SelectPath.
	/// </summary>
	public partial class SelectPath : Form
	{
		private string ftphost = System.Configuration.ConfigurationManager.AppSettings["FTPHOST"];
		private string username = System.Configuration.ConfigurationManager.AppSettings["FTPID"];
		private string password = System.Configuration.ConfigurationManager.AppSettings["FTPPWD"];
		
		public SelectPath()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			TreeNode root = new TreeNode();
			root.Text = "服务器根";
			
			string[] list=this.ListDirectory("");
			if(list!=null)
			{
				foreach (var element in list) {
				TreeNode temp = new TreeNode(element);
				temp.Nodes.Add(new TreeNode());
				root.Nodes.Add(temp);
				}
			}
			this.treeView1.Nodes.Add(root);
			this.treeView1.Nodes[0].Expand();
			this.treeView1.AfterExpand+=new TreeViewEventHandler(treeView1_AfterExpand);
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private string  getfullpath(TreeNode tn)
		{
			string path = tn.Text;
			if(tn.Parent!=null&&!tn.Parent.Text.Equals("服务器根"))
			{
				path=getfullpath(tn.Parent)+@"/"+path;
			}
			return path;
		}
		private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode atn =e.Node;
			
			if(atn.Nodes.Count==1&&string.IsNullOrEmpty(atn.Nodes[0].Text))
			{
				atn.Nodes.RemoveAt(0);
				string[] list=this.ListDirectory( getfullpath(atn));
				if(list!=null)
				{
					foreach (var element in list) {
					TreeNode temp = new TreeNode(element);
					temp.Nodes.Add(new TreeNode());
					atn.Nodes.Add(temp);
					}
				}
			}
		}
		TreeNode[] Createtree(string path)
		{
			List<TreeNode> tn = new List<TreeNode>();
			string[] list=this.ListDirectory(path);
			if(list!=null)
			{
				foreach (var element in list) {
				TreeNode temp = new TreeNode(element);
				foreach (var element1 in Createtree(path+"/"+element)) {
					temp.Nodes.Add(element1);
				}
				tn.Add(temp);
			}
			}
			
			return tn.ToArray();
		}
		
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
	}
}
