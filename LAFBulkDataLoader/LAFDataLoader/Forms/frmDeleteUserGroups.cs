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
    public partial class frmDeleteUserGroups : Form
    {

        private List<LAFUserGroups> userGroupsToLoad;

        
        string filename;



        public frmDeleteUserGroups()
        {
            InitializeComponent();
          
           
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void optSysStable_CheckedChanged(object sender, EventArgs e)
        {

        }

        

        private void setApplicationCombo()
        {
            var appData = DataProcessingMethods.GetLAFApplications();

            foreach (LAFApplicationData row in appData)
            {
                if (!comboApplication2.Items.Contains(row.Name))
                {
                    comboApplication2.Items.Add(row.Name);
                }
            }
        }

        private void frmDeleteUserGroups_Load(object sender, EventArgs e)
        {

            setApplicationCombo();
            


        }

        private void comboApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            btnBrowse.Enabled = true;

        }

        private void Next_Click(object sender, EventArgs e)
        {
            string appName = comboApplication2.Text;
            //string appName = ((ComboBoxItem)comboApplication.SelectedItem).Content.ToString();
            DataProcessingMethods.DeleteUserGroups(userGroupsToLoad, appName, txtChangeDesc.Text, filename,progressBar1,null);
            
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
                this.userGroupsToLoad = ExcelMethods.GetData<LAFUserGroups>(filename,"UserGroups");
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

