/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/5/26
 * Time: 21:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WatchCilent
{
	partial class Dbconfig
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
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(349, 46);
			this.label1.TabIndex = 0;
			this.label1.Text = "欢迎使用中科惠软更新包管理系统";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(249, 18);
			this.label2.TabIndex = 1;
			this.label2.Text = "请先设置数据库路径：";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(12, 86);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(349, 21);
			this.textBox1.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(151, 113);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "保存";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// Dbconfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(375, 148);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.Name = "Dbconfig";
			this.Text = "数据库配置";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
	}
}
