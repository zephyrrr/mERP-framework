using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NHibernate;
using NHibernate.Type;

namespace Feng.NH
{
    public class DeleteLogInterceptor : NHibernate.EmptyInterceptor
    {
        /// <summary>
        /// ReexecuteDeleteLog
        /// </summary>
        /// <param name="list"></param>
        public static void ReexecuteDeleteLog(IList<AuditLogRecord> list)
        {
            using (INHibernateRepository rep = new Repository("All"))
            {
                try
                {
                    rep.BeginTransaction();
                    foreach (AuditLogRecord i in list)
                    {
                        if (i.LogType != "Delete")
                            continue;

                        if (rep.Session.SessionFactory.GetClassMetadata(i.EntityName) == null)
                        {
                            throw new ArgumentException("supplied Repository don't contain " + i.EntityName);
                        }

                        object entity = rep.Session.Get(i.EntityName, i.EntityId);
                        if (entity != null)
                        {
                            rep.Delete(entity);
                        }
                    }
                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        private ISession m_session;

        private List<AuditLogRecord> m_logRecords = new List<AuditLogRecord>();

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="session"></param>
        public override void SetSession(ISession session)
        {
            if (m_session != null && session != null)
            {
                throw new InvalidOperationException("Session should be null now!");
            }
            this.m_session = session;
        }

        /// <summary>
        /// OnDelete
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <param name="propertyNames"></param>
        /// <param name="types"></param>
        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            AuditLogRecord log = AuditLogInterceptor.CreateNew(entity, id, AuditLogType.Delete, m_session);
            m_logRecords.Add(log);

            base.OnDelete(entity, id, state, propertyNames, types);
        }

        /// <summary>
        /// PostFlush
        /// </summary>
        /// <param name="entities"></param>
        public override void PostFlush(System.Collections.ICollection entities)
        {
            try
            {
                if (m_logRecords.Count > 0)
                {
                    for (int i = 0; i < m_logRecords.Count; ++i)
                    {
                        m_session.Save(m_logRecords[i]);
                    }
                }
            }
            catch (HibernateException ex)
            {
                throw new CallbackException(ex);
            }
            finally
            {
                m_logRecords.Clear();
            }
        }
    }
}