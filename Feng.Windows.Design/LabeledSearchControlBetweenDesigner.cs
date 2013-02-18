using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Windows.Forms.Design;
using System.Drawing;
using Feng.Windows.Forms;

namespace Feng.Windows.Forms.Design
{
	/// <summary>
    /// LabeledSearchControlBetween Designer
	/// </summary>
	public class LabeledSearchControlBetweenDesigner : ParentControlDesigner
	{
		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
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
			//AddLabels(this, System.EventArgs.Empty);
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		/// <summary>
		/// AssociatedComponents
		/// </summary>
		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				ArrayList list = new ArrayList();
				list.AddRange(base.AssociatedComponents);
                LabeledSearchControlBetween component = (LabeledSearchControlBetween)base.Component;
				foreach (object obj2 in component.Controls)
				{
					if (!list.Contains(obj2))
					{
						list.AddRange(component.Controls);
					}
				}
				return list;
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

				DesignerVerbCollection verbs = new DesignerVerbCollection();
                LabeledSearchControlBetween component = (LabeledSearchControlBetween)base.Component;

                Guid menuGroup = Guid.NewGuid();
				for (int i = 0; i < ControlsName.SearchControlsTypeName.Length; ++i)
				{
                    v.Add(new DesignerVerb("ChangeTo" + " " + ControlsName.GetNameFromFullName(ControlsName.SearchControlsTypeName[i]), 
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
            AddDataControl(Type.GetType(ControlsName.SearchControlsTypeName[idx] + ", Feng.Windows.Forms", true));
		}


		private bool m_addingTroughVerb;
		/// <summary>
		/// AddingTroughVerb
		/// </summary>
		protected bool AddingTroughVerb
		{
			get	{ return this.m_addingTroughVerb; }
			set	{ this.m_addingTroughVerb = value; }
		}

		private void AddDataControl(System.Type type)
		{
            LabeledSearchControlBetween component = (LabeledSearchControlBetween)base.Component;
			this.m_addingTroughVerb = true;
			try
			{
				// Remove
				IDesignerHost service = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
				for (int i = component.Controls.Count - 1; i >= 0; i--)
				{
					service.DestroyComponent(component.Controls[i]);
				}

				DesignerTransaction dt = service.CreateTransaction("Add DataControls in LabeledFindControlBetween");
				IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
				c.OnComponentChanging(component, null);

				// Add Contreols
				System.Windows.Forms.Control control1 = (System.Windows.Forms.Control)service.CreateComponent(type);
				component.Controls.Add(control1);
				System.Windows.Forms.Control control2 = (System.Windows.Forms.Control)service.CreateComponent(type);
				component.Controls.Add(control2);

                System.Windows.Forms.Label label = (System.Windows.Forms.Label)service.CreateComponent(typeof(MyLabel));
				component.Controls.Add(label);
                System.Windows.Forms.Label label1 = (System.Windows.Forms.Label)service.CreateComponent(typeof(MyLabel));
				component.Controls.Add(label1);
                System.Windows.Forms.Label label2 = (System.Windows.Forms.Label)service.CreateComponent(typeof(MyLabel));
				component.Controls.Add(label2);

				label.Text = "БъЬт";
                label1.Text = ">";
                label2.Text = "<";

                LabeledSearchControlBetween.ResetLayout(control1, control2, label, label1, label2);

				component.Control1 = control1;
				component.Control2 = control2;
                component.Label = label as MyLabel;

				c.OnComponentChanged(component, null, null, null);
				dt.Commit();
			}
			finally
			{
				this.m_addingTroughVerb = false;
			}
		}

		//private void AddLabels(object sender, System.EventArgs e)
		//{
		//    LabeledFindControl component = (LabeledFindControl)base.Component;
		//    this.m_addingTroughVerb = true;
		//    try
		//    {
		//        IDesignerHost service = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));

		//        // Add
		//        DesignerTransaction dt = service.CreateTransaction("Add Labels in LabeledFindControl");
		//        IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
		//        c.OnComponentChanging(component, null);
		//        System.Windows.Forms.Control label = (System.Windows.Forms.Control)service.CreateComponent(typeof(MyLabel));
		//        component.Controls.Add(label);
		//        System.Windows.Forms.Control label1 = (System.Windows.Forms.Control)service.CreateComponent(typeof(MyLabel));
		//        component.Controls.Add(label1);
		//        System.Windows.Forms.Control label2 = (System.Windows.Forms.Control)service.CreateComponent(typeof(MyLabel));
		//        component.Controls.Add(label2);

		//        label.Location = new System.Drawing.Point(0, 0);
		//        label.Size = new Size(50, 21);
		//        label1.Location = new System.Drawing.Point(50, 0);
		//        label1.Size = new Size(20, 21);
		//        label1.Text = ">";
		//        label2.Location = new System.Drawing.Point(50, 24);
		//        label2.Size = new Size(20, 21);
		//        label2.Text = "<";

		//        c.OnComponentChanged(component, null, null, null);
		//        dt.Commit();
		//    }
		//    finally
		//    {
		//        this.m_addingTroughVerb = false;
		//    }
		//}
	}
}
