using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 各个Button显示状态（包含Enable状态和Visible状态），每个Button可单独设置或者用"|"和"＆~"运算符组合设置。
    /// <p>
    /// <example>
    /// 只设置"Add和Edit"
    /// <code>state = EnabledStateType.Add | EnabledStateType.Edit</code>
    /// 只不设置 "Cancel"
    /// <code>state = EnabledStateType.All ＆ ~EnabledStateType.Calcel</code>
    /// </example>
    /// 另外，定义了4个特殊状态
    /// <list type="bullet">
    /// <item>None: 全不设置</item>
    /// <item>All: 全设置</item>
    /// <item>Normal: "Add, Edit, Delete" 设置, "OK, Cancel" 不设置</item>
    /// <item>Acting: "Add, Edit, Delete" 不设置, "OK, Cancel" 设置</item>
    /// </list>
    /// </p>
    /// </summary>
    [Flags]
    public enum ShowStateType
    {
        /// <summary>
        /// 全不设置
        /// </summary>
        None = 0,
        /// <summary>
        /// 只设置Add
        /// </summary>
        Add = 1,
        /// <summary>
        /// 只设置Edit
        /// </summary>
        Edit = 2,
        /// <summary>
        /// 只设置Delete
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 只设置OK
        /// </summary>
        Ok = 8,
        /// <summary>
        /// 只设置Cancel
        /// </summary>
        Cancel = 16,
        /// <summary>
        /// 全设置
        /// </summary>
        All = Add | Edit | Delete | Ok | Cancel,
        /// <summary>
        /// "Add, Edit, Delete" 设置, "OK, Cancel" 不设置
        /// </summary>
        Normal = Add | Edit | Delete,
        /// <summary>
        /// "Add, Edit, Delete" 不设置, "OK, Cancel" 设置
        /// </summary>
        Acting = Ok | Cancel,
    };

	/// <summary>
	/// 一组Button，包含增加(Add)、修改(Edit)、删除(Delete)、确定(OK)和取消(Cancel)。
	/// </summary>
    [ToolboxItem(false)]
    [Obsolete()]
    [DefaultProperty("State")]
	public class ActionButtons : System.Windows.Forms.UserControl, IStateControl, IReadOnlyControl
	{
		#region "ActionButtons States"

		// 把button放入数组，方便操作
		private ShowStateType[] m_stateTypes = 
			{ShowStateType.Add, ShowStateType.Edit, ShowStateType.Delete, ShowStateType.Cancel, ShowStateType.Ok};
        private FlowLayoutPanel flowLayoutPanel1;
        private MyButton btnOK;
        private MyButton btnCancel;
        private MyButton btnDelete;
        private MyButton btnEdit;
        private MyButton btnAdd;
		private MyButton[] m_buttons;

		/// <summary>
		/// 各个Button Enable状态
		/// </summary>
		public void SetEnabledState(ShowStateType state)
		{
			for (int i=0; i<m_buttons.Length; ++i)
			{
				if ((state & m_stateTypes[i]) > 0)
					m_buttons[i].Enabled = true;
				else
					m_buttons[i].Enabled = false;
			}
		}

        /// <summary>
        /// 设置Button Enable状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="enable"></param>
        public void SetEnabledState(int index, bool enable)
        {
            if (index < 0 || index >= 5)
            {
				throw new ArgumentException("index is beyond the Range", "index");
            }
            m_buttons[index].Enabled = enable;
        }

		/// <summary>
		/// 各个Button Visible状态
		/// </summary>
		public void SetVisibleState(ShowStateType state)
		{
			for (int i=0; i<m_buttons.Length; ++i)
			{
				if ((state & m_stateTypes[i]) > 0)
					m_buttons[i].Visible = true;
				else
					m_buttons[i].Visible = false;
			}

            OnResize(System.EventArgs.Empty);
		}

		/// <summary>
		/// See<see cref="IReadOnlyControl.ReadOnly"/>
		/// </summary>
		public bool ReadOnly
		{
			get { return false; }
            set
            {
                if (!value)
                {
                    if (ReadOnlyChanged != null)
                    {
                        ReadOnlyChanged(this, System.EventArgs.Empty);
                    }
                }
            }
		}

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ReadOnlyChanged;

		/// <summary>
		/// ActionButtons操作状态
		/// </summary>
		public void SetState(StateType state)
		{
			switch (state)
            {
                case StateType.None:
                    SetEnabledState(ShowStateType.Add);
                    break;
                case StateType.View:
					SetEnabledState(ShowStateType.Normal);
                    break;
                case StateType.Add:
					SetEnabledState(ShowStateType.Acting);
                    break;
                case StateType.Edit:
					SetEnabledState(ShowStateType.Acting);
                    break;
                default:
                    throw new NotSupportedException("Invalid State");
            }
		}

		#endregion

		#region Component Designer generated code

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Constructor
		/// </summary>
		public ActionButtons()
		{
			InitializeComponent();

			m_buttons = new MyButton[] { btnAdd, btnEdit, btnDelete, btnCancel, btnOK};

        }


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
        }
		private void InitializeComponent()
		{
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new Feng.Windows.Forms.MyButton();
            this.btnEdit = new Feng.Windows.Forms.MyButton();
            this.btnDelete = new Feng.Windows.Forms.MyButton();
            this.btnCancel = new Feng.Windows.Forms.MyButton();
            this.btnOK = new Feng.Windows.Forms.MyButton();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnAdd);
            this.flowLayoutPanel1.Controls.Add(this.btnEdit);
            this.flowLayoutPanel1.Controls.Add(this.btnDelete);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnOK);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(373, 25);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 21);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "增加";
            this.btnAdd.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(75, 0);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(72, 21);
            this.btnEdit.TabIndex = 8;
            this.btnEdit.Text = "修改";
            this.btnEdit.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(150, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 21);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(225, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 21);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(300, 0);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 21);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.Button_Click);
            // 
            // ActionButtons
            // 
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ActionButtons";
            this.Size = new System.Drawing.Size(377, 24);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region "Events"

		private bool m_showWarning = true;

		/// <summary>
		/// 是否在删除和取消时显示提示信息
		/// </summary>
		[Description("是否在删除和取消时显示提示信息")]
		[DefaultValue(true)]
		public bool ShowWarning
		{
			get { return m_showWarning; }
			set { m_showWarning = value; }
		}
	
		/// <summary>
		/// Event for Add Button's Click
		/// </summary>
		public event EventHandler AddClick;
		/// <summary>
		/// Event for Edit Button's Click
		/// </summary>
		public event EventHandler EditClick;
		/// <summary>
		/// Event for Delete Button's Click
		/// </summary>
		public event EventHandler DeleteClick;
		/// <summary>
		/// Event for OK Button's Click
		/// </summary>
		public event EventHandler OkClick;
		/// <summary>
		/// Event for Cancel Button's Click
		/// </summary>
		public event EventHandler CancelClick;

		// 把Button Click转换为相应Event
		private void Button_Click (object sender, System.EventArgs e)
		{
			if (sender == btnAdd && AddClick != null)
				AddClick(sender, e);
			else if (sender == btnEdit && EditClick != null)
				EditClick(sender, e);
			else if (sender == btnDelete && DeleteClick != null)
			{
				// 删除记录操作，需确认
                if (!m_showWarning || MessageForm.ShowYesNo("本操作将" + btnDelete.Text + "记录", btnDelete.Text + "记录"))
				{
					DeleteClick(sender, e);
				}
			}
			else if (sender == btnOK && OkClick != null)
			{
				OkClick(sender, e);
			}
			else if (sender == btnCancel && CancelClick != null)
			{
                if (!m_showWarning || MessageForm.ShowYesNo("操作终止，填写的数据不被保存，请确认", "取消"))
				{
					CancelClick(sender, e);
				}
			}
		}
		#endregion

		#region "Button's Text and Visiblity"
		///// <summary>
        ///// 当ActionButtons大小改变时，改变各个Buttons的位置，等距排列
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnResize(System.EventArgs e)
        //{
        //    int x = this.Size.Width / 5;
        //    int n = 0;
        //    btnAdd.Location = new Point(x * n, btnAdd.Location.Y);
        //    if (btnAdd.Visible) n++;

        //    btnEdit.Location = new Point(x * n, btnEdit.Location.Y);
        //    if (btnEdit.Visible) n++;

        //    btnDelete.Location = new Point(x * n, btnDelete.Location.Y);
        //    if (btnDelete.Visible) n++;

        //    btnCancel.Location = new Point(x * n, btnCancel.Location.Y);
        //    if (btnCancel.Visible) n++;

        //    btnOK.Location = new Point(x * n, btnOK.Location.Y);
        //    if (btnOK.Visible) n++;

        //    base.OnResize(e);
        //}

        /// <summary>
        /// 设置Button's Text
        /// </summary>
        /// <param name="buttonsText"></param>
        public void SetButtonsText(string[] buttonsText)
        {
            for (int i = 0; i < buttonsText.Length; ++i)
            {
                if (!string.IsNullOrEmpty(buttonsText[i]))
                {
                    m_buttons[i].Text = buttonsText[i];
                }
            }
        }

        /// <summary>
        /// 设置Button's Text
        /// </summary>
        /// <param name="index"></param>
        /// <param name="buttonText"></param>
        public void SetButtonText(int index, string buttonText)
        {
            if (index < 0 || index >= 5)
            {
                throw new ArgumentException("index is beyond the Range", "index");
            }
            m_buttons[index].Text = buttonText;
        }

        /// <summary>
        /// 重置Button's Text
        /// </summary>
        public void ResetButtonTexts()
        {
            string[] s = new string[] {"添加", "修改", "删除", "取消", "确定"};
            for (int i=0; i<5; ++i)
            {
                m_buttons[i].Text = s[i];
            }
		}
		#endregion
	}
}


