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
        private OperateManager _operateManager;
        
         private DrawStyle DrawStyle
        {
            get { return DrawStyle.Ellipse; }
        }
         
         internal OperateManager OperateManager
        {
            get
            {
                if (_operateManager == null)
                {
                    _operateManager = new OperateManager();
                }
                return _operateManager;
            }
        }

		
		public PaintPictureBox()
		{
		}
		
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			 Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
			
			DrawOperate(g);

            if (DrawStyle != DrawStyle.None)
            {
                DrawTools(g, _endPoint);
            }
			
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
            if (e.Button == MouseButtons.Left)
            {
                    _mouseDown = true;
                    _mouseDownPoint = e.Location;
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
                
                if(_mouseDown)
                {
	                if (DrawStyle != DrawStyle.None)
	                {
	                    AddOperate(e.Location);
	                }
                }
               
                _mouseDown = false;
                _mouseDownPoint = Point.Empty;
            }
        }
		
		private void AddOperate(Point point)
        {
            

            Color color = Color.Red;
            switch (DrawStyle)
            {
                case DrawStyle.Rectangle:
                    OperateManager.AddOperate(
                        OperateType.DrawRectangle,
                        color,
                       Rectangle.FromLTRB(
                        _mouseDownPoint.X,
                        _mouseDownPoint.Y,
                        point.X,
                        point.Y));
                    break;
                case DrawStyle.Ellipse:
                    OperateManager.AddOperate(
                       OperateType.DrawEllipse,
                       color,
                      Rectangle.FromLTRB(
                       _mouseDownPoint.X,
                       _mouseDownPoint.Y,
                       point.X,
                       point.Y));
                    break;
                case DrawStyle.Arrow:
                    Point[] points = new Point[] { _mouseDownPoint, point };
                    OperateManager.AddOperate(
                        OperateType.DrawArrow,
                        color,
                        points);
                    break;
                
            }
        }
		
		public Bitmap GetImg()
		{
			Bitmap b = new Bitmap(this.Width, this.Height);
			this.DrawToBitmap(b, new Rectangle(0, 0, this.Width, this.Height));
			return b;
		}
		
		
		 private void DrawOperate(Graphics g)
        {
            foreach (OperateObject obj in OperateManager.OperateList)
            {
                switch (obj.OperateType)
                {
                    case OperateType.DrawRectangle:
                        using (Pen pen = new Pen(obj.Color))
                        {
                            g.DrawRectangle(
                                pen,
                                (Rectangle)obj.Data);
                        }
                        break;
                    case OperateType.DrawEllipse:
                        using (Pen pen = new Pen(obj.Color))
                        {
                            g.DrawEllipse(
                                pen,
                                (Rectangle)obj.Data);
                        }
                        break;
                    case OperateType.DrawArrow:
                        Point[] points = obj.Data as Point[];
                        using (Pen pen = new Pen(obj.Color))
                        {
                            pen.EndCap = LineCap.Custom;
                            pen.CustomEndCap = new AdjustableArrowCap(4, 4, true);
                            g.DrawLine(pen, points[0], points[1]);
                        }
                        break;
                    case OperateType.DrawLine:
                        using (Pen pen = new Pen(obj.Color))
                        {
                            g.DrawLines(pen, obj.Data as Point[]);
                        }
                        break;
                }
            }
        }
		
		 private void DrawTools(Graphics g, Point point)
        {
            
            switch (DrawStyle)
            {
                case DrawStyle.Rectangle:
                    using (Pen pen = new Pen(Color.Red))
                    {
                        g.DrawRectangle(
                            pen,
                            Rectangle.FromLTRB(
                            _mouseDownPoint.X,
                            _mouseDownPoint.Y,
                            point.X,
                            point.Y));
                    }
                    break;
                case DrawStyle.Ellipse:
                    using (Pen pen = new Pen(Color.Red))
                    {
                        g.DrawEllipse(
                            pen,
                            Rectangle.FromLTRB(
                            _mouseDownPoint.X,
                            _mouseDownPoint.Y,
                            point.X,
                            point.Y));
                    }
                    break;
                case DrawStyle.Arrow:
                    using (Pen pen = new Pen(Color.Red))
                    {
                        pen.EndCap = LineCap.ArrowAnchor;
                        pen.EndCap = LineCap.Custom;
                        pen.CustomEndCap = new AdjustableArrowCap(4, 4, true);
                        g.DrawLine(pen, _mouseDownPoint, point);
                    }
                    break;
                case DrawStyle.Text:
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
                            point.X,
                            point.Y));
                    }
                    break;
            }
        }
	}
}
