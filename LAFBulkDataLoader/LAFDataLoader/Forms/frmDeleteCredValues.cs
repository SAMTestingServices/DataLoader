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
    public partial class frmDeleteCredValues : Form
    {
 
        private List<CredentialValues> credentialsToLoad;
        private string credentialDescriptionType;

        private string appName;

        string filename;



        public frmDeleteCredValues()
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

        private void frmDeleteCredValues_Load(object sender, EventArgs e)
        {

            setApplicationCombo();
            setCredentialCombo(null);
            this.appName = comboApplication.Text;

        }

        private void comboApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.appName = comboApplication.Text;
            setCredentialCombo(appName);
        }

        private void Next_Click(object sender, EventArgs e)
        {
            credentialDescriptionType = comboCredential.Text;
            DataProcessingMethods.DeleteCredentialValues(credentialsToLoad,  credentialDescriptionType, txtChangeDesc.Text, filename,progressBar1,null, appName);
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
                //

                this.credentialsToLoad = ExcelMethods.GetData<CredentialValues>(filename,"CredentialValues");
                MessageBox.Show("File upload successful. Number of credential records to create = " + credentialsToLoad.Count, "Load successful",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtChangeDesc.Enabled = true;
                

            }
        }

        private void txtChangeDesc_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChangeDesc.Text)) { Next.Enabled = false; }
            else Next.Enabled = true;
        }
    }
            }

