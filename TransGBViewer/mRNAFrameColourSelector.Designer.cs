namespace TransGBViewer
{
    partial class mRNAFrameColourSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mRNAFrameColourSelector));
            btnCancel = new Button();
            btnAccept = new Button();
            groupBox1 = new GroupBox();
            btnPlusTwo = new Button();
            btnPlusOne = new Button();
            btnPlusZero = new Button();
            pPlusTwo = new PictureBox();
            pPlusOne = new PictureBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            pPlusZero = new PictureBox();
            lblInstructions = new Label();
            rdoDialog = new RadioButton();
            rdoList = new RadioButton();
            label6 = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pPlusTwo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pPlusOne).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pPlusZero).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(503, 179);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Location = new Point(422, 179);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(75, 23);
            btnAccept.TabIndex = 1;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(btnPlusTwo);
            groupBox1.Controls.Add(btnPlusOne);
            groupBox1.Controls.Add(btnPlusZero);
            groupBox1.Controls.Add(pPlusTwo);
            groupBox1.Controls.Add(pPlusOne);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(pPlusZero);
            groupBox1.Controls.Add(lblInstructions);
            groupBox1.Controls.Add(rdoDialog);
            groupBox1.Controls.Add(rdoList);
            groupBox1.Controls.Add(label6);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(566, 161);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Colour selection";
            // 
            // btnPlusTwo
            // 
            btnPlusTwo.Location = new Point(485, 121);
            btnPlusTwo.Name = "btnPlusTwo";
            btnPlusTwo.Size = new Size(75, 23);
            btnPlusTwo.TabIndex = 9;
            btnPlusTwo.Text = "Select";
            btnPlusTwo.UseVisualStyleBackColor = true;
            btnPlusTwo.Click += btnPlusTwo_Click;
            // 
            // btnPlusOne
            // 
            btnPlusOne.Location = new Point(485, 92);
            btnPlusOne.Name = "btnPlusOne";
            btnPlusOne.Size = new Size(75, 23);
            btnPlusOne.TabIndex = 7;
            btnPlusOne.Text = "Select";
            btnPlusOne.UseVisualStyleBackColor = true;
            btnPlusOne.Click += btnPlusOne_Click;
            // 
            // btnPlusZero
            // 
            btnPlusZero.Location = new Point(485, 63);
            btnPlusZero.Name = "btnPlusZero";
            btnPlusZero.Size = new Size(75, 23);
            btnPlusZero.TabIndex = 5;
            btnPlusZero.Text = "Select";
            btnPlusZero.UseVisualStyleBackColor = true;
            btnPlusZero.Click += btnPlusZero_Click;
            // 
            // pPlusTwo
            // 
            pPlusTwo.Location = new Point(456, 121);
            pPlusTwo.Name = "pPlusTwo";
            pPlusTwo.Size = new Size(23, 23);
            pPlusTwo.TabIndex = 63;
            pPlusTwo.TabStop = false;
            // 
            // pPlusOne
            // 
            pPlusOne.Location = new Point(456, 92);
            pPlusOne.Name = "pPlusOne";
            pPlusOne.Size = new Size(23, 23);
            pPlusOne.TabIndex = 61;
            pPlusOne.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(316, 125);
            label3.Name = "label3";
            label3.Size = new Size(98, 15);
            label3.TabIndex = 8;
            label3.Text = "+2 reading frame";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(316, 96);
            label2.Name = "label2";
            label2.Size = new Size(98, 15);
            label2.TabIndex = 6;
            label2.Text = "+1 reading frame";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(322, 67);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 4;
            label1.Text = "The same frame";
            // 
            // pPlusZero
            // 
            pPlusZero.Location = new Point(456, 63);
            pPlusZero.Name = "pPlusZero";
            pPlusZero.Size = new Size(23, 23);
            pPlusZero.TabIndex = 56;
            pPlusZero.TabStop = false;
            // 
            // lblInstructions
            // 
            lblInstructions.AutoSize = true;
            lblInstructions.Location = new Point(6, 67);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(296, 15);
            lblInstructions.TabIndex = 3;
            lblInstructions.Text = "Select the colour of the open reading frames that have:";
            // 
            // rdoDialog
            // 
            rdoDialog.AutoSize = true;
            rdoDialog.Location = new Point(441, 15);
            rdoDialog.Name = "rdoDialog";
            rdoDialog.Size = new Size(119, 19);
            rdoDialog.TabIndex = 2;
            rdoDialog.Text = "Colour dialog box";
            rdoDialog.UseVisualStyleBackColor = true;
            // 
            // rdoList
            // 
            rdoList.AutoSize = true;
            rdoList.Checked = true;
            rdoList.Location = new Point(316, 15);
            rdoList.Name = "rdoList";
            rdoList.Size = new Size(95, 19);
            rdoList.TabIndex = 1;
            rdoList.TabStop = true;
            rdoList.Text = "List of names";
            rdoList.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Location = new Point(6, 19);
            label6.Name = "label6";
            label6.Size = new Size(235, 37);
            label6.TabIndex = 0;
            label6.Text = "Select the colour using a dialog box or a list of predefined names:";
            // 
            // mRNAFrameColourSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(590, 214);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "mRNAFrameColourSelector";
            Text = "Reading frame colour selection";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pPlusTwo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pPlusOne).EndInit();
            ((System.ComponentModel.ISupportInitialize)pPlusZero).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnCancel;
        private Button btnAccept;
        private GroupBox groupBox1;
        private PictureBox pPlusZero;
        private Label lblInstructions;
        private RadioButton rdoDialog;
        private RadioButton rdoList;
        private Label label6;
        private PictureBox pPlusTwo;
        private PictureBox pPlusOne;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnPlusTwo;
        private Button btnPlusOne;
        private Button btnPlusZero;
    }
}