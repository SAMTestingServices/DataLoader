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
    public partial class frmHome : Form
    {

        string environment;
        string instanceConnection; 
        string database;
        IDbConnection conn;


public frmHome()
        {
            InitializeComponent();
             environment = ConfigurationManager.AppSettings["Environment"]; ;
            instanceConnection = ConfigurationManager.AppSettings["Connection"];
            database = ConfigurationManager.AppSettings["Database"];

            txtConnection.Text = instanceConnection;
            txtEnvironment.Text = environment;
            txtDatabase.Text = database;
           
        }



        private void button1_Click(object sender, EventArgs e)
        {

        }









        

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void optSysStable_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmHome_Load(object sender, EventArgs e)
        {

        }

        

        private void Next_Click(object sender, EventArgs e)
        {
            try
            {
                string datasource = this.instanceConnection;

                string auditDatabase = "";
                
                conn = DataProcessingMethods.GetDBConnection(datasource);
                conn.Open();
    
                frmOperationSelect frmOperationSelect = new frmOperationSelect(database, auditDatabase);
                MessageBox.Show("Connection to Database successful\n\r\n\rClick OK to Proceed " , "Authentication successful",
MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmOperationSelect.Show();
                this.Hide();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message.ToString(), "Error",
MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                
            }
            finally
            {
                conn.Close();
            }
        }

    }
}

