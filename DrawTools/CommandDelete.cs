using System;
using System.Collections.Generic;
using System.Text;

namespace DrawTools
{
    /// <summary>
    /// Delete command
    /// </summary>
    class CommandDelete : Command
    {
        List<DrawObject> cloneList;    // contains selected items which are deleted
        List<int> indexList;           // contains index of selected items which are deleted

        // Create this command BEFORE applying Delete All function.
        public CommandDelete(GraphicsList graphicsList)
        {
            cloneList = new List<DrawObject>();
            indexList = new List<int>();

            // Make clone of the list selection.

            int n = graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (graphicsList[i].Selected)
                {
                    cloneList.Add(graphicsList[i].Clone());
                    indexList.Add(i);
                }
            }
        }

        public override void Undo(GraphicsList list)
        {
            list.UnselectAll();

            // Add all objects from cloneList to list.

            int n = cloneList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                list.Insert(indexList[i], cloneList[i]);
            }
        }

        public override void Redo(GraphicsList list)
        {
            // Delete from list all objects kept in cloneList

            int n = indexList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                list.RemoveAt(indexList[i]);
            }
        }
    }
}
