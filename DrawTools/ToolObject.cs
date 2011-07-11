using System;
using System.Windows.Forms;
using System.Drawing;


namespace DrawTools
{
	/// <summary>
	/// Base class for all tools which create new graphic object
	/// </summary>
	abstract class ToolObject : DrawTools.Tool
	{
        private Cursor cursor;

        /// <summary>
        /// Tool cursor.
        /// </summary>
        protected Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }


        /// <summary>
        /// Left mouse is released.
        /// New object is created and resized.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.GraphicsList[0].Normalize();
            drawArea.AddCommandToHistory(new CommandAdd(drawArea.GraphicsList[0]));
            drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;

            drawArea.Capture = false;
            drawArea.Refresh();
        }

        /// <summary>
        /// Add new object to draw area.
        /// Function is called when user left-clicks draw area,
        /// and one of ToolObject-derived tools is active.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="o"></param>
        protected void AddNewObject(DrawArea drawArea, DrawObject o)
        {
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            drawArea.GraphicsList.Add(o);

            drawArea.Capture = true;
            drawArea.Refresh();

            drawArea.SetDirty();
        }
	}
}
