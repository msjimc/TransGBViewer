using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public class mRNADisplayMiscFeature
    {
        private int startPoint = 0;
        private int endPoint = 0;
        private string description = "";
        private Color color = Color.Black;

        public mRNADisplayMiscFeature(int start, int end, string desc, Color col)
        {
            startPoint = start;
            endPoint = end;
            description = desc;
            color = col;
        }

        public int StartPoint { get { return startPoint; } set { startPoint = value; } }
        public int EndPoint { get { return endPoint; } set { endPoint = value; } }
        public int length { get { return endPoint - startPoint + 1; } }
        public string Description { get { return description; } set { description = value; } }
        public Color Color { get { return color; } set { color = value; } }

        public string DiplayName()
        {
            if (startPoint== endPoint)
            { return description + " (" + startPoint.ToString() + ")"; }
            else
            { return description + " (" + startPoint.ToString() + " - " + endPoint.ToString() + ")"; }        
        }
    }
}
