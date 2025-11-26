using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TransGBViewer
{
    public partial class mRNADomainSearchUpdate : Form
    {
        private AccessionIDType accessionIDType;
        private bool quit = false;
        private DomianIdentifier mapper;
        private mRNADisplayParameters parameters;
        private Dictionary<string, List<string>> mRNAToProteinKey;
        private Dictionary<string, ProteinDomainFeature> mProteinDomainFeature;
        private string ReturnResultString = "";

        List<string> idList = new List<string>();
        public mRNADomainSearchUpdate(mRNADisplayParameters Parameters)
        {
            InitializeComponent();
            parameters = Parameters;
            idList = parameters.SequenceNames;
            accessionIDType = AccessionIDType.GenBankmRNA;
        }

        public mRNADomainSearchUpdate(List<string> IDList, AccessionIDType TheAccessionIDType)
        {
            InitializeComponent();
            idList = IDList;
            accessionIDType = TheAccessionIDType;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (mapper != null) { mapper.Quit = true; }
            Close();
        }

        private void FeatureFinderGenBankmRNA()
        {
            btnAccept.Enabled = false;
            try
            {
                AddStatusText("Searcing for: \r\n" + string.Join('\t', idList));

                mapper = new DomianIdentifier();
                Dictionary<string, string> ids = mapper.GetProteinIDsFrommRNAID(idList,parameters.ProteinIDs, this);
                if (ids.Count == 0)
                {
                    AddStatusText("No protein IDs found for the provided mRNA IDs.");
                    return;
                }

                List<string> npID = ids.Values.ToList();
                AddStatusText("Searching for domains at Uniprot");
                string jobId = mapper.SubmitMappingJob(npID);
                if (jobId == "") { return; }
                AddStatusText("Submitted job waiting for resuts of job: " + jobId);
                Dictionary<string, List<string>> uniProtId = mapper.GetMappingResults(jobId, this);
                if (uniProtId.Count == 0) { return; }

                mProteinDomainFeature = new Dictionary<string, ProteinDomainFeature>();
                setKeyDictionary(ids, uniProtId);

                List<string> queried = new List<string>();
                foreach (string value in uniProtId.Keys)
                {
                    if (queried.Contains(value) == false)
                    {
                        queried.Add(value);
                        string url = $"https://www.ebi.ac.uk/proteins/api/features?offset=0&size=100&accession={value}";
                        WebClient wClient = new WebClient();
                        AddStatusText("Submitting domain: " + value + " to EBI for data");
                        string results = wClient.DownloadString(url);
                        
                        ReturnResultString += value + "\n" + results + "\n";
                        
                        AddStatusText("Received response from EBI");
                        string sequence = DomianIdentifier.getSequenceFromEBIResult(results);
                        ProteinDomainFeature pdf = new ProteinDomainFeature(results, sequence, value);
                        if (mProteinDomainFeature.ContainsKey(value) == true)
                        { mProteinDomainFeature[value] = pdf; }
                        else { mProteinDomainFeature.Add(value, pdf); }
                    }
                }
                AddStatusText("Search completed\r\n\r\nPress Accept to use this data or Cancel to ignore it.");
                btnAccept.Enabled = true;
            }
            catch { AddStatusText("An error occured during the search, please try again later"); }
        }

        private void FeatureFinderGenBankProtein()
        {
            btnAccept.Enabled = false;
            try
            {                
                AddStatusText("Searcing for: \r\n" + string.Join('\t', idList));

                mapper = new DomianIdentifier();
                Dictionary<string, string> ids = new Dictionary<string, string>();
                foreach (string id in idList) {ids.Add(id, id); }
                
                List<string> npID = idList;
                AddStatusText("Searching for domains at Uniprot");
                string jobId = mapper.SubmitMappingJob(npID);
                if (jobId == "") { return; }
                AddStatusText("Submitted job waiting for resuts of job: " + jobId);
                Dictionary<string, List<string>> uniProtId = mapper.GetMappingResults(jobId, this);
                if (uniProtId.Count == 0) { return; }

                mProteinDomainFeature = new Dictionary<string, ProteinDomainFeature>();
                setKeyDictionary(ids, uniProtId);

                List<string> queried = new List<string>();
                foreach (string value in uniProtId.Keys)
                {
                    if (queried.Contains(value) == false)
                    {
                        queried.Add(value);
                        string url = $"https://www.ebi.ac.uk/proteins/api/features?offset=0&size=100&accession={value}";
                        WebClient wClient = new WebClient();
                        AddStatusText("Submitting domain: " + value + " to EBI for data");
                        string results = wClient.DownloadString(url);
                        
                        ReturnResultString += value + "\n" + results;
                        
                        AddStatusText("Received response from EBI");
                        string sequence = DomianIdentifier.getSequenceFromEBIResult(results);
                        ProteinDomainFeature pdf = new ProteinDomainFeature(results, sequence, value);
                        if (mProteinDomainFeature.ContainsKey(value) == true)
                        { mProteinDomainFeature[value] = pdf; }
                        else { mProteinDomainFeature.Add(value, pdf); }
                    }
                }
                AddStatusText("Search completed\r\n\r\nPress Accept to use this data or Cancel to ignore it.");
                btnAccept.Enabled = true;
            }
            catch { AddStatusText("An error occured during the search, please try again later"); }
        }

        private void FeatureFinderUniProtKB()
        {
            btnAccept.Enabled = false;
            try
            {             
                mProteinDomainFeature = new Dictionary<string, ProteinDomainFeature>();
                
                List<string> queried = new List<string>();
                foreach (string value in idList)
                {
                    if (queried.Contains(value) == false)
                    {
                        queried.Add(value);
                        string url = $"https://www.ebi.ac.uk/proteins/api/features?offset=0&size=100&accession={value}";
                        WebClient wClient = new WebClient();
                        AddStatusText("Submitting domain: " + value + " to EBI for data");
                        string results = wClient.DownloadString(url);
                        
                        ReturnResultString += value + "\n" + results;
                        
                        AddStatusText("Received response from EBI");
                        string sequence = DomianIdentifier.getSequenceFromEBIResult(results);
                        ProteinDomainFeature pdf = new ProteinDomainFeature(results, sequence, value);
                        if (mProteinDomainFeature.ContainsKey(value) == true)
                        { mProteinDomainFeature[value] = pdf; }
                        else { mProteinDomainFeature.Add(value, pdf); }
                    }
                }
                AddStatusText("Search completed\r\n\r\nPress Accept to use this data or Cancel to ignore it.");
                btnAccept.Enabled = true;
            }
            catch { AddStatusText("An error occured during the search, please try again later"); }
        }

        public string ReturnString { get {  return ReturnResultString; } } 
        public Dictionary<string, ProteinDomainFeature> GetmProteinDomainFeature { get { return mProteinDomainFeature; } }
        private void setKeyDictionary(Dictionary<string, string> ids, Dictionary<string, List<string>> unprotIds)
        {
            mRNAToProteinKey = new Dictionary<string, List<string>>();
            foreach (string mRNA in ids.Keys)
            {
                string protein = ids[mRNA];
                foreach (string domain in unprotIds.Keys)
                {
                    List<string> hits = unprotIds[domain];
                    if (hits.Contains(protein) == true)
                    {
                        if (mRNAToProteinKey.ContainsKey(mRNA) == false)
                        {
                            List<string> list = new List<string>();
                            list.Add(domain);
                            mRNAToProteinKey.Add(mRNA, list);
                        }
                        else { mRNAToProteinKey[mRNA].Add(domain); }
                    }
                }
            }
        }

        public Dictionary<string, List<string>> GetmRNAToProteinKey { get { return mRNAToProteinKey; } }
        public void AddStatusText(string message)
        {
            txtStatus.AppendText(message + "\r\n");
            txtStatus.SelectionStart = txtStatus.Text.Length;
            txtStatus.ScrollToCaret();
            Application.DoEvents();
        }
              
        private void mRNADomainSearchUpdate_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            
            switch (accessionIDType)
            {
                case AccessionIDType.GenBankmRNA:
                    FeatureFinderGenBankmRNA();
                    break;
                case AccessionIDType.GenBankProtein:
                    FeatureFinderGenBankProtein();
                    break;
                case AccessionIDType.UniProtKB:
                    FeatureFinderUniProtKB();
                    break;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {

        }
    }
}
