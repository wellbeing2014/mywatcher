/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-6-15
 * 时间: 17:05
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace WatchService
{
	
	/// <summary>
	/// Description of Communication.
	/// </summary>
	public partial class Communication 
	{
		public static System.Net.IPAddress GetLocalIP()
	    {
	        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
	        if (host.AddressList.Length < 1)
	            throw new Exception("Can't find any valid IP address.");
	
	        System.Net.IPAddress myIP = null;
	        foreach (var p in host.AddressList)
	        {
	            if (!p.IsIPv6LinkLocal)
	            {
	                myIP = p;
	                break;
	            }
	        }
	        if (myIP == null)
	            throw new Exception("Can't find any valid IPV4 address.");
	
	        return myIP;
	    }

   
    /// <summary>
    /// Communication based on UDP by Scott.Yan
    /// </summary>
   	public class UDPManage
    {
        /// <summary>
        /// this is a group address
        /// </summary>
      	static  private System.Net.IPAddress GroupIP = System.Net.IPAddress.Parse("224.0.0.2");

        /// <summary>
        /// the birthday of Scott.Yan in Chinese lunar calendar
        /// </summary>
       	//private const int UDPPort = 1019;

       	static private System.Net.Sockets.UdpClient UdpClient;


        /// <summary>
        /// broadcast a message to others
        /// </summary>
        /// <param name="msg"></param>
        static	public void Broadcast(string msg)
        {
            var epGroup = new System.Net.IPEndPoint(GroupIP, 1020);
            var buffer = System.Text.Encoding.UTF8.GetBytes(msg);
            UdpClient=new System.Net.Sockets.UdpClient(1019);
            UdpClient.Send(buffer, buffer.Length, epGroup);
            UdpClient.Close();
        }
        static	public void BroadcastToFQ(string msg,string ip)
        {
            var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), 2425);
            msg = "1_lbt4_09#65664#205服务器#0#0#0:1289671407:205飞秋1号小月月:更新包监控:288:"+msg;
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient=new System.Net.Sockets.UdpClient(2426);
            UdpClient.Send(buffer, buffer.Length, epGroup);
            UdpClient.Close();
        }
       	
    }
	}
}
