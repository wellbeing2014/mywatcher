/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/6/15
 * Time: 21:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using WatchCore.pojo;
using System.Text.RegularExpressions;
using WatchCore.Common;

namespace WatchService
{
	public class WatchService : ServiceBase
	{
		public const string MyServiceName = "WatchService";
		static private string  dbpath = null;
		static private string  logpath = null;
		static private string[]  msgip ;
		static private string[]  watchpaths ;
		static System.Timers.Timer tt ;
		private Communication.TCPManage tcp;
		static private FeiQIM feiq = new FeiQIM(2426);
		
		
		public WatchService()
		{
			InitializeComponent();
			logpath = System.Configuration.ConfigurationSettings.AppSettings["logpath"];
//			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
//			Microsoft.Win32.RegistryKey reg = key.CreateSubKey("software\\WatchService");  
//   			reg.SetValue("dbpath",dbpath ); 
		}
		
		private void InitializeComponent()
		{
			this.ServiceName = MyServiceName;
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			// TODO: Add cleanup code here (if required)
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// Start this service.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			// TODO: Add start code here (if required) to start your service.
			//Console.WriteLine("初始化系统");
			Thread.Sleep(10000);
			WriteToLog(System.DateTime.Now.ToLocalTime()+":初始化系统");
			try
			{
				IDictionary watchpath =(IDictionary)System.Configuration.ConfigurationManager.GetSection("watchpath");
				watchpaths = new string[watchpath.Count];
				watchpath.Values.CopyTo(watchpaths,0);
				IDictionary temp = (IDictionary)System.Configuration.ConfigurationManager.GetSection("iplist");
				msgip = new string[temp.Count];
				temp.Values.CopyTo(msgip,0);
				msgToFQ("更新包监控系统已启动");
			}
			catch(Exception e)
			{
				//Console.WriteLine("读取配置文件错误");
				//Console.WriteLine(e.ToString());
				WriteToLog("读取配置文件错误");
				WriteToLog(e.ToString());
			}
			//Console.WriteLine("开始监控...");
			//Console.WriteLine("记录XLS位置："+logfile);
			//Console.Write("监控文件夹位置：");
			WriteToLog("开始监控...");
			// TODO: Implement Functionality Here
			int pathcount=0;
			while(pathcount<watchpaths.Length)
			{
				try
				{
				//Console.WriteLine(watchpaths[pathcount]);
				WriteToLog(watchpaths[pathcount]);
				FileSystemWatcher curWatcher = new FileSystemWatcher();
				curWatcher.BeginInit();
				curWatcher.IncludeSubdirectories = true;
				curWatcher.Path = @watchpaths[pathcount];
				curWatcher.Filter ="*.rar";
				//curWatcher.Changed += new FileSystemEventHandler(OnFileChanged);
				curWatcher.Created += new FileSystemEventHandler(OnFileCreated);
				//curWatcher.Deleted += new FileSystemEventHandler(OnFileDeleted);
				//curWatcher.Renamed += new RenamedEventHandler(OnFileRenamed);
				curWatcher.EnableRaisingEvents = true;
				curWatcher.EndInit();
				pathcount++;
						}
				catch(Exception)
				{
					//Console.WriteLine("监控路径失败！");
					WriteToLog("监控路径失败！");
					pathcount++;
				}
			}
			tcp  = new Communication.TCPManage();
			tcp.StartListen(listenhandler);
			feiq.HostName="测试服务监控";
			feiq.UserName= "测试ROBOT";
			feiq.StartListen();
			feiq.LISTENED_SRCEENSHAKE = LISTENED_SRCEENSHAKE;
			feiq.LISTENED_MSG = LISTENED_MSG;
			
			DateTime dt =DateTime.Now;
			int hour = dt.Hour*3600000;
			int munite=dt.Minute*60000;
			int second= dt.Second*1000;
			int msecond = dt.Millisecond;
			tt = new System.Timers.Timer(86400000-hour-munite-second-msecond+180000);//实例化Timer类，设置间隔时间为10000毫秒； 
			tt.Elapsed += new System.Timers.ElapsedEventHandler(theout1);//到达时间的时候执行事件； 
			tt.AutoReset = false;//设置是执行一次（false）还是一直执行(true)； 
			tt.Start();
		}
		
		private void LISTENED_SRCEENSHAKE(string ip)
		{
			feiq.SendMsgToSomeIP("你抖我，我也抖你",ip);
			feiq.SendScreenShakeToSomeIP(ip);
		}
		
		private void LISTENED_MSG(string ip,string msg)
		{
			feiq.SendMsgToSomeIP("你回复的内容：\n"+msg+@" \n请你按照规矩回复内容",ip);
		}
		
		
		
		
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			// TODO: Add tear-down code here (if required) to stop your service.
			tcp.StopListen();
			feiq.StopListen();
			Thread.Sleep(10000);
			WriteToLog(System.DateTime.Now.ToLocalTime()+":系统关闭");
		}
		
