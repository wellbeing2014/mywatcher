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
using WatchCore.dao;
using WatchCore.pojo;
using System.ComponentModel;
using WatchCore.Common;

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
        private bool isChange = false;
        private PackageInfo packageinfo = new PackageInfo();
        List<ProjectInfo> projects;
        List<PersonInfo> datasource_person;
        List<ModuleInfo> datasource_module;
        
       
		public BussinessForm(PackageInfo packageinfo)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			//传入更新包
			this.frompath = packageinfo.Packagepath;
			this.packageinfo = packageinfo;
			this.Closing+=new CancelEventHandler(Form_Closing);
			
			
			InitializeComponent();
			this.textBox1.Text = frompath;
			
			String[] temp =frompath.Split('\\');
			filename = temp[temp.Length-1];
			/*加载模块选择框*/
			datasource_module = ModuleDao.getAllModuleInfo();
			ModuleInfo all = new ModuleInfo();
			all.Fullname ="选择模块";
			all.Id=0;
			datasource_module.Insert(0,all);
			this.comboBox1.DataSource = datasource_module;
			this.comboBox1.DisplayMember ="Fullname";
			this.comboBox1.ValueMember = "Id";
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
			
			/*加载责任人选择框*/
			datasource_person =PersonDao.getAllPersonInfo();
			PersonInfo person = new PersonInfo();
			person.Fullname = "选择责任人";
			person.Id = 0;
			datasource_person.Insert(0,person);
			this.comboBox2.DataSource = datasource_person;
			this.comboBox2.DisplayMember = "Fullname";
			this.comboBox2.ValueMember = "Id";
			this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			//测试复杂度
			this.comboBox3.DataSource =FunctionUtils.Convert(CommonConst.TestRate);
			this.comboBox3.DisplayMember = "col0";
			this.comboBox3.ValueMember = "col1";
			
			/*绑定模块*/
			int index_module = 0;
			if(packageinfo.Moduleid!=0)//更新包已有模块ID
			{
				for (index_module=0;index_module<datasource_module.Count ;index_module++ ) 
				{
					if(packageinfo.Moduleid==datasource_module[index_module].Id)
					{
						this.comboBox1.SelectedItem=this.comboBox1.Items[index_module];
						isChange = false;
						break;
					}
				}
			}
			else//没有模块ID自动绑定
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
								//this.packageinfo.Managerid = list[0].Managerid;
								break;
							}
						}
						//SavePackage();//自动绑定成功。。
						isChange=true;
						
					}
					else MessageBox.Show("没能找到与更新包相关的模块，请手动确认");
				}
				else MessageBox.Show("没能找到与更新包相关的模块，请手动确认");
			}
			
			/*绑定责任人*/
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
			
			if(packageinfo.TestRate!=0)
			{
				int rate_index;
				int rate_lenth =CommonConst.TestRate.GetLength(0);
				for(rate_index=0;rate_index<rate_lenth;rate_index++)
				{
					string sel_rate = CommonConst.TestRate[rate_index,1];
					if(packageinfo.TestRate == Int32.Parse(sel_rate))
					{
						this.comboBox3.SelectedIndex=rate_index;
						break;
					}
				}
			}
				
			
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
		
		//拷贝循环		
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
					try {
						var fs = new FileStream(frompath, FileMode.Open, FileAccess.Read);
		           		totalSize = (int) fs.Length;
		            	stream = fs;
					} catch (Exception) {
						
						MessageBox.Show("读取文件失败："+frompath,"提示");
						return ;
					}
					
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
		
       /// <summary>
       /// 取消按钮
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
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
			if(isChange)
			{
				DialogResult result=MessageBox.Show("内容有改动是否保存？","提示",MessageBoxButtons.YesNo);
				if(DialogResult.Yes ==result)
				{
					isChange=false;
			
					SavePackage();
					MessageBox.Show("已保存","提示");
				}
				
			}
			this.Close();
			this.Dispose();
			
		}
		/// <summary>
		/// 保存按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button5Click(object sender, System.EventArgs e)
		{
			isChange=false;
			
			SavePackage();
			MessageBox.Show("已保存","提示");
		}
		/// <summary>
		/// 保存方法
		/// </summary>
		void SavePackage()
		{
			if(this.comboBox1.SelectedIndex!=0||
			   this.comboBox2.SelectedIndex!=0)
			{
				packageinfo.Managerid =Int32.Parse(this.comboBox2.SelectedValue.ToString());
				packageinfo.Moduleid = Int32.Parse(this.comboBox1.SelectedValue.ToString());
				packageinfo.TestRate = Int32.Parse(this.comboBox3.SelectedValue.ToString());
			}
			else
			{
				MessageBox.Show("请关联责任人和平台","提示");
				return ;
			}
			
			packageinfo.Packagepath = this.textBox1.Text;
			packageinfo.State=CommonConst.PACKSTATE_YiChuLi;
			if(packageinfo.Id>0)
			{
				SqlDBUtil.update(packageinfo);
			}
			else
			{
				SqlDBUtil.insert(packageinfo);
			}
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
		

		void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			isChange=true;
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
				try {
					FileInfo packfile = new FileInfo(frompath);
					if (packfile.Exists) packfile.Delete();
					SqlDBUtil.delete(this.packageinfo);
					MessageBox.Show("更新包删除成功");
					this.Close();
					this.Dispose();
				} catch (Exception) {
					MessageBox.Show("读取更新包错误，或不存在","提示");
				}
				
			}
		}
		void Button6Click(object sender, EventArgs e)
		{
			FunctionUtils.openRarFile(this.packageinfo.Packagepath);
		}
		void Button7Click(object sender, EventArgs e)
		{
			String[] filename = this.packageinfo.Packagepath.Split('\\');
			String curpath = this.packageinfo.Packagepath.Replace(filename[filename.Length-1],"");
			FunctionUtils.openDirectory(curpath);
		}
		void Form_Closing(object sender,CancelEventArgs e)
		{
			if(isChange)
			{
				DialogResult result=MessageBox.Show("内容有改动是否保存？","提示",MessageBoxButtons.YesNo);
				if(DialogResult.Yes ==result)
				{
					isChange=false;
			
					SavePackage();
					MessageBox.Show("已保存","提示");
				}
				
			}
			e.Cancel = false;
		}
		
	}
}
