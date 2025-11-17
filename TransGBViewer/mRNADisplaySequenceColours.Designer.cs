namespace TransGBViewer
{
    partial class mRNADisplaySequenceColours
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mRNADisplaySequenceColours));
            groupBox1 = new GroupBox();
            btnColourSelection = new Button();
            label4 = new Label();
            rdoCustomDialogBox = new RadioButton();
            rdoNameColour = new RadioButton();
            label3 = new Label();
            chkNonCoding = new CheckBox();
            chkCoding = new CheckBox();
            label2 = new Label();
            clbSequenceNames = new CheckedListBox();
            label1 = new Label();
            btnAccept = new Button();
            btnCancel = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(btnColourSelection);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(rdoCustomDialogBox);
            groupBox1.Controls.Add(rdoNameColour);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(chkNonCoding);
            groupBox1.Controls.Add(chkCoding);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(clbSequenceNames);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(463, 303);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // btnColourSelection
            // 
            btnColourSelection.Location = new Point(152, 265);
            btnColourSelection.Name = "btnColourSelection";
            btnColourSelection.Size = new Size(75, 23);
            btnColourSelection.TabIndex = 9;
            btnColourSelection.Text = "Select";
            btnColourSelection.UseVisualStyleBackColor = true;
            btnColourSelection.Click += btnColourSelection_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 247);
            label4.Name = "label4";
            label4.Size = new Size(170, 15);
            label4.TabIndex = 8;
            label4.Text = "Press 'Select' to pick the colour";
            // 
            // rdoCustomDialogBox
            // 
            rdoCustomDialogBox.AutoSize = true;
            rdoCustomDialogBox.Location = new Point(117, 208);
            rdoCustomDialogBox.Name = "rdoCustomDialogBox";
            rdoCustomDialogBox.Size = new Size(96, 19);
            rdoCustomDialogBox.TabIndex = 7;
            rdoCustomDialogBox.Text = "Colour picker";
            rdoCustomDialogBox.UseVisualStyleBackColor = true;
            // 
            // rdoNameColour
            // 
            rdoNameColour.AutoSize = true;
            rdoNameColour.Checked = true;
            rdoNameColour.Location = new Point(6, 208);
            rdoNameColour.Name = "rdoNameColour";
            rdoNameColour.Size = new Size(73, 19);
            rdoNameColour.TabIndex = 6;
            rdoNameColour.TabStop = true;
            rdoNameColour.Text = "By Name";
            rdoNameColour.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.Location = new Point(6, 152);
            label3.Name = "label3";
            label3.Size = new Size(221, 46);
            label3.TabIndex = 5;
            label3.Text = "Select whether to select the colour by name or with the custom colour picker.";
            // 
            // chkNonCoding
            // 
            chkNonCoding.AutoSize = true;
            chkNonCoding.Location = new Point(117, 130);
            chkNonCoding.Name = "chkNonCoding";
            chkNonCoding.Size = new Size(91, 19);
            chkNonCoding.TabIndex = 4;
            chkNonCoding.Text = "Non-coding";
            chkNonCoding.UseVisualStyleBackColor = true;
            chkNonCoding.CheckedChanged += chkNonCoding_CheckedChanged;
            // 
            // chkCoding
            // 
            chkCoding.AutoSize = true;
            chkCoding.Location = new Point(6, 130);
            chkCoding.Name = "chkCoding";
            chkCoding.Size = new Size(68, 19);
            chkCoding.TabIndex = 3;
            chkCoding.Text = "Coding ";
            chkCoding.UseVisualStyleBackColor = true;
            chkCoding.CheckedChanged += chkCoding_CheckedChanged;
            // 
            // label2
            // 
            label2.Location = new Point(0, 72);
            label2.Name = "label2";
            label2.Size = new Size(227, 55);
            label2.TabIndex = 2;
            label2.Text = "Select whether you want to change  the colour of the codiing, non-coding or both types of sequence.";
            // 
            // clbSequenceNames
            // 
            clbSequenceNames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            clbSequenceNames.FormattingEnabled = true;
            clbSequenceNames.Location = new Point(239, 22);
            clbSequenceNames.Name = "clbSequenceNames";
            clbSequenceNames.Size = new Size(218, 274);
            clbSequenceNames.TabIndex = 1;
            clbSequenceNames.SelectedIndexChanged += clbSequenceNames_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Location = new Point(0, 22);
            label1.Name = "label1";
            label1.Size = new Size(227, 50);
            label1.TabIndex = 0;
            label1.Text = "Select the accession IDs of the sequence you which to change in the list on the right. ";
            // 
            // btnAccept
            // 
            btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Location = new Point(313, 321);
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
            btnCancel.Location = new Point(394, 321);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // mRNADisplaySequenceColours
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(487, 356);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(503, 395);
            Name = "mRNADisplaySequenceColours";
            Text = "Sequence colour selection";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private CheckedListBox clbSequenceNames;
        private Label label1;
        private Button btnAccept;
        private Button btnCancel;
        private Label label3;
        private CheckBox chkNonCoding;
        private CheckBox chkCoding;
        private Label label2;
        private Button btnColourSelection;
        private Label label4;
        private RadioButton rdoCustomDialogBox;
        private RadioButton rdoNameColour;
    }
}