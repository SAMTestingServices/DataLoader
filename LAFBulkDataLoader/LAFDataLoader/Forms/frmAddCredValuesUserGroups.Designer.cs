namespace WindowsFormsApplication3
{
    partial class frmAddCredValuesToUserGroups
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
            this.btnUploadData = new System.Windows.Forms.Button();
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
            this.btnReset = new System.Windows.Forms.Button();
            this.chkAudit = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnUploadData
            // 
            this.btnUploadData.Enabled = false;
            this.btnUploadData.Location = new System.Drawing.Point(389, 401);
            this.btnUploadData.Name = "btnUploadData";
            this.btnUploadData.Size = new System.Drawing.Size(121, 23);
            this.btnUploadData.TabIndex = 5;
            this.btnUploadData.Text = "Upload data";
            this.btnUploadData.UseVisualStyleBackColor = true;
            this.btnUploadData.Click += new System.EventHandler(this.Next_Click);
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
            this.label2.Size = new System.Drawing.Size(533, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Step 3 - Select options (Add Credential Values to User Groups)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(349, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "1. Select the application in which you want to add Credentials to Groups:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(272, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "2. Select the credential type for this group of credentials:";
            // 
            // comboApplication
            // 
            this.comboApplication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboApplication.FormattingEnabled = true;
            this.comboApplication.Location = new System.Drawing.Point(389, 146);
            this.comboApplication.Name = "comboApplication";
            this.comboApplication.Size = new System.Drawing.Size(121, 21);
            this.comboApplication.TabIndex = 1;
            this.comboApplication.SelectedIndexChanged += new System.EventHandler(this.comboApplication_SelectedIndexChanged);
            // 
            // comboCredential
            // 
            this.comboCredential.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCredential.FormattingEnabled = true;
            this.comboCredential.Location = new System.Drawing.Point(389, 197);
            this.comboCredential.Name = "comboCredential";
            this.comboCredential.Size = new System.Drawing.Size(121, 21);
            this.comboCredential.TabIndex = 2;
            this.comboCredential.SelectedIndexChanged += new System.EventHandler(this.comboCredential_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(287, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "3. Click to select a USer Group / Credential Value data file: ";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(389, 246);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(121, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // filePath
            // 
            this.filePath.Location = new System.Drawing.Point(16, 288);
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
            this.label6.Location = new System.Drawing.Point(13, 406);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "5. Click to upload the data:";
            // 
            // labelUploading
            // 
            this.labelUploading.AutoSize = true;
            this.labelUploading.Location = new System.Drawing.Point(191, 502);
            this.labelUploading.Name = "labelUploading";
            this.labelUploading.Size = new System.Drawing.Size(150, 13);
            this.labelUploading.TabIndex = 18;
            this.labelUploading.Text = "Currently updating Database...";
            this.labelUploading.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 458);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(494, 23);
            this.progressBar1.TabIndex = 17;
            // 
            // txtChangeDesc
            // 
            this.txtChangeDesc.Enabled = false;
            this.txtChangeDesc.Location = new System.Drawing.Point(389, 343);
            this.txtChangeDesc.Name = "txtChangeDesc";
            this.txtChangeDesc.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtChangeDesc.Size = new System.Drawing.Size(121, 20);
            this.txtChangeDesc.TabIndex = 4;
            this.txtChangeDesc.TextChanged += new System.EventHandler(this.txtChangeDesc_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 346);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(300, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "4. Enter an identifier for this change (e.g. Service Request ID):";
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(410, 101);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(84, 23);
            this.btnReset.TabIndex = 25;
            this.btnReset.Text = "Reset options";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkAudit
            // 
            this.chkAudit.AutoSize = true;
            this.chkAudit.Location = new System.Drawing.Point(282, 405);
            this.chkAudit.Name = "chkAudit";
            this.chkAudit.Size = new System.Drawing.Size(80, 17);
            this.chkAudit.TabIndex = 26;
            this.chkAudit.Text = "Audit Only?";
            this.chkAudit.UseVisualStyleBackColor = true;
            // 
            // frmAddCredsToUserGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 557);
            this.Controls.Add(this.chkAudit);
            this.Controls.Add(this.btnReset);
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
            this.Controls.Add(this.btnUploadData);
            this.Name = "frmAddCredsToUserGroups";
            this.Text = "Add Credential Values to Groups - LAF Bulk Data loader";
            this.Load += new System.EventHandler(this.frmAddCredsToUserGroups_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUploadData;
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
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox chkAudit;
    }
}

