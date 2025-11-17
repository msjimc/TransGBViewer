namespace TransGBViewer
{
    partial class AAColourSelectionListBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AAColourSelectionListBox));
            btnCancel = new Button();
            btnAccept = new Button();
            groupBox1 = new GroupBox();
            label1 = new Label();
            txtSuggestion = new TextBox();
            p1Example = new PictureBox();
            cboColours = new ComboBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)p1Example).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Abort;
            btnCancel.Location = new Point(264, 174);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Location = new Point(183, 174);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(75, 23);
            btnAccept.TabIndex = 6;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            btnAccept.Click += btnAccept_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtSuggestion);
            groupBox1.Controls.Add(p1Example);
            groupBox1.Controls.Add(cboColours);
            groupBox1.Location = new Point(10, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(325, 152);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Select";
            // 
            // label1
            // 
            label1.Location = new Point(8, 19);
            label1.Name = "label1";
            label1.Size = new Size(311, 31);
            label1.TabIndex = 6;
            label1.Text = "Start to write the colour's name in the text area below and the select from the suggested names";
            // 
            // txtSuggestion
            // 
            txtSuggestion.Location = new Point(6, 65);
            txtSuggestion.Name = "txtSuggestion";
            txtSuggestion.Size = new Size(313, 23);
            txtSuggestion.TabIndex = 4;
            txtSuggestion.TextChanged += txtSuggestion_TextChanged;
            // 
            // p1Example
            // 
            p1Example.Location = new Point(244, 123);
            p1Example.Name = "p1Example";
            p1Example.Size = new Size(75, 23);
            p1Example.TabIndex = 3;
            p1Example.TabStop = false;
            // 
            // cboColours
            // 
            cboColours.DropDownStyle = ComboBoxStyle.DropDownList;
            cboColours.FormattingEnabled = true;
            cboColours.Location = new Point(6, 94);
            cboColours.Name = "cboColours";
            cboColours.Size = new Size(313, 23);
            cboColours.TabIndex = 0;
            cboColours.SelectedIndexChanged += cboColours_SelectedIndexChanged;
            // 
            // AAColourSelectionListBox
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(349, 209);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AAColourSelectionListBox";
            Text = "Amino acid colour scheme";
            Load += AAColourSelectionListBox_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)p1Example).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnCancel;
        private Button btnAccept;
        private GroupBox groupBox1;
        private TextBox txtSuggestion;
        private PictureBox p1Example;
        private ComboBox cboColours;
        private Label label1;
    }
}