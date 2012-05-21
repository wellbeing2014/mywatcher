/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/4
 * 时间: 10:09
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WatchCilent.UI.UICheck
{
	/// <summary>
	/// Description of RegionHelper.
	/// </summary>
	 internal static class RegionHelper
    {
        public static void CreateRegion(Control control, Rectangle rect)
        {
            using (GraphicsPath path =
                GraphicsPathHelper.CreatePath(rect, 8, RoundStyle.All, false))
            {
                if (control.Region != null)
                {
                    control.Region.Dispose();
                }
                control.Region = new Region(path);
            }
        }
    }
}
