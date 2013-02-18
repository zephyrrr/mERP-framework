using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using NHibernate.Linq.Functions;

namespace Feng.NH
{
    public class LinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public LinqToHqlGeneratorsRegistry()
            : base()
        {
            var comp1 = new CompareToHqlGenerator();
            foreach (var i in comp1.SupportedMethods)
            {
                RegisterGenerator(i, comp1);
            }

            var comp2 = new CompareHqlGenerator();
            foreach (var i in comp2.SupportedMethods)
            {
                RegisterGenerator(i, comp2);
            }
        }
    }
}
