using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Contract_Book
{
    public partial class FrmNewAccount : Form
    {
        FrmLogin frmLogIn;
        public FrmNewAccount(FrmLogin ff)
        {
            InitializeComponent();
            frmLogIn = ff;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();           
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Guid uniqueId;
            Gateway gateWayObject = new Gateway();
            List<SqlParameter> parameter = new List<SqlParameter>();
            if (txtUserName.Text != string.Empty && txtPassword.Text != string.Empty && txtConfirmPassword.Text != string.Empty)
            {
                if (txtPassword.Text == txtConfirmPassword.Text)
                {
                    try
                    {
                        uniqueId = Guid.NewGuid();

                        string insertString = "if not exists (select user_name from log_in where user_name='" + txtUserName.Text +
                            "')insert into [log_in] (log_in_id, user_name, password) values(@log_in_id, @user_name, @password)";
                        parameter.Add(new SqlParameter("@log_in_id", uniqueId));
                        parameter.Add(new SqlParameter("@user_name", txtUserName.Text));
                        parameter.Add(new SqlParameter("@password", txtPassword.Text));

                        if (gateWayObject.InsertData(insertString, parameter))
                        {
                            FrmMain frmMain = new FrmMain(frmLogIn, uniqueId.ToString());      //open main from with from log in and log_in_id                     
                            this.Close();
                            frmLogIn.Hide();
                            frmMain.Show();                            
                        }
                        else
                        {
                            MessageBox.Show("The User Name already exist.\nPlease try another", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Password does not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Field should not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtUserName.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
        }

    }
}
