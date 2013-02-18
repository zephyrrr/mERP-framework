using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    public class BoundGridGroupByRow : MyGroupByRow
    {
       /// <summary>
        /// Constructor
        /// </summary>
        public BoundGridGroupByRow()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        public BoundGridGroupByRow(BoundGridGroupByRow template)
            : base(template)
        {
        }

        /// <summary>
        /// CreateGroupManagerRowTemplate
        /// </summary>
        /// <returns></returns>
        protected override Xceed.Grid.GroupManagerRow CreateGroupManagerRowTemplate()
        {
            return new BoundGridGroupManagerRow();
        }
    }
}
