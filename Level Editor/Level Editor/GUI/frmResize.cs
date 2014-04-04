using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Level_Editor
{
    public partial class frmResize : Form
    {
        public frmResize()
        {
            InitializeComponent();
        }

        private bool okClicked;
        public bool ClickedOk
        {
            get { return okClicked; }
        }

        private int newWidth;
        public int MapWidth
        {
            get { return newWidth; }
        }

        private int newHeight;
        public int MapHeight
        {
            get { return newHeight; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Enter Values For The Map Height And Width");
                return;
            }
            newWidth = int.Parse(textBox1.Text);
            newHeight = int.Parse(textBox2.Text);
            okClicked = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            okClicked = false;
        }


    }
}
