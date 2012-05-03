using System;
using System.Collections.Generic;
using System.Text;

namespace WatchCilent.UI.UICheck
{
   /*
	 * 由SharpDevelop创建。
	 * 用户： ZhuXinpei
	 * 日期: 2012/5/3
	 * 时间: 14:49
	 * 
	 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
	 */
   /// <summary>
    /// 建立圆角路径的样式。
    /// </summary>
    public enum RoundStyle
    {
        /// <summary>
        /// 四个角都不是圆角。
        /// </summary>
        None = 0,
        /// <summary>
        /// 四个角都为圆角。
        /// </summary>
        All = 1,
        /// <summary>
        /// 左边两个角为圆角。
        /// </summary>
        Left = 2,
        /// <summary>
        /// 右边两个角为圆角。
        /// </summary>
        Right = 3,
        /// <summary>
        /// 上边两个角为圆角。
        /// </summary>
        Top = 4,
        /// <summary>
        /// 下边两个角为圆角。
        /// </summary>
        Bottom = 5
    }
}