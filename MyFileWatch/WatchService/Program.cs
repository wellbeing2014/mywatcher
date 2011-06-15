/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/6/15
 * Time: 21:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Text;

namespace WatchService
{
	static class Program
	{
		/// <summary>
		/// This method starts the service.
		/// </summary>
		static void Main(string[] args)
		{
			// To run more than one service you have to add them here
			//ServiceBase.Run(new ServiceBase[] { new WatchService() });
			// 运行服务
        if (args.Length == 0)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new WatchService() };
            ServiceBase.Run(ServicesToRun);
        }
        // 安装服务
        else if (args[0].ToLower() == "/i" || args[0].ToLower() == "-i")
        {
            try
            {
                string[] cmdline = { };
                string serviceFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;

                TransactedInstaller transactedInstaller = new TransactedInstaller();
                AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceFileName, cmdline);
                transactedInstaller.Installers.Add(assemblyInstaller);
                transactedInstaller.Install(new System.Collections.Hashtable());
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        // 删除服务
        else if (args[0].ToLower() == "/u" || args[0].ToLower() == "-u")
        {
            try
            {
                string[] cmdline = { };
                string serviceFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;

                TransactedInstaller transactedInstaller = new TransactedInstaller();
                AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceFileName, cmdline);
                transactedInstaller.Installers.Add(assemblyInstaller);
                transactedInstaller.Uninstall(null);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
		}
	}
}
