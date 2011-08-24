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
			ResponeMsg = 33,//消息应答
			ShakeHand = 114, //消息前握手信息
			ResponeShakeHand = 115//应答握手
		}
		public FeiQIM(int Port)
		{
			
			if(!FunctionUtils.checkPort(Port.ToString()))
				throw new Exception("端口被占用");
			else 
			{
				uDPPort = Port;
				UdpClient = new System.Net.Sockets.UdpClient(uDPPort);
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
        private System.Threading.Thread thUDPListener;
        private bool IsListening = true;
        private System.Windows.Forms.Control Owner;
        private Action<string,string> DgGetMsg;
        
       	private string FeiQHead = "1_lbt4_09#65664#MACADDR#0#0#0";
		private string MsgId ="1000000000" ;
		private string UserName ="测试服务ROBOT" ;
		private string HostName ="Wicrosoft206";
		private string msgtype = MsgType.OnLine.ToString("D") ;
		private string MsgHeader ="{0}:{1}:{2}:{3}:{4}:";
		//Key为MSGid,Value为MSGTONTENT
		private Dictionary<string , string> msgtable = new Dictionary<string, string>();
		
		
		
		 /// <summary>
	    /// 回复握手消息
	    /// </summary>
        public void SendResponeShakeHandToSomeIP(string ip,string msgbody)
        {
        	//this.msgtype = FeiQIM.MsgType.OnLine.ToString();
        	string msg=String.Format(MsgHeader,FeiQHead,MsgId,UserName,HostName,MsgType.ResponeShakeHand.ToString("D"))+msgbody;
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
        	string msg=String.Format(MsgHeader,FeiQHead,MsgId,UserName,HostName,MsgType.OnLine.ToString("D"));
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
        	msg=String.Format(MsgHeader,FeiQHead,MsgId,UserName,HostName, FeiQIM.MsgType.Msg.ToString("D"))+msg;
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
			msg=String.Format(MsgHeader,FeiQHead,msgid.ToString(),UserName,HostName, FeiQIM.MsgType.Msg.ToString("D"))+msg;
            var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), 2425);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            msgtable.Add(msgid.ToString(),msg);
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
			string msg=String.Format(MsgHeader,FeiQHead,msgid.ToString(),UserName,HostName, FeiQIM.MsgType.ResponeMsg.ToString("D"))+msgid;
            var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), 2425);
            var buffer = System.Text.Encoding.Default.GetBytes(msg);
            UdpClient.Send(buffer, buffer.Length, epGroup);
        }

        /// <summary>
        /// listen to the group 
        /// </summary>
        /// <param name="owner">"this" in most case</param>
        /// <param name="dgGetMsg">handles message arriving</param>
        public void StartListen(System.Windows.Forms.Control owner, Action<string,string> dgGetMsg)
        {
            Owner = owner;
            DgGetMsg = dgGetMsg;
            IsListening = true;
            //UdpClient = new System.Net.Sockets.UdpClient(uDPPort);
            //UdpClient.JoinMulticastGroup(GroupIP);
            thUDPListener = new System.Threading.Thread(ListenHandler);
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
        /// 检查消息是否已经发送。
        /// </summary>
        /// <param name="msgid"></param>
        /// <returns></returns>
        public bool CheckMsgIsReceived(string msgid)
        {
        	return msgtable.ContainsKey(msgid);
        }
		
        /// <summary>
        /// 监听方法
        /// </summary>
        private void ListenHandler()
        {
        	var epGroup = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 2425);
            byte[] buffer = null;
            string[] msgtou = null;
			string feiqtou = null;
			string msgid = null;
			string username = null;
			string hostname = null;
			string msgtype = null;
			int bodystart = 0;
			string msgbody = null;
            while (IsListening)
            {
                System.Threading.Thread.Sleep(1000);
                try { buffer = UdpClient.Receive(ref epGroup); }
                catch(Exception e) { }
                if (buffer == null || buffer.Length < 1)
                    continue;
                var msg = System.Text.Encoding.Default.GetString(buffer);
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
                		SendResponeShakeHandToSomeIP(epGroup.Address.ToString(),msgbody);
                		//Owner.Invoke(DgGetMsg,epGroup.Address.ToString(),msgbody);
                		break;
                	case FeiQIM.MsgType.Msg:
                		SendResponeToSomeIP(msgid,epGroup.Address.ToString());
                		Owner.Invoke(DgGetMsg,epGroup.Address.ToString(),msgbody);
                		break;
                	case FeiQIM.MsgType.SrceenShake:
                		SendMsgToSomeIP("抖你妹啊~~~",epGroup.Address.ToString());
                		break;
                	case FeiQIM.MsgType.OnLine:
                		
                		break;
                	case FeiQIM.MsgType.OffLine:
                		
                		break;
                	case FeiQIM.MsgType.ResponeMsg:
                		msgtable.Remove(msgbody);
                		break;
                	default:
                		break;
                }
                
                	
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
}
