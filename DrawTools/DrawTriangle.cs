using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawTools
{
    /// <summary>
    /// Triangle graphic object
    /// </summary>
    class DrawTriangle : DrawTools.DrawRectangle
    {
        public DrawTriangle()
            : this(0, 0, 1, 1)
        {
        }

        public DrawTriangle(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone() {
            DrawTriangle drawTriangle = new DrawTriangle();
            drawTriangle.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawTriangle);
            return drawTriangle;
        }

        public override void Draw(Graphics g) {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color, PenWidth);
            Point[] pts = { new Point(Rectangle.X, Rectangle.Y + Rectangle.Height), 
                            new Point(Rectangle.X + Rectangle.Width / 2, Rectangle.Y), 
                            new Point(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height) };
            // version 1
            // use Graphics.DrawLine(int x, int y)
            /* 
             * pen.StartCap = LineCap.Round;
             * pen.EndCap = LineCap.Round;
             * g.DrawLine(pen, pts[0], pts[1]);
             * g.DrawLine(pen, pts[1], pts[2]);
             * g.DrawLine(pen, pts[0], pts[2]);
             */

            // version 2
            // use Graphics.DrawLines(Pen pen, Point[] points)
            /* 
             * g.DrawLines(pen, pts);
             */

            // version 3
            // use Graphics.DrawPath(Pen pen, GraphicsPath path)
            /* 
             * GraphicsPath path = new GraphicsPath();
             * path.AddLines(pts);
             * g.DrawPath(pen, path);
             */

            // version 4
            // use Graphics.DrawPolygon(Pen pen, Point[] points)
            g.DrawPolygon(pen, pts);

            pen.Dispose();
        }
    }
}
