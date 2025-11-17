namespace TransGBViewer
{
    partial class mRNADisplayViewer
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mRNADisplayViewer));
            groupBox1 = new GroupBox();
            pmRNAView = new PictureBox();
            btnClose = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pmRNAView).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(pmRNAView);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(776, 403);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "mRNA sequences";
            // 
            // pmRNAView
            // 
            pmRNAView.Dock = DockStyle.Fill;
            pmRNAView.Location = new Point(3, 19);
            pmRNAView.Name = "pmRNAView";
            pmRNAView.Size = new Size(770, 381);
            pmRNAView.TabIndex = 0;
            pmRNAView.TabStop = false;
            pmRNAView.MouseClick += pmRNAView_MouseClick;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.Location = new Point(12, 421);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // mRNADisplayViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnClose);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "mRNADisplayViewer";
            Text = "Sequence Display";
            FormClosed += mRNADisplayViewer_FormClosed;
            Load += mRNADisplayViewer_Load;
            Resize += mRNADisplayViewer_Resize;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pmRNAView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private PictureBox pmRNAView;
        private Button btnClose;
        private System.Windows.Forms.Timer timer1;
    }
}