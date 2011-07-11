#region Using directives

using System;
using System.Windows.Forms;
using System.Drawing;

#endregion

namespace DrawTools
{
    /// <summary>
    /// Helper class used to show properties
    /// for one or more graphic objects
    /// </summary>
    class GraphicsProperties
    {
        private Color? color;
        private int? penWidth;

        public GraphicsProperties()
        {
            color = null;
            penWidth = null;
        }

        public Color? Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public int? PenWidth
        {
            get
            {
                return penWidth;
            }
            set
            {
                penWidth = value;
            }
        }
    }
}
