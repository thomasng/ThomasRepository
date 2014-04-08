namespace S6Db32ChangePathUtility
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFindRegistryData = new System.Windows.Forms.TextBox();
            this.gbModify = new System.Windows.Forms.GroupBox();
            this.btnStartModification = new System.Windows.Forms.Button();
            this.txtToData = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFindAll = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblRegistryProcessed = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbModify.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Location = new System.Drawing.Point(27, 20);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(856, 34);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "This utility will change the path of S6DB32.dll in the registry to solve the erro" +
    "r \"Lock violation\" when opening System Release product after updating to System " +
    "Release 8.1.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dll Name : ";
            // 
            // txtFindRegistryData
            // 
            this.txtFindRegistryData.Location = new System.Drawing.Point(82, 18);
            this.txtFindRegistryData.Name = "txtFindRegistryData";
            this.txtFindRegistryData.ReadOnly = true;
            this.txtFindRegistryData.Size = new System.Drawing.Size(160, 20);
            this.txtFindRegistryData.TabIndex = 2;
            // 
            // gbModify
            // 
            this.gbModify.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbModify.Controls.Add(this.btnStartModification);
            this.gbModify.Controls.Add(this.txtToData);
            this.gbModify.Controls.Add(this.label3);
            this.gbModify.Location = new System.Drawing.Point(30, 132);
            this.gbModify.Name = "gbModify";
            this.gbModify.Size = new System.Drawing.Size(853, 69);
            this.gbModify.TabIndex = 4;
            this.gbModify.TabStop = false;
            this.gbModify.Text = "Modify regitry value";
            // 
            // btnStartModification
            // 
            this.btnStartModification.Location = new System.Drawing.Point(467, 31);
            this.btnStartModification.Name = "btnStartModification";
            this.btnStartModification.Size = new System.Drawing.Size(156, 23);
            this.btnStartModification.TabIndex = 7;
            this.btnStartModification.Text = "Start modification";
            this.btnStartModification.UseVisualStyleBackColor = true;
            this.btnStartModification.Click += new System.EventHandler(this.btnStartModification_Click);
            // 
            // txtToData
            // 
            this.txtToData.Location = new System.Drawing.Point(85, 31);
            this.txtToData.Name = "txtToData";
            this.txtToData.ReadOnly = true;
            this.txtToData.Size = new System.Drawing.Size(366, 20);
            this.txtToData.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "New path : ";
            // 
            // btnFindAll
            // 
            this.btnFindAll.Location = new System.Drawing.Point(248, 17);
            this.btnFindAll.Name = "btnFindAll";
            this.btnFindAll.Size = new System.Drawing.Size(201, 23);
            this.btnFindAll.TabIndex = 8;
            this.btnFindAll.Text = "Find all matched registry key";
            this.btnFindAll.UseVisualStyleBackColor = true;
            this.btnFindAll.Click += new System.EventHandler(this.btnFindAllMatch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(808, 557);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(30, 229);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(853, 309);
            this.txtResult.TabIndex = 7;
            this.txtResult.WordWrap = false;
            // 
            // lblRegistryProcessed
            // 
            this.lblRegistryProcessed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRegistryProcessed.AutoSize = true;
            this.lblRegistryProcessed.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblRegistryProcessed.Location = new System.Drawing.Point(29, 557);
            this.lblRegistryProcessed.Name = "lblRegistryProcessed";
            this.lblRegistryProcessed.Size = new System.Drawing.Size(156, 13);
            this.lblRegistryProcessed.TabIndex = 8;
            this.lblRegistryProcessed.Text = "No of registr item processed : 0 ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Result:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnFindAll);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFindRegistryData);
            this.groupBox1.Location = new System.Drawing.Point(32, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(851, 61);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Analysis (Optional)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(914, 592);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblRegistryProcessed);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbModify);
            this.Controls.Add(this.lblDescription);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "System Release S6DB32.dll Change Path Utility";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.gbModify.ResumeLayout(false);
            this.gbModify.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFindRegistryData;
        private System.Windows.Forms.GroupBox gbModify;
        private System.Windows.Forms.Button btnStartModification;
        private System.Windows.Forms.TextBox txtToData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblRegistryProcessed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnFindAll;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

