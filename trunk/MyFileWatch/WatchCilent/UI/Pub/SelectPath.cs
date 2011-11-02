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
using WatchCore.Common;
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
		public string selpath;
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
			this.treeView1.Nodes.Add(root);
			this.treeView1.Nodes[0].Expand();
			this.treeView1.AfterExpand+=new TreeViewEventHandler(treeView1_AfterExpand);
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
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
				if(listResponse.StatusCode== FtpStatusCode.DirectoryStatus)
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
		//新建
		void Button3Click(object sender, EventArgs e)
		{
			string temp =this.treeView1.SelectedNode.FullPath.Replace('\\','/');
			string newpath = System.Text.RegularExpressions.Regex.Replace(
				temp,"服务器根目录",ftphost);
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
		//选中
		void Button1Click(object sender, EventArgs e)
		{
			string temp =this.treeView1.SelectedNode.FullPath.Replace('\\','/');
			selpath = System.Text.RegularExpressions.Regex.Replace(
				temp,"服务器根目录",ftphost);
		}
		
	}
}
