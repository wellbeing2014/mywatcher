/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/10
 * Time: 22:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using WatchCilent.dao;
using WatchCilent.vo;

namespace WatchCilent
{
	/// <summary>
	/// Description of BussinessForm.
	/// </summary>
	public partial class BussinessForm : Form
	{
		public int bussinessType = 0;
		private  int BUFFER_SIZE = 40960;
        private byte[] buffer;
        private int position; //已复制大小
        private Stream stream;
        private int totalSize; //总大小
        public String frompath; 
        public String[] topaths;
        private String topath =  null;
        private String filename = null;
        private int topathnum=0;
        private PackageInfo packageinfo = new PackageInfo();
        List<ProjectInfo> projects;
        List<PersonInfo> datasource_person;
        List<ModuleInfo> datasource_module;
		public BussinessForm(PackageInfo packageinfo)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.Closed+=new EventHandler(formClose);
			this.frompath = packageinfo.Packagepath;
			this.packageinfo = packageinfo;
			InitializeComponent();
			String[] temp =frompath.Split('\\');
			filename = temp[temp.Length-1];
			datasource_module = ModuleDao.getAllModuleInfo();
			ModuleInfo all = new ModuleInfo();
			all.Fullname ="选择模块";
			all.Id=0;
			datasource_module.Insert(0,all);
			this.comboBox1.DataSource = datasource_module;
			this.comboBox1.DisplayMember ="Fullname";
			this.comboBox1.ValueMember = "Id";
			datasource_person =PersonDao.getAllPersonInfo();
			PersonInfo person = new PersonInfo();
			person.Fullname = "选择责任人";
			person.Id = 0;
			datasource_person.Insert(0,person);
			this.comboBox2.DataSource = datasource_person;
			this.comboBox2.DisplayMember = "Fullname";
			this.comboBox2.ValueMember = "Id";
			int index_module = 0;
			if(packageinfo.Moduleid!=0)
			{
				for (index_module=0;index_module<datasource_module.Count ;index_module++ ) 
				{
					if(packageinfo.Moduleid==datasource_module[index_module].Id)
					{
						this.comboBox1.SelectedItem=this.comboBox1.Items[index_module];
						break;
					}
				}
			}
			else
			{
				string[] fi = filename.Split('(');
				if(fi.Length>=2)
				{
					string name = fi[0];
					string[] fi1 = fi[1].Split(')');
					string code = fi1[0];
					List<ModuleInfo> list =ModuleDao.getAllModuleInfoLikename(name,code);
					if (list.Count!=0)
					{
						for (index_module=0;index_module<datasource_module.Count ;index_module++ ) 
						{
							if(list[0].Id==datasource_module[index_module].Id)
							{
								this.comboBox1.SelectedItem=this.comboBox1.Items[index_module];
								break;
							}
						}
					}
					else MessageBox.Show("没能找到与更新包相关的模块，请手动确认");
				}
				else MessageBox.Show("没能找到与更新包相关的模块，请手动确认");
			}
			
			int index_manager = 0;
			if(packageinfo.Managerid!=0)
			{
				for (index_manager=0;index_manager<datasource_module.Count ;index_manager++ ) 
				{
					if(packageinfo.Managerid==datasource_person[index_manager].Id)
					{
						this.comboBox2.SelectedItem=this.comboBox2.Items[index_manager];
						break;
					}
				}
			}
			if(this.packageinfo.Id!=0)
			{
				this.button5.Enabled = false;
				this.checkBox1.Checked = true;
			}
			
			this.textBox1.Text = frompath;
			
			getAllProjectPath();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		public static String[] GetCheckedNode(TreeNodeCollection tnc)
		{
			List<String> selectNodes = new List<String> ();
		    foreach(TreeNode node in tnc)
		    {
		        if(node.Checked) 
		　　　	{
		        	selectNodes.Add(node.Tag.ToString());
		        }   
		   }
		    if(selectNodes.Count>0)
		    {
		  		return   selectNodes.ToArray();
		    }
		    else return null;
		}

		
		void Button1Click(object sender, EventArgs e)//复制
		{
			topathnum = 0;
			if(frompath!=null)
			{
				topaths = GetCheckedNode(treeView1.Nodes);
			}
			if(topaths==null||topaths.Length==0)
			{
				MessageBox.Show("请选择要复制的路径","提示");
			}
			else
			{
				copycircle();
			}
		}
				
