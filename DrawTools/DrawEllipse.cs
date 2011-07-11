using System;
using System.Windows.Forms;
using System.Drawing;

namespace DrawTools
{
	/// <summary>
	/// Ellipse graphic object
	/// </summary>
	class DrawEllipse : DrawTools.DrawRectangle
	{
		public DrawEllipse() : this(0, 0, 1, 1)
		{
		}

        public DrawEllipse(int x, int y, int width, int height) : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawEllipse drawEllipse = new DrawEllipse();
            drawEllipse.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawEllipse);
            return drawEllipse;
        }


        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);

            g.DrawEllipse(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));

            pen.Dispose();
        }


	}
}
