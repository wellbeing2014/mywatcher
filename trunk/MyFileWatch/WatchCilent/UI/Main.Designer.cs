/*
 * 由SharpDevelop创建。
 * 用户： wellbeing
 * 日期: 2011/6/26
 * 时间: 0:36
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace WatchCilent.UI
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.packageUI1 = new WatchCilent.PackageUI();
			this.testlistUI1 = new WatchCilent.UI.Test.TestListUI();
			this.publishUI1 = new WatchCilent.UI.Pub.PublishUI();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.更新包ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.发布ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.新建缺陷单元ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.接收自动处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.接收不自动处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
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
			// panel2
			// 
			this.panel2.Controls.Add(this.packageUI1);
			this.panel2.Controls.Add(this.publishUI1);
			this.panel2.Controls.Add(this.testlistUI1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 24);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(909, 463);
			this.panel2.TabIndex = 2;
			// 
			// packageUI1
			// 
			this.packageUI1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.packageUI1.Location = new System.Drawing.Point(0, 0);
			this.packageUI1.Name = "packageUI1";
			this.packageUI1.Size = new System.Drawing.Size(909, 463);
			this.packageUI1.TabIndex = 0;
			
			/// <summary>
			/// zengjia 
			/// </summary>
			this.testlistUI1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testlistUI1.Location = new System.Drawing.Point(0, 0);
			this.testlistUI1.Name = "testlistUI1";
			this.testlistUI1.Visible = false;
			
			
			this.publishUI1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.publishUI1.Location = new System.Drawing.Point(0, 0);
			this.publishUI1.Name = "publishUI1";
			this.publishUI1.Visible = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.更新包ToolStripMenuItem,
									this.toolStripMenuItem1,
									this.发布ToolStripMenuItem,
									this.配置ToolStripMenuItem,
									this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(909, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// 更新包ToolStripMenuItem
			// 
			this.更新包ToolStripMenuItem.Name = "更新包ToolStripMenuItem";
			this.更新包ToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
			this.更新包ToolStripMenuItem.Text = "更新包";
			this.更新包ToolStripMenuItem.Click += new System.EventHandler(this.更新包ToolStripMenuItemClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(41, 20);
			this.toolStripMenuItem1.Text = "测试";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1Click);
			// 
			// 发布ToolStripMenuItem
			// 
			this.发布ToolStripMenuItem.Name = "发布ToolStripMenuItem";
			this.发布ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.发布ToolStripMenuItem.Text = "发布";
			this.发布ToolStripMenuItem.Click += new System.EventHandler(this.发布ToolStripMenuItemClick);
			// 
			// 配置ToolStripMenuItem
			// 
			this.配置ToolStripMenuItem.Name = "配置ToolStripMenuItem";
			this.配置ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.配置ToolStripMenuItem.Text = "配置";
			this.配置ToolStripMenuItem.Click += new System.EventHandler(this.配置ToolStripMenuItemClick);
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
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// contentsToolStripMenuItem
			// 
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.contentsToolStripMenuItem.Text = "&Contents";
			// 
			// indexToolStripMenuItem
			// 
			this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
			this.indexToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.indexToolStripMenuItem.Text = "&Index";
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.searchToolStripMenuItem.Text = "&Search";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(115, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "notifyIcon1";
			this.notifyIcon1.Visible = true;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.新建缺陷单元ToolStripMenuItem,
									this.接收自动处理ToolStripMenuItem,
									this.接收不自动处理ToolStripMenuItem,
									this.退出ToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(205, 114);
			// 
			// 新建缺陷单元ToolStripMenuItem
			// 
			this.新建缺陷单元ToolStripMenuItem.Name = "新建缺陷单元ToolStripMenuItem";
			this.新建缺陷单元ToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.新建缺陷单元ToolStripMenuItem.Text = "新建缺陷单元(CTRL+N)";
			this.新建缺陷单元ToolStripMenuItem.Click += new System.EventHandler(this.新建缺陷单元ToolStripMenuItemClick);
			// 
			// 接收自动处理ToolStripMenuItem
			// 
			this.接收自动处理ToolStripMenuItem.Name = "接收自动处理ToolStripMenuItem";
			this.接收自动处理ToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.接收自动处理ToolStripMenuItem.Text = "接收自动处理";
			// 
			// 接收不自动处理ToolStripMenuItem
			// 
			this.接收不自动处理ToolStripMenuItem.Name = "接收不自动处理ToolStripMenuItem";
			this.接收不自动处理ToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.接收不自动处理ToolStripMenuItem.Text = "接收不自动处理";
			// 
			// 退出ToolStripMenuItem
			// 
			this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
			this.退出ToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
			this.退出ToolStripMenuItem.Text = "退出";
			this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItemClick);
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
			this.panel2.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolStripMenuItem 发布ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 接收不自动处理ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 接收自动处理ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 新建缺陷单元ToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 配置ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 更新包ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.Panel panel1;
		private WatchCilent.PackageUI packageUI1;
		private WatchCilent.UI.Pub.PublishUI  publishUI1;
		private WatchCilent.UI.Test.TestListUI testlistUI1;
		//private WatchCilent.PackageUI packageUI1;
		
		
	}
}
