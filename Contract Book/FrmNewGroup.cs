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
    public partial class FrmNewGroup : Form
    {
        FrmMain frmMain;
        string logInId;
        Control[] treeOfGroup;

        public FrmNewGroup(FrmMain ff, string id)
        {
            InitializeComponent();
            frmMain = ff;
            logInId = id;
        }

        private void FrmNewGroup_Load(object sender, EventArgs e)
        {
            if (this.Text == "Rename Group")
            {                
                treeOfGroup = frmMain.Controls.Find("treeOfGroup", true);   // get treeview from frmMain               
                
                txtGroup.Text = ((TreeView)treeOfGroup[0]).SelectedNode.Text;                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.Text == "New Group")
            {
                if (txtGroup.Text != string.Empty)
                {
                    try
                    {
                        treeOfGroup = frmMain.Controls.Find("treeOfGroup", true);

                        Guid uniqueId;   //generate unique id                    

                        Gateway gateWayObject = new Gateway();
                        List<SqlParameter> parameter = new List<SqlParameter>();

                        uniqueId = Guid.NewGuid();    //generate unique id

                        string insertString = "if not exists (select group_name from [mygroup] where group_name='" + txtGroup.Text +
                            "') insert into [mygroup] (group_id, group_name, log_in_id) values(@group_id, @group_name, @log_in_id)";

                        parameter.Add(new SqlParameter("@group_id", uniqueId.ToString()));
                        parameter.Add(new SqlParameter("@group_name", txtGroup.Text));
                        parameter.Add(new SqlParameter("@log_in_id", logInId));

                        if (gateWayObject.InsertData(insertString, parameter))
                        {
                            TreeNode myNode = new TreeNode(txtGroup.Text);
                            myNode.ImageIndex = 1;
                            myNode.SelectedImageIndex = 1;
                            ((TreeView)treeOfGroup[0]).Nodes[0].Nodes.Add(myNode);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("This group already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                   
                }
                else
                {
                    MessageBox.Show("Field should not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (this.Text == "Rename Group")
            {
                if (txtGroup.Text != string.Empty)
                {
                    try
                    {
                        string updateString = "if not exists (select group_name from mygroup where group_name='" + txtGroup.Text + "')" +
                        "update mygroup set group_name='" + txtGroup.Text + "' where group_name='" + ((TreeView)treeOfGroup[0]).SelectedNode.Text + "'";
                        Gateway gatewayObject = new Gateway();
                        if (gatewayObject.updateData(updateString))
                        {
                            ((TreeView)treeOfGroup[0]).SelectedNode.Text = txtGroup.Text;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("This group already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                    
                }
                else
                {
                    MessageBox.Show("Field should not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
