/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/6
 * Time: 0:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using WatchCilent.UI;

namespace WatchCilent
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		/// 
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new WatchCilent.UI.Test.SaveView());
			//Application.Run(new Main());
		}
		
	}
}
