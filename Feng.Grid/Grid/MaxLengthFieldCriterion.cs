using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Xceed.Validation;

namespace Feng.Grid
{
    /// <summary>
    /// 限制最大长度
    /// </summary>
    public class MaxLengthFieldCriterion : MyRegularExpressionCriterion, ICloneable
    {
        private int m_maxLength = -1;

        /// <summary>
        /// 
        /// </summary>
        public MaxLengthFieldCriterion()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criterionName"></param>
        /// <param name="level"></param>
        /// <param name="maxLength"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="trim"></param>
        /// <param name="customMessages"></param>
        public MaxLengthFieldCriterion(string criterionName, ValidationLevel level, int maxLength,
                                       bool caseSensitive, bool trim, CustomValidationMessages customMessages)
            : base(criterionName, level, null, caseSensitive, trim, customMessages)
        {
            this.MaxLength = maxLength;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentToValidate"></param>
        /// <returns></returns>
        public override bool CanEvaluate(object componentToValidate)
        {
            return (this.m_maxLength > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MaxLengthFieldCriterion(base.CriterionName, base.ValidationLevel, this.m_maxLength,
                                               base.CaseSensitive, base.Trim,
                                               base.CustomValidationMessages.Clone() as CustomValidationMessages);
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxLength
        {
            get { return m_maxLength; }
            set
            {
                if (m_maxLength == value)
                {
                    return;
                }

                m_maxLength = value;
                BuildRegex();
            }
        }

        private void BuildRegex()
        {
            base.Regex = new Regex(@"^.{0," + MaxLength + "}$", RegexOptions.Singleline);
            base.CustomValidationMessages.RegularExpression = base.CriterionName + "长度最长为为" + MaxLength;
        }
    }
}