using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Net
{
    public class AuthRepositoryFactory : Feng.Net.RepositoryFactory
    {
        public override IRepository GenerateRepository(string repCfgName = null)
        {
            return new Feng.Net.Repository(new MyAuthHttpClient());
        }
    }
}
