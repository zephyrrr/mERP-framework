namespace Feng.Windows.Forms
{
    using System.ComponentModel;

    /// <summary>
    /// 中文化的ValidationProvider，错误信息为中文
    /// </summary>
    public class MyValidationProvider : Xceed.Validation.ValidationProvider, IExtenderProvider
    {
        #region "Default Property"

        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyValidationProvider()
            : base()
        {
            base.DefaultValidationMessages = new MyValidationMessages();
        }

        /// <summary>
        /// DefaultValidationMessages
        /// </summary>
        public new Xceed.Validation.ValidationMessages DefaultValidationMessages
        {
            get { return base.DefaultValidationMessages; }
            set { base.DefaultValidationMessages = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendee"></param>
        /// <returns></returns>
        bool IExtenderProvider.CanExtend(object extendee)
        {
            //bool ret = ((IExtenderProvider)base).CanExtend(extendee);
            return true;
        }

        #endregion
    }

    /// <summary>
    /// MyValidationMessages(汉化错误信息)
    /// </summary>
    public class MyValidationMessages : Xceed.Validation.ValidationMessages
    {
        #region "Default Property"

        /// <summary>
        /// Constructor(汉化错误信息)
        /// </summary>
        public MyValidationMessages()
            : base()
        {
        }

        private string m_between = "在%min%和%max%之间";

        /// <summary>
        /// Between
        /// </summary>
        [DefaultValue("在%min%和%max%之间")]
        public override string Between
        {
            get { return m_between; }
            set { m_between = value; }
        }

        private string m_dataType = "应是%datatype%类型";

        /// <summary>
        /// DataType
        /// </summary>
        [DefaultValue("应是%datatype%类型")]
        public override string DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }

        private string m_date = "日期";

        /// <summary>
        /// Date
        /// </summary>
        [DefaultValue("日期")]
        public override string Date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        private string m_equal = "等于%value%";

        /// <summary>
        /// Equal
        /// </summary>
        [DefaultValue("等于%value%")]
        public override string Equal
        {
            get { return m_equal; }
            set { m_equal = value; }
        }

        private string m_greaterThan = "大于%value%";

        /// <summary>
        /// GreaterThan
        /// </summary>
        [DefaultValue("大于%value%")]
        public override string GreaterThan
        {
            get { return m_greaterThan; }
            set { m_greaterThan = value; }
        }

        private string m_greaterThanOrEqual = "大于等于%value%";

        /// <summary>
        /// GreaterThanOrEqual
        /// </summary>
        [DefaultValue("大于等于%value%")]
        public override string GreaterThanOrEqual
        {
            get { return m_greaterThanOrEqual; }
            set { m_greaterThanOrEqual = value; }
        }

        private string m_inSet = "在%name%集合中";

        /// <summary>
        /// InSet
        /// </summary>
        [DefaultValue("在%name%集合中")]
        public override string InSet
        {
            get { return m_inSet; }
            set { m_inSet = value; }
        }

        private string m_lessThan = "小于%value%";

        /// <summary>
        /// LessThan
        /// </summary>
        [DefaultValue("小于%value%")]
        public override string LessThan
        {
            get { return m_lessThan; }
            set { m_lessThan = value; }
        }

        private string m_lessThanOrEqual = "小于等于%value%";

        /// <summary>
        /// LessThanOrEqual
        /// </summary>
        [DefaultValue("小于等于%value%")]
        public override string LessThanOrEqual
        {
            get { return m_lessThanOrEqual; }
            set { m_lessThanOrEqual = value; }
        }

        private string m_notEqual = "不等于%value%";

        /// <summary>
        /// NotEqual
        /// </summary>
        [DefaultValue("不等于%value%")]
        public override string NotEqual
        {
            get { return m_notEqual; }
            set { m_notEqual = value; }
        }

        private string m_notInset = "不在%name%集合中";

        /// <summary>
        /// NotInSet
        /// </summary>
        [DefaultValue("不在%name%集合中")]
        public override string NotInSet
        {
            get { return m_notInset; }
            set { m_notInset = value; }
        }

        private string m_number = "数值";

        /// <summary>
        /// Number
        /// </summary>
        [DefaultValue("数值")]
        public override string Number
        {
            get { return m_number; }
            set { m_number = value; }
        }

        private string m_regularExpression = "应遵守%name%格式";

        /// <summary>
        /// RegularExpression
        /// </summary>
        [DefaultValue("应遵守%name%格式")]
        public override string RegularExpression
        {
            get { return m_regularExpression; }
            set { m_regularExpression = value; }
        }

        private string m_requiredField = "必须填写";

        /// <summary>
        /// RequiredField
        /// </summary>
        [DefaultValue("必须填写")]
        public override string RequiredField
        {
            get { return m_requiredField; }
            set { m_requiredField = value; }
        }

        private string m_value = "应%operator%";

        /// <summary>
        /// Value
        /// </summary>
        [DefaultValue("应%operator%")]
        public override string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        #endregion
    } ;
}

//namespace Feng.Windows.Forms
//{
//    using System;
//    using System.Collections;
//    using System.ComponentModel;
//    using System.Drawing;
//    using System.Drawing.Design;
//    using System.Reflection;
//    using System.Runtime.CompilerServices;
//    using System.Runtime.InteropServices;
//    using System.Security.Permissions;
//    using System.Windows.Forms;
//    using Xceed.Utils.Collections;
//    using Xceed.Validation;

//    /// <summary>
//    /// 
//    /// </summary>
//    public class MyValidationProvider : Component, IExtenderProvider, ISupportInitialize
//    {
//        private Container components;
//        private Hashtable m_boundGridEvents = new Hashtable();
//        private Hashtable m_controlExpressions = new Hashtable();
//        private ErrorProvider m_errorProvider = new ErrorProvider();
//        private bool m_initialising;
//        private Hashtable m_properties;
//        private bool m_shouldCancel;
//        private ValidationMessages m_validationMessageSettings = new ValidationMessages();

//        /// <summary>
//        /// 
//        /// </summary>
//        public MyValidationProvider()
//        {
//            this.m_errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
//            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
//            {
//                //Licenser.VerifyLicense();
//            }
//            this.InitializeComponent();
//        }

//        private void AssignGridControlErrorBehavior(object cell)
//        {
//            object obj2 = cell.CreateType().GetProperty("GridControl").GetValue(cell, null);
//            if (obj2 != null)
//            {
//                System.Type type = obj2.CreateType();
//                type.GetProperty("ErrorBlinkRate").SetValue(obj2, this.BlinkRate, null);
//                type.GetProperty("ErrorIcon").SetValue(obj2, this.Icon, null);
//                type.GetProperty("ErrorBlinkStyle").SetValue(obj2, this.BlinkStyle, null);
//            }
//        }

//        private void BindCellEvent(object o, string name)
//        {
//            EventInfo info = o.CreateType().GetEvent(name);
//            System.Type eventHandlerType = info.EventHandlerType;
//            ParameterInfo[] parameters = eventHandlerType.GetMethod("Invoke").GetParameters();
//            GridEvent eventConsumerType = EventHookAssembly.Instance.GetEventConsumerType(parameters[1].ParameterType);
//            EventBinding key = new EventBinding(o, name);
//            this.m_boundGridEvents.Add(key, eventConsumerType);
//            eventConsumerType.EventFired += new EventHandler(this.CellEventFired);
//            Delegate handler = Delegate.CreateDelegate(eventHandlerType, eventConsumerType, "HandleEvent");
//            info.AddEventHandler(o, handler);
//        }

//        private void CellEventFired(object sender, EventArgs eventArgs)
//        {
//            if (eventArgs.CreateType().ToString() == "Xceed.Grid.LeavingEditEventArgs")
//            {
//                this.General_CellLeavingEdit(sender, eventArgs);
//            }
//            else if (eventArgs.CreateType().ToString() == "Xceed.Grid.CellValidationErrorEventArgs")
//            {
//                this.General_CellValidationError(sender, eventArgs);
//            }
//            else if (eventArgs.CreateType().ToString() == "Xceed.Grid.EditLeftEventArgs")
//            {
//                this.General_CellEditLeft(sender, eventArgs);
//            }
//        }

//        public bool ContainsExpression(ValidationExpression expression)
//        {
//            return this.m_controlExpressions.ContainsValue(expression);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                if (this.components != null)
//                {
//                    this.components.Dispose();
//                }
//                object[] array = new object[this.m_controlExpressions.Count];
//                this.m_controlExpressions.CopyTo(array, 0);
//                foreach (DictionaryEntry entry in array)
//                {
//                    object key = entry.Key;
//                    Control ctr = key as Control;
//                    this.SetValidationExpression(key, null);
//                    if (ctr != null)
//                    {
//                        this.RemoveError(ctr);
//                    }
//                }
//            }
//            base.Dispose(disposing);
//        }

//        private void General_CellEditLeft(object sender, EventArgs e)
//        {
//            PropertyInfo property = sender.CreateType().GetProperty("ErrorDescription");
//            if (property != null)
//            {
//                property.SetValue(sender, string.Empty, null);
//            }
//        }

//        private void General_CellLeavingEdit(object sender, EventArgs e)
//        {
//            PropertyInfo errorDescription = null;
//            MethodInfo setValidationExpression = null;
//            MethodInfo getValidationExpression = null;
//            if (IsObjectACell(sender, out errorDescription, out getValidationExpression, out setValidationExpression))
//            {
//                if (getValidationExpression == null)
//                {
//                    throw new InvalidOperationException("Could not perform validation. No validation defined for the bound cell.");
//                }
//                ValidationExpression expression = getValidationExpression.Invoke(sender, null) as ValidationExpression;
//                if (expression == null)
//                {
//                    throw new InvalidOperationException("An attempt was made to evaluate a null expression on a cell leaving edit.");
//                }
//                System.Type type = e.CreateType();
//                PropertyInfo property = type.GetProperty("NewValue");
//                if (!expression.CanEvaluate(sender))
//                {
//                    throw new InvalidOperationException("An attempt was made to evaluate an invalid validation expression on a cell leaving edit.");
//                }
//                string validationMessage = string.Empty;
//                string valueToValidate = Convert.ToString(property.GetValue(e, null));
//                this.m_shouldCancel = !expression.Evaluate(this, sender, valueToValidate, out validationMessage);
//                if (this.m_shouldCancel && ((validationMessage != null) || (validationMessage != string.Empty)))
//                {
//                    this.AssignGridControlErrorBehavior(sender);
//                }
//                errorDescription.SetValue(sender, validationMessage, null);
//                if (this.m_shouldCancel)
//                {
//                    type.GetProperty("Cancel").SetValue(e, this.m_shouldCancel, null);
//                }
//            }
//        }

//        private void General_CellValidationError(object sender, EventArgs e)
//        {
//            if (this.m_shouldCancel)
//            {
//                PropertyInfo property = e.CreateType().GetProperty("CancelEdit");
//                if (property != null)
//                {
//                    property.SetValue(e, false, null);
//                }
//            }
//            this.m_shouldCancel = false;
//        }

//        private void General_Validating(object sender, CancelEventArgs e)
//        {
//            if (!this.m_initialising)
//            {
//                Control componentToValidate = sender as Control;
//                ValidationExpression expression = this.m_controlExpressions[componentToValidate] as ValidationExpression;
//                string validationMessage = string.Empty;
//                if (expression == null)
//                {
//                    throw new InvalidOperationException("An attempt was made to evaluate a null expression.");
//                }
//                if (!expression.CanEvaluate(componentToValidate))
//                {
//                    throw new InvalidOperationException("An attempt was made to evaluate an invalid validation expression.");
//                }
//                string valueToValidate = (string)TypeDescriptor.GetProperties(componentToValidate)["Text"].GetValue(componentToValidate);
//                e.Cancel = !expression.Evaluate(this, componentToValidate, valueToValidate, out validationMessage);
//                if (e.Cancel)
//                {
//                    this.ShowError(componentToValidate, validationMessage);
//                }
//                else
//                {
//                    this.RemoveError(componentToValidate);
//                }
//            }
//        }

//        [DefaultValue(3), Category("Behavior"), Description("Validation expression associated with the input component.")]
//        public ErrorIconAlignment GetIconAlignment(Control inputControl)
//        {
//            return this.m_errorProvider.GetIconAlignment(inputControl);
//        }

//        [Category("Behavior"), DefaultValue(0), Description("Space to leave between the specified control and the icon.")]
//        public int GetIconPadding(Control inputControl)
//        {
//            return this.m_errorProvider.GetIconPadding(inputControl);
//        }

//        protected object GetPropertyValue(object key)
//        {
//            if (this.m_properties == null)
//            {
//                return null;
//            }
//            return this.m_properties[key];
//        }

//        public string GetValidationError(object component)
//        {
//            if (!((IExtenderProvider)this).CanExtend(component))
//            {
//                throw new InvalidOperationException(component.CreateType().ToString() + " is not supported by the validation provider.");
//            }
//            Control control = component as Control;
//            string error = null;
//            if (control != null)
//            {
//                error = this.m_errorProvider.GetError(control);
//            }
//            if ((error == null) || (error != string.Empty))
//            {
//                PropertyInfo property = component.CreateType().GetProperty("ErrorDescription");
//                if (property != null)
//                {
//                    error = property.GetValue(component, null) as string;
//                }
//            }
//            return error;
//        }

//        [Editor("Xceed.Validation.Design.ValidationEditor,Xceed.Validation.Design,Version=1.2.8373.08220,Culture=neutral,PublicKeyToken=ba83ff368b7563c6", typeof(UITypeEditor)), DefaultValue((string)null), Description("Validation expression associated with the input component."), MergableProperty(false), Category("Data")]
//        public ValidationExpression GetValidationExpression(object inputComponent)
//        {
//            return (this.m_controlExpressions[inputComponent] as ValidationExpression);
//        }

//        private void InitializeComponent()
//        {
//        }

//        private bool IsGridControl(object inputComponent, out PropertyInfo currentRowProperty)
//        {
//            currentRowProperty = inputComponent.CreateType().GetProperty("CurrentRow");
//            if (currentRowProperty == null)
//            {
//                return false;
//            }
//            return (currentRowProperty.DeclaringType.ToString() == "Xceed.Grid.GridControl");
//        }

//        internal static bool IsObjectACell(object inputComponent)
//        {
//            System.Type type = inputComponent.CreateType();
//            return (((((type.GetEvent("LeavingEdit") != null) && (type.GetEvent("ValidationError") != null)) && ((type.GetProperty("ErrorDescription") != null) && (type.GetMethod("SetValidationExpression") != null))) && (type.GetMethod("GetValidationExpression") != null)) && (type.ToString() != "Xceed.Grid.ColumnManagerCell"));
//        }

//        internal static bool IsObjectACell(object inputComponent, out PropertyInfo errorDescription, out MethodInfo getValidationExpression, out MethodInfo setValidationExpression)
//        {
//            System.Type type = inputComponent.CreateType();
//            errorDescription = type.GetProperty("ErrorDescription");
//            getValidationExpression = type.GetMethod("GetValidationExpression");
//            setValidationExpression = type.GetMethod("SetValidationExpression");
//            return (((errorDescription != null) && (getValidationExpression != null)) && (setValidationExpression != null));
//        }

//        internal static bool IsObjectAWinTextBox(object inputComponent)
//        {
//            string str2;
//            System.Type baseType = inputComponent.CreateType();
//            if (((str2 = baseType.FullName) == null) || ((!(str2 == "Xceed.Editors.WinTextBox") && !(str2 == "Xceed.Editors.WinComboBox")) && !(str2 == "Xceed.Editors.WinDatePicker")))
//            {
//                while (baseType != typeof(object))
//                {
//                    if (baseType.FullName == "Xceed.Editors.WinTextBoxBase")
//                    {
//                        return true;
//                    }
//                    baseType = baseType.BaseType;
//                }
//                return false;
//            }
//            return true;
//        }

//        protected bool IsPropertyDefined(object key)
//        {
//            if (this.m_properties == null)
//            {
//                return false;
//            }
//            return (this.m_properties.IndexOf(key) != -1);
//        }

//        private bool IsRowBeingEdited(object row, out PropertyInfo cells)
//        {
//            cells = null;
//            System.Type type = row.CreateType();
//            PropertyInfo property = type.GetProperty("IsBeingEdited");
//            if (property == null)
//            {
//                return false;
//            }
//            if (!((bool)property.GetValue(row, null)))
//            {
//                return false;
//            }
//            cells = type.GetProperty("Cells");
//            if (cells == null)
//            {
//                return false;
//            }
//            return true;
//        }

//        private void RemoveError(Control ctr)
//        {
//            this.m_errorProvider.SetError(ctr, string.Empty);
//        }

//        protected void RemovePropertyValue(object key)
//        {
//            if (this.m_properties != null)
//            {
//                this.m_properties.Remove(key);
//                if (this.m_properties.Count == 0)
//                {
//                    this.m_properties = null;
//                }
//            }
//        }

//        private void ResetIcon()
//        {
//            this.Icon = new System.Drawing.Icon(typeof(ErrorProvider), "Error.ico");
//        }

//        [Description("Validation expression associated with the input component."), DefaultValue(3), Category("Behavior")]
//        public void SetIconAlignment(Control inputControl, ErrorIconAlignment value)
//        {
//            this.m_errorProvider.SetIconAlignment(inputControl, value);
//        }

//        [Description("Space to leave between the specified control and the icon."), Category("Behavior"), DefaultValue(0)]
//        public void SetIconPadding(Control inputControl, int value)
//        {
//            this.m_errorProvider.SetIconPadding(inputControl, value);
//        }

//        public void SetValidationError(object component, string error)
//        {
//            if (!((IExtenderProvider)this).CanExtend(component))
//            {
//                throw new InvalidOperationException(component.CreateType().ToString() + " is not supported by the validation provider.");
//            }
//            PropertyInfo property = component.CreateType().GetProperty("ErrorDescription");
//            if (property != null)
//            {
//                if ((error != null) && (error != string.Empty))
//                {
//                    this.AssignGridControlErrorBehavior(component);
//                }
//                property.SetValue(component, error, null);
//            }
//            else
//            {
//                Control control = component as Control;
//                if (control != null)
//                {
//                    this.m_errorProvider.SetError(control, error);
//                }
//            }
//        }

//        [Category("Data"), DefaultValue((string)null), MergableProperty(false), Editor("Xceed.Validation.Design.ValidationEditor,Xceed.Validation.Design,Version=1.2.8373.08220,Culture=neutral,PublicKeyToken=ba83ff368b7563c6", typeof(UITypeEditor)), Description("Validation expression associated with the input component.")]
//        public void SetValidationExpression(object inputComponent, ValidationExpression expression)
//        {
//            if (inputComponent != null)
//            {
//                if (!((IExtenderProvider)this).CanExtend(inputComponent))
//                {
//                    throw new InvalidOperationException(inputComponent.CreateType().ToString() + " is not supported by the validation provider.");
//                }
//                object obj1 = this.m_controlExpressions[inputComponent];
//                if (!this.m_initialising)
//                {
//                    if ((expression != null) && !base.DesignMode)
//                    {
//                        expression = expression.Clone() as ValidationExpression;
//                    }
//                    this.UpdateEventBindings(inputComponent, expression, false);
//                }
//                if (expression == null)
//                {
//                    this.m_controlExpressions.Remove(inputComponent);
//                }
//                else
//                {
//                    this.m_controlExpressions[inputComponent] = expression;
//                }
//            }
//        }

//        private bool ShouldSerializeIcon()
//        {
//            return (SystemIcons.Error != this.Icon);
//        }

//        private void ShowError(Control control, string errorMsg)
//        {
//            this.m_errorProvider.SetError(control, errorMsg);
//        }

//        bool IExtenderProvider.CanExtend(object extendee)
//        {
//            Control inputComponent = extendee as Control;
//            if (inputComponent == null)
//            {
//                return IsObjectACell(extendee);
//            }
//            if ((!(inputComponent is TextBox) && !(inputComponent is ComboBox)) && !IsObjectAWinTextBox(inputComponent))
//            {
//                return false;
//            }
//            return true;
//        }

//        void ISupportInitialize.BeginInit()
//        {
//            this.m_initialising = true;
//        }

//        void ISupportInitialize.EndInit()
//        {
//            this.m_initialising = false;
//            Hashtable hashtable = this.m_controlExpressions.Clone() as Hashtable;
//            foreach (DictionaryEntry entry in hashtable)
//            {
//                ValidationExpression expression = entry.Value as ValidationExpression;
//                object key = entry.Key;
//                if ((expression != null) && !base.DesignMode)
//                {
//                    expression = expression.Clone() as ValidationExpression;
//                    this.m_controlExpressions[key] = expression;
//                }
//                this.UpdateEventBindings(key, expression, true);
//            }
//        }

//        private void UnBindCellEvent(object o, string name)
//        {
//            EventInfo info = o.CreateType().GetEvent(name);
//            System.Type eventHandlerType = info.EventHandlerType;
//            eventHandlerType.GetMethod("Invoke").GetParameters();
//            EventBinding key = new EventBinding(o, name);
//            GridEvent target = this.m_boundGridEvents[key] as GridEvent;
//            if (target != null)
//            {
//                this.m_boundGridEvents.Remove(key);
//                target.EventFired -= new EventHandler(this.CellEventFired);
//                Delegate handler = Delegate.CreateDelegate(eventHandlerType, target, "HandleEvent");
//                info.RemoveEventHandler(o, handler);
//            }
//        }

//        private void UpdateEventBindings(object inputComponent, ValidationExpression expression, bool initialBinding)
//        {
//            ValidationExpression expression2 = initialBinding ? null : (this.m_controlExpressions[inputComponent] as ValidationExpression);
//            Control control = inputComponent as Control;
//            PropertyInfo errorDescription = null;
//            MethodInfo setValidationExpression = null;
//            MethodInfo getValidationExpression = null;
//            bool flag = false;
//            bool flag2 = false;
//            if ((expression == null) && (expression2 != null))
//            {
//                flag = expression2.ValidationLevel != ValidationLevel.Manual;
//            }
//            else if ((expression != null) && (expression2 != null))
//            {
//                flag = (expression2.ValidationLevel != ValidationLevel.Manual) && (expression.ValidationLevel == ValidationLevel.Manual);
//                flag2 = ((expression2.ValidationLevel == ValidationLevel.Manual) && (expression.ValidationLevel != ValidationLevel.Manual)) || (initialBinding && (expression.ValidationLevel != ValidationLevel.Manual));
//            }
//            else if ((expression != null) && (expression2 == null))
//            {
//                flag2 = expression.ValidationLevel != ValidationLevel.Manual;
//            }
//            else
//            {
//                if (expression == null)
//                {
//                }
//                return;
//            }
//            if (flag)
//            {
//                if (control != null)
//                {
//                    control.Validating -= new CancelEventHandler(this.General_Validating);
//                }
//                else if (IsObjectACell(inputComponent, out errorDescription, out getValidationExpression, out setValidationExpression))
//                {
//                    this.UnBindCellEvent(inputComponent, "LeavingEdit");
//                    this.UnBindCellEvent(inputComponent, "ValidationError");
//                    this.UnBindCellEvent(inputComponent, "EditLeft");
//                }
//            }
//            if (flag2)
//            {
//                if (control != null)
//                {
//                    control.Validating += new CancelEventHandler(this.General_Validating);
//                }
//                else if (IsObjectACell(inputComponent, out errorDescription, out getValidationExpression, out setValidationExpression))
//                {
//                    this.BindCellEvent(inputComponent, "LeavingEdit");
//                    this.BindCellEvent(inputComponent, "ValidationError");
//                    this.BindCellEvent(inputComponent, "EditLeft");
//                }
//            }
//            if ((control == null) && IsObjectACell(inputComponent, out errorDescription, out getValidationExpression, out setValidationExpression))
//            {
//                setValidationExpression.Invoke(inputComponent, new object[] { expression });
//            }
//        }

//        private bool Validate(Control parentControl)
//        {
//            bool flag = true;
//            foreach (Control control in parentControl.Controls)
//            {
//                flag &= this.Validate(control);
//            }
//            return (flag & this.ValidateControl(parentControl, true, true, null));
//        }

//        public bool Validate(object component, bool validateHidden, params object[] ignoredComponents)
//        {
//            Control parentControl = component as Control;
//            if (parentControl != null)
//            {
//                if (validateHidden)
//                {
//                    if ((ignoredComponents != null) && (ignoredComponents.Length > 0))
//                    {
//                        return this.ValidateControls(parentControl, ignoredComponents);
//                    }
//                    object[] objArray = ignoredComponents;
//                    for (int i = 0; i < objArray.Length; i++)
//                    {
//                        if (objArray[i] == null)
//                        {
//                            throw new InvalidOperationException("Elements within ignoredComponents should not be null");
//                        }
//                    }
//                    return this.ValidateControls(parentControl);
//                }
//                if ((ignoredComponents != null) && (ignoredComponents.Length > 0))
//                {
//                    return this.ValidateControlsIgnoreHidden(parentControl, ignoredComponents);
//                }
//                return this.ValidateControlsIgnoreHidden(parentControl);
//            }
//            if (component.CreateType().GetMethod("InputValidatorValidateAndSetErrorParameters") != null)
//            {
//                return this.ValidateGridElement(component, true, validateHidden, ignoredComponents);
//            }
//            return true;
//        }

//        public bool Validate(object component, bool validateChildren, bool validateHidden)
//        {
//            Control control = component as Control;
//            if (control != null)
//            {
//                if (!validateChildren)
//                {
//                    return this.ValidateControl(control, validateChildren, validateHidden, null);
//                }
//                if (validateHidden)
//                {
//                    return this.ValidateControls(control);
//                }
//                return this.ValidateControlsIgnoreHidden(control);
//            }
//            if (component.CreateType().GetMethod("InputValidatorValidateAndSetErrorParameters") != null)
//            {
//                return this.ValidateGridElement(component, validateChildren, validateHidden, null);
//            }
//            return true;
//        }

//        [EditorBrowsable(EditorBrowsableState.Never), StrongNameIdentityPermission(SecurityAction.InheritanceDemand, PublicKey = "002400000480000094000000060200000024000052534131000400000100010071E9D3E8FB4B521B04EA8F76D94C5FE657793FF51E88DD9DD1AD6D056525454770A74B63478A1B63ED2AD979E65BEE25DE44BA686242CE430F0C7DF1475C18BEB5467555F740961A969E5E411CA4567C73B471F32815041356C9A106309D46EF9F232CB4BB0B0746475B5DB8CE1D4FCF17D99A80A0F7205DFD03D43B109583C2")]
//        public bool ValidateCell(object cell, object stringValue, object validationExpression, object[] result)
//        {
//            if (((cell == null) || (validationExpression == null)) || (result == null))
//            {
//                return true;
//            }
//            bool flag = true;
//            ValidationExpression expression = validationExpression as ValidationExpression;
//            if (expression == null)
//            {
//                return true;
//            }
//            if (expression.ValidationLevel == ValidationLevel.Automatic)
//            {
//                return true;
//            }
//            if (!expression.CanEvaluate(cell))
//            {
//                throw new InvalidOperationException("An attempt was made to evaluate an invalid validation expression on a cell during custom validation.");
//            }
//            string validationMessage = string.Empty;
//            flag &= expression.Evaluate(this, cell, (string)stringValue, out validationMessage);
//            result[0] = (validationMessage == null) ? null : validationMessage.Clone();
//            return flag;
//        }

//        private bool ValidateControl(Control control, bool validateChildren, bool validateHidden, params object[] ignoredComponents)
//        {
//            if (control == null)
//            {
//                return true;
//            }
//            ValidationExpression expression = this.m_controlExpressions[control] as ValidationExpression;
//            if (expression == null)
//            {
//                if (control.CreateType().GetMethod("InputValidatorValidateAndSetErrorParameters") != null)
//                {
//                    return this.ValidateGridElement(control, validateChildren, validateHidden, ignoredComponents);
//                }
//                return true;
//            }
//            if (!((IExtenderProvider)this).CanExtend(control))
//            {
//                return true;
//            }
//            string validationMessage = string.Empty;
//            if (expression.ValidationLevel != ValidationLevel.Automatic)
//            {
//                if (!expression.CanEvaluate(control))
//                {
//                    throw new InvalidOperationException("An attempt was made to evaluate an invalid validation expression on " + control.ToString() + "." + Environment.NewLine + "Verify the type of each operand for expression control " + control.ToString());
//                }
//                string valueToValidate = (string)TypeDescriptor.GetProperties(control)["Text"].GetValue(control);
//                if (!expression.Evaluate(this, control, valueToValidate, out validationMessage))
//                {
//                    this.ShowError(control, validationMessage);
//                    return false;
//                }
//                this.RemoveError(control);
//            }
//            return true;
//        }

//        private bool ValidateControls(Control parentControl)
//        {
//            bool flag = true;
//            if (parentControl == null)
//            {
//                return flag;
//            }
//            foreach (Control control in parentControl.Controls)
//            {
//                flag &= this.ValidateControls(control);
//            }
//            return (flag & this.ValidateControl(parentControl, true, true, null));
//        }

//        private bool ValidateControls(Control parentControl, params object[] ignoredComponents)
//        {
//            bool flag = true;
//            bool flag2 = false;
//            if (ignoredComponents != null)
//            {
//                System.Type type = parentControl.CreateType();
//                for (int i = 0; i < ignoredComponents.Length; i++)
//                {
//                    if (ignoredComponents[i].Equals(parentControl) || ignoredComponents[i].Equals(type))
//                    {
//                        flag2 = true;
//                    }
//                }
//            }
//            if ((parentControl == null) || flag2)
//            {
//                return flag;
//            }
//            foreach (Control control in parentControl.Controls)
//            {
//                flag &= this.ValidateControls(control, ignoredComponents);
//            }
//            return (flag & this.ValidateControl(parentControl, true, true, ignoredComponents));
//        }

//        private bool ValidateControlsIgnoreHidden(Control parentControl)
//        {
//            bool flag = true;
//            if ((parentControl == null) || !parentControl.Visible)
//            {
//                return flag;
//            }
//            foreach (Control control in parentControl.Controls)
//            {
//                flag &= this.ValidateControlsIgnoreHidden(control);
//            }
//            return (flag & this.ValidateControl(parentControl, true, false, null));
//        }

//        private bool ValidateControlsIgnoreHidden(Control parentControl, params object[] ignoredComponents)
//        {
//            bool flag = true;
//            bool flag2 = false;
//            if (ignoredComponents != null)
//            {
//                System.Type type = parentControl.CreateType();
//                for (int i = 0; i < ignoredComponents.Length; i++)
//                {
//                    if (ignoredComponents[i].Equals(parentControl) || ignoredComponents[i].Equals(type))
//                    {
//                        flag2 = true;
//                    }
//                }
//            }
//            if (((parentControl == null) || !parentControl.Visible) || flag2)
//            {
//                return flag;
//            }
//            foreach (Control control in parentControl.Controls)
//            {
//                flag &= this.ValidateControlsIgnoreHidden(control, ignoredComponents);
//            }
//            return (flag & this.ValidateControl(parentControl, true, false, ignoredComponents));
//        }

//        private bool ValidateGridElement(object component, bool validateChildren, bool validateHidden, params object[] ignoredComponents)
//        {
//            System.Type type = component.CreateType();
//            MethodInfo method = type.GetMethod("InputValidatorValidateAndSetErrorParameters");
//            if (method == null)
//            {
//                throw new ArgumentException(type.Name + " is not recognized as a compatible grid element for this operation. ");
//            }
//            return (bool)method.Invoke(component, new object[] { new ValidateCellDelegate(this.ValidateCell), validateChildren, validateHidden, ignoredComponents, this.Icon, this.BlinkRate, this.BlinkStyle });
//        }

//        [DefaultValue(250)]
//        internal int BlinkRate
//        {
//            get
//            {
//                return this.m_errorProvider.BlinkRate;
//            }
//            set
//            {
//                this.m_errorProvider.BlinkRate = value;
//            }
//        }

//        [DefaultValue(2)]
//        internal ErrorBlinkStyle BlinkStyle
//        {
//            get
//            {
//                return this.m_errorProvider.BlinkStyle;
//            }
//            set
//            {
//                this.m_errorProvider.BlinkStyle = value;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public ValidationMessages DefaultValidationMessages
//        {
//            get
//            {
//                return this.m_validationMessageSettings;
//            }
//            set
//            {
//                this.m_validationMessageSettings = value;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public System.Drawing.Icon Icon
//        {
//            get
//            {
//                return this.m_errorProvider.Icon;
//            }
//            set
//            {
//                this.m_errorProvider.Icon = value;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        protected IDictionary Properties
//        {
//            get
//            {
//                if (this.m_properties == null)
//                {
//                    this.m_properties = new Hashtable();
//                }
//                return this.m_properties;
//            }
//        }

//        internal class EventBinding
//        {
//            private string m_eventName;
//            private object m_object;

//            public EventBinding(object o, string eventName)
//            {
//                this.m_object = o;
//                this.m_eventName = eventName;
//            }

//            public override bool Equals(object o)
//            {
//                MyValidationProvider.EventBinding binding = o as MyValidationProvider.EventBinding;
//                if (binding == null)
//                {
//                    return false;
//                }
//                return ((binding.m_object == this.m_object) && (this.m_eventName == binding.m_eventName));
//            }

//            public override int GetHashCode()
//            {
//                return (this.m_object.GetHashCode() + this.m_eventName.GetHashCode());
//            }
//        }

//        private delegate bool ValidateCellDelegate(object cell, object stringValue, object validationExpression, object[] result);
//    }
//}