/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-6-15
 * 时间: 16:45
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace WatchServer
{
	class Program
	{
		static private string  logfile = null;
		static private string[]  msgip ;
		static private string[]  watchpaths ;
		static System.Timers.Timer tt ;
		
		public static void Main(string[] args)
		{
			Console.WriteLine("初始化系统");
			try
			{
				IDictionary watchpath =(IDictionary)System.Configuration.ConfigurationManager.GetSection("watchpath");
				watchpaths = new string[watchpath.Count];
				watchpath.Values.CopyTo(watchpaths,0);
				logfile = System.Configuration.ConfigurationSettings.AppSettings["logfile"];
				IDictionary temp = (IDictionary)System.Configuration.ConfigurationManager.GetSection("iplist");
				msgip = new string[temp.Count];
				temp.Values.CopyTo(msgip,0);
				msgToFQ("更新包监控系统已启动");
			}
			catch(Exception e)
			{
				Console.WriteLine("读取配置文件错误");
				Console.WriteLine(e.ToString());
				Environment.Exit(0);
			}
			Console.WriteLine("开始监控...");
			Console.WriteLine("记录XLS位置："+logfile);
			Console.Write("监控文件夹位置：");
			// TODO: Implement Functionality Here
			int pathcount=0;
			while(pathcount<watchpaths.Length)
			{
				try
				{
				Console.WriteLine(watchpaths[pathcount]);
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
					Console.WriteLine("监控路径失败！");
					pathcount++;
				}
			}

			DateTime dt =DateTime.Now;
			int hour = dt.Hour*3600000;
			int munite=dt.Minute*60000;
			int second= dt.Second*1000;
			int msecond = dt.Millisecond;
			tt = new System.Timers.Timer(86400000-hour-munite-second-msecond+180000);//实例化Timer类，设置间隔时间为10000毫秒； 
			tt.Elapsed += new System.Timers.ElapsedEventHandler(theout1);//到达时间的时候执行事件； 
			tt.AutoReset = false;//设置是执行一次（false）还是一直执行(true)； 
			tt.Start();
			Console.ReadLine();
		}
		
		static void OnFileCreated(Object source, FileSystemEventArgs e)
		{
			 FileInfo fInfo = new FileInfo(e.FullPath);
			 if(!fInfo.IsReadOnly)
			 {
			 	 fInfo.IsReadOnly = true;
			 }
			Console.WriteLine((DateTime.Now).ToString()+" 创建文件："+e.FullPath);
			String[] filename = e.Name.Split('\\');
			String curpath = e.FullPath.Replace(filename[filename.Length-1],"");
			try 
			{
				Communication.UDPManage.Broadcast(e.FullPath);
			}
			catch (Exception) {
				Console.WriteLine("通知客户端消息失败！");
			}
			try
           	{
				String sql = "insert into [sheet1$](更新包名称,更新包位置,更新时间) values(@name,@patch,@updatetime)";
				ArrayList list = new ArrayList();
				list.Add(new String[2] {"@name",filename[filename.Length-1]});
				list.Add(new String[2] {"@patch",curpath});
				list.Add(new String[2] {"@updatetime",DateTime.Now.ToString() });
				DoOleSql(sql,logfile,list);
           	}
           	catch(Exception )
           	{
           		Console.WriteLine("记录XLS失败！");
           	}
           	try {
       			String  context =@"更新包提醒："+"\n在目录："+curpath+"\n创建了更新包："+filename[filename.Length-1];
       			msgToFQ(context);
           	} catch (Exception ) {
           		Console.WriteLine("发送飞秋消息失败！");
           	}
           		
		}

		
		static void DoOleSql(string sql, string database,ArrayList list)
	   	{    
	       	OleDbConnection conn = new OleDbConnection();
	       	if(database==null||database=="") return ;
	       	try
	        {//打开连接
	        	conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
	        				+ database + "; Extended Properties='Excel 8.0;HDR=no;IMEX=0'";
	            conn.Open();
	        }
	        catch (Exception)
	        {
	        	Console.WriteLine("打开XLS文件失败");
	        }
			 OleDbCommand  olecommand = new OleDbCommand(sql, conn); 
	        int i = 0;
	        while(i<list.Count)
	        {
	        	String[] temp = (String[])list[i];
	        	olecommand.Parameters.Add(temp[0],OleDbType.VarChar);
	        	olecommand.Parameters[temp[0]].Value=temp[1];
	        	i++;
	        }
	        try
	        {//执行语句
	        	olecommand.ExecuteNonQuery();
                conn.Close();
                Console.WriteLine("记录到XLS成功");
	        }
	        catch (Exception eee)
	        {
	        	Console.WriteLine("记录到XLS失败");
	            Console.WriteLine(eee.ToString());
	            conn.Close();
	        }
	        finally
	        {
	            conn.Close();//关闭数据库
	        }
	        conn.Close();
		}
		static private void msgToFQ(string msg)
		{
			int i = 0;
			while (i<msgip.Length) {
				Communication.UDPManage.BroadcastToFQ(msg,msgip[i]);
				i++;
			}
		}
		
				
		static private void theout(object source, System.Timers.ElapsedEventArgs e) 
		{ 
			Console.Write(DateTime.Now.ToLocalTime()+":开始检查昨日文件夹是否有文件存在：");
			string today= DateTime.Now.ToString("yyyyMMdd");
			string yesterday= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-1).ToString("yyyyMMdd");
			Console.WriteLine(yesterday);
			int i;
			for(i=0;i<watchpaths.Length;i++)
			{
				try {
					DirectoryInfo yesterdayDir = new DirectoryInfo(watchpaths[i]+"/"+yesterday);
					if(yesterdayDir.Exists&&yesterdayDir.GetFiles().Length==0)
					{
						yesterdayDir.Delete();
						Console.WriteLine("发现路径："+yesterdayDir.FullName+"为空，删除该文件夹成功！");
					}
					
					DirectoryInfo todayDir = new DirectoryInfo(watchpaths[i]+"/"+today);
					if(!todayDir.Exists)
					{
						Directory.CreateDirectory(watchpaths[i]+"/"+today);
						Console.WriteLine("在"+watchpaths[i]+"创建今日文件夹"+today+"成功！");
					}

				} catch (Exception) {
					Console.WriteLine("在"+watchpaths[i]+"删除昨日或创建今日文件夹有错误。");
				}
			}
		}  
	
				
		static private void theout1(object source, System.Timers.ElapsedEventArgs e) 
		{ 
			Console.WriteLine("启动日期文件夹定时器（每天12点零3分左右开始）");
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
		
				
	}
}