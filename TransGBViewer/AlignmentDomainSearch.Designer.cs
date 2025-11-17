namespace TransGBViewer
{
    partial class AlignmentDomainSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlignmentDomainSearch));
            groupBox1 = new GroupBox();
            txtStatus = new TextBox();
            btnSubmit = new Button();
            label4 = new Label();
            txtJobTitle = new TextBox();
            label3 = new Label();
            txtEmailAddress = new TextBox();
            label2 = new Label();
            cboNames = new ComboBox();
            label1 = new Label();
            btnAccept = new Button();
            btnCancel = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(txtStatus);
            groupBox1.Controls.Add(btnSubmit);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtJobTitle);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtEmailAddress);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cboNames);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(471, 397);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Searching InterProScan";
            // 
            // txtStatus
            // 
            txtStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtStatus.Location = new Point(6, 164);
            txtStatus.Multiline = true;
            txtStatus.Name = "txtStatus";
            txtStatus.ScrollBars = ScrollBars.Both;
            txtStatus.Size = new Size(459, 227);
            txtStatus.TabIndex = 8;
            txtStatus.WordWrap = false;
            // 
            // btnSubmit
            // 
            btnSubmit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSubmit.Location = new Point(390, 125);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(75, 23);
            btnSubmit.TabIndex = 7;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 129);
            label4.Name = "label4";
            label4.Size = new Size(158, 15);
            label4.TabIndex = 6;
            label4.Text = "Press 'Submit' to start search";
            // 
            // txtJobTitle
            // 
            txtJobTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtJobTitle.Location = new Point(254, 96);
            txtJobTitle.Name = "txtJobTitle";
            txtJobTitle.Size = new Size(211, 23);
            txtJobTitle.TabIndex = 5;
            txtJobTitle.TextChanged += txtJobTitle_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 99);
            label3.Name = "label3";
            label3.Size = new Size(123, 15);
            label3.TabIndex = 4;
            label3.Text = "Enter a search job title";
            // 
            // txtEmailAddress
            // 
            txtEmailAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtEmailAddress.Location = new Point(254, 67);
            txtEmailAddress.Name = "txtEmailAddress";
            txtEmailAddress.Size = new Size(211, 23);
            txtEmailAddress.TabIndex = 3;
            txtEmailAddress.TextChanged += txtEmailAddress_TextChanged;
            // 
            // label2
            // 
            label2.Location = new Point(6, 54);
            label2.Name = "label2";
            label2.Size = new Size(242, 36);
            label2.TabIndex = 2;
            label2.Text = "Enter a valid email address (required by InterProScan)";
            // 
            // cboNames
            // 
            cboNames.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboNames.FormattingEnabled = true;
            cboNames.Location = new Point(254, 22);
            cboNames.Name = "cboNames";
            cboNames.Size = new Size(211, 23);
            cboNames.TabIndex = 1;
            cboNames.SelectedIndexChanged += cboNames_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(242, 15);
            label1.TabIndex = 0;
            label1.Text = "Select the sequence's name in the alignment";
            // 
            // btnAccept
            // 
            btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Location = new Point(321, 415);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(75, 23);
            btnAccept.TabIndex = 1;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(402, 415);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // AlignmentDomainSearch
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(495, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AlignmentDomainSearch";
            Text = "Sequence domain search";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnAccept;
        private Button btnCancel;
        private TextBox txtJobTitle;
        private Label label3;
        private TextBox txtEmailAddress;
        private Label label2;
        private ComboBox cboNames;
        private Label label1;
        private Button btnSubmit;
        private Label label4;
        private TextBox txtStatus;
    }
}