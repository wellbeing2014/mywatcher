/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/6/12
 * 时间: 14:26
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using WatchCore.Common;

namespace WatchCilent.UI
{
	/// <summary>
	/// Description of MainPlug.
	/// </summary>
	public interface MainPlug
	{
		//获取菜单名称
		string[] getPlugName();
		//获取拥有权限
		string getAuthorCode();
		//获取样式
		CommonConst.UIShowSytle getSytle();
		//void OnShowMenu();
		//Object OnShowForm();
	}
}