		private void copycircle()
		{
			
			if(topathnum<topaths.Length)
			{
				totalSize =0;
				position = 0;
				topath = topaths[topathnum];
				topath = FunctionUtils.AutoCreateFolder(topath);
				if(topath!=null)
				{
					var fs = new FileStream(frompath, FileMode.Open, FileAccess.Read);
		            totalSize = (int) fs.Length;
		            stream = fs;
		            if (File.Exists(topaths[topathnum]+"/"+filename))
		                File.Delete(topaths[topathnum]+"/"+filename);
		            if(totalSize<BUFFER_SIZE)
		            {
		                BUFFER_SIZE=totalSize;
		            }
		            buffer = new byte[BUFFER_SIZE];
		            stream.BeginRead(buffer, 0, BUFFER_SIZE, new AsyncCallback( AsyncCopyFile ), null);
				}
				else 
				{
					MessageBox.Show("文件路径不存在，尝试创建路径失败","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
					topathnum++;
					copycircle();
				}
			}
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
	            if (stream.CanRead)
	            { 
	                // 锁定 FileStream
	                lock (stream)
	                {
	                    readedLength = stream.EndRead(ar); // 等到挂起的异步读取完成
	                }
	            }
	            else
	            {
	            	topathnum++;
	            	copycircle();
	            	m.BeginInvoke(null, null);
	                return;
	            }
	
	            // 写入磁盘
	            var fsWriter = new FileStream(topath+"\\"+filename, FileMode.Append, FileAccess.Write);
	            fsWriter.Write(buffer, 0, buffer.Length);
	            fsWriter.Close();
	
	            // 当前位置 
	            position += readedLength;
	
	            // 更新progressBar1
	           
	            m.BeginInvoke(null, null);
	
	            if (position >= totalSize) // 读取完毕
	            {
	                stream.Close(); //关闭
	                topathnum++;
	            	copycircle();
	                return;
	            }
	            if (stream.CanRead)
	            {
	                lock (stream)
	                {
	                    int leftSize = totalSize - position;
	                    if (leftSize < BUFFER_SIZE)
	                        buffer = new byte[leftSize];
	                    stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(AsyncCopyFile), null);
	                }
	            }
	            else
	            {
	            	topathnum++;
	            	m.BeginInvoke(null, null);
	           		copycircle();
	           		
	                return;
	            }
            } catch (Exception e) {
            	MessageBox.Show("复制文件出错！"+e.ToString());
            	topathnum++;
            	m.BeginInvoke(null, null);
	            copycircle();
	            
            }     
        }
        
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
            	label2.Text="正在复制到"+topath;
        	}
        	if(topathnum>=topaths.Length)
        	{
        		label2.Text="复制完成";
        	}
         }
        private delegate void SetStatus();
		
		void Button2Click(object sender, EventArgs e)
		{
			//stream.CanRead = false;
			if(stream!=null)
			{
			stream.Close();
			}
			if(topaths!=null)	
			{topathnum=topaths.Length;
			label2.Text="取消复制";
			}
			this.Close();
			this.Dispose();
		}
		void Button5Click(object sender, System.EventArgs e)
		{
			packageinfo.Managerid =Int32.Parse(this.comboBox2.SelectedValue.ToString());
			packageinfo.Moduleid = Int32.Parse(this.comboBox1.SelectedValue.ToString());
			if(packageinfo.Id>0)
			{
				AccessDBUtil.update(packageinfo);
			}
			else
			{
				AccessDBUtil.insert(packageinfo);
			}
			this.checkBox1.Checked = true;
		}
		void getAllProjectPath()
		{
			projects=ProjectInfoDao.getAllProjectInfo();
			List<ProjectInfo> selprojects =ProjectInfoDao.getAllProjectInfoByModuleid(Int32.Parse(this.comboBox1.SelectedValue.ToString()));
			this.treeView1.Nodes.Clear();
			for(int i=0;i<projects.Count;i++)
			{
				TreeNode tmp ;
				tmp = new TreeNode ( @projects[i].Projectname ) ;
				tmp.Tag=@projects[i].Projectpath;
				this.treeView1.Nodes.Add(tmp);
				for(int j=0;j<selprojects.Count;j++)
				{
					if(selprojects[j].Equals(projects[i]))
					{
						tmp.Checked = true;
					}
				}
			}
			
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			TextForCopyForm textforcopyform=new TextForCopyForm();
			textforcopyform.MaximizeBox=false;
			textforcopyform.MinimizeBox=false;
		
			if(textforcopyform.ShowDialog(this)==DialogResult.OK)
			{
				String path=textforcopyform.path;
				treeView1.Nodes.Add(path);
			}
			
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(this.checkBox1.Checked)
			{
				this.button5.Enabled = false;
			}
			else
			{
				this.button5.Enabled = true;
			}
		}
		void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			getAllProjectPath();
			PersonInfo selPerson =PersonDao.getPersonInfoByModuleid(Int32.Parse(this.comboBox1.SelectedValue.ToString()));
			for (int index_manager=0;index_manager<datasource_person.Count ;index_manager++ ) 
			{
				if(selPerson.Id==datasource_person[index_manager].Id)
				{
					this.comboBox2.SelectedItem=this.comboBox2.Items[index_manager];
					break;
				}
			}
		}
		
		
		void Button3Click(object sender, EventArgs e)
		{
			if(MessageBox.Show("删除更新包，不可恢复。确定要删除吗？","提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Asterisk)==DialogResult.OK)
			{
				DirectoryInfo packfile = new DirectoryInfo(frompath);
				
				if (packfile.Exists) packfile.Delete();
				AccessDBUtil.delete(this.packageinfo);
				MessageBox.Show("更新包删除成功");
				this.Close();
				this.Dispose();
			}
		}
		
					
		void formClose(object sender, EventArgs e)
		{
		}
	}
}
