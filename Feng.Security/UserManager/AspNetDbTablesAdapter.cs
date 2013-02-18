using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Feng.UserManager
{
    internal partial class AspNetDbTablesAdapter
    {
        private SqlCommand m_DeleteApplicationCommand;
        private SqlCommand m_DeleteallApplicationsCommand;

        private string GetConnectionString()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["LoginConnectionString"] == null)
            {
                throw new ArgumentException("You must special LoginConnectionString if u want to use AspNetDbTablesAdapter!");
            }
            return Properties.Settings.Default["LoginConnectionString"].ToString();
        }

        public AspNetDbTablesAdapter()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());

            m_DeleteApplicationCommand = new SqlCommand();
            m_DeleteallApplicationsCommand = new SqlCommand();

            m_DeleteApplicationCommand.Connection = connection;
            m_DeleteApplicationCommand.CommandType = CommandType.Text;

            m_DeleteallApplicationsCommand.Connection = connection;
            m_DeleteallApplicationsCommand.CommandType = CommandType.Text;

            m_DeleteApplicationCommand.CommandText =
                @"
DECLARE @ApplicationId uniqueidentifier
SELECT  @ApplicationId = ApplicationId
FROM dbo.aspnet_Applications
WHERE LOWER(@applicationName) = LoweredApplicationName
IF(@ApplicationId IS NOT NULL)
BEGIN
	DELETE FROM dbo.aspnet_UsersInRoles
WHERE
UserID IN (SELECT UserID FROM dbo.aspnet_Users WHERE ApplicationID = @ApplicationId)
AND
RoleID IN (SELECT RoleID FROM dbo.aspnet_Roles WHERE ApplicationID = @ApplicationId)
DELETE FROM dbo.aspnet_Roles WHERE ApplicationId = @ApplicationId
DELETE FROM dbo.aspnet_Membership WHERE ApplicationId = @ApplicationId
DELETE FROM dbo.aspnet_Users WHERE ApplicationId = @ApplicationId
DELETE FROM dbo.aspnet_Paths WHERE ApplicationId = @ApplicationId
DELETE FROM dbo.aspnet_Applications WHERE ApplicationId = @ApplicationId
END";
            m_DeleteApplicationCommand.Parameters.Add(new SqlParameter("@applicationName", SqlDbType.VarChar, 0,
                                                                       ParameterDirection.Input, 0, 0, null,
                                                                       DataRowVersion.Current, false, null, "", "", ""));

            m_DeleteallApplicationsCommand.CommandText =
                "DELETE FROM dbo.aspnet_UsersInRoles DELETE FROM dbo.aspnet_Roles DELETE FROM dbo.aspnet_Membership DELETE FROM dbo.aspnet_Users DELETE FROM dbo.aspnet_Paths DELETE FROM dbo.aspnet_Applications";
        }

        public void DeleteApplication(string applicationName)
        {
            if (String.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("applicationName cannot be null or empty");
            }

            m_DeleteApplicationCommand.Parameters[0].Value = applicationName;

            ExecuteCommand(m_DeleteApplicationCommand);
        }

        public void DeleteAllApplications()
        {
            ExecuteCommand(m_DeleteallApplicationsCommand);
        }

        private static void ExecuteCommand(SqlCommand command)
        {
            command.Connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                command.Connection.Close();
            }
        }
    }
}