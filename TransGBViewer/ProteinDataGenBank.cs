using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public class ProteinDataGenBank
    {
        private string name = "";
        private string defination = "";
        private Dictionary<string, Point> regions = new Dictionary<string, Point>();
        private Dictionary<string, Point> sites = new Dictionary<string, Point>();
        private string sequenceAA = "";
        public ProteinDataGenBank() { }

        public string Name { get { return name; } set { name = value; } }
        public string Defination { get { return defination; } set { defination = value; } }
        public Dictionary<string, Point> Regions { get { return regions; } set { regions = value; } }
        public Dictionary<string, Point> Sites { get { return sites; } set { sites = value; } }
        public string SequenceAA { get { return sequenceAA; } set { sequenceAA = value.ToUpper(); } }
    }

}