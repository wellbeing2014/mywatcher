/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/6
 * Time: 12:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WatchCilent
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
    /// Communication based on TCP by Scott.Yan
    /// </summary>
    public class TCPManage
    {
        private Action<string> DgGetMsg;

        private System.Windows.Forms.Control Owner;

        /// <summary>
        /// indicate whether the thread should stop listening
        /// </summary>
        private bool IsListening = true;

        /// <summary>
        /// 1123 is the birthday of Scott.Yan who is the author of this class
        /// </summary>
        private const int TCPPort = 1123;

        private System.Threading.Thread thTCPListener;

        /// <summary>
        /// send message to others
        /// </summary>
        /// <param name="destinationIP">the destination ip ,e.g.,192.168.1.1</param>
        /// <param name="msg">message you want to send</param>
        public void SendMessage(string destinationIP, string msg)
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
        public void StartListen(System.Windows.Forms.Control owner, Action<string> dgGetMsg)
        {
            IsListening = true;
            Owner = owner;
            DgGetMsg = dgGetMsg;
            thTCPListener = new System.Threading.Thread(ListenHandler);
            thTCPListener.Start();
        }

        /// <summary>
        /// call this method to stop listening
        /// </summary>
        public void StopListen()
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
                    Owner.Invoke(DgGetMsg, receive);
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
        private System.Net.IPAddress GroupIP = System.Net.IPAddress.Parse("224.0.0.2");

        /// <summary>
        /// the birthday of Scott.Yan in Chinese lunar calendar
        /// </summary>
        private const int UDPPort = 1020;

        private System.Net.Sockets.UdpClient UdpClient;

        private System.Threading.Thread thUDPListener;

        private bool IsListening = true;

        private System.Windows.Forms.Control Owner;

        private Action<string> DgGetMsg;

        /// <summary>
        /// broadcast a message to others
        /// </summary>
        /// <param name="msg"></param>
        public void Broadcast(string msg)
        {
            var epGroup = new System.Net.IPEndPoint(GroupIP, UDPPort);
            var buffer = System.Text.Encoding.UTF8.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, epGroup);
        }

        /// <summary>
        /// listen to the group 
        /// </summary>
        /// <param name="owner">"this" in most case</param>
        /// <param name="dgGetMsg">handles message arriving</param>
        public void StartListen(System.Windows.Forms.Control owner, Action<string> dgGetMsg)
        {
            Owner = owner;
            DgGetMsg = dgGetMsg;
            IsListening = true;
            UdpClient = new System.Net.Sockets.UdpClient(UDPPort);
            UdpClient.JoinMulticastGroup(GroupIP);
            thUDPListener = new System.Threading.Thread(ListenHandler);
            thUDPListener.Start();
        }

        /// <summary>
        /// stop listen
        /// </summary>
        public void StopListen()
        {
            IsListening = false;
            UdpClient.DropMulticastGroup(GroupIP);
            thUDPListener.Abort();
            UdpClient.Close();
           
        }

        private void ListenHandler()
        {
            var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Any, UDPPort);
            byte[] buffer = null;
            while (IsListening)
            {
                System.Threading.Thread.Sleep(1000);
                try { buffer = UdpClient.Receive(ref epGroup); }
                catch { }
                if (buffer == null || buffer.Length < 1)
                    continue;
                var msg = System.Text.Encoding.UTF8.GetString(buffer);
                if (msg.Length > 0)
                    Owner.Invoke(DgGetMsg, msg);
            }
        }

    }
	}
}
