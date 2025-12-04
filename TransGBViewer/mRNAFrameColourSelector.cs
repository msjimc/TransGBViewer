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
    public partial class mRNAFrameColourSelector : Form
    {
        Color[] frameColours;
        public mRNAFrameColourSelector(Color[] FrameColours)
        {
            InitializeComponent();

            frameColours = FrameColours;

            if (frameColours == null || frameColours.Length != 3)
            { frameColours = new Color[] { Color.PaleGreen, Color.LightBlue, Color.LightPink }; } 

            SetPictureBoxColour(pPlusZero, frameColours[0]);
            SetPictureBoxColour(pPlusOne, frameColours[1]);
            SetPictureBoxColour(pPlusTwo, frameColours[2]);
        }

        private void ColourSelection(int colourIndex)
        {
            if (rdoDialog.Checked == true)
            {
                frameColours[colourIndex] = SetColour(frameColours[colourIndex]);
            }
            else
            {
                AAColourSelectionListBox aacslb = new AAColourSelectionListBox(frameColours[colourIndex], frameColours[colourIndex], false, "");
                aacslb.SetTitle(Text);
                if (aacslb.ShowDialog() == DialogResult.OK)
                {
                    frameColours[colourIndex] = aacslb.SelectedColour();
                }
            }
        }

        private Color SetColour(Color thisColour)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = thisColour;
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            { return colorDialog.Color; }
            else { return thisColour; }
        }

        private void SetPictureBoxColour(PictureBox pBox, Color frameColour)
        {
            Bitmap bmp = new Bitmap(23, 23, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(frameColour);
            pBox.Image = bmp;
        }

        private void btnPlusZero_Click(object sender, EventArgs e)
        {
            ColourSelection(0);
            SetPictureBoxColour(pPlusZero, frameColours[0]);
        }

        private void btnPlusOne_Click(object sender, EventArgs e)
        {
            ColourSelection(1);
            SetPictureBoxColour(pPlusOne, frameColours[1]);
        }

        private void btnPlusTwo_Click(object sender, EventArgs e)
        {
            ColourSelection(2);
            SetPictureBoxColour(pPlusTwo, frameColours[2]);
        }

        public Color[] GetFrameColours { get { return frameColours; } }
        
            
    }
}
