using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Feng.Windows.Forms;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [DesignerAttribute(typeof(DataControlFormCreatorDesigner), typeof(IDesigner))]
    public class DataControlFormCreator : Component
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataControlFormCreator()
        {
        }
    }

    public class DataControlFormCreatorDesigner : ComponentDesigner
    {
        private Form m_parentForm;
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            m_parentForm = (component.Site.GetService(typeof(IDesignerHost)) as IDesignerHost).RootComponent as Form;
        }


        /// <summary>
        /// Verbs
        /// </summary>
        public override System.ComponentModel.Design.DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection v = new DesignerVerbCollection();

                // Verb to add RadioButton
                v.Add(new DesignerVerb("&Load GridInfos", new EventHandler(OnLoadGridInfos)));

                return v;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadGridInfos(object sender, System.EventArgs e)
        {
            XceedUtility.SetXceedLicense();

            Form form = new Form();
            form.AutoSize = true;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.Controls.Add(new FlowLayoutPanel());
            form.Controls[0].Controls.Add(new TextBox());
            form.Text = "Please input gridName";
            form.Controls[0].Controls.Add(new Button());
            (form.Controls[0].Controls[1] as Button).Text = "Ok";
            (form.Controls[0].Controls[1] as Button).DialogResult = DialogResult.OK;
            if (form.ShowDialog() == DialogResult.OK)
            {
                string gridName = (form.Controls[0].Controls[0] as TextBox).Text;
                if (string.IsNullOrEmpty(gridName))
                {
                    MessageForm.ShowError("GridName should not be null!");
                    return;
                }

                IList<GridColumnInfo> gridColumnInfos = ADInfoBll.Instance.GetGridColumnInfos(gridName);
                int x = 0, y = 0;
                foreach (GridColumnInfo columnInfo in gridColumnInfos)
                {
                    try
                    {
                        IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
                        DesignerTransaction dt = h.CreateTransaction("Add IDataControl");
                        IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

                        c.OnComponentChanging(m_parentForm, null);
                        Control control = AddDataControl(h, columnInfo);
                        control.Location = new System.Drawing.Point(x, y);
                        c.OnComponentChanged(m_parentForm, null, null, null);

                        dt.Commit();

                        x += 200;
                        if (x >= 800)
                        {
                            x = 0;
                            y += 25;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageForm.ShowError(ex.Message);
                    }
                }
            }
            form.Dispose();
        }

        private LabeledDataControl AddDataControl(IDesignerHost h, GridColumnInfo info)
        {
            //IDataControl dc = ControlFactory.GetDataControl(columnInfo);
            //Control control = dc as Control;
            //if (control == null)
            //{
            //    throw new ArgumentException("IDataControl should be System.Windows.Forms.Control!");
            //}
            //m_parentForm.Controls.Add(control);

            Control ic = null;
            foreach (string s in ControlsName.DataControlsTypeName)
            {
                if (s.LastIndexOf(info.DataControlType, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    ic = (Control)h.CreateComponent(Feng.Utils.ReflectionHelper.GetTypeFromName(s));
                    break;
                }
            }

            if (ic == null)
            {
                throw new InvalidOperationException("Invalid ControlType of " + info.DataControlType);
            }
            LabeledDataControl control = (LabeledDataControl)h.CreateComponent(typeof(LabeledDataControl));

            MyLabel lbl = (MyLabel)h.CreateComponent(typeof(MyLabel));
            lbl.Text = info.Caption;

            control.Controls.Add(lbl);
            control.Controls.Add(ic);
            control.Label = lbl;
            control.Control = ic;

            IDataControl dc = control as IDataControl;
            dc.Caption = info.Caption;
            dc.PropertyName = info.PropertyName;
            dc.Navigator = info.Navigator;
            //dc.Index = info.SeqNo;
            dc.Name = info.GridColumnName;

            control.ResetLayout();

            m_parentForm.Controls.Add(control);

            return control;
        }
    }
}
