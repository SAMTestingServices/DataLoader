namespace WindowsFormsApplication3
{
    partial class frmOperationSelect
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Next = new System.Windows.Forms.Button();
            this.optCredentialValues = new System.Windows.Forms.RadioButton();
            this.optUserGroups = new System.Windows.Forms.RadioButton();
            this.optUsers = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.optCredValuesToUserGroups = new System.Windows.Forms.RadioButton();
            this.optUsersToUserGroups = new System.Windows.Forms.RadioButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.optLAFDAUserGroups = new System.Windows.Forms.RadioButton();
            this.delUsersFromUG = new System.Windows.Forms.RadioButton();
            this.delCredValuesFromUG = new System.Windows.Forms.RadioButton();
            this.delUserGroups = new System.Windows.Forms.RadioButton();
            this.delCredValues = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
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
            this.label2.Size = new System.Drawing.Size(215, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Step 2 - Select operation";
            // 
            // Next
            // 
            this.Next.Location = new System.Drawing.Point(481, 206);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(75, 23);
            this.Next.TabIndex = 0;
            this.Next.Text = "Next";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.button1_Click);
            // 
            // optCredentialValues
            // 
            this.optCredentialValues.AutoSize = true;
            this.optCredentialValues.Location = new System.Drawing.Point(16, 144);
            this.optCredentialValues.Name = "optCredentialValues";
            this.optCredentialValues.Size = new System.Drawing.Size(141, 17);
            this.optCredentialValues.TabIndex = 1;
            this.optCredentialValues.TabStop = true;
            this.optCredentialValues.Text = "Create Credential Values";
            this.optCredentialValues.UseVisualStyleBackColor = true;
            this.optCredentialValues.CheckedChanged += new System.EventHandler(this.optCredentialValues_CheckedChanged);
            // 
            // optUserGroups
            // 
            this.optUserGroups.AutoSize = true;
            this.optUserGroups.Location = new System.Drawing.Point(16, 179);
            this.optUserGroups.Name = "optUserGroups";
            this.optUserGroups.Size = new System.Drawing.Size(118, 17);
            this.optUserGroups.TabIndex = 2;
            this.optUserGroups.TabStop = true;
            this.optUserGroups.Text = "Create User Groups";
            this.optUserGroups.UseVisualStyleBackColor = true;
            // 
            // optUsers
            // 
            this.optUsers.AutoSize = true;
            this.optUsers.Location = new System.Drawing.Point(16, 212);
            this.optUsers.Name = "optUsers";
            this.optUsers.Size = new System.Drawing.Size(86, 17);
            this.optUsers.TabIndex = 3;
            this.optUsers.TabStop = true;
            this.optUsers.Text = "Create Users";
            this.optUsers.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(282, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Select the operation you would like to load from the below:";
            // 
            // optCredValuesToUserGroups
            // 
            this.optCredValuesToUserGroups.AutoSize = true;
            this.optCredValuesToUserGroups.Location = new System.Drawing.Point(16, 244);
            this.optCredValuesToUserGroups.Name = "optCredValuesToUserGroups";
            this.optCredValuesToUserGroups.Size = new System.Drawing.Size(203, 17);
            this.optCredValuesToUserGroups.TabIndex = 8;
            this.optCredValuesToUserGroups.TabStop = true;
            this.optCredValuesToUserGroups.Text = "Add Credential Values to User Groups";
            this.optCredValuesToUserGroups.UseVisualStyleBackColor = true;
            // 
            // optUsersToUserGroups
            // 
            this.optUsersToUserGroups.AutoSize = true;
            this.optUsersToUserGroups.Location = new System.Drawing.Point(16, 279);
            this.optUsersToUserGroups.Name = "optUsersToUserGroups";
            this.optUsersToUserGroups.Size = new System.Drawing.Size(148, 17);
            this.optUsersToUserGroups.TabIndex = 9;
            this.optUsersToUserGroups.TabStop = true;
            this.optUsersToUserGroups.Text = "Add Users to User Groups";
            this.optUsersToUserGroups.UseVisualStyleBackColor = true;
            // 
            // optLAFDAUserGroups
            // 
            this.optLAFDAUserGroups.AutoSize = true;
            this.optLAFDAUserGroups.Location = new System.Drawing.Point(16, 312);
            this.optLAFDAUserGroups.Name = "optLAFDAUserGroups";
            this.optLAFDAUserGroups.Size = new System.Drawing.Size(133, 17);
            this.optLAFDAUserGroups.TabIndex = 10;
            this.optLAFDAUserGroups.TabStop = true;
            this.optLAFDAUserGroups.Text = "Create LAF DA Groups";
            this.optLAFDAUserGroups.UseVisualStyleBackColor = true;
            // 
            // delUsersFromUG
            // 
            this.delUsersFromUG.AutoSize = true;
            this.delUsersFromUG.Location = new System.Drawing.Point(239, 279);
            this.delUsersFromUG.Name = "delUsersFromUG";
            this.delUsersFromUG.Size = new System.Drawing.Size(180, 17);
            this.delUsersFromUG.TabIndex = 15;
            this.delUsersFromUG.TabStop = true;
            this.delUsersFromUG.Text = "Remove Users from User Groups";
            this.delUsersFromUG.UseVisualStyleBackColor = true;
            // 
            // delCredValuesFromUG
            // 
            this.delCredValuesFromUG.AutoSize = true;
            this.delCredValuesFromUG.Location = new System.Drawing.Point(239, 244);
            this.delCredValuesFromUG.Name = "delCredValuesFromUG";
            this.delCredValuesFromUG.Size = new System.Drawing.Size(235, 17);
            this.delCredValuesFromUG.TabIndex = 14;
            this.delCredValuesFromUG.TabStop = true;
            this.delCredValuesFromUG.Text = "Remove Credential Values from User Groups";
            this.delCredValuesFromUG.UseVisualStyleBackColor = true;
            // 
            // delUserGroups
            // 
            this.delUserGroups.AutoSize = true;
            this.delUserGroups.Location = new System.Drawing.Point(239, 179);
            this.delUserGroups.Name = "delUserGroups";
            this.delUserGroups.Size = new System.Drawing.Size(118, 17);
            this.delUserGroups.TabIndex = 12;
            this.delUserGroups.TabStop = true;
            this.delUserGroups.Text = "Delete User Groups";
            this.delUserGroups.UseVisualStyleBackColor = true;
            // 
            // delCredValues
            // 
            this.delCredValues.AutoSize = true;
            this.delCredValues.Location = new System.Drawing.Point(239, 144);
            this.delCredValues.Name = "delCredValues";
            this.delCredValues.Size = new System.Drawing.Size(139, 17);
            this.delCredValues.TabIndex = 11;
            this.delCredValues.TabStop = true;
            this.delCredValues.Text = "Delete credential values";
            this.delCredValues.UseVisualStyleBackColor = true;
            this.delCredValues.CheckedChanged += new System.EventHandler(this.delCredValues_CheckedChanged);
            // 
            // frmOperationSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 361);
            this.Controls.Add(this.delUsersFromUG);
            this.Controls.Add(this.delCredValuesFromUG);
            this.Controls.Add(this.delUserGroups);
            this.Controls.Add(this.delCredValues);
            this.Controls.Add(this.optLAFDAUserGroups);
            this.Controls.Add(this.optUsersToUserGroups);
            this.Controls.Add(this.optCredValuesToUserGroups);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.optUsers);
            this.Controls.Add(this.optUserGroups);
            this.Controls.Add(this.optCredentialValues);
            this.Controls.Add(this.Next);
            this.Name = "frmOperationSelect";
            this.Text = "Select Operation - LAF Bulk Data loader";
            this.Load += new System.EventHandler(this.frmOperationSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.RadioButton optCredentialValues;
        private System.Windows.Forms.RadioButton optUserGroups;
        private System.Windows.Forms.RadioButton optUsers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton optCredValuesToUserGroups;
        private System.Windows.Forms.RadioButton optUsersToUserGroups;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RadioButton optLAFDAUserGroups;
        private System.Windows.Forms.RadioButton delUsersFromUG;
        private System.Windows.Forms.RadioButton delCredValuesFromUG;
        private System.Windows.Forms.RadioButton delUserGroups;
        private System.Windows.Forms.RadioButton delCredValues;
    }
}

