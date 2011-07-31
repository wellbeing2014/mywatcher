/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011-7-16
 * 时间: 14:36
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace WatchCilent.Common
{
	/// <summary>
	/// Description of CommonConst.
	/// </summary>
	public static class CommonConst
	{
		public static readonly  string[] BUGLEVEL ={"一般","严重","轻微","紧急"};
		
		public static readonly string[] PACKSTATE ={"全部","已接收","已处理","已测试","已发布","已废止"};
		
		//更新包状态
		public const string PACKSTATE_YiJieShou = "已接收";
		public const string PACKSTATE_YiChuLi = "已处理";
		public const string PACKSTATE_YiCeShi = "已测试";
		public const string PACKSTATE_YiFaBu = "已发布";
		public const string PACKSTATE_YiFeiZhi = "已废止";
		 
		//测试数量级
		public static readonly string[,] TestRate = {
			{"舒畅","100"},
		 {"微恙","80"},
		 {"难受","60"},
		 {"纠结","40"},
		 {"蛋疼","20"}
		};
		
		public enum TestState
		{
			已确认,
			已废止,
			已修订
		}
		
		
		
		
		
	}
}
