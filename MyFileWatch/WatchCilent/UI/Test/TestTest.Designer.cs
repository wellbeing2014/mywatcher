/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-7-21
 * 时间: 17:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace WatchCilent.UI.Test
{
	partial class TestTest
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.testListUI1 = new WatchCilent.UI.Test.TestListUI();
			this.SuspendLayout();
			// 
			// testListUI1
			// 
			this.testListUI1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testListUI1.Location = new System.Drawing.Point(0, 0);
			this.testListUI1.Name = "testListUI1";
			this.testListUI1.Size = new System.Drawing.Size(702, 390);
			this.testListUI1.TabIndex = 0;
			// 
			// TestTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(702, 390);
			this.Controls.Add(this.testListUI1);
			this.Name = "TestTest";
			this.Text = "TestTest";
			this.ResumeLayout(false);
		}
		private WatchCilent.UI.Test.TestListUI testListUI1;
	}
}
