using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 以Id显示的实体类TextBox
    /// </summary>
    public class MyObjectTextBox : Xceed.Editors.WinTextBox, IDataValueControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MyObjectTextBox()
            : base()
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyObjectTextBox(MyObjectTextBox template)
            : base(template)
        {
            this.ObjectType = template.ObjectType;
            this.DisplayMember = template.DisplayMember;

            Initialize();
        }

        private void Initialize()
        {
            XceedUtility.SetUIStyle(this);

            this.Validated += new EventHandler(MyObjectTextBox_Validated);
        }

        void MyObjectTextBox_Validated(object sender, EventArgs e)
        {
            var dataValue = SelectedDataValue;
        }

        /// <summary>
        /// 显示时候用于显示的属性名
        /// </summary>
        public string DisplayMember
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedDataValue
        /// </summary>
        public virtual object SelectedDataValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.TextBoxArea.Text))
                {
                    m_selectedObject = null;
                }
                else
                {
                    object id = this.TextBoxArea.Text;
                    
                    try
                    {
                        IEntityBuffer eb = EntityBufferCollection.Instance[this.ObjectType];
                        if (eb != null)
                        {
                            m_selectedObject = eb.Get(id);
                        }
                        else
                        {
                            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository(this.ObjectType))
                            {
                                rep.BeginTransaction();
                                m_selectedObject = rep.Get(this.ObjectType, id);
                                rep.CommitTransaction();
                            }
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionProcess.ProcessWithNotify(new ArgumentException("不存在此信息，请查证后填写！", ex));
                        this.SelectedDataValue = m_selectedObject;
                    }
                }
                return m_selectedObject;
            }
            set
            {
                if (value != null)
                {
                    m_selectedObject = value;

                    this.TextBoxArea.Text = EntityHelper.ReplaceEntity(DisplayMember, m_selectedObject, null);
                }
                else
                {
                    m_selectedObject = null;
                    this.Text = string.Empty;
                }
            }
        }

        private object m_selectedObject;

        /// <summary>
        /// 实体类类型
        /// </summary>
        public Type ObjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        private bool m_bReadOnly;

        /// <summary>
        /// 获取或设置只读属性
        /// </summary>
        public bool ReadOnly
        {
            get { return m_bReadOnly; }
            set
            {
                if (m_bReadOnly != value)
                {
                    m_bReadOnly = value;

                    this.TextBoxArea.ReadOnly = value;

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
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MyObjectTextBox(this);
        }
    }
}
