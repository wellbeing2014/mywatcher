/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/6/19
 * Time: 22:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WatchCilent
{
	/// <summary>
	/// Description of WebBrowserEX.
	/// </summary>
	public class WebBrowserEX:System.Windows.Forms.WebBrowser
	{
		public WebBrowserEX():base()
		{
			
		}
		
		
//		protected   override   void   DefWndProc(ref   Message   m) 
//		{ 
//			
//			IDataObject data = Clipboard.GetDataObject();//从剪贴板中获取数据
//			if(data.GetDataPresent(typeof(Bitmap)))//判断是否是图片类型
//			{
//			    Bitmap map = (Bitmap) data.GetData(typeof(Bitmap));//将图片数据存到位图中
//			    map.Save(@"C:\a.bmp");//保存图片
//			}
//			System.Diagnostics.Trace.WriteLine(m.Msg.ToString());
//			base.DefWndProc   (ref   m); 
//		}	
		
	}
}
