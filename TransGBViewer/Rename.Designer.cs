namespace TransGBViewer
{
    partial class Rename
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rename));
            groupBox1 = new GroupBox();
            label2 = new Label();
            btnRemove = new Button();
            btnAdd = new Button();
            lvUser = new ListView();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader1 = new ColumnHeader();
            lvAlphabet = new ListView();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            txtNewName = new TextBox();
            label1 = new Label();
            cboTranscript = new ComboBox();
            btnCancel = new Button();
            btnAccept = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(btnRemove);
            groupBox1.Controls.Add(btnAdd);
            groupBox1.Controls.Add(lvUser);
            groupBox1.Controls.Add(lvAlphabet);
            groupBox1.Controls.Add(txtNewName);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(cboTranscript);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(834, 341);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 101);
            label2.Name = "label2";
            label2.Size = new Size(819, 15);
            label2.TabIndex = 3;
            label2.Text = "To include a sequence in the image, select it in the lefthand list and press the '>' button. To remove it select it in the righthand list and press the '<' button.";
            // 
            // btnRemove
            // 
            btnRemove.Location = new Point(389, 219);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(58, 23);
            btnRemove.TabIndex = 6;
            btnRemove.Text = "<";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(389, 190);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(58, 23);
            btnAdd.TabIndex = 5;
            btnAdd.Text = ">";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // lvUser
            // 
            lvUser.Columns.AddRange(new ColumnHeader[] { columnHeader3, columnHeader4, columnHeader1 });
            lvUser.Location = new Point(464, 119);
            lvUser.Name = "lvUser";
            lvUser.Size = new Size(361, 216);
            lvUser.TabIndex = 7;
            lvUser.UseCompatibleStateImageBehavior = false;
            lvUser.View = View.Details;
            lvUser.SelectedIndexChanged += lvUser_SelectedIndexChanged;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Order";
            columnHeader3.Width = 46;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Transcript";
            columnHeader4.Width = 80;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Display name";
            columnHeader1.Width = 235;
            // 
            // lvAlphabet
            // 
            lvAlphabet.Columns.AddRange(new ColumnHeader[] { columnHeader5, columnHeader6, columnHeader7 });
            lvAlphabet.Location = new Point(6, 119);
            lvAlphabet.Name = "lvAlphabet";
            lvAlphabet.Size = new Size(360, 216);
            lvAlphabet.TabIndex = 4;
            lvAlphabet.UseCompatibleStateImageBehavior = false;
            lvAlphabet.View = View.Details;
            lvAlphabet.SelectedIndexChanged += lvAlphabet_SelectedIndexChanged;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Order";
            columnHeader5.Width = 46;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Transcript";
            columnHeader6.Width = 80;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "Display names";
            columnHeader7.Width = 235;
            // 
            // txtNewName
            // 
            txtNewName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNewName.Location = new Point(6, 66);
            txtNewName.Name = "txtNewName";
            txtNewName.Size = new Size(822, 23);
            txtNewName.TabIndex = 2;
            txtNewName.TextChanged += txtNewName_TextChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(629, 15);
            label1.TabIndex = 0;
            label1.Text = "To rename a selected transcripts, select it in the dropdown box below and then type the prefered name in the text area.";
            // 
            // cboTranscript
            // 
            cboTranscript.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboTranscript.FormattingEnabled = true;
            cboTranscript.Location = new Point(6, 37);
            cboTranscript.Name = "cboTranscript";
            cboTranscript.Size = new Size(822, 23);
            cboTranscript.TabIndex = 1;
            cboTranscript.SelectedIndexChanged += cboTranscript_SelectedIndexChanged;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(765, 359);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Location = new Point(684, 359);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(75, 23);
            btnAccept.TabIndex = 1;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            btnAccept.Click += btnAccept_Click;
            // 
            // Rename
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(858, 392);
            Controls.Add(btnAccept);
            Controls.Add(btnCancel);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Rename";
            Text = "Rename transcripts";
            FormClosed += Rename_FormClosed;
            Load += Rename_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtNewName;
        private Label label1;
        private ComboBox cboTranscript;
        private ListView lvAlphabet;
        private ListView lvUser;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private Button btnRemove;
        private Button btnAdd;
        private ColumnHeader columnHeader1;
        private Label label2;
        private Button btnCancel;
        private Button btnAccept;
    }
}