using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;

namespace TransGBViewer
{
    public partial class Rename : Form
    {
        private Dictionary<string, string> labels;
        private List<string> toDraw;
        private bool useExonCDS = true;
        public Rename(Dictionary<string, string> Labels)
        {
            InitializeComponent();
            labels = Labels;
            int count = 1;

            List<string> added = new List<string>();

            cboTranscript.Items.Add("select");
            foreach (string label in labels.Keys)
            {
                cboTranscript.Items.Add(label);

                string partKey = getNameFromLabelKey(label);
                if (added.Contains(partKey) == false)
                {
                    lvAlphabet.Items.Add(count.ToString());
                    lvAlphabet.Items[count - 1].SubItems.Add(partKey);

                    string name1;
                    string name2;

                    if (labels.ContainsKey(partKey + " - exon"))
                    { name1 = labels[partKey + " - exon"]; }
                    else
                    { name1 = label; }

                    if (labels.ContainsKey(partKey + " - CDS"))
                    { name2 = labels[partKey + " - CDS"]; }
                    else
                    { name2 = label; }

                    if (name1.Equals(name2) == true)
                    { lvAlphabet.Items[count - 1].SubItems.Add(name1); }
                    else
                    { lvAlphabet.Items[count - 1].SubItems.Add(name1 + ", " + name2); }
                    added.Add(partKey);
                    count++;
                }
            }

            cboTranscript.SelectedIndex = 0;
            lvAlphabet.FullRowSelect = true;
            lvAlphabet.GridLines = true;
            lvUser.FullRowSelect = true;
            lvUser.GridLines = true;

        }

        public Rename(Dictionary<string, string> Labels, bool UseExonCDS)
        {
            InitializeComponent();
            useExonCDS = UseExonCDS;
            labels = Labels;
            int count = 1;

            List<string> added = new List<string>();

            cboTranscript.Items.Add("select");
            foreach (string label in labels.Keys)
            {
                cboTranscript.Items.Add(label);

                lvAlphabet.Items.Add(count.ToString());
                lvAlphabet.Items[count - 1].SubItems.Add(label);
                lvAlphabet.Items[count - 1].SubItems.Add(labels[label]);

                added.Add(label);
                count++;

            }

            cboTranscript.SelectedIndex = 0;
            lvAlphabet.FullRowSelect = true;
            lvAlphabet.GridLines = true;
            lvUser.FullRowSelect = true;
            lvUser.GridLines = true;

        }

        private string getNameFromLabelKey(string key)
        {
            int index = key.IndexOf('-') - 1;
            if (index < 1)
            { return key; }
            else
            {
                string answer = key.Substring(0, key.IndexOf('-') - 1);
                return answer;
            }
        }

        private void PopulateAlphabetview()
        {
            AdjustDisplayNames(lvAlphabet);
            AdjustDisplayNames(lvUser);
        }

        private void AdjustDisplayNames(ListView lv)
        {
            if (useExonCDS == true)
            {
                foreach (ListViewItem item in lv.Items)
                {
                    string name1;
                    string name2;
                    string partKey = item.SubItems[1].Text.ToString();

                    if (labels.ContainsKey(partKey + " - exon"))
                    { name1 = labels[partKey + " - exon"]; }
                    else
                    { name1 = partKey; }

                    if (labels.ContainsKey(partKey + " - CDS"))
                    { name2 = labels[partKey + " - CDS"]; }
                    else
                    { name2 = partKey; }

                    if (name1.Equals(name2) == true)
                    { item.SubItems[2].Text = name1; }
                    else
                    { item.SubItems[2].Text = name1 + ", " + name2; }
                }
            }
            else
            {
                foreach (ListViewItem item in lv.Items)
                {
                    string name = item.SubItems[1].Text;
                    if (labels.ContainsKey(name) == true)
                    { item.SubItems[2].Text = labels[name]; }
                }
            }
        }

        private void Rename_Load(object sender, EventArgs e)
        {

        }

        private void cboTranscript_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTranscript.SelectedIndex == 0)
            {
                txtNewName.Text = string.Empty;
                txtNewName.Enabled = false;
                return;
            }
            else
            {
                txtNewName.Enabled = true;
                txtNewName.Text = labels[cboTranscript.SelectedItem.ToString()];
            }
        }

        private void txtNewName_TextChanged(object sender, EventArgs e)
        {
            if (cboTranscript.SelectedIndex == 0) { return; }
            labels[cboTranscript.SelectedItem.ToString()] = txtNewName.Text.Trim();

            if (txtNewName.Text.Trim().Length > 0)
            { labels[cboTranscript.SelectedItem.ToString()] = txtNewName.Text.Trim(); }
            else
            {
                string oldName = getNameFromLabelKey(cboTranscript.Text);// cboTranscript.Text.Substring(0, cboTranscript.Text.LastIndexOf("-") - 1);
                labels[cboTranscript.SelectedItem.ToString()] = oldName;
            }
            PopulateAlphabetview();
        }

        public Dictionary<string, string> GetUpdatedLabels()
        {
            return labels;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lvAlphabet.SelectedItems.Count == 0) { return; }
            List<ListViewItem> selected = new List<ListViewItem>();
            for (int index = 0; index < lvAlphabet.SelectedItems.Count; index++)
            {
                ListViewItem item = lvAlphabet.SelectedItems[index];
                selected.Add(item);
                //lvAlphabet.Items.Remove(item);
                ListViewItem itemNew = new ListViewItem(item.Text.ToString());
                itemNew.SubItems.Add(item.SubItems[1]);
                itemNew.SubItems.Add(item.SubItems[2]);
                lvUser.Items.Add(itemNew);
            }

            foreach(ListViewItem item in selected)
            {lvAlphabet.Items.Remove(item); }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvUser.SelectedItems.Count == 0) { return; }
            ListViewItem item = lvUser.SelectedItems[0];
            lvUser.Items.Remove(item);
            ListViewItem itemNew = new ListViewItem(item.Text.ToString());
            itemNew.SubItems.Add(item.SubItems[1]);
            itemNew.SubItems.Add(item.SubItems[2]);
            lvAlphabet.Items.Add(itemNew);
        }

        private void Rename_FormClosed(object sender, FormClosedEventArgs e)
        {
            toDraw = new List<string>();
            foreach (ListViewItem item in lvUser.Items)
            {
                string name = item.SubItems[1].Text;
                toDraw.Add(name);
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {

        }

        private void lvAlphabet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAlphabet.SelectedItems.Count == 0) { return; }
            ListViewItem item = lvAlphabet.SelectedItems[0];

            string name = item.SubItems[1].Text;
            int index = cboTranscript.FindStringExact(name);
            if (index > -1)
            { cboTranscript.SelectedIndex = index; }
        }

        private void lvUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvUser.SelectedItems.Count == 0) { return; }
            ListViewItem item = lvUser.SelectedItems[0];

            string name = item.SubItems[1].Text;
            int index = cboTranscript.FindStringExact(name);
            if (index > -1)
            { cboTranscript.SelectedIndex = index; }
        }

        public List<string> getUserSelection { get { return toDraw; } }
    }
}
