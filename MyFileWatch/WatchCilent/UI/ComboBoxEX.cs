/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011-7-16
 * 时间: 17:24
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;

namespace WatchCilent.UI
{
	/// <summary>
	/// Description of ComboBoxEX.
	/// </summary>
	public class ComboBoxEX:System.Windows.Forms.ComboBox
	{
		private object[] setSelectItem = new object[2];
		
		public object[] SetSelectItem {
			get { return setSelectItem; }
			set { 
				setSelectItem = value; 
				SetSelectItemFunc();
			}
		}
		public ComboBoxEX():base()
		{
		}
		private void SetSelectItemFunc()
		{
			int index = -1;
        	if (this.itemsCollection != null)
	        {
	            if (value != null)
	            {
	            	foreach (Object obj in base.Items) {
	            		
	            	}
	            	
	               // index = this.itemsCollection.IndexOf(value);
	            }
	            else
	            {
	                this.SelectedIndex = -1;
	            }
	        }
	        if (index != -1)
	        {
	            this.SelectedIndex = index;
	        }	

			
			
		}
	}
}
