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
    public partial class frmCredValues : Form
    {
        private List<CredentialValues> credentialsToLoad;
        private string credentialDescriptionType;
       
        private string appName;
        private string srcAppName;
        private string appURL;
        private string credURNName;
        private string sourceApplicationURL;
        private int credID;
        private IEnumerable<LAFApplicationData> appData;
        
        private IEnumerable<LAFCredentialData> credData;
        private int UniformResourceNameId;
        private string Urn;
        private int ApplicationID;

        string filename;
       



        public frmCredValues()
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
            this.credData = DataProcessingMethods.GetLAFCredentials(appName);

            foreach (LAFCredentialData row in credData)
            {
                if (!comboCredential.Items.Contains(row.Description))
                {
                    comboCredential.Items.Add(row.Description);
                }
            }
        }

        private void setApplicationCombo(ComboBox cmboBox)
        {
            this.appData = DataProcessingMethods.GetLAFApplications();

            foreach (LAFApplicationData row in appData)
            {
                if (!cmboBox.Items.Contains(row.Name))
                {
                    cmboBox.Items.Add(row.Name);
                }
            }
        }

        private void fmrCredValues_Load(object sender, EventArgs e)
        {

            setApplicationCombo(comboApplication);
            setCredentialCombo(null);
            this.appName = comboApplication.Text;

        }

        private void comboApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.appName = comboApplication.Text;
            if (this.appName == "LAF")
            {
                setApplicationCombo(comboSrcApp);
                comboSrcApp.Visible = true;
                lblComboSrcApp.Visible = true;
                comboCredential.Enabled = false;
            }
            else
            {
                comboSrcApp.Visible = false;
                lblComboSrcApp.Visible = false;
                comboCredential.Enabled = true;
            }
            setCredentialCombo(appName);
        }



        private void Next_Click(object sender, EventArgs e)
        {
            
            
            credentialDescriptionType = comboCredential.Text;

            var applicationSelected = DataProcessingMethods.GetApplication(appData,appName);


                this.appURL = applicationSelected.ApplicationUrl;
                this.ApplicationID = applicationSelected.ApplicationID;

            var sourceAppSelected = DataProcessingMethods.GetApplication(appData, srcAppName);
            if (!(sourceAppSelected == null)) { this.sourceApplicationURL = sourceAppSelected.ApplicationUrl; }
            


            var credentialSelection = DataProcessingMethods.GetCredential(credData, credentialDescriptionType);

                this.UniformResourceNameId = credentialSelection.UniformResourceNameId;
                this.credID = credentialSelection.CredentialID;
                
           

                var URNData = DataProcessingMethods.GetURNData( appName, UniformResourceNameId);

                LAFUniformResourceNameData URNSelection = DataProcessingMethods.GetURN(URNData, UniformResourceNameId);

            
                    this.credURNName = URNSelection.Name;
                    this.Urn = URNSelection.Urn;
                
        
                        

            DataProcessingMethods.InsertCredentialValues(credentialsToLoad,  credentialDescriptionType, txtChangeDesc.Text, filename, progressBar1, chkAudit, appName, credURNName, appURL, Urn, ApplicationID,credID,UniformResourceNameId,sourceApplicationURL);
            
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
                this.credentialsToLoad = ExcelMethods.GetData<CredentialValues>(filename, "CredentialValues");
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

        private void comboSrcApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            srcAppName = comboSrcApp.Text;
            comboCredential.Enabled = true;

        }
    }
}

