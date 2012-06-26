/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/6/20
 * 时间: 11:14
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;   
using System.Configuration;

namespace WatchCilent
{
	/// <summary>
	/// Configuration section &lt;WimsProInfoSection&gt;
	/// </summary>
	/// <remarks>
	/// Assign properties to your child class that has the attribute 
	/// <c>[ConfigurationProperty]</c> to store said properties in the xml.
	/// </remarks>
	public sealed class WimsProInfoSectionSettings : ConfigurationSection
	{
		System.Configuration.Configuration _Config;


		#region ConfigurationProperties
		
		/*
		 *  Uncomment the following section and add a Configuration Collection 
		 *  from the with the file named WimsProInfoSection.cs
		 */
		// /// <summary>
		// /// A custom XML section for an application's configuration file.
		// /// </summary>
		// [ConfigurationProperty("customSection", IsDefaultCollection = true)]
		// public WimsProInfoSectionCollection WimsProInfoSection
		// {
		// 	get { return (WimsProInfoSectionCollection) base["customSection"]; }
		// }
		
		 [ConfigurationProperty("", IsDefaultCollection = true)]
		 public WimsProInfoCollection WimsProInfos
		 {
		 	get { return (WimsProInfoCollection) base[""]; }
		 }
		
		/// <summary>
		/// Collection of <c>WimsProInfoSectionElement(s)</c> 
		/// A custom XML section for an applications configuration file.
		/// </summary>
		[ConfigurationProperty("exampleAttribute", DefaultValue="exampleValue")]
		public string ExampleAttribute {
			get { return (string) this["exampleAttribute"]; }
			set { this["exampleAttribute"] = value; }
		}

		#endregion

		/// <summary>
		/// Private Constructor used by our factory method.
		/// </summary>
		private WimsProInfoSectionSettings () : base () {
			// Allow this section to be stored in user.app. By default this is forbidden.
			this.SectionInformation.AllowExeDefinition =
				ConfigurationAllowExeDefinition.MachineToLocalUser;
		}

		#region Public Methods
		
		/// <summary>
		/// Saves the configuration to the config file.
		/// </summary>
		public void Save() {
			_Config.Save();
		}
		
		#endregion
		
		#region Static Members
		
		/// <summary>
		/// Gets the current applications &lt;WimsProInfoSection&gt; section.
		/// </summary>
		/// <param name="ConfigLevel">
		/// The &lt;ConfigurationUserLevel&gt; that the config file
		/// is retrieved from.
		/// </param>
		/// <returns>
		/// The configuration file's &lt;WimsProInfoSection&gt; section.
		/// </returns>
		public static WimsProInfoSectionSettings GetSection (ConfigurationUserLevel ConfigLevel) {
			/* 
			 * This class is setup using a factory pattern that forces you to
			 * name the section &lt;WimsProInfoSection&gt; in the config file.
			 * If you would prefer to be able to specify the name of the section,
			 * then remove this method and mark the constructor public.
			 */ 
			System.Configuration.Configuration Config = ConfigurationManager.OpenExeConfiguration
				(ConfigLevel);
			WimsProInfoSectionSettings oWimsProInfoSectionSettings;
			
			oWimsProInfoSectionSettings =
				(WimsProInfoSectionSettings)Config.GetSection("WimsProInfoSectionSettings");
			if (oWimsProInfoSectionSettings == null) {
				oWimsProInfoSectionSettings = new WimsProInfoSectionSettings();
				Config.Sections.Add("WimsProInfoSectionSettings", oWimsProInfoSectionSettings);
			}
			oWimsProInfoSectionSettings._Config = Config;
			
			return oWimsProInfoSectionSettings;
		}
		
		#endregion
	}
}

