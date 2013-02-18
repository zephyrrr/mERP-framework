namespace Feng.Windows.Forms
{
    partial class BookmarkManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewBookMark = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelDesc = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxHref = new System.Windows.Forms.TextBox();
            this.labelHref = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxDesc = new System.Windows.Forms.TextBox();
            this.contextMenuStripTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.newFolderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newBookmarkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStripTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.ForeColor = System.Drawing.Color.Black;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.treeViewBookMark);
            this.splitContainer1.Panel1MinSize = 150;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2MinSize = 70;
            this.splitContainer1.Size = new System.Drawing.Size(730, 366);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeViewBookMark
            // 
            this.treeViewBookMark.AllowDrop = true;
            this.treeViewBookMark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewBookMark.HideSelection = false;
            this.treeViewBookMark.ImageIndex = 0;
            this.treeViewBookMark.ImageList = this.imageListTreeView;
            this.treeViewBookMark.ItemHeight = 20;
            this.treeViewBookMark.LabelEdit = true;
            this.treeViewBookMark.Location = new System.Drawing.Point(0, 0);
            this.treeViewBookMark.Name = "treeViewBookMark";
            this.treeViewBookMark.SelectedImageIndex = 0;
            this.treeViewBookMark.Size = new System.Drawing.Size(225, 362);
            this.treeViewBookMark.TabIndex = 0;
            this.treeViewBookMark.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.treeViewBookMark_GiveFeedback);
            this.treeViewBookMark.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewBookMark_MouseClick);
            this.treeViewBookMark.DragLeave += new System.EventHandler(this.treeViewBookMark_DragLeave);
            this.treeViewBookMark.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewBookMark_AfterLabelEdit);
            this.treeViewBookMark.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewBookMark_DragDrop);
            this.treeViewBookMark.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewBookMark_AfterSelect);
            this.treeViewBookMark.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewBookMark_DragEnter);
            this.treeViewBookMark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewBookMark_KeyDown);
            this.treeViewBookMark.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewBookMark_ItemDrag);
            this.treeViewBookMark.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewBookMark_DragOver);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListTreeView.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelDesc, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxHref, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelHref, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxDesc, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(495, 362);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // labelDesc
            // 
            this.labelDesc.AutoSize = true;
            this.labelDesc.Location = new System.Drawing.Point(3, 100);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(41, 12);
            this.labelDesc.TabIndex = 0;
            this.labelDesc.Text = "描述 :";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 12);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "名称 :";
            // 
            // textBoxHref
            // 
            this.textBoxHref.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHref.Location = new System.Drawing.Point(50, 53);
            this.textBoxHref.Name = "textBoxHref";
            this.textBoxHref.Size = new System.Drawing.Size(442, 21);
            this.textBoxHref.TabIndex = 2;
            this.textBoxHref.Validated += new System.EventHandler(this.textBox_Validated);
            this.textBoxHref.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // labelHref
            // 
            this.labelHref.AutoSize = true;
            this.labelHref.Location = new System.Drawing.Point(3, 50);
            this.labelHref.Name = "labelHref";
            this.labelHref.Size = new System.Drawing.Size(41, 12);
            this.labelHref.TabIndex = 0;
            this.labelHref.Text = "地址 :";
            // 
            // textBoxName
            // 
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxName.Location = new System.Drawing.Point(50, 3);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(442, 21);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.Validated += new System.EventHandler(this.textBox_Validated);
            this.textBoxName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // textBoxDesc
            // 
            this.textBoxDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDesc.Location = new System.Drawing.Point(50, 103);
            this.textBoxDesc.Multiline = true;
            this.textBoxDesc.Name = "textBoxDesc";
            this.textBoxDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDesc.Size = new System.Drawing.Size(442, 256);
            this.textBoxDesc.TabIndex = 3;
            this.textBoxDesc.Validated += new System.EventHandler(this.textBox_Validated);
            this.textBoxDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // contextMenuStripTreeView
            // 
            this.contextMenuStripTreeView.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteItem,
            this.toolStripSeparator1,
            this.newFolderItem,
            this.newBookmarkItem});
            this.contextMenuStripTreeView.Name = "contextMenuStrip1";
            this.contextMenuStripTreeView.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStripTreeView.Size = new System.Drawing.Size(125, 76);
            // 
            // deleteItem
            // 
            this.deleteItem.Name = "deleteItem";
            this.deleteItem.Size = new System.Drawing.Size(124, 22);
            this.deleteItem.Text = "删除";
            this.deleteItem.Click += new System.EventHandler(this.deleteItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // newFolderItem
            // 
            this.newFolderItem.Name = "newFolderItem";
            this.newFolderItem.Size = new System.Drawing.Size(124, 22);
            this.newFolderItem.Text = "新文件夹";
            this.newFolderItem.Click += new System.EventHandler(this.newFolderItem_Click);
            // 
            // newBookmarkItem
            // 
            this.newBookmarkItem.Name = "newBookmarkItem";
            this.newBookmarkItem.Size = new System.Drawing.Size(124, 22);
            this.newBookmarkItem.Text = "新书签";
            this.newBookmarkItem.Click += new System.EventHandler(this.newBookmarkItem_Click);
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // BookmarkManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 366);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BookmarkManagerForm";
            this.Text = "书签管理";
            this.Load += new System.EventHandler(this.BookmarkManagerForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BookmarkManagerForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStripTreeView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewBookMark;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxHref;
        private System.Windows.Forms.Label labelHref;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxDesc;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeView;
        private System.Windows.Forms.ToolStripMenuItem deleteItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem newFolderItem;
        private System.Windows.Forms.ToolStripMenuItem newBookmarkItem;
        private System.Windows.Forms.ImageList imageListDrag;
        private System.Windows.Forms.ImageList imageListTreeView;
    }
}