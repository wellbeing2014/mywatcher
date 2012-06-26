/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/6/20
 * 时间: 11:15
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;   
using System.Configuration;

namespace WatchCilent
{
	/// <summary>
	/// Represents a single XML tag inside a ConfigurationSection
	/// or a ConfigurationElementCollection.
	/// </summary>
	public sealed class WimsProInfoElement : ConfigurationElement
	{
		/// <summary>
		/// The attribute <c>name</c> of a <c>WimsProInfoElement</c>.
		/// </summary>
		[ConfigurationProperty("name", IsKey = true, IsRequired = true)]
		public string Name
		{
			get { return (string)this["name"]; }
			set { this["name"] = value; }
		}
	
	
		/// <summary>
		/// A demonstration of how to use a boolean property.
		/// </summary>
		[ConfigurationProperty("id")]
		public string Id {
			get { return (string)this["id"]; }
			set { this["id"] = value; }
		}
	}
	
}

