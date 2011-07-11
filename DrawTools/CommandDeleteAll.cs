using System;
using System.Collections.Generic;
using System.Text;

namespace DrawTools
{
    /// <summary>
    /// Delete All command
    /// </summary>
    class CommandDeleteAll : Command
    {
        List<DrawObject> cloneList;

        // Create this command BEFORE applying Delete All function.
        public CommandDeleteAll(GraphicsList graphicsList)
        {
            cloneList = new List<DrawObject>();

            // Make clone of the whole list.
            // Add objects in reverse order because GraphicsList.Add
            // insert every object to the beginning.
            int n = graphicsList.Count;

            for ( int i = n - 1; i >= 0; i-- )
            {
                cloneList.Add(graphicsList[i].Clone());
            }
        }

        public override void Undo(GraphicsList list)
        {
            // Add all objects from clone list to list -
            // opposite to DeleteAll
            foreach (DrawObject o in cloneList)
            {
                list.Add(o);
            }
        }

        public override void Redo(GraphicsList list)
        {
            // Clear list - make DeleteAll again
            list.Clear();
        }
    }
}
