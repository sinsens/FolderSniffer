using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FolderSniffer
{
    public partial class FormLogView : Form
    {
        public FormLogView()
        {
            InitializeComponent();
        }
        
        public void LogCat(string msg) {
            var t = DateTime.Now.ToLongTimeString();
            tbLog.AppendText("\n" + t + " " + msg+"\r");
        }
    }
}
