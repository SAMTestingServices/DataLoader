namespace WindowsFormsApplication3
{
    partial class frmHome
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
            this.btnNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Datbase = new System.Windows.Forms.Label();
            this.txtEnvironment = new System.Windows.Forms.TextBox();
            this.txtConnection = new System.Windows.Forms.TextBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(336, 134);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Connect";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.Next_Click);
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
            this.label2.Size = new System.Drawing.Size(200, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Step 1 - Connect to DB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Environment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Connection:";
            // 
            // Datbase
            // 
            this.Datbase.AutoSize = true;
            this.Datbase.Location = new System.Drawing.Point(13, 185);
            this.Datbase.Name = "Datbase";
            this.Datbase.Size = new System.Drawing.Size(56, 13);
            this.Datbase.TabIndex = 9;
            this.Datbase.Text = "Database:";
            // 
            // txtEnvironment
            // 
            this.txtEnvironment.Location = new System.Drawing.Point(100, 101);
            this.txtEnvironment.Name = "txtEnvironment";
            this.txtEnvironment.ReadOnly = true;
            this.txtEnvironment.Size = new System.Drawing.Size(168, 20);
            this.txtEnvironment.TabIndex = 10;
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(100, 141);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.ReadOnly = true;
            this.txtConnection.Size = new System.Drawing.Size(168, 20);
            this.txtConnection.TabIndex = 11;
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(100, 182);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.ReadOnly = true;
            this.txtDatabase.Size = new System.Drawing.Size(168, 20);
            this.txtDatabase.TabIndex = 12;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 240);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.txtConnection);
            this.Controls.Add(this.txtEnvironment);
            this.Controls.Add(this.Datbase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNext);
            this.Name = "frmHome";
            this.Text = "Select DB - LAF Bulk Data loader";
            this.Load += new System.EventHandler(this.frmHome_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Datbase;
        private System.Windows.Forms.TextBox txtConnection;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.TextBox txtEnvironment;
        private System.Windows.Forms.Label label3;
    }
}

