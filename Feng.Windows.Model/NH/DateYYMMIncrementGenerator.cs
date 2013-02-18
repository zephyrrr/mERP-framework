using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Id;
using Feng.Data;

namespace Feng.NH
{
    /// <summary>
    /// 根据日期生成YYMMXXXX形式的主键
    /// </summary>
    public class DateYYMMIncrementGenerator : IIdentifierGenerator
    {
        /// <summary>
        /// 生成主键
        /// </summary>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object Generate(ISessionImplementor session, object obj)
        {
            TypedEntityMetadata entityInfo = TypedEntityMetadata.GenerateEntityInfo(session.Factory, obj.GetType());

            return PrimaryMaxIdGenerator.GetMaxId(entityInfo.TableName, entityInfo.IdColumnName, entityInfo.IdLength, 
                PrimaryMaxIdGenerator.GetIdYearMonth()).ToString();
        }
    }
}
