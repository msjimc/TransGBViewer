namespace TransGBViewer
{
    partial class mRNAMiscellaneousFeatureSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mRNAMiscellaneousFeatureSelection));
            groupBox1 = new GroupBox();
            rdoCustomDialogBox = new RadioButton();
            rdoNameColour = new RadioButton();
            pCurrentColour = new PictureBox();
            btnColourSelection = new Button();
            label7 = new Label();
            label6 = new Label();
            cboList = new ComboBox();
            btnUncheck = new Button();
            label5 = new Label();
            btnCheck = new Button();
            lblSearchHitcount = new Label();
            txtSearchTerm = new TextBox();
            label4 = new Label();
            btnRemove = new Button();
            btnAdd = new Button();
            clbFeatures = new CheckedListBox();
            label3 = new Label();
            txtSetName = new TextBox();
            label2 = new Label();
            cboAccessionIDs = new ComboBox();
            label1 = new Label();
            btnCancel = new Button();
            btnAccept = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pCurrentColour).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(rdoCustomDialogBox);
            groupBox1.Controls.Add(rdoNameColour);
            groupBox1.Controls.Add(pCurrentColour);
            groupBox1.Controls.Add(btnColourSelection);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(cboList);
            groupBox1.Controls.Add(btnUncheck);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(btnCheck);
            groupBox1.Controls.Add(lblSearchHitcount);
            groupBox1.Controls.Add(txtSearchTerm);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(btnRemove);
            groupBox1.Controls.Add(btnAdd);
            groupBox1.Controls.Add(clbFeatures);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtSetName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cboAccessionIDs);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(615, 395);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Feature selection";
            // 
            // rdoCustomDialogBox
            // 
            rdoCustomDialogBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rdoCustomDialogBox.AutoSize = true;
            rdoCustomDialogBox.Location = new Point(403, 268);
            rdoCustomDialogBox.Name = "rdoCustomDialogBox";
            rdoCustomDialogBox.Size = new Size(96, 19);
            rdoCustomDialogBox.TabIndex = 20;
            rdoCustomDialogBox.Text = "Colour picker";
            rdoCustomDialogBox.UseVisualStyleBackColor = true;
            // 
            // rdoNameColour
            // 
            rdoNameColour.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rdoNameColour.AutoSize = true;
            rdoNameColour.Checked = true;
            rdoNameColour.Location = new Point(311, 268);
            rdoNameColour.Name = "rdoNameColour";
            rdoNameColour.Size = new Size(73, 19);
            rdoNameColour.TabIndex = 19;
            rdoNameColour.TabStop = true;
            rdoNameColour.Text = "By Name";
            rdoNameColour.UseVisualStyleBackColor = true;
            // 
            // pCurrentColour
            // 
            pCurrentColour.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pCurrentColour.Location = new Point(505, 268);
            pCurrentColour.Name = "pCurrentColour";
            pCurrentColour.Size = new Size(23, 23);
            pCurrentColour.TabIndex = 18;
            pCurrentColour.TabStop = false;
            // 
            // btnColourSelection
            // 
            btnColourSelection.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnColourSelection.Location = new Point(534, 268);
            btnColourSelection.Name = "btnColourSelection";
            btnColourSelection.Size = new Size(75, 23);
            btnColourSelection.TabIndex = 17;
            btnColourSelection.Text = "Colour";
            btnColourSelection.UseVisualStyleBackColor = true;
            btnColourSelection.Click += btnColourSelection_Click;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label7.Location = new Point(6, 267);
            label7.Name = "label7";
            label7.Size = new Size(297, 46);
            label7.TabIndex = 16;
            label7.Text = "To set the colour of the features, first select whether you wish to pick the colour by name or via a custom colour dialog box  and then press 'Colour'";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label6.Location = new Point(6, 354);
            label6.Name = "label6";
            label6.Size = new Size(285, 34);
            label6.TabIndex = 15;
            label6.Text = "To delete a set of features select its name from the dropdown list and press 'Remove'";
            // 
            // cboList
            // 
            cboList.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cboList.DropDownStyle = ComboBoxStyle.DropDownList;
            cboList.FormattingEnabled = true;
            cboList.Location = new Point(390, 365);
            cboList.Name = "cboList";
            cboList.Size = new Size(219, 23);
            cboList.TabIndex = 14;
            cboList.SelectedIndexChanged += cboList_SelectedIndexChanged;
            // 
            // btnUncheck
            // 
            btnUncheck.Location = new Point(220, 170);
            btnUncheck.Name = "btnUncheck";
            btnUncheck.Size = new Size(75, 23);
            btnUncheck.TabIndex = 13;
            btnUncheck.Text = "Uncheck";
            btnUncheck.UseVisualStyleBackColor = true;
            btnUncheck.Click += btnUncheck_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 170);
            label5.Name = "label5";
            label5.Size = new Size(113, 15);
            label5.TabIndex = 12;
            label5.Text = "Uncheck all features";
            // 
            // btnCheck
            // 
            btnCheck.Location = new Point(220, 137);
            btnCheck.Name = "btnCheck";
            btnCheck.Size = new Size(75, 23);
            btnCheck.TabIndex = 11;
            btnCheck.Text = "Check";
            btnCheck.UseVisualStyleBackColor = true;
            btnCheck.Click += btnCheck_Click;
            // 
            // lblSearchHitcount
            // 
            lblSearchHitcount.AutoSize = true;
            lblSearchHitcount.Location = new Point(6, 141);
            lblSearchHitcount.Name = "lblSearchHitcount";
            lblSearchHitcount.Size = new Size(93, 15);
            lblSearchHitcount.TabIndex = 10;
            lblSearchHitcount.Text = "Number of hits: ";
            // 
            // txtSearchTerm
            // 
            txtSearchTerm.Location = new Point(6, 109);
            txtSearchTerm.Name = "txtSearchTerm";
            txtSearchTerm.Size = new Size(289, 23);
            txtSearchTerm.TabIndex = 9;
            txtSearchTerm.TextChanged += txtSearchTerm_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 91);
            label4.Name = "label4";
            label4.Size = new Size(289, 15);
            label4.TabIndex = 8;
            label4.Text = "Select features containing this test in their description";
            // 
            // btnRemove
            // 
            btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRemove.Enabled = false;
            btnRemove.Location = new Point(309, 365);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(75, 23);
            btnRemove.TabIndex = 7;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.Enabled = false;
            btnAdd.Location = new Point(309, 321);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // clbFeatures
            // 
            clbFeatures.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            clbFeatures.FormattingEnabled = true;
            clbFeatures.Location = new Point(309, 60);
            clbFeatures.Name = "clbFeatures";
            clbFeatures.Size = new Size(300, 202);
            clbFeatures.TabIndex = 5;
            clbFeatures.ItemCheck += clbFeatures_ItemCheck;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 60);
            label3.Name = "label3";
            label3.Size = new Size(224, 15);
            label3.TabIndex = 4;
            label3.Text = "Select the features to be shown in this set";
            // 
            // txtSetName
            // 
            txtSetName.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtSetName.Location = new Point(390, 321);
            txtSetName.Name = "txtSetName";
            txtSetName.Size = new Size(219, 23);
            txtSetName.TabIndex = 3;
            txtSetName.TextChanged += txtSetName_TextChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.Location = new Point(6, 321);
            label2.Name = "label2";
            label2.Size = new Size(297, 31);
            label2.TabIndex = 2;
            label2.Text = "Enter the display name for the set of feature to be displayed on this line";
            // 
            // cboAccessionIDs
            // 
            cboAccessionIDs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboAccessionIDs.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAccessionIDs.FormattingEnabled = true;
            cboAccessionIDs.Location = new Point(309, 22);
            cboAccessionIDs.Name = "cboAccessionIDs";
            cboAccessionIDs.Size = new Size(300, 23);
            cboAccessionIDs.TabIndex = 1;
            cboAccessionIDs.SelectedIndexChanged += cboAccessionIDs_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(297, 35);
            label1.TabIndex = 0;
            label1.Text = "Select the accession ID of the sequence linked to the miscellaneous feature";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(552, 413);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Location = new Point(471, 413);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(75, 23);
            btnAccept.TabIndex = 3;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            // 
            // mRNAMiscellaneousFeatureSelection
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(639, 448);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "mRNAMiscellaneousFeatureSelection";
            Text = "Miscellaneous Genbank Feature Selection";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pCurrentColour).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cboAccessionIDs;
        private Label label1;
        private Button btnCancel;
        private Button btnAccept;
        private TextBox txtSetName;
        private Label label2;
        private CheckedListBox clbFeatures;
        private Label label3;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnCheck;
        private Label lblSearchHitcount;
        private TextBox txtSearchTerm;
        private Label label4;
        private Button btnUncheck;
        private Label label5;
        private PictureBox pCurrentColour;
        private Button btnColourSelection;
        private Label label7;
        private Label label6;
        private ComboBox cboList;
        private RadioButton rdoCustomDialogBox;
        private RadioButton rdoNameColour;
    }
}