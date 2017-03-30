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
    public partial class frmLAFDAUserGroups : Form
    {
       
        private List<DAUserGroups> userGroupsToLoad;
       
        
        string filename;



        public frmLAFDAUserGroups()
        {
            InitializeComponent();
            
  
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void optSysStable_CheckedChanged(object sender, EventArgs e)
        {

        }

        
      

        private void frmLAFDAUserGroups_Load(object sender, EventArgs e)
        {

            //setApplicationCombo();
            


        }

        private void comboApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            btnBrowse.Enabled = true;

        }

        private void Next_Click(object sender, EventArgs e)
        {
           
            
           DataProcessingMethods.InsertLAFDAUserGroups(userGroupsToLoad, "LAF", txtChangeDesc.Text, filename,progressBar1);
            Next.Enabled = false;
            txtChangeDesc.Text = null;
            filePath.Text = null;
            btnBrowse.Enabled = true;

                
        }
    

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void comboCredential_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnBrowse.Enabled = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                filename = openFileDialog1.FileName;
                
                filePath.Text = filename;
                this.userGroupsToLoad = ExcelMethods.GetData<DAUserGroups>(filename, "DAUserGroups");
                MessageBox.Show("File read successful. Number of  User Groups to create = " + userGroupsToLoad.Count, "File read complete",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtChangeDesc.Enabled = true;

            }
        }

        private void filePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtChangeDesc_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChangeDesc.Text)) { Next.Enabled = false; }
            else Next.Enabled = true;
        }
    }
            }

