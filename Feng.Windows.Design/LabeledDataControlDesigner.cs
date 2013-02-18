namespace Feng.Windows.Forms.Design
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.ComponentModel;
	using System.ComponentModel.Design;
	using System.Collections;
	using System.Windows.Forms.Design;
	using System.Drawing;
    using System.Windows.Forms;
	using Feng.Windows.Forms;

	/// <summary>
    /// LabeledDataControl Designer
	/// </summary>
    public class LabeledDataControlDesigner : ParentControlDesigner
	{
		private LabeledDataControl m_owner;

		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);

			// Record instance of control we're designing
			LabeledDataControl container = component as LabeledDataControl;

            if (container == null)
            {
                throw new ArgumentException("component is not LabeledDataControl");
            }

            if (container != null)
            {
                m_owner = container;
                // Hook up events
                IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                c.ComponentRemoving += new ComponentEventHandler(OnComponentRemoving);
            }
		}

		/// <summary>
		/// InitializeNewComponent
		/// </summary>
		/// <param name="defaultValues"></param>
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);

			// we can create a default textbox 
            AddDataControl(typeof(MyTextBox));
            OnAddLabel(null, System.EventArgs.Empty);
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			System.Windows.Forms.Control control;

			IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
			IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));

			// If the user is removing the control itself
			if (e.Component == m_owner)
			{
				c.OnComponentChanging(m_owner, null);
                control = m_owner.Control;
				if (control != null)
				{
					h.DestroyComponent(control);
					m_owner.Control = null;
				}
                Label label = m_owner.Label;
                if (label != null)
                {
                    h.DestroyComponent(label);
                    m_owner.Label = null;
                }
				c.OnComponentChanged(m_owner, null, null, null);
			}
            // If the user is removing a button
            else if (e.Component is System.Windows.Forms.Control)
            {
                control = (System.Windows.Forms.Control)e.Component;
                if (m_owner != null && m_owner.Control == control)
                {
                    c.OnComponentChanging(m_owner, null);
                    m_owner.Control = null;
                    c.OnComponentChanged(m_owner, null, null, null);
                    return;
                }
            }
            else if (e.Component is MyLabel)
            {
				MyLabel label = (MyLabel)e.Component;
				if (m_owner != null && m_owner.Label == label)
				{
					c.OnComponentChanging(m_owner, null);
					m_owner.Label = null;
					c.OnComponentChanged(m_owner, null, null, null);
					return;
				}
            }
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

			// Unhook events
			c.ComponentRemoving -= new ComponentEventHandler(OnComponentRemoving);

			base.Dispose(disposing);
		}

		/// <summary>
		/// AssociatedComponents
		/// </summary>
		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				ArrayList arr = new ArrayList();
                if (m_owner != null && m_owner.Control != null)
				{
                    arr.Add(m_owner.Control);
				}
                if (m_owner != null && m_owner.Label != null)
                {
                    arr.Add(m_owner.Label);
                }
				return arr;
			}
		}

		/// <summary>
		/// Verbs
		/// </summary>
		public override System.ComponentModel.Design.DesignerVerbCollection Verbs
		{
			get
			{
				DesignerVerbCollection v = new DesignerVerbCollection();
                if (m_owner == null)
                {
                    return v;
                }

				string verb = "&Change To";

                Guid menuGroup = Guid.NewGuid();
				for (int i = 0; i < ControlsName.DataControlsTypeName.Length; ++i)
                {
					v.Add(new DesignerVerb(verb + " " + ControlsName.GetNameFromFullName(ControlsName.DataControlsTypeName[i]), 
						new EventHandler(OnAddDataControl), 
						new CommandID(menuGroup, i)));
                }
               
				return v;
			}
		}

		private void OnAddDataControl(object sender, System.EventArgs e)
        {
            System.ComponentModel.Design.DesignerVerb verb = sender as System.ComponentModel.Design.DesignerVerb;
			int idx = verb.CommandID.ID;
            AddDataControl(Type.GetType(ControlsName.DataControlsTypeName[idx], true));
        }

		private void AddDataControl(System.Type type)
		{
            if (m_owner == null)
                return;

			System.Windows.Forms.Control control = m_owner.Control;
			if (control != null)
			{
				control.Dispose();
			}

			IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
			DesignerTransaction dt = h.CreateTransaction("Add DataControl");
			IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
			control = (System.Windows.Forms.Control)h.CreateComponent(type);
			c.OnComponentChanging(m_owner, null);
			m_owner.Control = control;
			c.OnComponentChanged(m_owner, null, null, null);
			dt.Commit();

			m_owner.ResetLayout();
		}

        private void OnAddLabel(object sender, System.EventArgs e)
        {
			if (m_owner == null)
				return;

            Label label = m_owner.Label;
            if (label == null)
            {
                IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
				DesignerTransaction dt = h.CreateTransaction("Add MyLabel");
                IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                label = (MyLabel)h.CreateComponent(typeof(MyLabel));
				label.Text = "БъЬт";
                c.OnComponentChanging(m_owner, null);
                m_owner.Label = label;
                c.OnComponentChanged(m_owner, null, null, null);
                dt.Commit();

                m_owner.ResetLayout();
            }
        }

        

		/// <summary>
		/// GetHitTest
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		protected override bool GetHitTest(System.Drawing.Point point)
		{
            if (m_owner == null || m_owner.Control == null)
				return false;

			point = m_owner.PointToClient(point);
			Rectangle wrct = m_owner.Control.Bounds;
			if (wrct.Contains(point))
				return true;
			else
				return false;
		}
	}
}
