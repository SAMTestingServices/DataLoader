namespace WindowsFormsApplication3
{
    partial class frmCredValues
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Next = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboApplication = new System.Windows.Forms.ComboBox();
            this.comboCredential = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.filePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.labelUploading = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtChangeDesc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkAudit = new System.Windows.Forms.CheckBox();
            this.comboSrcApp = new System.Windows.Forms.ComboBox();
            this.lblComboSrcApp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Next
            // 
            this.Next.Enabled = false;
            this.Next.Location = new System.Drawing.Point(390, 393);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(121, 23);
            this.Next.TabIndex = 0;
            this.Next.Text = "Upload data";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new System.Drawing.Point(112, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "LAF - Bulk data loader";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(422, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Step 3 - Select options (Create Credential Values)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(343, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "1. Select the application that you would like to add credential values to:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(301, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "2. Select the credential type for this group of credential values:";
            // 
            // comboApplication
            // 
            this.comboApplication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboApplication.FormattingEnabled = true;
            this.comboApplication.Location = new System.Drawing.Point(389, 105);
            this.comboApplication.Name = "comboApplication";
            this.comboApplication.Size = new System.Drawing.Size(121, 21);
            this.comboApplication.TabIndex = 9;
            this.comboApplication.SelectedIndexChanged += new System.EventHandler(this.comboApplication_SelectedIndexChanged);
            // 
            // comboCredential
            // 
            this.comboCredential.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCredential.Enabled = false;
            this.comboCredential.FormattingEnabled = true;
            this.comboCredential.Location = new System.Drawing.Point(389, 189);
            this.comboCredential.Name = "comboCredential";
            this.comboCredential.Size = new System.Drawing.Size(121, 21);
            this.comboCredential.TabIndex = 10;
            this.comboCredential.SelectedIndexChanged += new System.EventHandler(this.comboCredential_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(220, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "3. Click to select a Credential Value data file: ";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(389, 238);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(121, 23);
            this.btnBrowse.TabIndex = 12;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // filePath
            // 
            this.filePath.Enabled = false;
            this.filePath.Location = new System.Drawing.Point(16, 280);
            this.filePath.Name = "filePath";
            this.filePath.ReadOnly = true;
            this.filePath.Size = new System.Drawing.Size(494, 20);
            this.filePath.TabIndex = 13;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 398);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "5. Click to upload the data:";
            // 
            // labelUploading
            // 
            this.labelUploading.AutoSize = true;
            this.labelUploading.Location = new System.Drawing.Point(192, 496);
            this.labelUploading.Name = "labelUploading";
            this.labelUploading.Size = new System.Drawing.Size(150, 13);
            this.labelUploading.TabIndex = 18;
            this.labelUploading.Text = "Currently updating Database...";
            this.labelUploading.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(17, 452);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(494, 23);
            this.progressBar1.TabIndex = 17;
            // 
            // txtChangeDesc
            // 
            this.txtChangeDesc.Enabled = false;
            this.txtChangeDesc.Location = new System.Drawing.Point(390, 336);
            this.txtChangeDesc.Name = "txtChangeDesc";
            this.txtChangeDesc.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtChangeDesc.Size = new System.Drawing.Size(121, 20);
            this.txtChangeDesc.TabIndex = 23;
            this.txtChangeDesc.TextChanged += new System.EventHandler(this.txtChangeDesc_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 339);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(300, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "4. Enter an identifier for this change (e.g. Service Request ID):";
            // 
            // chkAudit
            // 
            this.chkAudit.AutoSize = true;
            this.chkAudit.Location = new System.Drawing.Point(289, 397);
            this.chkAudit.Name = "chkAudit";
            this.chkAudit.Size = new System.Drawing.Size(80, 17);
            this.chkAudit.TabIndex = 24;
            this.chkAudit.Text = "Audit Only?";
            this.chkAudit.UseVisualStyleBackColor = true;
            // 
            // comboSrcApp
            // 
            this.comboSrcApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSrcApp.FormattingEnabled = true;
            this.comboSrcApp.Location = new System.Drawing.Point(389, 147);
            this.comboSrcApp.Name = "comboSrcApp";
            this.comboSrcApp.Size = new System.Drawing.Size(121, 21);
            this.comboSrcApp.TabIndex = 26;
            this.comboSrcApp.Visible = false;
            this.comboSrcApp.SelectedIndexChanged += new System.EventHandler(this.comboSrcApp_SelectedIndexChanged);
            // 
            // lblComboSrcApp
            // 
            this.lblComboSrcApp.AutoSize = true;
            this.lblComboSrcApp.Location = new System.Drawing.Point(13, 150);
            this.lblComboSrcApp.Name = "lblComboSrcApp";
            this.lblComboSrcApp.Size = new System.Drawing.Size(355, 13);
            this.lblComboSrcApp.TabIndex = 25;
            this.lblComboSrcApp.Text = "    Select the source applicaiton that these LAF credential values relate to:";
            this.lblComboSrcApp.Visible = false;
            // 
            // frmCredValues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 530);
            this.Controls.Add(this.comboSrcApp);
            this.Controls.Add(this.lblComboSrcApp);
            this.Controls.Add(this.chkAudit);
            this.Controls.Add(this.txtChangeDesc);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelUploading);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboCredential);
            this.Controls.Add(this.comboApplication);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Next);
            this.Name = "frmCredValues";
            this.Text = "Create Credential Values - LAF Bulk Data loader";
            this.Load += new System.EventHandler(this.fmrCredValues_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboApplication;
        private System.Windows.Forms.ComboBox comboCredential;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox filePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelUploading;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtChangeDesc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkAudit;
        private System.Windows.Forms.ComboBox comboSrcApp;
        private System.Windows.Forms.Label lblComboSrcApp;
    }
}

