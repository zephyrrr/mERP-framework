using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Feng.Windows.Forms
{
    public partial class BookmarkManagerForm : Form
    {
        public BookmarkManagerForm()
        {
            InitializeComponent();

            this.imageListTreeView.Images.Add(Feng.Windows.ImageResource.Get("Feng", "Icons.iconFolder.png").Reference);
            this.imageListTreeView.Images.Add(Feng.Windows.ImageResource.Get("Feng", "Icons.iconBookmark.png").Reference);

            timer.Interval = 200;
            timer.Tick += new EventHandler(timer_Tick);
        }

        public static BookmarkInfo LoadBookmark(string bookmarkFileName)
        {
            if (string.IsNullOrEmpty(bookmarkFileName))
                return null;
            if (!System.IO.File.Exists(bookmarkFileName))
                return null;
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(bookmarkFileName);

                BookmarkInfo info = new BookmarkInfo();
                info.Name = dom.DocumentElement.Name;
                info.IsFolder = true;
                info.Childs = new List<BookmarkInfo>();

                LoadNode(dom.DocumentElement, info);
                return info;
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                System.IO.File.Delete(bookmarkFileName);
                return null;
            }
        }

        private static void LoadNode(XmlNode inXmlNode, BookmarkInfo parentInfo)
        {
            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                parentInfo.IsFolder = true;
                XmlNodeList nodeList = inXmlNode.ChildNodes;
                for (int i = 0; i <= nodeList.Count - 1; i++)
                {
                    XmlNode xNode = nodeList[i];

                    BookmarkInfo subInfo = new BookmarkInfo();
                    if (xNode.Attributes["Name"] != null)
                    {
                        subInfo.Name = xNode.Attributes["Name"].Value;
                        subInfo.Address = xNode.Attributes["Address"].Value;
                        subInfo.Description = xNode.Attributes["Description"].Value;
                        subInfo.IsFolder = Convert.ToBoolean(xNode.Attributes["IsFolder"].Value);
                        subInfo.Childs = new List<BookmarkInfo>();
                        parentInfo.Childs.Add(subInfo);

                        LoadNode(xNode, subInfo);
                    }
                }
            }
            else
            {
                XmlNode xNode = inXmlNode;
                parentInfo.IsFolder = false;
                //BookmarkInfo newInfo = new BookmarkInfo();
                if (xNode.Attributes["Name"] != null)
                {
                    parentInfo.Name = xNode.Attributes["Name"].Value;
                    parentInfo.Address = xNode.Attributes["Address"].Value;
                    parentInfo.Description = xNode.Attributes["Description"].Value;
                    parentInfo.IsFolder = Convert.ToBoolean(xNode.Attributes["IsFolder"].Value);
                }
            }
        }

        public static void SaveBookmark(BookmarkInfo info, string bookmarkFileName)
        {
            XmlDocument dom = new XmlDocument();

            // Write down the XML declaration
            XmlDeclaration xmlDeclaration = dom.CreateXmlDeclaration("1.0", "utf-8", null);

            // Create the root element
            XmlElement rootNode = dom.CreateElement("RootBookmark");
            dom.InsertBefore(xmlDeclaration, dom.DocumentElement);
            dom.AppendChild(rootNode);

            SaveNode(rootNode, info);

            dom.Save(bookmarkFileName);
        }
        private static void SaveNode(XmlNode inXmlNode, BookmarkInfo parentInfo)
        {
            if (parentInfo == null)
                return;

            foreach (BookmarkInfo subInfo in parentInfo.Childs)
            {
                /*invalid XML Character Replaced With
                    "<"  "&lt;"
                    ">"  "&gt;"
                    "\"" "&quot;"
                    "\'" "&apos;"
                    "&" "&amp;" */

                // 不需要Escape。System.Security.SecurityElement.Escape(subInfo.Name)

                // Create a new <Category> element and add it to the root node
                XmlElement parentNode = inXmlNode.OwnerDocument.CreateElement("Bookmark" + System.Guid.NewGuid().ToString());
                
                // Set attribute name and value!
                parentNode.SetAttribute("Name", subInfo.Name);
                parentNode.SetAttribute("Address", subInfo.Address);
                parentNode.SetAttribute("Description", subInfo.Description);
                parentNode.SetAttribute("IsFolder", subInfo.IsFolder.ToString());

                inXmlNode.AppendChild(parentNode);

                SaveNode(parentNode, subInfo);
            }
        }

        private string m_bookmarkFile = SystemDirectory.UserDataDirectory + "\\bookmark.xml";
        private const string rootNodeName = "书签";
        private BookmarkInfo m_rootBookmarkInfo;
        private void BookmarkManagerForm_Load(object sender, EventArgs e)
        {
            m_rootBookmarkInfo = LoadBookmark(m_bookmarkFile);
            if (m_rootBookmarkInfo == null)
                return;

            treeViewBookMark.Nodes.Clear();
            treeViewBookMark.Nodes.Add(new TreeNode(rootNodeName));
            TreeNode tNode = treeViewBookMark.Nodes[0];
            tNode.Tag = m_rootBookmarkInfo;

            // SECTION 3. Populate the TreeView with the DOM nodes.
            AddNode(m_rootBookmarkInfo, tNode);

            treeViewBookMark.SelectedNode = treeViewBookMark.Nodes[0];
            selected_node = treeViewBookMark.Nodes[0];
            treeViewBookMark.SelectedNode.Expand();
        }

        void BookmarkManagerForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            SaveBookmark(m_rootBookmarkInfo, m_bookmarkFile);

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Creates treeView
        /// References : 
        /// ms-help://MS.MSDNQTR.v90.en/enu_kbnetframeworkkb/netframeworkkb/317597.htm
        /// http://support.microsoft.com/kb/317597
        /// </summary>
        private void AddNode(BookmarkInfo parentInfo, TreeNode parentNode)
        {
            try
            {
                // Loop through the XML nodes until the leaf is reached.
                // Add the nodes to the TreeView during the looping process.
                if (parentInfo.IsFolder)
                {
                    foreach (BookmarkInfo subInfo in parentInfo.Childs)
                    {
                        TreeNode subNode;
                        subNode = new TreeNode(subInfo.Name);
                        subNode.Name = subInfo.Name + Guid.NewGuid().ToString();
                        subNode.Tag = subInfo;

                        if (subInfo.IsFolder)
                        {
                            subNode.ImageIndex = 0;
                            subNode.SelectedImageIndex = 0;
                        }
                        else
                        {
                            subNode.ImageIndex = 1;
                            subNode.SelectedImageIndex = 1;
                        }

                        parentNode.Nodes.Add(subNode);
                        
                        AddNode(subInfo, subNode);
                    }
                }
            }
            catch (Exception)
            {
                treeViewBookMark.Nodes.Clear();
                this.treeViewBookMark.Enabled = false;

                //StackFrame file_info = new StackFrame(true);
                //error(ref file_info, ex.Message);
                return;
            }
        }

        private void UpdateNode(TreeNode node)
        {
            if (node == null)
                return;
            BookmarkInfo info = node.Tag as BookmarkInfo;
            if (!string.IsNullOrEmpty(textBoxName.Text))
            {
                info.Name = textBoxName.Text.Trim();
            }
            info.Description = textBoxDesc.Text.Trim();
            info.Address = textBoxHref.Text.Trim();

            treeViewBookMark.SelectedNode = selected_node;
            treeViewBookMark.SelectedNode.Text = textBoxName.Text.Trim();
        }

        /// <summary>
        /// stores current treeView selected node
        /// </summary>
        private TreeNode selected_node;
        
        private void textBox_Validated(object sender, EventArgs e)
        {
            UpdateNode(selected_node);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateNode(selected_node);
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void deleteItem_Click(object sender, EventArgs e)
        {
            if (MessageForm.ShowYesNo("是否确认删除?"))
                deleteNodes(this.selected_node);
        }

        private void DeleteBookmarkInfo(BookmarkInfo toDeleteInfo, BookmarkInfo parentInfo)
        {
            if (parentInfo.IsFolder)
            {
                for (int i = 0; i < parentInfo.Childs.Count; ++i)
                {
                    if (parentInfo.Childs[i] == toDeleteInfo)
                    {
                        parentInfo.Childs.RemoveAt(i);
                        return;
                    }
                    DeleteBookmarkInfo(toDeleteInfo, parentInfo.Childs[i]);
                }
            }
        }

        /// <summary>
        /// Deletes some nodes from XML file
        /// </summary>
        private void deleteNodes(TreeNode node)
        {
            BookmarkInfo info = node.Tag as BookmarkInfo;
            DeleteBookmarkInfo(info, m_rootBookmarkInfo);

            node.Parent.Nodes.Remove(node);
        }

        private BookmarkInfo GetBookmarkInfo(TreeNode node)
        {
            return node.Tag as BookmarkInfo;
        }

        private void newFolderItem_Click(object sender, EventArgs e)
        {
            createNewFolder(selected_node);
        }

        private void createNewFolder(TreeNode node)
        {
            CreateNew(node, "新文件夹", true);
        }

        private void newBookmarkItem_Click(object sender, EventArgs e)
        {
            createNewBookmark(this.selected_node);
        }

        private void createNewBookmark(TreeNode node)
        {
            CreateNew(node, "新书签", false);
        }

        private void CreateNew(TreeNode node, string text, bool isFolder)
        {
            BookmarkInfo newInfo = new BookmarkInfo();
            newInfo.Name = text;
            newInfo.Childs = new List<BookmarkInfo>();
            newInfo.IsFolder = isFolder;

            BookmarkInfo nowInfo = GetBookmarkInfo(node);
            nowInfo.Childs.Add(newInfo);

            TreeNode newNode = new TreeNode(newInfo.Name, isFolder ? 0 : 1, isFolder ? 0 : 1);
            newNode.Tag = newInfo;
            this.selected_node.Nodes.Add(newNode);
            this.selected_node.Expand();

            this.treeViewBookMark.SelectedNode = this.selected_node = newNode;
        }
        #region treeview
        /// <summary>
        /// Renames node's label and save it in the temporary XML file
        /// </summary>
        private void treeViewBookMark_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            //try
            //{
            //    selected_node = e.Node;
            //    if (selected_node.Name != rootNodeName)
            //    {
            //        if (e.Label == null || e.Label.Trim() == "")
            //        {
            //            e.CancelEdit = true;
            //            return;
            //        }

            //        var SelectedNode = (from node in X_Element.Descendants(e.Node.Name)
            //                            select node).First();

            //        SelectedNode.SetAttributeValue("Name", e.Label.Trim());
            //        X_Element.Save(m_bookmarkFile);

            //        treeViewBookMark.SelectedNode = selected_node;
            //        textBoxName.Text = e.Label.Trim();
            //    }
            //}
            //catch (Exception)
            //{
            //    //StackFrame file_info = new StackFrame(true);
            //    //error(ref file_info, ex.Message);
            //    e.CancelEdit = true;
            //}
        }

        private void treeViewBookMark_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BookmarkInfo info = e.Node.Tag as BookmarkInfo;
            if (info == null || info == m_rootBookmarkInfo)
            {
                textBoxName.Enabled = false;
                textBoxName.Text = null;
                textBoxDesc.Enabled = false;
                textBoxDesc.Text = null;
                textBoxHref.Enabled = false;
                textBoxHref.Text = null;

                return;
            }

            textBoxName.Enabled = true;
            textBoxDesc.Enabled = true;
            textBoxName.Text = info.Name;
            textBoxDesc.Text = info.Description;
            if (info.IsFolder)
            {
                textBoxHref.Enabled = false;
                textBoxHref.Text = null;
            }
            else
            {
                textBoxHref.Enabled = true;
                textBoxHref.Text = info.Address;
            }

            this.selected_node = e.Node;
        }

        // Node being dragged
        private TreeNode dragNode = null;
        // Temporary drop node for selection
        private TreeNode tempDropNode = null;
        // Timer for scrolling
        private Timer timer = new Timer();
        private void treeViewBookMark_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //selected_node = (TreeNode)e.Item;
            //this.treeViewBookMark.DoDragDrop(selected_node, DragDropEffects.Move);

            // Get drag node and select it
            this.dragNode = (TreeNode)e.Item;
            this.treeViewBookMark.SelectedNode = this.dragNode;

            // Reset image list used for drag image
            this.imageListDrag.Images.Clear();
            this.imageListDrag.ImageSize = new Size(this.dragNode.Bounds.Size.Width + this.treeViewBookMark.Indent, this.dragNode.Bounds.Height);

            // Create new bitmap
            // This bitmap will contain the tree node image to be dragged
            Bitmap bmp = new Bitmap(this.dragNode.Bounds.Width + this.treeViewBookMark.Indent, this.dragNode.Bounds.Height);

            // Get graphics from bitmap
            Graphics gfx = Graphics.FromImage(bmp);

            // Draw node icon into the bitmap
            gfx.DrawImage(this.imageListTreeView.Images[0], 0, 0);

            // Draw node label into bitmap
            gfx.DrawString(this.dragNode.Text,
                this.treeViewBookMark.Font,
                new SolidBrush(this.treeViewBookMark.ForeColor),
                (float)this.treeViewBookMark.Indent, 1.0f);

            // Add bitmap to imagelist
            this.imageListDrag.Images.Add(bmp);

            // Get mouse position in client coordinates
            Point p = this.treeViewBookMark.PointToClient(Control.MousePosition);

            // Compute delta between mouse position and node bounds
            int dx = p.X + this.treeViewBookMark.Indent - this.dragNode.Bounds.Left;
            int dy = p.Y - this.dragNode.Bounds.Top;

            // Begin dragging image
            if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, dx, dy))
            {
                // Begin dragging
                this.treeViewBookMark.DoDragDrop(bmp, DragDropEffects.Move);
                // End dragging image
                DragHelper.ImageList_EndDrag();
            }	
        }

        private void treeViewBookMark_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Compute drag position and move image
            Point formP = this.PointToClient(new Point(e.X, e.Y));
            DragHelper.ImageList_DragMove(formP.X - this.treeViewBookMark.Left, formP.Y - this.treeViewBookMark.Top);

            // Get actual drop node
            TreeNode dropNode = this.treeViewBookMark.GetNodeAt(this.treeViewBookMark.PointToClient(new Point(e.X, e.Y)));
            if (dropNode == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;

            // if mouse is on a new node select it
            if (this.tempDropNode != dropNode)
            {
                DragHelper.ImageList_DragShowNolock(false);
                this.treeViewBookMark.SelectedNode = dropNode;
                DragHelper.ImageList_DragShowNolock(true);
                tempDropNode = dropNode;
            }

            // Avoid that drop node is child of drag node 
            TreeNode tmpNode = dropNode;
            while (tmpNode.Parent != null)
            {
                if (tmpNode.Parent == this.dragNode) e.Effect = DragDropEffects.None;
                tmpNode = tmpNode.Parent;
            }
        }

        /// <summary>
        /// Manages DragDrop operations
        /// </summary>   
        private void treeViewBookMark_DragDrop(object sender, DragEventArgs e)
        {
            // Unlock updates
            DragHelper.ImageList_DragLeave(this.treeViewBookMark.Handle);

            // Get drop node
            TreeNode dropNode = this.treeViewBookMark.GetNodeAt(this.treeViewBookMark.PointToClient(new Point(e.X, e.Y)));

            // If drop node isn't equal to drag node, add drag node as child of drop node
            if (this.dragNode != dropNode)
            {
                BookmarkInfo dropInfo = dropNode.Tag as BookmarkInfo;
                BookmarkInfo dragInfo = dragNode.Tag as BookmarkInfo;

                // Remove drag node from parent
                if (this.dragNode.Parent == null)
                {
                    m_rootBookmarkInfo.Childs.Remove(dragInfo);
                    this.treeViewBookMark.Nodes.Remove(this.dragNode);
                    
                }
                else
                {
                    (this.dragNode.Parent.Tag as BookmarkInfo).Childs.Remove(dragInfo);
                    this.dragNode.Parent.Nodes.Remove(this.dragNode);
                }

                if (dropInfo.IsFolder)
                {
                    // Add drag node to drop node
                    dropInfo.Childs.Add(dragInfo);
                    dropNode.Nodes.Add(this.dragNode);
                    
                }
                else
                {
                    // Add drag node to drop node
                    (dropNode.Parent.Tag as BookmarkInfo).Childs.Insert(dropNode.Index, dragInfo);
                    dropNode.Parent.Nodes.Insert(dropNode.Index, this.dragNode);
                }
                dropNode.ExpandAll();
                // Set drag node to null
                this.dragNode = null;

                // Disable scroll timer
                this.timer.Enabled = false;
            }
        }

        private void treeViewBookMark_DragEnter(object sender, DragEventArgs e)
        {
            DragHelper.ImageList_DragEnter(this.treeViewBookMark.Handle, e.X - this.treeViewBookMark.Left,
                e.Y - this.treeViewBookMark.Top);

            // Enable timer for scrolling dragged item
            this.timer.Enabled = true;
        }

        private void treeViewBookMark_DragLeave(object sender, EventArgs e)
        {
            DragHelper.ImageList_DragLeave(this.treeViewBookMark.Handle);

            // Disable timer for scrolling dragged item
            this.timer.Enabled = false;
        }
        private void treeViewBookMark_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                // Show pointer cursor while dragging
                e.UseDefaultCursors = false;
                this.treeViewBookMark.Cursor = Cursors.Default;
            }
            else e.UseDefaultCursors = true;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // get node at mouse position
            Point pt = PointToClient(Control.MousePosition);
            TreeNode node = this.treeViewBookMark.GetNodeAt(pt);

            if (node == null) return;

            // if mouse is near to the top, scroll up
            if (pt.Y < 30)
            {
                // set actual node to the upper one
                if (node.PrevVisibleNode != null)
                {
                    node = node.PrevVisibleNode;

                    // hide drag image
                    DragHelper.ImageList_DragShowNolock(false);
                    // scroll and refresh
                    node.EnsureVisible();
                    this.treeViewBookMark.Refresh();
                    // show drag image
                    DragHelper.ImageList_DragShowNolock(true);

                }
            }
            // if mouse is near to the bottom, scroll down
            else if (pt.Y > this.treeViewBookMark.Size.Height - 30)
            {
                if (node.NextVisibleNode != null)
                {
                    node = node.NextVisibleNode;

                    DragHelper.ImageList_DragShowNolock(false);
                    node.EnsureVisible();
                    this.treeViewBookMark.Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                }
            }
        }

        private void treeViewBookMark_KeyDown(object sender, KeyEventArgs e)
        {
            selected_node = treeViewBookMark.SelectedNode;

            if (e.KeyCode == Keys.Delete)
            {
                deleteItem_Click(deleteItem, System.EventArgs.Empty);
            }
        }

        private void treeViewBookMark_MouseClick(object sender, MouseEventArgs e)
        {
            selected_node = treeViewBookMark.GetNodeAt(new Point(e.X, e.Y));
            if (e.Button == MouseButtons.Right)
            {
                treeViewBookMark.SelectedNode = selected_node;
                contextMenuStripTreeView.Show(this.treeViewBookMark, new Point(e.X, e.Y));
            }
        }


        #endregion
    }

    public class DragHelper
    {
        [DllImport("comctl32.dll")]
        public static extern bool InitCommonControls();

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_BeginDrag(IntPtr himlTrack, int
            iTrack, int dxHotspot, int dyHotspot);

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragMove(int x, int y);

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern void ImageList_EndDrag();

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragLeave(IntPtr hwndLock);

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragShowNolock(bool fShow);

        static DragHelper()
        {
            InitCommonControls();
        }
    }
}
