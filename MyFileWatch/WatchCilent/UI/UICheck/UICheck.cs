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
		
		List<string> imagelist = new List<string>();
		
		//新建
		public UICheck()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.KeyPreview = true;
			
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
			
			
			
			
			//System.Diagnostics.Trace.WriteLine("aaaaaa");
//            Bitmap b = new Bitmap(@"QQ截图20120504102314.jpg");
//            this.pictureBox1.Image=b;
			
			this.KeyDown += new KeyEventHandler(pictureBox1_KeyDown);
			//this.pictureBox1.Select();
			
			this.pictureBox1.Click += new EventHandler(pictureBox1_Click);

			this.drawToolsControl.Visible = false;
			this.colorSelector.Visible = false; 
            this.textBox.Visible = false;
            this.CenterToParent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		//修改查看
		public UICheck(string id)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			 System.Diagnostics.Trace.WriteLine("aaaaaa");
            Bitmap b = new Bitmap(@"QQ截图20120504102314.jpg");
            this.pictureBox1.Image=b;
            
            
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
            
            this.CenterToParent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		#region 画图方法
		void Button4Click(object sender, System.EventArgs e)
		{
			//this.pictureBox1.Image.Save(@"C:\Users\wellbeing.wellbeing-PC\Desktop\aa.jpg");
			
			Bitmap b = this.pictureBox1.GetImg();
			b.Save("E:\\1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg); 
		}
		
		
			
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
			Rectangle bounds = new Rectangle(bounds1.X-pos,bounds1.Y-pos2,bounds1.Width,bounds1.Height);
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
					string imageurl = "Z:\\测试文档\\IMAGE\\"+this.textBox2.Text+"_"+((imagelist.Count+1)<10?("0"+(imagelist.Count+1).ToString()):(imagelist.Count+1).ToString())+".jpg";
					imagelist.Add(imageurl);
					retImage.Save(imageurl,ImageFormat.Jpeg);
										
					this.pictureBox1.Image = new Bitmap(imageurl);
				}
				else
					MessageBox.Show("剪贴板中没有图片文件","提示");
			 }
		}
		
		private void AddImage(Image img)
		{
			string imageurl = "Z:\\测试文档\\IMAGE\\"+this.textBox2.Text+"_"+((imagelist.Count+1)<10?("0"+(imagelist.Count+1).ToString()):(imagelist.Count+1).ToString())+".jpg";
			img.Save(imageurl,ImageFormat.Jpeg);
			imagelist.Add(imageurl);
		}
	
		private void AddImage(Image img)
		{
			string imageurl = "Z:\\测试文档\\IMAGE\\"+this.textBox2.Text+"_"+((imagelist.Count+1)<10?("0"+(imagelist.Count+1).ToString()):(imagelist.Count+1).ToString())+".jpg";
			img.Save(imageurl,ImageFormat.Jpeg);
			imagelist.Add(imageurl);
		}
	}
}
