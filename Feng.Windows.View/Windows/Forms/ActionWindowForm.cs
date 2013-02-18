using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Feng.Windows.Forms
{
    /// <summary>
    /// ´øActionButtonsµÄForm
    /// </summary>
    [Obsolete("Not use it now")]
    public partial class ActionWindowForm : MyChildForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActionWindowForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        protected ActionButtons ActionButtons
        {
            get { return actionButtons1; }
        }

        /// <summary>
        /// put actionButtons in mid of form
        /// </summary>
        /// <param name="levent"></param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

			if (actionButtons1 != null)
			{
				actionButtons1.Location = new Point((this.Size.Width - actionButtons1.Width) / 2, this.Height - 40);
			}
        }

		private IControlManager m_cm;
		/// <summary>
		/// ControlManager as IControlManager
		/// </summary>
		protected IControlManager ControlManager
		{
			get { return m_cm; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        protected void SetControlManager(IControlManager cm)
        {
            if (cm == null)
            {
                throw new ArgumentNullException("cm");
            }
            m_cm = cm;
        }

        /// <summary>
        /// Form_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected override void Form_Load(object sender, System.EventArgs e)
        {
			if (base.DesignMode)
				return;

            actionButtons1.AddClick += new System.EventHandler(btnAdd_Click);
            actionButtons1.EditClick += new System.EventHandler(btnEdit_Click);
            actionButtons1.DeleteClick += new System.EventHandler(btnDelete_Click);
            actionButtons1.CancelClick += new System.EventHandler(btnCancel_Click);
            actionButtons1.OkClick += new System.EventHandler(btnOK_Click);

			m_cm.StateControls.Add(actionButtons1);
        }

        /// <summary>
        /// btnAdd_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnAdd_Click(System.Object sender, System.EventArgs e)
        {
            m_cm.AddNew();
        }

        /// <summary>
        /// btnEdit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnEdit_Click(System.Object sender, System.EventArgs e)
        {
            m_cm.EditCurrent();
        }

        /// <summary>
        /// btnDelete_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnDelete_Click(System.Object sender, System.EventArgs e)
        {
            m_cm.DeleteCurrent();
        }

        /// <summary>
        /// btnCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnCancel_Click(System.Object sender, System.EventArgs e)
        {
            m_cm.CancelEdit();
        }

        /// <summary>
        /// btnOK_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            if (m_cm.SaveCurrent())
            {
                m_cm.EndEdit(true);
            }
        }
    }
}

