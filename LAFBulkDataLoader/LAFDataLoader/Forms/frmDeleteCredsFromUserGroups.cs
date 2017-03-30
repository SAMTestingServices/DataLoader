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
    public partial class frmDeleteCredsFromUserGroups : Form
    {
 
        private List<UserGroupCredentialValues> userGroupCredentialsToLoad;
        
        private string credentialDescriptionType;


        string filename;



        public frmDeleteCredsFromUserGroups()
        {
            InitializeComponent();
      
           
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void optSysStable_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void setCredentialCombo(string appName)
        {

            comboCredential.Items.Clear();
            var credentialData = DataProcessingMethods.GetLAFCredentials( appName);

            foreach (LAFCredentialData row in credentialData)
            {
                if (!comboCredential.Items.Contains(row.Description))
                {
                    comboCredential.Items.Add(row.Description);
                }
            }
        }

        private void setApplicationCombo()
        {
            var appData = DataProcessingMethods.GetLAFApplications();

            foreach (LAFApplicationData row in appData)
            {
                if (!comboApplication.Items.Contains(row.Name))
                {
                    comboApplication.Items.Add(row.Name);
                }
            }
        }

        private void frmDeleteCredsFromUserGroups_Load(object sender, EventArgs e)
        {

            setApplicationCombo();
            setCredentialCombo(null);


        }

        private void comboApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            string appName = comboApplication.Text;
            setCredentialCombo(appName);
            btnReset.Enabled = true;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            credentialDescriptionType = comboCredential.Text;
            DataProcessingMethods.DeleteUserGroupCredentialValues(userGroupCredentialsToLoad,  txtChangeDesc.Text, filename, progressBar1,null, credentialDescriptionType);
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void comboCredential_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCredential.SelectedIndex < 0) { btnBrowse.Enabled = false; }

            else { btnBrowse.Enabled = true; }; 
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                

                try
                {
                    filename = openFileDialog1.FileName;
                    this.userGroupCredentialsToLoad = ExcelMethods.GetData<UserGroupCredentialValues>(filename, "UserGroupCredentialValues");
                    MessageBox.Show("File read successful. Number of credentials to add to user Groups = " + userGroupCredentialsToLoad.Count, "Load successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


                    comboCredential.Enabled = false;
                    comboApplication.Enabled = false;
                    txtChangeDesc.Enabled = true;
                    filePath.Text = filename;
                    btnBrowse.Enabled = false;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Exception - unable to read file.\n\nEnsure the file worksheet is called 'UserGroupCredentialValues' and has the required columns.\n\nException details:\n\n" + ex.ToString(),"Exception", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void txtChangeDesc_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChangeDesc.Text)) { btnUploadData.Enabled = false; }
            else btnUploadData.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboApplication.Enabled = true;
            comboCredential.Enabled = true;
            comboApplication.SelectedIndex=-1;
            comboCredential.SelectedIndex=-1;

            btnBrowse.Enabled = false;
            btnUploadData.Enabled = false;
            filePath.Clear();
            txtChangeDesc.Clear();
            
           

            btnReset.Enabled = false;
        }
    }
            }

