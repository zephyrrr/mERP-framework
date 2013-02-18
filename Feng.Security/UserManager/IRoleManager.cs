// ? 2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;

namespace Feng.UserManager
{
    /// <summary>
    /// IRoleManager
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// AddUsersToRole
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userNames"></param>
        /// <param name="role"></param>
        void AddUsersToRole(string application, string[] userNames, string role);

        /// <summary>
        /// AddUsersToRoles
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userNames"></param>
        /// <param name="roles"></param>
        void AddUsersToRoles(string application, string[] userNames, string[] roles);

        /// <summary>
        /// AddUserToRole
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        void AddUserToRole(string application, string userName, string role);

        /// <summary>
        /// AddUserToRoles
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        void AddUserToRoles(string application, string userName, string[] roles);

        /// <summary>
        /// CreateRole
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        void CreateRole(string application, string role);

        /// <summary>
        /// DeleteRole
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <returns></returns>
        bool DeleteRole(string application, string role, bool throwOnPopulatedRole);

        /// <summary>
        /// DeleteAllRoles
        /// </summary>
        /// <param name="application"></param>
        /// <param name="throwOnPopulatedRole"></param>
        void DeleteAllRoles(string application, bool throwOnPopulatedRole);

        /// <summary>
        /// GetAllRoles
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        string[] GetAllRoles(string application);

        /// <summary>
        /// GetRolesForUser
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        string[] GetRolesForUser(string application, string userName);

        /// <summary>
        /// GetUsersInRole
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        string[] GetUsersInRole(string application, string role);

        /// <summary>
        /// IsRolesEnabled
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        bool IsRolesEnabled(string application);

        /// <summary>
        /// RoleExists
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool RoleExists(string application, string role);

        /// <summary>
        /// RemoveUserFromRole
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        void RemoveUserFromRole(string application, string userName, string roleName);

        /// <summary>
        /// RemoveUsersFromRole
        /// </summary>
        /// <param name="application"></param>
        /// <param name="users"></param>
        /// <param name="role"></param>
        void RemoveUsersFromRole(string application, string[] users, string role);

        /// <summary>
        /// RemoveUserFromRoles
        /// </summary>
        /// <param name="application"></param>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        void RemoveUserFromRoles(string application, string user, string[] roles);

        /// <summary>
        /// RemoveUsersFromRoles
        /// </summary>
        /// <param name="application"></param>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        void RemoveUsersFromRoles(string application, string[] users, string[] roles);
    }
}