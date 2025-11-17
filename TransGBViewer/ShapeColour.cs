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
    public partial class ShapeColour : Form
    {
        private Color shapeColour;
        public ShapeColour(Color ShapeColour)
        {
            InitializeComponent();

            shapeColour = ShapeColour;
            SetPictureBoxColour();
        }

        private void ColourSelection()
        {
            if (rdoDialogue.Checked == true)
            {
                shapeColour = SetColour(shapeColour);
                SetPictureBoxColour();
            }
            else
            {
                AAColourSelectionListBox aacslb = new AAColourSelectionListBox(shapeColour, shapeColour, false, "");
                aacslb.SetTitle(Text);
                if (aacslb.ShowDialog() == DialogResult.OK)
                {
                    shapeColour = aacslb.SelectedColour();
                    SetPictureBoxColour();
                }
            }
        }

        private void SetPictureBoxColour()
        {
            Bitmap bmp = new Bitmap(23, 23, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(shapeColour);
            p1Display.Image = bmp;
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

        public Color GetShapeColour { get { return shapeColour; } }
        private void btnBackground_Click(object sender, EventArgs e)
        {
            ColourSelection();
        }

        public void SetText(string title, string label)
        {
            this.Text = title;
            lblInstructions.Text = label;
        }

        private void ShapeColour_Load(object sender, EventArgs e)
        {

        }
    }
}
