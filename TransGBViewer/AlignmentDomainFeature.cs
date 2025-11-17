using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public class AlignmentDomainFeature
    {
        private bool isOK = false;
        private string analysis = "";
        private string signature = "";
        private string signatureDescription = "";
        private int start = -1;
        private int end = -1;
        private int startAlignment = -1;
        private int endAlignment = -1;
        private double score = 1;
        private string interProName = "";
        private string interProDescription = "";
        private string baseSequenceName = "";
        private string displayName = "";
        private int[] nameSelectionOptions = new int[] { 0, 0, 1, 0, 0 };

        private Color borderColour = Color.Black;
        private Color fillColour = Color.Green;
        private bool rounded = true;
        private bool drawBorder = true;
        private bool fillShape = true;
        private DomainNameLocation domainNameLocation = DomainNameLocation.left;
        private bool recalibrated = false;
        public AlignmentDomainFeature(string data, string BaseSequenceName)
        {
            try
            {
                string[] items = data.Split('\t');
                if (items.Length < 13) { isOK = false;return; }
                analysis = items[3];
                signature = items[4];
                signatureDescription = items[5];
                start = int.Parse(items[6]);
                end = int.Parse(items[7]);
                if (items[8] != "-")
                { score = double.Parse(items[8]); }
                else { score = -1; }
                interProName = items[11];
                interProDescription = items[12];
                baseSequenceName = BaseSequenceName;
                isOK = true;
            }
            catch { isOK = false; }
        }
        public AlignmentDomainFeature() { isOK = true; }
        public AlignmentDomainFeature Copy()
        {
            AlignmentDomainFeature copy = new AlignmentDomainFeature();
            copy.baseSequenceName = baseSequenceName;
            copy.displayName = displayName;
            copy.borderColour = borderColour;
            copy.fillColour = fillColour;
            copy.rounded = rounded;
            copy.drawBorder = drawBorder;
            copy.fillShape = fillShape;
            copy.analysis = analysis;
            copy.signature = signature;
            copy.signatureDescription = signatureDescription;
            copy.start = start;
            copy.end = end;
            copy.startAlignment = startAlignment;
            copy.EndAlignment = endAlignment;
            copy.score = score;
            copy.interProName = interProName;
            copy.interProDescription = interProDescription;
            copy.nameSelectionOptions = nameSelectionOptions;
            copy.domainNameLocation = domainNameLocation;
            copy.recalibrated = recalibrated;
            return copy;
        }

        public bool IsOK { get { return isOK; } }
        public string DisplayName { get { return displayName; } set { displayName = value; } }
        public int[] NameSelectionOptions { get { return nameSelectionOptions; } set { nameSelectionOptions = value; }}
        public string Analysis { get { return analysis; } }
        public string Signature { get { return signature; } }
        public string SignatureDescription { get { return signatureDescription; } }
        public int Start { get { return start; } }
        public int End { get { return end; } }
        public int StartAlignment { get { return startAlignment; } set { startAlignment = value; } }
        public int EndAlignment { get { return endAlignment; } set { endAlignment = value; } }
        public double Score { get { return score; } }
        public string InterProName { get { return interProName; } }
        public string InterProDescription { get { return interProDescription; } }
        public string BaseSequenceName { get { return baseSequenceName; } }
        public Color BorderColour { get { return borderColour; } set { borderColour = value; } }
        public Color FillColour { get { return fillColour; } set { fillColour = value; } }
        public bool Rounded { get { return rounded; } set { rounded = value; } }
        public bool DrawBorder { get { return drawBorder; } set { drawBorder = value; } }
        public bool FillShape { get { return fillShape; } set { fillShape = value; } }
        public DomainNameLocation DomainNameLocation { get { return domainNameLocation; } set { domainNameLocation = value; } }
        public bool Recalibrated { get { return recalibrated; } set { recalibrated = value; } }
        public override string ToString()
        {
            string coordinates = "";
            if (end == start)
            { coordinates = " (" + start.ToString() + ")"; }
            else
            { coordinates = " (" + start.ToString() + "-" + end.ToString() + ")"; }

            string answer = Analysis + ": ";
            if (signature != "-" && interProName != "-")
            { return Analysis + ": " + Signature + " and " + InterProName + coordinates; }
            else if (interProName != "-")
            { return Analysis + ": " + InterProName + coordinates; }
            else if (Signature != "-")
            { return Analysis + ": " + Signature + coordinates; }
            else { return Analysis + coordinates; }
        }


    }
}
