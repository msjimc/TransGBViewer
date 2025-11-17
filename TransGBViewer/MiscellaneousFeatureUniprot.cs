using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransGBViewer
{

    public partial class MiscellaneousFeatureUniprot : Form
    {
        private Dictionary<string, List<ProteinDomainSubFeature>> sets;
        private MinimumRequiredParameters parameters;

        public MiscellaneousFeatureUniprot(Dictionary<string, List<ProteinDomainSubFeature>> Sets, mRNADisplayParameters Parameters)
        {
            InitializeComponent();
            sets = Sets;
            parameters = new MinimumRequiredParameters(); 
            parameters.SequenceNames = Parameters.SequenceNames;
            parameters.MRNAToProteinKey = Parameters.MRNAToProteinKey;
            parameters.MiscellaneousFeatureUniprotAll = Parameters.MiscellaneousFeatureUniprotAll;
            parameters.MRNAUniProtAlignmentLimits = Parameters.MRNAUniProtAlignmentLimits;
            parameters.CDSs = Parameters.CDSs;


            cboAccessIDs.Items.Add("Select");
            foreach (string name in parameters.SequenceNames)
            { cboAccessIDs.Items.Add(name); }
            cboAccessIDs.SelectedIndex = 0;
            setcboList();
            SetpCurrentColour();
            if (sets.Count> 0) { btnRemove.Enabled = true; }
        }

        public MiscellaneousFeatureUniprot(Dictionary<string, List<ProteinDomainSubFeature>> Sets, AlignDisplayOptionsParameters Parameters)
        {
            InitializeComponent();
            sets = Sets;
            parameters = new MinimumRequiredParameters();
            parameters.SequenceNames = Parameters.SequenceNames;
            parameters.MiscellaneousFeatureUniprotAll = Parameters.ProteinDomains;
            parameters.MRNAToProteinKey = Parameters.MRNAToProteinKey;                           
            parameters.MRNAUniProtAlignmentLimits = Parameters.MRNAUniProtAlignmentLimits;       
            parameters.CDSs = Parameters.CDSs;                                                   

            cboAccessIDs.Items.Add("Select");
            foreach (string name in parameters.MRNAToProteinKey.Keys)
            { cboAccessIDs.Items.Add(name); }
            cboAccessIDs.SelectedIndex = 0;
            setcboList();
            SetpCurrentColour();
            if (sets.Count > 0) { btnRemove.Enabled = true; }
        }


        private void SetcboUniProtAccessionIDs()
        {
            cboUniProtAccessIDs.Items.Clear();
            cboUniProtAccessIDs.Items.Clear();

            cboFeatureType.Items.Clear();
            clbFeatures.Items.Clear();

            cboUniProtAccessIDs.Items.Add("Select");
            if (cboAccessIDs.SelectedIndex > 0)
            {
                string name = cboAccessIDs.Text;
                if (parameters.MRNAToProteinKey.ContainsKey(name) == true)
                {
                    cboUniProtAccessIDs.Items.AddRange(parameters.MRNAToProteinKey[name].ToArray());
                }
                else
                { cboUniProtAccessIDs.SelectedIndex = 0; }
            }
            cboUniProtAccessIDs.SelectedIndex = 0;
        }

        private void MiscellaneousFeatureUniprot_Load(object sender, EventArgs e)
        {

        }

        private void cboAccessIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetcboUniProtAccessionIDs();
        }

        private void cboUniProtAccessIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            clbFeatures.Items.Clear();

            if (cboUniProtAccessIDs.SelectedIndex == 0) { return; }
            string uniprotID = cboUniProtAccessIDs.Text;
            cboFeatureType.Items.Clear();
            cboFeatureType.Items.Add("Select");
            if (parameters.MiscellaneousFeatureUniprotAll.ContainsKey(uniprotID) == true)
            {
                ProteinDomainFeature pdf = parameters.MiscellaneousFeatureUniprotAll[uniprotID];
                foreach (string key in pdf.SubDomains.Keys)
                {
                    cboFeatureType.Items.Add(key);
                }
            }
            cboFeatureType.SelectedIndex = 0;
        }

        private void cboFeatureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            clbFeatures.Items.Clear();
            if (cboFeatureType.SelectedIndex == 0) { return; }
            string uniprotID = cboUniProtAccessIDs.Text;
            string featureType = cboFeatureType.Text;

            mRNAUniProtAlignment limits = GetUniProtLimits(cboAccessIDs.Text, uniprotID);
            if (limits.StartOfmRNAAA == -1) { return; }

            if (parameters.MiscellaneousFeatureUniprotAll.ContainsKey(uniprotID) == true)
            {
                ProteinDomainFeature pdf = parameters.MiscellaneousFeatureUniprotAll[uniprotID];
                if (pdf.SubDomains.ContainsKey(featureType) == true)
                {
                    foreach (ProteinDomainSubFeature pdsf in pdf.SubDomains[featureType])
                    {
                        if (pdsf.StartPointAA >= limits.StartOfUniprot && pdsf.EndPointAA <= limits.EndOfUniprot)
                        { clbFeatures.Items.Add(pdsf.Description); }
                    }
                }
            }
        }

        private mRNAUniProtAlignment GetUniProtLimits(string name, string uniProtName)
        {
            string key = name + "#" + uniProtName;
            mRNAUniProtAlignment limits;
            if (parameters.MRNAUniProtAlignmentLimits.ContainsKey(key) == true && parameters.CDSs.ContainsKey(name) == true)
            {
                limits = parameters.MRNAUniProtAlignmentLimits[key];
                Point orf = parameters.CDSs[name];
                mRNAUniProtAlignment limitsDNA;
                limitsDNA.StartOfmRNAAA = ((limits.StartOfmRNAAA - 1) * 3) + orf.X;
                limitsDNA.EndOfmRNAA = ((limits.EndOfmRNAA - 1) * 3) + orf.X + 2;
                limitsDNA.StartOfUniprot = limits.StartOfUniprot;
                limitsDNA.EndOfUniprot = limits.EndOfUniprot;
                return limitsDNA;
            }
            else
            {
                mRNAUniProtAlignment limitsDNA;
                limitsDNA.StartOfmRNAAA = -1;
                limitsDNA.EndOfmRNAA = -1;
                limitsDNA.StartOfUniprot = -1;
                limitsDNA.EndOfUniprot = -1;
                return limitsDNA;
            }
        }
        private void txtSearchTerm_TextChanged(object sender, EventArgs e)
        {
            string text = txtSearchTerm.Text.ToLower();
            if (clbFeatures.Items.Count == 0 || text.Length < 3)
            {
                lblSearchHitcount.Text = "Number of hits: 0";
                btnCheck.Enabled = false;
                return;
            }

            int counts = 0;
            foreach (var items in clbFeatures.Items)
            {
                if (items.ToString().ToLower().Contains(text) == true) { counts++; }
            }
            lblSearchHitcount.Text = "Number of hits: " + counts.ToString();
            if (counts > 0) { btnCheck.Enabled = true; }
            else { btnCheck.Enabled = false; }

        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string text = txtSearchTerm.Text.ToLower();
            if (clbFeatures.Items.Count == 0 || text.Length < 3)
            {
                btnCheck.Enabled = false;
                return;
            }

            for (int index = 0; index < clbFeatures.Items.Count; index++)
            {
                if (clbFeatures.Items[index].ToString().ToLower().Contains(text) == true)
                { clbFeatures.SetItemChecked(index, true); }
            }
            setAddButtonStatus();
        }

        private void setAddButtonStatus()
        {
            if (clbFeatures.CheckedItems.Count > 0 && txtSetName.Text.Trim().Length > 2)
            { btnAdd.Enabled = true; }
            else { btnAdd.Enabled = false; }
        }

        private void btnUncheck_Click(object sender, EventArgs e)
        {
            UncheckAll();
        }

        private void UncheckAll()
        {
            for (int index = 0; index < clbFeatures.Items.Count; index++)
            { clbFeatures.SetItemChecked(index, false); }
        }

        private Color selectedColour = Color.Black;
        private void btnColourSelection_Click(object sender, EventArgs e)
        {
            if (rdoCustomDialogBox.Checked == true)
            {
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.AllowFullOpen = true;
                colorDialog.FullOpen = true;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedColour = colorDialog.Color;
                    SetpCurrentColour();
                }
            }
            else
            {
                AAColourSelectionListBox aacslb = new AAColourSelectionListBox(selectedColour, selectedColour, false, "");
                aacslb.Text = "Select sequence colour";
                if (aacslb.ShowDialog() == DialogResult.OK)
                {
                    selectedColour = aacslb.SelectedColour();
                    SetpCurrentColour();
                }
            }
        }

        private void SetpCurrentColour()
        {
            Bitmap bitmap = new Bitmap(pCurrentColour.Width, pCurrentColour.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(selectedColour);
            pCurrentColour.Image = bitmap;
        }

        private void txtSetName_TextChanged(object sender, EventArgs e)
        {
            string name = txtSetName.Text.Trim();
            if (sets.ContainsKey(name + "#" + cboUniProtAccessIDs.Text.Trim() + "#" + cboAccessIDs.Text.Trim()) == true)
            {
                btnAdd.Text = "Update";
            }
            else { btnAdd.Text = "Add"; }

            setAddButtonStatus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboAccessIDs.SelectedIndex == 0 || cboUniProtAccessIDs.SelectedIndex == 0) { return; }

            List<ProteinDomainSubFeature> set = new List<ProteinDomainSubFeature>();
            List<string> descriptions = new List<string>();
            foreach (var item in clbFeatures.CheckedItems)
            {
                descriptions.Add(item.ToString());
            }

            if (descriptions.Count == 0)
            {
                MessageBox.Show("No features were selected", "error");
                return;
            }

            string name = cboUniProtAccessIDs.Text;
            if (parameters.MiscellaneousFeatureUniprotAll.ContainsKey(name) == true)
            {
                ProteinDomainFeature pdf = parameters.MiscellaneousFeatureUniprotAll[name];
                string typeOfFeature = cboFeatureType.Text;
                if (pdf.SubDomains.ContainsKey(typeOfFeature) == true)
                {
                    foreach (ProteinDomainSubFeature pdsf in pdf.SubDomains[typeOfFeature])
                    {
                        if (descriptions.Contains(pdsf.Description) == true)
                        {
                            pdsf.Colour = selectedColour;
                            set.Add(pdsf);
                        }
                    }
                }
            }

            string key = txtSetName.Text.Trim() + "#" + name + "#" + cboAccessIDs.Text;
            if (sets.ContainsKey(key) == true)
            { sets[key] = set; }
            else { sets.Add(key, set); }

            setcboList();
            UncheckAll();
            txtSetName.Clear();
            if (sets.Count > 0) { btnRemove.Enabled = true; }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cboList.SelectedIndex == 0) { return; }

            string key = cboList.Text;
            if (sets.ContainsKey(key) == true)
            {
                sets.Remove(key);
                setcboList();
            }
            if (sets.Count == 0) { btnRemove.Enabled = false; }
        }

        private void setcboList()
        {
            string current = cboList.Text;
            cboList.Items.Clear();
            cboList.Items.Add("Select");
            foreach (string key in sets.Keys)
            {
                cboList.Items.Add(key);
            }
            if (cboList.Items.Contains(current) == true)
            { cboList.Text = current; }
            else { cboList.SelectedIndex = 0; }

            if (cboList.Items.Count > 1) { btnAccept.Enabled = true; }
        }

        private void clbFeatures_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            setAddButtonStatus();
        }

        public Dictionary<string, List<ProteinDomainSubFeature>> GetSets { get { return sets; } }
    }
}
