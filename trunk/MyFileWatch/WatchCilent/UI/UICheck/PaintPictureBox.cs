/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2012/5/4
 * 时间: 20:49
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
	/// Description of PaintPictureBox.
	/// </summary>
	public class PaintPictureBox:PictureBox
	{
		private Point _mouseDownPoint;
        private Point _endPoint;
        private bool _mouseDown;
		
		public PaintPictureBox()
		{
		}
		
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			
			Graphics g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
			
            
             using (Pen pen = new Pen(Color.Red))
                    {
                        pen.DashStyle = DashStyle.DashDot;
                        pen.DashCap = DashCap.Round;
                        pen.DashPattern = new float[] { 9f, 3f, 3f, 3f };

                        g.DrawRectangle(
                            pen,
                            Rectangle.FromLTRB(
                            _mouseDownPoint.X,
                            _mouseDownPoint.Y,
                            _endPoint.X,
                            _endPoint.Y));
                    }
			
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			_mouseDownPoint = e.Location;
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			_endPoint = e.Location;
			base.Invalidate();
		}
		
		public Bitmap GetImg()
		{
			Bitmap b = new Bitmap(this.Width, this.Height);
			this.DrawToBitmap(b, new Rectangle(0, 0, this.Width, this.Height));
			return b;
		}
		
		
	}
}
