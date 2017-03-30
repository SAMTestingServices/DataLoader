namespace WindowsFormsApplication3
{
    partial class frmCreateUserGroups
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
            this.comboApplication2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.filePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.labelUploading = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.txtChangeDesc = new System.Windows.Forms.TextBox();
            this.chkAudit = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Next
            // 
            this.Next.Enabled = false;
            this.Next.Location = new System.Drawing.Point(389, 336);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(121, 23);
            this.Next.TabIndex = 4;
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
            this.label2.Size = new System.Drawing.Size(380, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Step 3 - Select options (Create User Groups)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(315, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "1. Select the application that you would like to add usergroups to:";
            // 
            // comboApplication2
            // 
            this.comboApplication2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboApplication2.FormattingEnabled = true;
            this.comboApplication2.Location = new System.Drawing.Point(389, 105);
            this.comboApplication2.Name = "comboApplication2";
            this.comboApplication2.Size = new System.Drawing.Size(121, 21);
            this.comboApplication2.TabIndex = 1;
            this.comboApplication2.SelectedIndexChanged += new System.EventHandler(this.comboApplication_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(197, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "2. Click to select a User Group data file: ";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(389, 160);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(121, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // filePath
            // 
            this.filePath.Location = new System.Drawing.Point(16, 212);
            this.filePath.Name = "filePath";
            this.filePath.ReadOnly = true;
            this.filePath.Size = new System.Drawing.Size(494, 20);
            this.filePath.TabIndex = 13;
            this.filePath.TextChanged += new System.EventHandler(this.filePath_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 341);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "4. Click to upload the data:";
            // 
            // labelUploading
            // 
            this.labelUploading.AutoSize = true;
            this.labelUploading.Location = new System.Drawing.Point(191, 430);
            this.labelUploading.Name = "labelUploading";
            this.labelUploading.Size = new System.Drawing.Size(150, 13);
            this.labelUploading.TabIndex = 18;
            this.labelUploading.Text = "Currently updating Database...";
            this.labelUploading.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 386);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(494, 23);
            this.progressBar1.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 269);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(300, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "3. Enter an identifier for this change (e.g. Service Request ID):";
            // 
            // txtChangeDesc
            // 
            this.txtChangeDesc.AcceptsReturn = true;
            this.txtChangeDesc.Enabled = false;
            this.txtChangeDesc.Location = new System.Drawing.Point(389, 266);
            this.txtChangeDesc.Name = "txtChangeDesc";
            this.txtChangeDesc.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtChangeDesc.Size = new System.Drawing.Size(121, 20);
            this.txtChangeDesc.TabIndex = 3;
            this.txtChangeDesc.TextChanged += new System.EventHandler(this.txtChangeDesc_TextChanged);
            // 
            // chkAudit
            // 
            this.chkAudit.AutoSize = true;
            this.chkAudit.Location = new System.Drawing.Point(288, 340);
            this.chkAudit.Name = "chkAudit";
            this.chkAudit.Size = new System.Drawing.Size(80, 17);
            this.chkAudit.TabIndex = 20;
            this.chkAudit.Text = "Audit Only?";
            this.chkAudit.UseVisualStyleBackColor = true;
            // 
            // frmUserGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 475);
            this.Controls.Add(this.chkAudit);
            this.Controls.Add(this.txtChangeDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelUploading);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboApplication2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Next);
            this.Name = "frmUserGroups";
            this.Text = "Create User Groups - LAF Bulk Data loader";
            this.Load += new System.EventHandler(this.frmUserGroups_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboApplication2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox filePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelUploading;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtChangeDesc;
        private System.Windows.Forms.CheckBox chkAudit;
    }
}

