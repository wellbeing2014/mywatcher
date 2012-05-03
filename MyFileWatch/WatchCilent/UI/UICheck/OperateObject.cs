/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/3
 * 时间: 17:29
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace WatchCilent.UI.UICheck
{
	/// <summary>
	/// Description of OperateObject.
	/// </summary>
	 internal class OperateObject
    {
        private OperateType _operateType;
        private Color _color;
        private object _data;

        public OperateObject() { }

        public OperateObject(
            OperateType operateType, Color color, object data)
        {
            _operateType = operateType;
            _color = color;
            _data = data;
        }

        public OperateType OperateType
        {
            get { return _operateType; }
            set { _operateType = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
