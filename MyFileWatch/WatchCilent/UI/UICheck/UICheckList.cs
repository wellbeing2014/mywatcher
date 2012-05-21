/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinpei
 * 日期: 2012/5/17
 * 时间: 8:56
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WatchCilent.UI.UICheck
{
	/// <summary>
	/// Description of UICheckList.
	/// </summary>
	public partial class UICheckList : UserControl
	{
		public UICheckList()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		/// <summary>
		/// 时间段显示、隐藏
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			Point cb1p=new Point();
			cb1p=this.comboBox1.Location;
			Point cb2p=new Point();
			cb2p=this.comboBox2.Location;
			Point cb3p=new Point();
			cb3p=this.comboBox3.Location;
			if(!this.checkBox2.Checked)
			{
				this.dateTimePicker1.Dispose();
				this.dateTimePicker2.Dispose();
				this.label1.Dispose();
				this.label2.Dispose();
				cb1p.X=cb1p.X-250;
				cb2p.X=cb2p.X-250;
				cb3p.X=cb3p.X-250;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
				this.comboBox3.Location=cb3p;
			}
			else
			{
				this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
				this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
				this.label2 = new System.Windows.Forms.Label();
				this.label1 = new System.Windows.Forms.Label();
				
				this.panel1.Controls.Add(this.dateTimePicker1);
				this.panel1.Controls.Add(this.dateTimePicker2);
				this.panel1.Controls.Add(this.label1);
				this.panel1.Controls.Add(this.label2);
				
				// 
				// dateTimePicker1
				// 
				this.dateTimePicker1.Location = new System.Drawing.Point(86, 5);
				this.dateTimePicker1.Name = "dateTimePicker1";
				this.dateTimePicker1.Size = new System.Drawing.Size(106, 21);
				this.dateTimePicker1.TabIndex = 72;
				// 
				// dateTimePicker2
				// 
				this.dateTimePicker2.Location = new System.Drawing.Point(221, 5);
				this.dateTimePicker2.Name = "dateTimePicker2";
				this.dateTimePicker2.Size = new System.Drawing.Size(107, 21);
				this.dateTimePicker2.TabIndex = 73;
				// 
				// label1
				// 
				this.label1.Location = new System.Drawing.Point(61, 8);
				this.label1.Name = "label1";
				this.label1.Size = new System.Drawing.Size(19, 18);
				this.label1.TabIndex = 74;
				this.label1.Text = "起";
				// 
				// label2
				// 
				this.label2.Location = new System.Drawing.Point(198, 8);
				this.label2.Name = "label2";
				this.label2.Size = new System.Drawing.Size(17, 18);
				this.label2.TabIndex = 75;
				this.label2.Text = "止";
				cb1p.X=cb1p.X+250;
				cb2p.X=cb2p.X+250;
				cb3p.X=cb3p.X+250;
				this.comboBox1.Location=cb1p;
				this.comboBox2.Location=cb2p;
				this.comboBox3.Location=cb3p;
//				this.dateTimePicker1.ValueChanged += new EventHandler(conditionChanged);
//				this.dateTimePicker2.ValueChanged += new EventHandler(conditionChanged);
			}
		}
		
		
		/// <summary>
		/// 新增
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button1Click(object sender, EventArgs e)
		{
			UICheck uc = new UICheck("a");
			uc.ShowDialog();
		}
	}
}
