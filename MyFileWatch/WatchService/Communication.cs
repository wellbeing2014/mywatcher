/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-6-15
 * 时间: 17:05
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
 public class TCPManage
    {
         //private Action<string> DgGetMsg;

         //private System.ServiceProcess.ServiceBase Owner;

        /// <summary>
        /// indicate whether the thread should stop listening
        /// </summary>
        private bool IsListening = true;

        /// <summary>
        /// 1123 is the birthday of Scott.Yan who is the author of this class
        /// </summary>
       private const int TCPPort = 1124;

        private System.Threading.Thread thTCPListener;
        public delegate void Dolistened(string msg);
        private Dolistened doreal;

        /// <summary>
        /// send message to others
        /// </summary>
        /// <param name="destinationIP">the destination ip ,e.g.,192.168.1.1</param>
        /// <param name="msg">message you want to send</param>
        public  void SendMessage(string destinationIP, string msg)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(msg);
            var destIP = System.Net.IPAddress.Parse(destinationIP);
            var myIP = Communication.GetLocalIP();

            var epDest = new System.Net.IPEndPoint(destIP, TCPPort);
            var dpLocal = new System.Net.IPEndPoint(myIP, TCPPort);
            var tcpClient = new System.Net.Sockets.TcpClient();

            tcpClient.Connect(epDest);
            var netStream = tcpClient.GetStream();
            if (netStream.CanWrite)
                netStream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// call this method to start listening
        /// <param name="owner">formally you should pass "this"</param>
        /// <param name="dgGetMsg">a delegate handles when receive a message</param>
        /// </summary>            
        public  void StartListen( Dolistened dolistened)
        {
            IsListening = true;
            //Owner = owner;
            doreal = dolistened;
            thTCPListener = new System.Threading.Thread(ListenHandler);
            thTCPListener.Start();
        }

        /// <summary>
        /// call this method to stop listening
        /// </summary>
        public  void StopListen()
        {
            IsListening = false;
            
        }

       	 private void ListenHandler()
        {
            var myIP = Communication.GetLocalIP();
            var epLocal = new System.Net.IPEndPoint(myIP, TCPPort);
            var tcpListener = new System.Net.Sockets.TcpListener(epLocal);
            tcpListener.Start();

            while (IsListening)
            {
                System.Threading.Thread.Sleep(1000);
                if (tcpListener.Pending())
                {
                    var tcpClient = tcpListener.AcceptTcpClient();
                    var netStream = tcpClient.GetStream();
                    var buffer = new byte[1024];
                    if (!netStream.DataAvailable)
                        continue;

                    List<byte> bufferTotal = new List<byte>();
                    while (netStream.DataAvailable)
                    {
                        netStream.Read(buffer, 0, 1024);
                        bufferTotal.AddRange(buffer);
                    }
                    tcpClient.Close();
                    netStream.Close();
                    var receive = System.Text.Encoding.UTF8.GetString(bufferTotal.ToArray());
                    //Invoke(DgGetMsg, receive);
                    doreal(receive.TrimEnd('\0'));
                }
            }
            tcpListener.Stop();
        }

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
            msg = "1_lbt4_09#65664#206服务器#0#0#0:1289671407:飞秋1号小月月:更新包监控及测试:288:"+msg;
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient=new System.Net.Sockets.UdpClient(2426);
            UdpClient.Send(buffer, buffer.Length, epGroup);
            UdpClient.Close();
        }
    }
	}
}
