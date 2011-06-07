/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/9
 * Time: 23:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DirWatcher
{
	/// <summary>
	/// Description of Listend.
	/// </summary>
	public class Listend:Listendbase
	{
		public Listend()
		{
		}
		public override Object listendaction(String msg)    //函数重载
		{
			return msg;
		}

	}
}
