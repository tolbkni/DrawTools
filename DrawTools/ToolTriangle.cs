using System;
using System.Windows.Forms;

namespace DrawTools
{
    /// <summary>
    /// Triangle tool
    /// </summary>
    class ToolTriangle : DrawTools.ToolRectangle
    {
        public ToolTriangle()
        {
            Cursor = new Cursor(GetType(), "Triangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawTriangle(e.X, e.Y, 1, 1));
        }
    }
}
