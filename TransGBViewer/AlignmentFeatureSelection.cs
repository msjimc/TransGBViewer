using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TransGBViewer
{
    public partial class AlignmentFeatureSelection : Form
    {        
        private MinimumRequiredParameters parameters;
        private Dictionary<string, AlignmentDomainFeature> selection = new Dictionary<string, AlignmentDomainFeature>();
        public AlignmentFeatureSelection(Dictionary<string, List< AlignmentDomainFeature>> Domains, Dictionary<string, AlignmentDomainFeature> Selection,bool limitToSequenceNames)
        {
            InitializeComponent();
            parameters = new MinimumRequiredParameters();

            Dictionary<string, AlignmentDomainFeature> SelectionLocal = new Dictionary<string, AlignmentDomainFeature>();
            foreach (string key in Selection.Keys)
            { SelectionLocal.Add(key, Selection[key]); }

            parameters.Domains = Domains;
            parameters.Selected = Selection;
            selection = Selection;
            setControls(limitToSequenceNames);
        }

        private void setControls(bool limitToSequenceNames)
        { 
            cboSequenceNames.Items.Add("Select");
            foreach (string key in parameters.Domains.Keys)
            { cboSequenceNames.Items.Add(key); }

            cboSequenceNames.SelectedIndex = 0;
            cboFeatureName.Enabled = false;

            cboAnalysisValue.SelectedIndex = 0;
            cboSignatureValue.SelectedIndex = 1;
            cboSignitureDescriptionValue.SelectedIndex = 0;
            cboInterProValue.SelectedIndex = 0;
            cboInterproDescriptionValue.SelectedIndex = 0;
            //if (limitToSequenceNames == false)
            //{ cboNameLocation.SelectedIndex = 0; }
            //else
            //{ 
            //    cboNameLocation.SelectedIndex = 3;
            //    cboNameLocation.Enabled = false;
            //}
        }

        private void cboSequenceNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawExampleDomain();

            cboFeatureName.Items.Clear();
            cboFeatureName.Enabled = false;
            string sequenceName = cboSequenceNames.Text;
            if (parameters.Domains.ContainsKey(sequenceName) == false)
            {
                cboFeatureName.Items.Clear();
                return; 
            }

            cboFeatureName.Items.Add("Select");
            foreach (AlignmentDomainFeature adf in parameters.Domains[sequenceName])
            { cboFeatureName.Items.Add(adf.ToString()); }

            cboFeatureName.SelectedIndex = 0;
            if (cboFeatureName.Items.Count > 1)
            { cboFeatureName.Enabled = true; }
        }

        private void cboFeatureName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFeatureName.SelectedIndex == 0) 
            {
                btnAdd.Enabled = false;
                DrawExampleDomain(); 
                return;
            }
            btnAdd.Enabled = true;
            AlignmentDomainFeature adf = parameters.Domains[cboSequenceNames.Text][cboFeatureName.SelectedIndex - 1];
            string displayName = adf.DisplayName;
            cboAnalysisValue.SelectedIndex = adf.NameSelectionOptions[0];
            cboSignatureValue.SelectedIndex = adf.NameSelectionOptions[1];
            cboSignitureDescriptionValue.SelectedIndex = adf.NameSelectionOptions[2];
            cboInterProValue.SelectedIndex = adf.NameSelectionOptions[3];
            cboInterproDescriptionValue.SelectedIndex = adf.NameSelectionOptions[4];
            chkDrawBorder.Checked = adf.DrawBorder;
            chkFillShape.Checked = adf.FillShape;
            chkRoundedDomains.Checked = adf.Rounded;
            //switch (adf.DomainNameLocation)
            //{
            //    case DomainNameLocation.none:
            //        cboNameLocation.SelectedIndex = 0;
            //        break;
            //    case DomainNameLocation.Above:
            //        cboNameLocation.SelectedIndex = 2;
            //        break;
            //    case DomainNameLocation.left:
            //        cboNameLocation.SelectedIndex = 3;
            //        break;
            //    case DomainNameLocation.inside:
            //        cboNameLocation.SelectedIndex = 1;
            //        break;
            //}
            //MakeDisplayName();
            SetButtons();
            adf.DisplayName = displayName;
            txtDisplayName.Text = displayName;
        }

        private void cboAnalysisValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeDisplayName();
        }

        private void cboSignatureValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeDisplayName();
        }

        private void cboSignitureDescriptionValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeDisplayName();
        }

        private void cboInterProValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeDisplayName();
        }

        private void cboInterproDescriptionValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeDisplayName();
        }

        private void MakeDisplayName()
        {
            string displayname = GetAlignmentDomainFeatureName();
            lblDescription.Text = "Description: " + displayname;
            txtDisplayName.Text = displayname;
            DrawExampleDomain();
        }

        private string GetAlignmentDomainFeatureName()
        {
            if (cboSequenceNames.SelectedIndex == 0 || cboFeatureName.SelectedIndex == 0) { return "-"; }
            AlignmentDomainFeature adf = parameters.Domains[cboSequenceNames.Text][cboFeatureName.SelectedIndex - 1];
            return GetAlignmentDomainFeatureName(adf);
        }
        private string GetAlignmentDomainFeatureName(AlignmentDomainFeature adf)
        {
            string description = "";
            for (int index = 1; index < 6; index++)
            {
                if (cboAnalysisValue.SelectedIndex == index) { description += ", " + adf.Analysis; }
                if (cboSignatureValue.SelectedIndex == index) { description += ", " + adf.Signature; }
                if (cboSignitureDescriptionValue.SelectedIndex == index) { description += ": " + adf.SignatureDescription; }
                if (cboInterProValue.SelectedIndex == index) { description += ", " + adf.InterProName; }
                if (cboInterproDescriptionValue.SelectedIndex == index) { description += ": " + adf.InterProDescription; }
            }
            if (description.Length > 2)
            { description = description.Substring(2); }
            else { description = "-"; }

            return description;
        }

        private void cboNameLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawExampleDomain();
        }

        private void chkRoundedDomains_CheckedChanged(object sender, EventArgs e)
        {
            DrawExampleDomain();
        }

        private void chkDrawBorder_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDrawBorder.Checked == false) { chkFillShape.Checked = true; }
            DrawExampleDomain();
        }

        private void chkFillShape_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFillShape.Checked == false) { chkDrawBorder.Checked = true; }
            DrawExampleDomain();
        }

        private void btnFillColour_Click(object sender, EventArgs e)
        {
            if (cboSequenceNames.SelectedIndex == 0 || cboFeatureName.SelectedIndex < 1) { return; }
            AlignmentDomainFeature adf = parameters.Domains[cboSequenceNames.Text][cboFeatureName.SelectedIndex - 1];

            ShapeColour sc = new ShapeColour(adf.FillColour);
            sc.SetText("Select the domains fill colour", "To select the domain's colour press the 'Colour' button");
            if (sc.ShowDialog() == DialogResult.OK)
            {
                adf.FillColour = sc.GetShapeColour;
                DrawExampleDomain();
            }
        }

        private void DrawExampleDomain()
        {
            Bitmap bmp = new Bitmap(pStyle.Width, pStyle.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            
            if (cboSequenceNames.SelectedIndex == 0 || cboFeatureName.SelectedIndex < 1) { pStyle.Image = bmp; return; }
            AlignmentDomainFeature adf = parameters.Domains[cboSequenceNames.Text][cboFeatureName.SelectedIndex - 1];

            setAlignmentDomainFeature(adf);
            scalar scale = new scalar(96.0f);

            int length = adf.End + 1 - adf.Start;
            int shapeWidth = length * 13;
            if (shapeWidth > pStyle.Width -30) { shapeWidth = pStyle.Width-30; }            
            int offset = (pStyle.Width - shapeWidth) / 2;
            RectangleF shape = new RectangleF(offset, 2, shapeWidth, 20);

            Font f = new Font("Arial", 10);
            SizeF size = g.MeasureString(adf.DisplayName, f);
            float textStart = (pStyle.Width - size.Width) / 2;

            shape.Y = (pStyle.Height - shape.Height) / 2; 
            if (adf.FillShape == true) { CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(adf.FillColour), adf.Rounded, scale, 4, false); }
            if (adf.DrawBorder == true)
            {
                Pen pen = new Pen(adf.BorderColour, scale.f[1]);
                CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, adf.Rounded, scale, 6);
            }

            pStyle.Image = bmp;
        }

        private void setAlignmentDomainFeature(AlignmentDomainFeature adf)
        {
            adf.DisplayName = GetAlignmentDomainFeatureName(adf);
            adf.Rounded = chkRoundedDomains.Checked;
            adf.FillShape = chkFillShape.Checked;
            adf.DrawBorder = chkDrawBorder.Checked;
            adf.DomainNameLocation = DomainNameLocation.left;
            getNameSeletionOptions(adf);
        }

        private void getNameSeletionOptions(AlignmentDomainFeature adf)
        {
            adf.NameSelectionOptions[0] = cboAnalysisValue.SelectedIndex;
            adf.NameSelectionOptions[1] = cboSignatureValue.SelectedIndex;
            adf.NameSelectionOptions[2] = cboSignitureDescriptionValue.SelectedIndex;
            adf.NameSelectionOptions[3] = cboInterProValue.SelectedIndex;
            adf.NameSelectionOptions[4] = cboInterproDescriptionValue.SelectedIndex;
        }

       private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboSequenceNames.SelectedIndex == 0 || cboFeatureName.SelectedIndex < 1) { return; }
            AlignmentDomainFeature adf = parameters.Domains[cboSequenceNames.Text][cboFeatureName.SelectedIndex - 1];
            adf.DisplayName = txtDisplayName.Text.Trim();
            string key = cboFeatureName.Text.Trim();

            if (selection.ContainsKey(key) == false)
            { selection.Add(key, adf.Copy()); }
            else { selection[key] = adf.Copy(); }
            SetButtons();
        }

        private void SetButtons()
        {
            string key = cboFeatureName.Text.Trim();
            btnRemove.Enabled = selection.ContainsKey(key);
            if (selection.ContainsKey(key) == true)
            { btnAdd.Text = "Update"; }
            else
            { btnAdd.Text = "Add"; }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cboSequenceNames.SelectedIndex == 0 || cboFeatureName.SelectedIndex < 1) { return; }
            string key = cboFeatureName.Text.Trim();
            if (selection.ContainsKey(key) == true)
            { selection.Remove(key); }
            SetButtons();
        }

        public Dictionary<string, AlignmentDomainFeature> SelectedDomains { get { return selection; } }
    }
}

