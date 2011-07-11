using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Security;
using Microsoft.Win32;

using DocToolkit;


namespace DrawTools
{
    partial class MainForm : Form
    {
        #region Members

        private DrawArea drawArea;
        private DocManager docManager;
        private DragDropManager dragDropManager;
        private MruManager mruManager;
        private PersistWindowState persistState;

        private string argumentFile = "";   // file name from command line

        const string registryPath = "Software\\AlexF\\DrawTools";

        #endregion

        #region Properties

        /// <summary>
        /// File name from the command line
        /// </summary>
        public string ArgumentFile
        {
            get
            {
                return argumentFile;
            }
            set
            {
                argumentFile = value;
            }
        }

        /// <summary>
        /// Get reference to Edit menu item.
        /// Used to show context menu in DrawArea class.
        /// </summary>
        /// <value></value>
        public ToolStripMenuItem ContextParent
        {
            get
            {
                return editToolStripMenuItem;
            }
        }

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            persistState = new PersistWindowState(registryPath, this);
        }

        #endregion

        #region Toolbar Event Handlers

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            CommandNew();
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            CommandOpen();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            CommandSave();
        }

        private void toolStripButtonPointer_Click(object sender, EventArgs e)
        {
            CommandPointer();
        }

        private void toolStripButtonRectangle_Click(object sender, EventArgs e)
        {
            CommandRectangle();
        }

        private void toolStripButtonEllipse_Click(object sender, EventArgs e)
        {
            CommandEllipse();
        }

        private void toolStripButtonLine_Click(object sender, EventArgs e)
        {
            CommandLine();
        }

        private void toolStripButtonPencil_Click(object sender, EventArgs e)
        {
            CommandPolygon();
        }

        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            CommandAbout();
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            CommandUndo();
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            CommandRedo();
        }

        #endregion Toolbar Event Handlers

        #region Menu Event Handlers

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandOpen();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.GraphicsList.SelectAll();
            drawArea.Refresh();

        }

        private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawArea.GraphicsList.UnselectAll();
            drawArea.Refresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandDelete command = new CommandDelete(drawArea.GraphicsList);

            if (drawArea.GraphicsList.DeleteSelection())
            {
                drawArea.SetDirty();
                drawArea.Refresh();
                drawArea.AddCommandToHistory(command);
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandDeleteAll command = new CommandDeleteAll(drawArea.GraphicsList);

            if (drawArea.GraphicsList.Clear())
            {
                drawArea.SetDirty();
                drawArea.Refresh();
                drawArea.AddCommandToHistory(command);
            }
        }

        private void moveToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawArea.GraphicsList.MoveSelectionToFront())
            {
                drawArea.SetDirty();
                drawArea.Refresh();
            }

        }

        private void moveToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawArea.GraphicsList.MoveSelectionToBack())
            {
                drawArea.SetDirty();
                drawArea.Refresh();
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawArea.GraphicsList.ShowPropertiesDialog(drawArea))
            {
                drawArea.SetDirty();
                drawArea.Refresh();
            }

        }

        private void pointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandPointer();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandRectangle();
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandEllipse();
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandLine();
        }

        private void pencilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandPolygon();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandAbout();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandUndo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandRedo();
        }

        #endregion Menu Event Handlers

        #region DocManager Event Handlers

        /// <summary>
        /// Load document from the stream supplied by DocManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void docManager_LoadEvent(object sender, SerializationEventArgs e)
        {
            // DocManager asks to load document from supplied stream
            try
            {
                drawArea.GraphicsList = (GraphicsList)e.Formatter.Deserialize(e.SerializationStream);
            }
            catch (ArgumentNullException ex)
            {
                HandleLoadException(ex, e);
            }
            catch (SerializationException ex)
            {
                HandleLoadException(ex, e);
            }
            catch (SecurityException ex)
            {
                HandleLoadException(ex, e);
            }
        }


        /// <summary>
        /// Save document to stream supplied by DocManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void docManager_SaveEvent(object sender, SerializationEventArgs e)
        {
            // DocManager asks to save document to supplied stream
            try
            {
                e.Formatter.Serialize(e.SerializationStream, drawArea.GraphicsList);
            }
            catch (ArgumentNullException ex)
            {
                HandleSaveException(ex, e);
            }
            catch (SerializationException ex)
            {
                HandleSaveException(ex, e);
            }
            catch (SecurityException ex)
            {
                HandleSaveException(ex, e);
            }
        }

        #endregion

        #region Event Handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Create draw area
            drawArea = new DrawArea();
            drawArea.Location = new System.Drawing.Point(0, 0);
            drawArea.Size = new System.Drawing.Size(10, 10);
            drawArea.Owner = this;
            this.Controls.Add(drawArea);

            // Helper objects (DocManager and others)
            InitializeHelperObjects();

            drawArea.Initialize(this, docManager);
            ResizeDrawArea();

            LoadSettingsFromRegistry();

            // Submit to Idle event to set controls state at idle time
            Application.Idle += delegate(object o, EventArgs a)
            {
                SetStateOfControls();
            };

            // Open file passed in the command line
            if (ArgumentFile.Length > 0)
                OpenDocument(ArgumentFile);

            // Subscribe to DropDownOpened event for each popup menu
            // (see details in MainForm_DropDownOpened)
            foreach (ToolStripItem item in menuStrip1.Items)
            {
                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ((ToolStripMenuItem)item).DropDownOpened += MainForm_DropDownOpened;
                }
            }
        }

        /// <summary>
        /// Resize draw area when form is resized
        /// </summary>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized  &&
                drawArea != null )
            {
                ResizeDrawArea();
            }
        }

        /// <summary>
        /// Form is closing
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( e.CloseReason == CloseReason.UserClosing )
            {
                if (!docManager.CloseDocument())
                    e.Cancel = true;
            }

            SaveSettingsToRegistry();
        }

        /// <summary>
        /// Popup menu item (File, Edit ...) is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_DropDownOpened(object sender, EventArgs e)
        {
            // Reset active tool to pointer.
            // This prevents bug in rare case when non-pointer tool is active, user opens
            // main main menu and after this clicks in the drawArea. MouseDown event is not
            // raised in this case (why ??), and MouseMove event works incorrectly.
            drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
        }


        #endregion Event Handlers

        #region Other Functions

        /// <summary>
        /// Set state of controls.
        /// Function is called at idle time.
        /// </summary>
        public void SetStateOfControls()
        {
            // Select active tool
            toolStripButtonPointer.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Pointer);
            toolStripButtonRectangle.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Rectangle);
            toolStripButtonEllipse.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Ellipse);
            toolStripButtonLine.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Line);
            toolStripButtonPencil.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Polygon);

            pointerToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Pointer);
            rectangleToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Rectangle);
            ellipseToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Ellipse);
            lineToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Line);
            pencilToolStripMenuItem.Checked = (drawArea.ActiveTool == DrawArea.DrawToolType.Polygon);

            bool objects = (drawArea.GraphicsList.Count > 0);
            bool selectedObjects = (drawArea.GraphicsList.SelectionCount > 0);

            // File operations
            saveToolStripMenuItem.Enabled = objects;
            toolStripButtonSave.Enabled = objects;
            saveAsToolStripMenuItem.Enabled = objects;

            // Edit operations
            deleteToolStripMenuItem.Enabled = selectedObjects;
            deleteAllToolStripMenuItem.Enabled = objects;
            selectAllToolStripMenuItem.Enabled = objects;
            unselectAllToolStripMenuItem.Enabled = objects;
            moveToFrontToolStripMenuItem.Enabled = selectedObjects;
            moveToBackToolStripMenuItem.Enabled = selectedObjects;
            propertiesToolStripMenuItem.Enabled = selectedObjects;

            // Undo, Redo
            undoToolStripMenuItem.Enabled = drawArea.CanUndo;
            toolStripButtonUndo.Enabled = drawArea.CanUndo;

            redoToolStripMenuItem.Enabled = drawArea.CanRedo;
            toolStripButtonRedo.Enabled = drawArea.CanRedo;
        }

        /// <summary>
        /// Set draw area to all form client space except toolbar
        /// </summary>
        private void ResizeDrawArea()
        {
            Rectangle rect = this.ClientRectangle;

            drawArea.Left = rect.Left;
            drawArea.Top = rect.Top + menuStrip1.Height + toolStrip1.Height;
            drawArea.Width = rect.Width;
            drawArea.Height = rect.Height - menuStrip1.Height - toolStrip1.Height; ;
        }

        /// <summary>
        /// Initialize helper objects from the DocToolkit Library.
        /// 
        /// Called from Form1_Load. Initialized all objects except
        /// PersistWindowState wich must be initialized in the
        /// form constructor.
        /// </summary>
        private void InitializeHelperObjects()
        {
            // DocManager

            DocManagerData data = new DocManagerData();
            data.FormOwner = this;
            data.UpdateTitle = true;
            data.FileDialogFilter = "DrawTools files (*.dtl)|*.dtl|All Files (*.*)|*.*";
            data.NewDocName = "Untitled.dtl";
            data.RegistryPath = registryPath;

            docManager = new DocManager(data);
            docManager.RegisterFileType("dtl", "dtlfile", "DrawTools File");

            // Subscribe to DocManager events.
            docManager.SaveEvent += docManager_SaveEvent;
            docManager.LoadEvent += docManager_LoadEvent;

            // Make "inline subscription" using anonymous methods.
            docManager.OpenEvent += delegate(object sender, OpenFileEventArgs e)
            {
                // Update MRU List
                if (e.Succeeded)
                    mruManager.Add(e.FileName);
                else
                    mruManager.Remove(e.FileName);
            };

            docManager.DocChangedEvent += delegate(object o, EventArgs e)
            {
                drawArea.Refresh();
                drawArea.ClearHistory();
            };

            docManager.ClearEvent += delegate(object o, EventArgs e)
            {
                if (drawArea.GraphicsList != null)
                {
                    drawArea.GraphicsList.Clear();
                    drawArea.ClearHistory();
                    drawArea.Refresh();
                }
            };

            docManager.NewDocument();

            // DragDropManager
            dragDropManager = new DragDropManager(this);
            dragDropManager.FileDroppedEvent += delegate(object sender, FileDroppedEventArgs e)
            {
                OpenDocument(e.FileArray.GetValue(0).ToString());
            };

            // MruManager
            mruManager = new MruManager();
            mruManager.Initialize(
                this,                              // owner form
                recentFilesToolStripMenuItem,      // Recent Files menu item
                fileToolStripMenuItem,            // parent
                registryPath);                     // Registry path to keep MRU list

            mruManager.MruOpenEvent += delegate(object sender, MruFileOpenEventArgs e)
            {
                OpenDocument(e.FileName);
            };
        }
        /// <summary>
        /// Handle exception from docManager_LoadEvent function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        private void HandleLoadException(Exception ex, SerializationEventArgs e)
        {
            MessageBox.Show(this,
                "Open File operation failed. File name: " + e.FileName + "\n" +
                "Reason: " + ex.Message,
                Application.ProductName);

            e.Error = true;
        }

        /// <summary>
        /// Handle exception from docManager_SaveEvent function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        private void HandleSaveException(Exception ex, SerializationEventArgs e)
        {
            MessageBox.Show(this,
                "Save File operation failed. File name: " + e.FileName + "\n" +
                "Reason: " + ex.Message,
                Application.ProductName);

            e.Error = true;
        }

        /// <summary>
        /// Open document.
        /// Used to open file passed in command line or dropped into the window
        /// </summary>
        /// <param name="file"></param>
        public void OpenDocument(string file)
        {
            docManager.OpenDocument(file);
        }

        /// <summary>
        /// Load application settings from the Registry
        /// </summary>
        void LoadSettingsFromRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);

                DrawObject.LastUsedColor = Color.FromArgb((int)key.GetValue(
                    "Color",
                    Color.Black.ToArgb()));

                DrawObject.LastUsedPenWidth = (int)key.GetValue(
                    "Width",
                    1);
            }
            catch (ArgumentNullException ex)
            {
                HandleRegistryException(ex);
            }
            catch (SecurityException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleRegistryException(ex);
            }
        }

        /// <summary>
        /// Save application settings to the Registry
        /// </summary>
        void SaveSettingsToRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);

                key.SetValue("Color", DrawObject.LastUsedColor.ToArgb());
                key.SetValue("Width", DrawObject.LastUsedPenWidth);
            }
            catch (SecurityException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleRegistryException(ex);
            }
        }

        private void HandleRegistryException(Exception ex)
        {
            Trace.WriteLine("Registry operation failed: " + ex.Message);
        }

        /// <summary>
        /// Set Pointer draw tool
        /// </summary>
        private void CommandPointer()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandRectangle()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Rectangle;
        }

        /// <summary>
        /// Set Ellipse draw tool
        /// </summary>
        private void CommandEllipse()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Ellipse;
        }

        /// <summary>
        /// Set Line draw tool
        /// </summary>
        private void CommandLine()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Line;
        }

        /// <summary>
        /// Set Polygon draw tool
        /// </summary>
        private void CommandPolygon()
        {
            drawArea.ActiveTool = DrawArea.DrawToolType.Polygon;
        }

        /// <summary>
        /// Show About dialog
        /// </summary>
        private void CommandAbout()
        {
            FrmAbout frm = new FrmAbout();
            frm.ShowDialog(this);
        }

        /// <summary>
        /// Open new file
        /// </summary>
        private void CommandNew()
        {
            docManager.NewDocument();
        }

        /// <summary>
        /// Open file
        /// </summary>
        private void CommandOpen()
        {
            docManager.OpenDocument("");
        }

        /// <summary>
        /// Save file
        /// </summary>
        private void CommandSave()
        {
            docManager.SaveDocument(DocManager.SaveType.Save);
        }

        /// <summary>
        /// Save As
        /// </summary>
        private void CommandSaveAs()
        {
            docManager.SaveDocument(DocManager.SaveType.SaveAs);
        }

        /// <summary>
        /// Undo
        /// </summary>
        private void CommandUndo()
        {
            drawArea.Undo();
        }

        /// <summary>
        /// Redo
        /// </summary>
        private void CommandRedo()
        {
            drawArea.Redo();
        }

        #endregion
    }
}