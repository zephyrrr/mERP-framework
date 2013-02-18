namespace Feng.Windows.Forms
{
    partial class ArchiveDataSetReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pageNavigator1 = new Feng.Windows.Forms.PageNavigator();
            this.bindingNavigatorAllCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPageNameItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorPageCountItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorMoveLastItem = new Feng.Windows.Forms.MyToolStripButton();
            this.bindingNavigatorMoveNextItem = new Feng.Windows.Forms.MyToolStripButton();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMovePreviousItem = new Feng.Windows.Forms.MyToolStripButton();
            this.bindingNavigatorMoveFirstItem = new Feng.Windows.Forms.MyToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.myCrystalReportViewer = new Feng.Windows.Forms.MyCrystalReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.pageNavigator1)).BeginInit();
            this.pageNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageNavigator1
            // 
            this.pageNavigator1.AddNewItem = null;
            this.pageNavigator1.AllCountItem = this.bindingNavigatorAllCountItem;
            this.pageNavigator1.BindingSource = null;
            this.pageNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.pageNavigator1.CountItemFormat = "/{0}页";
            this.pageNavigator1.DeleteItem = null;
            this.pageNavigator1.Dock = System.Windows.Forms.DockStyle.None;
            this.pageNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPageNameItem,
            this.bindingNavigatorPageCountItem,
            this.bindingNavigatorAllCountItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorMoveFirstItem});
            this.pageNavigator1.Location = new System.Drawing.Point(506, 0);
            this.pageNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.pageNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.pageNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.pageNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.pageNavigator1.Name = "pageNavigator1";
            this.pageNavigator1.PageCountItem = this.bindingNavigatorPageCountItem;
            this.pageNavigator1.PageNameItem = this.bindingNavigatorPageNameItem;
            this.pageNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.pageNavigator1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.pageNavigator1.Size = new System.Drawing.Size(294, 25);
            this.pageNavigator1.TabIndex = 7;
            this.pageNavigator1.Text = "pageNavigator1";
            // 
            // bindingNavigatorAllCountItem
            // 
            this.bindingNavigatorAllCountItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorAllCountItem.Name = "bindingNavigatorAllCountItem";
            this.bindingNavigatorAllCountItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigatorAllCountItem.Size = new System.Drawing.Size(65, 22);
            this.bindingNavigatorAllCountItem.Text = "共0条/每页";
            this.bindingNavigatorAllCountItem.ToolTipText = "总条数";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(41, 22);
            this.bindingNavigatorCountItem.Text = "/{0}页";
            this.bindingNavigatorCountItem.ToolTipText = "总页数";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPageNameItem
            // 
            this.bindingNavigatorPageNameItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorPageNameItem.Name = "bindingNavigatorPageNameItem";
            this.bindingNavigatorPageNameItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigatorPageNameItem.Size = new System.Drawing.Size(17, 22);
            this.bindingNavigatorPageNameItem.Text = "条";
            this.bindingNavigatorPageNameItem.ToolTipText = "总条数";
            // 
            // bindingNavigatorPageCountItem
            // 
            this.bindingNavigatorPageCountItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorPageCountItem.Name = "bindingNavigatorPageCountItem";
            this.bindingNavigatorPageCountItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigatorPageCountItem.Size = new System.Drawing.Size(40, 25);
            this.bindingNavigatorPageCountItem.Text = "25";
            this.bindingNavigatorPageCountItem.ToolTipText = "每页条数";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            this.bindingNavigatorMoveLastItem.ToolTipText = "移到最后一页";
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            this.bindingNavigatorMoveNextItem.ToolTipText = "移到下一页";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(30, 25);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "当前位置";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            this.bindingNavigatorMovePreviousItem.ToolTipText = "移到上一页";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            this.bindingNavigatorMoveFirstItem.ToolTipText = "移到第一页";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(850, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // myCrystalReportViewer
            // 
            this.myCrystalReportViewer.ActiveViewIndex = -1;
            this.myCrystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.myCrystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myCrystalReportViewer.Location = new System.Drawing.Point(0, 25);
            this.myCrystalReportViewer.Name = "myCrystalReportViewer";
            this.myCrystalReportViewer.SelectionFormula = "";
            this.myCrystalReportViewer.Size = new System.Drawing.Size(850, 588);
            this.myCrystalReportViewer.TabIndex = 8;
            this.myCrystalReportViewer.TemplateDataSet = null;
            this.myCrystalReportViewer.ViewTimeSelectionFormula = "";
            // 
            // ArchiveDataSetReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 613);
            this.Controls.Add(this.myCrystalReportViewer);
            this.Controls.Add(this.pageNavigator1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ArchiveDataSetReportForm";
            this.Text = "ArchiveDataSetReportForm";
            ((System.ComponentModel.ISupportInitialize)(this.pageNavigator1)).EndInit();
            this.pageNavigator1.ResumeLayout(false);
            this.pageNavigator1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PageNavigator pageNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorAllCountItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorPageNameItem;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPageCountItem;
        private Feng.Windows.Forms.MyToolStripButton bindingNavigatorMoveLastItem;
        private Feng.Windows.Forms.MyToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private Feng.Windows.Forms.MyToolStripButton bindingNavigatorMovePreviousItem;
        private Feng.Windows.Forms.MyToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Feng.Windows.Forms.MyCrystalReportViewer myCrystalReportViewer;
    }
}