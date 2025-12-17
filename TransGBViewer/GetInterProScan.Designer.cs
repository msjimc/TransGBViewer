namespace TransGBViewer
{
    partial class GetInterProScan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetInterProScan));
            groupBox5 = new GroupBox();
            btnImportSearch = new Button();
            label42 = new Label();
            btnSaveDomainString = new Button();
            label41 = new Label();
            btnSearch = new Button();
            label39 = new Label();
            btnAccept = new Button();
            btnCancel = new Button();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(btnImportSearch);
            groupBox5.Controls.Add(label42);
            groupBox5.Controls.Add(btnSaveDomainString);
            groupBox5.Controls.Add(label41);
            groupBox5.Controls.Add(btnSearch);
            groupBox5.Controls.Add(label39);
            groupBox5.Location = new Point(12, 12);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(465, 117);
            groupBox5.TabIndex = 0;
            groupBox5.TabStop = false;
            groupBox5.Text = "Search InterProScan for domains";
            // 
            // btnImportSearch
            // 
            btnImportSearch.Location = new Point(384, 22);
            btnImportSearch.Name = "btnImportSearch";
            btnImportSearch.Size = new Size(75, 23);
            btnImportSearch.TabIndex = 1;
            btnImportSearch.Text = "Import";
            btnImportSearch.UseVisualStyleBackColor = true;
            btnImportSearch.Click += btnImportSearch_Click;
            // 
            // label42
            // 
            label42.AutoSize = true;
            label42.Location = new Point(6, 26);
            label42.Name = "label42";
            label42.Size = new Size(270, 15);
            label42.TabIndex = 0;
            label42.Text = "To import a previously saved search press 'Import'";
            // 
            // btnSaveDomainString
            // 
            btnSaveDomainString.Enabled = false;
            btnSaveDomainString.Location = new Point(384, 80);
            btnSaveDomainString.Name = "btnSaveDomainString";
            btnSaveDomainString.Size = new Size(75, 23);
            btnSaveDomainString.TabIndex = 5;
            btnSaveDomainString.Text = "Save";
            btnSaveDomainString.UseVisualStyleBackColor = true;
            btnSaveDomainString.Click += btnSaveDomainString_Click;
            // 
            // label41
            // 
            label41.AutoSize = true;
            label41.Location = new Point(6, 84);
            label41.Name = "label41";
            label41.Size = new Size(165, 15);
            label41.TabIndex = 4;
            label41.Text = "Save the response as a text file";
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(384, 51);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // label39
            // 
            label39.Location = new Point(6, 41);
            label39.Name = "label39";
            label39.Size = new Size(352, 33);
            label39.TabIndex = 2;
            label39.Text = "To search InterProScan for domain in a sequence in the alignment press 'Search'";
            // 
            // btnAccept
            // 
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Location = new Point(315, 138);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(75, 23);
            btnAccept.TabIndex = 1;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            btnAccept.Click += btnAccept_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(396, 138);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // GetInterProScan
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(486, 167);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(groupBox5);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimizeBox = false;
            Name = "GetInterProScan";
            Text = "Import InterProScan features";
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox5;
        private Button btnImportSearch;
        private Label label42;
        private Button btnSaveDomainString;
        private Label label41;
        private Button btnSearch;
        private Label label39;
        private Button btnAccept;
        private Button btnCancel;
    }
}