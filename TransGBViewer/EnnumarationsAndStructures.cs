using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace TransGBViewer
{
    public enum DomainNameLocation
    {
        Above,
        inside,
        none,
        left
    }
    public enum ClassDrawingOrder
    {
        Gene_sequences,
        Interval_markers,
        Miscellaneous_elements,
        Sequence_variants,
        GenBank_features,
        Uniprot_features,
        InterProScan_features,
        GenBank_protein_features
    }
    public enum DisplayOrder
    {
        Overlay,
        ExonsThenCDS,
        CDSThenExons,
        AllExonsThenAllCDS,
        AllCDSThenAllExons,
        OnlyExons,
        OnlyCDS,
    }

    public enum VerticalAlignment
    {
        Condensed,
        IndividualLines
    }

    public enum FeatureLinks
    {
        None,
        Line,
        TrianglesAbove,
        TrianglesBelow,
        ArcsAbove,
        ArcsBelow,
    }

    public enum geneStructure
    {
        Exon,
        Arrow,
        Block
    }

    public enum DrawLabels
    {
        none,
        left,
        right,
        leftNoDraw,
        rightNoDraw,
        rightJustified,
        leftJustified,
        above,
        below
    };

    public enum SizeMarkerLocation
    {
        None,
        Top,
        Bottom,
        AboveFeature
    };

    public enum ScaleType
    {
        million,
        halfMillion,
        twoHundredFiftyThousand,
        hundredThousand,
        fiftyThousand,
        twentyFiveThousand,
        tenThousand,
        fiveThousand,
        twoThousand,
        thousand
    };

    public enum ShapeType
    {
        NotSet,
        ArrowDown,
        ArrowLeft,
        ArrowRight,
        ArrowUp,
        Box,
        Circle,
        CrossDiagonal,
        CrossVertical,
        Diamond,
        Hexagon,
        Octagon,
        Pentagon,
        RectangleHorizontal,
        RectangleVertical,
        Square,
        Star,
        TriangleUp,
        TriangleDown,
        TriangleLeft,
        TriangleRight,
        UnderLine,
        VerticalLine
    }

    public enum ShapeLocation
    {
        None,
        Above,
        Below,
        SpecificSequence
    }

    public enum VariantRSIDFiltering
    {
        All,
        OnlyRSIDs,
        NoRSIDs,
        Named
    }

    public enum AccessionIDType
    {
        GenBankProtein,
        GenBankmRNA,
        UniProtKB
    }

    internal class EnnumarationsAndStructures
    {

    }

    public struct GenBankProteinFeature
    {
        public Point Location;
        public Color Colour;
    }
    public struct itemsInlistView
    {
        public int index;
        public string feature;
        public String line;

        public itemsInlistView(int Index, string Feature, string Line)
        {
            line = Line;
            index = Index;
            feature = Feature;
        }
    }

    public struct mRNAUniProtAlignment
    {
        public int StartOfmRNAAA;
        public int StartOfUniprot;
        public int EndOfmRNAA;
        public int EndOfUniprot;
    }

    public class exonGraphNode
    {
        public int index;
        public List<int> next;
        public List<int> previous;
        public int count;
        public Point region;
        public string seqence;
        public int order = -1;
        public bool hidden = false;

        public exonGraphNode(int Index, Point Region, string Seqence)
        {
            index = Index;
            region = Region;
            seqence = Seqence;
            next = new List<int>();
            previous = new List<int>();
            count = 1;
        }

        public exonGraphNode Clone()
        {
            exonGraphNode egn = new exonGraphNode(index, region, seqence);
            egn.next = new List<int>(next);
            egn.previous = new List<int>(previous);
            egn.count = count;
            egn.order = order;
            egn.hidden = false;
            return egn;
        }

    }
 
    public class scalar
    {
        public Dictionary<int, float> f;
        public Dictionary<int, int> i;
        public int DPI { get; set; }
        public float scale { get;set; }
        public scalar(float dpi)
        {
            scale = dpi / 96;
            DPI = (int)dpi;
            f = new Dictionary<int, float>(200);
            i= new Dictionary<int, int>(200);
            for (float index = 0; index < 201; index++)
            {
                f.Add((int)index, (float)(index * dpi) / 96.0f);
                i.Add((int)index, (int)((index * dpi) / 96.0f));
            }
        }       
    }

    class MinimumRequiredParameters
    {
        private List<string> sequenceNames;
        private Dictionary<string, List<string>> mRNAToProteinKey = new Dictionary<string, List<string>>();
        private Dictionary<string, ProteinDomainFeature> miscellaneousFeatureUniprotAll = new Dictionary<string, ProteinDomainFeature>();
        private Dictionary<string, mRNAUniProtAlignment> mRNAUniProtAlignmentLimits = new Dictionary<string, mRNAUniProtAlignment>();
        private Dictionary<string, Point> cdss = new Dictionary<string, Point>();

        private Dictionary<string, List<AlignmentDomainFeature>> domains = new Dictionary<string, List<AlignmentDomainFeature>>();
        private Dictionary<string, AlignmentDomainFeature> selected = new Dictionary<string, AlignmentDomainFeature>();
        public List<string> SequenceNames { get { return sequenceNames; } set { sequenceNames = value; } }
        public Dictionary<string, List<string>> MRNAToProteinKey { get { return mRNAToProteinKey; } set { mRNAToProteinKey = value; } }
        public Dictionary<string, ProteinDomainFeature> MiscellaneousFeatureUniprotAll { get { return miscellaneousFeatureUniprotAll; } set { miscellaneousFeatureUniprotAll = value; } }
        public Dictionary<string, mRNAUniProtAlignment> MRNAUniProtAlignmentLimits { get { return mRNAUniProtAlignmentLimits; } set { mRNAUniProtAlignmentLimits = value; } }
        public Dictionary<string, Point> CDSs { get { return cdss; } set { cdss = value; } }

        public Dictionary<string, List<AlignmentDomainFeature>> Domains { get { return domains; } set { domains = value; } }
        public Dictionary<string, AlignmentDomainFeature> Selected { get { return selected; } set { selected = value; } }
    }

    class MinimumRequiredParametersProtein
    {
        public MinimumRequiredParametersProtein() { }
        private Dictionary<string, string> aminoAcidSequences = new Dictionary<string, string>();
        private Dictionary<string, List<AlignmentDomainFeature>> domains = new Dictionary<string, List<AlignmentDomainFeature>>();

        public Dictionary<string, string> AminoAcidSequences { get { return aminoAcidSequences; } set { aminoAcidSequences = value; } }
        public Dictionary<string, List<AlignmentDomainFeature>> Domains { get { return domains; } set { domains = value; } }
    }

}
