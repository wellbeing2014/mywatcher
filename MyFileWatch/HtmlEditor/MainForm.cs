using System;  
using System.Collections.Generic;  
using System.ComponentModel;  
using System.Data;  
using System.Drawing;  
using System.Linq;  
using System.Text;  
using System.Windows.Forms;  
  
namespace HtmlEditor  
{  
    public partial class MainForm : Form  
    {  
        public MainForm()  
        {  
            InitializeComponent();  
        }  
 
        private void MainForm_Load(object sender, EventArgs e)  
        {  
            this.webBrowser.DocumentText = string.Empty;  
            this.webBrowser.Document.ExecCommand("EditMode", false, null);  
            this.webBrowser.Document.ExecCommand("LiveResize", false, null);  
  
            foreach (FontFamily font in FontFamily.Families)  
            {  
                this.FontNamesComboBox.Items.Add(font.Name);  
            }  
        }  
  
        private void BoldButton_Click(object sender, EventArgs e)  
        {  
            this.webBrowser.Document.ExecCommand("Bold", false, null);  
        }  
  
        private void ShowHtmlSourceButton_Click(object sender, EventArgs e)  
        {  
            MessageBox.Show(this.webBrowser.DocumentText);  
        }  
  
        private void FontNamesComboBox_SelectedIndexChanged(object sender, EventArgs e)  
        {  
            this.webBrowser.Document.ExecCommand("FontName", false,  
                this.FontNamesComboBox.Text);  
        }  
  
        private void UnderlineButton_Click(object sender, EventArgs e)  
        {  
           this.webBrowser.Document.ExecCommand("Underline", false, null);  
        }  
    }  
}  


