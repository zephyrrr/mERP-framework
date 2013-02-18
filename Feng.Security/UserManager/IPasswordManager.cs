// ? 2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;

namespace Feng.UserManager
{
    /// <summary>
    /// IPasswordManager
    /// </summary>
    public interface IPasswordManager
    {
        /// <summary>
        /// EnablePasswordReset
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        bool EnablePasswordReset(string application);

        /// <summary>
        /// EnablePasswordRetrieval
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        bool EnablePasswordRetrieval(string application);

        /// <summary>
        /// GeneratePassword
        /// </summary>
        /// <param name="application"></param>
        /// <param name="length"></param>
        /// <param name="numberOfNonAlphanumericCharacters"></param>
        /// <returns></returns>
        string GeneratePassword(string application, int length, int numberOfNonAlphanumericCharacters);

        /// <summary>
        /// GetMaxInvalidPasswordAttempts
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        int GetMaxInvalidPasswordAttempts(string application);

        /// <summary>
        /// GetMinRequiredNonAlphanumericCharacters
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        int GetMinRequiredNonAlphanumericCharacters(string application);

        /// <summary>
        /// GetMinRequiredPasswordLength
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        int GetMinRequiredPasswordLength(string application);

        /// <summary>
        /// GetPasswordAttemptWindow
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        int GetPasswordAttemptWindow(string application);

        /// <summary>
        /// GetPasswordStrengthRegularExpression
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        string GetPasswordStrengthRegularExpression(string application);

        /// <summary>
        /// RequiresQuestionAndAnswer
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        bool RequiresQuestionAndAnswer(string application);

        /// <summary>
        /// ResetPassword
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        string ResetPassword(string application, string userName);

        /// <summary>
        /// ResetPasswordWithQuestionAndAnswer
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <returns></returns>
        string ResetPasswordWithQuestionAndAnswer(string application, string userName, string passwordAnswer);

        /// <summary>
        /// GetPassword
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <returns></returns>
        string GetPassword(string application, string userName, string passwordAnswer);

        /// <summary>
        /// GetPasswordQuestion
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        string GetPasswordQuestion(string application, string userName);

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        void ChangePassword(string application, string userName, string newPassword);

        /// <summary>
        /// ChangePasswordWithAnswer
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="newPassword"></param>
        void ChangePasswordWithAnswer(string application, string userName, string passwordAnswer, string newPassword);

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        bool ChangePasswordWithOldPassword(string application, string userName, string oldPassword, string newPassword);
    }
}