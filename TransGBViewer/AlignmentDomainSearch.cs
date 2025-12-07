using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace TransGBViewer
{
    public partial class AlignmentDomainSearch : Form
    {
        private string interProScanString = "";
        private string thisSequence = "";
        Dictionary<string, string> sequences = null;
        Dictionary<string, List<AlignmentDomainFeature>> domains = new Dictionary<string, List<AlignmentDomainFeature>>();
        public AlignmentDomainSearch(Dictionary<string, string> Sequences)
        {
            InitializeComponent();
            sequences = Sequences;
            cboNames.Items.Add("Select");
            foreach (string key in sequences.Keys)
            { cboNames.Items.Add(key); }
            cboNames.SelectedIndex = 0;
            btnAccept.Enabled = false;
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                btnSubmit.Enabled = false;

                AddStatusText(" ");
                thisSequence = cboNames.Text.Trim();
                var client = new HttpClient();
                var url = "https://www.ebi.ac.uk/Tools/services/rest/iprscan5/run";

                string emailAddress = txtEmailAddress.Text.Trim();
                string jobID = txtJobTitle.Text.Trim();
                string sequence = ">" + thisSequence + "\n" + sequences[cboNames.Text.Trim()];
                sequence = sequence.Replace("-", "");

                var parameters = new Dictionary<string, string>
        {
            { "email", emailAddress }, // Required
            { "title", jobID },
            { "sequence", sequence },
            { "goterms", "false" }, // Optional: include GO terms
            { "pathways", "false" } // Optional: include pathway info
        };

                AddStatusText($"Starting search with : {thisSequence}");

                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync(url, content);
                var jobId = await response.Content.ReadAsStringAsync();

                AddStatusText($"Job submitted. ID: {jobId}");

                // Poll for status
                string statusUrl = $"https://www.ebi.ac.uk/Tools/services/rest/iprscan5/status/{jobId}";
                string resultUrl = $"https://www.ebi.ac.uk/Tools/services/rest/iprscan5/result/{jobId}/tsv";

                while (true)
                {
                    var status = await client.GetStringAsync(statusUrl);
                    AddStatusText($"Status: {status}");
                    if (status == "FINISHED") break;
                    AddStatusText("Waiting for 5 seconds before requesting results again.");
                    await Task.Delay(5000);
                }

                // Get results
                string result = await client.GetStringAsync(resultUrl);
                interProScanString += thisSequence + "\n" + result + '\n';
                AddStatusText("Processing returned data");
                CreateAlignmentDomainFeatures(result);
                AddStatusText($"Recieved data on: {domains[thisSequence].Count.ToString()} new domains.");
                AddStatusText(thisSequence + ":");
                foreach (AlignmentDomainFeature adf in domains[thisSequence])
                { AddStatusText(adf.Signature + ": " + adf.SignatureDescription + ", " + adf.InterProName + ": " + adf.InterProDescription); }

            }
            catch { AddStatusText("An error occured getting domain data"); }

            btnSubmit.Enabled = true;

            if (domains.Count > 0) { btnAccept.Enabled = true; } else { btnAccept.Enabled = false; }
        }

        private void CreateAlignmentDomainFeatures(string returnedresults)
        {
            if (string.IsNullOrEmpty(returnedresults)) { return; }

            string[] hits = returnedresults.Split('\n');
            foreach (string line in hits)
            {
                AlignmentDomainFeature adf = new AlignmentDomainFeature(line, thisSequence);
                if (adf.IsOK == true)
                {
                   if (domains.ContainsKey(thisSequence) == true)
                    {
                        bool add = true;
                        foreach (AlignmentDomainFeature present in domains[thisSequence])
                        {
                            if (present.ToString() == adf.ToString())
                            { add = false; break; }
                        }
                        if (add == true) { domains[thisSequence].Add(adf); }                     
                    }
                   else
                    {
                        List<AlignmentDomainFeature> list = new List<AlignmentDomainFeature>();
                        list.Add(adf);
                        domains.Add(thisSequence, list);
                    }
                }
            }
        }

        public Dictionary<string, List<AlignmentDomainFeature>> Domains { get { return domains; } }
        public string ReturnedString { get { return interProScanString; } }
        public void AddStatusText(string message)
        {
            txtStatus.AppendText(message + "\r\n");
            txtStatus.SelectionStart = txtStatus.Text.Length;
            txtStatus.ScrollToCaret();
            Application.DoEvents();
        }

        private void setSubmitButton()
        {
            bool enable = true;
            if (cboNames.SelectedIndex == 0) { enable = false; }
            if (enable == true && txtJobTitle.Text.Trim().Length < 3) { enable = false; }
            if (enable == true)
            {
                string email = txtEmailAddress.Text.Trim();
                try
                {
                    var addr = new MailAddress(email);
                    enable = addr.Address == email;
                }
                catch
                { enable = false; }
            }
            btnSubmit.Enabled = enable;
        }

        private void cboNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            setSubmitButton();
        }

        private void txtEmailAddress_TextChanged(object sender, EventArgs e)
        {
            setSubmitButton();
        }

        private void txtJobTitle_TextChanged(object sender, EventArgs e)
        {
            setSubmitButton();
        }
    }
}
