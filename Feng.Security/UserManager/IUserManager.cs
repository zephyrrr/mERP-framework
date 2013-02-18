// ?2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;

namespace Feng.UserManager
{
    /// <summary>
    /// IUserManager
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Authenticate(string applicationName, string userName, string password);

        /// <summary>
        /// IsInRole
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool IsInRole(string applicationName, string userName, string role);

        /// <summary>
        /// GetRoles
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        string[] GetRoles(string applicationName, string userName);
    }
}