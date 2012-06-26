/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/3
 * 时间: 11:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.IO;
using WatchCore.dao;
using WatchCore.pojo;
using WatchCore.Common;
using MFCComboBox;
namespace WatchCilent.UI.UICheck
{
	/// <summary>
	/// Description of UICheck.
	/// </summary>
	
	public partial class UICheck : Form
	{
		//监控系统主机IP
		string WisofServiceHost = GlobalParams.WisofServiceHost;
		[DllImport("user32.dll", EntryPoint="GetScrollPos")]
		public static extern int GetScrollPos (
		 int hwnd,
		 int nBar
		);
		 public DrawStyle DrawStyle
        {
            get { return drawToolsControl.DrawStyle; }
        }
		 
		private Color SelectedColor
        {
            get { return colorSelector.SelectedColor;}
        }
		
		private int FontSize
        {
            get { return colorSelector.FontSize; }
        }
		
		
		DataTable Source_Person = PersonDao.getPersonTable();
		DataTable Source_Module = ModuleDao.getAllModuleTable();
		DataTable Source_Project = ProjectInfoDao.getAllProjectTable();
		
		List<UICheckView> Checkviewlist = new List<UICheckView>();
		OperateManager[] OperateList = null;
		int curImageno =0;
		int totalImageno=0;
		
		bool _isedit = false;
		
		UIcheckinfo uiinfo = null;
		
		
		//新建
		public UICheck()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.KeyPreview = true;//先响应控件的ctrl+V的keydown事件。
			
			this.comboBox1.DataSource = Source_Person;
			this.comboBox1.DisplayMember = "fullname";
			this.comboBox1.ValueMember = "id";
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.textBox2.Text =UICheckDao.getNewCheckNO();
			
			this.comboBox2.DataSource = Source_Module;
			this.comboBox2.DisplayMember = "fullname";
			this.comboBox2.ValueMember = "id";
			this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;		

			this.comboBox3.DataSource =Source_Project;
			this.comboBox3.DisplayMember = "projectname";
			this.comboBox3.ValueMember = "id";
			this.comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.multiColumnFilterComboBox1.DataSource = PackageDao.getAllUnTestPack();
			
