/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/3/28
 * Time: 11:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WatchCore.pojo
{
	/// <summary>
	/// Description of PersonInfo.
	/// </summary>
	public class PersonInfo
	{
		
		private  string px = "Id";
		
		public string Px {
			get { return px; }
			set { px = value; }
		}
		private int id;
		
		public int Id {
			get { return id; }
			set { id = value; }
		}
		private string fullname;
		
		public string Fullname {
			get { return fullname; }
			set { fullname = value; }
		}
		private string ip;
		
		public string Ip {
			get { return ip; }
			set { ip = value; }
		}
		
		private string password;
		
		public string Password {
			get { return password; }
			set { password = value; }
		}
		public PersonInfo()
		{
		}
	}
}
