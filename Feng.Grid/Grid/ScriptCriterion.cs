using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Xceed.Validation;

namespace Feng.Grid
{
    public class ScriptCriterion : ValidationCriterion, ICloneable
    {
        private Dictionary<string, object> m_param;
        private string m_script;

        /// <summary>
        /// 
        /// </summary>
        public ScriptCriterion()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criterionName"></param>
        /// <param name="level"></param>
        /// <param name="script"></param>
        /// <param name="param"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="trim"></param>
        /// <param name="customMessages"></param>
        public ScriptCriterion(string criterionName, ValidationLevel level, string script,  Dictionary<string, object> param,
                                            bool caseSensitive, bool trim,
                                            CustomValidationMessages customMessages)
            : base(criterionName, level, caseSensitive, trim, customMessages)
        {
            this.m_script = script;
            this.m_param = param;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentToValidate"></param>
        /// <returns></returns>
        public override bool CanEvaluate(object componentToValidate)
        {
            return !string.IsNullOrEmpty(this.m_script);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new ScriptCriterion(base.CriterionName, base.ValidationLevel, this.ValidScript, this.m_param, 
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

            if (valueToValidate != null && base.Trim)
            {
                valueToValidate = valueToValidate.Trim();
            }
            if (string.IsNullOrEmpty(valueToValidate))
            {
                valueToValidate = null;
            }

            Dictionary<string, object> p = new Dictionary<string, object> { 
                {"value", valueToValidate},
                {"cell", cell}, 
                {"dc", dataControl} };

            // "cm"
            foreach (var i in m_param)
            {
                p.Add(i.Key, i.Value);
            }
            object success = Script.CalculateExpression(m_script, p);
            if (success == null || !Convert.ToBoolean(success))
            {
                validationMessage = (base.CustomValidationMessages.RegularExpression != null)
                                        ? base.CustomValidationMessages.RegularExpression
                                        : validationProvider.DefaultValidationMessages.RegularExpression;
                validationMessage = validationMessage.Replace("%name%", base.CriterionName);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ValidScript
        {
            get { return this.m_script; }
            set { this.m_script = value; }
        }
    }
}
