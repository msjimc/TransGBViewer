using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    public class CommonGraphicTasks
    {
        public static void DrawRectangle(Graphics g, RectangleF r, Brush color, bool rounded, scalar scaleDPI, int diameter, bool clip)
        {
            DrawRectangle(g, r, color, rounded, scaleDPI, diameter, -1, clip);           
        }
        public static void DrawRectangle(Graphics g, RectangleF r, Brush color, bool rounded, scalar scaleDPI, int diameter, int leftRight, bool clip)
        {
            diameter = scaleDPI.i[diameter];
            if (r.Width < scaleDPI.i[4])
            {
                if (clip == true)
                {
                    RectangleF flankingR = new RectangleF(r.X - scaleDPI.i[1], r.Y - scaleDPI.i[3], r.Width + scaleDPI.i[3], scaleDPI.i[3]);
                    g.FillRectangle(Brushes.White, flankingR);
                    flankingR.Y = r.Y + r.Height;
                    g.FillRectangle(Brushes.White, flankingR);
                }

                if (r.Width < scaleDPI.scale * 1.0f) { r.Width = scaleDPI.scale * 1.0f; }
                g.FillRectangle(color, r);
            }
            else if (rounded == false || leftRight == 3)
            {
                g.FillRectangle(color, r);
            }
            else if (rounded == true)
            {
                GraphicsPath path;
                if (leftRight == 1)
                { path = CreateRightRoundedRectangle(r, diameter); }
                else if (leftRight == 2)
                { path = CreateLeftRoundedRectangle(r, diameter); }
                else //if (leftRight == -1)
                { path = CreateRoundedRectangle(r, diameter); }

                g.FillPath(color, path);
            }
        }

        public static void DrawBordersRectangle(Graphics g, RectangleF r, Pen color, bool rounded, scalar scaleDPI, int diametert)
        {
            DrawBordersRectangle(g, r, color, rounded, scaleDPI, diametert,-1);
        }
        public static void DrawBordersRectangle(Graphics g, RectangleF r, Pen color, bool rounded, scalar scaleDPI, int diameter, int leftRight)
        {
            diameter = scaleDPI.i[diameter];
            if (r.Width < scaleDPI.i[4])
            {
                // skip
            }
            else if (rounded == false || leftRight == 3)
            {
                g.DrawRectangle(color, r);
            }
            else if (rounded == true)
            {
                GraphicsPath path;
                if (leftRight == 1)
                { path = CreateRightRoundedRectangle(r, diameter); }
                else if (leftRight == 2)
                { path = CreateLeftRoundedRectangle(r, diameter); }
                else //if (leftRight == -1)
                { path = CreateRoundedRectangle(r, diameter); }
                g.DrawPath(color, path);
            }
        }

        public static GraphicsPath CreateRoundedRectangle(RectangleF bounds, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = cornerRadius * 2;
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddLine(bounds.Left + cornerRadius, bounds.Top, bounds.Right - cornerRadius, bounds.Top);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddLine(bounds.Right, bounds.Top + cornerRadius, bounds.Right, bounds.Bottom - cornerRadius);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddLine(bounds.Right - cornerRadius, bounds.Bottom, bounds.Left + cornerRadius, bounds.Bottom);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.AddLine(bounds.Left, bounds.Bottom - cornerRadius, bounds.Left, bounds.Top + cornerRadius);
            path.CloseFigure();
            return path;
        }

        public static GraphicsPath CreateRightRoundedRectangle(RectangleF bounds, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = cornerRadius * 2;

            path.AddLine(bounds.Left, bounds.Top, bounds.Right - cornerRadius, bounds.Top);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddLine(bounds.Right, bounds.Top + cornerRadius, bounds.Right, bounds.Bottom - cornerRadius);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddLine(bounds.Right - cornerRadius, bounds.Bottom, bounds.Left, bounds.Bottom);
            path.AddLine(bounds.Left, bounds.Bottom, bounds.Left, bounds.Top);

            path.CloseFigure();
            return path;
        }

        public static GraphicsPath CreateLeftRoundedRectangle(RectangleF bounds, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = cornerRadius * 2;

            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddLine(bounds.Left + cornerRadius, bounds.Top, bounds.Right, bounds.Top);
            path.AddLine(bounds.Right, bounds.Top, bounds.Right, bounds.Bottom);
            path.AddLine(bounds.Right, bounds.Bottom, bounds.Left + cornerRadius, bounds.Bottom);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.AddLine(bounds.Left, bounds.Bottom - cornerRadius, bounds.Left, bounds.Top + cornerRadius);

            path.CloseFigure();
            return path;
        }

        public static void DrawArrow(Graphics g, RectangleF r, Brush color, bool rounded, bool isForward, scalar scaleDPI)
        {
            if (r.Width < scaleDPI.i[4])
            {
                if (r.Width < 1.0f * scaleDPI.scale) { r.Width = 1.0f * scaleDPI.scale; }
                g.FillRectangle(color, r);
            }
            else
            {
                GraphicsPath path = CreateRoundedArrow(r, scaleDPI.i[3], isForward, scaleDPI);
                g.FillPath(color, path);
            }
        }
        public static GraphicsPath CreateRoundedArrow(RectangleF bounds, int cornerRadius, bool isForward, scalar scaleDPI)
        {
            GraphicsPath path = new GraphicsPath();
            int four = scaleDPI.i[4];
            float midY = bounds.Top + bounds.Height / 2;
            if (isForward)
            {
                path.StartFigure();
                path.AddLine(bounds.Left, bounds.Top, bounds.Right - four, bounds.Top);
                path.AddLine(bounds.Right - four, bounds.Top, bounds.Right, midY);
                path.AddLine(bounds.Right, midY, bounds.Right - four, bounds.Bottom);
                path.AddLine(bounds.Right - four, bounds.Bottom, bounds.Left, bounds.Bottom);
                path.CloseFigure();
            }
            else
            {
                path.StartFigure();
                path.AddLine(bounds.Right, bounds.Top, bounds.Left + four, bounds.Top);
                path.AddLine(bounds.Left + four, bounds.Top, bounds.Left, midY);
                path.AddLine(bounds.Left, midY, bounds.Left + four, bounds.Bottom);
                path.AddLine(bounds.Left + four, bounds.Bottom, bounds.Right, bounds.Bottom);
                path.CloseFigure();
            }
            return path;
        }

        public static double GetAngleInRadians(int angle)
        {
            double radians = (angle / 180.0d) * (double)Math.PI;
            return radians;
        }

        public static SizeF RotateBy(SizeF position, double radians)
        {
            SizeF answer = new SizeF();
            if (radians == 0.0f)
            { answer = position; }
            else
            {
                answer.Width = (float)(Math.Cos(radians) * position.Width) - (float)(Math.Sin(radians) * position.Height);
                answer.Height = (float)(Math.Sin(radians) * position.Width) + (float)(Math.Cos(radians)) * position.Height;
            }
            return answer;
        }
    }
}
