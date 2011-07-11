#region Using directives

using System;
using System.Windows.Forms;
using System.Diagnostics;

#endregion

namespace DocToolkit
{
    /// <summary>
    /// DragDropManager class allows to open files dropped from 
    /// Windows Explorer in Windows Form application.
    /// 
    /// Using:
    /// 1) Write function which opens file selected from MRU:
    ///
    /// private void dragDropManager_FileDroppedEvent(object sender, FileDroppedEventArgs e)
    /// {
    ///    // open file(s) from e.FileArray:
    ///    // e.FileArray.GetValue(0).ToString() ...
    /// }
    ///     
    /// 2) Add member of this class to the parent form:
    ///  
    /// private DragDropManager dragDropManager;
    /// 
    /// 3) Create class instance in parent form initialization code:
    ///  
    /// dragDropManager = new DragDropManager(this);
    /// dragDropManager.FileDroppedEvent += dragDropManager_FileDroppedEvent; 
    /// 
    /// </summary>
    public class DragDropManager
    {
        private Form frmOwner;          // reference to owner form

        // Event raised when drops file(s) to the form
        public event FileDroppedEventHandler FileDroppedEvent;

        public DragDropManager(Form owner)
        {
            frmOwner = owner;

            // ensure that parent form allows dropping
            frmOwner.AllowDrop = true;

            // subscribe to parent form's drag-drop events
            frmOwner.DragEnter += OnDragEnter;
            frmOwner.DragDrop += OnDragDrop;
        }


        /// <summary>
        /// Handle parent form DragEnter event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // If file is dragged, show cursor "Drop allowed"
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Handle parent form DragDrop event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // When file(s) are dragged from Explorer to the form, IDataObject
            // contains array of file names. If one file is dragged,
            // array contains one element.
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

            if (a != null)
            {
                if (FileDroppedEvent != null)
                {
                    // Raise event asynchronously.
                    // Explorer instance from which file is dropped is not responding
                    // all the time when DragDrop handler is active, so we need to return
                    // immidiately (especially if OpenFiles shows MessageBox).

                    FileDroppedEvent.BeginInvoke(this, new FileDroppedEventArgs(a), null, null);

                    frmOwner.Activate();        // in the case Explorer overlaps parent form
                }
            }

            // NOTE: exception handling is not used here.
            // Caller responsibility is to handle exceptions 
            // in the function invoked by FileDroppedEvent.
        }
    }

    public delegate void FileDroppedEventHandler(object sender, FileDroppedEventArgs e);

    public class FileDroppedEventArgs : System.EventArgs
    {
        private Array fileArray;

        public FileDroppedEventArgs(Array array)
        {
            this.fileArray = array;
        }

        public Array FileArray
        {
            get
            {
                return fileArray;
            }
        }
    }
}

