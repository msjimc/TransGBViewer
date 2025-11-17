using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public class Shape
    {
        private ShapeType shapeType;
        private ShapeLocation shapeLocation;
        private float shapeScale;
        private int firstAA;
        private int x;
        private int y;
        private string sequencekey;
        private float drawHeight;
        private float boxWidth = 1.0f;
        private float boxHeight;    
        private Color fillColour = Color.Black;
        private float scaleFactor = 1.4f;
        private PointF[] shapePoints;
        public Shape()
        { }

        public Shape clone()
        {
            Shape shape = new Shape();
            shape.shapeType = shapeType;
            shape.shapeLocation = shapeLocation;
            shape.shapeScale = shapeScale;
            shape.firstAA = firstAA;
            shape.x = x;
            shape.y = y;
            shape.Sequencekey= sequencekey;
            shape.drawHeight = drawHeight;
            shape.boxWidth = boxWidth;
            shape.boxHeight = boxHeight;
            shape.fillColour = fillColour;
            shape.scaleFactor = scaleFactor;
            shape.shapePoints = shapePoints;
            return shape;
        }
        public PointF[] GetShapePoints(scalar scale, int Yoffset, int from)
        {
            shapePoints = shapes.squarePoints;
            switch (shapeType)
            {
                case ShapeType.ArrowDown:
                    shapePoints = shapes.downArrowPoints;
                    break;
                case ShapeType.ArrowLeft:
                    shapePoints = shapes.leftArrowPoints;
                    break;
                case ShapeType.ArrowRight:
                    shapePoints = shapes.rightArrowPoints;
                    break;
                case ShapeType.ArrowUp:
                    shapePoints = shapes.upArrowPoints;
                    break;
                case ShapeType.Box:
                    shapePoints = new PointF[1];
                    break;
                case ShapeType.Circle:
                    shapePoints = CreateCirclePoints();
                    break;
                case ShapeType.CrossDiagonal:
                    shapePoints = shapes.diagonalCrossPoints;
                    break;
                case ShapeType.CrossVertical:
                    shapePoints = shapes.crossPoints;
                    break;
                case ShapeType.Diamond:
                    shapePoints = shapes.diamondPoints;
                    break;
                case ShapeType.Hexagon:
                    shapePoints = shapes.hexagonPoints;
                    break;
                case ShapeType.Octagon:
                    shapePoints = shapes.octagonPoints;
                    break;
                case ShapeType.Pentagon:
                    shapePoints = shapes.pentagonPoints;
                    break;
                case ShapeType.RectangleHorizontal:
                    shapePoints = shapes.topHalfRectanglePoints;
                    break;
                case ShapeType.RectangleVertical:
                    shapePoints = shapes.leftHalfRectanglePoints;
                    break;
                case ShapeType.Square:
                    shapePoints = shapes.squarePoints;
                    break;
                case ShapeType.Star:
                    shapePoints = shapes.StarPoints;
                    break;
                case ShapeType.TriangleUp:
                    shapePoints = shapes.trianglePointsUp;
                    break;
                case ShapeType.TriangleDown:
                    shapePoints = shapes.trianglePointsDown;
                    break;
                case ShapeType.TriangleLeft:
                    shapePoints = shapes.trianglePointsLeft;
                    break;
                case ShapeType.TriangleRight:
                    shapePoints = shapes.trianglePointsRight;
                    break;
                case ShapeType.UnderLine:
                    shapePoints = new PointF[1];
                    break;
                default:

                    break;
            }

            if (shapePoints.Length == 1)
            { return shapePoints; }
            else
            {
                int thirteen = scale.i[13];
                int fourteen = scale.i[14];
                float xPlace = 0;
                if (x < 0)
                { xPlace = -scale.f[-x] + ((firstAA - from) * thirteen); }
                else
                { xPlace = scale.f[x] + ((firstAA - from) * thirteen); }
                xPlace += thirteen;

                float yPlace = (drawHeight * fourteen);
                if (y < 0) { yPlace += -(scale.f[-y]); }
                else { yPlace += (scale.f[y]); }

                yPlace += Yoffset;

                float scaleinterface = 1.0f / 14;
                scaleinterface = scaleFactor * scaleinterface * scale.scale;
                PointF[] shapePointsAdjusted = shapes.ScaledAndTranslated(shapePoints, scaleinterface, xPlace, yPlace);
                return shapePointsAdjusted;
            }
        }

        public static PointF[] CreateCirclePoints()
        {
            float centerX = 50;
            float centerY = 50;
            float radius = 50;
            int pointCount = 50;
            PointF[] points = new PointF[pointCount];
            double angleStep = 2 * Math.PI / pointCount;

            for (int i = 0; i < pointCount; i++)
            {
                double angle = i * angleStep;
                float x = centerX + radius * (float)Math.Cos(angle);
                float y = centerY + radius * (float)Math.Sin(angle);
                points[i] = new PointF(x, y);
            }
            return points;
        }        

        public ShapeType ShapeType { get => shapeType; set => shapeType = value; }
        public ShapeLocation ShapeLocation { get => shapeLocation; set => shapeLocation = value; }
        public float ShapeScale { get => shapeScale; set => shapeScale = value; }
        public int ShapeFirstAA { get => firstAA; set => firstAA = value; }
        public int ShapeXoffset { get => x; set => x = value; }
        public int ShapeYoffset { get => y; set => y = value; }
        public string Sequencekey { get => sequencekey; set => sequencekey = value; }
        public float DrawHeight { get => drawHeight; set => drawHeight = value; }
        public float BoxWidth { get => boxWidth; set => boxWidth = value; }
        public float BoxHeight { get => boxHeight; set => boxHeight = value; }
        public Color FillColour { get => fillColour; set => fillColour = value; }
        public float ScaleFactor { get => scaleFactor; set => scaleFactor = value; }
        public PointF[] ShapePoints {  get => shapePoints; set => shapePoints = value; }

    }
}
