using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public class GeneFeatureMarker
    {
        private float x;
        private float y;
        private float width = 20;
        private float height = 20;
        private Color colour = Color.Black;
        private PointF[] shapePoints;
        private ShapeType shapeType = ShapeType.NotSet;
        private bool drawMe = false;
        private bool solidFill = true;
        private int viewFrom = 0;
        private int viewTo = 0;
        private float place = 0.0f;
        private string linkedName = "";
        private int currentTop = 0;
        private int currentLeft = 0;
        private int basePlace = int.MinValue;
        private float[] mRNABasePlace = { float.MinValue, float.MinValue };

        public GeneFeatureMarker() { }

        public PointF[] GetResizedfeaturePoints(scalar scale)
        {
            PointF[] resizedPoints = new PointF[ShapePoints.Length];
            float widthScale = (float)width / 100.0f;
            float heightScale = (float)height / 100.0f;

            for (int i = 0; i < ShapePoints.Length; i++)
            {
                float newX = (ShapePoints[i].X * widthScale * scale.scale) + x ;
                float newY = (ShapePoints[i].Y * heightScale * scale.scale) + scale.scale * y;
                resizedPoints[i] = new PointF(newX, newY);
            }
            return resizedPoints;
        }

        public GeneFeatureMarker Clone()
        {
            GeneFeatureMarker copy = new GeneFeatureMarker();
            copy.X = x;
            copy.Y = y;
            copy.Width = width;
            copy.Height = height;
            copy.Colour = colour;
            copy.ShapePoints = shapePoints;
            copy.SetShapePoints(shapeType);
            copy.DrawMe = drawMe;
            copy.ViewFrom = viewFrom;
            copy.CurrentTop = currentTop;
            copy.CurrentLeft = currentLeft;
            copy.ViewTo = viewTo;
            copy.LinkedName = linkedName;
            copy.Place = place;
            copy.SolidFill = solidFill;
            return copy;
        }

        public void SetShapePoints(ShapeType ShapeType)
        {
            shapePoints = new PointF[1];
            shapeType = ShapeType;

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
                    shapePoints = new PointF[4];
                    shapePoints[0] = new PointF(0.0f, 0.0f);
                    shapePoints[1] = new PointF(0.0f, 100.0f);
                    shapePoints[2] = new PointF(100.0f, 100.0f);
                    shapePoints[3] = new PointF(100.0f, 0.0f);
                    break;
                case ShapeType.Circle:
                    shapePoints = Shape.CreateCirclePoints();
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
                case ShapeType.VerticalLine:
                    shapePoints = new PointF[4];
                    shapePoints[0] = new PointF(0.0f, 0.0f);
                    shapePoints[1] = new PointF(0.0f, 100.0f);
                    shapePoints[2] = new PointF(10.0f, 100.0f);
                    shapePoints[3] = new PointF(10.0f, 0.0f);
                    break;
                default:

                    break;
            }
        }

        public void SetLinkedItem(string linkedItem, int from, int to, int Top)
        {
            linkedName = linkedItem;
            viewFrom = from;
            viewTo = to;
        }

        public void UpdateTop(int Top, scalar scaleDPI)
        {
            Top = (int)((float)Top / scaleDPI.scale);
            y -= currentTop;
            currentTop = Top;
            y += currentTop;
        }
        public void UpdateTop(int Top, int Left, scalar scaleDPI)
        {
            Top = (int)((float)Top / scaleDPI.scale);
            y -= currentTop;
            currentTop = Top;
            y += currentTop;

            Left = (int)((float)Left / scaleDPI.scale);
            if (currentLeft == 0)
            { currentLeft = Left; }
            else
            {
                x -= currentLeft;
                currentLeft = Left;
                x += currentLeft;
            }
        }

        public string LinkedName { get => linkedName; set => linkedName = value; }
        public int ViewFrom { get => viewFrom; set => viewFrom = value; }
        public int ViewTo { get => viewTo; set => viewTo = value; }
        public float Place { get => place; set => place = value; }        
        public float X { get { return x; } set { x = value; } }
        public float Y { get => y; set => y = value; }
        public int CurrentTop { get => currentTop; set => currentTop = value; }
        public int CurrentLeft { get => currentLeft ; set => currentLeft = value; }
        public float Width { get => width; set => width = value; }
        public float Height { get => height; set => height = value; }
        public int BasePlace { get => basePlace; set => basePlace = value; }
        public float[] MRNABasePlace { get => mRNABasePlace; set => mRNABasePlace = value; }
        public Color Colour { get => colour; set => colour = value; }
        public PointF[] ShapePoints { get => shapePoints; set => shapePoints = value; }
        public bool SolidFill { get => solidFill; set => solidFill = value; }   
        public ShapeType ShapeType { get => shapeType; }
        public bool DrawMe { get => drawMe; set => drawMe = value; }
    }
}
