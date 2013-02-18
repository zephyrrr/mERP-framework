using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Xceed.Validation;

namespace Feng.Grid
{
    public class AndValidCriterion : ValidationCriterion, ICloneable
    {
        private ValidationCriterion m_criterion1, m_criterion2;

        /// <summary>
        /// 
        /// </summary>
        public AndValidCriterion()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criterionName"></param>
        /// <param name="level"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="trim"></param>
        /// <param name="customMessages"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public AndValidCriterion(string criterionName, ValidationLevel level, bool caseSensitive, bool trim, CustomValidationMessages customMessages,
            ValidationCriterion v1, ValidationCriterion v2)
            : base(criterionName, level, caseSensitive, trim, customMessages)
        {
            this.m_criterion1 = v1;
            this.m_criterion2 = v2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentToValidate"></param>
        /// <returns></returns>
        public override bool CanEvaluate(object componentToValidate)
        {
            return m_criterion1.CanEvaluate(componentToValidate) || m_criterion2.CanEvaluate(componentToValidate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new AndValidCriterion(base.CriterionName, base.ValidationLevel, base.CaseSensitive, base.Trim, 
                base.CustomValidationMessages.Clone() as CustomValidationMessages,
                m_criterion1, m_criterion2);
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
            string str = string.Empty;
            bool r1 = m_criterion1.Evaluate(validationProvider, componentToValidate, valueToValidate, out validationMessage);
            str += validationMessage;
            bool r2 = m_criterion2.Evaluate(validationProvider, componentToValidate, valueToValidate, out validationMessage);
            str += validationMessage;
            validationMessage = str;
            return r1 && r2;
        }
    }
}
