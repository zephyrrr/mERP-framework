using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    /// <summary>
    /// GroupByRow
    /// </summary>
    public class MyGroupByRow : Xceed.Grid.GroupByRow
    {
        private const string title = "如想以某列分组，请拖动列标题到此处";
        /// <summary>
        /// Constructor
        /// </summary>
        public MyGroupByRow()
            : base()
        {
            this.NoGroupText = title;

            this.GroupTemplate = new Xceed.Grid.Group();
            this.GroupTemplate.HeaderRows.Add(CreateGroupManagerRowTemplate());
        }

        /// <summary>
        /// CreateGroupManagerRowTemplate
        /// </summary>
        /// <returns></returns>
        protected virtual Xceed.Grid.GroupManagerRow CreateGroupManagerRowTemplate()
        {
            return new MyGroupManagerRow();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        public MyGroupByRow(MyGroupByRow template)
            : base(template)
        {
            this.NoGroupText = title;
        }
    }
}
