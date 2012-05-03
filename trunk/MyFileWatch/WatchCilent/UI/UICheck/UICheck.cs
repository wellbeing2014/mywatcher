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
		 private Image _image;
        private CaptureImageToolColorTable _colorTable;
        private Cursor _selectCursor = Cursors.Default;
        private Cursor _drawCursor = Cursors.Cross;

        private Point _mouseDownPoint;
        private Point _endPoint;
        private bool _mouseDown;
        private Rectangle _selectImageRect;
        private Rectangle _selectImageBounds;
        private bool _selectedImage;
        private SizeGrip _sizeGrip;
        private Dictionary<SizeGrip,Rectangle> _sizeGripRectList;
        private OperateManager _operateManager;
        private List<Point> _linePointList;

        private static readonly Font TextFont =
           new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
		
		public UICheck()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			 drawToolsControl1.ButtonDrawStyleClick += new EventHandler(
                DrawToolsControlButtonDrawStyleClick);
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		private void DrawToolsControlButtonDrawStyleClick(object sender, EventArgs e)
        {
            switch (DrawStyle)
            {
                case DrawStyle.Rectangle:
                case DrawStyle.Ellipse:
                case DrawStyle.Arrow:
                case DrawStyle.Line:
                    colorSelector.Reset();
                    ShowColorSelector();
                    if (SizeGrip != SizeGrip.None)
                    {
                        SizeGrip = SizeGrip.None;
                    }
                    break;
            }
        }
		
		
		 #region Properties

       
        public CaptureImageToolColorTable ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new CaptureImageToolColorTable();
                }
                return _colorTable;
            }
            set
            {
                _colorTable = value;
                base.Invalidate();
                SetControlColorTable();
            }
        }

        private void SetControlColorTable()
        {
            CaptureImageToolColorTable colorTable = ColorTable;
            ToolStripRendererEx renderer = new ToolStripRendererEx(colorTable);
            contextMenuStrip.Renderer = renderer;
            drawToolsControl.ColorTable = colorTable;
            colorSelector.ColorTable = colorTable;
        }

        public Image Image
        {
            get { return _image; }
        }

        public Cursor SelectCursor
        {
            get { return _selectCursor; }
            set { _selectCursor = value; }
        }

        public Cursor DrawCursor
        {
            get { return _drawCursor; }
            set { _drawCursor = value; }
        }

        internal bool SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; }
        }

        internal Rectangle SelectImageRect
        {
            get { return _selectImageRect; }
            set
            {
                _selectImageRect = value;
                if (!_selectImageRect.IsEmpty)
                {
                    CalCulateSizeGripRect();
                    base.Invalidate();
                }
            }
        }

        internal SizeGrip SizeGrip
        {
            get { return _sizeGrip; }
            set { _sizeGrip = value; }
        }

        internal Dictionary<SizeGrip, Rectangle> SizeGripRectList
        {
            get
            {
                if (_sizeGripRectList == null)
                {
                    _sizeGripRectList = new Dictionary<SizeGrip,Rectangle>();
                }
                return _sizeGripRectList;
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
        }

        private DrawStyle DrawStyle
        {
            get { return drawToolsControl.DrawStyle; }
        }

        private Color SelectedColor
        {
            get { return colorSelector.SelectedColor; }
        }

        private int FontSize
        {
            get { return colorSelector.FontSize; }
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

        #endregion
        
        #region Override Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            toolTip.SetToolTip(this, ToolTipStartCapture);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = SelectCursor;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (textBox.Visible)
            {
                if (SelectImageRect.Contains(e.Location) ||
                    e.Button == MouseButtons.Left)
                {
                    string text = textBox.Text;
                    Font font = textBox.Font;
                    Color color = textBox.ForeColor;

                    HideTextBox();
                    if (OperateManager.OperateCount > 0)
                    {
                        OperateObject obj =
                            OperateManager.OperateList[OperateManager.OperateCount - 1];
                        if (obj.OperateType == OperateType.DrawText)
                        {
                            DrawTextData textData = obj.Data as DrawTextData;
                            if (!textData.Completed)
                            {
                                if (string.IsNullOrEmpty(text))
                                {
                                    OperateManager.RedoOperate();
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
                }
                base.Invalidate();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (SelectedImage)
                {
                    if (SizeGrip != SizeGrip.None)
                    {
                        _mouseDown = true;
                        _mouseDownPoint = e.Location;
                        HideDrawToolsControl();
                        base.Invalidate();
                    }

                    if (DrawStyle != DrawStyle.None)
                    {
                        if (SelectImageRect.Contains(e.Location))
                        {
                            _mouseDown = true;
                            _mouseDownPoint = e.Location;

                            if (DrawStyle == DrawStyle.Line)
                            {
                                LinePointList.Add(_mouseDownPoint);
                            }
                            ClipCursor(false);
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
            if (_mouseDown)
            {
                if (!SelectedImage)
                {
                    SelectImageRect = GetSelectImageRect(e.Location);
                }
                else
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
            }
            else
            {
                if (!SelectedImage)
                {
                    toolTip.SetToolTip(this, ToolTipStartCapture);
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
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                if (!SelectedImage)
                {
                    SelectImageRect = GetSelectImageRect(e.Location);
                    if (!SelectImageRect.IsEmpty)
                    {
                        SelectedImage = true;
                        ShowDrawToolsControl();
                    }
                }
                else
                {
                    _endPoint = e.Location;
                    base.Invalidate();
                    if (DrawStyle != DrawStyle.None)
                    {
                        ClipCursor(true);
                        AddOperate(e.Location);
                    }
                    else if (SizeGrip != SizeGrip.None)
                    {
                        _selectImageBounds = SelectImageRect;
                        ShowDrawToolsControl();
                        SizeGrip = SizeGrip.None;
                    }
                }

                _mouseDown = false;
                _mouseDownPoint = Point.Empty;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (SelectedImage)
                {
                    if (SelectImageRect.Contains(e.Location))
                    {
                        contextMenuStrip.Show(this, e.Location);
                    }
                    else
                    {
                        ResetSelectImage();
                    }
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            bool contains = SelectImageRect.Contains(e.Location);
            if (e.Button == MouseButtons.Left)
            {
                if (contains)
                {
                    DrawLastImage();
                    DialogResult = DialogResult.OK;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!contains)
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (SelectImageRect.Width != 0 && SelectImageRect.Height != 0 )
            {
                Rectangle rect = SelectImageRect;
                CaptureImageToolColorTable colorTable = ColorTable;
                if (_mouseDown)
                {
                    if (!SelectedImage || SizeGrip != SizeGrip.None)
                    {
                        using (SolidBrush brush = new SolidBrush(
                            Color.FromArgb(90, colorTable.BackColorNormal)))
                        {
                            g.FillRectangle(brush, rect);
                        }

                        DrawImageSizeInfo(g, rect);
                    }
                }

                using (Pen pen = new Pen(colorTable.BorderColor))
                {
                    g.DrawRectangle(pen, rect);

                    using (SolidBrush brush = new SolidBrush(colorTable.BackColorPressed))
                    {
                        foreach (Rectangle sizeGripRect in SizeGripRectList.Values)
                        {
                            g.FillRectangle(
                                brush,
                                sizeGripRect);
                        }
                    }
                }

                DrawOperate(g);

                if (DrawStyle != DrawStyle.None)
                {
                    DrawTools(g, _endPoint);
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_sizeGripRectList != null)
            {
                _sizeGripRectList.Clear();
                _sizeGripRectList = null;
            }
            if (_operateManager != null)
            {
                _operateManager.Dispose();
                _operateManager = null;
            }
            if (_linePointList != null)
            {
                _linePointList.Clear();
                _linePointList = null;
            }

            _selectCursor = null;
            _drawCursor = null;
        }

        #endregion
		
		 #region Draw Methods

        private void DrawImageSizeInfo(Graphics g, Rectangle rect)
        {
            string text = string.Format(
                            "{0}x{1}",
                            rect.Width,
                            rect.Height);
            Size textSize = TextRenderer.MeasureText(text, TextFont);
            Rectangle screenBounds = Screen.GetBounds(this);
            int x = 0;
            int y = 0;
            if (rect.X + textSize.Width > screenBounds.Right - 3)
            {
                x = screenBounds.Right - textSize.Width - 3;
            }
            else
            {
                x = rect.X + 2;
            }

            if (rect.Y - textSize.Width < screenBounds.Y + 3)
            {
                y = rect.Y + 2;
            }
            else
            {
                y = rect.Y - textSize.Height - 2;
            }

            Rectangle textRect = new Rectangle(
                x, y, textSize.Width, textSize.Height);
            g.FillRectangle(Brushes.Black, textRect);
            TextRenderer.DrawText(
                g,
                text,
                TextFont,
                textRect,
                Color.White);
        }

        private void DrawTools(Graphics g, Point point)
        {
            if (!SelectImageRect.Contains(_mouseDownPoint))
            {
                return;
            }

            Color color = SelectedColor;

            switch (DrawStyle)
            {
                case DrawStyle.Rectangle:
                    using (Pen pen = new Pen(color))
                    {
                        g.DrawRectangle(
                            pen,
                            ImageBoundsToRect(Rectangle.FromLTRB(
                            _mouseDownPoint.X,
                            _mouseDownPoint.Y,
                            point.X,
                            point.Y)));
                    }
                    break;
                case DrawStyle.Ellipse:
                    using (Pen pen = new Pen(color))
                    {
                        g.DrawEllipse(
                            pen,
                            ImageBoundsToRect(Rectangle.FromLTRB(
                            _mouseDownPoint.X,
                            _mouseDownPoint.Y,
                            point.X,
                            point.Y)));
                    }
                    break;
                case DrawStyle.Arrow:
                    using (Pen pen = new Pen(color))
                    {
                        pen.EndCap = LineCap.ArrowAnchor;
                        pen.EndCap = LineCap.Custom;
                        pen.CustomEndCap = new AdjustableArrowCap(4, 4, true);
                        g.DrawLine(pen, _mouseDownPoint, point);
                    }
                    break;
                case DrawStyle.Text:
                    using (Pen pen = new Pen(color))
                    {
                        pen.DashStyle = DashStyle.DashDot;
                        pen.DashCap = DashCap.Round;
                        pen.DashPattern = new float[] { 9f, 3f, 3f, 3f };

                        g.DrawRectangle(
                            pen,
                            ImageBoundsToRect(Rectangle.FromLTRB(
                            _mouseDownPoint.X,
                            _mouseDownPoint.Y,
                            point.X,
                            point.Y)));
                    }
                    break;
                case DrawStyle.Line:
                    if (LinePointList.Count < 2)
                    {
                        return;
                    }

                    Point[] points = LinePointList.ToArray();

                    using (Pen pen = new Pen(color))
                    {
                        g.DrawLines(
                           pen,
                           points);
                    }
                    break;
            }
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
                    case OperateType.DrawLine:
                        using (Pen pen = new Pen(obj.Color))
                        {
                            g.DrawLines(pen, obj.Data as Point[]);
                        }
                        break;
                }
            }
        }

        private void DrawLastImage()
        {
            using (Bitmap allBmp = new Bitmap(
                Width, Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics allGraphics = Graphics.FromImage(allBmp))
                {
                    allGraphics.InterpolationMode = 
                        InterpolationMode.HighQualityBicubic;
                    allGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    allGraphics.DrawImage(
                        BackgroundImage,
                        Point.Empty);
                    DrawOperate(allGraphics);
                    allGraphics.Flush();

                    Bitmap bmp = new Bitmap(
                       SelectImageRect.Width,
                       SelectImageRect.Height,
                       PixelFormat.Format32bppArgb);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(
                        allBmp,
                        0,
                        0,
                        SelectImageRect,
                        GraphicsUnit.Pixel);

                    g.Flush();
                    g.Dispose();
                    _image = bmp;
                }
            }
        }

        #endregion     
	}
}
