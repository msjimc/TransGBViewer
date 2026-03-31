using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace TransGBViewer
{
    public partial class AAColourSelectionListBox : Form
    {
        private Color textColour;
        private Color backgroundColour;
        private bool changingTextColour = false;
        private string aa;
        private AutoCompleteStringCollection colourNameslist;

        public AAColourSelectionListBox(Color TextColour, Color BackgroundColour, bool ChangingTextColour, string AA)
        {
            InitializeComponent();
            textColour = TextColour;
            backgroundColour = BackgroundColour;
            changingTextColour = ChangingTextColour;
            aa = AA;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {

        }

        private void AAColourSelectionListBox_Load(object sender, EventArgs e)
        {
            List<Color> predefinedColors = Enum.GetValues(typeof(KnownColor))
                                        .Cast<KnownColor>()
                                        .Select(Color.FromKnownColor)
                                        .Where(c => !c.IsSystemColor)
                                        .ToList();
            var sorted = predefinedColors.OrderBy(c => c.Name).ToList(); 

            colourNameslist = new AutoCompleteStringCollection();
            foreach (var color in sorted)
            {
                cboColours.Items.Add(color.Name.ToString());
                colourNameslist.Add(color.Name.ToString());
            }

            int index=0;
            if (changingTextColour == true)
            {index = cboColours.Items.IndexOf(textColour.Name);}
            else
            {index = cboColours.Items.IndexOf(backgroundColour.Name);}
            if (index > -1)
            { cboColours.SelectedIndex = index; }                  

            txtSuggestion.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSuggestion.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSuggestion.AutoCompleteCustomSource = colourNameslist;
        }

        private void txtSuggestion_TextChanged(object sender, EventArgs e)
        {
            if (cboColours.Items.Contains(txtSuggestion.Text))
            {
                int index = cboColours.Items.IndexOf(txtSuggestion.Text);
                cboColours.SelectedIndex = index;
            }
            else
            {
                string name = txtSuggestion.Text.Trim().ToLower();
                for (int index = 0; index < cboColours.Items.Count; index++)
                {
                    if (name.Equals(cboColours.Items[index].ToString().ToLower()))
                    {
                        cboColours.SelectedIndex = index;
                        break;
                    }
                }
            }
        }

        public Color SelectedColour()
        {
            if (changingTextColour)
            { return textColour; }
            else
            { return backgroundColour; }
        }

        private void cboColours_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changingTextColour == true)
            { textColour = Color.FromName(cboColours.Text); }
            else
            { backgroundColour = Color.FromName(cboColours.Text); }           

            Bitmap bmp = new Bitmap(p1Example.Width, p1Example.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(backgroundColour);            
            Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            Size size = g.MeasureString(aa, f).ToSize();
            int x = (bmp.Width - size.Width) / 2;
            int y = (bmp.Height - size.Height) / 2;
            g.DrawString(aa,f,new SolidBrush(textColour), x, y);
            p1Example.Image = bmp;
            
        }

        public void SetTitle(string text)
        { Text = text; }
    }
}
