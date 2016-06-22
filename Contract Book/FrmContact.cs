using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Contract_Book
{
    public partial class FrmContact : Form
    {
        FrmMain frmMain;
        string logInId;
        DataTable dtGroup = new DataTable();
        byte[] imageData;

        public FrmContact(FrmMain ff, string id)
        {
            InitializeComponent();
            frmMain = ff;
            logInId = id;
        }

        private void FrmContact_Load(object sender, EventArgs e)
        {
            // load default image into imageData
            MemoryStream ms = new MemoryStream();
            Properties.Resources.no_photo.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            imageData = ms.ToArray();

            // assign group name into the group combo box
            try
            {                
                Gateway gatewayObject = new Gateway();                
                string sqlString = "select group_id, group_name from mygroup where log_in_id='" + logInId + "'";
                dtGroup = gatewayObject.SelectData(sqlString);

                foreach (DataRow dr in dtGroup.Rows)
                {
                    cmbGroup.Items.Add(dr["group_name"]);                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Control[] treeOfGroup = frmMain.Controls.Find("treeOfGroup", true);      
            string selectedGroupName = ((TreeView)treeOfGroup[0]).SelectedNode.Text;

            cmbGroup.SelectedItem = selectedGroupName;    //select the group name as selected node

            //filtering the openFileDialog file type
            openFileDialog1.Filter = "Image File(.jpg)|*.jpg|Image File(.jpeg)|*.jpeg|Image File(.png)|*.png|Bitmap File(.bmp)|*.bmp|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            //MessageBox.Show(Properties.Resources.no_photo.ToString());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Guid contactId = Guid.NewGuid();
                string groupId = null;
                foreach (DataRow dr in dtGroup.Rows)
                {
                    if (dr["group_name"].ToString() == cmbGroup.SelectedItem.ToString())
                    {
                        groupId = dr["group_id"].ToString();
                    }
                }
                

                Gateway gatewayObject = new Gateway();
                List<SqlParameter> parameter = new List<SqlParameter>();

                string insertStringContact = "insert into [my_contact] values(@contact_id, @first_name, @last_name, @group_id, @log_in_id," +
                    "@address, @city, @zip_code, @country, @phone1, @phone2, @phone3, @phone4, @phone5, @email1, @email2, @email3, @webpage," +
                    "@school, @sgroup, @college, @cgroup, @university, @subject," +
                    "@title, @department, @bsaddress, @bscity, @bszip_code, @bscountry, @bsphone1, @bsphone2, @fax, @bsemail, @bswebpage," +
                    "@date_of_birth, @picture, @notes)";

                parameter.Add(new SqlParameter("@contact_id", contactId.ToString()));
                parameter.Add(new SqlParameter("@first_name", txtFirstName.Text));
                parameter.Add(new SqlParameter("@last_name", txtLastName.Text));
                parameter.Add(new SqlParameter("@group_id", groupId));
                parameter.Add(new SqlParameter("@log_in_id", logInId));

                parameter.Add(new SqlParameter("@address", txtAddress.Text));
                parameter.Add(new SqlParameter("@city", txtCity.Text));
                parameter.Add(new SqlParameter("@zip_code", txtZipCode.Text));
                parameter.Add(new SqlParameter("@country", txtCountry.Text));
                parameter.Add(new SqlParameter("@phone1", txtPhone1.Text));
                parameter.Add(new SqlParameter("@phone2", txtPhone2.Text));
                parameter.Add(new SqlParameter("@phone3", txtPhone3.Text));
                parameter.Add(new SqlParameter("@phone4", txtPhone4.Text));
                parameter.Add(new SqlParameter("@phone5", txtPhone5.Text));
                parameter.Add(new SqlParameter("@email1", txtEmail1.Text));
                parameter.Add(new SqlParameter("@email2", txtEmail2.Text));
                parameter.Add(new SqlParameter("@email3", txtEmail3.Text));
                parameter.Add(new SqlParameter("@webpage", txtWebPage.Text));

                parameter.Add(new SqlParameter("@school", txtSchool.Text));
                parameter.Add(new SqlParameter("@sgroup", txtSGroup.Text));
                parameter.Add(new SqlParameter("@college", txtCollege.Text));
                parameter.Add(new SqlParameter("@cgroup", txtCGroup.Text));
                parameter.Add(new SqlParameter("@university", txtUniversity.Text));
                parameter.Add(new SqlParameter("@subject", txtSubject.Text));
                
                parameter.Add(new SqlParameter("@title", txtBsTitle.Text));
                parameter.Add(new SqlParameter("@department", txtBsDepartment.Text));
                parameter.Add(new SqlParameter("@bsaddress", txtBsAddress.Text));
                parameter.Add(new SqlParameter("@bscity", txtBsCity.Text));
                parameter.Add(new SqlParameter("@bszip_code", txtBsZipCode.Text));
                parameter.Add(new SqlParameter("@bscountry", txtBsCountry.Text));
                parameter.Add(new SqlParameter("@bsphone1", txtBsPhone1.Text));
                parameter.Add(new SqlParameter("@bsphone2", txtBsPhone2.Text));
                parameter.Add(new SqlParameter("@fax", txtBsFax.Text));
                parameter.Add(new SqlParameter("@bsemail", txtBsEmail.Text));
                parameter.Add(new SqlParameter("@bswebpage", txtBsWebPage.Text));
        
                parameter.Add(new SqlParameter("@date_of_birth", dateTimePicker1.Value));
                parameter.Add(new SqlParameter("@picture", (object)imageData));
                parameter.Add(new SqlParameter("@notes", txtNotes.Text));

                if (gatewayObject.InsertData(insertStringContact, parameter))
                {
                    MessageBox.Show("Data inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Close();
            }
        }

        private byte[] ReadFile(string sPath)   //read image file into byte array
        {
            byte[] data = null;
            try
            {
                FileInfo fileInfo = new FileInfo(sPath);
                long numBytes = fileInfo.Length;

                FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

                BinaryReader br = new BinaryReader(fStream);

                data = br.ReadBytes((int)numBytes);    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return data;
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            bool flag = false;
            if (cmbGroup.SelectedIndex > -1)
            {
                flag = true;
            }
                
            if (txtFirstName.Text != string.Empty && flag)
            {
                btnSave.Enabled = true;
                lblAddAnotherContact.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                lblAddAnotherContact.Enabled = false;
            }
        }

        private void lblAddAnotherContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string aa = dateTimePicker1.Value.ToString();
            MessageBox.Show(aa);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {                
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                imageData = ReadFile(openFileDialog1.FileName);

            }
        }

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtFirstName.Text != string.Empty)
            {
                btnSave.Enabled = true;
                lblAddAnotherContact.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                lblAddAnotherContact.Enabled = false;
            }
        }

    }
}
