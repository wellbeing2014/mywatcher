﻿/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-8-3
 * 时间: 10:16
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace WatchCilent.UI.Pub
{
	partial class UploadFTP
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
			this.label1 = new System.Windows.Forms.Label();
			this.exListView1 = new EXControls.EXListView();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(12, 281);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(249, 14);
			this.label1.TabIndex = 3;
			// 
			// exListView1
			// 
			this.exListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.exListView1.ControlPadding = 4;
			this.exListView1.FullRowSelect = true;
			this.exListView1.Location = new System.Drawing.Point(12, 3);
			this.exListView1.Name = "exListView1";
			this.exListView1.OwnerDraw = true;
			this.exListView1.Size = new System.Drawing.Size(624, 238);
			this.exListView1.TabIndex = 4;
			this.exListView1.UseCompatibleStateImageBehavior = false;
			this.exListView1.View = System.Windows.Forms.View.Details;
			// 
			// UploadFTP
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(648, 309);
			this.Controls.Add(this.exListView1);
			this.Controls.Add(this.label1);
			this.Name = "UploadFTP";
			this.Text = "上传至FTP";
			this.ResumeLayout(false);
		}
		private EXControls.EXListView exListView1;
		private System.Windows.Forms.Label label1;
	}
}