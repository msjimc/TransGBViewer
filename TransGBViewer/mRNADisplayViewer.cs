using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransGBViewer
{
    public partial class mRNADisplayViewer : Form
    {
        mRNADisplayOptions mRANDO = null;
        public mRNADisplayViewer(mRNADisplayOptions mRNADisplayOptions)
        {
            InitializeComponent();
            mRANDO = mRNADisplayOptions;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mRNADisplayViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            mRANDO.DisplayClosed();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (resizing == true)
            {
                resizing = false;
            }
            else
            {
                timer1.Enabled = false;
                pmRNAView.Image = mRANDO.DisplayResized(new Size(pmRNAView.Width, pmRNAView.Height));
            }
        }

        bool resizing = false;
        private void mRNADisplayViewer_Resize(object sender, EventArgs e)
        {
            resizing = true;
            timer1.Enabled = true;
        }

        private void pmRNAView_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
        }

        public Size GetDrawingArea()
        {
            return new Size(pmRNAView.Width, pmRNAView.Height);
        }

        public void AddImage(Bitmap bmp)
        {
            if (bmp != null)
            { pmRNAView.Image = bmp; }
        }

        private void mRNADisplayViewer_Load(object sender, EventArgs e)
        {

        }

        private void pmRNAView_MouseClick(object sender, MouseEventArgs e)
        {
            mRANDO.getMouseClickLocation(e.Location);
        }
    }
}
