/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/6
 * Time: 0:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using WatchCilent.UI;
using WatchCilent.UI.Pub;
using WatchCilent.UI.Theme;
using System.Runtime.InteropServices;


namespace WatchCilent
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		private const int WS_SHOWNORMAL = 1;
	  [DllImport("User32.dll")]
	  private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
	  [DllImport("User32.dll")] 
	  private static extern bool SetForegroundWindow(IntPtr hWnd);
		/// <summary>
		/// Program entry point.
		/// </summary>
		/// 
		[STAThread]
		private static void Main(string[] args)
		{
			Process pr=GetRunningInstance();
			if(pr==null)
			{
				
				//MessageBox.Show(System.Environment.CurrentDirectory+"releasenot.txt");
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				
				//Application.Run(new UI.Main());
				//检查列表
				//Application.Run(new UI.UICheck.UICheckList());
				//查看界面检查
				//Application.Run(new UI.UICheck.UICheck(11,false));
				//Application.Run(new Dbconfig());
				//Application.Run(new Main());
				Application.Run(new UI.WIMS.TaskInfo());
				//Application.Run(new UI.Theme.ChoseThemeDialog(null));
				//Application.Run(new UpdateWims());
				//Application.Run(new SelectUnit());
			}
			else
			{
				HandleRunningInstance(pr);
			}
			//Application.Run(new WatchCilent.UI.Test.SaveView());
			//Application.Run(new UploadFTP());
			//Application.Run(new SelectPath());
			//Application.Run(new WatchCilent.UI.Test.TestResult());
			//Application.Run(new WatchCilent.UI.theme.Form1());
		}
		
		/// <summary>
        /// 获取应用程序的实例,没有其它的例程，返回Null
        /// </summary>
        /// <returns>返回当前Process实例</returns>
        public static Process GetRunningInstance()
        {
            //获取当前进程
            Process currentProcess = Process.GetCurrentProcess();
            string currentFileName = currentProcess.MainModule.FileName;
            //创建新的 Process 组件的数组，并将它们与本地计算机上共享指定的进程名称的所有进程资源关联。
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            //遍历正在有相同名字运行的进程
            foreach (Process process in processes)
            {
                if (process.MainModule.FileName == currentFileName)
                {
                    if (process.Id != currentProcess.Id)//排除当前的进程
                        return process;//返回已启动的进程实例
                }
            }
            return null;
        }

		
		/// <summary>
		/// 获取窗口句柄
		/// </summary>
		/// <param name="instance">Process进程实例</param>
		public static void HandleRunningInstance(Process instance)
		{
			//确保窗口没有被最小化或最大化
			ShowWindowAsync (instance.MainWindowHandle , WS_SHOWNORMAL);
	
			//设置真实例程为foreground window
			SetForegroundWindow (instance.MainWindowHandle);
		}

		
	}
}
