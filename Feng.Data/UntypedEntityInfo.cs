using System;
using System.Collections.Generic;
using System.Text;
using Feng.Utils;

namespace Feng
{
    /// <summary>
    /// DataRow EntityMetadata
    /// </summary>
    public class UntypedEntityMetadata : IEntityMetadata
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName"></param>
        public UntypedEntityMetadata(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            m_tableName = tableName;
            LoadSchema();
        }

        private System.Data.DataTable m_schema;
        private Dictionary<string, IPropertyMetadata> m_propertyInfos = new Dictionary<string, IPropertyMetadata>();
        private void LoadSchema()
        {
            m_schema = Feng.Data.DbHelper.Instance.GetSchema(m_tableName);

            foreach (System.Data.DataRow row in m_schema.Rows)
            {
                if (ConvertHelper.ToBoolean(row["IsIdentity"]).Value)
                {
                    m_idName = (string)row["ColumnName"];
                    m_idLength = ConvertHelper.ToInt(row["ColumnSize"]).Value;
                }

                m_propertyInfos[(string)row["ColumnName"]] = new EntityPropertyMetadata
                        {
                            Name = (string)row["ColumnName"],
                            Length = ConvertHelper.ToInt(row["ColumnSize"]).Value,
                            NotNull = !ConvertHelper.ToBoolean(row["AllowDBNull"]).Value
                        };
            }
        }

        private string m_idName;
        /// <summary>
        /// Id 名称
        /// </summary>
        public string IdName
        {
            get { return m_idName; }
        }

        private int m_idLength;
        /// <summary>
        /// 主键长度
        /// </summary>
        public int IdLength
        {
            get { return m_idLength; }
        }

        private string m_tableName;
        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName
        {
            get { return m_tableName; }
        }

        /// <summary>
        /// 获得实体类Property的属性
        /// </summary>
        /// <param name="proeprtyName"></param>
        /// <returns></returns>
        public IPropertyMetadata GetPropertMetadata(string proeprtyName)
        {
            if (m_propertyInfos.ContainsKey(proeprtyName))
                return m_propertyInfos[proeprtyName];
            else
                return null;
        }
    }
}
