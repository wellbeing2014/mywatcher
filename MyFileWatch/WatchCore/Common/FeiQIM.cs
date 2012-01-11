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
using System.Data;
using System.Net.Sockets;

namespace WatchCore.Common
{
	/// <summary>
	/// Description of FeiQIM.
	/// </summary>
	public class FeiQIM
	{
		#region 变量声明及属性
		
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
			OnLineState =120,//在线状态
			Writing = 121,//正在输入
			Writed = 122 //输入停止
		}
		
		//与FEIQ通信端口号
		private int uDPPort = 2425;
		//广播段 子网上的所有系统
        private System.Net.IPAddress GroupIP = System.Net.IPAddress.Parse("226.81.9.8");
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
		private string MsgId ="1000000000" ;
		private string userName ="测试ROBOT" ;
		private string hostName ="Wicrosoft206";
		private string msgtype = MsgType.OnLine.ToString("D") ;
		private string MsgHeader ="{0}:{1}:{2}:{3}:{4}:";
		//消息表
		public DataTable msgdt = new DataTable("dt");
		private bool autoResend = true;
		/// <summary>
		/// 自动重发
		/// </summary>
		public bool AutoResend {
			get { return autoResend; }
			set { autoResend = value; }
		}
		
		public int UDPPort {
			get { return uDPPort; }
			set { uDPPort = value; }
		}
		public string UserName {
			get { return userName; }
			set { userName = value; }
		}
		public string HostName {
			get { return hostName; }
			set { hostName = value; }
		}
		/// <summary>
        /// "飞秋的消息头。例子：1_lbt4_09#65664#MACADDR#0#0#0"
        /// </summary>
		public string FeiQHead {
			get { return feiQHead; }
			set { feiQHead = value; }
		}
		#endregion
		
		
		public FeiQIM(int Port)
		{
			msgdt.Columns.Add(new DataColumn("ip", System.Type.GetType("System.String")));
			msgdt.Columns.Add(new DataColumn("msg", System.Type.GetType("System.String")));
			msgdt.Columns.Add(new DataColumn("msgid", System.Type.GetType("System.String")));
			if(!FunctionUtils.checkPort(Port.ToString()))
				throw new Exception("端口被占用");
			else 
			{
				UdpClient = new System.Net.Sockets.UdpClient(Port);
				UdpClient.JoinMulticastGroup(GroupIP);
				//BroadcastOnLine();
			}
		}
		
		#region 业务处理

		
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
        	string msg=String.Format(MsgHeader,feiQHead,MsgId,userName,hostName,MsgType.JoinIN.ToString("D"));
           	var SendIp = new System.Net.IPEndPoint(GroupIP, uDPPort);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, SendIp);
        }
        
        public void SendOnLineToSomeIP(string ip)
        {
        	//string msg=String.Format(MsgHeader,feiQHead,MsgId,userName,hostName,MsgType.JoinIN.ToString("D"));
        	string msg1=String.Format(MsgHeader,feiQHead,MsgId,userName,hostName,MsgType.OnLineState.ToString("D"))+"测试的问题注意哦";
           	var SendIp = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), uDPPort);
            //var buffer = System.Text.Encoding.Default.GetBytes(msg);
            var buffer1 = System.Text.Encoding.Default.GetBytes(msg1);
            //UdpClient.Send(buffer, buffer.Length, SendIp);
            //SendMsgToSomeIP("",ip);
            Thread.Sleep(100);
            UdpClient.Send(buffer1, buffer1.Length, SendIp);
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
            DataRow dr = msgdt.NewRow();
            dr[0]=ip;//消息表IP
           	dr[1]=msg;//消息表MSG
           	dr[2]=msgid.ToString();//消息表MSGID
            UdpClient.Send(buffer, buffer.Length, epGroup);
            msgdt.Rows.Add(dr);
            return msgid.ToString();
        }
        
        public string ReSendMsgToSomeIP(DataRow dr)
        {
        	var ip = dr[0].ToString();
        	var msg = dr[1].ToString();
        	var msgid = dr[2].ToString();
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
        /// 监听方法
        /// </summary>
        private void ListenHandler()
        {
        	var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 2425);
        	UdpClient.Client.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReceiveTimeout,1000);
            byte[] buffer = null;
            while (IsListening)
            {
                System.Threading.Thread.Sleep(20);
                try { buffer = UdpClient.Receive(ref epGroup); }
                catch(Exception) { }
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
            		if(AutoResend)
            		{
            			Thread.Sleep(200);
						DataRow[] dr1 = msgdt.Select("ip='"+ip+"'");
			    		foreach (var element in dr1) {
							ReSendMsgToSomeIP(element);
			    		}
            		}
            		if(this.LISTENED_ONLINE!=null)
            		{
            			LISTENED_ONLINE(ip);
            		}
            		break;
            	case FeiQIM.MsgType.OffLine:
            		
            		break;
            	case FeiQIM.MsgType.ResponeMsg:
            		DataRow[] dr = msgdt.Select("msgid='"+msgbody+"'");
            		if(dr.Length>0)
            			msgdt.Rows.Remove(dr[0]);
            		
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
		#endregion		
		
		#region 全局控制
		
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
        #endregion
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
