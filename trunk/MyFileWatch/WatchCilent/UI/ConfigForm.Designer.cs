﻿using System;
using System.Collections.Generic;
using WatchCore.Common;
using WatchCore.dao;
using WatchCore.pojo;

/*
 * Created by SharpDevelop.
 * User: wellbeing
 * Date: 2011/5/20
 * Time: 22:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WatchCilent
{
	partial class ConfigForm
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
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.button5 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.button9 = new System.Windows.Forms.Button();
			this.button10 = new System.Windows.Forms.Button();
			this.listView3 = new System.Windows.Forms.ListView();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.textBox6 = new System.Windows.Forms.TextBox();
			this.button11 = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.listView2 = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textBox8 = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textBox7 = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.button8 = new System.Windows.Forms.Button();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.button12 = new System.Windows.Forms.Button();
			this.button13 = new System.Windows.Forms.Button();
			this.listBox3 = new System.Windows.Forms.ListBox();
			this.listBox4 = new System.Windows.Forms.ListBox();
			this.label15 = new System.Windows.Forms.Label();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.tabPage4.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.panel1);
			this.tabPage4.Location = new System.Drawing.Point(4, 21);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(655, 372);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "版本项目关联";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.button5);
			this.panel1.Controls.Add(this.button4);
			this.panel1.Controls.Add(this.listBox2);
			this.panel1.Controls.Add(this.listBox1);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.comboBox2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(649, 366);
			this.panel1.TabIndex = 0;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(378, 40);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 13);
			this.label6.TabIndex = 7;
			this.label6.Text = "已选版本";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(5, 37);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "待选版本";
			// 
			// button5
			// 
			this.button5.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.button5.Location = new System.Drawing.Point(287, 207);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 5;
			this.button5.Text = "撤选";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.Button5Click);
			// 
			// button4
			// 
			this.button4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.button4.Location = new System.Drawing.Point(287, 141);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 4;
			this.button4.Text = "选择";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.Button4Click);
			// 
			// listBox2
			// 
			this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox2.FormattingEnabled = true;
			this.listBox2.ItemHeight = 12;
			this.listBox2.Location = new System.Drawing.Point(378, 56);
			this.listBox2.Name = "listBox2";
			this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listBox2.Size = new System.Drawing.Size(266, 292);
			this.listBox2.TabIndex = 3;
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(5, 56);
			this.listBox1.Name = "listBox1";
			this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listBox1.Size = new System.Drawing.Size(266, 292);
			this.listBox1.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 6);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 23);
			this.label4.TabIndex = 1;
			this.label4.Text = "项目名称：";
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(79, 3);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(201, 20);
			this.comboBox2.TabIndex = 0;
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.ComboBox2SelectedIndexChanged);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.button9);
			this.tabPage3.Controls.Add(this.button10);
			this.tabPage3.Controls.Add(this.listView3);
			this.tabPage3.Controls.Add(this.groupBox3);
			this.tabPage3.Controls.Add(this.button11);
			this.tabPage3.Location = new System.Drawing.Point(4, 21);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(655, 372);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "责任人管理";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(64, 61);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(50, 23);
			this.button9.TabIndex = 8;
			this.button9.Text = "修改";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.Button9Click);
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(8, 61);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(50, 23);
			this.button10.TabIndex = 7;
			this.button10.Text = "新增";
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new System.EventHandler(this.Button10Click);
			// 
			// listView3
			// 
			this.listView3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.columnHeader7,
									this.columnHeader8});
			this.listView3.FullRowSelect = true;
			this.listView3.GridLines = true;
			this.listView3.Location = new System.Drawing.Point(8, 90);
			this.listView3.Name = "listView3";
			this.listView3.Size = new System.Drawing.Size(639, 276);
			this.listView3.TabIndex = 6;
			this.listView3.UseCompatibleStateImageBehavior = false;
			this.listView3.View = System.Windows.Forms.View.Details;
			this.listView3.Click += new System.EventHandler(this.ListView3Click);
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "姓名";
			this.columnHeader7.Width = 141;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "IP地址";
			this.columnHeader8.Width = 196;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.textBox5);
			this.groupBox3.Controls.Add(this.textBox6);
			this.groupBox3.Location = new System.Drawing.Point(8, 4);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(639, 51);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "编辑";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(229, 17);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(59, 23);
			this.label11.TabIndex = 3;
			this.label11.Text = "IP地址：";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 17);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(44, 23);
			this.label12.TabIndex = 2;
			this.label12.Text = "姓名：";
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(291, 14);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(157, 21);
			this.textBox5.TabIndex = 1;
			// 
			// textBox6
			// 
			this.textBox6.Location = new System.Drawing.Point(56, 14);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new System.Drawing.Size(149, 21);
			this.textBox6.TabIndex = 0;
			// 
			// button11
			// 
			this.button11.Location = new System.Drawing.Point(120, 61);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(50, 23);
			this.button11.TabIndex = 9;
			this.button11.Text = "删除";
			this.button11.UseVisualStyleBackColor = true;
			this.button11.Click += new System.EventHandler(this.Button11Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.button6);
			this.tabPage2.Controls.Add(this.button7);
			this.tabPage2.Controls.Add(this.listView2);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Controls.Add(this.button8);
			this.tabPage2.Location = new System.Drawing.Point(4, 21);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(655, 372);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "项目管理";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(64, 86);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(50, 23);
			this.button6.TabIndex = 8;
			this.button6.Text = "修改";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.Button6Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(8, 86);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(50, 23);
			this.button7.TabIndex = 7;
			this.button7.Text = "新增";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.Button7Click);
			// 
			// listView2
			// 
			this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.columnHeader4,
									this.columnHeader5,
									this.columnHeader6,
									this.columnHeader9});
			this.listView2.FullRowSelect = true;
			this.listView2.GridLines = true;
			this.listView2.Location = new System.Drawing.Point(8, 120);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(639, 246);
			this.listView2.TabIndex = 6;
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = System.Windows.Forms.View.Details;
			this.listView2.Click += new System.EventHandler(this.ListView2Click);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "项目名称";
			this.columnHeader4.Width = 101;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "路径:";
			this.columnHeader5.Width = 201;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "URL";
			this.columnHeader6.Width = 224;
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "FTP地址";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textBox8);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.textBox7);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.textBox3);
			this.groupBox2.Controls.Add(this.textBox4);
			this.groupBox2.Location = new System.Drawing.Point(8, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(639, 76);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "编辑";
			// 
			// textBox8
			// 
			this.textBox8.Location = new System.Drawing.Point(73, 43);
			this.textBox8.Name = "textBox8";
			this.textBox8.Size = new System.Drawing.Size(257, 21);
			this.textBox8.TabIndex = 7;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(6, 46);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(61, 23);
			this.label10.TabIndex = 6;
			this.label10.Text = "FTP路径:";
			// 
			// textBox7
			// 
			this.textBox7.Location = new System.Drawing.Point(380, 41);
			this.textBox7.Name = "textBox7";
			this.textBox7.Size = new System.Drawing.Size(253, 21);
			this.textBox7.TabIndex = 5;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(341, 44);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(33, 23);
			this.label7.TabIndex = 4;
			this.label7.Text = "URL:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(336, 17);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(38, 23);
			this.label8.TabIndex = 3;
			this.label8.Text = "路径:";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 17);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(61, 23);
			this.label9.TabIndex = 2;
			this.label9.Text = "项目名称:";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(380, 14);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(253, 21);
			this.textBox3.TabIndex = 1;
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(73, 14);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(257, 21);
			this.textBox4.TabIndex = 0;
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(120, 86);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(50, 23);
			this.button8.TabIndex = 9;
			this.button8.Text = "删除";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.Button8Click);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.button3);
			this.tabPage1.Controls.Add(this.button2);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.listView1);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 21);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(655, 372);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "版本管理";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(120, 66);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(50, 23);
			this.button3.TabIndex = 4;
			this.button3.Text = "删除";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.Button3Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(64, 66);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(50, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "修改";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 66);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(50, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "新增";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.columnHeader1,
									this.columnHeader2,
									this.columnHeader3});
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.Location = new System.Drawing.Point(8, 95);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(639, 273);
			this.listView1.TabIndex = 1;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.Click += new System.EventHandler(this.listView1_Click);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "版本名称";
			this.columnHeader1.Width = 131;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "代码";
			this.columnHeader2.Width = 130;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "责任人";
			this.columnHeader3.Width = 86;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Location = new System.Drawing.Point(8, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(639, 51);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "编辑";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(487, 14);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 20);
			this.comboBox1.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(426, 17);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "责任人：";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(229, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "代码：";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "名称：";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(279, 14);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(130, 21);
			this.textBox2.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(56, 14);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(149, 21);
			this.textBox1.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(663, 397);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.panel2);
			this.tabPage5.Location = new System.Drawing.Point(4, 21);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage5.Size = new System.Drawing.Size(655, 372);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "项目版本关联";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label13);
			this.panel2.Controls.Add(this.label14);
			this.panel2.Controls.Add(this.button12);
			this.panel2.Controls.Add(this.button13);
			this.panel2.Controls.Add(this.listBox3);
			this.panel2.Controls.Add(this.listBox4);
			this.panel2.Controls.Add(this.label15);
			this.panel2.Controls.Add(this.comboBox3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(649, 366);
			this.panel2.TabIndex = 0;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(378, 48);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(100, 13);
			this.label13.TabIndex = 15;
			this.label13.Text = "已选项目";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(5, 45);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 13);
			this.label14.TabIndex = 14;
			this.label14.Text = "待选项目";
			// 
			// button12
			// 
			this.button12.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.button12.Location = new System.Drawing.Point(287, 215);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(75, 23);
			this.button12.TabIndex = 13;
			this.button12.Text = "撤选";
			this.button12.UseVisualStyleBackColor = true;
			this.button12.Click += new System.EventHandler(this.Button12Click);
			// 
			// button13
			// 
			this.button13.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.button13.Location = new System.Drawing.Point(287, 149);
			this.button13.Name = "button13";
			this.button13.Size = new System.Drawing.Size(75, 23);
			this.button13.TabIndex = 12;
			this.button13.Text = "选择";
			this.button13.UseVisualStyleBackColor = true;
			this.button13.Click += new System.EventHandler(this.Button13Click);
			// 
			// listBox3
			// 
			this.listBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox3.FormattingEnabled = true;
			this.listBox3.ItemHeight = 12;
			this.listBox3.Location = new System.Drawing.Point(378, 64);
			this.listBox3.Name = "listBox3";
			this.listBox3.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listBox3.Size = new System.Drawing.Size(266, 292);
			this.listBox3.TabIndex = 11;
			// 
			// listBox4
			// 
			this.listBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.listBox4.FormattingEnabled = true;
			this.listBox4.ItemHeight = 12;
			this.listBox4.Location = new System.Drawing.Point(5, 64);
			this.listBox4.Name = "listBox4";
			this.listBox4.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listBox4.Size = new System.Drawing.Size(266, 292);
			this.listBox4.TabIndex = 10;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(5, 14);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(68, 23);
			this.label15.TabIndex = 9;
			this.label15.Text = "版本名称：";
			// 
			// comboBox3
			// 
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new System.Drawing.Point(79, 11);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(201, 20);
			this.comboBox3.TabIndex = 8;
			this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.ComboBox3SelectedIndexChanged);
			// 
			// ConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(663, 397);
			this.Controls.Add(this.tabControl1);
			this.Name = "ConfigForm";
			this.Text = "配置";
			this.tabPage4.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.ListBox listBox4;
		private System.Windows.Forms.ListBox listBox3;
		private System.Windows.Forms.Button button13;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textBox8;
		private System.Windows.Forms.TextBox textBox7;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.TextBox textBox6;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ListView listView3;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ListView listView2;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl tabControl1;
		List<PersonInfo> datasource_person ;
		List<ProjectInfo> projectlist;
		List<ProjectInfo> projectlist2;
		List<ModuleInfo> modulelist;
		List<ModuleInfo> modulelist2;
		
		
		
		
		
	}
}
