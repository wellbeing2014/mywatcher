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
			
			if (_mouseDown)
            {
                
                    if (DrawStyle != DrawStyle.None)
                    {
                        _endPoint = e.Location;
                        if (DrawStyle == DrawStyle.Line)
                        {
                            LinePointList.Add(_endPoint);
                        }
                        base.Invalidate();
                    }
                    else if (SizeGrip != SizeGrip.None)
                    {
                        ChangeSelctImageRect(e.Location);
                    }

            }
            else
            {
               
                if (DrawStyle == DrawStyle.None)
                {
                    if (OperateManager.OperateCount == 0)
                    {
                        SetSizeGrip(e.Location);
                    }
                }
                else
                {
                    if(SelectImageRect.Contains(e.Location))
                    {
                        Cursor = DrawCursor;
                    }
                    else
                    {
                        Cursor = SelectCursor;
                    }
                }
            }
			
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedImage)
                {
                    if (DrawStyle != DrawStyle.None)
                    {
                        _mouseDown = true;
                        _mouseDownPoint = e.Location;

                        if (DrawStyle == DrawStyle.Line)
                        {
                            LinePointList.Add(_mouseDownPoint);
                        }
                    }
                }
                else
                {
                    _mouseDown = true;
                    _mouseDownPoint = e.Location;
                }
            }
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			_endPoint = e.Location;
			base.Invalidate();
		}
		
		
		 protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                
                _endPoint = e.Location;
                base.Invalidate();
               
                _mouseDown = false;
                _mouseDownPoint = Point.Empty;
            }
        }
		
		
		
		public Bitmap GetImg()
		{
			Bitmap b = new Bitmap(this.Width, this.Height);
			this.DrawToBitmap(b, new Rectangle(0, 0, this.Width, this.Height));
			return b;
		}
		
		
	}
}
