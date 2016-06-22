using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Contract_Book
{
    public partial class FrmCollumnsSetting : Form
    {
        string logInId;
        public FrmCollumnsSetting(string id)
        {
            InitializeComponent();
            logInId = id;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int indexOfItem = listBox1.IndexFromPoint(e.X, e.Y);
            this.Text = indexOfItem.ToString();
        }

    }
}
