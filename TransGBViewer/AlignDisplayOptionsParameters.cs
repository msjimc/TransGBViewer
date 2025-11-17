using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public enum LocationOfTexts
    {
        none,
        top,
        bottom
    }
    public class AlignDisplayOptionsParameters
    {
        private Dictionary<string, string> sequences = new Dictionary<string, string>();
        private Dictionary<string, Point> cdss = new Dictionary<string, Point>();
        private List<string> sequenceNames = new List<string>();
        private List<string> orginalSequenceNames = new List<string>();
        private Dictionary<string, string> displayNames = new Dictionary<string, string>();
        private Dictionary<string, Color> aaColours = new Dictionary<string, Color>();
        private Dictionary<string, Color> aaForeColours = new Dictionary<string, Color>();
        private Dictionary<string, Color> baseClassColours = new Dictionary<string, Color>();
        private Dictionary<string, Color> baseClassForeColours = new Dictionary<string, Color>();
        private int conservedResidue = 100;
        private int conservedClass = 100;
        private string consensusStrict = string.Empty;
        private string consensusType = string.Empty;
        private List<Color> consensusClassColours = new List<Color>();
        private Font fontAA = new System.Drawing.Font("Consolas", 9, FontStyle.Regular);
        private Font fontName = new System.Drawing.Font("Aptos Narrow", 9, FontStyle.Regular);
        private int labelWidth = 100;
        private LocationOfTexts locationOfAACoordinates = LocationOfTexts.none;
        private LocationOfTexts locationOfConsensusAA = LocationOfTexts.none;
        private LocationOfTexts locationOfConsensusClass = LocationOfTexts.none;
        private int markersAbove = 0;
        private int markersBelow = 0;
        private int markersOther = 0;
        //private int markersTemp = 0;
        private int aAPositionIncrement = 10;
        private int longestSequenceLength = 0;

        private Dictionary<string, string> AAClassSets = new Dictionary<string, string>();
        private Size imageArea = new Size(800, 600);

        private Dictionary<string, Shape> shapeMarkers = new Dictionary<string, Shape>();

        private Dictionary<string, List<AlignmentDomainFeature>> domains = new Dictionary<string, List<AlignmentDomainFeature>>();
        private Dictionary<string, AlignmentDomainFeature> selected = new Dictionary<string, AlignmentDomainFeature>();

        private Dictionary<string, List<string>> mRNAToProteinKey = new Dictionary<string, List<string>>();
        private Dictionary<string, ProteinDomainFeature> proteinDomains = new Dictionary<string, ProteinDomainFeature>();
        private Dictionary<string, List<ProteinDomainSubFeature>> selectedProteinDomains = new Dictionary<string, List<ProteinDomainSubFeature>>();
        private Dictionary<string, mRNAUniProtAlignment> mRNAUniProtAlignmentLimits = new Dictionary<string, mRNAUniProtAlignment>();
        private string searchString = string.Empty;

        private Dictionary<string, int> nameFeatureLink = new Dictionary<string, int>();
        private Dictionary<string, ProteinDataGenBank> proteinDataGenBanks = new Dictionary<string, ProteinDataGenBank>();
        private Dictionary<string, mRNAUniProtAlignment> alignedAAGenBankLimits = new Dictionary<string, mRNAUniProtAlignment>();
        private Dictionary<string, string> alignedAAToGenBank = new Dictionary<string, string>();
        private Dictionary<string, List<GenbankProteinFeature>> genbankProteinFeatureSets = new Dictionary<string, List<GenbankProteinFeature>>();
        public AlignDisplayOptionsParameters(Dictionary<string, string> Sequences, List<string> SequenceNames)
        {
            orginalSequenceNames = new List<string>();
            sequenceNames = SequenceNames;
            sequences = Sequences;
            foreach (string key in sequences.Keys)
            {
                Point p = new Point(1, sequences[key].Length);
                if (cdss.ContainsKey(key) == false)
                { cdss.Add(key, p); }
                else { cdss[key] = p; }
            }

            int max = 0;
            foreach (string name in sequenceNames)
            { orginalSequenceNames.Add(name); }

            foreach (string sequence in sequences.Values)
            {
                if (max < sequence.Length) { max = sequence.Length; }
            }
            longestSequenceLength = max;
            AAClassSets.Add("Hydrophobic", "AILMFWYV");
            AAClassSets.Add("Polar", "STNQC");
            AAClassSets.Add("Acidic", "DE");
            AAClassSets.Add("Basic", "KRH");
            AAClassSets.Add("Glycine", "G");
            AAClassSets.Add("Proline", "P");

            ResetDisplayNames();
            ResetAAColours();
            ResetBaseClassColours();
        }

        public void ResetSequenceOrder()
        {
            sequenceNames = new List<string>();
            foreach (string name in orginalSequenceNames)
            {
                if (sequences.ContainsKey(name) == true)
                { sequenceNames.Add(name); }
            }
        }

        public void ResetDisplayNames()
        {
            displayNames = new Dictionary<string, string>();
            foreach (string name in sequenceNames)
            {
                if (displayNames.ContainsKey(name) == false)
                { displayNames.Add(name, name); }
            }
        }

        public void ResetAAClasses()
        {
            AAClassSets = new Dictionary<string, string>();
            AAClassSets.Add("Hydrophobic", "AILMFWYV");
            AAClassSets.Add("Polar", "STNQC");
            AAClassSets.Add("Acidic", "DE");
            AAClassSets.Add("Basic", "KRH");
            AAClassSets.Add("Glycine", "g");
            AAClassSets.Add("Proline", "p");
        }
        public void ResetAAColours()
        {
            aaColours.Clear();
            aaForeColours.Clear();
            string aa = "ACDEFGHIKLMNPQRSTVWY";
            foreach (char AA in aa)
            {
                switch (AA)
                {
                    case 'D':
                    case 'E':
                        aaColours.Add(AA.ToString(), Color.Red);//Bright Red
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    case 'C':
                    case 'M':
                        aaColours.Add(AA.ToString(), Color.Yellow);//Yellow
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    case 'K':
                    case 'R':
                        aaColours.Add(AA.ToString(), Color.Blue);//Blue
                        aaForeColours.Add(AA.ToString(), Color.White);
                        break;
                    case 'S':
                    case 'T':
                        aaColours.Add(AA.ToString(), Color.Orange);//Orange
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    case 'F':
                    case 'Y':
                        aaColours.Add(AA.ToString(), Color.FromArgb(39, 106, 179));//Mid Blue
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    case 'N':
                    case 'Q':
                        aaColours.Add(AA.ToString(), Color.Cyan);//Cyan
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    case 'G':
                        aaColours.Add(AA.ToString(), Color.LightGray); // RGBA;//Light Grey
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    case 'L':
                    case 'V':
                    case 'I':
                        aaColours.Add(AA.ToString(), Color.Green);//Green
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    case 'A':
                        aaColours.Add(AA.ToString(), Color.DarkGreen);//Dark Grey
                        aaForeColours.Add(AA.ToString(), Color.White);
                        break;
                    case 'W':
                        aaColours.Add(AA.ToString(), Color.Purple);//Purple
                        aaForeColours.Add(AA.ToString(), Color.White);
                        break;
                    case 'H':
                        aaColours.Add(AA.ToString(), Color.LightBlue);//Pale Blue
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    case 'P':
                        aaColours.Add(AA.ToString(), Color.FromArgb(255, 203, 164));//Flesh
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                    default:
                        aaColours.Add(AA.ToString(), Color.White);//Tan
                        aaForeColours.Add(AA.ToString(), Color.Black);
                        break;
                }
            }
        }

        public void ResetBaseClassColours()
        {
            baseClassColours = new Dictionary<string, Color>();
            baseClassColours.Add("Hydrophobic", Color.PaleGreen);
            baseClassColours.Add("Polar", Color.FromArgb(255, 205, 155));
            baseClassColours.Add("Acidic", Color.Red);
            baseClassColours.Add("Basic", Color.LightBlue);
            baseClassColours.Add("Proline", Color.FromArgb(255, 203, 164));
            baseClassColours.Add("Glycine", Color.LightGray);
            baseClassColours.Add("None", Color.White);
            baseClassForeColours = new Dictionary<string, Color>();
            baseClassForeColours.Add("Hydrophobic", Color.Black);
            baseClassForeColours.Add("Polar", Color.Black);
            baseClassForeColours.Add("Acidic", Color.Black);
            baseClassForeColours.Add("Basic", Color.Black);
            baseClassForeColours.Add("Proline", Color.Black);
            baseClassForeColours.Add("Glycine", Color.Black);
            baseClassForeColours.Add("None", Color.Black);
        }

        public Color IsAAGroup(Dictionary<char, int> AAS)
        {
            Dictionary<string, int> classCount = new Dictionary<string, int>();

            foreach (char AA in AAS.Keys)
            {
                string AAClass = GetAAClass(AA.ToString());
                if (classCount.ContainsKey(AAClass) == false)
                { classCount.Add(AAClass, AAS[AA]); }
                classCount[AAClass] += AAS[AA];
            }
            float cutOffAA = (float)(conservedClass * sequenceNames.Count) / 100;

            foreach (string AAClass in classCount.Keys)
            {
                if (classCount[AAClass] >= cutOffAA && AAClass != "None")
                { return baseClassColours[AAClass]; }
            }
            return Color.White;
        }

        public Color ClassAAColour(Dictionary<char, int> AAs)
        {
            return ClassAAColour(AAs, 1.0f);
        }

        public Color ClassAAColour(Dictionary<char, int> AAs, float proprotion)
        {
            int h1 = 0;
            int p1 = 0;
            int a1 = 0;
            int b1 = 0;
            int g1 = 0;
            int p2 = 0;
            float cutoff = proprotion;

            foreach (char AA1 in AAs.Keys)
            {
                if (AAClassSets["Hydrophobic"].Contains(AA1))
                { h1 += AAs[AA1]; }
                else if (AAClassSets["Polar"].Contains(AA1))
                { p1 += AAs[AA1]; }
                else if (AAClassSets["Acidic"].Contains(AA1))
                { a1 += AAs[AA1]; }
                else if (AAClassSets["Basic"].Contains(AA1))
                { b1 += AAs[AA1]; }
                else if (AAClassSets["Glycine"].Contains(AA1))
                { g1 += AAs[AA1]; }
                else if (AAClassSets["Proline"].Contains(AA1))
                { p2 += AAs[AA1]; }
            }

            if (h1 >= cutoff)
            { return baseClassColours["Hydrophobic"]; } // All hydrophobic
            else if (p1 >= cutoff)
            { return BaseClassColours["Polar"]; } // All polar
            else if (a1 >= cutoff)
            { return BaseClassColours["Acidic"]; }// All acidic
            else if (b1 >= cutoff)
            { return BaseClassColours["Basic"]; } // All basic           
            else if (p2 >= cutoff)
            { return BaseClassColours["Proline"]; }// All proline
            else if (g1 >= cutoff)
            { return BaseClassColours["Glycine"]; } // All glycine           
            else
            { return Color.White; }// Unknown or mixed types (not all same class
        }

        public SolidBrush GetFontColour(char AA)
        {
            Color ColourCodeAA = Color.White;
            if (aaForeColours.ContainsKey(AA.ToString()) == true)
            { ColourCodeAA = aaForeColours[AA.ToString()]; }

            SolidBrush CodeAA = new SolidBrush(ColourCodeAA);
            return CodeAA;
        }

        public SolidBrush GetBrushColour(char AA)
        {
            Color ColourCodeAA = Color.White;
            if (aaColours.ContainsKey(AA.ToString()) == true)
            { ColourCodeAA = aaColours[AA.ToString()]; }
            SolidBrush CodeAA = new SolidBrush(ColourCodeAA);
            return CodeAA;
        }

        public bool AreAASameClass(char AA1, char AA2)
        {
            if (AA1 == AA2)
            { return true; }
            else if (AAClassSets["Hydrophobic"].Contains(AA1) && AAClassSets["Hydrophobic"].Contains(AA2))
            { return true; }
            else if (AAClassSets["Polar"].Contains(AA1) && AAClassSets["Polar"].Contains(AA2))
            { return true; }
            else if (AAClassSets["Acidic"].Contains(AA1) && AAClassSets["Acidic"].Contains(AA2))
            { return true; }
            else if (AAClassSets["Basic"].Contains(AA1) && AAClassSets["Basic"].Contains(AA2))
            { return true; }
            else if (AAClassSets["Glycine"].Contains(AA1) && AAClassSets["Glycine"].Contains(AA2))
            { return true; }
            else if (AAClassSets["Proline"].Contains(AA1) && AAClassSets["Proline"].Contains(AA2))
            { return true; }
            else { return false; }
        }

        public string GetAAClass(string AA)
        {

            if (AAClassSets["Hydrophobic"].Contains(AA)) return "Hydrophobic";
            else if (AAClassSets["Polar"].Contains(AA)) return "Polar";
            else if (AAClassSets["Acidic"].Contains(AA)) return "Acidic";
            else if (AAClassSets["Basic"].Contains(AA)) return "Basic";
            else if (AAClassSets["Glycine"].Contains(AA)) return "Glycine";
            else if (AAClassSets["Proline"].Contains(AA)) return "Proline";
            else return "None";
        }

        public static string ThreeAAToOneAA(string[] input)
        {
            string answer = "";
            string oneLetter = "";
            foreach (string threeLetter in input)
            {
                switch (threeLetter)
                {
                    case "ALA":
                        oneLetter = "A";
                        break;
                    case "ARG":
                        oneLetter = "R";
                        break;
                    case "ASN":
                        oneLetter = "N";
                        break;
                    case "ASP":
                        oneLetter = "D";
                        break;
                    case "CYS":
                        oneLetter = "C";
                        break;
                    case "GLU":
                        oneLetter = "E";
                        break;
                    case "GLN":
                        oneLetter = "Q";
                        break;
                    case "GLY":
                        oneLetter = "G";
                        break;
                    case "HIS":
                        oneLetter = "H";
                        break;
                    case "ILE":
                        oneLetter = "I";
                        break;
                    case "LEU":
                        oneLetter = "L";
                        break;
                    case "LYS":
                        oneLetter = "K";
                        break;
                    case "MET":
                        oneLetter = "M";
                        break;
                    case "PHE":
                        oneLetter = "F";
                        break;
                    case "PRO":
                        oneLetter = "P";
                        break;
                    case "SER":
                        oneLetter = "S";
                        break;
                    case "THR":
                        oneLetter = "T";
                        break;
                    case "TRP":
                        oneLetter = "W";
                        break;
                    case "TYR":
                        oneLetter = "Y";
                        break;
                    case "VAL":
                        oneLetter = "V";
                        break;
                    default:
                        oneLetter = "-";
                        break;
                }

                answer += oneLetter;
            }
            return answer;
        }
        public Dictionary<string, string> Sequences { get { return sequences; } set { sequences = value; } }
        public Dictionary<string, Point> CDSs { get { return cdss; } set { cdss = value; } }
        public List<string> SequenceNames { get { return sequenceNames; } set { sequenceNames = value; } }
        public Dictionary<string, string> DisplayNames { get { return displayNames; } set { displayNames = value; } }
        public string ConsensusStrict { get { return consensusStrict; } set { consensusStrict = value; } }
        public string ConsensusType { get { return consensusType; } set { consensusType = value; } }
        public Dictionary<string, Color> AAColours { get { return aaColours; } set { aaColours = value; } }
        public Dictionary<string, Color> AAForeColours { get { return aaForeColours; } set { aaForeColours = value; } }
        public Dictionary<string, Color> BaseClassColours { get { return baseClassColours; } set { baseClassColours = value; } }
        public Dictionary<string, Color> BaseClassForeColours { get { return baseClassForeColours; } set { baseClassForeColours = value; } }
        public List<Color> ConsensusClassColours { get { return consensusClassColours; } set { consensusClassColours = value; } }
        public int ConservedClass { get { return conservedClass; } set { conservedClass = value; } }
        public int ConservedResidue { get { return conservedResidue; } set { conservedResidue = value; } }
        public Size ImageArea { get { return imageArea; } set { imageArea = value; } }
        public int LongestSequenceLength { get { return longestSequenceLength; } set { longestSequenceLength = value; } }
        public LocationOfTexts LocationOfAACoordinates { get { return locationOfAACoordinates; } set { locationOfAACoordinates = value; } }
        public int MarkersAbove { get { return markersAbove; } set { markersAbove = value; } }
        public int MarksBelow { get { return markersBelow; } set { markersBelow = value; } }
        public int MarkersOther { get { return markersOther; } set { markersOther = value; } }
        public Font FontAA { get { return fontAA; } set { fontAA = value; } }
        public Font FontName { get { return fontName; } set { fontName = value; } }
        public int LabelWidth { get { return labelWidth; } set { labelWidth = value; } }
        public int AAPositionIncrement { get { return aAPositionIncrement; } set { aAPositionIncrement = value; } }
        public LocationOfTexts LocationOfConsensusAA { get { return locationOfConsensusAA; } set { locationOfConsensusAA = value; } }
        public LocationOfTexts LocationOfConsensusClass { get { return locationOfConsensusClass; } set { locationOfConsensusClass = value; } }
        public Dictionary<string, string> GetAAClassSets { get { return AAClassSets; } set { AAClassSets = value; } }
        public Dictionary<string, Shape> ShapeMarkers { get { return shapeMarkers; } set { shapeMarkers = value; } }
        public Dictionary<string, List<AlignmentDomainFeature>> Domains { get { return domains; } set { domains = value; } }
        public Dictionary<string, AlignmentDomainFeature> Selected { get { return selected; } set { selected = value; } }

        public Dictionary<string, List<string>> MRNAToProteinKey { get { return mRNAToProteinKey; } set { mRNAToProteinKey = value; } }
        public Dictionary<string, ProteinDomainFeature> ProteinDomains { get { return proteinDomains; } set { proteinDomains = value; } }
        public Dictionary<string, List<ProteinDomainSubFeature>> SelectedProteinDomains { get { return selectedProteinDomains; } set { selectedProteinDomains = value; } }
        public Dictionary<string, mRNAUniProtAlignment> MRNAUniProtAlignmentLimits { get { return mRNAUniProtAlignmentLimits; } set { mRNAUniProtAlignmentLimits = value; } }
        public string SearchString { get { return searchString; } set { searchString = value; } }
        public Dictionary<string, int> NameFeatureLink { get { return nameFeatureLink; } set { nameFeatureLink = value; } }
        public Dictionary<string, ProteinDataGenBank> ProteinDataGenBanks { get { return proteinDataGenBanks; } set { proteinDataGenBanks = value; } }
        public Dictionary<string, mRNAUniProtAlignment> AlignedAAGenBankLimits { get { return alignedAAGenBankLimits; }set { alignedAAGenBankLimits = value; } }
        public Dictionary<string, string> AlignedAAToGenBank { get { return alignedAAToGenBank; } set { alignedAAToGenBank = value; } }
        public Dictionary<string, List<GenbankProteinFeature>> GenbankProteinFeatureSets { get { return genbankProteinFeatureSets; } set { genbankProteinFeatureSets = value; } }

        public void ResetSequencenamesFont()
        { fontName = new System.Drawing.Font("Aptos Narrow", 9, FontStyle.Regular); }

        public void MarkerGap(int value)
        {
            markersAbove = 0;
            markersBelow = 0;

            foreach (Shape s in shapeMarkers.Values)
            {
                if (s.ShapeLocation == ShapeLocation.Above)
                { markersAbove = 1; }
                else if (s.ShapeLocation == ShapeLocation.Below)
                { markersBelow = 1; }
                else { markersOther = 1; }
            }

            if (value == 1)
            { markersAbove = 1; }
            else if (value == -1)
            { markersBelow = 1; }

        }

        public void Save(string fileName)
        {
            System.IO.StreamWriter fw = null;

            try
            {
                fw = new StreamWriter(fileName);

                fw.WriteLine("aaColours");
                WriteData(aaColours, fw);

                fw.WriteLine("aaForeColours");
                WriteData(aaForeColours, fw);

                fw.WriteLine("baseClassColours");
                WriteData(baseClassColours, fw);

                fw.WriteLine("baseClassForeColours");
                WriteData(baseClassForeColours, fw);

                fw.WriteLine("AAClassSets");
                WriteData(AAClassSets, fw);

                fw.WriteLine("fontName");
                WriteFont(fontName, fw);

                fw.WriteLine("pairs");
                fw.WriteLine("conservedResidue=" + conservedResidue.ToString());
                fw.WriteLine("conservedClass=" + conservedClass.ToString());
                fw.WriteLine("labelWidth=" + labelWidth.ToString());
                fw.WriteLine("locationOfAACoordinates=" + locationOfAACoordinates.ToString());
                fw.WriteLine("locationOfConsensusAA=" + locationOfConsensusAA.ToString());
                fw.WriteLine("locationOfConsensusClass=" + locationOfConsensusClass.ToString());

            }
            catch (Exception ex) { }
            finally { fw?.Close(); }

        }

        public void Read(string filename)
        {
            System.IO.StreamReader fs = null;

            try
            {
                fs = new StreamReader(filename);
                while (fs.Peek() > 0)
                {
                    string line = fs.ReadLine();
                    switch (line.Trim())
                    {
                        case "end":

                            break;
                        case "aaColours":
                            ReadData(aaColours, fs);
                            break;
                        case "aaForeColours":
                            ReadData(aaForeColours, fs);
                            break;
                        case "baseClassColours":
                            ReadData(baseClassColours, fs);
                            break;
                        case "baseClassForeColours":
                            ReadData(baseClassForeColours, fs);
                            break;
                        case "AAClassSets":
                            ReadData(AAClassSets, fs);
                            break;
                        case "fontAA":
                            line = fs.ReadLine();
                            fontAA = ReadFont(line, FontAA);
                            break;
                        case "fontName":
                            line = fs.ReadLine();
                            fontName = ReadFont(line, FontName);
                            break;
                        case "pairs":
                            ReadPairs(fs);
                            break;
                    }
                }
            }
            catch (Exception ex) { }
            finally { fs?.Close(); }
        }

        private void WriteData(Dictionary<string, Color> data, System.IO.StreamWriter fw)
        {
            string line;
            foreach (var kvp in data)
            {
                if (kvp.Value.IsKnownColor == true)
                { line = $"{kvp.Key},{kvp.Value.Name}"; }
                else
                { line = $"{kvp.Key},{kvp.Value.A},{kvp.Value.R},{kvp.Value.G},{kvp.Value.B}"; }

                fw.WriteLine(line);
            }
            fw.WriteLine("end");
        }

        private void WriteData(Dictionary<string, string> data, System.IO.StreamWriter fw)
        {
            string line;
            foreach (var kvp in data)
            {
                line = $"{kvp.Key},{kvp.Value}";
                fw.WriteLine(line);
            }
            fw.WriteLine("end");
        }

        private void ReadData(Dictionary<string, Color> data, System.IO.StreamReader fs)
        {
            data.Clear();
            while (fs.Peek() > 0)
            {
                string line = fs.ReadLine();
                if (line == "end") { break; }

                string[] items = line.Split(',');
                if (items.Length == 5)
                {
                    int a = int.Parse(items[1]);
                    int r = int.Parse(items[2]);
                    int g = int.Parse(items[3]);
                    int b = int.Parse(items[4]);
                    data[items[0]] = Color.FromArgb(a, r, g, b);
                }
                else if (items.Length == 2)
                {
                    Color col = Color.FromName(items[1]);
                    if (col.IsKnownColor == true)
                    { data[items[0]] = col; }
                }
            }
        }

        private void ReadData(Dictionary<string, string> data, System.IO.StreamReader fs)
        {
            data.Clear();
            while (fs.Peek() > 0)
            {
                string line = fs.ReadLine();
                if (line == "end") { break; }

                string[] items = line.Split(',');
                if (items.Length == 2)
                {
                    data[items[0]] = items[1];
                }
            }
        }

        private void WriteFont(Font font, System.IO.StreamWriter fw)
        {
            fw.WriteLine("Name=" + font.Name + "\t" + "Size=" + font.Size.ToString() + "\t" + "Style=" + font.Style.ToString() + "\t" + "Unit=" + font.Unit.ToString());
        }

        private Font ReadFont(string line, Font current)
        {
            string name = "";
            float size = 8;
            FontStyle style = FontStyle.Regular;
            GraphicsUnit unit = GraphicsUnit.Point;
            try
            {
                string[] pairs = line.Split('\t');
                if (pairs.Length == 4)
                {

                    foreach (string pair in pairs)
                    {
                        string[] items = pair.Split('=');
                        if (items.Length == 2)
                        {
                            switch (items[0])
                            {
                                case "Name":
                                    name = items[1];
                                    break;
                                case "Size":
                                    size = float.Parse(items[1]);
                                    break;
                                case "Style":
                                    style = (FontStyle)Enum.Parse(typeof(FontStyle), items[1]);
                                    break;
                                case "Unit":
                                    unit = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), items[1]);
                                    break;
                            }
                        }
                    }

                }

                if (name == "") { return current; }
                else
                { return new Font(name, size, style, unit); }
            }
            catch { return current; }
        }

        private void ReadPairs(System.IO.StreamReader fs)
        {
            while (fs.Peek() > 0)
            {
                string line = fs.ReadLine();
                string[] items = line.Split('=');
                if (items.Length == 2)
                {
                    switch (items[0])
                    {
                        case "conservedResidue":
                            conservedResidue = int.Parse(items[1]);
                            break;
                        case "conservedClass":
                            conservedClass = int.Parse(items[1]);
                            break;
                        case "labelWidth":
                            labelWidth = int.Parse(items[1]);
                            break;
                        case "locationOfAACoordinates":
                            locationOfAACoordinates = Enum.Parse<LocationOfTexts>(items[1]);
                            break;
                        case "locationOfConsensusAA":
                            locationOfConsensusAA = Enum.Parse<LocationOfTexts>(items[1]);
                            break;
                        case "locationOfConsensusClass":
                            locationOfConsensusClass = Enum.Parse<LocationOfTexts>(items[1]);
                            break;
                        case "end":
                            return;
                            break;
                    }

                }
            }

        }

        #region Layout
        private List<ClassDrawingOrder> layout = new List<ClassDrawingOrder> { ClassDrawingOrder.Interval_markers, ClassDrawingOrder.Gene_sequences, ClassDrawingOrder.InterProScan_features, ClassDrawingOrder.Uniprot_features, ClassDrawingOrder.GenBank_protein_features };
        public List<ClassDrawingOrder> Layout { get { return layout; } set { layout = value; } }
        #endregion

    }
}

