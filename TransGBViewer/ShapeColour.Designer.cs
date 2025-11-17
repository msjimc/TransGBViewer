namespace TransGBViewer
{
    partial class ShapeColour
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShapeColour));
            btnCancel = new Button();
            btnAccept = new Button();
            groupBox1 = new GroupBox();
            btnBackground = new Button();
            p1Display = new PictureBox();
            lblInstructions = new Label();
            rdoDialogue = new RadioButton();
            rdoList = new RadioButton();
            label6 = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)p1Display).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(500, 120);
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
            btnAccept.Location = new Point(419, 120);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(75, 23);
            btnAccept.TabIndex = 6;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(btnBackground);
            groupBox1.Controls.Add(p1Display);
            groupBox1.Controls.Add(lblInstructions);
            groupBox1.Controls.Add(rdoDialogue);
            groupBox1.Controls.Add(rdoList);
            groupBox1.Controls.Add(label6);
            groupBox1.Location = new Point(14, 14);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(561, 100);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Colour selection";
            // 
            // btnBackground
            // 
            btnBackground.Location = new Point(474, 63);
            btnBackground.Name = "btnBackground";
            btnBackground.Size = new Size(81, 23);
            btnBackground.TabIndex = 57;
            btnBackground.Text = "Colour";
            btnBackground.UseVisualStyleBackColor = true;
            btnBackground.Click += btnBackground_Click;
            // 
            // p1Display
            // 
            p1Display.Location = new Point(445, 63);
            p1Display.Name = "p1Display";
            p1Display.Size = new Size(23, 23);
            p1Display.TabIndex = 56;
            p1Display.TabStop = false;
            // 
            // lblInstructions
            // 
            lblInstructions.AutoSize = true;
            lblInstructions.Location = new Point(6, 67);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(295, 15);
            lblInstructions.TabIndex = 55;
            lblInstructions.Text = "Set the shape's colour  by pressing the 'Colour' button.";
            // 
            // rdoDialogue
            // 
            rdoDialogue.AutoSize = true;
            rdoDialogue.Location = new Point(416, 15);
            rdoDialogue.Name = "rdoDialogue";
            rdoDialogue.Size = new Size(132, 19);
            rdoDialogue.TabIndex = 54;
            rdoDialogue.Text = "Colour dialogue box";
            rdoDialogue.UseVisualStyleBackColor = true;
            // 
            // rdoList
            // 
            rdoList.AutoSize = true;
            rdoList.Checked = true;
            rdoList.Location = new Point(281, 15);
            rdoList.Name = "rdoList";
            rdoList.Size = new Size(95, 19);
            rdoList.TabIndex = 53;
            rdoList.TabStop = true;
            rdoList.Text = "List of names";
            rdoList.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Location = new Point(6, 19);
            label6.Name = "label6";
            label6.Size = new Size(235, 37);
            label6.TabIndex = 52;
            label6.Text = "Select the colour using a dialogue box or a list of predefined names:";
            // 
            // ShapeColour
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(589, 157);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ShapeColour";
            Text = "Shape colour selection";
            Load += ShapeColour_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)p1Display).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnCancel;
        private Button btnAccept;
        private GroupBox groupBox1;
        private Button btnBackground;
        private PictureBox p1Display;
        private Label lblInstructions;
        private RadioButton rdoDialogue;
        private RadioButton rdoList;
        private Label label6;
    }
}