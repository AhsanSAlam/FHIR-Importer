namespace FHIRTestAppWin
{
    partial class fhirDemoForm
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
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.uploadDataButton = new System.Windows.Forms.Button();
            this.uploadStatusLabel = new System.Windows.Forms.Label();
            this.getDataButton = new System.Windows.Forms.Button();
            this.resourceIDTextBox = new System.Windows.Forms.TextBox();
            this.dataTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.getConfigentialAppTokenButton = new System.Windows.Forms.Button();
            this.resourceSelectorComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(74, 273);
            this.statusTextBox.Multiline = true;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.statusTextBox.Size = new System.Drawing.Size(1508, 169);
            this.statusTextBox.TabIndex = 1;
            // 
            // uploadDataButton
            // 
            this.uploadDataButton.Location = new System.Drawing.Point(702, 36);
            this.uploadDataButton.Name = "uploadDataButton";
            this.uploadDataButton.Size = new System.Drawing.Size(329, 65);
            this.uploadDataButton.TabIndex = 2;
            this.uploadDataButton.Text = "Upload Patient Bundle";
            this.uploadDataButton.UseVisualStyleBackColor = true;
            this.uploadDataButton.Click += new System.EventHandler(this.UploadDataButton_Click);
            // 
            // uploadStatusLabel
            // 
            this.uploadStatusLabel.AutoSize = true;
            this.uploadStatusLabel.Location = new System.Drawing.Point(68, 238);
            this.uploadStatusLabel.Name = "uploadStatusLabel";
            this.uploadStatusLabel.Size = new System.Drawing.Size(202, 32);
            this.uploadStatusLabel.TabIndex = 3;
            this.uploadStatusLabel.Text = "Upload Status:";
            // 
            // getDataButton
            // 
            this.getDataButton.Location = new System.Drawing.Point(74, 130);
            this.getDataButton.Name = "getDataButton";
            this.getDataButton.Size = new System.Drawing.Size(270, 54);
            this.getDataButton.TabIndex = 4;
            this.getDataButton.Text = "Get Data";
            this.getDataButton.UseVisualStyleBackColor = true;
            this.getDataButton.Click += new System.EventHandler(this.GetDataButton_Click);
            // 
            // resourceIDTextBox
            // 
            this.resourceIDTextBox.Location = new System.Drawing.Point(860, 140);
            this.resourceIDTextBox.Name = "resourceIDTextBox";
            this.resourceIDTextBox.Size = new System.Drawing.Size(712, 38);
            this.resourceIDTextBox.TabIndex = 5;
            // 
            // dataTextBox
            // 
            this.dataTextBox.Location = new System.Drawing.Point(74, 504);
            this.dataTextBox.Multiline = true;
            this.dataTextBox.Name = "dataTextBox";
            this.dataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dataTextBox.Size = new System.Drawing.Size(1508, 546);
            this.dataTextBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 469);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 32);
            this.label1.TabIndex = 7;
            this.label1.Text = "Data:";
            // 
            // getConfigentialAppTokenButton
            // 
            this.getConfigentialAppTokenButton.Location = new System.Drawing.Point(74, 36);
            this.getConfigentialAppTokenButton.Name = "getConfigentialAppTokenButton";
            this.getConfigentialAppTokenButton.Size = new System.Drawing.Size(511, 63);
            this.getConfigentialAppTokenButton.TabIndex = 8;
            this.getConfigentialAppTokenButton.Text = "Get Confidential App Token";
            this.getConfigentialAppTokenButton.UseVisualStyleBackColor = true;
            this.getConfigentialAppTokenButton.Click += new System.EventHandler(this.GetConfigentialAppTokenButton_Click);
            // 
            // resourceSelectorComboBox
            // 
            this.resourceSelectorComboBox.FormattingEnabled = true;
            this.resourceSelectorComboBox.Items.AddRange(new object[] {
            "Select a Resource",
            "Patient",
            "Encounter",
            "Observation",
            "Condition"});
            this.resourceSelectorComboBox.Location = new System.Drawing.Point(394, 139);
            this.resourceSelectorComboBox.Name = "resourceSelectorComboBox";
            this.resourceSelectorComboBox.Size = new System.Drawing.Size(340, 39);
            this.resourceSelectorComboBox.TabIndex = 9;
            this.resourceSelectorComboBox.SelectedIndexChanged += new System.EventHandler(this.ResourceSelectorComboBox_SelectedIndexChanged);
            // 
            // fhirDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1641, 1073);
            this.Controls.Add(this.resourceSelectorComboBox);
            this.Controls.Add(this.getConfigentialAppTokenButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataTextBox);
            this.Controls.Add(this.resourceIDTextBox);
            this.Controls.Add(this.getDataButton);
            this.Controls.Add(this.uploadStatusLabel);
            this.Controls.Add(this.uploadDataButton);
            this.Controls.Add(this.statusTextBox);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "fhirDemoForm";
            this.Text = "FHIR Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Button uploadDataButton;
        private System.Windows.Forms.Label uploadStatusLabel;
        private System.Windows.Forms.Button getDataButton;
        private System.Windows.Forms.TextBox resourceIDTextBox;
        private System.Windows.Forms.TextBox dataTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button getConfigentialAppTokenButton;
        private System.Windows.Forms.ComboBox resourceSelectorComboBox;
    }
}

