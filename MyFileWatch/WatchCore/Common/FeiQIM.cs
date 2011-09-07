/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011-8-20
 * 时间: 13:24
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace WatchCore.Common
{
	/// <summary>
	/// Description of FeiQIM.
	/// </summary>
	public class FeiQIM
	{
		public enum MsgType:int
		{
			Msg=288,//消息内容
			SrceenShake=209,//闪屏
			OnLine = 6291457,//上线
			OffLine = 6291458,//下线
			JoinIN = 6291459,
			ResponeMsg = 33,//消息应答
			ShakeHand = 114, //消息前握手信息
			ResponeShakeHand = 115,//应答握手
			Writing = 121,//正在输入
			Writed = 122 //输入停止
		}
		public FeiQIM(int Port)
		{
			
			if(!FunctionUtils.checkPort(Port.ToString()))
				throw new Exception("端口被占用");
			else 
			{
				UdpClient = new System.Net.Sockets.UdpClient(Port);
				UdpClient.JoinMulticastGroup(GroupIP);
				BroadcastOnLine();
			}
		}
		//与FEIQ通信端口号
		private int uDPPort = 2425;
		public int UDPPort {
			get { return uDPPort; }
			set { uDPPort = value; }
		}
		//广播段 子网上的所有系统
        private System.Net.IPAddress GroupIP = System.Net.IPAddress.Parse("224.0.0.1");
      
        private System.Net.Sockets.UdpClient UdpClient;
        private Thread thUDPListener;
        private bool IsListening = true;
        
        //收到消息的接口方法
        public delegate void ListenedMsg(string ip,string msg);
        public ListenedMsg LISTENED_MSG;
        //收到闪屏的接口方法
        public delegate void ListenedSrceenShake(string ip);
        public ListenedSrceenShake LISTENED_SRCEENSHAKE;
        //收到上线信息的接口方法
        public delegate void ListenedOnLine(string ip);
        public ListenedOnLine LISTENED_ONLINE;
        //收到输入状态的接口方法
        public delegate void ListenedWriting(string ip);
        public ListenedWriting LISTENED_WRITING;
        
       	private string feiQHead = "1_lbt4_09#65664#MACADDR#0#0#0";
       	
		public string FeiQHead {
			get { return feiQHead; }
			set { feiQHead = value; }
		}
		private string MsgId ="1000000000" ;
		private string userName ="测试服务ROBOT" ;
		
		public string UserName {
			get { return userName; }
			set { userName = value; }
		}
		private string hostName ="Wicrosoft206";
		
		public string HostName {
			get { return hostName; }
			set { hostName = value; }
		}
		private string msgtype = MsgType.OnLine.ToString("D") ;
		private string MsgHeader ="{0}:{1}:{2}:{3}:{4}:";
		
		
		
		
		 /// <summary>
	    /// 回复握手消息
	    /// </summary>
        public void SendResponeShakeHandToSomeIP(string ip,string msgbody)
        {
        	//this.msgtype = FeiQIM.MsgType.OnLine.ToString();
        	string msg=String.Format(MsgHeader,feiQHead,MsgId,userName,hostName,MsgType.ResponeShakeHand.ToString("D"))+msgbody;
        	var SendIp = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), uDPPort);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, SendIp);
        }
	    /// <summary>
	    /// 广播上线消息
	    /// </summary>
        public void BroadcastOnLine()
        {
        	//this.msgtype = FeiQIM.MsgType.OnLine.ToString();
        	string msg=String.Format(MsgHeader,feiQHead,MsgId,userName,hostName,MsgType.JoinIN.ToString("D"))+userName+"..";
            var SendIp = new System.Net.IPEndPoint(GroupIP, uDPPort);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, SendIp);
        }
        
        /// <summary>
        /// 广播自定义消息
        /// </summary>
        /// <param name="msg"></param>
        public void BroadcastMsg(string msg)
        {
        	msg=String.Format(MsgHeader,feiQHead,MsgId,userName,hostName, FeiQIM.MsgType.Msg.ToString("D"))+msg;
            var SendIp = new System.Net.IPEndPoint(GroupIP, uDPPort);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, SendIp);
        }
        
        /// <summary>
        /// 发送消息给指定IP的飞秋 返回 MSGID
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        public string SendMsgToSomeIP(string msg,string ip)
        {
        	Random ro = new Random(); 
			int msgid = ro.Next(1000000000,1999999999);
			msg=String.Format(MsgHeader,feiQHead,msgid.ToString(),userName,hostName, FeiQIM.MsgType.Msg.ToString("D"))+msg;
            var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), uDPPort);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, epGroup);
            return msgid.ToString();
        }
        /// <summary>
        /// 发送应答消息给IP
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="ip"></param>
        public void SendResponeToSomeIP(string msgid,string ip)
        {
			string msg=String.Format(MsgHeader,feiQHead,msgid.ToString(),userName,hostName, FeiQIM.MsgType.ResponeMsg.ToString("D"))+msgid;
            var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), uDPPort);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, epGroup);
        }
        
        public void SendScreenShakeToSomeIP(string ip)
        {
        	Random ro = new Random(); 
			int msgid = ro.Next(1000000000,1999999999);
        	string msg=String.Format(MsgHeader,feiQHead,msgid.ToString(),userName,hostName, FeiQIM.MsgType.SrceenShake.ToString("D"));
            var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), uDPPort);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, epGroup);
        }

        /// <summary>
        /// listen to the group 
        /// </summary>
        /// <param name="owner">"this" in most case</param>
        /// <param name="dgGetMsg">handles message arriving</param>
        public void StartListen()
        {
            IsListening = true;
            thUDPListener = new Thread(ListenHandler);
            thUDPListener.Start();
        }

        /// <summary>
        /// stop listen
        /// </summary>
        public void StopListen()
        {
            if(IsListening)
            {
            	//UdpClient.DropMulticastGroup(GroupIP);
            	IsListening = false;
            	thUDPListener.Abort();
            }
           
        }

        /// <summary>
        /// 监听方法
        /// </summary>
        private void ListenHandler()
        {
        	var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 2425);
            byte[] buffer = null;
            
            while (IsListening)
            {
                System.Threading.Thread.Sleep(200);
                try { buffer = UdpClient.Receive(ref epGroup); }
                catch(Exception e) { }
                if (buffer == null || buffer.Length < 1)
                    continue;
                var msg = System.Text.Encoding.Default.GetString(buffer);
                var ip = epGroup.Address.ToString();
                Thread listendthread = new Thread(listendThread);
                ListenedPara para = new ListenedPara();
                para.Ip = ip;
                para.Msg = msg;
                listendthread.Start(para);
            }
        }
        
        
        public void listendThread(object a)
        {
        	string[] msgtou = null;
			string feiqtou = null;
			string msgid = null;
			string username = null;
			string hostname = null;
			string msgtype = null;
			int bodystart = 0;
			string msgbody = null;
        	ListenedPara para = a as ListenedPara;
        	string msg = para.Msg;
        	string ip = para.Ip;
        	if (msg.Length > 0)
            {
            	msgtou = msg.Split(':');
				feiqtou = msgtou[0];
				msgid = msgtou[1];
				username = msgtou[2];
				hostname = msgtou[3];
				msgtype = msgtou[4];
				bodystart = feiqtou.Length+msgid.Length+username.Length+hostname.Length+msgtype.Length+5;
				msgbody = msg.Substring(bodystart);
            }
            MsgType revmsgtype = (MsgType)(Int32.Parse(msgtype));
            switch (revmsgtype) {
            	case FeiQIM.MsgType.ShakeHand:
            		SendResponeShakeHandToSomeIP(ip,msgbody);
            		break;
            	case FeiQIM.MsgType.Msg:
            		SendResponeToSomeIP(msgid,ip);
            		if(this.LISTENED_MSG!=null)
            		{
            			LISTENED_MSG(ip,msgbody);
            		}
            		break;
            	case FeiQIM.MsgType.SrceenShake:
            		
            		if(this.LISTENED_SRCEENSHAKE!=null)
            		{
            			LISTENED_SRCEENSHAKE(ip);
            		}
            		break;
            	case FeiQIM.MsgType.OnLine:
            		if(this.LISTENED_ONLINE!=null)
            		{
            			LISTENED_ONLINE(ip);
            		}
            		break;
            	case FeiQIM.MsgType.OffLine:
            		
            		break;
            	case FeiQIM.MsgType.ResponeMsg:
            		
            		break;
            	case FeiQIM.MsgType.Writing:
            		if(this.LISTENED_WRITING!=null)
            		{
            			LISTENED_WRITING(ip);
            		}
            		break;
            	default:
            		break;
            }
        }
       	
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void close()
        {
        	if(IsListening)
            {
            	UdpClient.DropMulticastGroup(GroupIP);
            	IsListening = false;
            	thUDPListener.Abort();
            }
        	UdpClient.Close();
        }
	}
	class ListenedPara
	{
		private string ip;
		
		public string Ip {
			get { return ip; }
			set { ip = value; }
		}
		private string msg;
		
		public string Msg {
			get { return msg; }
			set { msg = value; }
		}
	}
}
