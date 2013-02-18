using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.NH
{
    public interface INHibernateStatelessRepository : IDisposable
    {
        NHibernate.IStatelessSession Session
        {
            get;
        }
    }
}
