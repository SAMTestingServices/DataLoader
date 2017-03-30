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
using System.Configuration;

namespace WindowsFormsApplication3
{
    public partial class frmOperationSelect : Form
    {
        
        private string database;
        private string auditDatabase;
        private string mode;


        public frmOperationSelect(string database,string auditDatabase)
        {

            InitializeComponent();
            
            this.database = database;
            this.auditDatabase = auditDatabase;
            this.mode = "DB";

            
        }
        public frmOperationSelect()
        {

            InitializeComponent();
            updateToAPIMode();
            this.mode = "API";


        }

        private void updateToAPIMode()
        {

            optUsers.Enabled = false;
            delCredValues.Enabled = false;
            delCredValuesFromUG.Enabled = false;
            delUserGroups.Enabled = false;
            delUsersFromUG.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

          
                if (optCredentialValues.Checked == true)
                {
                    WindowsFormsApplication3.frmCredValues frmCredValues = new WindowsFormsApplication3.frmCredValues();
                    frmCredValues.Show();
                }
                if (optUserGroups.Checked == true)
                {
                    WindowsFormsApplication3.frmCreateUserGroups frmUserGroups = new WindowsFormsApplication3.frmCreateUserGroups();
                    frmUserGroups.Show();
                }

                if (optCredValuesToUserGroups.Checked == true)
                {
                    WindowsFormsApplication3.frmAddCredValuesToUserGroups frmAddCredsToUserGroups = new WindowsFormsApplication3.frmAddCredValuesToUserGroups();
                    frmAddCredsToUserGroups.Show();
                }
                if (optUsersToUserGroups.Checked == true)
                {

                    if (DataProcessingMethods.CheckIfStoredProcsExist("sp_InsertUserGroupMember"))
                        {
                            WindowsFormsApplication3.frmAddUsersToUserGroups frmAddUsersToUserGroups1 = new WindowsFormsApplication3.frmAddUsersToUserGroups();
                            frmAddUsersToUserGroups1.Show();
                        }
                                              
                }
                if (optUsers.Checked == true)
                {
                    if (DataProcessingMethods.CheckIfStoredProcsExist("sp_InsertUser") && DataProcessingMethods.CheckIfStoredProcsExist("sp_Audit_InsertUser"))
                        {
                            WindowsFormsApplication3.frmCreateUsers frmCreateUsers1 = new WindowsFormsApplication3.frmCreateUsers();
                            frmCreateUsers1.Show();
                        }          
                }

                if (optLAFDAUserGroups.Checked == true)
                {
                    WindowsFormsApplication3.frmLAFDAUserGroups frmLAFDAUserGroups1 = new WindowsFormsApplication3.frmLAFDAUserGroups();
                    frmLAFDAUserGroups1.Show();
                }
                if (delCredValues.Checked == true)
                {
                if (DataProcessingMethods.CheckIfStoredProcsExist("sp_DeleteCredentialValue") )
                {
                    WindowsFormsApplication3.frmDeleteCredValues frmDeleteCredValues1 = new WindowsFormsApplication3.frmDeleteCredValues();
                    frmDeleteCredValues1.Show();
                }
                
                }
            if (delUserGroups.Checked == true)
            {

                if (DataProcessingMethods.CheckIfStoredProcsExist("sp_DeleteUserGroup"))
                {
                    

                    WindowsFormsApplication3.frmDeleteUserGroups frmDeleteUserGroups1 = new WindowsFormsApplication3.frmDeleteUserGroups();
                    frmDeleteUserGroups1.Show();
                }
              
            }
                if (delCredValuesFromUG.Checked == true)
                {
                if (DataProcessingMethods.CheckIfStoredProcsExist("sp_DeleteUserGroupCredentialValues"))
                {
                    WindowsFormsApplication3.frmDeleteCredsFromUserGroups fromDeleteCredsFromUserGroups1 = new WindowsFormsApplication3.frmDeleteCredsFromUserGroups();
                    fromDeleteCredsFromUserGroups1.Show();
                }

                
                }
                if (delUsersFromUG.Checked == true)
                {
                if (DataProcessingMethods.CheckIfStoredProcsExist("sp_DeleteUserGroupMemberFromUserGroup"))
                {
                    WindowsFormsApplication3.frmDeleteUsersFromUserGroups frmDeleteUsersFromUserGroups1 = new WindowsFormsApplication3.frmDeleteUsersFromUserGroups();
                    frmDeleteUsersFromUserGroups1.Show();
                }

                
                }

            

         
        }

        

        private void frmOperationSelect_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void optCredentialValues_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void delCredValues_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
