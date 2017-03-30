namespace WindowsFormsApplication3
{
    partial class frmLAFDAUserGroups
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
            this.label5 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.filePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.labelUploading = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.txtChangeDesc = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Next
            // 
            this.Next.Enabled = false;
            this.Next.Location = new System.Drawing.Point(389, 284);
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
            this.label2.Size = new System.Drawing.Size(407, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Step 3 - Select options (Create LAF DA Groups)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(197, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "1. Click to select a User Group data file: ";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(389, 108);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(121, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // filePath
            // 
            this.filePath.Location = new System.Drawing.Point(16, 160);
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
            this.label6.Location = new System.Drawing.Point(13, 289);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "3. Click to upload the data:";
            // 
            // labelUploading
            // 
            this.labelUploading.AutoSize = true;
            this.labelUploading.Location = new System.Drawing.Point(191, 378);
            this.labelUploading.Name = "labelUploading";
            this.labelUploading.Size = new System.Drawing.Size(150, 13);
            this.labelUploading.TabIndex = 18;
            this.labelUploading.Text = "Currently updating Database...";
            this.labelUploading.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 334);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(494, 23);
            this.progressBar1.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(300, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "2. Enter an identifier for this change (e.g. Service Request ID):";
            // 
            // txtChangeDesc
            // 
            this.txtChangeDesc.AcceptsReturn = true;
            this.txtChangeDesc.Enabled = false;
            this.txtChangeDesc.Location = new System.Drawing.Point(389, 214);
            this.txtChangeDesc.Name = "txtChangeDesc";
            this.txtChangeDesc.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtChangeDesc.Size = new System.Drawing.Size(121, 20);
            this.txtChangeDesc.TabIndex = 3;
            this.txtChangeDesc.TextChanged += new System.EventHandler(this.txtChangeDesc_TextChanged);
            // 
            // frmLAFDAUserGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 434);
            this.Controls.Add(this.txtChangeDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelUploading);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Next);
            this.Name = "frmLAFDAUserGroups";
            this.Text = "Create LAF DA User Groups - LAF Bulk Data loader";
            this.Load += new System.EventHandler(this.frmLAFDAUserGroups_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox filePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelUploading;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtChangeDesc;
    }
}

