// ? 2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;

namespace Feng.UserManager
{
    /// <summary>
    /// IApplicationManager
    /// </summary>
    public interface IApplicationManager
    {
        /// <summary>
        /// DeleteApplication
        /// </summary>
        /// <param name="application"></param>
        void DeleteApplication(string application);

        /// <summary>
        /// GetApplications
        /// </summary>
        /// <returns></returns>
        string[] GetApplications();

        /// <summary>
        /// DeleteAllApplications
        /// </summary>
        void DeleteAllApplications();
    }
}