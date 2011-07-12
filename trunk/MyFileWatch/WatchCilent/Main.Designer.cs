/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011/6/26
 * 时间: 0:36
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace WatchCilent
{
	partial class Main
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.panel1 = new System.Windows.Forms.Panel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.测试列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.生成测试报告ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.测试统计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.更新包ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.更新包列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.更新包统计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel2 = new System.Windows.Forms.Panel();
			this.packageUI1 = new WatchCilent.PackageUI();
			this.panel1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.menuStrip1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(909, 487);
			this.panel1.TabIndex = 1;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripMenuItem1,
									this.更新包ToolStripMenuItem,
									this.配置ToolStripMenuItem,
									this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(909, 25);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.测试列表ToolStripMenuItem,
									this.生成测试报告ToolStripMenuItem,
									this.测试统计ToolStripMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(44, 21);
			this.toolStripMenuItem1.Text = "测试";
			// 
			// 测试列表ToolStripMenuItem
			// 
			this.测试列表ToolStripMenuItem.Name = "测试列表ToolStripMenuItem";
			this.测试列表ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
			this.测试列表ToolStripMenuItem.Text = "测试列表";
			this.测试列表ToolStripMenuItem.Click += new System.EventHandler(this.测试列表ToolStripMenuItemClick);
			// 
			// 生成测试报告ToolStripMenuItem
			// 
			this.生成测试报告ToolStripMenuItem.Name = "生成测试报告ToolStripMenuItem";
			this.生成测试报告ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
			this.生成测试报告ToolStripMenuItem.Text = "生成测试报告";
			// 
			// 测试统计ToolStripMenuItem
			// 
			this.测试统计ToolStripMenuItem.Name = "测试统计ToolStripMenuItem";
			this.测试统计ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
			this.测试统计ToolStripMenuItem.Text = "测试统计";
			// 
			// 更新包ToolStripMenuItem
			// 
			this.更新包ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.更新包列表ToolStripMenuItem,
									this.更新包统计ToolStripMenuItem});
			this.更新包ToolStripMenuItem.Name = "更新包ToolStripMenuItem";
			this.更新包ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
			this.更新包ToolStripMenuItem.Text = "更新包";
			// 
			// 更新包列表ToolStripMenuItem
			// 
			this.更新包列表ToolStripMenuItem.Name = "更新包列表ToolStripMenuItem";
			this.更新包列表ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.更新包列表ToolStripMenuItem.Text = "更新包列表";
			// 
			// 更新包统计ToolStripMenuItem
			// 
			this.更新包统计ToolStripMenuItem.Name = "更新包统计ToolStripMenuItem";
			this.更新包统计ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.更新包统计ToolStripMenuItem.Text = "更新包统计";
			// 
			// 配置ToolStripMenuItem
			// 
			this.配置ToolStripMenuItem.Name = "配置ToolStripMenuItem";
			this.配置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
			this.配置ToolStripMenuItem.Text = "配置";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.contentsToolStripMenuItem,
									this.indexToolStripMenuItem,
									this.searchToolStripMenuItem,
									this.toolStripSeparator5,
									this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// contentsToolStripMenuItem
			// 
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.contentsToolStripMenuItem.Text = "&Contents";
			// 
			// indexToolStripMenuItem
			// 
			this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
			this.indexToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.indexToolStripMenuItem.Text = "&Index";
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.searchToolStripMenuItem.Text = "&Search";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(124, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.packageUI1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 25);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(909, 462);
			this.panel2.TabIndex = 2;
			// 
			// packageUI1
			// 
			this.packageUI1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.packageUI1.Location = new System.Drawing.Point(0, 0);
			this.packageUI1.Name = "packageUI1";
			this.packageUI1.Size = new System.Drawing.Size(909, 462);
			this.packageUI1.TabIndex = 0;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(909, 487);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Main";
			this.Text = "Wisoft测试管理系统";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 配置ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 更新包统计ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 更新包列表ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 更新包ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 测试统计ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 生成测试报告ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 测试列表ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.Panel panel1;
		private WatchCilent.PackageUI packageUI1;
		//private WatchCilent.PackageUI packageUI1;
		
		
	}
}
