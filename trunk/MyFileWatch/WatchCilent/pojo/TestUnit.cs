/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/6/21
 * Time: 22:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WatchCilent.pojo
{
	/// <summary>
	/// Description of TestUnit.
	/// </summary>
	public class TestUnit
	{
		public TestUnit()
		{
		}
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
		private byte[] testcontent;
		
		public byte[] Testcontent {
			get { return testcontent; }
			set { testcontent = value; }
		}
	}
}
