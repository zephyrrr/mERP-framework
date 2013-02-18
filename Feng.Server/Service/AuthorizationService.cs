using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Server.Service
{
    public class AuthorizationService
    {
        public AuthorizationService()
        {
            if (System.Web.HttpContext.Current != null)
            {
                System.Threading.Thread.CurrentPrincipal = System.Web.HttpContext.Current.User;
            }
            //var s = System.ServiceModel.ServiceSecurityContext.Current.PrimaryIdentity.Name;
        }
    }
}
