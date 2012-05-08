/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/4
 * 时间: 9:52
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace WatchCilent.UI.UICheck
{
	public class ColorLabel : Control
    {
        #region Fields

        private Color _borderColor = Color.FromArgb(65, 173, 236);

        #endregion

        #region Constructors

        public ColorLabel()
            : base()
        {
            SetStyles();
        }

        #endregion

        #region Properties

        [DefaultValue(typeof(Color),"65, 173, 236")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set 
            {
                _borderColor = value;
                base.Invalidate();
            }
        }

        protected override Size DefaultSize
        {
            get { return new Size(16, 16); }
        }

        #endregion

        #region Private Methods

        private void SetStyles()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw, true);
            base.UpdateStyles();
        }

        #endregion

        #region OverideMethods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Rectangle rect = ClientRectangle;
            using (SolidBrush brush = new SolidBrush(base.BackColor))
            {
                g.FillRectangle(
                    brush,
                    rect);
            }

            ControlPaint.DrawBorder(
                g,
                rect,
                _borderColor,
                ButtonBorderStyle.Solid);

            rect.Inflate(-1, -1);
            ControlPaint.DrawBorder(
                g,
                rect,
                Color.White,
                ButtonBorderStyle.Solid);
        }

        #endregion
    }
}
