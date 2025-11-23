using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace TransGBViewer
{
    public partial class mRNAMiscellaneousFeatureSelection : Form
    {
        private Dictionary<string, List<mRNADisplayMiscFeature>> sets;
        private mRNADisplayParameters parametes;
        private Color selectedColour = Color.Black;

        public mRNAMiscellaneousFeatureSelection(Dictionary<string, List<mRNADisplayMiscFeature>> Sets, mRNADisplayParameters Parametes)
        {
            InitializeComponent();
            sets = Sets;
            parametes = Parametes;
            cboAccessionIDs.Items.Clear();
            cboAccessionIDs.Items.Add("Select");
            cboAccessionIDs.Items.AddRange(parametes.SequenceNames.ToArray());
            cboAccessionIDs.SelectedIndex = 0;
            SetpCurrentColour();
            setcboList();
            if (Sets.Count > 0) { btnRemove.Enabled = true; }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboAccessionIDs.SelectedIndex == 0) { return; }

            List<mRNADisplayMiscFeature> set = new List<mRNADisplayMiscFeature>();
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

            string name = cboAccessionIDs.Text;
            if (parametes.MiscFeaturesWhole.ContainsKey(name) == true)
            {
                foreach (mRNADisplayMiscFeature mf in parametes.MiscFeaturesWhole[name])
                {
                    if (descriptions.Contains(mf.DiplayName()) == true)
                    {
                        mf.Color = selectedColour;
                        set.Add(mf);
                    }
                }
            }

            string key = txtSetName.Text.Trim() + "#" + name;
            if (sets.ContainsKey(key) == true)
            { sets[key] = set; }
            else { sets.Add(key, set); }

            setcboList();
            UncheckAll();
            txtSetName.Clear();
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
        }

        private void cboAccessionIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAccessionIDs.SelectedIndex == 0) { return; }

            clbFeatures.Items.Clear();
            string name = cboAccessionIDs.Text;
            if (parametes.MiscFeaturesWhole.ContainsKey(name) == true)
            {
                foreach (mRNADisplayMiscFeature mf in parametes.MiscFeaturesWhole[name])
                { clbFeatures.Items.Add(mf.DiplayName()); }
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

        private void clbFeatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            setAddButtonStatus();
        }

        private void clbFeatures_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                setAddButtonStatus();
            }));
        }

        private void txtSetName_TextChanged(object sender, EventArgs e)
        {
            string name = txtSetName.Text.Trim();
            if (sets.ContainsKey(name + "#" + cboAccessionIDs.Text.Trim()) == true)
            { 
                btnAdd.Text = "Update";
            }
            else { btnAdd.Text = "Add"; }

            setAddButtonStatus();
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

        private void cboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboList.SelectedIndex < 1) { return; }
            btnRemove.Enabled = true;
        }

        public Dictionary<string, List<mRNADisplayMiscFeature>> GetSets { get { return sets; } }
    }
}
