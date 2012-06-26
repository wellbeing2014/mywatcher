/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/31
 * 时间: 13:51
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using WatchCore.Common;
using WatchCore.pojo;
namespace WatchCilent
{
	/// <summary>
	/// Description of GlobalParams.
	/// </summary>
	public class GlobalParams
	{
		
		//public static Object WimsProInfo = 
		/// <summary>
		/// 本系统用户名ID
		/// </summary>
		public static string UserId {
			get{ return System.Configuration.ConfigurationManager.AppSettings["UserId"]; }
		}
		/// <summary>
		/// 本系统用户名
		/// </summary>
		public static string Username {
			get{ return System.Configuration.ConfigurationManager.AppSettings["username"]; }
		}
		/// <summary>
		/// WIMS接口地址
		/// </summary>
		public static string WimsUrl {
			get{ return System.Configuration.ConfigurationManager.AppSettings["WimsUrl"] ;}
		}
		/// <summary>
		/// WIMS接口用户ID
		/// </summary>
		public static string WimsLoginId {
			get{ return System.Configuration.ConfigurationManager.AppSettings["WimsLoginId"]; }
		}
		
		/// <summary>
		/// FTP地址
		/// </summary>
		public static string FTPHOST {
			get{ return System.Configuration.ConfigurationManager.AppSettings["FTPHOST"]; }
		}
		/// <summary>
		/// FTP用户名
		/// </summary>
		public static string FTPID {
			get{ return System.Configuration.ConfigurationManager.AppSettings["FTPID"]; }
		}
		/// <summary>
		/// FTP密码
		/// </summary>
		public static string FTPPWD {
			get{ return  System.Configuration.ConfigurationManager.AppSettings["FTPPWD"]; }
		}
		
		/// <summary>
		/// 数据库连接串
		/// </summary>
		public static string SqlDB {
			get{ return  System.Configuration.ConfigurationManager.AppSettings["SqlDB"]; }
		}
		/// <summary>
		/// 监控系统主机IP
		/// </summary>
		public static string WisofServiceHost {
			get { return System.Configuration.ConfigurationManager.AppSettings["WisofServiceHost"]; }
		}
		
		/// <summary>
		/// 缺陷列表的HTML路径
		/// </summary>
		public static string UnitHTMLpath {
			get { return FunctionUtils.AutoCreateFolder(System.Configuration.ConfigurationManager.AppSettings["UnitHtmlPath"]); }
		}
		
		/// <summary>
		/// 浏览缺陷地址
		/// </summary>
		public static string HtmlUrl {
			get { return System.Configuration.ConfigurationManager.AppSettings["HtmlUrl"]; }
		}
		
		/// <summary>
		/// 缺陷列表的DOC路径
		/// </summary>
		public static string UnitDOCpath {
			get { return FunctionUtils.AutoCreateFolder(System.Configuration.ConfigurationManager.AppSettings["UnitDocPath"]); }
		}
		
		/// <summary>
		/// 默认路径
		/// </summary>
		public static string Defaultpath {
			get { return System.Environment.CurrentDirectory; }
		}
		
		/// <summary>
		/// 临时文件路径
		/// </summary>
		public static string Temppath {
			get { return System.IO.Path.GetTempPath(); }
		}
		
		public static PersonInfo user ;
		
		public static PersonInfo User {
			get { return user; }
			set { user = value; }
		}
		public GlobalParams()
		{
		}
	}
}
