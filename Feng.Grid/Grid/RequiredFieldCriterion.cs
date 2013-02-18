using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Validation;

namespace Feng.Grid
{
    /// <summary>
    /// RequiredFieldCriterion
    /// </summary>
    public class RequiredFieldCriterion : Xceed.Validation.ValidationCriterion, ICloneable
    {
        private Type m_dataType;
        //private static Dictionary<Type, object> s_nullValues = new Dictionary<Type, object>();

        static RequiredFieldCriterion()
        {
            //s_nullValues[typeof(string)] = string.Empty;
            //s_nullValues[typeof(Guid)] = Guid.Empty;
            ////m_nullValues[typeof(Int32)] = 0;
            ////m_nullValues[typeof(Int64)] = 0;
            //s_nullValues[typeof(DateTime)] = new DateTime();
            ////m_nullValues[typeof(Decimal)] = Decimal.Zero;
            ////m_nullValues[typeof(Double)] = 0d;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RequiredFieldCriterion()
            : this(typeof (string))
        {
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="dataType"></param>
        public RequiredFieldCriterion(Type dataType)
        {
            this.m_dataType = dataType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="criterionName"></param>
        /// <param name="dataType"></param>
        /// <param name="level"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="trim"></param>
        /// <param name="customMessages"></param>
        public RequiredFieldCriterion(string criterionName, ValidationLevel level, Type dataType, bool caseSensitive,
                                      bool trim, CustomValidationMessages customMessages)
            : base(criterionName, level, caseSensitive, trim, customMessages)
        {
            this.m_dataType = dataType;
        }

        /// <summary>
        /// CanEvaluate
        /// </summary>
        /// <param name="componentToValidate"></param>
        /// <returns></returns>
        public override bool CanEvaluate(object componentToValidate)
        {
            return true;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new RequiredFieldCriterion(base.CriterionName, base.ValidationLevel, this.DataType,
                                              base.CaseSensitive, base.Trim,
                                              base.CustomValidationMessages.Clone() as CustomValidationMessages);
        }

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <param name="validationProvider"></param>
        /// <param name="componentToValidate"></param>
        /// <param name="valueToValidate"></param>
        /// <param name="validationMessage"></param>
        /// <returns></returns>
        public override bool Evaluate(ValidationProvider validationProvider, object componentToValidate,
                                      string valueToValidate, out string validationMessage)
        {
            validationMessage = string.Empty;

            IDataValueControl dataControl = componentToValidate as IDataValueControl;
            if (dataControl != null)
            {
                valueToValidate = (dataControl.SelectedDataValue == null
                                       ? null
                                       : dataControl.SelectedDataValue.ToString());
                if (dataControl.ReadOnly)
                {
                    return true;
                }
            }
            Xceed.Grid.Cell cell = componentToValidate as Xceed.Grid.Cell;
            if (cell != null)
            {
                if (cell.ReadOnly)
                {
                    return true;
                }
            }

            //object nullValue;
            //if (!s_nullValues.ContainsKey(m_dataType))
            //{
            //    nullValue = string.Empty;
            //}
            //else
            //{
            //    nullValue = s_nullValues[m_dataType];
            //}


            if (valueToValidate == null)
            {
                valueToValidate = string.Empty;
            }
            if ((valueToValidate != null) && base.Trim)
            {
                valueToValidate = valueToValidate.Trim();
            }

            if (string.IsNullOrEmpty(valueToValidate))
            {
                validationMessage = (base.CustomValidationMessages.RequiredField != null)
                                        ? base.CustomValidationMessages.RequiredField
                                        : validationProvider.DefaultValidationMessages.RequiredField;
                validationMessage = validationMessage.Replace("%name%", base.CriterionName);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// DataType
        /// </summary>
        public Type DataType
        {
            get { return this.m_dataType; }
            set { this.m_dataType = value; }
        }
    }
}