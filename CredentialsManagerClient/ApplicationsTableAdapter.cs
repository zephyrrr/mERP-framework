using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

[DataObject(true), ToolboxItem(true), DesignerCategory("code"), Designer("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), HelpKeyword("vs.data.TableAdapter")]
public class ApplicationsTableAdapter : Component
{
    private SqlDataAdapter m_adapter;
    private bool m_clearBeforeFill = true;
    private SqlCommand[] m_commandCollection;
    private SqlConnection m_connection;

    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public virtual int Delete(string Original_ApplicationName, string Original_LoweredApplicationName, Guid Original_ApplicationId, string Original_Description)
    {
        int num;
        if (Original_ApplicationName == null)
        {
            throw new ArgumentNullException("Original_ApplicationName");
        }
        this.Adapter.DeleteCommand.Parameters[0].Value = Original_ApplicationName;
        if (Original_LoweredApplicationName == null)
        {
            throw new ArgumentNullException("Original_LoweredApplicationName");
        }
        this.Adapter.DeleteCommand.Parameters[1].Value = Original_LoweredApplicationName;
        this.Adapter.DeleteCommand.Parameters[2].Value = Original_ApplicationId;
        if (Original_Description == null)
        {
            this.Adapter.DeleteCommand.Parameters[3].Value = 1;
            this.Adapter.DeleteCommand.Parameters[4].Value = DBNull.Value;
        }
        else
        {
            this.Adapter.DeleteCommand.Parameters[3].Value = 0;
            this.Adapter.DeleteCommand.Parameters[4].Value = Original_Description;
        }
        ConnectionState state = this.Adapter.DeleteCommand.Connection.State;
        this.Adapter.DeleteCommand.Connection.Open();
        try
        {
            num = this.Adapter.DeleteCommand.ExecuteNonQuery();
        }
        finally
        {
            if (state == ConnectionState.Closed)
            {
                this.Adapter.DeleteCommand.Connection.Close();
            }
        }
        return num;
    }

    [DataObjectMethod(DataObjectMethodType.Fill, true)]
    public virtual int Fill(AspNetDbDataSet.aspnet_ApplicationsDataTable dataTable)
    {
        this.Adapter.SelectCommand = this.CommandCollection[0];
        if (this.m_clearBeforeFill)
        {
            dataTable.Clear();
        }
        return this.Adapter.Fill(dataTable);
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public virtual AspNetDbDataSet.aspnet_ApplicationsDataTable GetData()
    {
        this.Adapter.SelectCommand = this.CommandCollection[0];
        AspNetDbDataSet.aspnet_ApplicationsDataTable dataTable = new AspNetDbDataSet.aspnet_ApplicationsDataTable();
        this.Adapter.Fill(dataTable);
        return dataTable;
    }

    private void InitAdapter()
    {
        this.m_adapter = new SqlDataAdapter();
        DataTableMapping mapping = new DataTableMapping {
            SourceTable = "Table",
            DataSetTable = "aspnet_Applications"
        };
        mapping.ColumnMappings.Add("applicationName", "applicationName");
        mapping.ColumnMappings.Add("LoweredApplicationName", "LoweredApplicationName");
        mapping.ColumnMappings.Add("ApplicationId", "ApplicationId");
        mapping.ColumnMappings.Add("Description", "Description");
        this.m_adapter.TableMappings.Add(mapping);
        this.m_adapter.DeleteCommand = new SqlCommand();
        this.m_adapter.DeleteCommand.Connection = this.Connection;
        this.m_adapter.DeleteCommand.CommandText = "DELETE FROM [dbo].[aspnet_Applications] WHERE (([applicationName] = @Original_ApplicationName) AND ([LoweredApplicationName] = @Original_LoweredApplicationName) AND ([ApplicationId] = @Original_ApplicationId) AND ((@IsNull_Description = 1 AND [Description] IS NULL) OR ([Description] = @Original_Description)))";
        this.m_adapter.DeleteCommand.CommandType = CommandType.Text;
        this.m_adapter.DeleteCommand.Parameters.Add(new SqlParameter("@Original_ApplicationName", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "applicationName", DataRowVersion.Original, false, null, "", "", ""));
        this.m_adapter.DeleteCommand.Parameters.Add(new SqlParameter("@Original_LoweredApplicationName", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "LoweredApplicationName", DataRowVersion.Original, false, null, "", "", ""));
        this.m_adapter.DeleteCommand.Parameters.Add(new SqlParameter("@Original_ApplicationId", SqlDbType.UniqueIdentifier, 0, ParameterDirection.Input, 0, 0, "ApplicationId", DataRowVersion.Original, false, null, "", "", ""));
        this.m_adapter.DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_Description", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Original, true, null, "", "", ""));
        this.m_adapter.DeleteCommand.Parameters.Add(new SqlParameter("@Original_Description", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Original, false, null, "", "", ""));
        this.m_adapter.InsertCommand = new SqlCommand();
        this.m_adapter.InsertCommand.Connection = this.Connection;
        this.m_adapter.InsertCommand.CommandText = "INSERT INTO [dbo].[aspnet_Applications] ([applicationName], [LoweredApplicationName], [ApplicationId], [Description]) VALUES (@applicationName, @LoweredApplicationName, @ApplicationId, @Description);\r\nSELECT applicationName, LoweredApplicationName, ApplicationId, Description FROM aspnet_Applications WHERE (ApplicationId = @ApplicationId)";
        this.m_adapter.InsertCommand.CommandType = CommandType.Text;
        this.m_adapter.InsertCommand.Parameters.Add(new SqlParameter("@applicationName", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "applicationName", DataRowVersion.Current, false, null, "", "", ""));
        this.m_adapter.InsertCommand.Parameters.Add(new SqlParameter("@LoweredApplicationName", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "LoweredApplicationName", DataRowVersion.Current, false, null, "", "", ""));
        this.m_adapter.InsertCommand.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.UniqueIdentifier, 0, ParameterDirection.Input, 0, 0, "ApplicationId", DataRowVersion.Current, false, null, "", "", ""));
        this.m_adapter.InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
        this.m_adapter.UpdateCommand = new SqlCommand();
        this.m_adapter.UpdateCommand.Connection = this.Connection;
        this.m_adapter.UpdateCommand.CommandText = "UPDATE [dbo].[aspnet_Applications] SET [applicationName] = @applicationName, [LoweredApplicationName] = @LoweredApplicationName, [ApplicationId] = @ApplicationId, [Description] = @Description WHERE (([applicationName] = @Original_ApplicationName) AND ([LoweredApplicationName] = @Original_LoweredApplicationName) AND ([ApplicationId] = @Original_ApplicationId) AND ((@IsNull_Description = 1 AND [Description] IS NULL) OR ([Description] = @Original_Description)));\r\nSELECT applicationName, LoweredApplicationName, ApplicationId, Description FROM aspnet_Applications WHERE (ApplicationId = @ApplicationId)";
        this.m_adapter.UpdateCommand.CommandType = CommandType.Text;
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@applicationName", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "applicationName", DataRowVersion.Current, false, null, "", "", ""));
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@LoweredApplicationName", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "LoweredApplicationName", DataRowVersion.Current, false, null, "", "", ""));
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.UniqueIdentifier, 0, ParameterDirection.Input, 0, 0, "ApplicationId", DataRowVersion.Current, false, null, "", "", ""));
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Original_ApplicationName", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "applicationName", DataRowVersion.Original, false, null, "", "", ""));
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Original_LoweredApplicationName", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "LoweredApplicationName", DataRowVersion.Original, false, null, "", "", ""));
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Original_ApplicationId", SqlDbType.UniqueIdentifier, 0, ParameterDirection.Input, 0, 0, "ApplicationId", DataRowVersion.Original, false, null, "", "", ""));
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_Description", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Original, true, null, "", "", ""));
        this.m_adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Original_Description", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Original, false, null, "", "", ""));
    }

    private void InitCommandCollection()
    {
        this.m_commandCollection = new SqlCommand[] { new SqlCommand() };
        this.m_commandCollection[0].Connection = this.Connection;
        this.m_commandCollection[0].CommandText = "SELECT applicationName, LoweredApplicationName, ApplicationId, Description FROM dbo.aspnet_Applications";
        this.m_commandCollection[0].CommandType = CommandType.Text;
    }

    private void InitConnection()
    {
        this.m_connection = new SqlConnection();
        this.m_connection.ConnectionString = DataAccessUtil.GetConnectionString();
    }

    [DataObjectMethod(DataObjectMethodType.Insert, true)]
    public virtual int Insert(string ApplicationName, string LoweredApplicationName, Guid ApplicationId, string Description)
    {
        int num;
        if (ApplicationName == null)
        {
            throw new ArgumentNullException("applicationName");
        }
        this.Adapter.InsertCommand.Parameters[0].Value = ApplicationName;
        if (LoweredApplicationName == null)
        {
            throw new ArgumentNullException("LoweredApplicationName");
        }
        this.Adapter.InsertCommand.Parameters[1].Value = LoweredApplicationName;
        this.Adapter.InsertCommand.Parameters[2].Value = ApplicationId;
        if (Description == null)
        {
            this.Adapter.InsertCommand.Parameters[3].Value = DBNull.Value;
        }
        else
        {
            this.Adapter.InsertCommand.Parameters[3].Value = Description;
        }
        ConnectionState state = this.Adapter.InsertCommand.Connection.State;
        this.Adapter.InsertCommand.Connection.Open();
        try
        {
            num = this.Adapter.InsertCommand.ExecuteNonQuery();
        }
        finally
        {
            if (state == ConnectionState.Closed)
            {
                this.Adapter.InsertCommand.Connection.Close();
            }
        }
        return num;
    }

    public virtual int Update(AspNetDbDataSet dataSet)
    {
        return this.Adapter.Update(dataSet, "aspnet_Applications");
    }

    public virtual int Update(DataRow[] dataRows)
    {
        return this.Adapter.Update(dataRows);
    }

    public virtual int Update(AspNetDbDataSet.aspnet_ApplicationsDataTable dataTable)
    {
        return this.Adapter.Update(dataTable);
    }

    public virtual int Update(DataRow dataRow)
    {
        return this.Adapter.Update(new DataRow[] { dataRow });
    }

    [DataObjectMethod(DataObjectMethodType.Update, true)]
    public virtual int Update(string ApplicationName, string LoweredApplicationName, Guid ApplicationId, string Description, string Original_ApplicationName, string Original_LoweredApplicationName, Guid Original_ApplicationId, string Original_Description)
    {
        int num;
        if (ApplicationName == null)
        {
            throw new ArgumentNullException("applicationName");
        }
        this.Adapter.UpdateCommand.Parameters[0].Value = ApplicationName;
        if (LoweredApplicationName == null)
        {
            throw new ArgumentNullException("LoweredApplicationName");
        }
        this.Adapter.UpdateCommand.Parameters[1].Value = LoweredApplicationName;
        this.Adapter.UpdateCommand.Parameters[2].Value = ApplicationId;
        if (Description == null)
        {
            this.Adapter.UpdateCommand.Parameters[3].Value = DBNull.Value;
        }
        else
        {
            this.Adapter.UpdateCommand.Parameters[3].Value = Description;
        }
        if (Original_ApplicationName == null)
        {
            throw new ArgumentNullException("Original_ApplicationName");
        }
        this.Adapter.UpdateCommand.Parameters[4].Value = Original_ApplicationName;
        if (Original_LoweredApplicationName == null)
        {
            throw new ArgumentNullException("Original_LoweredApplicationName");
        }
        this.Adapter.UpdateCommand.Parameters[5].Value = Original_LoweredApplicationName;
        this.Adapter.UpdateCommand.Parameters[6].Value = Original_ApplicationId;
        if (Original_Description == null)
        {
            this.Adapter.UpdateCommand.Parameters[7].Value = 1;
            this.Adapter.UpdateCommand.Parameters[8].Value = DBNull.Value;
        }
        else
        {
            this.Adapter.UpdateCommand.Parameters[7].Value = 0;
            this.Adapter.UpdateCommand.Parameters[8].Value = Original_Description;
        }
        ConnectionState state = this.Adapter.UpdateCommand.Connection.State;
        this.Adapter.UpdateCommand.Connection.Open();
        try
        {
            num = this.Adapter.UpdateCommand.ExecuteNonQuery();
        }
        finally
        {
            if (state == ConnectionState.Closed)
            {
                this.Adapter.UpdateCommand.Connection.Close();
            }
        }
        return num;
    }

    private SqlDataAdapter Adapter
    {
        get
        {
            if (this.m_adapter == null)
            {
                this.InitAdapter();
            }
            return this.m_adapter;
        }
    }

    public bool ClearBeforeFill
    {
        get
        {
            return this.m_clearBeforeFill;
        }
        set
        {
            this.m_clearBeforeFill = value;
        }
    }

    protected SqlCommand[] CommandCollection
    {
        get
        {
            if (this.m_commandCollection == null)
            {
                this.InitCommandCollection();
            }
            return this.m_commandCollection;
        }
    }

    internal SqlConnection Connection
    {
        get
        {
            if (this.m_connection == null)
            {
                this.InitConnection();
            }
            return this.m_connection;
        }
        set
        {
            this.m_connection = value;
            if (this.Adapter.InsertCommand != null)
            {
                this.Adapter.InsertCommand.Connection = value;
            }
            if (this.Adapter.DeleteCommand != null)
            {
                this.Adapter.DeleteCommand.Connection = value;
            }
            if (this.Adapter.UpdateCommand != null)
            {
                this.Adapter.UpdateCommand.Connection = value;
            }
            for (int i = 0; i < this.CommandCollection.Length; i++)
            {
                if (this.CommandCollection[i] != null)
                {
                    this.CommandCollection[i].Connection = value;
                }
            }
        }
    }
}

