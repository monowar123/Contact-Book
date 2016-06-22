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
    public partial class FrmMain : Form
    {
        FrmLogin frmLogIn;
        string logInId = "989BF1F2-2DBF-414E-AAA5-19BEF0A533A8";

        public FrmMain()
        {
            InitializeComponent();            
        }

        public FrmMain(FrmLogin ff, string id)
        {
            InitializeComponent();
            frmLogIn = ff;
            logInId = id;
            frmLogIn.Hide();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            LoadGroupName();

            try
            {
                Gateway gatewayObject = new Gateway();
                DataTable dtObject = new DataTable();
                DataTable dtCollName = new DataTable();
                string colNameString = null;
                string selectColl = "select * from columns where log_in_id='" + logInId + "';";
                dtCollName = gatewayObject.SelectData(selectColl);
                foreach (DataRow dr in dtCollName.Rows)
                {
                    colNameString += ",";
                    colNameString += dr["coll_name"];
                }
                string sqlString = "select contact_id, first_name, last_name" + colNameString + " from my_contact where my_contact.log_in_id='" + logInId + "'";

                dtObject = gatewayObject.SelectData(sqlString);

                dataGridView1.DataSource = dtObject;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cmbSearchItem.SelectedIndex = 0;
        }

        private void LoadGroupName()
        {
            //load the treeOfGroup item form database
            //load the group name into the tree view
            Gateway gatewayObject = new Gateway();
            string sqlString = "select group_id, group_name from mygroup where log_in_id='" + logInId + "'";
            DataTable dtGroupName = new DataTable();
            dtGroupName = gatewayObject.SelectData(sqlString);

            foreach (DataRow dr in dtGroupName.Rows)
            {
                TreeNode myNode = new TreeNode(dr["group_name"].ToString());
                myNode.ImageIndex = 1;
                myNode.SelectedImageIndex = 1;
                treeOfGroup.Nodes[0].Nodes.Add(myNode);
            }
            treeOfGroup.ExpandAll();
            treeOfGroup.SelectedNode = treeOfGroup.Nodes[0];
        }

        private void newGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmNewGroup frmNewGroup = new FrmNewGroup(this, logInId);
            frmNewGroup.ShowDialog();
        }

        private void renameGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNewGroup frmNewGroup = new FrmNewGroup(this, logInId);
           
            Bitmap myBitmap = Properties.Resources.pencil;        //convert bitmap to icon
            IntPtr myPtr = myBitmap.GetHicon();
            Icon myIcon = Icon.FromHandle(myPtr);

            frmNewGroup.Icon = myIcon;
            frmNewGroup.Text = "Rename Group";
            frmNewGroup.ShowDialog();
        }

        private void deleteGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newContactToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmContact frmContact = new FrmContact(this, logInId);
            frmContact.ShowDialog();
        }

        private void setFontsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void setColumnsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmCollumnsSetting frmCollumnsSetting = new FrmCollumnsSetting(logInId);
            frmCollumnsSetting.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)   //log out
        {
            this.Close();
            frmLogIn.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)   // show details
        {
            FrmContact frmContact = new FrmContact(this, logInId);
            frmContact.Text = "Details";

            Bitmap myBitmap = Properties.Resources.Documents;        //convert bitmap to icon
            IntPtr myPtr = myBitmap.GetHicon();
            Icon myIcon = Icon.FromHandle(myPtr);

            frmContact.Icon = myIcon;
            frmContact.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

    }
}
