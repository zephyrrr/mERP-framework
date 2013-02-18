using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Xceed.Validation;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRegularExpressionCriterion : ValidationCriterion, ICloneable
    {
        private Regex m_regex;

        /// <summary>
        /// 
        /// </summary>
        public MyRegularExpressionCriterion()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criterionName"></param>
        /// <param name="level"></param>
        /// <param name="regularExpression"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="trim"></param>
        /// <param name="customMessages"></param>
        public MyRegularExpressionCriterion(string criterionName, ValidationLevel level, Regex regularExpression,
                                            bool caseSensitive, bool trim,
                                            CustomValidationMessages customMessages)
            : base(criterionName, level, caseSensitive, trim, customMessages)
        {
            this.m_regex = regularExpression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentToValidate"></param>
        /// <returns></returns>
        public override bool CanEvaluate(object componentToValidate)
        {
            return (this.m_regex != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MyRegularExpressionCriterion(base.CriterionName, base.ValidationLevel, this.Regex,
                                                    base.CaseSensitive, base.Trim,
                                                    base.CustomValidationMessages.Clone() as CustomValidationMessages);
        }

        /// <summary>
        /// 验证是否符合格式。（只读的不验证）
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
            
            if (valueToValidate == null)
            {
                valueToValidate = string.Empty;
            }

            if (base.Trim)
            {
                valueToValidate = valueToValidate.Trim();
            }

            if (string.IsNullOrEmpty(valueToValidate))
            {
                return true;
            }

            bool success = true;
            //Regex regex = base.CaseSensitive ? new Regex(this.RegularExpression ?? "") : new Regex(this.RegularExpression ?? "", RegexOptions.IgnoreCase);
            success = m_regex.Match(valueToValidate).Success;
            if (!success)
            {
                validationMessage = (base.CustomValidationMessages.RegularExpression != null)
                                        ? base.CustomValidationMessages.RegularExpression
                                        : validationProvider.DefaultValidationMessages.RegularExpression;
                validationMessage = validationMessage.Replace("%name%", base.CriterionName);
            }
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        public Regex Regex
        {
            get { return this.m_regex; }
            set { this.m_regex = value; }
        }
    }
}