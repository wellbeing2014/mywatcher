/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011-7-16
 * 时间: 14:36
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace WatchCore.Common
{
	/// <summary>
	/// Description of CommonConst.
	/// </summary>
	public static class CommonConst
	{
//		static Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
//		static Microsoft.Win32.RegistryKey dbc = key.OpenSubKey("software\\WisoftWatchClient");  
//		public static string dbpath =@dbc.GetValue("dbpath").ToString();
//		public static string UnitHtmlPath =@dbc.GetValue("UnitHtmlPath").ToString();
//		public static string UnitDocPath =@dbc.GetValue("UnitDocPath").ToString();
//		public static string HtmlUrl =@dbc.GetValue("HtmlUrl").ToString();
//		public static string WisofServiceHost =@dbc.GetValue("WisofServiceHost").ToString();
		
		
		public static readonly string[] BUGTYPE = {"界面样式","功能设计","建议质疑","编码逻辑"};
		public const string BUGTYPE_JieMian = "界面样式";
		public const string BUGTYPE_GongNeng = "功能设计";
		public const string BUGTYPE_JianYi = "建议质疑";
		public const string BUGTYPE_BianMa = "编码逻辑";
		
		/// <summary>
		/// BUG等级
		/// </summary>
		public static readonly  string[] BUGLEVEL ={"一般","严重","轻微","紧急"};
		public const string BUGLEVEL_YiBan = "一般";
		public const string BUGLEVEL_YanZhong = "严重";
		public const string BUGLEVEL_QinWei = "轻微";
		public const string BUGLEVEL_JinJI = "紧急";
		
		public static readonly string[] PACKSTATE ={"全部","已接收","已处理","已测试","已发布","已废止"};
		
		//更新包状态
		public const string PACKSTATE_YiJieShou = "已接收";
		public const string PACKSTATE_YiChuLi = "已处理";
		public const string PACKSTATE_YiCeShi = "已测试";
		public const string PACKSTATE_YiFaBu = "已发布";
		public const string PACKSTATE_YiFeiZhi = "已废止";
		 
		//测试数量级
		public static readonly string[,] TestRate = {
			{"舒畅(1-5)","100"},
			{"微恙(6-10)","80"},
		 {"难受(11-20)","60"},
		 {"纠结(21-30)","40"},
		 {"蛋疼(31-)","20"}
		};
		
		public enum TestState
		{
			已确认,
			已废止,
			已修订
		}
		
		
		
		
		
	}
}
