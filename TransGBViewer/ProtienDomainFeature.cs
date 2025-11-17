using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public class ProteinDomainSubFeature
    {
        private bool isOK = false;
        private int begin = -1;
        private int beginAlignment = -1;
        private int end = -1;
        private int endAlignment = -1;
        private string type = "";
        private string description = "";
        private string category = "";
        private string ligand = "";
        private Color colour = Color.Black;

        public ProteinDomainSubFeature(int Beginning, int End, string Type, string Description, string Category)
        {
            begin = Beginning;
            end = End;  
            type = Type;
            description = Description;
            category = Category;
        }

        public ProteinDomainSubFeature(string fragment)
        {
            try
            {
                string[] features = { "\"begin\":\"", "\"end\":\"", "\"type\":\"", "\"description\":\"", "\"category\":\"", "\"ftId\":\"" };

                int startOfValue = 0;
                int endofValue = 0;

                for (int index = 0; index < features.Length; index++)
                {
                    string feature = features[index];

                    startOfValue = fragment.IndexOf(feature);
                    endofValue = fragment.IndexOf("\"", startOfValue + feature.Length+1);
                    if (startOfValue > -1)
                    {
                        startOfValue += feature.Length;
                        string bit = fragment.Substring(startOfValue, endofValue - startOfValue);
                        switch (index)
                        {
                            case 0:
                                begin = int.Parse(bit);
                                break;
                            case 1:
                                end = int.Parse(bit);
                                break;
                            case 2:
                                type = bit;
                                break;
                            case 3:
                                if (bit == "\",")
                                { description = type + SetPlace(begin, end); }
                                else { description = bit + SetPlace(begin, end); }
                                break;
                            case 4:
                                category = bit;
                                break;
                            case 5:
                                if (description == "")
                                { description = bit + SetPlace(begin, end); }
                                break;
                        }
                    }
                }
                
                if (description == "") { description = type + SetPlace(begin, end); }

                if (type.ToUpper().StartsWith("BINDING") == true)
                {
                    ligand = getLigand(fragment);
                    description = ligand + " " + description;
                }

                isOK = true;
            } 
            catch 
            { isOK = false; }
        }

        private string SetPlace(int begin, int end)
        {
            if (begin==end)
            { return " (" + begin.ToString() + ")"; }
            else
            { return " (" + begin.ToString() + " - " + end.ToString() +  ")"; }
        }
        private string getLigand(string fragment)
        {
            string ligand = "";
            int startOfValue = 0;
            int endofValue = 0;

            string feature = "\"ligand\":";

            startOfValue = fragment.IndexOf(feature);
            endofValue = fragment.IndexOf("\",\"", startOfValue + feature.Length + 1);
            startOfValue = fragment.LastIndexOf(":", endofValue);
             if (startOfValue > -1)
            {
                startOfValue += 2;           
                ligand = fragment.Substring(startOfValue, endofValue - startOfValue);                
            }

            return ligand;
        }        

        public bool IsOK { get { return isOK; } }
        public int StartPoint { get { return (begin - 1) * 3; } }
        public int EndPoint { get { return ((end - 1) * 3) + 2; } }
        public int StartPointAA { get { return begin; } set { begin = value; } }
        public int EndPointAA { get { return end; } set { end = value; } }
        public int StartPointAlignment { get { return beginAlignment; } set { beginAlignment = value; } }
        public int EndPointAlignment { get { return endAlignment; } set { endAlignment = value; } }
        public string Type { get { return type; } set { type = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string Category { get { return category; } set { category = value; } }
        public string Ligand { get { return ligand; } set { ligand = value; } }
        public Color Colour { get { return colour; } set { colour = value; } }
    }

    public class ProteinDomainFeature
    {       
        private string featureOwner = "";
        private string sequence = "";
        private Dictionary<string, List<ProteinDomainSubFeature>> subDomains = new Dictionary<string, List<ProteinDomainSubFeature>>();

        public ProteinDomainFeature() { }
        
        public ProteinDomainFeature(string data, string DomainSequence, string ownerName)
        {
            featureOwner = ownerName;
            sequence = DomainSequence;

            List<string> typeFragments = GetTypeFragments(data);
            subDomains = GetTypeFragmentsInClasses(typeFragments);

        }

        private List<string> GetTypeFragments(string data)
        {
            List<string> typeFragments = new List<string>();

            int index = data.IndexOf("{\"type\":");
            while (index > -1)
            {
                int startPoint = index;
                index = data.IndexOf("{\"type\":", startPoint + 1);
                if (index > -1)
                {
                    string fragment = data.Substring(startPoint, index - startPoint);
                    typeFragments.Add(fragment);
                }
            }

            return typeFragments;
        }

        private Dictionary<string, List<ProteinDomainSubFeature>> GetTypeFragmentsInClasses(List<string> typeFragments)
        {
            Dictionary<string, List<ProteinDomainSubFeature>> data = new Dictionary<string, List<ProteinDomainSubFeature>>();            

            foreach (string typeFragment in typeFragments)
            {
                ProteinDomainSubFeature pdsf = new ProteinDomainSubFeature(typeFragment);
                if (pdsf.IsOK == true)
                {
                    if (data.ContainsKey(pdsf.Type) == true)
                        { data[pdsf.Type].Add(pdsf); }
                    else
                    {
                        List<ProteinDomainSubFeature> list = new List<ProteinDomainSubFeature>();
                        list.Add(pdsf);
                        data.Add(pdsf.Type, list);
                    }
                }
            }
            return data;
        }

        public string FeatureOwner { get { return featureOwner; } set { featureOwner = value; } }
        public string Sequence { get { return sequence; } set { sequence = value; } }
        public Dictionary<string, List<ProteinDomainSubFeature>> SubDomains { get { return subDomains; } set { subDomains = value; } }
    }

}
