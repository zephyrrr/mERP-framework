// ? 2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;

namespace Feng.UserManager
{
    /// <summary>
    /// IMembershipManager
    /// </summary>
    public interface IMembershipManager
    {
        /// <summary>
        /// CreateUser
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        MembershipCreateStatus CreateUser(string application, string userName, string password, string email,
                                          string passwordQuestion, string passwordAnswer, bool isApproved);

        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        bool DeleteUser(string application, string userName, bool deleteAllRelatedData);

        /// <summary>
        /// DeleteAllUsers
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deleteAllRelatedData"></param>
        void DeleteAllUsers(string application, bool deleteAllRelatedData);

        /// <summary>
        /// GetUserNameByEmail
        /// </summary>
        /// <param name="application"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        string GetUserNameByEmail(string application, string email);

        /// <summary>
        /// GetNumberOfUsersOnline
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        int GetNumberOfUsersOnline(string application);

        /// <summary>
        /// UpdateUser
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="oldAnswer"></param>
        /// <param name="newQuestion"></param>
        /// <param name="newAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="isLockedOut"></param>
        void UpdateUser(string application, string userName, string email, string oldAnswer, string newQuestion,
                        string newAnswer, bool isApproved, bool isLockedOut);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="newQuestion"></param>
        /// <param name="newAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="isLockedOut"></param>
        void UpdateUserWithPassword(string application, string userName, string email, string password,
                                    string newQuestion, string newAnswer, bool isApproved, bool isLockedOut);

        /// <summary>
        /// UserIsOnlineTimeWindow
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        int UserIsOnlineTimeWindow(string application);

        /// <summary>
        /// GetAllUsers
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        string[] GetAllUsers(string application);

        /// <summary>
        /// GetUserInfo
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        UserInfo GetUserInfo(string application, string userName);
    }
}