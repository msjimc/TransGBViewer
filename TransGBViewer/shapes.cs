using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    internal class shapes
    {


        public static PointF[] StarPoints = new PointF[] {
            new PointF(50.00f, 0.00f),
            new PointF(62.50f, 37.50f),
            new PointF(100.00f, 37.50f),
            new PointF(71.88f, 62.50f),
            new PointF(81.25f, 100.00f),
            new PointF(50.00f, 78.13f),
            new PointF(18.75f, 100.00f),
            new PointF(28.13f, 62.50f),
            new PointF(0.00f, 37.50f),
            new PointF(37.50f, 37.50f),
            new PointF(50.00f, 0.00f)
        };

        public static PointF[] trianglePointsRight = new PointF[]
        {
            new PointF(0.00f, 0.00f),
            new PointF(100.00f, 50.00f),
            new PointF(00.00f, 100.00f),
            new PointF(00.00f, 0.00f)
        };

        public static PointF[] trianglePointsUp = new PointF[]
        {
            new PointF(50.00f, 0.00f),
            new PointF(100.00f, 100.00f),
            new PointF(0.00f, 100.00f),
            new PointF(50.00f, 0.00f)
        };

        public static PointF[] trianglePointsDown = new PointF[]
        {
            new PointF(0.00f, 0.00f),
            new PointF(100.00f, 0.00f),
            new PointF(50.00f, 100.00f),
            new PointF(0.00f, 0.00f)
        };
        public static PointF[] trianglePointsLeft = new PointF[]
        {
            new PointF(100.00f, 0.00f),
            new PointF(100.00f, 100.00f),
            new PointF(0.00f, 50.00f),
            new PointF(100.00f, 0.00f)
         };

        public static PointF[] diamondPoints = new PointF[]
        {
            new PointF(50.00f, 0.00f),
            new PointF(100.00f, 50.00f),
            new PointF(50.00f, 100.00f),
            new PointF(0.00f, 50.00f),
            new PointF(50.00f, 0.00f)
        };

        public static PointF[] squarePoints = new PointF[]
        {
            new PointF(0.00f, 0.00f),
            new PointF(100.00f, 0.00f),
            new PointF(100.00f, 100.00f),
            new PointF(0.00f, 100.00f),
            new PointF(0.00f, 0.00f)
        };

        public static PointF[] crossPoints = new PointF[]
        {
            new PointF(43.75f, 0.00f),
            new PointF(56.25f, 0.00f),
            new PointF(56.25f, 43.75f),
            new PointF(100.00f, 43.75f),
            new PointF(100.00f, 56.25f),
            new PointF(56.25f, 56.25f),
            new PointF(56.25f, 100.00f),
            new PointF(43.75f, 100.00f),
            new PointF(43.75f, 56.25f),
            new PointF(0.00f, 56.25f),
            new PointF(0.00f, 43.75f),
            new PointF(43.75f, 43.75f),
            new PointF(43.75f, 0.00f)
        };

        public static PointF[] topHalfRectanglePoints = new PointF[]
        {
            new PointF(0.00f, 0.00f),
            new PointF(100.00f, 0.00f),
            new PointF(100.00f, 50.00f),
            new PointF(0.00f, 50.00f),
            new PointF(0.00f, 0.00f)
        };

        public static PointF[] leftHalfRectanglePoints = new PointF[]
        {
            new PointF(0.00f, 0.00f),
            new PointF(50.00f, 0.00f),
            new PointF(50.00f, 100.00f),
            new PointF(0.00f, 100.00f),
            new PointF(0.00f, 0.00f)
        };

        public static PointF[] upArrowPoints = new PointF[]
        {
            new PointF(50.00f, 0.00f),
            new PointF(100.00f, 40f),
            new PointF(70.00f, 40f),
            new PointF(70.00f, 100.00f),
            new PointF(30.00f, 100.00f),
            new PointF(30.00f, 40f),
            new PointF(0.00f, 40f),
            new PointF(50.00f, 0.00f)
         };        

        public static PointF[] downArrowPoints = new PointF[]
        {
            new PointF(50.00f, 100.00f),
            new PointF(100.00f,60f),
            new PointF(70.00f, 60f),
            new PointF(70.00f, 0.00f),
            new PointF(30.00f, 0.00f),
            new PointF(30.00f, 60f),
            new PointF(0.00f, 60f),
            new PointF(50.00f, 100.00f)
        };


        public static PointF[] leftArrowPoints = new PointF[]
        {
            new PointF(0.00f, 50.00f),
            new PointF(40f, 0.00f),
            new PointF(40f, 30.00f),
            new PointF(100.00f, 30.00f),
            new PointF(100.00f, 70.00f),
            new PointF(40f, 70.00f),
            new PointF(40f, 100.00f),
            new PointF(0.00f, 50.00f)
        };


        public static PointF[] rightArrowPoints = new PointF[]
        {
            new PointF(100.00f, 50.00f),
            new PointF(60f, 0.00f),
            new PointF(60f, 30.00f),
            new PointF(0.00f, 30.00f),
            new PointF(00.00f, 70.00f),
            new PointF(60f, 70.00f),
            new PointF(60f, 100.00f),
            new PointF(100.00f, 50.00f)
        };

        public static PointF[] diagonalCrossPoints = new PointF[]
        {
            new PointF(88.89f, 0f),
            new PointF(100f, 11.11f),
            new PointF(61.11f, 50f),
            new PointF(100f, 88.89f),
            new PointF(88.89f, 100f),
            new PointF(50f, 61.11f),
            new PointF(11.11f, 100f),
            new PointF(0f, 88.89f),
            new PointF(38.88f, 50f),
            new PointF(0f, 11.11f),
            new PointF(11.11f, 0f),
            new PointF(50f, 38.88f),
            new PointF(88.89f, 0f)

            //new PointF(70.71f, 0f),
            //new PointF(79.54f, 8.838f),
            //new PointF(48.61f, 39.77f),
            //new PointF(79.54f, 70.71f),
            //new PointF(70.71f, 79.54f),
            //new PointF(39.77f, 48.61f),
            //new PointF(8.838f, 79.54f),
            //new PointF(0f, 70.71f),
            //new PointF(30.93f, 39.77f),
            //new PointF(0f, 8.838f),
            //new PointF(8.838f, 0f),
            //new PointF(39.77f, 30.93f),
            //new PointF(70.71f, 0f)
        };


        public static PointF[] pentagonPoints = new PointF[]
        {
            new PointF(50.00f, 0.00f),
            new PointF(100.00f, 42.86f),
            new PointF(81.25f, 100.00f),
            new PointF(18.75f, 100.00f),
            new PointF(0.00f, 42.86f),
            new PointF(50.00f, 0.00f)
        };

        public static PointF[] hexagonPoints = new PointF[]
        {
            new PointF(100.00f, 50.00f),
            new PointF(75.00f, 100.00f),
            new PointF(25.00f, 100.00f),
            new PointF(0.00f, 50.00f),
            new PointF(25.00f, 0.00f),
            new PointF(75.00f, 0.00f),
            new PointF(100.00f, 50.00f)
        };


        public static PointF[] octagonPoints = new PointF[]
        {
            new PointF(66.67f, 0.00f),
            new PointF(100.00f, 33.33f),
            new PointF(100.00f, 66.67f),
            new PointF(66.67f, 100.00f),
            new PointF(33.33f, 100.00f),
            new PointF(0.00f, 66.67f),
            new PointF(0.00f, 33.33f),
            new PointF(33.33f, 0.00f),
            new PointF(66.67f, 0.00f)
        };

        public static PointF[] ScaledAndTranslated(PointF[] shape, float scale, float X, float Y)
        {

            PointF[] newShape = new PointF[shape.Length];
            int counter = 0;
            foreach (PointF p in shape)
            {
                newShape[counter].X = (p.X * scale) + X;
                newShape[counter].Y = (p.Y * scale) + Y;
                counter++;
            }

            return newShape;
        }

    }
}
