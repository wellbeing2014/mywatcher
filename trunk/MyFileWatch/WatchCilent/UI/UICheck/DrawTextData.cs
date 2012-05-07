/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2012/5/7
 * 时间: 18:56
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
	/// <summary>
	/// Description of DrawTextData.
	/// </summary>
	 internal class DrawTextData
    {
        private string _text;
        private Font _font;
        private Rectangle _textRect;
        private bool _completed;

        public DrawTextData() { }

        public DrawTextData(string text, Font font, Rectangle textRect) 
        {
            _text = text;
            _font = font;
            _textRect = textRect;
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public Rectangle TextRect
        {
            get { return _textRect; }
            set { _textRect = value; }
        }

        public bool Completed
        {
            get { return _completed; }
            set { _completed = value; }
        }
    }
}