			this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("packagename",200,true));
            this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("packtime", 60, true));
            this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("code", 60, true));
            this.multiColumnFilterComboBox1.DisplayMember = "packagename";
            this.multiColumnFilterComboBox1.ValueMember = "id";
            this.multiColumnFilterComboBox1.Validated+= new EventHandler(Package_SelectedValueChanged);
			this.textBox1.Enabled = false;//新建时禁止评论图片
            
			this.KeyDown += new KeyEventHandler(pictureBox1_KeyDown);
			this.pictureBox1.Click += new EventHandler(pictureBox1_Click);
			this.drawToolsControl.Visible = false;
			this.colorSelector.Visible = false; 
            this.textBox.Visible = false;
            this.CenterToParent();
            
           	uiinfo = new UIcheckinfo();
           	this.button3.Visible = false;
            this.button6.Visible = false;
           
            ValidateUPDOWN();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		//修改查看
		public UICheck(int id,bool isedit)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			_isedit = isedit;
			uiinfo = UICheckDao.getUIcheckInfoById(id);
			
			this.comboBox1.DataSource = Source_Person;
			this.comboBox1.DisplayMember = "fullname";
			this.comboBox1.ValueMember = "id";
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			this.textBox2.Text =UICheckDao.getNewCheckNO();
			
			this.comboBox2.DataSource = Source_Module;
			this.comboBox2.DisplayMember = "fullname";
			this.comboBox2.ValueMember = "id";
			this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;		

			this.comboBox3.DataSource =Source_Project;
			this.comboBox3.DisplayMember = "projectname";
			this.comboBox3.ValueMember = "id";
			this.comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			//this.multiColumnFilterComboBox1.DataSource = PackageDao.getAllUnTestPack();
			
			this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("packagename",200,true));
            this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("packtime", 60, true));
            this.multiColumnFilterComboBox1.ViewColList.Add(new MComboColumn("code", 60, true));
            this.multiColumnFilterComboBox1.DisplayMember = "packagename";
            this.multiColumnFilterComboBox1.ValueMember = "id";
            this.multiColumnFilterComboBox1.Validated+= new EventHandler(Package_SelectedValueChanged);

            this.textBox2.ReadOnly = true;
            TestUnitBingData();
            
            if(isedit)
            {
            	this.checkBox1.Checked = false;
	            this.pictureBox1.SelectColor = Color.Red;
	            this.textBox.Visible = false;
	            this.colorSelector.Visible = false;  
	            drawToolsControl.ButtonRedoClick += new EventHandler(
	                DrawToolsControlButtonRedoClick);
	            drawToolsControl.ButtonDrawStyleClick += new EventHandler(
	                DrawToolsControlButtonDrawStyleClick);
	            colorSelector.ColorChanged += new EventHandler(colorSelector_ColorChanged);
	            colorSelector.FontSizeChanged += new EventHandler(colorSelector_FontSizeChanged);
	             
	            this.pictureBox1.TextBoxHide += new EventHandler(TextBoxExLostFocus);
	            this.pictureBox1.TextBoxShow +=new EventHandler(pictureBox1_TextBoxShow);
	            
            }
            else
            {
            	this.checkBox1.Checked = true;
            	this.textBox1.ReadOnly = true;
            	this.drawToolsControl.Visible = false;
				this.colorSelector.Visible = false; 
            	this.textBox.Visible = false;
            	this.button4.Visible = false;
            	this.button3.Visible = false;
            	this.button6.Visible = false;
            	
            }
            this.CenterToParent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		
		/// <summary>
		/// 绑定数据到控件
		/// </summary>
		void TestUnitBingData()
		{
			this.multiColumnFilterComboBox1.Items.Clear();
			this.multiColumnFilterComboBox1.Items.Add(this.uiinfo);
			this.multiColumnFilterComboBox1.DisplayMember = "Packagename";
            this.multiColumnFilterComboBox1.ValueMember = "Packageid";
			this.multiColumnFilterComboBox1.SelectedIndex = 0;
			this.multiColumnFilterComboBox1.Enabled = false;
			
			//绑定主管
			foreach (DataRow element in this.Source_Person.Rows) {
				string tempid = element["id"].ToString();
				if(Int32.Parse(tempid)== uiinfo.Adminid)
				{
					int sel = this.Source_Person.Rows.IndexOf(element);
					this.comboBox1.SelectedIndex = sel;
					break;
				}
			}
			
			//绑定模块（平台）
			
			foreach (DataRow element in this.Source_Module.Rows) {
				string tempid = element["id"].ToString();
				if(Int32.Parse(tempid)== uiinfo.Moduleid)
				{
					int sel = this.Source_Module.Rows.IndexOf(element);
					this.comboBox2.SelectedIndex = sel;
					break;
				}
			}
			
			//绑定项目
			foreach (DataRow element in this.Source_Project.Rows) {
				string tempid = element["id"].ToString();
				if(Int32.Parse(tempid)== uiinfo.Projectid)
				{
					int sel = this.Source_Project.Rows.IndexOf(element);
					this.comboBox3.SelectedIndex = sel;
					break;
				}
			}
			
			this.textBox2.Text = uiinfo.Checkno;
			
			//从数据库中查出图片，赋值到picturebox
			Checkviewlist = UICheckDao.getUICheckViewByNO(this.uiinfo.Checkno);
			if(Checkviewlist.Count>0)
			{
				this.curImageno =1;
				MemoryStream ms = null;
				if(Checkviewlist[this.curImageno-1].Checkedimage==null)
				{
					ms = new MemoryStream(Checkviewlist[this.curImageno-1].Srcimage);
				}
				else 
					ms = new MemoryStream(Checkviewlist[this.curImageno-1].Checkedimage);
				this.pictureBox1.Image = Image.FromStream(ms);
				this.textBox1.Text = Checkviewlist[this.curImageno-1].Checkmark;
			}
			OperateList = new OperateManager[Checkviewlist.Count];
			
			
			ValidateUPDOWN();
		}
		
		
		#region 画图方法
		
			
		private void colorSelector_FontSizeChanged(object sender, EventArgs e)
		{
			this.pictureBox1.Font = this.Font;
		}
		private void colorSelector_ColorChanged(object sender, EventArgs e)
		{
			this.pictureBox1.SelectColor = SelectedColor;
		}
		
		private void DrawToolsControlButtonRedoClick(object sender, EventArgs e)
        {
            if (this.pictureBox1.OperateManager.OperateCount > 0)
            {
                this.pictureBox1.OperateManager.RedoOperate();
                this.pictureBox1.Invalidate();
            }
        }
		private void DrawToolsControlButtonDrawStyleClick(object sender, EventArgs e)
        {
			this.pictureBox1.DrawStyle = DrawStyle;
			this.pictureBox1.SelectColor = Color.Red;
			switch (DrawStyle)
            {
                case DrawStyle.Rectangle:
                case DrawStyle.Ellipse:
                case DrawStyle.Arrow:
                case DrawStyle.Line:
                    colorSelector.Reset();
                    ShowColorSelector();
                   
                    break;
                case DrawStyle.Text:
                    colorSelector.ChangeToFontStyle();
                    ShowColorSelector();
                   
                    break;
                case DrawStyle.None:
                    HideColorSelector();
                    break;
            }
        }
		
		private void pictureBox1_TextBoxShow(object sender, EventArgs e)
		{
			//MessageBox.Show("ca");
			ShowTextBox();
		}
		private void TextBoxExLostFocus(object sender, EventArgs e)
        {
            if (textBox.Visible)
            {
                string text = textBox.Text;
                Font font = textBox.Font;
                Color color = textBox.ForeColor;

                HideTextBox();
                if (this.pictureBox1.OperateManager.OperateCount > 0)
                {
                    OperateObject obj =
                        this.pictureBox1.OperateManager.OperateList[this.pictureBox1.OperateManager.OperateCount - 1];
                    if (obj.OperateType == OperateType.DrawText)
                    {
                        DrawTextData textData = obj.Data as DrawTextData;
                        if (!textData.Completed)
                        {
                            if (string.IsNullOrEmpty(text))
                            {
                                this.pictureBox1.OperateManager.RedoOperate();
                            }
                            else
                            {
                                obj.Color = color;
                                textData.Font = font;
                                textData.Text = text;
                                textData.Completed = true;
                            }
                        }
                    }
                }
                base.Invalidate();
            }
        }
		private void ShowColorSelector()
        {
            if (!colorSelector.Visible)
            {
            	this.pictureBox1.SelectColor = Color.Red;
                colorSelector.Visible = true;
            }
        }
		
		 private void HideColorSelector()
        {
            if (colorSelector.Visible)
            {
                colorSelector.Visible = false;
                colorSelector.Reset();
            }
        }
		 
		  private void HideTextBox()
        {
            textBox.Visible = false;
            textBox.Text = string.Empty;
            textBox.TabStop =true;
        }
		  
		private void ShowTextBox()
        {
			int pos = GetScrollPos((int)this.panel2.Handle, 0);//水平滚动条位置   
  			int pos2 = GetScrollPos((int)this.panel2.Handle , 1); 
			Rectangle  bounds1 = this.pictureBox1.ShowTextBox();
			Rectangle bounds = new Rectangle(bounds1.X-pos,(bounds1.Y-pos2+this.panel2.Location.Y),bounds1.Width,bounds1.Height);
            bounds.Inflate(-1, -1);
            textBox.Bounds = bounds;
            textBox.Text = "";
            textBox.ForeColor = SelectedColor;
            textBox.Font = new Font(
               textBox.Font.FontFamily,
               (float)FontSize);
            textBox.Visible = true;
            textBox.Focus();
        }
		
		#endregion
		//---------------------------------------------------------------------------------------------
		
		
		/// <summary>
		/// 选择更新包时自动绑定其他信息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Package_SelectedValueChanged(object sender, EventArgs e)
		{
			if(this.multiColumnFilterComboBox1.SelectedItem==null)
			{
				return ;
			}
			DataRowView package =(DataRowView) this.multiColumnFilterComboBox1.SelectedItem;
			string  moduleid =  package["realmoduleid"].ToString();
			string  managerid =  package["managerid"].ToString();
			//绑定主管
			foreach (DataRow element in this.Source_Person.Rows) {
				string tempid = element["id"].ToString();
				if(tempid.Equals(managerid))
				{
					int sel = this.Source_Person.Rows.IndexOf(element);
					this.comboBox1.SelectedIndex = sel;
					break;
				}
			}
			
			//绑定模块（平台）
			foreach (DataRow element in this.Source_Module.Rows) {
				string tempid = element["id"].ToString();
				if(tempid.Equals(moduleid))
				{
					int sel = this.Source_Module.Rows.IndexOf(element);
					this.comboBox2.SelectedIndex = sel;
					break;
				}
			}
		}
		
		
		void pictureBox1_Click(object sender, EventArgs e)
		{
			//System.Diagnostics.Trace.WriteLine("选中了");
			this.pictureBox1.Select();
		}
		
		
		void pictureBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode==Keys.Delete)
			{
				this.pictureBox1.Image =null;
				DelImage();
			}
			 if (e.Control==true && e.KeyCode == Keys.V)
			 {
				System.Windows.Forms.IDataObject   iData   =   System.Windows.Forms.Clipboard.GetDataObject(); 
				System.Drawing.Image   retImage   =   null; 
				if(   iData   !=     null   )   
				{ 
					if(   iData.GetDataPresent(   System.Windows.Forms.DataFormats.Bitmap   )   ) 
					{ 
						retImage   =   (System.Drawing.Image)iData.GetData(   System.Windows.Forms.DataFormats.Bitmap   ); 
					}   
					else   if(   iData.GetDataPresent(   System.Windows.Forms.DataFormats.Dib     )   ) 
					{ 
						retImage   =   (System.Drawing.Image)iData.GetData(   System.Windows.Forms.DataFormats.Dib   ); 
					}
				}
				
				if(retImage!=null)
				{
					AddImage(retImage);
				}
			 }
		}
		
		private void AddImage(Image img)
		{
			UICheckView uic = new UICheckView();
			uic.Checkno = this.textBox2.Text;
			uic.Checkmark = this.textBox1.Text;
			uic.Checkedimage = null;
			//内存流存入图片
			MemoryStream ms  = new MemoryStream();
            img.Save(ms,ImageFormat.Jpeg);
            Byte[] bytBLOBData = new Byte[ms.Length];
            ms.Position = 0;
			ms.Read(bytBLOBData, 0, Convert.ToInt32(ms.Length));

			uic.Srcimage = bytBLOBData;
			Checkviewlist.Add(uic);
			this.pictureBox1.Image = Image.FromStream(ms);;
			this.totalImageno = Checkviewlist.Count;
			this.curImageno = Checkviewlist.Count;
			
			ValidateUPDOWN();
		}
		
		private void DelImage()
		{
			if(Checkviewlist.Count==0)
				return;
			Checkviewlist.RemoveAt(this.curImageno-1);
			UICheckDao.DelUICheckViewByImageno(this.uiinfo.Checkno,(this.curImageno-1).ToString());
			
			int i = Checkviewlist.Count;
			MemoryStream ms = null;
			if(this.curImageno<i)
			{
				//this.curImageno = this.curImageno +1;
				ms = new MemoryStream(Checkviewlist[this.curImageno-1].Srcimage);
				this.pictureBox1.Image = Image.FromStream(ms);
			}
			else if(this.curImageno>=i&&i!=0)
			{
				this.curImageno = i;
				ms = new MemoryStream(Checkviewlist[this.curImageno-1].Srcimage);
				this.pictureBox1.Image =Image.FromStream(ms);
			}
			else
			{
				this.curImageno = 0;
				this.pictureBox1.Image = null;
			}
			
			this.totalImageno = Checkviewlist.Count;
			ValidateUPDOWN();
		}
		
		
		void Button2Click(object sender, EventArgs e)
		{
			int i = Checkviewlist.Count;
			this.totalImageno = i;
			MemoryStream ms =null;
			if(_isedit)
			{
				//当前将图片操作和文字说明赋值给对象
				OperateList[this.curImageno - 1] = this.pictureBox1.OperateManager;
				Checkviewlist[this.curImageno - 1].Checkmark = this.textBox1.Text;
				if(((Button)sender).Text =="上一张")
				{
					//将上一张图片操作和文字说明赋值给控件对象
					this.curImageno = this.curImageno - 1;
				}
				if(((Button)sender).Text =="下一张")
				{
					this.curImageno = this.curImageno + 1;
				}
				if(Checkviewlist[this.curImageno-1].Checkedimage==null)
				{
					ms = new MemoryStream(Checkviewlist[this.curImageno-1].Srcimage);
				}
				else 
					ms = new MemoryStream(Checkviewlist[this.curImageno-1].Checkedimage);
			}
			else
			{
				if(((Button)sender).Text =="上一张")
				{
					this.curImageno = this.curImageno - 1;
				}
				if(((Button)sender).Text =="下一张")
				{
					this.curImageno = this.curImageno + 1;
				}
				if(Checkviewlist[this.curImageno-1].Checkedimage==null)
				{
					ms = new MemoryStream(Checkviewlist[this.curImageno-1].Srcimage);
				}
				else 
					ms = new MemoryStream(Checkviewlist[this.curImageno-1].Checkedimage);
			}
			
			//将图片操作和文字说明赋值给控件对象
			if(OperateList!=null&&OperateList[this.curImageno - 1]!=null)
			{
				this.pictureBox1.OperateManager = OperateList[this.curImageno - 1];
			}
			else this.pictureBox1.OperateManager = null;
			this.textBox1.Text = Checkviewlist[this.curImageno - 1].Checkmark;
			
			this.pictureBox1.Image = Image.FromStream(ms);
			ValidateUPDOWN();
		}
		
		void ValidateUPDOWN()
		{
			if(this.Checkviewlist.Count==0)
			{
				this.button1.Enabled =false;
				this.button2.Enabled = false;
			}
			else
			{
				this.button1.Enabled =true;
				this.button2.Enabled = true;
			}
			if(this.curImageno==Checkviewlist.Count)
			{
				this.button1.Enabled =false;
			}
			if(this.curImageno==1)
			{
				this.button2.Enabled = false;
			}
			this.totalImageno = Checkviewlist.Count;
			this.label4.Text = String.Format("({0}/{1})",curImageno,totalImageno);
		}
		
		
		//检查基本信息
		bool UICheckInfoSave()
		{
			//责任人赋值
			if(this.comboBox1.SelectedItem!=null&&this.comboBox1.SelectedIndex!=0)
			{
				uiinfo.Adminname = this.comboBox1.Text;
				uiinfo.Adminid = (int)this.comboBox1.SelectedValue;
			}
			else
			{
				MessageBox.Show("请选择责任人","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return false ;
			}
			
			//模块赋值
			if(this.comboBox2.SelectedItem!=null&&this.comboBox2.SelectedIndex!=0)
			{
				uiinfo.Modulename= this.comboBox2.Text;
				uiinfo.Moduleid = (int)this.comboBox2.SelectedValue;
			}
			else
			{
				MessageBox.Show("请选择模块","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return false;
			}
			
			//项目赋值	
			if(this.comboBox3.SelectedItem!=null&&this.comboBox3.SelectedIndex!=0)
			{
				uiinfo.Projectname= this.comboBox3.Text;
				uiinfo.Projectid = (int)this.comboBox3.SelectedValue;
			}
			else
			{
				MessageBox.Show("请选择项目","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return false;
			}	
			
			//更新包赋值			
			uiinfo.Packagename =this.multiColumnFilterComboBox1.Text;
			if(this.multiColumnFilterComboBox1.SelectedValue!=null)
			{
				uiinfo.Packageid = (int)this.multiColumnFilterComboBox1.SelectedValue;
			}
			
			//判断insert和update
			if(uiinfo.Id==0)
			{
				if(!UICheckDao.getNewCheckNO().Equals(this.textBox2.Text))
				{
					uiinfo.Checkno = UICheckDao.getNewCheckNO();
					MessageBox.Show("检查编号被占用，系统重新分配的编号为："+uiinfo.Checkno,"提示");
				}
				else uiinfo.Checkno = this.textBox2.Text;
			}
			else 
				uiinfo.Checkno = this.textBox2.Text;
			
			//状态赋值
			uiinfo.State = Enum.GetName(typeof(CommonConst.CheckState),CommonConst.CheckState.未检查);
			if(string.IsNullOrEmpty(uiinfo.Createtime))
			{
				uiinfo.Createtime = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
			}
			
			return true;
		}
		
		
		//保存按钮
		void Button4Click(object sender, System.EventArgs e)
		{
			
			if(UICheckInfoSave())
			{
				string msgstr = "保存成功！";
				//更新
				if(uiinfo.Id!=null&&uiinfo.Id!=0)
				{
					uiinfo.Checkerid = Int32.Parse(GlobalParams.UserId);
					uiinfo.Checkername = GlobalParams.Username;
					SqlDBUtil.update(uiinfo);
					msgstr = "更新成功！";
				}
				//插入
				else
				{
					uiinfo.Id = SqlDBUtil.insertReturnID(uiinfo);
					SendMessageToManager();
				}
				
				if(_isedit)//是否编辑图片
				{
					//判断是否有当前未保存的编辑操作，如果有，则给操作数组赋值。
					if(this.pictureBox1.OperateManager!=null)
					{
						OperateList[this.curImageno-1] = this.pictureBox1.OperateManager;
					}
					//将当前的文字说明保存给对象。因为有可能编辑过。
					Checkviewlist[this.curImageno-1].Checkmark = this.textBox1.Text;
				}
				
				
				//循环保存图片信息
				for (int i = 0; i < Checkviewlist.Count; i++) {
					//设置图片编号为循环顺序
					Checkviewlist[i].Imageno =i.ToString();
					UICheckView uc = Checkviewlist[i];
					
				//保存图片到对象中
					MemoryStream ms = null;
					if (uc.Checkedimage!=null)
					{
						ms =new MemoryStream(Checkviewlist[i].Checkedimage);
					}
					else
						ms =new MemoryStream(Checkviewlist[i].Srcimage);
					//先将基础图片赋值到控件
					this.pictureBox1.Image = Image.FromStream(ms);
					//判断有无编辑操作
					if(OperateList!=null&&OperateList[i]!=null)
						this.pictureBox1.OperateManager = OperateList[i];
					else
						this.pictureBox1.OperateManager =null;
					//从控件中抓取编辑过的图片，保存到对象中
					MemoryStream ms1  = new MemoryStream();
					this.pictureBox1.GetImg().Save(ms1,ImageFormat.Jpeg);
		            Byte[] bytBLOBData = new Byte[ms1.Length];
		            ms1.Position = 0;
					ms1.Read(bytBLOBData, 0, Convert.ToInt32(ms1.Length));
					uc.Checkedimage =  bytBLOBData;
					uc.Checkmark=Checkviewlist[i].Checkmark;
					if(uc.Id!=null&&uc.Id!=0)
						SqlDBUtil.update(uc);
					else
						Checkviewlist[i].Id = SqlDBUtil.insertReturnID(uc);
				}
				//还原到当前的图片上
				MemoryStream ms2 = null;
				if (Checkviewlist[this.curImageno-1].Checkedimage!=null)
				{
					ms2 =new MemoryStream(Checkviewlist[this.curImageno-1].Checkedimage);
				}
				else
					ms2 =new MemoryStream(Checkviewlist[this.curImageno-1].Srcimage);
				this.pictureBox1.Image = Image.FromStream(ms2);
				if(OperateList!=null&&OperateList[this.curImageno-1]!=null)
					this.pictureBox1.OperateManager = OperateList[this.curImageno-1];
				else
					this.pictureBox1.OperateManager =null;
				MessageBox.Show(msgstr,"提示");
			}
				
		}
		
		//关闭按钮
		void Button5Click(object sender, EventArgs e)
		{
			this.Close();
			this.Dispose();
		}
		
		//重做当前
		void Button3Click(object sender, EventArgs e)
		{
			MemoryStream ms = new MemoryStream(Checkviewlist[this.curImageno-1].Srcimage);
			this.pictureBox1.Image = Image.FromStream(ms);
			Checkviewlist[this.curImageno-1].Checkedimage = null;
		}
		
		//重做全部
		void Button6Click(object sender, EventArgs e)
		{
			for (int i = 0; i < Checkviewlist.Count; i++) {
				Checkviewlist[i].Checkedimage = null;
			}
			MemoryStream ms = new MemoryStream(Checkviewlist[this.curImageno-1].Srcimage);
			this.pictureBox1.Image = Image.FromStream(ms);
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(this.checkBox1.Checked)
			{
				if(_isedit)
				{
					this.drawToolsControl.Visible = false;
					this.colorSelector.Visible = false; 
            		this.textBox.Visible = false;
            		this.button4.Visible = false;
            		this.button3.Visible = false;
            		this.button6.Visible = false;
            		this.textBox1.ReadOnly = true;
            		_isedit = false;
				}
			}
			else
			{
				_isedit = true;
				
				this.drawToolsControl.Visible = true;
				this.button4.Visible = true;
				this.pictureBox1.SelectColor = Color.Red;
	            this.textBox.Visible = false;
	            this.colorSelector.Visible = false;  
	            drawToolsControl.ButtonRedoClick += new EventHandler(
	                DrawToolsControlButtonRedoClick);
	            drawToolsControl.ButtonDrawStyleClick += new EventHandler(
	                DrawToolsControlButtonDrawStyleClick);
	            colorSelector.ColorChanged += new EventHandler(colorSelector_ColorChanged);
	            colorSelector.FontSizeChanged += new EventHandler(colorSelector_FontSizeChanged);
	             
	            this.pictureBox1.TextBoxHide += new EventHandler(TextBoxExLostFocus);
	            this.pictureBox1.TextBoxShow +=new EventHandler(pictureBox1_TextBoxShow);
	            
	            this.button4.Visible = true;
            	this.button3.Visible = true;
            	this.button6.Visible = true;
            	this.textBox1.ReadOnly = false;
            	
            	//TestUnitBingData();
			}
		}
		
		/// <summary>
		/// 给主管发送飞秋消息。
		/// </summary>
		void SendMessageToManager()
		{
			
			List<PersonInfo> meigonglist = PersonDao.getPersonByRole(2);
			try {
				string content ="您好：{0},\n"+uiinfo.Packagename+"的检查列表，已在"+uiinfo.Createtime+"被创建,请登录查看详细并检查。";
				foreach (var element in meigonglist) {
					string[] iplist1 = element.Ip.Split(';');
					foreach(string ip1 in iplist1)
					{
						Communication.TCPManage.SendMessage(WisofServiceHost,String.Format(content,element.Fullname)+"##"+ip1);
					}
				}
				
			} catch (Exception e) {
				MessageBox.Show("通知主管失败！:"+e.ToString());
			}
		}
	}
}
