﻿/*
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
        private DrawStyle _drawStyle;
        private Color _selectColor;
        private int _textFontSize;
        
        private List<Point> _linePointList;
        private static readonly object EventTextBoxShow = new object();
        private static readonly object EventTextBoxHide = new object();
        
        public event EventHandler TextBoxHide
        {
            add { base.Events.AddHandler(EventTextBoxHide, value); }
            remove { base.Events.RemoveHandler(EventTextBoxHide, value); }
        }
        public event EventHandler TextBoxShow
        {
            add { base.Events.AddHandler(EventTextBoxShow, value); }
            remove { base.Events.RemoveHandler(EventTextBoxShow, value); }
        }
        
        public int TextFontSize {
			get { return _textFontSize; }
			set { _textFontSize = value; }
		}
        
		public Color SelectColor {
			get { return _selectColor; }
			set { _selectColor = value; }
		}
        
		public DrawStyle DrawStyle {
			get { return _drawStyle; }
			set { _drawStyle = value; }
		}
        
        private List<Point> LinePointList
        {
            get
            {
                if (_linePointList == null)
                {
                    _linePointList = new List<Point>(100);
                }
                return _linePointList;
            }
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
            set
            {
            	_operateManager = value;
            }
            	
            
        }

		
		public PaintPictureBox()
		{
		}
		
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			
			 Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
			
			DrawOperate(g);
			if(_mouseDown)
			{
		        if (DrawStyle != DrawStyle.None)
		        {
		            DrawTools(g, _endPoint);
		        }
			}
			
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
            if (e.Button == MouseButtons.Left)
            {
            	if(DrawStyle == DrawStyle.Text)
            	{
            		
            		Rectangle rec = new Rectangle(_mouseDownPoint.X,_mouseDownPoint.Y,_endPoint.X,_endPoint.Y);
            		if(!rec.Contains(e.Location))
            		{
            			OnTextBoxHide();
            		}
            	}
                 _mouseDown = true;
                 _mouseDownPoint = e.Location;
            }
            
          
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			_endPoint = e.Location;
			if(_mouseDown)
			{
				if (DrawStyle == DrawStyle.Line)
	            {
	                LinePointList.Add(_endPoint);
	            }
				
			}
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
            Color color = this.SelectColor ;
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
                    
                case DrawStyle.Line:
                    if (LinePointList.Count < 2)
                    {
                        return;
                    }
                    OperateManager.AddOperate(
                        OperateType.DrawLine, 
                        color, 
                        LinePointList.ToArray());
                    LinePointList.Clear();
                    break;
                 case DrawStyle.Text:
                   EventHandler handler = base.Events[EventTextBoxShow] as EventHandler;
		            if (handler != null)
		            {
		            	handler(this,new EventArgs());
		            }
                    Rectangle textRect = Rectangle.FromLTRB(
                       _mouseDownPoint.X,
                       _mouseDownPoint.Y,
                       point.X,
                       point.Y);
                    DrawTextData textData = new DrawTextData(
                        string.Empty,
                        base.Font,
                        textRect);

                    OperateManager.AddOperate(
                        OperateType.DrawText,
                        color,
                        textData);
                    break;
                
            }
        }
		
		private void OnTextBoxHide()
		{
			EventHandler handler = base.Events[EventTextBoxHide] as EventHandler;
            if (handler != null)
            {
            	handler(this,new EventArgs());
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
                        
                    case OperateType.DrawText:
                        DrawTextData textdata = obj.Data as DrawTextData;

                        if (string.IsNullOrEmpty(textdata.Text))
                        {
                            using (Pen pen = new Pen(obj.Color))
                            {
                                pen.DashStyle = DashStyle.DashDot;
                                pen.DashCap = DashCap.Round;
                                pen.DashPattern = new float[] { 9f, 3f, 3f, 3f };
                                g.DrawRectangle(
                                    pen,
                                    textdata.TextRect);
                            }
                        }
                        else
                        {
                            using (SolidBrush brush = new SolidBrush(obj.Color))
                            {
                                g.DrawString(
                                    textdata.Text,
                                    textdata.Font,
                                    brush,
                                    textdata.TextRect);
                            }
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
                    using (Pen pen = new Pen(this.SelectColor))
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
                    using (Pen pen = new Pen(this.SelectColor))
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
                    using (Pen pen = new Pen(this.SelectColor))
                    {
                        pen.EndCap = LineCap.ArrowAnchor;
                        pen.EndCap = LineCap.Custom;
                        pen.CustomEndCap = new AdjustableArrowCap(4, 4, true);
                        g.DrawLine(pen, _mouseDownPoint, point);
                    }
                    break;
                case DrawStyle.Text:
                    using (Pen pen = new Pen(this.SelectColor))
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
                case DrawStyle.Line:
                    if (LinePointList.Count < 2)
                    {
                        return;
                    }

                    Point[] points = LinePointList.ToArray();

                    using (Pen pen = new Pen(this.SelectColor))
                    {
                        g.DrawLines(
                           pen,
                           points);
                    }
                    break;
            }
        }
		 
		public Rectangle ShowTextBox()
        {
            Rectangle bounds = 
                Rectangle.FromLTRB(
                _mouseDownPoint.X,
                _mouseDownPoint.Y,
                _endPoint.X,
                _endPoint.Y);
            return bounds;
		}
	}
}