		static void OnFileCreated(Object source, FileSystemEventArgs e)
		{
			//将更新包设成只读
			FileInfo fInfo = new FileInfo(e.FullPath);
			if(!fInfo.IsReadOnly)
			{
			 	 fInfo.IsReadOnly = true;
			}
			WriteToLog((DateTime.Now).ToString()+" 创建文件："+e.FullPath);
			//更新包写入到数据库
			PackageInfo pack = new PackageInfo();
			string[] temp= e.FullPath.Split('\\');
			string temps = temp[temp.Length-1];
			String curpath = e.FullPath.Replace(temp[temp.Length-1],"");
			pack.Packagepath = e.FullPath;
			pack.Packagename = temps.Replace(@".rar","");
			pack.Packtime=DateTime.Now.ToLocalTime().ToString();
			pack.State="已接收";
			try {
				SqlDBUtil.insert(pack);
			} catch (Exception) {
				
				WriteToLog("数据库插入更新包失败："+pack.Packagepath);
			}
			
			//通知客户端watchclient
			try
			{
				WatchCore.Common.Communication.UDPManage.Broadcast(e.FullPath,1020);
			}
			catch (Exception) {
				WriteToLog("通知客户端消息失败！");
			}
			//发布飞秋消息
           	try {
       			String  context =@"更新包提醒："+"\n在目录："+curpath+"\n创建了更新包："+temps;
       			msgToFQ(context);
           	} catch (Exception ) {
           		WriteToLog("发送飞秋消息失败！");
           	}
		}

		private void listenhandler(string msg)
		{
			try {
				
				string[] msgtemp = 
					Regex.Split(msg,"##",RegexOptions.IgnoreCase);
				feiq.SendMsgToSomeIP(msgtemp[0],msgtemp[1]);
			} catch (Exception e2) {
				
				WriteToLog(DateTime.Now.ToLocalTime()+":用飞秋转发来自客户端的信息失败。（"+msg+"）"+e2.ToString());
			}	
		}
		
		static private void msgToFQ(string msg)
		{
			int i = 0;
			while (i<msgip.Length) {
				feiq.SendMsgToSomeIP(msg,msgip[i]);
				i++;
			}
		}
		
				
		static private void theout(object source, System.Timers.ElapsedEventArgs e) 
		{ 
			
			string today= DateTime.Now.ToString("yyyyMMdd");
			string yesterday= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-1).ToString("yyyyMMdd");
			WriteToLog(DateTime.Now.ToLocalTime()+":开始检查昨日文件夹("+yesterday+")是否有文件存在：");
			List<string> deletedir = new List<string>();
			List<string> createdir = new List<string>();
			int i;
			for(i=0;i<watchpaths.Length;i++)
			{
				try {
					DirectoryInfo yesterdayDir = new DirectoryInfo(watchpaths[i]+"/"+yesterday);
					if(yesterdayDir.Exists&&yesterdayDir.GetFiles().Length==0)
					{
						yesterdayDir.Delete();
						deletedir.Add(yesterdayDir.FullName);
						//WriteToLog("发现路径："+yesterdayDir.FullName+"为空，删除该文件夹成功！");
					}
					
					DirectoryInfo todayDir = new DirectoryInfo(watchpaths[i]+"/"+today);
					if(!todayDir.Exists)
					{
						Directory.CreateDirectory(watchpaths[i]+"/"+today);
						createdir.Add(watchpaths[i]+"/"+today);
						//WriteToLog("在"+watchpaths[i]+"创建今日文件夹"+today+"成功！");
					}

					} catch (Exception) {
					WriteToLog("在"+watchpaths[i]+"删除昨日或创建今日文件夹有错误。");
				}
			}
			WriteToLog("发现昨日为空文件夹删除：");
			foreach(string del in deletedir)
			{
				WriteToLog(del);
			}
			WriteToLog("创建今日文件夹：");
			foreach(string cre in createdir)
			{
				WriteToLog(cre);
			}
			
		}  
	
				
		static private void theout1(object source, System.Timers.ElapsedEventArgs e) 
		{ 
			WriteToLog("启动日期文件夹定时器（每天12点零3分左右开始）");
			System.Timers.Timer t;
			//创建年月日文件夹及检查文件夹空的删除的定时任务
			t = new System.Timers.Timer(86400000);//实例化Timer类，设置间隔时间为10000毫秒； 
			t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件； 
			t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)； 
			t.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；
			t.Start();
			theout(source,e);
			tt.Close();
			tt.Dispose();
		} 
		
		
		static public void WriteToLog(string log)
		{
			if(!File.Exists(logpath))  
            {  
                StreamWriter sr = File.CreateText(logpath);  
                sr.WriteLine(log);  
                sr.Close();  
            }  
            else 
            {  
                StreamWriter sr = File.AppendText(logpath);  
                sr.WriteLine(log); 
                sr.Close();  
            }  
		}
	}
}
