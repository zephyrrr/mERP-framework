using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using NHibernate;
using NHibernate.Exceptions;

namespace Feng.NH
{
    /// <summary>
    /// MS数据库的Exception转换（根据错误代码转换成不同Exception）
    /// </summary>
    public class MsSqlExceptionConverter : ISQLExceptionConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="adoExceptionContextInfo"></param>
        /// <returns></returns>
        public Exception Convert(AdoExceptionContextInfo adoExceptionContextInfo)
        {
            var sqle = ADOExceptionHelper.ExtractDbException(adoExceptionContextInfo.SqlException) as SqlException;
            if (sqle != null)
            {
                switch (sqle.Number)
                {
                    case 547:
                    case 2627:
                        return new ConstraintViolationException(adoExceptionContextInfo.SqlException.Message,
                            null, adoExceptionContextInfo.Sql, null);
                    case 208:
                        return new SQLGrammarException(adoExceptionContextInfo.SqlException.Message,
                            null, adoExceptionContextInfo.Sql);
                    case 3960:
                        return new StaleObjectStateException(adoExceptionContextInfo.EntityName, adoExceptionContextInfo.EntityId);
                }
            }
            return SQLStateConverter.HandledNonSpecificException(adoExceptionContextInfo.SqlException,
                adoExceptionContextInfo.SqlException.Message, adoExceptionContextInfo.Sql);
        }
    }
}
