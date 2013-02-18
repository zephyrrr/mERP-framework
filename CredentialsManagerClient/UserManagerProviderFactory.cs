namespace CredentialsManagerClient
{
    using System;
    using Feng.UserManager;

    public class UserManagerProviderFactory
    {
        internal static ProviderManager m_pm = new ProviderManager();

        internal static IApplicationManager CreateApplicationManager()
        {
            return m_pm.DefaultProvider.CreateApplicationManager();
        }

        internal static IMembershipManager CreateMembershipManager()
        {
            return m_pm.DefaultProvider.CreateMembershipManager();
        }

        internal static IPasswordManager CreatePasswordManager()
        {
            return m_pm.DefaultProvider.CreatePasswordManager();
        }

        internal static IRoleManager CreateRoleManager()
        {
            return m_pm.DefaultProvider.CreateRoleManager();
        }

        internal static IUserManager CreateUserManager()
        {
            return m_pm.DefaultProvider.CreateUserManager();
        }
    }
}

