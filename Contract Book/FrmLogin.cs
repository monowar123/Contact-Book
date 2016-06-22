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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void lblNewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmNewAccount frmNewAccount = new FrmNewAccount(this);           
            frmNewAccount.ShowDialog();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text != string.Empty && txtPassword.Text != string.Empty)
            {
                try
                {
                    string sqlString = "select log_in_id from log_in where user_name='" + txtUserName.Text + "' and password='" + txtPassword.Text + "'";
                    Gateway gateWayObject = new Gateway();
                    DataTable dataTableObject = new DataTable();

                    dataTableObject = gateWayObject.SelectData(sqlString);
                    if (dataTableObject.Rows.Count == 1)
                    {
                        string logInId = dataTableObject.Rows[0]["log_in_id"].ToString();
                        FrmMain frmMain = new FrmMain(this, logInId);
                        frmMain.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid User Name or Password", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Field should not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtUserName.Clear();
            txtPassword.Clear();
        }

    }
}
