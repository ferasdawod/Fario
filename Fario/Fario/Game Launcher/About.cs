using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IMPORT_PLATFORM
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(lblFacebook.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(lblTwitter.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(lblHotmailDA.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(lblHotmailSD.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(lblGmail.Text);
        }
    }
}
