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
    public partial class GetInterProScan : Form
    {
        private mRNADisplayParameters mRNAParameters;
        private AlignDisplayOptionsParameters alignmentParameters;
        private MinimumRequiredParametersProtein parametersMinimum;

        private string interProScanResult = "";

        public GetInterProScan(mRNADisplayParameters Parameters)
        {
            InitializeComponent();
            mRNAParameters = Parameters;
            parametersMinimum = new MinimumRequiredParametersProtein();
            parametersMinimum.Domains = Parameters.Domains;
            parametersMinimum.AminoAcidSequences = Parameters.SequenceAminoAcid;
            btnAccept.Enabled = false;
        }

        public GetInterProScan(AlignDisplayOptionsParameters Parameters)
        {
            InitializeComponent();
            alignmentParameters = Parameters;
            parametersMinimum = new MinimumRequiredParametersProtein();
            parametersMinimum.Domains = Parameters.Domains;
            parametersMinimum.AminoAcidSequences = Parameters.Sequences;
            btnAccept.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnImportSearch_Click(object sender, EventArgs e)
        {
            string fileName = FileString.OpenAs("Select the save introProScan return string", "Text file (*.txt)|*.txt");
            if (System.IO.File.Exists(fileName) == false) { return; }

            StreamReader sf = null;
            try
            {
                sf = new StreamReader(fileName);
                string baseName = "";

                parametersMinimum.Domains = new Dictionary<string, List<AlignmentDomainFeature>>();
                while (sf.Peek() > 0)
                {
                    string line = sf.ReadLine();
                    int indexTab = line.IndexOf('\t');
                    int indexPipe = line.IndexOf('|');
                    if (indexTab == -1 & indexPipe == -1)
                    { baseName = line.Trim(); }
                    else if (indexTab > -1)
                    {
                        if (parametersMinimum.Domains.ContainsKey(baseName) == false)
                        {
                            List<AlignmentDomainFeature> list = new List<AlignmentDomainFeature>();
                            parametersMinimum.Domains[baseName] = list;
                        }
                        AlignmentDomainFeature adf = new AlignmentDomainFeature(line, baseName);
                        if (adf.IsOK == true) { parametersMinimum.Domains[baseName].Add(adf); }
                    }
                }
                RecalibrateAlignmentDomainFeatures();

                btnAccept.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show("An error occured: " + ex.Message, "Error"); }
            finally { sf?.Close(); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            AlignmentDomainSearch ads = new AlignmentDomainSearch(parametersMinimum.AminoAcidSequences);
            if (ads.ShowDialog() == DialogResult.OK)
            {
                interProScanResult = ads.ReturnedString;
                parametersMinimum.Domains = ads.Domains;
                if (parametersMinimum.Domains.Count > 0)
                {
                    btnSaveDomainString.Enabled = true;
                    btnAccept.Enabled = true;
                    RecalibrateAlignmentDomainFeatures();
                }
            }
        }

        private void RecalibrateAlignmentDomainFeatures()
        {
            foreach (string key in parametersMinimum.Domains.Keys)
            {
                List<AlignmentDomainFeature> list = parametersMinimum.Domains[key];
                string sequence = parametersMinimum.AminoAcidSequences[key];

                foreach (AlignmentDomainFeature adf in list)
                {
                    if (adf.Recalibrated == false)
                    {
                        int indexSequence = 0;
                        for (int indexAlignment = 0; indexAlignment < sequence.Length; indexAlignment++)
                        {
                            if (sequence[indexAlignment] != '-')
                            {
                                if (indexSequence == adf.Start)
                                { adf.StartAlignment = indexAlignment; }
                                if (indexSequence == adf.End)
                                { adf.EndAlignment = indexAlignment; break; }
                                indexSequence++;
                            }
                        }
                        adf.Recalibrated = true;
                    }
                }
            }
        }
        private void btnSaveDomainString_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(interProScanResult) == true) { return; }

            string fileName = FileString.SaveAs("Save introProScan's return string", "Text file (*.txt)|*.txt");
            if (fileName == "Cancel") { return; }

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(fileName);
                sw.WriteLine(interProScanResult);
                sw.Close();
            }
            catch (Exception ex)
            { MessageBox.Show("An error occured: " + ex.Message, "Error"); }
            finally { sw?.Close(); }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (mRNAParameters != null)
            { mRNAParameters.Domains = parametersMinimum.Domains; }
            else if (alignmentParameters != null)
            { alignmentParameters.Domains = parametersMinimum.Domains; }
        }
    }
}
