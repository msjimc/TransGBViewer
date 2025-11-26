namespace TransGBViewer
{
    partial class AlignmentFeatureSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlignmentFeatureSelection));
            groupBox1 = new GroupBox();
            txtDisplayName = new TextBox();
            btnRemove = new Button();
            btnAdd = new Button();
            label12 = new Label();
            label11 = new Label();
            pStyle = new PictureBox();
            btnFillColour = new Button();
            chkFillShape = new CheckBox();
            chkDrawBorder = new CheckBox();
            chkRoundedDomains = new CheckBox();
            label10 = new Label();
            label9 = new Label();
            lblDescription = new Label();
            cboInterproDescriptionValue = new ComboBox();
            label8 = new Label();
            cboInterProValue = new ComboBox();
            label7 = new Label();
            cboSignitureDescriptionValue = new ComboBox();
            label6 = new Label();
            cboSignatureValue = new ComboBox();
            label5 = new Label();
            cboAnalysisValue = new ComboBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            cboFeatureName = new ComboBox();
            cboSequenceNames = new ComboBox();
            label1 = new Label();
            btnAccept = new Button();
            btnCancel = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pStyle).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(txtDisplayName);
            groupBox1.Controls.Add(btnRemove);
            groupBox1.Controls.Add(btnAdd);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(pStyle);
            groupBox1.Controls.Add(btnFillColour);
            groupBox1.Controls.Add(chkFillShape);
            groupBox1.Controls.Add(chkDrawBorder);
            groupBox1.Controls.Add(chkRoundedDomains);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(lblDescription);
            groupBox1.Controls.Add(cboInterproDescriptionValue);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(cboInterProValue);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(cboSignitureDescriptionValue);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(cboSignatureValue);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(cboAnalysisValue);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cboFeatureName);
            groupBox1.Controls.Add(cboSequenceNames);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(583, 376);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Domain selection options";
            // 
            // txtDisplayName
            // 
            txtDisplayName.Location = new Point(275, 239);
            txtDisplayName.Name = "txtDisplayName";
            txtDisplayName.Size = new Size(302, 23);
            txtDisplayName.TabIndex = 3;
            // 
            // btnRemove
            // 
            btnRemove.Enabled = false;
            btnRemove.Location = new Point(502, 80);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(75, 23);
            btnRemove.TabIndex = 30;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.Location = new Point(421, 80);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 28;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // label12
            // 
            label12.Location = new Point(6, 77);
            label12.Name = "label12";
            label12.Size = new Size(313, 31);
            label12.TabIndex = 27;
            label12.Text = "To add, update or remove the selected domain press 'Add', 'Update' or 'Remove'";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(6, 301);
            label11.Name = "label11";
            label11.Size = new Size(76, 15);
            label11.TabIndex = 26;
            label11.Text = "Domain style";
            // 
            // pStyle
            // 
            pStyle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pStyle.Location = new Point(88, 296);
            pStyle.Name = "pStyle";
            pStyle.Size = new Size(489, 69);
            pStyle.TabIndex = 25;
            pStyle.TabStop = false;
            // 
            // btnFillColour
            // 
            btnFillColour.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFillColour.Location = new Point(521, 268);
            btnFillColour.Name = "btnFillColour";
            btnFillColour.Size = new Size(56, 23);
            btnFillColour.TabIndex = 24;
            btnFillColour.Text = "Colour";
            btnFillColour.UseVisualStyleBackColor = true;
            btnFillColour.Click += btnFillColour_Click;
            // 
            // chkFillShape
            // 
            chkFillShape.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkFillShape.AutoSize = true;
            chkFillShape.Checked = true;
            chkFillShape.CheckState = CheckState.Checked;
            chkFillShape.Location = new Point(440, 271);
            chkFillShape.Name = "chkFillShape";
            chkFillShape.Size = new Size(75, 19);
            chkFillShape.TabIndex = 21;
            chkFillShape.Text = "Fill shape";
            chkFillShape.UseVisualStyleBackColor = true;
            chkFillShape.CheckedChanged += chkFillShape_CheckedChanged;
            // 
            // chkDrawBorder
            // 
            chkDrawBorder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkDrawBorder.AutoSize = true;
            chkDrawBorder.Checked = true;
            chkDrawBorder.CheckState = CheckState.Checked;
            chkDrawBorder.Location = new Point(334, 271);
            chkDrawBorder.Name = "chkDrawBorder";
            chkDrawBorder.Size = new Size(91, 19);
            chkDrawBorder.TabIndex = 20;
            chkDrawBorder.Text = "Draw border";
            chkDrawBorder.UseVisualStyleBackColor = true;
            chkDrawBorder.CheckedChanged += chkDrawBorder_CheckedChanged;
            // 
            // chkRoundedDomains
            // 
            chkRoundedDomains.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkRoundedDomains.AutoSize = true;
            chkRoundedDomains.Checked = true;
            chkRoundedDomains.CheckState = CheckState.Checked;
            chkRoundedDomains.Location = new Point(206, 272);
            chkRoundedDomains.Name = "chkRoundedDomains";
            chkRoundedDomains.Size = new Size(116, 19);
            chkRoundedDomains.TabIndex = 19;
            chkRoundedDomains.Text = "Rounded corners";
            chkRoundedDomains.UseVisualStyleBackColor = true;
            chkRoundedDomains.CheckedChanged += chkRoundedDomains_CheckedChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 272);
            label10.Name = "label10";
            label10.Size = new Size(194, 15);
            label10.TabIndex = 18;
            label10.Text = "Select the domains style and colour";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 242);
            label9.Name = "label9";
            label9.Size = new Size(263, 15);
            label9.TabIndex = 16;
            label9.Text = "Enter an alternative display name for the domain";
            // 
            // lblDescription
            // 
            lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblDescription.Location = new Point(6, 217);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(571, 19);
            lblDescription.TabIndex = 15;
            lblDescription.Text = "Description: -";
            // 
            // cboInterproDescriptionValue
            // 
            cboInterproDescriptionValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboInterproDescriptionValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cboInterproDescriptionValue.FormattingEnabled = true;
            cboInterproDescriptionValue.Items.AddRange(new object[] { "Hide", "1", "2", "3", "4", "5" });
            cboInterproDescriptionValue.Location = new Point(521, 191);
            cboInterproDescriptionValue.Name = "cboInterproDescriptionValue";
            cboInterproDescriptionValue.Size = new Size(56, 23);
            cboInterproDescriptionValue.TabIndex = 14;
            cboInterproDescriptionValue.SelectedIndexChanged += cboInterproDescriptionValue_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(396, 194);
            label8.Name = "label8";
            label8.Size = new Size(111, 15);
            label8.TabIndex = 13;
            label8.Text = "InterPro description";
            // 
            // cboInterProValue
            // 
            cboInterProValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboInterProValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cboInterProValue.FormattingEnabled = true;
            cboInterProValue.Items.AddRange(new object[] { "Hide", "1", "2", "3", "4", "5" });
            cboInterProValue.Location = new Point(285, 191);
            cboInterProValue.Name = "cboInterProValue";
            cboInterProValue.Size = new Size(56, 23);
            cboInterProValue.TabIndex = 12;
            cboInterProValue.SelectedIndexChanged += cboInterProValue_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(230, 194);
            label7.Name = "label7";
            label7.Size = new Size(49, 15);
            label7.TabIndex = 11;
            label7.Text = "InterPro";
            // 
            // cboSignitureDescriptionValue
            // 
            cboSignitureDescriptionValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboSignitureDescriptionValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSignitureDescriptionValue.FormattingEnabled = true;
            cboSignitureDescriptionValue.Items.AddRange(new object[] { "Hide", "1", "2", "3", "4", "5" });
            cboSignitureDescriptionValue.Location = new Point(521, 162);
            cboSignitureDescriptionValue.Name = "cboSignitureDescriptionValue";
            cboSignitureDescriptionValue.Size = new Size(56, 23);
            cboSignitureDescriptionValue.TabIndex = 10;
            cboSignitureDescriptionValue.SelectedIndexChanged += cboSignitureDescriptionValue_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(396, 165);
            label6.Name = "label6";
            label6.Size = new Size(119, 15);
            label6.TabIndex = 9;
            label6.Text = "Signature description";
            // 
            // cboSignatureValue
            // 
            cboSignatureValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboSignatureValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSignatureValue.FormattingEnabled = true;
            cboSignatureValue.Items.AddRange(new object[] { "Hide", "1", "2", "3", "4", "5" });
            cboSignatureValue.Location = new Point(285, 162);
            cboSignatureValue.Name = "cboSignatureValue";
            cboSignatureValue.Size = new Size(56, 23);
            cboSignatureValue.TabIndex = 8;
            cboSignatureValue.SelectedIndexChanged += cboSignatureValue_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(224, 165);
            label5.Name = "label5";
            label5.Size = new Size(57, 15);
            label5.TabIndex = 7;
            label5.Text = "Signature";
            // 
            // cboAnalysisValue
            // 
            cboAnalysisValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboAnalysisValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAnalysisValue.FormattingEnabled = true;
            cboAnalysisValue.Items.AddRange(new object[] { "Hide", "1", "2", "3", "4", "5" });
            cboAnalysisValue.Location = new Point(88, 162);
            cboAnalysisValue.Name = "cboAnalysisValue";
            cboAnalysisValue.Size = new Size(56, 23);
            cboAnalysisValue.TabIndex = 6;
            cboAnalysisValue.SelectedIndexChanged += cboAnalysisValue_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(29, 165);
            label4.Name = "label4";
            label4.Size = new Size(53, 15);
            label4.TabIndex = 5;
            label4.Text = "Analysis:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label3.Location = new Point(6, 118);
            label3.Name = "label3";
            label3.Size = new Size(571, 35);
            label3.TabIndex = 4;
            label3.Text = "Create the text used to describe the feature in the display. Select a value to the right of the parameter: 1 while add the text first, while 5 will add it last (hide will ignore the text).";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 54);
            label2.Name = "label2";
            label2.Size = new Size(273, 15);
            label2.TabIndex = 3;
            label2.Text = "Select the domian using it's name and description.";
            // 
            // cboFeatureName
            // 
            cboFeatureName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboFeatureName.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFeatureName.FormattingEnabled = true;
            cboFeatureName.Location = new Point(285, 51);
            cboFeatureName.Name = "cboFeatureName";
            cboFeatureName.Size = new Size(292, 23);
            cboFeatureName.TabIndex = 2;
            cboFeatureName.SelectedIndexChanged += cboFeatureName_SelectedIndexChanged;
            // 
            // cboSequenceNames
            // 
            cboSequenceNames.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboSequenceNames.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSequenceNames.FormattingEnabled = true;
            cboSequenceNames.Location = new Point(285, 22);
            cboSequenceNames.Name = "cboSequenceNames";
            cboSequenceNames.Size = new Size(292, 23);
            cboSequenceNames.TabIndex = 1;
            cboSequenceNames.SelectedIndexChanged += cboSequenceNames_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(259, 15);
            label1.TabIndex = 0;
            label1.Text = "Select the sequence used to retrieve the domain";
            // 
            // btnAccept
            // 
            btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Location = new Point(439, 399);
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
            btnCancel.Location = new Point(520, 399);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // AlignmentFeatureSelection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(602, 434);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "AlignmentFeatureSelection";
            Text = "Domain selection";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pStyle).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label2;
        private ComboBox cboFeatureName;
        private ComboBox cboSequenceNames;
        private Label label1;
        private Button btnAccept;
        private Button btnCancel;
        private Label label3;
        private ComboBox cboInterproDescriptionValue;
        private Label label8;
        private ComboBox cboInterProValue;
        private Label label7;
        private ComboBox cboSignitureDescriptionValue;
        private Label label6;
        private ComboBox cboSignatureValue;
        private Label label5;
        private ComboBox cboAnalysisValue;
        private Label label4;
        private Label label9;
        private Label lblDescription;
        private CheckBox chkFillShape;
        private CheckBox chkDrawBorder;
        private CheckBox chkRoundedDomains;
        private Label label10;
        private Button btnFillColour;
        private Label label11;
        private PictureBox pStyle;
        private Button btnRemove;
        private Button btnAdd;
        private Label label12;
        private TextBox txtDisplayName;
    }
}