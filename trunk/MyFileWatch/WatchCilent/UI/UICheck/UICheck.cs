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

namespace WatchCilent.UI.UICheck
{
	/// <summary>
	/// Description of UICheck.
	/// </summary>
	public partial class UICheck : Form
	{
		
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
		public UICheck()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			 
            Bitmap b = new Bitmap(@"QQ截图20120504102314.jpg");
            this.pictureBox1.Image=b;
            
            this.pictureBox1.SelectColor = Color.Red;
            this.textBox.Visible = false;
            this.colorSelector.Visible = false;
//             drawToolsControl.ButtonAcceptClick += new EventHandler(
//                DrawToolsControlButtonAcceptClick);
//            drawToolsControl.ButtonSaveClick += new EventHandler(
//                DrawToolsControlButtonSaveClick);
            drawToolsControl.ButtonRedoClick += new EventHandler(
                DrawToolsControlButtonRedoClick);
            drawToolsControl.ButtonDrawStyleClick += new EventHandler(
                DrawToolsControlButtonDrawStyleClick);
             colorSelector.ColorChanged += new EventHandler(colorSelector_ColorChanged);
             colorSelector.FontSizeChanged += new EventHandler(colorSelector_FontSizeChanged);
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button4Click(object sender, System.EventArgs e)
		{
			//this.pictureBox1.Image.Save(@"C:\Users\wellbeing.wellbeing-PC\Desktop\aa.jpg");
			
			Bitmap b = this.pictureBox1.GetImg();
			b.Save("E:\\1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg); 
		}
		
		
			
		private void colorSelector_FontSizeChanged(object sender, EventArgs e)
		{
			this.pictureBox1.
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
        }
		  
		private void ShowTextBox()
        {
			Rectangle  bounds = this.pictureBox1.ShowTextBox();
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
	}
}
