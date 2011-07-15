using System;
using System.Windows.Forms;
using System.Drawing;


namespace DrawTools
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolRectangle : DrawTools.ToolObject
	{

		public ToolRectangle()
		{
            Cursor = new Cursor(GetType(), "Rectangle.cur");
		}

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawRectangle(e.X, e.Y, 1, 1));
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                Point point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 5);
                drawArea.Refresh();
            }
        }
	}
}
