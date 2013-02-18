using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Feng.Data
{
    public class DbHelperNH
    {
        /// <summary>
        /// 采用NHibernate的Transaction
        /// 在外部开始Transaction，外部处理Exception
        /// </summary>
        /// <param name="cmds"></param>
        /// <param name="session"></param>
        public static void UpdateTransaction(ICollection<MyDbCommand> cmds, NHibernate.ISession session)
        {
            if (cmds == null)
            {
                throw new ArgumentNullException("cmds");
            }
            if (cmds.Count == 0)
            {
                return;
            }
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            DbConnection conn = session.Connection as DbConnection;
            if (conn == null)
            {
                throw new NotSupportedException("session must be DbConnection session");
            }

            DbHelper.UpdateTransaction(cmds, new DbHelper.SetCommand(delegate(MyDbCommand cmd)
            {
                session.Transaction.Enlist(cmd.Command);
                cmd.Command.Connection = conn;
            }));
        }
    }
}
