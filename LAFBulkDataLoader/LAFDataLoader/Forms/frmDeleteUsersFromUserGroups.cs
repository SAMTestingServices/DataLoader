using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApplication3
{
    public partial class frmDeleteUsersFromUserGroups : Form
    {
        
        private List<UserGroupMembers> userGroupMembersToLoad;

        string filename;

        private void frmDeleteUsersFromUserGroups_Load(object sender, EventArgs e)
        {
            
        


        }

        public frmDeleteUsersFromUserGroups()
        {
            InitializeComponent();
    
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void optSysStable_CheckedChanged(object sender, EventArgs e)
        {

        }

        

        


      

        private void Next_Click(object sender, EventArgs e)
        {
            labelUploading.Visible = true;
            DataProcessingMethods.DeleteUserGroupMembers(userGroupMembersToLoad, txtChangeDesc.Text,filename, progressBar1,null);
            labelUploading.Visible = false;


        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

 

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                filename = openFileDialog1.FileName;
              
                filePath.Text = filename;
                this.userGroupMembersToLoad = ExcelMethods.GetData<UserGroupMembers>(filename,"UserGroupMembers");
                MessageBox.Show("File upload successful. Number of Users to add to user Groups = " + userGroupMembersToLoad.Count, "Load successful",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtChangeDesc.Enabled = true;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void txtChangeDesc_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChangeDesc.Text)) { Next.Enabled = false; }
            else Next.Enabled = true;
        }
    }
            }

