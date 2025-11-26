using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{

    public class mRNADisplayParameters
    {
        private string datatype = "";
        private List<string> sequenceNames = new List<string>();
        private Dictionary<string, string> sequenceDisplayNames = new Dictionary<string, string>();
        private Dictionary<string, string> sequenceGeneNames = new Dictionary<string, string>();
        private Dictionary<string, string> sequenceDefinations = new Dictionary<string, string>();
        private Dictionary<string, string> sequenceAminoAcid = new Dictionary<string, string>();
        private Dictionary<string, string> proteinIDs = new Dictionary<string, string>();
        private Dictionary<string, string> sequenceDNA = new Dictionary<string, string>();
        private Dictionary<string, Color> sequenceExonColour = new Dictionary<string, Color>(); 
        private Dictionary<string, Color> sequenceCDSColour = new Dictionary<string, Color>();
        private Dictionary<string, Point> cdss = new Dictionary<string, Point>();
        private Dictionary<string, List<Point>> exons = new Dictionary<string, List<Point>>();
        private Dictionary<string, List<mRNADisplayMiscFeature>> miscFeaturesAll = new Dictionary<string, List<mRNADisplayMiscFeature>>();
        private Dictionary<string, exonGraphNode> exonSet = new Dictionary<string, exonGraphNode>();
        private string sequenceBase = "";
        private Dictionary<string, List<mRNADisplayMiscFeature>> miscellaneousFeatureGBSet = new Dictionary<string, List<mRNADisplayMiscFeature>>();
        private Dictionary<string, List<string>> mRNAToProteinKey = new Dictionary<string, List<string>>();
        private Dictionary<string, mRNAUniProtAlignment> mRNAUniProtAlignmentLimits = new Dictionary<string, mRNAUniProtAlignment>();
        private Dictionary<string, ProteinDomainFeature> miscellaneousFeatureUniprotAll = new Dictionary<string, ProteinDomainFeature>();
        private Dictionary<string, List<ProteinDomainSubFeature>> miscellaneousFeatureUniprotSet = new Dictionary<string, List<ProteinDomainSubFeature>>();
        private string searchString = "";
               
        private Dictionary<string, List<AlignmentDomainFeature>> domains = new Dictionary<string, List<AlignmentDomainFeature>>();
        private Dictionary<string, AlignmentDomainFeature> selected = new Dictionary<string, AlignmentDomainFeature>();

        public mRNADisplayParameters()
        {
        }

        public List<mRNADisplayMiscFeature> GetMisc_features(string SequenceName)
        {
            if (miscFeaturesAll.ContainsKey(SequenceName) == true)
            { return miscFeaturesAll[SequenceName]; }
            else { return new List<mRNADisplayMiscFeature>(); }
        }

        public void AddMiscfeature(string name, mRNADisplayMiscFeature mRNAMF)
        {
            if (miscFeaturesAll.ContainsKey(name) == true)
            { miscFeaturesAll[name].Add(mRNAMF); }
            else 
            {
                miscFeaturesAll.Add(name, new List<mRNADisplayMiscFeature>());
                miscFeaturesAll[name].Add(mRNAMF);
            }
        }

        public void AddDefination(string name,string def)
        {
            if (sequenceDefinations.ContainsKey(name))
            { sequenceDefinations[name] = def; }
            else
            { sequenceDefinations.Add(name, def); }
        }

        public void AddSequenceName(string name)
        {
            if (sequenceNames.Contains(name) == false)
            {
                sequenceNames.Add(name);
                sequenceDisplayNames.Add(name, name);
                sequenceExonColour.Add(name, Color.Gray);
                sequenceCDSColour.Add(name, Color.Green);
            }
        }
        public void AddSequenceGeneName(string name, string GeneName)
        {
            if  (sequenceGeneNames.ContainsKey(name)== true)
            { sequenceGeneNames[name] = GeneName; }
            else { sequenceGeneNames.Add(name, GeneName); }
        }

        public void AddExons(string name, Point exon)
        {
            if (exons.ContainsKey(name) == true)
            { exons[name].Add(exon); }
            else
            { 
                exons.Add(name, new List<Point>()); 
                exons[name].Add(exon);
            }
        }

        public void AddCDS(string name, Point cds)
        {
            if (cdss.ContainsKey(name) == true)
            { cdss[name] = cds; }
            else
            { cdss.Add(name, cds); }
        }
        public void AddAminoAcid(string name, string sequence)
        {
            if (sequenceAminoAcid.ContainsKey(name) == true)
            { sequenceAminoAcid[name] = sequence; }
            else
            { sequenceAminoAcid.Add(name, sequence); }
        }

        public void AddDNASequence(string name, string sequence)
        {
            if (sequenceDNA.ContainsKey(name) == true)
            { sequenceDNA[name] = sequence; }
            else
            { sequenceDNA.Add(name, sequence); }
        }

        public Point GetSequenceCDS(string name)
        {
            if (cdss.ContainsKey(name) == true)
            { return cdss[name]; }
            else
            { return new Point(0, 0); }
        }

        public List<Point> GetSequenceExon(string name)
        {
            if (exons.ContainsKey(name) == true)
            { return exons[name]; }
            else
            { return new  List<Point>(); }
        }

        public int Transcriptlength(string name)
        {
            return exons[name][exons[name].Count - 1].Y;          
        }

        public void ResetDisplayNames()
        {
            sequenceDisplayNames.Clear();
            foreach (string name in sequenceNames)
            { sequenceDisplayNames.Add(name, name); }
        }

        public string getExonSequence(string name, Point exon)
        {
            string answer = sequenceDNA[name].Substring(exon.X - 1, exon.Y - exon.X + 1);
            return answer;
        }
        private void setZoomValues()
        {
            zoom.X = 1;
            zoom.Y = AllSequences.Length;
        }

        public string DataType { get { return datatype; } set { datatype = value; } }
        public Dictionary<string, List<mRNADisplayMiscFeature>> MiscFeaturesWhole { get { return miscFeaturesAll; } set { miscFeaturesAll = value; } }
        public List<string> SequenceNames { get { return sequenceNames; } set { sequenceNames = value; } }
        public Dictionary<string, string> SequenceDisplayNames { get { return sequenceDisplayNames; } set { sequenceDisplayNames = value; } }
        public Dictionary<string, string> GeneName { get { return sequenceGeneNames; } set { sequenceGeneNames = value; } }
        public Dictionary<string, string> SequenceDefinations { get { return sequenceDefinations; } set { sequenceDefinations = value; } }
        public Dictionary<string, string> SequenceAminoAcid { get { return sequenceAminoAcid; } set { sequenceAminoAcid = value; } }
        public Dictionary<string, string> ProteinIDs { get { return proteinIDs; } set { proteinIDs = value; } }
        public Dictionary<string, string> SequenceDNA { get { return sequenceDNA; } set { sequenceDNA = value; } }
        public Dictionary<string, Point> CDSs { get { return cdss; } set { cdss = value; } }
        public Dictionary<string, List<Point> > Exons { get { return exons; } set { exons = value; } }
        public Dictionary<string,Color> SequenceExonColour { get { return sequenceExonColour; } set { sequenceExonColour = value; } }
        public Dictionary<string, Color> SequenceCDSColour { get { return sequenceCDSColour; } set { sequenceCDSColour = value; } }
        public Dictionary<string, exonGraphNode> ExonSet { get { return exonSet; } set { exonSet = value; } }
        public string AllSequences { get { return sequenceBase; } set { sequenceBase = value.Trim(); setZoomValues(); } }      
        public Dictionary<string, List<mRNADisplayMiscFeature>> MiscellaneousFeatureGBSet { get { return miscellaneousFeatureGBSet; } set {  miscellaneousFeatureGBSet = value; } }
        public Dictionary<string,List<string>> MRNAToProteinKey { get { return mRNAToProteinKey; } set { mRNAToProteinKey = value; } }
        public Dictionary<string,ProteinDomainFeature> MiscellaneousFeatureUniprotAll { get { return miscellaneousFeatureUniprotAll; } set { miscellaneousFeatureUniprotAll = value; } }
        public Dictionary<string,List<ProteinDomainSubFeature>> MiscellaneousFeatureUniprotSet { get { return miscellaneousFeatureUniprotSet; } set { miscellaneousFeatureUniprotSet = value; } }
        public Dictionary<string, mRNAUniProtAlignment> MRNAUniProtAlignmentLimits { get { return mRNAUniProtAlignmentLimits; } set { mRNAUniProtAlignmentLimits = value; } }
        public string SearchString { get { return searchString; } set { searchString = value; } }
        public Dictionary<string, List<AlignmentDomainFeature>> Domains { get { return domains; } set { domains = value; } }
        public Dictionary<string, AlignmentDomainFeature> Selected { get { return selected; } set { selected = value; } }


        #region Variable parametes
        private int labelWidth = 140;
        private DrawLabels legendsLocation = DrawLabels.left;
        private Font idFont = new Font("arial", 10);
        private Font featureFont = new Font("arial", 10);
        private Size drawingArea;
        private bool drawExonBorders = true;
        private bool fillExons = true;  
        private Point zoom = new Point();
        private bool coodindatesStartAt1bp = false;
        private bool showORF = true;
        private bool rounded = true;
        private bool reduced = true;
        private int intronGap = 0;
        private string showExonLimits = "None";
        private string showORFLimits = "None";
        private float limitsLineWidth = 1.5f;
        private Color[] lineLimitColours = new Color[] { Color.Black, Color.Black, Color.Black, Color.Black };
        private SizeMarkerLocation sizeMarkerLocation = SizeMarkerLocation.None;
        private int majorTick = 100;
        private int minorTick = 20;
        private int angleOfRotation = 0;
        private bool showSuperscript = true;

         public int LabelWidth { get { return labelWidth; } set { labelWidth = value; } }
        public DrawLabels LegendsLocation { get { return legendsLocation; } set { legendsLocation = value; } }
        public Font IDFont { get { return idFont; } set { idFont = value; } }
        public Font FeatureFont { get { return featureFont; } set { featureFont = value; }  }
        public Size DrawingArea { get { return drawingArea; } set { drawingArea = value; } }
        public bool DrawExonBorders { get { return drawExonBorders; } set { drawExonBorders = value; } }
        public bool FillExons { get { return fillExons; } set { fillExons = value; } }
        public Point Zoom { get { return zoom; } set { zoom = value; } }
        public bool CoodindatesStartAt1bp { get { return coodindatesStartAt1bp; } set { coodindatesStartAt1bp = value; } }
        public bool ShowOrf { get { return showORF; } set { showORF = value; } }
        public bool Rounded { get { return rounded; } set { rounded = value; } }
        public bool Reduced { get { return reduced; } set { reduced = value; } }
        public int  IntronGap { get { return intronGap; } set { intronGap = value; } }
        public string ShowExonLimits { get { return showExonLimits; } set { showExonLimits = value; } }
        public string ShowORFLimits { get { return showORFLimits; } set { showORFLimits = value; } }
        public float LimitsLineWidth { get { return limitsLineWidth; } set { limitsLineWidth = value; } }
        public Color[] LineLimitColours { get { return lineLimitColours; } set { LineLimitColours = value; } }
        public SizeMarkerLocation SizeMarkerLocation { get { return sizeMarkerLocation; } set { sizeMarkerLocation = value; } }
        public int MajorTickMark { get { return majorTick; } set { majorTick = value; } }
        public int MinorTickMark { get { return minorTick; } set { minorTick = value; } }
        public int AngleOfrotation { get { return angleOfRotation; } set { angleOfRotation = value; } }
        public bool ShowSuperscript { get { return showSuperscript; } set { showSuperscript = value; } }
        #endregion

        #region markers
        private Dictionary<string, float> featureRows = new Dictionary<string, float>();
        private Dictionary<string, GeneFeatureMarker> geneFeatureMarkers = new Dictionary<string, GeneFeatureMarker>();
        public Dictionary<string, float> FeatureRows { get { return featureRows; } set { featureRows = value; } }
        public Dictionary<string, GeneFeatureMarker> GeneFeatureMarkers { get { return geneFeatureMarkers; } set { geneFeatureMarkers = value; } }
        #endregion
        #region Layout
        private List<ClassDrawingOrder> layout = new List<ClassDrawingOrder> { ClassDrawingOrder.Interval_markers, ClassDrawingOrder.Gene_sequences, ClassDrawingOrder.GenBank_features, ClassDrawingOrder.Uniprot_features, ClassDrawingOrder.InterProScan_features };
        public List<ClassDrawingOrder> Layout { get { return layout; } set { layout = value; } }
        #endregion

    }


}
