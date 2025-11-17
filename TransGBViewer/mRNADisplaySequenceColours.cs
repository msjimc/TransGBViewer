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
    public partial class mRNADisplaySequenceColours : Form
    {
        List<string> names;
        Dictionary<string, Color> codingColors;
        Dictionary<string, Color> nonCodingColors;
        bool changesMade = false;

        public mRNADisplaySequenceColours(List<string> Names, Dictionary<string, Color> CodingColours, Dictionary<string, Color> NonCodingColours)
        {
            InitializeComponent();
            names = Names;
            codingColors = CodingColours;
            nonCodingColors = NonCodingColours;
            foreach (string name in names)
            { clbSequenceNames.Items.Add(name); }
            btnColourSelection.Enabled = false;
            btnAccept.Enabled = false;
        }


        private void btnColourSelection_Click(object sender, EventArgs e)
        {
            Color selected = Color.Transparent;
            if (rdoCustomDialogBox.Checked == true)
            {
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.AllowFullOpen = true;
                colorDialog.FullOpen = true;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                { 
                    selected = colorDialog.Color;                     
                }               
            }
            else
            {
                AAColourSelectionListBox aacslb = new AAColourSelectionListBox(Color.Black, this.BackColor, false, "");
                aacslb.Text = "Select sequence colour";
                if (aacslb.ShowDialog() == DialogResult.OK)
                {
                    selected = aacslb.SelectedColour();
                }
            }

            if (selected != Color.Transparent)
            {
                if (chkCoding.Checked == true)
                {
                    foreach (var item in clbSequenceNames.CheckedItems)
                    {
                        string name = item.ToString();
                        if (codingColors.ContainsKey(name) == true)
                        { 
                            codingColors[name]= selected;
                            changesMade = true;
                        }
                    }
                }
                if (chkNonCoding.Checked == true)
                {
                    foreach (var item in clbSequenceNames.CheckedItems)
                    {
                        string name = item.ToString();
                        if (nonCodingColors.ContainsKey(name) == true)
                        { 
                            nonCodingColors[name] = selected;
                            changesMade = true;
                        }
                    }
                }
            }
            IsOK();
        }

        private void chkCoding_CheckedChanged(object sender, EventArgs e)
        {
            IsOK();
        }

        private void chkNonCoding_CheckedChanged(object sender, EventArgs e)
        {
            IsOK();
        }

         private void clbSequenceNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                IsOK();
            }));
        }
        private void IsOK()
        {
            int test = 0;
            if (chkCoding.Checked == true || chkNonCoding.Checked == true)
            { test = 1; }
            if (clbSequenceNames.CheckedItems.Count > 0)
            { test += 1; }
            if (test == 2)
            { btnColourSelection.Enabled = true; }
            else { btnColourSelection.Enabled = false; }

            btnAccept.Enabled = changesMade;
        }

        public Dictionary<string, Color> getCodingColours { get { return codingColors; } }
        public Dictionary<string, Color> getNonCodingColours { get { return nonCodingColors; } }
    }
}
