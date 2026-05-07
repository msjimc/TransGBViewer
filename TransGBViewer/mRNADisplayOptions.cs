using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TransGBViewer
{
    public partial class mRNADisplayOptions : Form
    {
        scalar interfaceScale = new scalar(96);
        mRNADisplayViewer mRNADV = null;
        mRNADisplayParameters parameters = new mRNADisplayParameters();

        public mRNADisplayOptions()
        {
            InitializeComponent();

            InitalizeFormatAndDisplayTab();

        }

        #region Reading and process Data files
        private void btnSelectSequence_Click(object sender, EventArgs e)
        {

            if (chkFolder.Checked == true)
            {
                string folderName = FileString.GetFolder("Select the folder of GenBank files", "");
                if (System.IO.Directory.Exists(folderName) == false) { return; }

                parameters = new mRNADisplayParameters();
                List<string> genbankFiles = new List<string>(); ;

                string[] extension = { "*.gb", "*.genbank", "*.gb.gz", "*.genbank.gz" };
                foreach (string ext in extension)
                {
                    string[] files = System.IO.Directory.GetFiles(folderName, ext);
                    if (files.Length > 0) { genbankFiles.AddRange(files); }
                }

                foreach (string file in genbankFiles)
                { ReadGenBankFile(file); }

                if (genbankFiles.Count == 0)
                { MessageBox.Show("No GenBank files in folder."); return; }
            }
            else
            {
                String fileName = FileString.OpenAs("Select the gene data file", "GenBank file (*.gb;*.genbank;*.gb.gz;*.genbank.gz))|*.gb;*.genbank;*.gb.gz;*.genbank.gz");
                if (System.IO.File.Exists(fileName) == false || fileName == "Cancel") { return; }

                parameters = new mRNADisplayParameters();

                ReadGenBankFile(fileName);
            }

            parameters.ExonSet = makeMinimumExonSet();
            if (mRNADV == null)
            {
                mRNADV = new mRNADisplayViewer(this);
                mRNADV.Show();
            }


            checkForExons();

            makeAllTranscriptSequence();
            InitalizeFormatAndDisplayTab();
            ReDraw();

            setFrame();
            MakeListOfSwappableExons();
        }

        private void checkForExons()
        {
            List<string> noExons = new List<string>();
            foreach (string name in parameters.SequenceNames)
            {
                if (parameters.Exons.ContainsKey(name) == false)
                { noExons.Add(name); }
            }
            string list = "";
            if (noExons.Count > 1)
            {
                for (int index = 0; index < noExons.Count - 1; index++)
                { list += noExons[index] + ", "; }
                list = list.Substring(0, list.Length - 1) + " and " + noExons[noExons.Count - 1];
            }
            else if (noExons.Count == 1)
            {
                list = (noExons[0]);
            }
            else { return; }

            if (MessageBox.Show("These sequences have no exon data, do you want to try to create it?" + "\r\n" + list, "No exon data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                makeExonsList(noExons);
            }

        }

        private void makeExonsList(List<string> noExons)
        {
            string SequenceBase = GetLongestTranscriptWithGaps();
            string[] exonSeqs = SequenceBase.Split(' ');
            string SequenceBaseGapless = SequenceBase.Replace(" ", "");

            foreach (string name in noExons)
            {
                List<Point> possibleExons = new List<Point>();
                foreach (string exonSeq in exonSeqs)
                {
                    if (exonSeq != "")
                    {
                        string alignedBase = "";
                        string alignedTest = "";
                        int score = 0;
                        string debugstring = parameters.SequenceDNA[name];
                        (alignedBase, alignedTest, score) = SequenceAlignment.LocalDNAAlignment(exonSeq, parameters.SequenceDNA[name]);
                        //string result = "bad";
                        if ((float)score / 2 > (float)alignedTest.Length * 0.9)
                        {
                            string exon = alignedBase.Replace("-", "").Trim();
                            int place = parameters.SequenceDNA[name].IndexOf(exon);
                            if (place > -1)
                            {
                                Point p = new Point(place + 1, place + exon.Length);
                                possibleExons.Add(p);
                            }
                        }
                    }
                }

                possibleExons = possibleExons.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
                List<Point> missedBits = new List<Point>();
                for (int index = 0; index < possibleExons.Count - 1; index++)
                {
                    if (possibleExons[index + 1].X > possibleExons[index].Y + 10)
                    { missedBits.Add(new Point(possibleExons[index].Y + 1, possibleExons[index + 1].X - 1)); }
                }
                if (missedBits.Count > 0)
                {
                    possibleExons.AddRange(missedBits);
                    possibleExons = possibleExons.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
                }
                parameters.Exons[name] = possibleExons;
            }

        }

        private void ReadGenBankFile(string FileName)
        {
            FileProcessing fp = new FileProcessing(FileName);

            string line = "";
            string definition = "";
            String name = "";
            string geneName = "";

            while (fp.Peek() > 0)
            {
                line = fp.ReadLine();

                if (line.StartsWith("LOCUS"))
                {
                    if (line.Contains("DNA     linear") == true)
                    { parameters.DataType = "genomic"; }
                    else if (line.Contains("mRNA    linear") == true)
                    { parameters.DataType = "mRNA"; }
                    else if (line.Contains("aa            linear") == true)
                    { parameters.DataType = "protein"; }
                }
                else if (line.StartsWith("DEFINITION  ") == true)
                {
                    definition = line.Substring(12).Trim();
                }
                else if (line.StartsWith("ACCESSION   ") == true)
                {
                    name = line.Substring(12).Trim();
                    if (name.Contains(" ") == true)
                    { name = name.Substring(0, name.IndexOf(" ")).Trim(); }
                    parameters.AddSequenceName(name);
                    parameters.AddDefination(name, definition);
                    definition = "";
                }
                else if (line.StartsWith("                     /gene=\""))
                {
                    if (line.EndsWith("\"" + geneName + "\"") == false)
                    {
                        geneName = line.Substring(28, line.Length - 29);
                        parameters.AddSequenceGeneName(name, geneName);
                    }
                }
                else if (line.StartsWith("     misc_feature    "))
                {
                    Point temp = GetCoordinates(line);
                    string note = "";
                    if (temp.Y - temp.X < 3)
                    { note = MiscFeatureNoteShort(fp); }
                    else
                    { note = MiscFeatureNoteLong(fp); }
                    mRNADisplayMiscFeature mRNADMF = new mRNADisplayMiscFeature(temp.X, temp.Y, note, Color.Black);
                    parameters.AddMiscfeature(name, mRNADMF);
                }
                else if (line.StartsWith("     exon            "))
                {
                    parameters.AddExons(name, GetCoordinates(line));
                }
                else if (line.StartsWith("     CDS"))
                {
                    parameters.AddCDS(name, GetCoordinates(line));
                    string[] returnedData = GetTranslation(fp);
                    parameters.ProteinIDs[name] = returnedData[0];
                    parameters.AddAminoAcid(name, returnedData[1]);
                }
                else if (line.StartsWith("     polyA_site      ") == true)
                {
                    Point temp = GetCoordinates(line);
                    string note = polyA_siteNote(fp);
                    mRNADisplayMiscFeature mRNADMF = new mRNADisplayMiscFeature(temp.X, temp.Y, note, Color.Black);
                    parameters.AddMiscfeature(name, mRNADMF);
                }
                else if (line.StartsWith("     regulatory      "))
                {
                    Point temp = GetCoordinates(line);
                    string note = RegulatoryNote(fp);
                    mRNADisplayMiscFeature mRNADMF = new mRNADisplayMiscFeature(temp.X, temp.Y, note, Color.Black);
                    parameters.AddMiscfeature(name, mRNADMF);
                }
                else if (line.StartsWith("ORIGIN") == true)
                {
                    string sequence = getSequence(fp);
                    parameters.SequenceDNA.Add(name, sequence);
                }
            }
            fp.Close();
        }

        private string getSequence(FileProcessing fp)
        {
            StringBuilder sequence = new StringBuilder();
            string line = "";
            while (fp.Peek() > 0 && fp.Peek() != 79)
            {
                line = fp.ReadLine();
                if (line.StartsWith("//") == true)
                { break; }
                else
                {
                    string bit = line.Substring(10).Trim().Replace(" ", "");
                    sequence.Append(bit);
                }
            }
            return sequence.ToString();
        }

        private string[] GetTranslation(FileProcessing fp)
        {
            string line = "";
            string[] translation = { "", "" };
            while (fp.Peek() > 0 && fp.Peek() != 79)
            {
                line = fp.ReadLine();
                if (line.StartsWith("                     /protein_id=\""))
                { translation[0] = line.Substring(34).Trim().Replace("\"", ""); }
                else if (line.StartsWith("                     /translation=\""))
                {
                    translation[1] = line.Substring(35).Trim();
                    while (fp.Peek() > 0)
                    {
                        line = fp.ReadLine();
                        if (line.StartsWith("                     ") == true)
                        {
                            translation[1] += line.Substring(21).Trim();
                            if (line.TrimEnd().EndsWith("\"") == true)
                            {
                                translation[1] = translation[1].Substring(0, translation[1].Length - 1);
                                fp.RePresentLastLine();
                                return translation;
                            }
                        }
                        else if (line.StartsWith("        ") == false)
                        { break; }
                    }
                }
            }
            fp.RePresentLastLine();
            return translation;
        }

        private Point GetCoordinates(string line)
        {
            string bit = line.Substring(21).Trim();
            int index = bit.IndexOf("..");
            if (index > 0)
            {
                bit = bit.Replace("<", "").Replace(">", "").Replace("complement(", "").Replace(")", "").Trim();
                index = bit.IndexOf("..");
                int start = Convert.ToInt32(bit.Substring(0, index));
                int end = Convert.ToInt32(bit.Substring(index + 2));
                return new Point(start, end);
            }
            else
            {
                int answer = 0;
                if (int.TryParse(bit, out answer) == true)
                { return new Point(answer, answer); }
                else { return new Point(0, 0); }
            }
        }

        private string MiscFeatureNoteShort(FileProcessing fp)
        {
            string note = "";
            string line = "";
            while (fp.Peek() > 0 && fp.Peek() != 79)
            {
                line = fp.ReadLine();
                if (line.StartsWith("                     /note=\"") == true)
                {
                    if (line.Contains(";") == false)
                    { return line.Substring(28).Trim().Replace("\"", ""); }
                    else
                    {
                        note = line.Substring(line.IndexOf(";") + 1).Trim().Replace("\"", "");
                        int index = note.IndexOf("Region:");
                        if (index > -1)
                        { note = note.Substring(index + 7).Trim(); }
                        return note;
                    }
                }
                else if (line.StartsWith("        ") == false)
                { fp.RePresentLastLine(); break; }
            }
            fp.RePresentLastLine();
            return "Misc_Feature";
        }

        private string MiscFeatureNoteLong(FileProcessing fp)
        {
            string note = "";
            string line = "";
            while (fp.Peek() > 0 && fp.Peek() != 79)
            {
                line = fp.ReadLine();
                if (line.StartsWith("                     /note=\"") == true)
                {
                    while (fp.Peek() > 0)
                    {
                        line = fp.ReadLine();
                        if (line.StartsWith("                     Region:") == true)
                        {
                            note = line.Substring(28).Trim().Replace("\"", "");
                            int index = note.IndexOf("/");
                            if (index > -1)
                            { note = note.Substring(0, index).Trim(); }
                            return note;
                        }
                        else if (line.StartsWith("        ") == false)
                        {
                            fp.RePresentLastLine();
                            break;
                        }
                    }
                }
                else if (line.StartsWith("        ") == false)
                { fp.RePresentLastLine(); break; }
            }
            fp.RePresentLastLine();
            return "Misc_Feature";
        }

        private string RegulatoryNote(FileProcessing fp)
        {
            string line = "";
            while (fp.Peek() > 0 && fp.Peek() != 79)
            {
                line = fp.ReadLine();
                if (line.StartsWith("                     /regulatory_class=\"") == true)
                { return line.Substring(39).Trim().Replace("\"", ""); }
                else if (line.StartsWith("        ") == false)
                { fp.RePresentLastLine(); break; }

            }
            fp.RePresentLastLine();
            return "Regulatory element";
        }

        private string polyA_siteNote(FileProcessing fp)
        {
            string line = "";
            while (fp.Peek() > 0 && fp.Peek() != 79)
            {
                line = fp.ReadLine();
                if (line.StartsWith("                     /note=\"") == true)
                { return line.Substring(28).Trim().Replace("\"", ""); }
                else if (line.StartsWith("        ") == false)
                { break; }
            }

            fp.RePresentLastLine();
            return "PolyA site";
        }

        private Dictionary<string, exonGraphNode> makeOrderedExonList(List<exonGraphNode> exonOrderedSet)
        {
            Dictionary<string, exonGraphNode> answer = new Dictionary<string, exonGraphNode>();
            foreach (exonGraphNode egn in exonOrderedSet)
            {
                answer.Add(egn.seqence, egn);
            }
            return answer;
        }

        private Dictionary<string, exonGraphNode> makeMinimumExonSet()
        {
            Dictionary<string, exonGraphNode> exonSetDict = new Dictionary<string, exonGraphNode>();
            foreach (string name in parameters.SequenceNames)
            {
                //if (parameters.Exons.ContainsKey(name) == false)
                //{
                //    Point e = new Point(1, parameters.SequenceDNA[name].Length - 1);
                //    List<Point> es = new List<Point>();
                //    es.Add(e);
                //    parameters.Exons.Add(name, es);
                //}
                if (parameters.Exons.ContainsKey(name) == false) continue;
                List<Point> exons = parameters.Exons[name];
                foreach (Point exon in exons)
                {
                    string sequence = parameters.SequenceDNA[name].Substring(exon.X - 1, exon.Y - exon.X + 1);
                    if (exonSetDict.ContainsKey(sequence) == true)
                    {
                        exonGraphNode eGN = exonSetDict[sequence];
                        eGN.count++;
                        exonSetDict[sequence] = eGN;
                    }
                    else
                    {
                        exonGraphNode eGN = new exonGraphNode(exonSetDict.Count, exon, sequence);
                        eGN.count = 1;
                        exonSetDict.Add(sequence, eGN);
                    }
                }
            }

            List<exonGraphNode> exonSet = LinkExonGraph(exonSetDict);

            return makeOrderedExonList(exonSet);
        }

        private List<exonGraphNode> LinkExonGraph(Dictionary<string, exonGraphNode> exonSetDict)
        {

            foreach (string name in parameters.SequenceNames)
            {
                if (parameters.Exons.ContainsKey(Name) == false) continue;
                List<Point> exons = parameters.Exons[name];
                string previousSequence = "";
                string sequence = "";
                string nextSequence = "";
                for (int index = 0; index < exons.Count; index++)
                {
                    previousSequence = sequence;
                    if (string.IsNullOrEmpty(nextSequence) == true)
                    { sequence = parameters.SequenceDNA[name].Substring(exons[0].X - 1, exons[0].Y - exons[0].X + 1); ; }
                    else { sequence = nextSequence; }

                    if (index < exons.Count - 1)
                    { nextSequence = parameters.SequenceDNA[name].Substring(exons[index + 1].X - 1, exons[index + 1].Y - exons[index + 1].X + 1); }
                    else { nextSequence = ""; }

                    if (previousSequence == "")
                    { exonSetDict[sequence].previous.Add(-1); }
                    else
                    { exonSetDict[sequence].previous.Add(exonSetDict[previousSequence].index); }

                    if (nextSequence == "")
                    { exonSetDict[sequence].next.Add(-1); }
                    else
                    { exonSetDict[sequence].next.Add(exonSetDict[nextSequence].index); }

                }
            }

            SetExonColumn(exonSetDict);

            return exonSetDict.Values.ToList();
        }

        private void SetExonColumn(Dictionary<string, exonGraphNode> exonSet)
        {
            if (exonSet.Count == 0) { return; }
            List<exonGraphNode> orderedList = new List<exonGraphNode>();
            List<exonGraphNode> listOfexons = exonSet.Values.ToList();

            foreach (string name in parameters.SequenceNames)
            {
                if (parameters.Exons.ContainsKey(name) == false) continue;
                List<Point> exons = parameters.Exons[name];
                if (orderedList.Count == 0)
                {
                    foreach (Point point in exons)
                    {
                        string sequence = parameters.SequenceDNA[name].Substring(point.X - 1, point.Y - point.X + 1);
                        exonGraphNode exon = exonSet[sequence];
                        orderedList.Add(exon);
                    }
                    foreach (string name2 in parameters.SequenceNames)
                    {
                        if (parameters.Exons.ContainsKey(name2) == false) continue;
                        List<Point> exons2 = parameters.Exons[name];
                        string sequence = parameters.SequenceDNA[name2].Substring(parameters.Exons[name2][0].X - 1, parameters.Exons[name2][0].Y - parameters.Exons[name2][0].X + 1);
                        exonGraphNode exon = exonSet[sequence];
                        if (orderedList.Contains(exon) == false) { orderedList.Insert(0, exon); }
                    }
                }
                else
                {
                    foreach (Point point in exons)
                    {
                        string sequence = parameters.SequenceDNA[name].Substring(point.X - 1, point.Y - point.X + 1);
                        if (orderedList.Contains(exonSet[sequence]) == false)
                        {
                            exonGraphNode exon = exonSet[sequence];
                            int index = 0;
                            while (index < orderedList.Count && relativePosition(orderedList[index], exon, listOfexons) > 0)
                            { index++; }
                            if (index == orderedList.Count)
                            { orderedList.Insert(0, exon); }
                            else
                            { orderedList.Insert(index, exon); }
                        }
                    }
                }
            }

            for (int index = 0; index < orderedList.Count; index++)
            {
                orderedList[index].order = index;
            }
        }

        private int relativePosition(exonGraphNode exon, exonGraphNode exonTest, List<exonGraphNode> listOfExons)
        {
            if (exon.index == exonTest.index) { return 0; }

            if (IsExonDownStream(exon, exonTest, listOfExons) == true) { return 1; }
            else { return -1; }
        }

        private bool IsExonDownStream(exonGraphNode exon, exonGraphNode exonTest, List<exonGraphNode> listOfExons)
        {
            int index = exonTest.index;
            if (exon.next.Contains(exonTest.index) == true)
            { return true; }
            else
            {
                foreach (int next in exon.next)
                {
                    if (next >= 0)
                    {
                        exonGraphNode nextExon = listOfExons[next];
                        if (IsExonDownStream(nextExon, exonTest, listOfExons) == true)
                        { return true; }
                    }
                }
            }
            return false;
        }

        #endregion

        #region Get Homology

        private void makeAllTranscriptSequence()
        {
            string SequenceBase = GetLongestTranscriptWithGaps();

            List<exonGraphNode> absentBits = GetMissingExonsFromLongestgapedtranscript(SequenceBase);

            foreach (exonGraphNode missing in absentBits)
            {
                int fragmentIndex = -1;
                float score = 0.0f;
                exonGraphNode egn = null;
                (score, egn, fragmentIndex) = GetBestHit(SequenceBase, missing);
                if (score < 1.5)
                { SequenceBase = AddMissingExon(SequenceBase, missing); }
            }

            foreach (exonGraphNode missing in absentBits)
            {
                int fragmentIndex = -1;
                float score = 0.0f;
                exonGraphNode egn = null;
                (score, egn, fragmentIndex) = GetBestHit(SequenceBase, missing);
                if (score > 1.5)
                {
                    string bit1 = "";
                    string bit2 = "";
                    string[] bits = SequenceBase.Split(' ');
                    (bit1, bit2) = SequenceAlignment.BlockedAlignment(bits[fragmentIndex], missing.seqence);
                    if (SequenceBase.IndexOf(bit1) > -1)
                    {
                        int placeInTranscript = SequenceBase.IndexOf(bit1);
                        int placeInExon = missing.seqence.IndexOf(bit1);
                        SequenceBase = AddExtraExonSequences(SequenceBase, placeInTranscript, missing.seqence, placeInExon, bit1);
                    }
                    else if (SequenceBase.IndexOf(bit2) > -1)
                    {
                        int placeInTranscript = SequenceBase.IndexOf(bit2);
                        int placeInExon = missing.seqence.IndexOf(bit2);
                        SequenceBase = AddExtraExonSequences(SequenceBase, placeInTranscript, missing.seqence, placeInExon, bit2);
                    }
                }
            }

            SequenceBase = RemoveduplicatedExons(SequenceBase);

            parameters.AllSequences = SequenceBase;

        }

        private string RemoveduplicatedExons(string sequenceBase)
        {
            string newSequence = sequenceBase;
            foreach (string name in parameters.SequenceNames)
            {
                if (parameters.Exons.ContainsKey(name) == false) continue;
                List<Point> exons = parameters.Exons[name];
                foreach (Point exon in exons)
                {
                    string exonSequence = parameters.getExonSequence(name, exon);
                    int indexFront = sequenceBase.IndexOf(exonSequence);
                    int indexEnd = sequenceBase.LastIndexOf(exonSequence);
                    if (indexEnd != indexFront)
                    {
                        int indexFirstFirstSpace = sequenceBase.LastIndexOf(" ", indexFront + 1);
                        int indexFirstSecondSpace = sequenceBase.IndexOf(" ", indexFront);
                        string firstExon = sequenceBase.Substring(indexFirstFirstSpace + 1, indexFirstSecondSpace - indexFirstFirstSpace - 1);

                        int indexSecondFirstSpace = sequenceBase.LastIndexOf(" ", indexEnd + 1);
                        int indexSecondSecondSpace = sequenceBase.IndexOf(" ", indexEnd);
                        string secondtExon = sequenceBase.Substring(indexSecondFirstSpace + 1, indexSecondSecondSpace - indexSecondFirstSpace - 1);

                        if (indexFirstFirstSpace != indexSecondFirstSpace && indexFirstSecondSpace != indexSecondSecondSpace)
                        {
                            if (exonSequence == firstExon && exonSequence != secondtExon)
                            { newSequence = sequenceBase.Substring(0, indexFirstFirstSpace + 1) + " " + sequenceBase.Substring(indexFirstSecondSpace); }
                            else if (exonSequence != firstExon && exonSequence == secondtExon)
                            { newSequence = sequenceBase.Substring(0, indexSecondFirstSpace + 1) + " " + sequenceBase.Substring(indexSecondSecondSpace); }
                            else if (exonSequence == firstExon && exonSequence == secondtExon)
                            { newSequence = sequenceBase.Substring(0, indexFirstFirstSpace + 1) + " " + sequenceBase.Substring(indexFirstSecondSpace); }
                            else
                            {
                                string extendedExon = CombineTwoExons(firstExon, secondtExon, exonSequence);
                                newSequence = sequenceBase.Substring(0, indexSecondFirstSpace + 1) + " " + sequenceBase.Substring(indexSecondSecondSpace);
                                newSequence = sequenceBase.Substring(0, indexFirstFirstSpace + 1) + " " + extendedExon + " " + sequenceBase.Substring(indexFirstSecondSpace);
                            }
                        }

                    }
                }
            }

            int length = newSequence.Length;
            newSequence = newSequence.Replace("  ", " ");
            while (length != newSequence.Length)
            {
                length = newSequence.Length;
                newSequence = newSequence.Replace("  ", " ");
            }
            return newSequence;
        }

        private string AddMissingExon(string sequence, exonGraphNode missing)
        {
            StringBuilder newSequence = new StringBuilder(sequence.Length + missing.seqence.Length + 1);
            int place = missing.order;
            string[] sequences = sequence.Split(' ');
            for (int index = 0; index < sequences.Length; index++)
            {
                if (index == place)
                { newSequence.Append(" " + missing.seqence); }
                newSequence.Append(" " + sequences[index]);
            }
            return newSequence.ToString().Substring(1);
        }

        private string GetLongestTranscriptWithGaps()
        {

            string longest = " ";
            int currentLongest = 0;
            foreach (string name in parameters.SequenceNames)
            {
                int l = parameters.SequenceDNA[name].Length;
                if (l > currentLongest)
                {
                    currentLongest = l;
                    longest = name;
                }
            }

            string SequenceBase = "";
            foreach (Point exon in parameters.Exons[longest])
            { SequenceBase += parameters.getExonSequence(longest, exon) + " "; }

            return SequenceBase;
        }

        private List<exonGraphNode> GetMissingExonsFromLongestgapedtranscript(string gappedSequence)
        {
            List<exonGraphNode> absentBits = new List<exonGraphNode>();
            foreach (exonGraphNode egn in parameters.ExonSet.Values)
            {
                if (gappedSequence.Contains(egn.seqence) == false)
                { absentBits.Add(egn); }
            }

            return absentBits;
        }

        private string AddExtraExonSequences(string sequenceBase, int placeInSequencebase, string newSequence, int placeInNewSequence, string common)
        {
            int indexFirstSpace = sequenceBase.LastIndexOf(" ", placeInSequencebase + 1);
            int indexSecondSpace = sequenceBase.IndexOf(" ", placeInSequencebase);
            string fragment = sequenceBase.Substring(indexFirstSpace + 1, indexSecondSpace - indexFirstSpace - 1);

            int present5 = fragment.IndexOf(common);
            int possibleExtra5 = newSequence.IndexOf(common);
            string extendedSequence = fragment;

            if (possibleExtra5 > present5)
            { extendedSequence = newSequence.Substring(0, possibleExtra5 - present5) + fragment; }

            int present3 = fragment.Length - (fragment.IndexOf(common) + common.Length);
            int possibleExtra3 = newSequence.Length - (newSequence.IndexOf(common) + common.Length);

            if (possibleExtra3 > present3)
            { extendedSequence += newSequence.Substring(newSequence.Length - (possibleExtra3 - present3)); }

            string sequenceFinal = sequenceBase.Substring(0, indexFirstSpace + 1) + " " + extendedSequence + " " + sequenceBase.Substring(indexSecondSpace);

            return sequenceFinal.Replace("  ", " ");
        }

        private string CombineTwoExons(string firstExon, string secondExon, string common)
        {
            int present5 = firstExon.IndexOf(common);
            int possibleExtra5 = secondExon.IndexOf(common);
            string extendedSequence = firstExon;

            if (possibleExtra5 > present5)
            { extendedSequence = secondExon.Substring(0, possibleExtra5 - present5) + firstExon; }

            int present3 = firstExon.Length - (firstExon.IndexOf(common) + common.Length);
            int possibleExtra3 = secondExon.Length - (secondExon.IndexOf(common) + common.Length);

            if (possibleExtra3 > present3)
            { extendedSequence += secondExon.Substring(secondExon.Length - (possibleExtra3 - present3)); }

            return extendedSequence;
        }

        private (float score, exonGraphNode hit, int index) GetBestHit(string sequence, exonGraphNode missing)
        {
            string bit = missing.seqence;
            return GetBestHit(sequence, missing.seqence);
        }

        private (float score, exonGraphNode hit, int index) GetBestHit(string sequence, string missing)
        {
            string bit = missing;
            float trackingScore = float.MinValue;
            string[] sequenceBits = sequence.Split(' ');

            int hitIndex = -1;
            string hit = "";
            for (int index = 0; index < sequenceBits.Length; index++)
            {
                string alignedBase = "";
                string alignedTest = "";
                int score = 0;
                (alignedBase, alignedTest, score) = SequenceAlignment.LocalDNAAlignment(sequenceBits[index], bit);
                int length = bit.Length;
                if (length > sequenceBits[index].Length) { length = sequenceBits[index].Length; }

                float adjustedScore = ((float)score / length);
                if (adjustedScore > trackingScore)
                {
                    trackingScore = adjustedScore;
                    hit = bit;
                    hitIndex = index;
                    if (trackingScore == 2)
                    { return (trackingScore, parameters.ExonSet[hit], hitIndex); }
                }
            }

            return (trackingScore, parameters.ExonSet[hit], hitIndex);
        }

        private void MakeListOfSwappableExons()
        {
            String[] allExons = parameters.AllSequences.Split(' ');
            List<Point> swappable = new List<Point>();
            for (int indexToMoveBack = 0; indexToMoveBack < allExons.Length - 1; indexToMoveBack++)
            {
                string newOrder = "";
                for (int index = 0; index < allExons.Length; index++)
                {
                    if (index == indexToMoveBack)
                    { newOrder += " " + allExons[index + 1] + " " + allExons[index]; }
                    else if (index != indexToMoveBack + 1)
                    { newOrder += " " + allExons[index]; }
                }

                if (TestOrder(newOrder) == true)
                { swappable.Add(new Point(indexToMoveBack, indexToMoveBack + 1)); }
            }

            cblMoveableAlternativeExons.Items.Clear();
            foreach (Point p in swappable)
            {
                string item = "";
                item = (p.X + 1).ToString() + " <--> " + (p.Y + 1).ToString();
                cblMoveableAlternativeExons.Items.Add(item, false);
            }
        }

        private bool TestOrder(string newOrder)
        {
            foreach (string key in parameters.Exons.Keys)
            {
                int lastIndex = -1;
                List<Point> exons = parameters.Exons[key];
                foreach (Point exon in exons)
                {
                    string exonSequence = parameters.getExonSequence(key, exon);
                    int index = newOrder.IndexOf(exonSequence);
                    if (index == -1)
                    { return false; }
                    else if (index <= lastIndex)
                    { return false; }
                    lastIndex = index;
                }
            }
            return true;
        }

        private void cblMoveableAlternativeExons_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < cblMoveableAlternativeExons.Items.Count; i++)
                {
                    if (i != e.Index)
                    { cblMoveableAlternativeExons.SetItemChecked(i, false); }
                }
                btnMove.Enabled = true;
            }
            else { btnMove.Enabled = false; }

        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (cblMoveableAlternativeExons.CheckedItems.Count > 0)
            {
                int selectedIndex = cblMoveableAlternativeExons.CheckedIndices[0];
                string name = cblMoveableAlternativeExons.Items[selectedIndex].ToString();

                string[] items = name.Split(' ');
                int indexToMoveBack = Convert.ToInt32(items[0]) - 1;

                string newOrder = "";
                String[] allExons = parameters.AllSequences.Split(' ');
                for (int index = 0; index < allExons.Length; index++)
                {
                    if (index == indexToMoveBack)
                    { newOrder += " " + allExons[index + 1] + " " + allExons[index]; }
                    else if (index != indexToMoveBack + 1)
                    { newOrder += " " + allExons[index]; }
                }

                parameters.AllSequences = newOrder.Trim();
            }


            ReDraw();
            MakeListOfSwappableExons();
        }

        #endregion

        #region Interface       

        public Bitmap DisplayResized(Size imageSize)
        {
            nudGeneFeatureX.Maximum = imageSize.Width - parameters.LabelWidth;
            nudGeneFeatureY.Maximum = imageSize.Height;
            return DrawGraph(interfaceScale);
        }

        public void DisplayClosed()
        { mRNADV = null; }

        public void pDisplayMouseDown(Point p)
        {
        }

        #endregion

        #region Drawing Stuff
        private Bitmap DrawGraph(scalar scale)
        {
            if (parameters.SequenceNames.Count == 0) { return Blank(); }
            if (mRNADV == null || mRNADV.WindowState == FormWindowState.Minimized) { return null; }

            parameters.DrawingArea = mRNADV.GetDrawingArea();
            float width = parameters.DrawingArea.Width * scale.scale;

            Bitmap bmp = new Bitmap((int)(parameters.DrawingArea.Width * scale.scale), (int)(parameters.DrawingArea.Height * scale.scale), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmp.SetResolution(scale.DPI, scale.DPI);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.Clear(Color.White);

            float w = (parameters.LabelWidth + 1) * scale.scale;

            Bitmap bmpLabels = new Bitmap((int)((parameters.LabelWidth + 1) * scale.scale), (int)(parameters.DrawingArea.Height * scale.scale), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmpLabels.SetResolution(scale.DPI, scale.DPI);
            Graphics gLabels = Graphics.FromImage(bmpLabels);
            gLabels.SmoothingMode = SmoothingMode.AntiAlias;
            gLabels.CompositingQuality = CompositingQuality.HighQuality;
            gLabels.Clear(Color.White);

            Point limitRegion = parameters.Zoom;

            int numberOfGaps = getNumberOfGapsInAllSequences(limitRegion);
            int sequenceWidth = limitRegion.Y + 1 - limitRegion.X - numberOfGaps;

            int gapWidth = numberOfGaps * scale.i[parameters.IntronGap];
            float scaleFactor = (width - gapWidth - scale.scale * (30 + parameters.LabelWidth)) / sequenceWidth;

            Draw(g, gLabels, bmp, bmpLabels, scale, scaleFactor, limitRegion);


            return bmp;
        }

        private int getNumberOfGapsInAllSequences(Point limitRegion)
        {
            int ignore = getNumberOfGapsInAllSequences(limitRegion.X);
            int whole = getNumberOfGapsInAllSequences(limitRegion.Y);
            return whole - ignore;
        }

        private int getNumberOfGapsInAllSequences(int endPoint)
        {
            int answer = 0;
            string allSequences = parameters.AllSequences;
            for (int index = 0; index < endPoint; index++)
            {
                if (endPoint < index + 1)
                { break; }
                if (allSequences[index] == ' ')
                { answer++; }
            }
            return answer;
        }

        private Dictionary<string, int> getReferencedAccessionID()
        {
            Dictionary<string, int> legend = new Dictionary<string, int>();
            if (parameters.ShowSuperscript == true)
            {
                foreach (string key in parameters.MiscellaneousFeatureGBSet.Keys)
                {
                    string name = key.Substring(key.IndexOf("#") + 1);
                    if (legend.ContainsKey(name) == false)
                    { legend.Add(name, legend.Count + 1); }
                }
                foreach (string key in parameters.MiscellaneousFeatureUniprotSet.Keys)
                {
                    string name = key.Substring(key.LastIndexOf("#") + 1);
                    if (legend.ContainsKey(name) == false)
                    { legend.Add(name, legend.Count + 1); }
                }
                foreach (AlignmentDomainFeature feature in parameters.Selected.Values)
                {
                    string name = feature.BaseSequenceName;
                    if (legend.ContainsKey(name) == false)
                    { legend.Add(name, legend.Count + 1); }
                }
            }
            int counter = 1;
            foreach (string name in parameters.SequenceNames)
            {
                if (legend.ContainsKey(name) == true)
                { legend[name] = counter; counter++; }
            }

            return legend;
        }

        private Bitmap Draw(Graphics g, Graphics gLabels, Bitmap bmp, Bitmap bmpLabels, scalar scale, float scalefactor, Point limits)
        {
            int height = scale.i[10];
            Dictionary<string, int> legends = getReferencedAccessionID();
            int lineTop = height;
            int intervalTop = 0;
            string allSequences = parameters.AllSequences;
            List<RectangleF> masks = new List<RectangleF>();
            try
            {
                g.SmoothingMode = SmoothingMode.None;
                if (parameters.ShowExonLimits != "None")
                { DrawSpliceSites(g, lineTop, parameters.DrawingArea.Height * scale.scale, scale, scalefactor, parameters.ShowExonLimits, allSequences, limits); }
            }
            finally
            { g.SmoothingMode = SmoothingMode.AntiAlias; }

            foreach (ClassDrawingOrder dataset in parameters.Layout)
            {
                if (dataset == ClassDrawingOrder.Interval_markers)
                {
                    if (parameters.SizeMarkerLocation != SizeMarkerLocation.None)
                    {
                        intervalTop = height;
                        if (height > 0) { height += scale.i[10]; }
                        height = DrawSizeMarkers(g, scale, scalefactor, height - scale.i[10], bmp.Width, limits);
                        height += scale.i[5];
                    }
                }
                else if (dataset == ClassDrawingOrder.Gene_sequences)
                {
                    int currentHeight = height;
                    foreach (string name in parameters.SequenceDisplayNames.Keys)
                    {
                        if (parameters.Exons.ContainsKey(name) == false) continue;
                        List<Point> exonPoints = parameters.Exons[name];
                        if (parameters.LegendsLocation != DrawLabels.none)
                        {
                            string display = parameters.SequenceDisplayNames[name];
                            if (name == cboFrame.Text)
                            { display += "*"; }

                            if (legends.ContainsKey(name) == true)
                            { display += AddSupercript(legends[name]); }
                            height = DrawFeatureLabels(g, gLabels, parameters.IDFont, display, bmp.Width, scale.i[5], height, scale, name);
                        }

                        parameters.FeatureRows[parameters.SequenceDisplayNames[name]] = height;
                        foreach (Point exon in exonPoints)
                        {
                            string sequence = parameters.getExonSequence(name, exon);
                            int placeInAllsequences = allSequences.IndexOf(sequence);
                            int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limits.X, placeInAllsequences));
                            float startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                                ((placeInAllsequences - limits.X - numberofGapsbeforeSequence) * scalefactor);
                            float endPoint = startPoint + (sequence.Length * scalefactor);

                            DrawExons(g, startPoint, endPoint, height, scale.i[20], scale, name, limits);
                            if (parameters.ShowOrf == true || parameters.Reduced == true) { DrawOrf(g, startPoint, endPoint, height, scale.i[20], scale, name, exon, limits); }
                        }

                        if (parameters.ExonWithFrame.Count > 0)
                        { DrawFrames(g, height, scale.i[20], scalefactor, scale, name, limits); }

                        height += scale.i[25];
                    }
                    height += scale.i[10];
                    FillMask(g, currentHeight, height, scale, bmp.Width);
                }
                else if (dataset == ClassDrawingOrder.GenBank_features)
                {
                    if (parameters.MiscellaneousFeatureGBSet.Count > 0)
                    {
                        int currentHeight = height;
                        List<string> named = new List<string>();
                        height = DrawMscellaneousFeaturesGB(g, gLabels, height, scale.i[20], scale, scalefactor, named, limits, legends);
                        FillMask(g, currentHeight, height, scale, bmp.Width);
                    }
                }
                else if (dataset == ClassDrawingOrder.Uniprot_features)
                {
                    if (parameters.MiscellaneousFeatureUniprotSet.Count > 0)
                    {
                        int currentHeight = height;
                        List<string> named = new List<string>();
                        height = DrawMscellaneousFeaturesUniProt(g, gLabels, height, scale.i[20], scale, scalefactor, named, limits, legends);
                        FillMask(g, currentHeight, height, scale, bmp.Width);
                    }
                }
                else if (dataset == ClassDrawingOrder.InterProScan_features)
                {
                    if (parameters.Selected.Count > 0)
                    {
                        int currentHeight = height;
                        List<string> named = new List<string>();
                        height = DrawMiscellaneousFeaturesInterProScan(g, gLabels, height, scale.i[20], scale, scalefactor, named, limits, legends);
                        FillMask(g, currentHeight, height, scale, bmp.Width);
                    }
                }
                else if (dataset == ClassDrawingOrder.Sequence)
                {
                    if (parameters.BindingSites.Count > 0)
                    {
                        int currentHeight = height;
                        List<string> named = new List<string>();
                        height = DrawBindingSites(g, gLabels, height, scale.i[20], scale, scalefactor, named, legends, allSequences, limits, bmp);
                        FillMask(g, currentHeight, height, scale, bmp.Width);
                    }
                }
            }

            try
            {
                g.SmoothingMode = SmoothingMode.None;

                if (parameters.ShowORFLimits != "None")
                {
                    DrawORFSites(g, lineTop, parameters.DrawingArea.Height * scale.scale, scale, scalefactor, parameters.ShowORFLimits, allSequences, limits);
                    if (intervalTop > 0) { intervalTop += scale.i[10]; }
                    DrawSizeMarkers(g, scale, scalefactor, intervalTop - scale.i[10], bmp.Width, limits);
                }
            }
            finally
            { g.SmoothingMode = SmoothingMode.AntiAlias; }

            g.FillRectangle(Brushes.White, 0, height, parameters.DrawingArea.Width, parameters.DrawingArea.Height - height);

            if (parameters.LabelWidth > 0)
            { g.DrawImage(bmpLabels, 0, 0); }

            DrawGeneFeatures(g, scalefactor, scale);
            setcboFeatureTopsList();
            return bmp;

        }

        private void FillMask(Graphics g, int topHeight, int bottomHeight, scalar scale, int bmpWidth)
        {
            g.FillRectangle(Brushes.White, new RectangleF(0, topHeight, (scale.scale * parameters.LabelWidth) + scale.i[13], bottomHeight - topHeight));
            g.FillRectangle(Brushes.White, new RectangleF(bmpWidth - scale.i[13], topHeight, scale.i[13], bottomHeight - topHeight));
        }

        private void setcboFeatureTopsList()
        {
            string current = cboFeatureTops.Text;
            cboFeatureTops.Items.Clear();
            cboFeatureTops.Items.Add("Select");
            foreach (string key in parameters.FeatureRows.Keys)
            {
                cboFeatureTops.Items.Add(key);
            }
            int index = cboFeatureTops.Items.IndexOf(current);
            if (index > -1)
            { cboFeatureTops.SelectedIndex = index; }
            else { cboFeatureTops.SelectedIndex = 0; }
        }

        private int DrawFeatureLabels(Graphics g, Graphics gLabels, System.Drawing.Font f, string text, int width, int offset, int height, scalar scale, string key)
        {
            if (parameters.LegendsLocation == DrawLabels.left)
            {
                gLabels.DrawString(text, f, Brushes.Black, offset, height + scale.i[3]);
            }
            else if (parameters.LegendsLocation == DrawLabels.above)
            {
                SizeF area = g.MeasureString(text, f);
                float textCenter = area.Width / 2;
                float center = width / 2;
                g.FillRectangle(Brushes.White, new RectangleF(center - textCenter, height, area.Width, area.Height));
                g.DrawString(text, f, Brushes.Black, center - textCenter, height);
                height += scale.i[20];
            }
            return height;
        }

        private string AddSupercript(int number)
        {
            string answer = "";
            string text = number.ToString();
            foreach (char c in text)
            {
                switch (c)
                {
                    case '0':
                        answer += "\u2017";
                        break;
                    case '1':
                        answer += "\u00B9";
                        break;
                    case '2':
                        answer += "\u00B2";
                        break;
                    case '3':
                        answer += "\u00B3";
                        break;
                    case '4':
                        answer += "\u2074";
                        break;
                    case '5':
                        answer += "\u2075";
                        break;
                    case '6':
                        answer += "\u2076";
                        break;
                    case '7':
                        answer += "\u2077";
                        break;
                    case '8':
                        answer += "\u2078";
                        break;
                    case '9':
                        answer += "\u2079";
                        break;
                }
            }
            return answer;
        }

        private void DrawExons(Graphics g, float startPoint, float endPoint, int Top, float height, scalar scale, string name, Point limitRegion)
        {
            float drawHeight = height;
            float offsetHeight = 0;
            if (parameters.Reduced == true)
            {
                drawHeight /= 2.0f;
                offsetHeight = drawHeight / 2;
            }
            RectangleF shape = new RectangleF(startPoint, Top + offsetHeight, endPoint - startPoint, drawHeight);
            CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(parameters.SequenceExonColour[name]), parameters.Rounded, scale, 2, true);
            if (parameters.DrawExonBorders == true)
            {
                Pen pen = new Pen(Color.Black, scale.f[1]);
                CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2);
            }
        }

        private void DrawFrames(Graphics g, int Top, int height, float scalefactor, scalar scale, string name, Point limits)
        {
            Color[] frameColours = parameters.ReadingFrameColours;

            string allSequences = parameters.AllSequences;
            List<ReadingFrame> exons = parameters.ExonWithFrame[name];
            foreach (ReadingFrame exon in exons)
            {
                string sequence = allSequences.Substring(exon.start, exon.end + 1 - exon.start);
                int placeInAllsequences = allSequences.IndexOf(sequence);
                int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limits.X, placeInAllsequences));
                float startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                    ((placeInAllsequences - limits.X - numberofGapsbeforeSequence) * scalefactor);
                float endPoint = startPoint + (sequence.Length * scalefactor);

                RectangleF shape = new RectangleF(startPoint, Top, endPoint - startPoint, height);
                CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(frameColours[exon.frame - 5]), parameters.Rounded, scale, 2, true);
                if (parameters.DrawExonBorders == true)
                {
                    Pen pen = new Pen(Color.Black, scale.f[1]);
                    CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2);
                }
            }
        }

        private void DrawSpliceSites(Graphics g, int top, float bottom, scalar scale, float scalefactor, string name, string allSequences, Point limitRegion)
        {
            string[] names = new string[] { name };
            if (name == "All")
            { names = parameters.SequenceNames.ToArray(); }
            List<float> drawn = new List<float>();

            foreach (string nameGB in names)
            {
                if (parameters.Exons.ContainsKey(nameGB) == true)
                {
                    List<Point> exons = parameters.Exons[nameGB];
                    foreach (Point p in exons)
                    {
                        string sequence = parameters.getExonSequence(nameGB, p);
                        int placeInAllsequences = allSequences.IndexOf(sequence);
                        int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limitRegion.X, placeInAllsequences));
                        float startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                            ((placeInAllsequences - limitRegion.X - numberofGapsbeforeSequence) * scalefactor);
                        float endPoint = startPoint + (sequence.Length * scalefactor);
                        if (drawn.Contains(startPoint) == false)
                        {
                            Pen pen = new Pen(parameters.LineLimitColours[0], scale.scale * parameters.LimitsLineWidth);
                            pen.DashStyle = DashStyle.Custom;
                            pen.DashPattern = new float[] { 1, 1 };
                            g.DrawLine(pen, startPoint, top, startPoint, bottom);
                            drawn.Add(startPoint);
                        }
                        if (drawn.Contains(endPoint) == false)
                        {
                            Pen pen = new Pen(parameters.LineLimitColours[1], scale.scale * parameters.LimitsLineWidth);
                            pen.DashStyle = DashStyle.Custom;
                            pen.DashPattern = new float[] { 1, 1 };
                            g.DrawLine(pen, endPoint, top, endPoint, bottom);
                            drawn.Add(endPoint);
                        }
                    }
                }
            }
        }

        private void DrawORFSites(Graphics g, int top, float bottom, scalar scale, float scalefactor, string name, string allSequences, Point limitRegion)
        {
            string[] names = new string[] { name };
            if (name == "All")
            { names = parameters.SequenceNames.ToArray(); }
            List<float> drawn = new List<float>();

            foreach (string nameGB in names)
            {
                if (parameters.CDSs.ContainsKey(nameGB) == true && parameters.Exons.ContainsKey(nameGB) == true)
                {
                    Point orf = parameters.CDSs[nameGB];
                    List<Point> exons = parameters.Exons[nameGB];
                    foreach (Point exon in exons)
                    {
                        if (exon.X <= orf.X && exon.Y >= orf.X)
                        {
                            string sequence = parameters.getExonSequence(nameGB, exon);
                            int placeInAllsequences = allSequences.IndexOf(sequence);
                            int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limitRegion.X, placeInAllsequences));
                            float startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                                ((placeInAllsequences - limitRegion.X - numberofGapsbeforeSequence) * scalefactor);
                            float endPoint = startPoint + (sequence.Length * scalefactor);
                            int distanceFromStart = orf.X - exon.X + 1;
                            float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                            float orfLine = startPoint + (offset * (endPoint - startPoint));
                            if (drawn.Contains(startPoint) == false)
                            {
                                Pen pen = new Pen(parameters.LineLimitColours[2], scale.scale * parameters.LimitsLineWidth);
                                pen.DashStyle = DashStyle.Custom;
                                pen.DashPattern = new float[] { 1, 1 };
                                g.DrawLine(pen, orfLine, top, orfLine, bottom);
                                drawn.Add(startPoint);
                            }
                        }

                        if (exon.X <= orf.Y && exon.Y >= orf.Y)
                        {
                            string sequence = parameters.getExonSequence(nameGB, exon);
                            int placeInAllsequences = allSequences.IndexOf(sequence);
                            int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limitRegion.X, placeInAllsequences));
                            float startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                                ((placeInAllsequences - limitRegion.X - numberofGapsbeforeSequence) * scalefactor);
                            float endPoint = startPoint + (sequence.Length * scalefactor);
                            int distanceFromStart = orf.Y - exon.X + 1;
                            float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                            float orfLine = startPoint + (offset * (endPoint - startPoint));
                            if (drawn.Contains(endPoint) == false)
                            {
                                Pen pen = new Pen(parameters.LineLimitColours[3], scale.scale * parameters.LimitsLineWidth);
                                pen.DashStyle = DashStyle.Custom;
                                pen.DashPattern = new float[] { 1, 1 };
                                g.DrawLine(pen, orfLine, top, orfLine, bottom);
                                drawn.Add(endPoint);
                            }
                        }
                    }
                }
            }
        }

        private void DrawOrf(Graphics g, float startPoint, float endPoint, int Top, int height, scalar scale, string name, Point exon, Point limitRegion)
        {
            if (parameters.CDSs.ContainsKey(name) == true)
            {
                RectangleF shape = new RectangleF(-1, -1, 1, 1);
                SolidBrush orfColor;
                if (parameters.ShowOrf == true)
                { orfColor = new SolidBrush(parameters.SequenceCDSColour[name]); }
                else
                { orfColor = new SolidBrush(parameters.SequenceExonColour[name]); }

                Point orf = parameters.CDSs[name];
                Pen pen = new Pen(Color.Black, scale.f[1]);
                if (exon.X >= orf.X && exon.Y <= orf.Y)
                {
                    shape = new RectangleF(startPoint, Top, (endPoint - startPoint), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, orfColor, parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
                else if (exon.X <= orf.X && exon.Y >= orf.X)// orf starts in exon
                {
                    int distanceFromStart = orf.X - exon.X + 1;
                    float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                    float orfStart = startPoint + (offset * (endPoint - startPoint));
                    shape = new RectangleF(orfStart, Top, (endPoint - orfStart), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, orfColor, parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
                else if (exon.X <= orf.Y && exon.Y >= orf.Y)// orf Ends in exon
                {
                    int distanceFromStart = orf.Y - exon.X;
                    float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                    float orfEnd = startPoint + (offset * (endPoint - startPoint));
                    shape = new RectangleF(startPoint, Top, (orfEnd - startPoint), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, orfColor, parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }

                if (parameters.ShowOrf == false)
                {
                    if (exon.X < orf.X || exon.Y > orf.Y)
                    {
                        float drawHeight = height;
                        float offsetHeight = 0;
                        if (parameters.Reduced == true)
                        {
                            drawHeight /= 2.0f;
                            offsetHeight = drawHeight / 2;
                        }
                        if (exon.X < orf.X)
                        { g.DrawLine(new Pen(parameters.SequenceExonColour[name], scale.i[2]), shape.X, Top + offsetHeight, shape.X, Top + offsetHeight + drawHeight); }
                        if (exon.Y > orf.Y)
                        { g.DrawLine(new Pen(parameters.SequenceExonColour[name], scale.i[2]), shape.Right, Top + offsetHeight, shape.Right, Top + offsetHeight + drawHeight); }
                    }
                }
            }
        }

        private int DrawMscellaneousFeaturesGB(Graphics g, Graphics gLabels, int Top, int height, scalar scale, float scalefactor, List<string> named, Point limitRegion, Dictionary<string, int> legends)
        {
            string allSequences = parameters.AllSequences;
            foreach (string key in parameters.MiscellaneousFeatureGBSet.Keys)
            {
                List<mRNADisplayMiscFeature> set = parameters.MiscellaneousFeatureGBSet[key];
                string name = key.Substring(key.IndexOf("#") + 1);
                string label = key.Substring(0, key.IndexOf("#"));
                if (parameters.LegendsLocation != DrawLabels.none && named.Contains(Top.ToString() + ":" + label) == false)
                {
                    named.Add(Top.ToString() + ":" + label);
                    string display = label;
                    if (legends.ContainsKey(name) == true)
                    { display += AddSupercript(legends[name]); }
                    Top = DrawFeatureLabels(g, gLabels, parameters.FeatureFont, display, parameters.DrawingArea.Width, scale.i[5], Top, scale, key);
                }

                parameters.FeatureRows[key] = Top;

                List<Point> exons = parameters.Exons[name];
                foreach (Point exon in exons)
                {
                    string sequence = parameters.getExonSequence(name, exon);
                    int placeInAllsequences = allSequences.IndexOf(sequence);
                    int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limitRegion.X, placeInAllsequences));
                    float startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                        ((placeInAllsequences - limitRegion.X - numberofGapsbeforeSequence) * scalefactor);
                    float endPoint = startPoint + (sequence.Length * scalefactor);
                    Top = DrawMiscellaneousFeaturesGB(g, startPoint, endPoint, Top, height, scale, set, exon);
                }

                Top += scale.i[25];
            }
            return Top;
        }

        private int DrawMiscellaneousFeaturesGB(Graphics g, float startPoint, float endPoint, int Top, int height, scalar scale, List<mRNADisplayMiscFeature> set, Point exon)
        {
            if (set.Count == 0) { return Top; }
            Pen pen = new Pen(Color.Black, scale.f[1]);
            RectangleF shape = new RectangleF(-1, -1, 1, 1);
            foreach (mRNADisplayMiscFeature mf in set)
            {
                if (exon.X >= mf.StartPoint && exon.Y <= mf.EndPoint)
                {
                    shape = new RectangleF(startPoint, Top, (endPoint - startPoint), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(mf.Color), parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
                if (exon.X <= mf.StartPoint && exon.Y >= mf.EndPoint)
                {
                    int distanceFromStart = mf.StartPoint - exon.X + 1;
                    float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                    float orfStart = startPoint + (offset * (endPoint - startPoint));
                    distanceFromStart = mf.EndPoint - exon.X + 1;
                    offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                    float orfEnd = startPoint + (offset * (endPoint - startPoint));
                    shape = new RectangleF(orfStart, Top, (orfEnd - orfStart), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(mf.Color), parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
                else if (exon.X <= mf.StartPoint && exon.Y >= mf.StartPoint)// orf starts in exon
                {
                    int distanceFromStart = mf.StartPoint - exon.X + 1;
                    float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                    float orfStart = startPoint + (offset * (endPoint - startPoint));
                    shape = new RectangleF(orfStart, Top, (endPoint - orfStart), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(mf.Color), parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
                else if (exon.X <= mf.EndPoint && exon.Y >= mf.EndPoint)// orf starts in exon
                {
                    int distanceFromStart = mf.EndPoint - exon.X;
                    float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                    float orfEnd = startPoint + (offset * (endPoint - startPoint));
                    shape = new RectangleF(startPoint, Top, (orfEnd - startPoint), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(mf.Color), parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
            }
            return Top;
        }

        private int DrawMscellaneousFeaturesUniProt(Graphics g, Graphics gLabels, int Top, int height, scalar scale, float scalefactor, List<string> named, Point limitRegion, Dictionary<string, int> legends)
        {
            string allSequences = parameters.AllSequences;
            foreach (string key in parameters.MiscellaneousFeatureUniprotSet.Keys)
            {
                List<ProteinDomainSubFeature> set = parameters.MiscellaneousFeatureUniprotSet[key];
                string[] names = key.Split('#');
                string name = names[2];
                string Uniprot = names[1];
                string label = names[0];
                mRNAUniProtAlignment offsets = GetUniProtLimits(name, Uniprot);
                if (offsets.StartOfmRNAAA > -1)
                {
                    if (parameters.LegendsLocation != DrawLabels.none && named.Contains(Top.ToString() + ":" + label) == false)
                    {
                        named.Add(Top.ToString() + ":" + label);
                        string display = label;
                        if (legends.ContainsKey(name) == true)
                        { display += AddSupercript(legends[name]); }
                        Top = DrawFeatureLabels(g, gLabels, parameters.FeatureFont, display, parameters.DrawingArea.Width, scale.i[5], Top, scale, key);
                    }

                    parameters.FeatureRows[label] = Top;

                    if (parameters.Exons.ContainsKey(name) == true)
                    {
                        List<Point> exons = parameters.Exons[name];
                        foreach (Point exon in exons)
                        {
                            string sequence = parameters.getExonSequence(name, exon);
                            int placeInAllsequences = allSequences.IndexOf(sequence);
                            int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limitRegion.X, placeInAllsequences));
                            float startPoint = scale.scale * (parameters.LabelWidth + 15) + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                                ((placeInAllsequences - limitRegion.X - numberofGapsbeforeSequence) * scalefactor);
                            float endPoint = startPoint + (sequence.Length * scalefactor);
                            Top = DrawMiscellaneousFeaturesUniProt(g, startPoint, endPoint, Top, height, scale, set, exon, offsets);
                        }
                    }
                    Top += scale.i[25];
                }
            }
            return Top;
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

        private int DrawMiscellaneousFeaturesUniProt(Graphics g, float startPoint, float endPoint, int Top, int height, scalar scale, List<ProteinDomainSubFeature> set, Point exon, mRNAUniProtAlignment offsets)
        {
            if (set.Count == 0) { return Top; }
            Pen pen = new Pen(Color.Black, scale.f[1]);
            RectangleF shape = new RectangleF(-1, -1, 1, 1);
            foreach (ProteinDomainSubFeature pdsfRaw in set)
            {
                if (pdsfRaw.StartPointAA >= offsets.StartOfUniprot && pdsfRaw.EndPointAA <= offsets.EndOfUniprot)
                {
                    int aaOffset = (offsets.StartOfUniprot - 1) * 3;
                    Point pdsf = new Point(pdsfRaw.StartPoint + offsets.StartOfmRNAAA - aaOffset, pdsfRaw.EndPoint + offsets.StartOfmRNAAA - aaOffset);
                    if (exon.X >= pdsf.X && exon.Y <= pdsf.Y)
                    {
                        shape = new RectangleF(startPoint, Top, (endPoint - startPoint), height);
                        CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(pdsfRaw.Colour), parameters.Rounded, scale, 2, true);
                        if (parameters.DrawExonBorders == true)
                        { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                    }
                    if (exon.X <= pdsf.X && exon.Y >= pdsf.Y)
                    {
                        int distanceFromStart = pdsf.X - exon.X + 1;
                        float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                        float orfStart = startPoint + (offset * (endPoint - startPoint));
                        distanceFromStart = pdsf.Y - exon.X + 1;
                        offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                        float orfEnd = startPoint + (offset * (endPoint - startPoint));
                        shape = new RectangleF(orfStart, Top, (orfEnd - orfStart), height);
                        CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(pdsfRaw.Colour), parameters.Rounded, scale, 2, true);
                        if (parameters.DrawExonBorders == true)
                        { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                    }
                    else if (exon.X <= pdsf.X && exon.Y >= pdsf.X)// orf starts in exon
                    {
                        int distanceFromStart = pdsf.X - exon.X + 1;
                        float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                        float orfStart = startPoint + (offset * (endPoint - startPoint));
                        shape = new RectangleF(orfStart, Top, (endPoint - orfStart), height);
                        CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(pdsfRaw.Colour), parameters.Rounded, scale, 2, true);
                        if (parameters.DrawExonBorders == true)
                        { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                    }
                    else if (exon.X <= pdsf.Y && exon.Y >= pdsf.Y)// orf starts in exon
                    {
                        int distanceFromStart = pdsf.Y - exon.X;
                        float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                        float orfEnd = startPoint + (offset * (endPoint - startPoint));
                        shape = new RectangleF(startPoint, Top, (orfEnd - startPoint), height);
                        CommonGraphicTasks.DrawRectangle(g, shape, new SolidBrush(pdsfRaw.Colour), parameters.Rounded, scale, 2, true);
                        if (parameters.DrawExonBorders == true)
                        { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                    }
                }
            }
            return Top;
        }

        private int DrawMiscellaneousFeaturesInterProScan(Graphics g, Graphics gLabels, int Top, int height, scalar scale, float scalefactor, List<string> named, Point limitRegion, Dictionary<string, int> legends)
        {
            string allSequences = parameters.AllSequences;
            foreach (string key in parameters.Selected.Keys)
            {
                AlignmentDomainFeature feature = parameters.Selected[key];
                string name = feature.BaseSequenceName;
                if (parameters.CDSs.ContainsKey(name) == true)
                {
                    string text = key;
                    if (parameters.LegendsLocation != DrawLabels.none && named.Contains(Top.ToString() + ":" + key) == false)
                    {
                        named.Add(Top.ToString() + ":" + key);
                        string display = feature.DisplayName;
                        if (legends.ContainsKey(name) == true)
                        { display += AddSupercript(legends[name]); }
                        Top = DrawFeatureLabels(g, gLabels, parameters.FeatureFont, display, parameters.DrawingArea.Width, scale.i[5], Top, scale, key);
                    }

                    parameters.FeatureRows[feature.DisplayName] = Top;

                    int mRNAStart = parameters.CDSs[name].X + ((feature.Start - 1) * 3);
                    int mRNAEnd = parameters.CDSs[name].X + ((feature.End - 1) * 3);
                    Point boundaries = new Point(mRNAStart, mRNAEnd);
                    List<Point> exonPoints = parameters.Exons[name];
                    foreach (Point exon in exonPoints)
                    {
                        string sequence = parameters.getExonSequence(name, exon);
                        int placeInAllsequences = allSequences.IndexOf(sequence);
                        int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limitRegion.X, placeInAllsequences));
                        float startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                            ((placeInAllsequences - limitRegion.X - numberofGapsbeforeSequence) * scalefactor);
                        float endPoint = startPoint + (sequence.Length * scalefactor);

                        { DrawMiscellaneousFeaturesInterProScan(g, startPoint, endPoint, feature, Top, scale.i[20], scale, name, exon, limitRegion, boundaries); }
                    }
                    Top += scale.i[25];
                }
            }
            return Top;
        }

        private void DrawMiscellaneousFeaturesInterProScan(Graphics g, float startPoint, float endPoint, AlignmentDomainFeature feature, int Top, int height, scalar scale, string name, Point exon, Point limitRegion, Point boundaries)
        {
            if (parameters.CDSs.ContainsKey(name) == true)
            {
                RectangleF shape = new RectangleF(-1, -1, 1, 1);
                SolidBrush orfColor = new SolidBrush(feature.FillColour);

                Pen pen = new Pen(Color.Black, scale.f[1]);
                if (exon.X >= boundaries.X && exon.Y <= boundaries.Y)
                {
                    shape = new RectangleF(startPoint, Top, (endPoint - startPoint), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, orfColor, parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
                else if (exon.X <= boundaries.X && exon.Y >= boundaries.X)// orf starts in exon
                {
                    int distanceFromStart = boundaries.X - exon.X + 1;
                    float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                    float orfStart = startPoint + (offset * (endPoint - startPoint));
                    shape = new RectangleF(orfStart, Top, (endPoint - orfStart), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, orfColor, parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
                else if (exon.X <= boundaries.Y && exon.Y >= boundaries.Y)// orf Ends in exon
                {
                    int distanceFromStart = boundaries.Y - exon.X;
                    float offset = (float)distanceFromStart / (exon.Y - exon.X + 1);
                    float orfEnd = startPoint + (offset * (endPoint - startPoint));
                    shape = new RectangleF(startPoint, Top, (orfEnd - startPoint), height);
                    CommonGraphicTasks.DrawRectangle(g, shape, orfColor, parameters.Rounded, scale, 2, true);
                    if (parameters.DrawExonBorders == true)
                    { CommonGraphicTasks.DrawBordersRectangle(g, shape, pen, parameters.Rounded, scale, 2); }
                }
            }
        }

        private int DrawBindingSites(Graphics g, Graphics gLabels, int Top, int height, scalar scale, float scalefactor, List<string> named, Dictionary<string, int> legends, string allSequences, Point limits, Bitmap bmp)
        {
            int currentHeight = Top;
            foreach (string key in parameters.BindingSites.Keys)
            {
                string[] bits = key.Split('#');
                string name = bits[0];

                if (parameters.Exons.ContainsKey(name) == false) continue;
                List<Point> exonPoints = parameters.Exons[name];
                if (parameters.LegendsLocation != DrawLabels.none)
                {
                    string display = bits[1];
                    if (name == cboFrame.Text)
                    { display += "*"; }

                    if (legends.ContainsKey(name) == true)
                    { display += AddSupercript(legends[name]); }
                    Top = DrawFeatureLabels(g, gLabels, parameters.IDFont, display, bmp.Width, scale.i[5], Top, scale, name);
                }

                List<Point> bindingSites = parameters.BindingSites[key];

                parameters.FeatureRows[parameters.SequenceDisplayNames[name]] = Top;
                foreach (Point bs in bindingSites)
                {
                    foreach (Point exon in exonPoints)
                    {                       
                        string sequence = parameters.getExonSequence(name, exon);
                        int placeInAllsequences = allSequences.IndexOf(sequence);
                        int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limits.X, placeInAllsequences));
                        float startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                            ((placeInAllsequences - limits.X - numberofGapsbeforeSequence) * scalefactor);
                        float endPoint = startPoint + (sequence.Length * scalefactor);

                        bool draw = false;
                        if (exon.X <= bs.X && exon.Y >= bs.Y)
                        {
                            startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                            ((placeInAllsequences + (bs.X - exon.X) - limits.X - numberofGapsbeforeSequence) * scalefactor);
                            endPoint = startPoint + ((bs.Y-bs.X) * scalefactor);
                            draw = true;
                        }
                        else if (exon.X <= bs.X && exon.Y >= bs.X)
                        {
                            startPoint = scale.i[parameters.LabelWidth] + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                               ((placeInAllsequences + (bs.X - exon.X) - limits.X - numberofGapsbeforeSequence) * scalefactor);
                            draw = true;
                        }
                        else if (exon.X <= bs.Y && exon.Y >= bs.Y)
                        {
                            endPoint = startPoint + ((exon.X - bs.Y) * scalefactor);
                            draw = true;
                        }
                        if (draw == true)
                        {
                            RectangleF r = new RectangleF(startPoint, Top + ((float)height / 3), (endPoint - startPoint), ((float)height / 3));
                            CommonGraphicTasks.DrawRectangle(g, r, Brushes.Red, parameters.Rounded, scale, 2, true);
                            if (parameters.DrawExonBorders == true)
                            { CommonGraphicTasks.DrawBordersRectangle(g, r, Pens.Black, parameters.Rounded, scale, 2); }
                            Top += scale.i[25];
                        }
                    }
                }

                if (parameters.ExonWithFrame.Count > 0)
                { DrawFrames(g, height, scale.i[20], scalefactor, scale, name, limits); }

                //height += scale.i[25];
            }
            Top += scale.i[10] +30;
            FillMask(g, currentHeight, Top, scale, bmp.Width);
            return Top;
        }

        private int DrawSizeMarkers(Graphics g, scalar scale, float scalefactor, float Top, int imageWidth, Point limitRegion)
        {
            float initialTop = Top;
            double radians = CommonGraphicTasks.GetAngleInRadians(parameters.AngleOfrotation);
            SizeF textWidthAndHeight = GetWidthAndHeightOfRotatedText(g, parameters.MajorTickMark, parameters.AllSequences.Length, radians);
            int extraHeight = scale.i[10];

            int minorTickHeight = scale.i[3];
            int majorTickHeight = scale.i[10];
            if (parameters.SizeMarkerLocation == SizeMarkerLocation.Bottom)
            {
                Top += scale.i[10];
                minorTickHeight = -minorTickHeight;
                majorTickHeight = -majorTickHeight;
                extraHeight = scale.i[12] + (int)(textWidthAndHeight.Height + 0.5f);
                RectangleF clean = new RectangleF(0, initialTop - scale.i[1], parameters.DrawingArea.Width * scale.scale, extraHeight + scale.i[10]);
                g.FillRectangle(Brushes.White, clean);
            }
            else if (parameters.SizeMarkerLocation == SizeMarkerLocation.Top)
            {
                Top += scale.i[10] + textWidthAndHeight.Height;
                RectangleF clean = new RectangleF(0, initialTop - scale.i[4], parameters.DrawingArea.Width * scale.scale, extraHeight + scale.i[10] + textWidthAndHeight.Height);
                g.FillRectangle(Brushes.White, clean);
            }


            Pen pen = new Pen(Color.Black, scale.scale * 1.25f);
            string gappedSequence = parameters.AllSequences;
            int index = limitRegion.X;

            int lastIndex = index;
            float startPoint = (parameters.LabelWidth + 15) * scale.scale;
            while (index > -1)
            {
                index = gappedSequence.IndexOf(" ", index + 1);
                if (index > limitRegion.Y) { index = -1; }
                if (index > -1)
                {
                    float endPoint = startPoint + ((index - 1 - lastIndex) * scalefactor);
                    g.DrawLine(pen, startPoint, Top, endPoint, Top);
                    startPoint = endPoint + (parameters.IntronGap * scale.scale);
                    lastIndex = index;
                }
                else
                {
                    float endPoint = startPoint + ((gappedSequence.Length - lastIndex) * scalefactor);
                    g.DrawLine(pen, startPoint, Top, endPoint, Top);
                }
            }
            g.FillRectangle(Brushes.White, new RectangleF(imageWidth - scale.i[15], Top - scale.i[2], scale.i[15], scale.i[5]));
            int labelOffset = (int)((parameters.LabelWidth + 15) * scale.scale);
            int rotation = 0; int heightOffset = 0;
            if (parameters.SizeMarkerLocation == SizeMarkerLocation.Top)
            { rotation = parameters.AngleOfrotation; heightOffset = -scale.i[5]; }
            else
            { rotation = -parameters.AngleOfrotation; heightOffset = scale.i[35]; }

            float lastTextEnd = 0;
            int displayOffSet = 0;
            if (parameters.CoodindatesStartAt1bp == true)
            {
                displayOffSet = 0;
                WritePlaceText(g, "1 bp", rotation, labelOffset, Top + heightOffset, scale);
                lastTextEnd = labelOffset + GetWidthAndHeightOfRotatedText(g, radians, "1 bp").Width;
            }
            else
            {
                displayOffSet = limitRegion.X;
                WritePlaceText(g, limitRegion.X.ToString() + " bp", rotation, labelOffset, Top + heightOffset, scale);
                lastTextEnd = labelOffset + GetWidthAndHeightOfRotatedText(g, radians, limitRegion.X.ToString() + " bp").Width;
            }

            g.DrawLine(pen, (parameters.LabelWidth + 15) * scale.scale, Top, (parameters.LabelWidth + 15) * scale.scale, Top - majorTickHeight);
            int sequenceplace = displayOffSet;
            for (int place = limitRegion.X + 1; place < limitRegion.Y; place++)
            {
                sequenceplace++; ;
                if (sequenceplace % parameters.MinorTickMark == 0 || sequenceplace % parameters.MajorTickMark == 0)
                {
                    int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limitRegion.X, place));
                    float point = labelOffset + (numberofGapsbeforeSequence * parameters.IntronGap * scale.scale) + ((place - limitRegion.X - numberofGapsbeforeSequence) * scalefactor);

                    if (sequenceplace % parameters.MajorTickMark == 0)
                    {
                        g.SmoothingMode = SmoothingMode.None;
                        g.DrawLine(pen, point, Top, point, Top - majorTickHeight);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        if (point > lastTextEnd + scale.i[5] && place > 0)
                        {
                            lastTextEnd = point + GetWidthAndHeightOfRotatedText(g, radians, (sequenceplace + 0).ToString() + " bp").Width;
                            if (lastTextEnd < imageWidth + scale.i[5])
                            { WritePlaceText(g, sequenceplace.ToString() + " bp", rotation, point, Top + heightOffset, scale); }
                        }
                    }
                    else
                    {
                        g.SmoothingMode = SmoothingMode.None;
                        g.DrawLine(pen, point, Top, point, Top - minorTickHeight);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                    }
                }
            }
            return (int)Top + extraHeight;
        }

        private SizeF GetWidthAndHeightOfRotatedText(Graphics g, int interval, int sequenceLength, double radians)
        {
            SizeF longest = g.MeasureString("1 bp", new System.Drawing.Font("Arial", 10));
            for (int index = interval; index < sequenceLength; index += interval)
            {
                SizeF thisLength = GetWidthAndHeightOfRotatedText(g, radians, index.ToString() + " bp");
                if (thisLength.Width > longest.Width) { longest.Width = thisLength.Width; }
                if (thisLength.Height > longest.Height) { longest.Height = thisLength.Height; }
            }
            return longest;
        }

        private SizeF GetWidthAndHeightOfRotatedText(Graphics g, double radians, string text)
        {
            SizeF longest = g.MeasureString(text, new System.Drawing.Font("Arial", 10));

            SizeF answer = new SizeF();
            if (radians == 0.0f)
            { answer = longest; }
            else
            {
                answer.Width = (float)(Math.Cos(radians) * longest.Width) - (float)(Math.Sin(radians) * longest.Height);
                answer.Height = (float)(Math.Sin(radians) * longest.Width) + (float)(Math.Cos(radians)) * longest.Height;
            }
            return answer;
        }

        private void WritePlaceText(Graphics g, string text, float degrees, float X, float Y, scalar scale)
        {
            System.Drawing.Font f = new System.Drawing.Font("Arial", 10);
            SizeF adjusted;

            if (parameters.SizeMarkerLocation == SizeMarkerLocation.Top)
            {
                X -= scale.i[05];
                Y -= scale.i[16];
                g.TranslateTransform(X, Y);
                g.RotateTransform(-degrees);
                degrees += 113;
                SizeF adjust = new SizeF(scale.f[4], -scale.f[4]);
                adjusted = CommonGraphicTasks.RotateBy(adjust, CommonGraphicTasks.GetAngleInRadians(-(int)(degrees * 0.5)));
                g.DrawString(text, f, Brushes.Black, adjusted.Width, adjusted.Height);
            }
            else
            {
                X += scale.i[2];
                Y -= scale.i[28];
                g.TranslateTransform(X, Y);
                g.RotateTransform(-degrees);
                degrees += 45;
                SizeF adjust = new SizeF(scale.f[5], scale.f[5]);
                adjusted = CommonGraphicTasks.RotateBy(adjust, CommonGraphicTasks.GetAngleInRadians((int)degrees * 2));
                g.DrawString(text, f, Brushes.Black, adjusted.Width, adjusted.Height);
            }
            g.ResetTransform();
        }

        private Bitmap Blank()
        {
            Size imageArea = mRNADV.GetDrawingArea();
            Bitmap blank = new Bitmap((int)(imageArea.Width), (int)(imageArea.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(blank);
            g.Clear(Color.White);
            return blank;
        }

        #endregion


        #region stuff

        private void DrawCylinder(Graphics g)
        {

            // Cylinder dimensions
            float x = 50, y = 250;
            float width = 200, height = 20;
            float ellipseWidth = 5;
            float ellipseHalfWidth = ellipseWidth / 2;
            // Brushes and pens
            Brush bodyBrush = Brushes.LightSteelBlue;
            Brush topBrush = Brushes.SkyBlue;
            Brush bottomBrush = Brushes.SlateGray;
            Pen outlinePen = Pens.Black;

            // Draw bottom ellipse (simulated 3D arc)
            g.FillEllipse(bodyBrush, x, y, ellipseWidth, height);
            g.DrawEllipse(outlinePen, x, y, ellipseWidth, height);
            // Draw body rectangle
            g.FillRectangle(bodyBrush, x + ellipseHalfWidth, y, width - ellipseWidth, height);
            g.DrawLine(outlinePen, x + ellipseHalfWidth, y, x + width - ellipseHalfWidth, y);
            g.DrawLine(outlinePen, x + ellipseHalfWidth, y + height, x + width - ellipseHalfWidth, y + height);
            // Draw top ellipse
            g.FillEllipse(topBrush, x + width - ellipseWidth, y, ellipseWidth, height);
            g.DrawEllipse(outlinePen, x + width - ellipseWidth, y, ellipseWidth, height);

        }

        private void DrawSheet(Graphics g)
        {
            Rectangle bounds = new Rectangle(455, 242, 200, 15);
            float amplitude = 2; // height of wave
            float frequency = 8.0f; // number of waves
            float step = 1;       // pixel step between points

            int numberOfPoints = bounds.Width / (int)step;
            PointF[] points = new PointF[numberOfPoints * 2];
            for (int i = 0; i < numberOfPoints; i++)
            {
                float x = (i * step) + bounds.X;
                float y = (bounds.Height / 2) + bounds.Y + (float)(Math.Sin((double)i / frequency) * amplitude);
                points[i] = new PointF(x, y);
                points[(numberOfPoints * 2) - (i + 1)] = new PointF(x, y + 15);
            }

            Pen outlinePen = Pens.Black;
            Brush topBrush = Brushes.LightPink;
            g.FillPolygon(topBrush, points);
            g.DrawPolygon(outlinePen, points);
        }

        private void DrawStrand(Graphics g)
        {
            Rectangle bounds = new Rectangle(255, 254, 200, 05);
            float amplitude = 2; // height of wave
            float frequency = 4.0f; // number of waves
            float step = 1;       // pixel step between points

            int numberOfPoints = bounds.Width / (int)step;
            PointF[] points = new PointF[numberOfPoints * 2];
            for (int i = 0; i < numberOfPoints; i++)
            {
                float x = (i * step) + bounds.X;
                float y = (bounds.Height / 2) + bounds.Y + (float)(Math.Sin((double)i / frequency) * amplitude);
                points[i] = new PointF(x, y);
                points[(numberOfPoints * 2) - (i + 1)] = new PointF(x, y + 5);
            }

            Pen outlinePen = Pens.Black;
            Brush topBrush = Brushes.LightGray;
            g.FillPolygon(topBrush, points);
            g.DrawPolygon(outlinePen, points);
        }

        #endregion


        private void btnSearchInternet_Click(object sender, EventArgs e)
        {
            mRNADomainSearchUpdate mRNADSU = new mRNADomainSearchUpdate(parameters);
            if (mRNADSU.ShowDialog() == DialogResult.OK)
            {
                parameters.MiscellaneousFeatureUniprotAll = mRNADSU.GetmProteinDomainFeature;
                parameters.MRNAToProteinKey = mRNADSU.GetmRNAToProteinKey;
                parameters.SearchString = mRNADSU.ReturnString;
                btnCreateUniProtsets.Enabled = true;
                btnSaveUniProtKB.Enabled = true;
                CompareUniProtSequenceTomRNAAASequence();
            }
        }

        private void btnImportUniprotKB_Click(object sender, EventArgs e)
        {
            string fileName = FileString.OpenAs("Select the UniProt search results", "Text file (*.txt)|*.txt");
            if (System.IO.File.Exists(fileName) == false) { return; }
            StreamReader sf = null;

            btnSaveUniProtKB.Enabled = false;
            btnCreateUniProtsets.Enabled = false;
            try
            {
                sf = new StreamReader(fileName);
                Dictionary<string, List<string>> temp = new Dictionary<string, List<string>>();
                while (sf.Peek() > 0)
                {
                    string line = sf.ReadLine();

                    if (line.Contains("|") == true)
                    {
                        string[] items = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
                        if (temp.ContainsKey(items[0]) == false)
                        {
                            List<string> list = new List<string>();
                            temp.Add(items[0], list);
                        }
                        for (int index = 1; index < items.Length; index++)
                        { temp[items[0]].Add(items[index]); }
                    }
                    else if (line.Contains("\"") == false)
                    {
                        string baseName = line.Trim();
                        if (sf.Peek() > 0)
                        {
                            string data = sf.ReadLine();
                            string sequence = DomianIdentifier.getSequenceFromEBIResult(data);
                            ProteinDomainFeature pdf = new ProteinDomainFeature(data, sequence, baseName);
                            if (parameters.MiscellaneousFeatureUniprotAll.ContainsKey(baseName) == true)
                            { parameters.MiscellaneousFeatureUniprotAll[baseName] = pdf; }
                            else { parameters.MiscellaneousFeatureUniprotAll.Add(baseName, pdf); }
                        }
                    }
                }
                if (temp != null)
                { parameters.MRNAToProteinKey = temp; }
                CompareUniProtSequenceTomRNAAASequence();
                btnCreateUniProtsets.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show("An error occured: " + ex.Message, "Error"); }
            finally
            {
                sf?.Close();
            }
        }

        private void btnSaveUniProtKB_Click(object sender, EventArgs e)
        {
            string fileName = FileString.SaveAs("Save UniProt search results", "Text file (*.txt)|*.txt");
            if (fileName == "Cancel") { return; }
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(fileName);
                foreach (string key in parameters.MRNAToProteinKey.Keys)
                {
                    string answer = "";
                    foreach (string name in parameters.MRNAToProteinKey[key])
                    { answer += "|" + name; }

                    sw.WriteLine(key + answer);
                }
                sw.WriteLine(parameters.SearchString);
                sw.Close();
            }
            catch (Exception ex)
            { MessageBox.Show("An error occured: " + ex.Message, "Error"); }
            finally
            {
                sw?.Close();
            }
        }

        private void CompareUniProtSequenceTomRNAAASequence()
        {
            foreach (string name in parameters.MRNAToProteinKey.Keys)
            {
                if (parameters.SequenceAminoAcid.ContainsKey(name) == true)
                {
                    string mRNAAA = parameters.SequenceAminoAcid[name].ToUpper();
                    foreach (string uniProtID in parameters.MRNAToProteinKey[name])
                    {
                        string uniProtAA = parameters.MiscellaneousFeatureUniprotAll[uniProtID].Sequence.ToUpper();
                        string AlignedmRNA = ""; string AlignedUniprot = "";
                        (AlignedmRNA, AlignedUniprot) = SequenceAlignment.AlignProteinLocal(mRNAAA, uniProtAA);
                        mRNAUniProtAlignment alignment = makeAlignmentUniProtLimits(mRNAAA, AlignedmRNA, uniProtAA, AlignedUniprot);
                        string key = name + "#" + uniProtID;
                        if (parameters.MRNAUniProtAlignmentLimits == null)
                        { parameters.MRNAUniProtAlignmentLimits = new Dictionary<string, mRNAUniProtAlignment>(); }
                        if (parameters.MRNAUniProtAlignmentLimits.ContainsKey(key) == false)
                        { parameters.MRNAUniProtAlignmentLimits.Add(key, alignment); }
                        else
                        { parameters.MRNAUniProtAlignmentLimits[key] = alignment; }
                    }
                }
            }
        }

        private mRNAUniProtAlignment makeAlignmentUniProtLimits(string mRNAAA, string AlignedmRNA, string uniProtAA, string AlignedUniprot)
        {
            string cleanAlignedmRNA = AlignedmRNA.Replace("-", "");
            string cleanAlignedUniprot = AlignedUniprot.Replace("-", "");

            mRNAUniProtAlignment alignment = new mRNAUniProtAlignment();
            alignment.StartOfmRNAAA = mRNAAA.IndexOf(cleanAlignedmRNA);
            alignment.StartOfUniprot = uniProtAA.IndexOf(cleanAlignedUniprot);
            alignment.EndOfmRNAA = alignment.StartOfmRNAAA + AlignedmRNA.Length;
            alignment.EndOfUniprot = alignment.StartOfUniprot + AlignedUniprot.Length;
            return alignment;
        }

        private void btnCreateUniProtsets_Click(object sender, EventArgs e)
        {
            if (parameters == null) { return; }
            Dictionary<string, List<ProteinDomainSubFeature>> Sets = parameters.MiscellaneousFeatureUniprotSet;

            MiscellaneousFeatureUniprot mRNAMFSU = new MiscellaneousFeatureUniprot(Sets, parameters);
            if (mRNAMFSU.ShowDialog() == DialogResult.OK)
            {
                parameters.MiscellaneousFeatureUniprotSet = mRNAMFSU.GetSets;
                ReDraw();
            }
        }


        #region interface actions       

        private void InitalizeFormatAndDisplayTab()
        {
            cboCoordinates.SelectedIndex = 0;

            cboSpliceSites.Items.Clear();
            cboSpliceSites.Items.Add("None");
            cboSpliceSites.Items.Add("All");

            cboORFLimits.Items.Clear();
            cboORFLimits.Items.Add("None");
            cboORFLimits.Items.Add("All");

            cboFrame.Items.Clear();
            cboFrame.Items.Add("None");

            if (parameters != null)
            {
                cboORFLimits.Items.AddRange(parameters.SequenceNames.ToArray());
                cboSpliceSites.Items.AddRange(parameters.SequenceNames.ToArray());
                cboFrame.Items.AddRange(parameters.SequenceNames.ToArray());
            }
            cboFrame.SelectedIndex = 0;
            cboORFLimits.SelectedIndex = 0;
            cboSpliceSites.SelectedIndex = 0;
            cboCoordinates.SelectedIndex = 1;
            cboLabels.SelectedIndex = 0;

            if (parameters != null)
            {
                setExampleColourBox(parameters.LineLimitColours[0], pDonor);
                setExampleColourBox(parameters.LineLimitColours[1], pAcceptor);
                setExampleColourBox(parameters.LineLimitColours[2], pStart);
                setExampleColourBox(parameters.LineLimitColours[3], pStop);
            }

            if (parameters != null && parameters.AllSequences != "")
            {
                nudZoomFrom.Minimum = 1;
                nudZoomTo.Minimum = 1;
                nudZoomFrom.Maximum = parameters.AllSequences.Length;
                nudZoomTo.Maximum = parameters.AllSequences.Length;
                nudZoomFrom.Value = 1;
                nudZoomTo.Value = parameters.AllSequences.Length;
            }

            cboGeneFeatureShapetype.SelectedIndex = 0;
            setFeatureMarkerColour();

            btnLayoutUp.Text = "↑"; // Unicode up arrow
            btnLayoutDown.Text = "↓"; // Down arrow
            lvLayout.FullRowSelect = true;
            lvLayout.GridLines = true;
            setUPlvLayout();

            cboImageDPI.SelectedIndex = 0;

            cboSequenceTranscriptName.Items.Clear();
            cboSequenceTranscriptName.Items.Add("Select");
            cboSequenceTranscriptName.Items.AddRange(parameters.SequenceNames.ToArray());
            cboSequenceTranscriptName.SelectedIndex = 0;
        }

        private void cboLabels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLabels.Text == "Left")
            {
                nudLabelWidth.Enabled = true;
                parameters.LabelWidth = (int)nudLabelWidth.Value;
                parameters.LegendsLocation = DrawLabels.left;
            }
            else
            {
                nudLabelWidth.Enabled = false;
                parameters.LabelWidth = 0;
                if (cboLabels.Text == "None")
                { parameters.LegendsLocation = DrawLabels.none; }
                else if (cboLabels.Text == "Above")
                { parameters.LegendsLocation = DrawLabels.above; }
            }
            ReDraw();
        }

        private void nudLabelWidth_ValueChanged(object sender, EventArgs e)
        {
            parameters.LabelWidth = (int)nudLabelWidth.Value;
            ReDraw();
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = parameters.IDFont;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Font tFont = new System.Drawing.Font(fd.Font.FontFamily, 10, fd.Font.Style);
                parameters.IDFont = tFont;
                ReDraw();
            }
        }

        private void btnFeatureFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = parameters.FeatureFont;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Font tFont = new System.Drawing.Font(fd.Font.FontFamily, 10, fd.Font.Style);
                parameters.FeatureFont = tFont;
                ReDraw();
            }
        }

        private void nudZoomFrom_ValueChanged(object sender, EventArgs e)
        {
            nudZoomTo.Minimum = nudZoomFrom.Value;
            parameters.Zoom = new Point((int)nudZoomFrom.Value, (int)nudZoomTo.Value);
            ReDraw();
        }

        private void nudZoomTo_ValueChanged(object sender, EventArgs e)
        {
            nudZoomFrom.Maximum = nudZoomTo.Value;
            parameters.Zoom = new Point((int)nudZoomFrom.Value, (int)nudZoomTo.Value);
            ReDraw();
        }

        private void chkCoordindatesAt1bp_CheckedChanged(object sender, EventArgs e)
        {
            parameters.CoodindatesStartAt1bp = chkCoordindatesAt1bp.Checked;
            ReDraw();
        }

        private void chkShowCodingSequences_CheckedChanged(object sender, EventArgs e)
        {
            parameters.ShowOrf = chkShowCodingSequences.Checked;
            ReDraw();
        }

        private void chkRound_CheckedChanged(object sender, EventArgs e)
        {
            parameters.Rounded = chkRound.Checked;
            ReDraw();
        }

        private void chkReduce_CheckedChanged(object sender, EventArgs e)
        {
            parameters.Reduced = chkReduce.Checked;
            ReDraw();
        }

        private void nudIntronGap_ValueChanged(object sender, EventArgs e)
        {
            parameters.IntronGap = (int)nudIntronGap.Value;
            ReDraw();
        }

        private void setExampleColourBox(Color colour, PictureBox pBox)
        {
            Bitmap bmpBox = new Bitmap(pBox.Width, pBox.Height);
            Graphics gBox = Graphics.FromImage(bmpBox);
            gBox.Clear(colour);
            pBox.Image = bmpBox;
        }
        private void cboSpliceSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameters.ShowExonLimits = cboSpliceSites.Text;
            ReDraw();
        }

        private void btnDonor_Click(object sender, EventArgs e)
        {
            ShapeColour sc = new ShapeColour(parameters.LineLimitColours[0]);
            sc.SetText("Select the donor line colour", "To select the lines colour pres the 'Colour' button");
            if (sc.ShowDialog() == DialogResult.OK)
            {
                parameters.LineLimitColours[0] = sc.GetShapeColour;
                setExampleColourBox(parameters.LineLimitColours[0], pDonor);
                ReDraw();
            }
        }

        private void btnAcceptor_Click(object sender, EventArgs e)
        {
            ShapeColour sc = new ShapeColour(parameters.LineLimitColours[1]);
            sc.SetText("Select the acceptor line colour", "To select the line's colour press the 'Colour' button");
            if (sc.ShowDialog() == DialogResult.OK)
            {
                parameters.LineLimitColours[1] = sc.GetShapeColour;
                setExampleColourBox(parameters.LineLimitColours[1], pAcceptor);
                ReDraw();
            }
        }

        private void cboORFLimits_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameters.ShowORFLimits = cboORFLimits.Text;
            ReDraw();
        }

        private void btnORFStart_Click(object sender, EventArgs e)
        {
            ShapeColour sc = new ShapeColour(parameters.LineLimitColours[2]);
            sc.SetText("Select the start codon line colour", "To select the lines colour pres the 'Colour' button");
            if (sc.ShowDialog() == DialogResult.OK)
            {
                parameters.LineLimitColours[2] = sc.GetShapeColour;
                setExampleColourBox(parameters.LineLimitColours[2], pStart);
                ReDraw();
            }
        }

        private void btnORFStop_Click(object sender, EventArgs e)
        {
            ShapeColour sc = new ShapeColour(parameters.LineLimitColours[3]);
            sc.SetText("Select the stop codon line colour", "To select the lines colour pres the 'Colour' button");
            if (sc.ShowDialog() == DialogResult.OK)
            {
                parameters.LineLimitColours[3] = sc.GetShapeColour;
                setExampleColourBox(parameters.LineLimitColours[3], pStop);
                ReDraw();
            }
        }

        private void nudLineWidth_ValueChanged(object sender, EventArgs e)
        {
            parameters.LimitsLineWidth = (float)nudLineWidth.Value;
            ReDraw();
        }

        private void cboCoordinates_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboCoordinates.Text)
            {
                case "None":
                    parameters.SizeMarkerLocation = SizeMarkerLocation.None;
                    break;
                case "Above":
                    parameters.SizeMarkerLocation = SizeMarkerLocation.Top;
                    break;
                case "Below":
                    parameters.SizeMarkerLocation = SizeMarkerLocation.Bottom;
                    break;
            }
            ReDraw();
        }

        private void nudMajorTick_ValueChanged(object sender, EventArgs e)
        {
            parameters.MajorTickMark = (int)nudMajorTick.Value;
            ReDraw();
        }

        private void nudMinorTick_ValueChanged(object sender, EventArgs e)
        {
            parameters.MinorTickMark = (int)nudMinorTick.Value;
            ReDraw();
        }

        private void ReDraw()
        {
            if (mRNADV == null)
            {
                mRNADV = new mRNADisplayViewer(this);
                mRNADV.Show();
            }
            mRNADV.AddImage(DrawGraph(interfaceScale));
        }

        private void nudLabelRotation_ValueChanged(object sender, EventArgs e)
        {
            parameters.AngleOfrotation = (int)nudLabelRotation.Value;
            ReDraw();
        }
        #endregion

        private void btnSequenceColours_Click(object sender, EventArgs e)
        {
            if (parameters == null) { return; }
            List<string> Names = new List<String>();
            Dictionary<string, Color> codingColours = new Dictionary<string, Color>();
            Dictionary<string, Color> nonCodingColours = new Dictionary<string, Color>();

            foreach (string name in parameters.SequenceNames)
            {
                Names.Add(name);
                codingColours.Add(name, parameters.SequenceCDSColour[name]);
                nonCodingColours.Add(name, parameters.SequenceExonColour[name]);
            }

            mRNADisplaySequenceColours mRNADSC = new mRNADisplaySequenceColours(Names, codingColours, nonCodingColours);
            if (mRNADSC.ShowDialog() == DialogResult.OK)
            {
                parameters.SequenceCDSColour = mRNADSC.getCodingColours;
                parameters.SequenceExonColour = mRNADSC.getNonCodingColours;
                ReDraw();
            }

        }

        private void btnCreateMiscellaneousSets_Click(object sender, EventArgs e)
        {
            if (parameters == null) { return; }
            Dictionary<string, List<mRNADisplayMiscFeature>> Sets = parameters.MiscellaneousFeatureGBSet;
            mRNAMiscellaneousFeatureSelection mRNAMFS = new mRNAMiscellaneousFeatureSelection(Sets, parameters);
            if (mRNAMFS.ShowDialog() == DialogResult.OK)
            {
                parameters.MiscellaneousFeatureGBSet = mRNAMFS.GetSets;
                ReDraw();
            }
        }

        private void chkLinkFeaturesToSequences_CheckedChanged(object sender, EventArgs e)
        {
            parameters.ShowSuperscript = chkLinkFeaturesToSequences.Checked;
            ReDraw();
        }

        #region Draw markers
        private GeneFeatureMarker currentGeneFeatureMarker = new GeneFeatureMarker();

        private void DrawCurrentFeatureMarker()
        {
            setShowButton();
            if (currentGeneFeatureMarker != null && currentGeneFeatureMarker.DrawMe == true)
            { ReDraw(); }
        }

        private void cboFeatureTops_SelectedIndexChanged(object sender, EventArgs e)
        {
            int top = 0;
            if (parameters.FeatureRows.ContainsKey(cboFeatureTops.Text) == true)
            { top = (int)parameters.FeatureRows[cboFeatureTops.Text]; }
            currentGeneFeatureMarker.SetLinkedItem(cboFeatureTops.Text, 0, parameters.AllSequences.Length, top);
        }

        private void nudGeneFeatureX_ValueChanged(object sender, EventArgs e)
        {
            currentGeneFeatureMarker.X = (int)nudGeneFeatureX.Value + parameters.LabelWidth;
            DrawCurrentFeatureMarker();
        }

        private void nudGeneFeatureY_ValueChanged(object sender, EventArgs e)
        {
            currentGeneFeatureMarker.Y = (int)nudGeneFeatureY.Value;
            DrawCurrentFeatureMarker();
        }


        private void chlGenefeatureMouseClick_CheckedChanged(object sender, EventArgs e)
        {

        }
        public void getMouseClickLocation(Point location)
        {
            if (currentGeneFeatureMarker == null) { return; }
            if (chlGenefeatureMouseClick.Checked == false) { return; }

            int X = (int)nudGeneFeatureX.Value;
            int Y = (int)nudGeneFeatureY.Value;

            try
            {
                nudGeneFeatureX.Maximum = parameters.DrawingArea.Width - parameters.LabelWidth;
                nudGeneFeatureY.Maximum = parameters.DrawingArea.Height;

                nudGeneFeatureX.Value = location.X - parameters.LabelWidth;
                nudGeneFeatureY.Value = location.Y;
                //currentGeneFeatureMarker.X = location.X;
                //currentGeneFeatureMarker.Y = location.Y;
                DrawCurrentFeatureMarker();
            }
            catch
            {
                nudGeneFeatureX.Value = X;
                nudGeneFeatureY.Value = Y;
            }

        }

        private void btnGeneFeatureFillColour_Click(object sender, EventArgs e)
        {
            if (currentGeneFeatureMarker == null) { return; }
            Color shapeColour = currentGeneFeatureMarker.Colour;
            ShapeColour sc = new ShapeColour(shapeColour);

            sc.SetText("Feature colour selection", "Set the feature's colour  by pressing the 'Colour' button.");

            if (sc.ShowDialog() == DialogResult.OK)
            {
                currentGeneFeatureMarker.Colour = sc.GetShapeColour;
                DrawCurrentFeatureMarker();
                setFeatureMarkerColour();
            }
        }

        private void setFeatureMarkerColour()
        {
            Bitmap show = new Bitmap(pGeneFeatureFillColour.Width, pGeneFeatureFillColour.Height);
            Graphics gShow = Graphics.FromImage(show);
            gShow.Clear(currentGeneFeatureMarker.Colour);
            pGeneFeatureFillColour.Image = show;
        }

        private void nudGeneFeatureW_ValueChanged(object sender, EventArgs e)
        {
            currentGeneFeatureMarker.Width = (int)nudGeneFeatureW.Value;
            DrawCurrentFeatureMarker();
        }

        private void nudGeneFeatureH_ValueChanged(object sender, EventArgs e)
        {
            currentGeneFeatureMarker.Height = (int)nudGeneFeatureH.Value;
            DrawCurrentFeatureMarker();
        }
        private void setShowButton()
        {
            if (currentGeneFeatureMarker.DrawMe == false)
            { btnDrawCurrentGeneFeatureMarker.PerformClick(); }
            if (currentGeneFeatureMarker.X == 0 && currentGeneFeatureMarker.DrawMe == true)
            { currentGeneFeatureMarker.X = parameters.LabelWidth; }
        }

        private void btnDrawCurrentGeneFeatureMarker_Click(object sender, EventArgs e)
        {
            currentGeneFeatureMarker.DrawMe = !currentGeneFeatureMarker.DrawMe;
            ReDraw();

            if (currentGeneFeatureMarker.DrawMe == true) { btnDrawCurrentGeneFeatureMarker.Text = "Hide"; }
            else { btnDrawCurrentGeneFeatureMarker.Text = "Draw"; }
        }

        private void cboGeneFeatureName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboGeneFeatureName_Change();
        }

        private void cboGeneFeatureName_TextChanged(object sender, EventArgs e)
        {
            cboGeneFeatureName_Change();
        }

        private void cboGeneFeatureName_Change()
        {
            string key = cboGeneFeatureName.Text.Trim();
            if (key.Length < 3)
            {
                btnGeneFeatureAddUpdate.Enabled = false;
                return;
            }
            else { btnGeneFeatureAddUpdate.Enabled = true; }

            if (parameters.GeneFeatureMarkers.ContainsKey(key) == true)
            {
                btnGeneFeatureAddUpdate.Text = "Update";
                btnGeneFeatureDelete.Enabled = true;
            }
            else
            {
                btnGeneFeatureAddUpdate.Text = "Add";
                btnGeneFeatureDelete.Enabled = false;
            }

        }
        private void btnGeneFeatureAddUpdate_Click(object sender, EventArgs e)
        {
            string key = cboGeneFeatureName.Text.Trim();
            if (key.Length < 3) { return; }

            float[] basePlace = getLocationFromX(parameters.Zoom.X, parameters.Zoom.Y, currentGeneFeatureMarker.X, interfaceScale, parameters.LabelWidth, parameters.IntronGap);

            if (parameters.GeneFeatureMarkers.ContainsKey(key) == true)
            {
                parameters.GeneFeatureMarkers[key] = currentGeneFeatureMarker.Clone();
                parameters.GeneFeatureMarkers[key].MRNABasePlace = basePlace;
                currentGeneFeatureMarker.DrawMe = false;
                btnDrawCurrentGeneFeatureMarker.Text = "Draw";
            }
            else
            {
                parameters.GeneFeatureMarkers.Add(key, currentGeneFeatureMarker.Clone());
                parameters.GeneFeatureMarkers[key].MRNABasePlace = basePlace;
                currentGeneFeatureMarker.DrawMe = false;
                btnDrawCurrentGeneFeatureMarker.Text = "Draw";
            }
            UpdatecboGeneFeatureName();
            ReDraw();
        }

        private float[] getLocationFromX(int theStart, int theEnd, float X, scalar scale, int labelWidth, int intronGap)
        {
            float[] answer = { 0, 0 };
            Point limitRegion = parameters.Zoom;

            int numberOfGaps = getNumberOfGapsInAllSequences(limitRegion);
            int sequenceWidth = limitRegion.Y + 1 - limitRegion.X - numberOfGaps;

            int gapWidth = numberOfGaps * scale.i[parameters.IntronGap];
            float scaleFactor = (parameters.DrawingArea.Width - gapWidth - scale.scale * (30 + parameters.LabelWidth)) / sequenceWidth;

            int limitX = parameters.Zoom.X;
            string allSequences = parameters.AllSequences;
            string[] sequences = allSequences.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string sequence in sequences)
            {
                int placeInAllsequences = allSequences.IndexOf(sequence);
                int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(limitX, placeInAllsequences));
                float startPoint = parameters.LabelWidth + scale.i[15] + (numberofGapsbeforeSequence * scale.i[parameters.IntronGap]) +
                    ((placeInAllsequences - limitX - numberofGapsbeforeSequence) * scaleFactor);
                float endPoint = startPoint + (sequence.Length * scaleFactor);
                if (X < startPoint)
                {
                    float bit = startPoint - X;
                    float baseOffset = bit * 100 / scale.i[parameters.IntronGap];
                    answer[0] = placeInAllsequences;
                    answer[1] = -baseOffset;
                    break;
                }
                else if (X <= endPoint)
                {
                    float offset = (int)(X - startPoint);
                    float widthInPixels = endPoint - startPoint;
                    float proportion = offset / widthInPixels;
                    float basePlace = ((offset / widthInPixels) * sequence.Length) + placeInAllsequences;
                    answer[0] = (int)basePlace;
                    answer[1] = float.MinValue;
                    break;
                }
            }
            return answer;
        }

        private void btnGeneFeatureDelete_Click(object sender, EventArgs e)
        {
            string key = cboGeneFeatureName.Text.Trim();
            if (key.Length < 3) { return; }

            if (parameters.GeneFeatureMarkers.ContainsKey(key) == true)
            {
                parameters.GeneFeatureMarkers.Remove(key);
                currentGeneFeatureMarker.DrawMe = false;
            }
            UpdatecboGeneFeatureName();
            btnGeneFeatureAddUpdate.Text = "Add";
            btnGeneFeatureDelete.Enabled = (cboGeneFeatureName.Items.Count == 0);
            ReDraw();
        }

        private void UpdatecboGeneFeatureName()
        {
            cboGeneFeatureName.Items.Clear();
            cboGeneFeatureName.Text = "";
            foreach (string key in parameters.GeneFeatureMarkers.Keys)
            {
                cboGeneFeatureName.Items.Add(key);
            }
            cboGeneFeatureName.SelectedIndex = -1;
        }

        private void DrawGeneFeatures(Graphics g, float scale, scalar scaleDPI)
        {

            if (currentGeneFeatureMarker != null && currentGeneFeatureMarker.DrawMe == true)
            {
                if (parameters.FeatureRows.ContainsKey(currentGeneFeatureMarker.LinkedName) == true)
                {
                    currentGeneFeatureMarker.UpdateTop((int)parameters.FeatureRows[currentGeneFeatureMarker.LinkedName], scaleDPI.i[parameters.LabelWidth], scaleDPI);

                    PointF[] shape = currentGeneFeatureMarker.GetResizedfeaturePoints(scaleDPI);
                    if (currentGeneFeatureMarker.ShapeType == ShapeType.Box)
                    { g.DrawPolygon(new Pen(currentGeneFeatureMarker.Colour, scaleDPI.i[2]), shape); }
                    else
                    {
                        if (currentGeneFeatureMarker.SolidFill == true)
                        { g.FillPolygon(new SolidBrush(currentGeneFeatureMarker.Colour), shape); }
                        else
                        { g.DrawPolygon(new Pen(currentGeneFeatureMarker.Colour, scaleDPI.i[2]), shape); }

                    }
                    RectangleF rr = GetRectangleF(shape);
                    g.DrawRectangle(Pens.Black, rr);
                }
            }

            foreach (GeneFeatureMarker gfm in parameters.GeneFeatureMarkers.Values)
            {
                if (parameters.FeatureRows.ContainsKey(gfm.LinkedName) == true)
                {
                    float offset = 0;
                    if (gfm.MRNABasePlace[1] != float.MinValue)
                    { offset = (float)(parameters.IntronGap * gfm.MRNABasePlace[1]) / 100; }
                    if (float.IsNaN(offset) == true) { offset = 0; }

                    float X = scaleDPI.i[15] + offset + (getXOffset(parameters.Zoom.X, (int)gfm.MRNABasePlace[0], scale, scaleDPI) + (scaleDPI.scale * parameters.LabelWidth));

                    gfm.X = X;

                    gfm.UpdateTop((int)parameters.FeatureRows[gfm.LinkedName], scaleDPI);
                    PointF[] shape = gfm.GetResizedfeaturePoints(scaleDPI);



                    if (gfm.ShapeType == ShapeType.Box)
                    { g.DrawPolygon(new Pen(gfm.Colour, scaleDPI.i[2]), shape); }
                    else
                    {
                        if (gfm.SolidFill == true)
                        { g.FillPolygon(new SolidBrush(gfm.Colour), shape); }
                        else
                        { g.DrawPolygon(new Pen(gfm.Colour, scaleDPI.i[2]), shape); }
                    }
                }
            }
        }

        private float getXOffset(int theStart, int place, float scaleFactor, scalar scale)
        {
            int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(theStart, place));
            float gapWidth = numberofGapsbeforeSequence * parameters.IntronGap * scale.scale;
            int basesOffset = place + 1 - theStart;
            float X = gapWidth + ((basesOffset - numberofGapsbeforeSequence) * scaleFactor);

            return X;
        }

        private RectangleF GetRectangleF(PointF[] shape)
        {
            float top = int.MaxValue;
            float bottom = int.MinValue;
            float left = int.MaxValue;
            float right = int.MinValue;
            foreach (PointF p in shape)
            {
                if (top > p.Y) { top = p.Y; }
                if (bottom < p.Y) { bottom = p.Y; }
                if (right < p.X) { right = p.X; }
                if (left > p.X) { left = p.X; }
            }

            RectangleF r = new RectangleF(left, top, right - left, bottom - top);
            return r;
        }


        private void chkGenefeatureSolid_CheckedChanged(object sender, EventArgs e)
        {
            currentGeneFeatureMarker.SolidFill = chkGenefeatureSolid.Checked;
            DrawCurrentFeatureMarker();
        }


        private void cboGeneFeatureShapetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentGeneFeatureMarker == null) { return; }

            switch (cboGeneFeatureShapetype.Text)
            {
                case "Arrow (Down)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.ArrowDown);
                    break;
                case "Arrow (Left)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.ArrowLeft);
                    break;
                case "Arrow (Right)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.ArrowRight);
                    break;
                case "Arrow (Up)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.ArrowUp);
                    break;
                case "Box":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.Box);
                    break;
                case "Circle":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.Circle);
                    break;
                case "Cross (Diagonal)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.CrossDiagonal);
                    break;
                case "Cross (Vertical)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.CrossVertical);
                    break;
                case "Diamond":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.Diamond);
                    break;
                case "Hexagon":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.Hexagon);
                    break;
                case "Octagon":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.Octagon);
                    break;
                case "Pentagon":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.Pentagon);
                    break;
                case "Rectangle (Horizontal)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.RectangleHorizontal);
                    break;
                case "Rectangle (Vertical)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.RectangleVertical);
                    break;
                case "Square":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.Square);
                    break;
                case "Star":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.Star);
                    break;
                case "Triangle (Up)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.TriangleUp);
                    break;
                case "Triangle (Left)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.TriangleLeft);
                    break;
                case "Triangle (Right)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.TriangleRight);
                    break;
                case "Triangle (Down)":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.TriangleDown);
                    break;
                case "Vertical line":
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.VerticalLine);
                    break;
                default:
                    currentGeneFeatureMarker.SetShapePoints(ShapeType.NotSet);
                    break;
            }
            DrawCurrentFeatureMarker();
        }
        #endregion

        #region Layout
        private void setUPlvLayout()
        {
            lvLayout.Items.Clear();
            List<ClassDrawingOrder> features = parameters.Layout;
            foreach (ClassDrawingOrder key in features)
            {
                lvLayout.Items.Add(key.ToString().Replace("_", " "));
            }
        }

        private void btnLayoutUp_Click(object sender, EventArgs e)
        {
            if (lvLayout.SelectedIndices.Count == 0) { return; }
            if (lvLayout.SelectedIndices[0] == 0) { return; }
            int index = lvLayout.SelectedIndices[0];

            ClassDrawingOrder key = parameters.Layout[index];
            parameters.Layout.RemoveAt(index);
            parameters.Layout.Insert(index - 1, key);
            setUPlvLayout();
            lvLayout.Items[index - 1].Selected = true;
            lvLayout.Items[index - 1].EnsureVisible();
            ReDraw();
        }

        private void btnLayoutDown_Click(object sender, EventArgs e)
        {
            if (lvLayout.SelectedIndices.Count == 0) { return; }
            if (lvLayout.SelectedIndices[0] == lvLayout.Items.Count - 1) { return; }
            int index = lvLayout.SelectedIndices[0];

            ClassDrawingOrder key = parameters.Layout[index];
            parameters.Layout.RemoveAt(index);
            parameters.Layout.Insert(index + 1, key);
            setUPlvLayout();
            lvLayout.Items[index + 1].Selected = true;
            lvLayout.Items[index + 1].EnsureVisible();
            ReDraw();
        }

        private void btnChangeSequenceNamesAndOrder_Click(object sender, EventArgs e)
        {
            Rename r = new Rename(parameters.SequenceDisplayNames, false);
            if (r.ShowDialog() == DialogResult.OK)
            {
                parameters.SequenceDisplayNames = r.GetUpdatedLabels();
                List<string> order = r.getUserSelection;

                Dictionary<string, string> newDataSet = new Dictionary<string, string>();
                foreach (string key in order)
                {
                    if (parameters.SequenceDisplayNames.ContainsKey(key) == true)
                    {
                        newDataSet.Add(key, parameters.SequenceDisplayNames[key]);
                    }
                }
                parameters.SequenceDisplayNames = newDataSet;
                ReDraw();
            }
        }

        private void btnResetSequenceNamesAndOrder_Click(object sender, EventArgs e)
        {
            parameters.ResetDisplayNames();
            ReDraw();
        }
        #endregion

        private void cboImageDPI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboImageDPI.SelectedIndex == 0)
            { btnSaveImage.Enabled = false; }
            else { btnSaveImage.Enabled = true; }
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (cboImageDPI.SelectedIndex == 0) { return; }
            string fileName = FileString.SaveAs("Save alignment image", "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp|tiff (*.tiff)|*.tiff");
            if (fileName == "Cancel") { return; }
            try
            {
                int dpi = int.Parse(cboImageDPI.Text);
                scalar saveAs = new scalar(dpi);
                Bitmap bmp = DrawGraph(saveAs);
                bmp.Save(fileName);
            }
            catch (Exception ex) { MessageBox.Show("An error occured: " + ex.Message, "Error saving image"); }
        }

        private void btnImportInterProScan_Click(object sender, EventArgs e)
        {
            GetInterProScan gips = new GetInterProScan(parameters);
            if (gips.ShowDialog() == DialogResult.OK)
            {
                btnCreateInterProScanFeaturesSets.Enabled = true;
            }
        }

        private void btnCreateInterProScanFeaturesSets_Click(object sender, EventArgs e)
        {
            AlignmentFeatureSelection afs = new AlignmentFeatureSelection(parameters.Domains, parameters.Selected, true);
            if (afs.ShowDialog() == DialogResult.OK)
            {
                parameters.Selected = afs.SelectedDomains;
                ReDraw();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void setFrame()
        {
            int lengthOfConsnsus = parameters.AllSequences.Length;

            foreach (string name in parameters.SequenceNames)
            {
                if (parameters.CDSs.ContainsKey(name) == false) continue;
                Point orf = parameters.CDSs[name];
                int[] frame = Enumerable.Repeat(9, lengthOfConsnsus).ToArray();
                List<Point> exonPoints = parameters.Exons[name];
                int codon = 1;
                foreach (Point exon in exonPoints)
                {
                    Point adjustedExon = new Point(exon.X, exon.Y);
                    if (adjustedExon.X < orf.X) { adjustedExon.X = orf.X; }
                    if (adjustedExon.Y > orf.Y) { adjustedExon.Y = orf.Y; }
                    if (adjustedExon.X < adjustedExon.Y)
                    {
                        string sequence = parameters.getExonSequence(name, adjustedExon);
                        int placeInAllsequences = parameters.AllSequences.IndexOf(sequence);
                        int numberofGapsbeforeSequence = getNumberOfGapsInAllSequences(new Point(0, placeInAllsequences));

                        for (int index = 0; index < sequence.Length; index++)
                        {
                            int place = index + placeInAllsequences;
                            frame[place] = codon++;
                            if (codon == 4) { codon = 1; }
                        }
                    }
                    parameters.Codons[name] = frame;
                }

            }
            SetOpenreadingframes("None");
        }

        private void SetOpenreadingframes(string baseSequenceName)
        {
            parameters.ExonWithFrame = new Dictionary<string, List<ReadingFrame>>();
            if (baseSequenceName == "None") { return; }
            int[] mainFrame = (int[])parameters.Codons[baseSequenceName].Clone();

            List<ReadingFrame> data = new List<ReadingFrame>();

            foreach (string key in parameters.Codons.Keys)
            {
                ReadingFrame rf = new ReadingFrame(0, 0, 0);

                data = new List<ReadingFrame>();

                int lastValue = 4;
                int[] otherFrame = parameters.Codons[key];
                int answer = 0;
                for (int index = 0; index < otherFrame.Length; index++)
                {
                    if (parameters.AllSequences[index] == ' ')
                    { answer = 0; }
                    else if (otherFrame[index] != 9 && mainFrame[index] != 9)
                    {
                        int diff = otherFrame[index] - mainFrame[index];
                        if (diff == 0)
                        { answer = 5; }
                        else if (diff == -1 || diff == 2)
                        { answer = 6; }
                        else
                        { answer = 7; }
                    }
                    else { answer = 4; }

                    if (answer != lastValue)
                    {
                        if (otherFrame[index] == 0 && rf.frame > 4)//end of exon
                        {
                            rf.end = index - 1;
                            data.Add(rf.Clone());
                            rf = new ReadingFrame();
                            lastValue = answer;
                        }
                        else if (answer > 4 && rf.frame != answer) // start of new block
                        {
                            if (answer > 4)
                            {
                                rf.frame = answer;
                                rf.start = index;
                                lastValue = answer;
                            }
                        }
                        else if (answer <= 4 && rf.frame > 4) // end of block
                        {
                            rf.end = index - 1;
                            data.Add(rf.Clone());
                            rf = new ReadingFrame();
                            lastValue = answer;
                        }
                    }
                }
                if (rf.start > 0)
                {
                    rf.end = otherFrame.Length - 1;
                    data.Add(rf.Clone());
                }
                parameters.ExonWithFrame[key] = data.ToList();
            }
        }

        private void cboFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOpenreadingframes(cboFrame.Text);
            ReDraw();
        }

        private void btnFrameColours_Click(object sender, EventArgs e)
        {
            Color[] frameColours = parameters.ReadingFrameColours;
            mRNAFrameColourSelector mRNAFCS = new mRNAFrameColourSelector(frameColours);
            if (mRNAFCS.ShowDialog() == DialogResult.OK)
            {
                parameters.ReadingFrameColours = mRNAFCS.GetFrameColours;
                ReDraw();
            }
        }


        private void SetSequenceButtonActivity()
        {
            btnSequenceAdd.Enabled = false;
            btnSequencerRmove.Enabled = false;            

            if (parameters.SequenceNames.Count == 0) { return; }
            if (txtSequenceDisplayname.Text.Trim().Length < 3) { return; }
            if (cboSequenceTranscriptName.SelectedIndex == 0) { return; }

            string key = cboSequenceTranscriptName.Text + "#" + txtSequenceDisplayname.Text.Trim();
            if (parameters.BindingSites.ContainsKey(key) == true)
            { btnSequencerRmove.Enabled = true; }

            if (CleanSequence(txtSequencesSequence.Text).Length > 5) { return; }
            btnSequenceAdd.Enabled = true;          
        }
        private void txtSequenceDisplayname_TextChanged(object sender, EventArgs e)
        {
            SetSequenceButtonActivity();
        }

        private void txtSequencesSequence_TextChanged(object sender, EventArgs e)
        {
            SetSequenceButtonActivity();
        }

        private void cboSequenceTranscriptName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSequenceButtonActivity();
        }

        private void btnSequenceAdd_Click(object sender, EventArgs e)
        {
            string sequence = parameters.SequenceDNA[cboSequenceTranscriptName.Text];
            List<string> seqs = new List<string>();
            foreach (string s in txtSequencesSequence.Lines)
            {
                string cleaned = CleanSequence(s);
                if (cleaned.Length > 5)
                seqs.Add(cleaned.Trim().ToLower()); 
            }

            List<Point> sites = new List<Point>();
            foreach(string s in seqs)
            {
                List<int> hits = IndexOf(sequence, s, 0.9f);
                if (hits.Count > 0)
                {
                    foreach (int place in hits)
                    { sites.Add(new Point(place + 1, place + s.Length)); }
                }
            }

            if (sites.Count > 0)
            { parameters.BindingSites[cboSequenceTranscriptName.Text + "#" + txtSequenceDisplayname.Text.Trim()] = sites; }

            txtSequencesSequence.Clear();
            txtSequenceDisplayname.Clear();
            cboSequenceTranscriptName.SelectedIndex = 0;
            ReDraw();
        }

        private void btnSequencerRmove_Click(object sender, EventArgs e)
        {
            if (parameters.BindingSites.ContainsKey(cboSequenceTranscriptName.Text + "#" + txtSequenceDisplayname.Text.Trim()) == true)
            { parameters.BindingSites.Remove(cboSequenceTranscriptName.Text + "#" + txtSequenceDisplayname.Text.Trim()); }

            txtSequencesSequence.Clear();
            txtSequenceDisplayname.Clear();
            cboSequenceTranscriptName.SelectedIndex = 0;
            ReDraw();
        }

        private string CleanSequence(string sequence)
        {
            string answer = "";
            for(int index =0; index< sequence.Length; index++)
            {
                switch (sequence[index])
                {
                    case 'a':
                    case 'A':
                        answer += "a";
                        break;
                    case 'c':
                    case 'C':
                        answer += "c";
                        break;
                    case 'g':
                    case 'G':
                        answer += "g";
                        break;
                    case 't':
                    case 'T':
                        answer += "t";
                        break;
                }
            }
            return answer;
        }

        private static List<int> IndexOf(string target, string sequence, float scoreCutoff)
        {            
            int sequencelength = sequence.Length;
            int score = 0;
            int bestScore = 0;
            int bestPlace = -1;
            List<int> hits = new List<int>();
            
            for (int index = 0; index < target.Length - sequence.Length - 1; index++)
            {
                for (int inner = 0; inner < sequence.Length; inner++)
                {
                    if (sequence[inner] == target[inner + index])
                    { score++; }
                }
                if ((float)score / sequencelength > scoreCutoff)
                {
                    if (score == bestScore) { hits.Add(index); }
                    if (score > bestScore)
                    {
                        hits = new List<int>();
                        hits.Add(index);                        
                    }
                    bestScore = score;
                }
                score = 0;
            }

            return hits;
        }

    }
}
