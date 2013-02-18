using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid;

namespace Feng.Grid.Columns
{
    /// <summary>
    /// 用于显示警示的Column
    /// </summary>
    public class WarningColumn : Column
    {
        private IList<GridColumnWarningInfo> m_warningInfos;

        private string m_warningName;

        protected WarningColumn(WarningColumn template)
            : base(template)
        {
            this.m_warningName = template.m_warningName;
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="warningName"></param>
        public WarningColumn(string fieldName, string warningName)
            : base(fieldName, typeof(string))
        {
            m_warningName = warningName;
            Initialize();
        }

        private void Initialize()
        {
            m_warningInfos = ADInfoBll.Instance.GetWarningInfo(m_warningName);
            string imageNames, textNames;
            StringBuilder sb = new StringBuilder();
            foreach (GridColumnWarningInfo info in m_warningInfos)
            {
                sb.Append(info.ImageName);
                sb.Append(",");
            }
            if (sb.Length > 0)
            {
                imageNames = sb.Remove(sb.Length - 1, 1).ToString();
                sb.Remove(0, sb.Length);
            }
            else
            {
                imageNames = string.Empty;
            }

            foreach (GridColumnWarningInfo info in m_warningInfos)
            {
                sb.Append(info.Text);
                sb.Append(",");
            }
            if (sb.Length > 0)
            {
                textNames = sb.Remove(sb.Length - 1, 1).ToString();
                sb.Remove(0, sb.Length);
            }
            else
            {
                textNames = string.Empty;
            }

            this.CellViewerManager = new Viewers.ImageTextViewer(imageNames, textNames);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Column CreateInstance()
        {
            return new WarningColumn(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public string Calculate(object entity)
        {
            StringBuilder sb = new StringBuilder();
            foreach (GridColumnWarningInfo info in m_warningInfos)
            {
                if (!Authority.AuthorizeByRule(info.Visible))
                    continue;

                object ret = EntityScript.CalculateExpression(info.Expression, entity);
                if (ret != null && (bool)ret)
                {
                    sb.Append(info.Text);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
    }
}
