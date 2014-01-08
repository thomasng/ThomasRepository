namespace TestEmail
{
    partial class Form1
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
            this.txtSenderEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReceivedEmail = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.btnInfo = new System.Windows.Forms.Button();
            this.chkEmailAcctOnly = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.btnDefaultEmailSend = new System.Windows.Forms.Button();
            this.btnVerifySenderEmail = new System.Windows.Forms.Button();
            this.btnResolve = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSenderEmail
            // 
            this.txtSenderEmail.Location = new System.Drawing.Point(127, 23);
            this.txtSenderEmail.Name = "txtSenderEmail";
            this.txtSenderEmail.Size = new System.Drawing.Size(234, 20);
            this.txtSenderEmail.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sender\'s Email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Receiver\'s Email";
            // 
            // txtReceivedEmail
            // 
            this.txtReceivedEmail.Location = new System.Drawing.Point(127, 49);
            this.txtReceivedEmail.Name = "txtReceivedEmail";
            this.txtReceivedEmail.Size = new System.Drawing.Size(234, 20);
            this.txtReceivedEmail.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(513, 23);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(146, 54);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send Email";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtResults
            // 
            this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResults.Location = new System.Drawing.Point(28, 186);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(631, 197);
            this.txtResults.TabIndex = 7;
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(151, 100);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(139, 23);
            this.btnInfo.TabIndex = 8;
            this.btnInfo.Text = "Get Information...";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // chkEmailAcctOnly
            // 
            this.chkEmailAcctOnly.AutoSize = true;
            this.chkEmailAcctOnly.Location = new System.Drawing.Point(28, 106);
            this.chkEmailAcctOnly.Name = "chkEmailAcctOnly";
            this.chkEmailAcctOnly.Size = new System.Drawing.Size(98, 17);
            this.chkEmailAcctOnly.TabIndex = 9;
            this.chkEmailAcctOnly.Text = "Email Acct only";
            this.chkEmailAcctOnly.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Subject";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(127, 75);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(234, 20);
            this.txtSubject.TabIndex = 10;
            // 
            // btnDefaultEmailSend
            // 
            this.btnDefaultEmailSend.Location = new System.Drawing.Point(513, 84);
            this.btnDefaultEmailSend.Name = "btnDefaultEmailSend";
            this.btnDefaultEmailSend.Size = new System.Drawing.Size(146, 43);
            this.btnDefaultEmailSend.TabIndex = 12;
            this.btnDefaultEmailSend.Text = "Send by default email address";
            this.btnDefaultEmailSend.UseVisualStyleBackColor = true;
            this.btnDefaultEmailSend.Click += new System.EventHandler(this.btnDefaultEmailSend_Click);
            // 
            // btnVerifySenderEmail
            // 
            this.btnVerifySenderEmail.Location = new System.Drawing.Point(367, 23);
            this.btnVerifySenderEmail.Name = "btnVerifySenderEmail";
            this.btnVerifySenderEmail.Size = new System.Drawing.Size(140, 54);
            this.btnVerifySenderEmail.TabIndex = 13;
            this.btnVerifySenderEmail.Text = "Verify Sender email address";
            this.btnVerifySenderEmail.UseVisualStyleBackColor = true;
            this.btnVerifySenderEmail.Click += new System.EventHandler(this.btnVerifySenderEmail_Click);
            // 
            // btnResolve
            // 
            this.btnResolve.Location = new System.Drawing.Point(366, 84);
            this.btnResolve.Name = "btnResolve";
            this.btnResolve.Size = new System.Drawing.Size(141, 43);
            this.btnResolve.TabIndex = 14;
            this.btnResolve.Text = "Default email address";
            this.btnResolve.UseVisualStyleBackColor = true;
            this.btnResolve.Click += new System.EventHandler(this.btnResolve_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(151, 129);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(139, 23);
            this.btnTest.TabIndex = 15;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 395);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnResolve);
            this.Controls.Add(this.btnVerifySenderEmail);
            this.Controls.Add(this.btnDefaultEmailSend);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.chkEmailAcctOnly);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReceivedEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSenderEmail);
            this.Name = "Form1";
            this.Text = "MYOB Email Testing Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSenderEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReceivedEmail;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.CheckBox chkEmailAcctOnly;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Button btnDefaultEmailSend;
        private System.Windows.Forms.Button btnVerifySenderEmail;
        private System.Windows.Forms.Button btnResolve;
        private System.Windows.Forms.Button btnTest;
    }
}

