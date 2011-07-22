/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/6
 * Time: 1:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Microsoft.Win32;
using Word;

namespace WatchCilent.Common
{
	/// <summary>
	/// Description of RarCtrl.
	/// </summary>
	public partial class FunctionUtils 
	{
		
		
		 /// <summary>
        /// 是否安装了Winrar
        /// </summary>
        /// <returns></returns>
        static public bool Exists()
        {
            RegistryKey the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
            return !string.IsNullOrEmpty(the_Reg.GetValue("").ToString());
        }

        /// <summary>
        /// 打包成Rar
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="rarPatch"></param>
        /// <param name="rarName"></param>
        static public void CompressRAR(string patch, string rarPatch, string rarName)
        {
            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
             //   the_rar = the_rar.Substring(1, the_rar.Length - 7);
                Directory.CreateDirectory(patch);
                //命令参数
                //the_Info = " a    " + rarName + " " + @"C:Test?70821.txt"; //文件压缩
                the_Info = " a    " + rarName + " " + patch + " -r"; ;
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //打包文件存放目录
                the_StartInfo.WorkingDirectory = rarPatch;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="unRarPatch"></param>
        /// <param name="rarPatch"></param>
        /// <param name="rarName"></param>
        /// <returns></returns>
        public string unCompressRAR(string unRarPatch, string rarPatch, string rarName)
        {
            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;


            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                //the_rar = the_rar.Substring(1, the_rar.Length - 7);

                if (Directory.Exists(unRarPatch) == false)
                {
                    Directory.CreateDirectory(unRarPatch);
                }
                the_Info = "x " + rarName + " " + unRarPatch + " -y";

                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = rarPatch;//获取压缩包路径

                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return unRarPatch;
        }
        
        static public void openRarFile(string rarPatch)
        {
            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;
            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_Info = "\""+rarPatch+"\"";
                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                
                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
//                the_Process.WaitForExit();
//                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public void openDirectory(string patch)
        {
            try
            {
                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = "explorer.exe";
                the_StartInfo.Arguments = "\""+patch+"\"";
                
                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //自动创建不存在路径,如果包涵&datetime&则自动创建带
       	static public  String AutoCreateFolder(string filePath)   
		{   
	      	try
	    	{
	    		string[] fArr = filePath.Split('\\'); 
	    		String root = null;
	    		int beginInt ;
	    		if(fArr.Length>=3&&fArr[0]==""&&fArr[1]=="")
				{
					root = @"\\"+@fArr[2]+@"\";
					beginInt = 3;
				}
				else
				{
					root = fArr[0]+@"\";
					beginInt =1;
				}
				
                string tempFolderPath =null;
		        for (int fArrItem = beginInt; fArrItem < fArr.Length; fArrItem++)  
		        {  
		            //如果此项不为空且不包含“.”符号（即非文件名称）则判断文件目录是否存在  
		            if (fArr[fArrItem] != "" && fArr[fArrItem].IndexOf('.') == -1)   
		            {   
		            	if(fArr[fArrItem].Contains("$datetime$"))
		            	{
		            		String temptime =fArr[fArrItem].Replace("$datetime$",DateTime.Now.ToShortDateString().Replace("/",""));
		            		tempFolderPath = filePath.Substring(0, filePath.IndexOf(fArr[fArrItem]))+temptime; 
		            	}
            	     	// MessageBox.Show("已进入循环体，Item:" + fArrItem + "===fArrItem:" + fArr[fArrItem] + "===fArrLength:" + fArr.Length);   
		                //截取文件路径中至此项的字符串+文件夹根目录，判断此目录是否存在   
		               	else  tempFolderPath = filePath.Substring(0, filePath.IndexOf(fArr[fArrItem]) + fArr[fArrItem].Length);   
		                //tempFolderPath = root + "\\" + tempFolderPath;  
		                //  MessageBox.Show("需要创建的目录地址：" + folderRoot+"\\"+tempFolderPath);  
		                //检测当前目录是否存在，如果不存在则创建  
		                DirectoryInfo tempCreateFolder = new DirectoryInfo(@tempFolderPath);  
		                if (!tempCreateFolder.Exists)  
		                {  
		                    tempCreateFolder.Create();  
		                } 
		            }  
		        }  
		        return tempFolderPath;
	    	}
	    	catch(Exception)
	    	{
	    		return null;
	    	}
	    
		}  
       	
       	public static bool checkPort(string port)
		{
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("netstat", "-aon");
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string result = p.StandardOutput.ReadToEnd().ToLower();
            string name =Environment.MachineName.ToLower()+ ":"+port;
            string ip1 = Communication.GetLocalIP().ToString()+ ":"+port;
            string ip2 = "0.0.0.0"+ ":"+port;
            string ip3 = "127.0.0.1"+ ":"+port;
            if (result.IndexOf(name) >= 0||
                result.IndexOf(ip1) >= 0||
                result.IndexOf(ip2) >= 0||
                result.IndexOf(ip3) >= 0
               	)
            {
                return false;
            }
            else
            {
                return true;
            }  

		}


    }

	
}
