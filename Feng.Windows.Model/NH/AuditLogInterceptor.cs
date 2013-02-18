using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NHibernate;
using NHibernate.Type;

namespace Feng.NH
{
    /// <summary>
    /// 审核日志类型
    /// </summary>
    public enum AuditLogType
    {
        /// <summary>
        /// 添加
        /// </summary>
        Save,
        /// <summary>
        /// 更新
        /// </summary>
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 开始操作
        /// </summary>
        TransactionCompletion,
        /// <summary>
        /// 结束操作
        /// </summary>
        TransactionBegin,
        /// <summary>
        /// 特定
        /// </summary>
        Custom
    }

    /// <summary>
    /// 审核日志Interceptor
    /// </summary>
    public class AuditLogInterceptor : NHibernate.EmptyInterceptor
    {
        //private static string GetEntityInfos(object entity)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Type type = entity.GetType();
        //    PropertyInfo[] properties = type.GetProperties();
        //    foreach (PropertyInfo p in properties)
        //    {
        //        sb.Append(p.Name);
        //        sb.Append("=");
        //        object v = EntityHelper.GetPropertyValue(entity, p.Name);
        //        sb.Append(v == null ? "null" : v.ToString());
        //        sb.Append(System.Environment.NewLine);
        //    }
        //    return sb.ToString();
        //}

        private ISession m_session;

        private List<AuditLogRecord> m_logRecords = new List<AuditLogRecord>();

        private AuditLogRecord CreateNew(object entity, object id, AuditLogType logType)
        {
            return CreateNew(entity, id, logType, m_session);
        }
        internal static AuditLogRecord CreateNew(object entity, object id, AuditLogType logType, ISession session)
        {
            AuditLogRecord log = new AuditLogRecord();
            log.EntityId = id == null ? string.Empty : id.ToString();
            log.EntityName = entity == null ? string.Empty : session.GetEntityName(entity);
            log.LogType = logType.ToString();
            log.Created = DateTime.Now;
            log.CreatedBy = SystemConfiguration.UserName;
            log.ClientId = SystemConfiguration.ClientId;
            log.OrgId = SystemConfiguration.OrgId;
            log.IsActive = true;
            return log;
        }

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

        private Dictionary<Type, bool> m_auditable = new Dictionary<Type, bool>();
        private bool IsAuditable(object entity)
        {
            Type type = entity.GetType();
            if (!m_auditable.ContainsKey(type))
            {
                m_auditable[type] = (entity.GetType().GetCustomAttributes(typeof (AuditableAttribute), false).Length > 0);
            }
            return m_auditable[type];
        }

        private const string m_emptyStateString = "";
        private string GetStateString(object[] state, IType[] types, int idx)
        {
            if (state == null)
            {
                return m_emptyStateString;
            }
            if (idx < state.Length)
            {
                return types[idx].ToLoggableString(state[idx], m_session.SessionFactory as NHibernate.Engine.ISessionFactoryImplementor);
            }
            else
            {
                return m_emptyStateString;
            }
        }
        /// <summary>
        /// OnSave
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <param name="propertyNames"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (IsAuditable(entity))
            {
                AuditLogRecord log = CreateNew(entity, id, AuditLogType.Save);

                StringBuilder sb = new StringBuilder();
                for(int i=0; i<propertyNames.Length; ++i)
                {
                    sb.Append(propertyNames[i]);
                    sb.Append(":");
                    sb.Append(GetStateString(state, types, i));
                    sb.Append(" | ");
                }
                log.Message = sb.ToString().Substring(0, Math.Min(sb.Length, 4000));
                m_logRecords.Add(log);
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        /// <summary>
        /// OnFlushDirty
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="currentState"></param>
        /// <param name="previousState"></param>
        /// <param name="propertyNames"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState,
                                          string[] propertyNames, IType[] types)
        {
            if (IsAuditable(entity))
            {
                AuditLogRecord log = CreateNew(entity, id, AuditLogType.Update);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < propertyNames.Length; ++i)
                {
                    sb.Append(propertyNames[i]);
                    sb.Append(":");
                    sb.Append(GetStateString(previousState, types, i));
                    sb.Append("->");
                    sb.Append(GetStateString(currentState, types, i));
                    sb.Append(" | ");
                }
                log.Message = sb.ToString().Substring(0, Math.Min(sb.Length, 4000));
                m_logRecords.Add(log);
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
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
            if (IsAuditable(entity))
            {
                AuditLogRecord log = CreateNew(entity, id, AuditLogType.Delete);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < propertyNames.Length; ++i)
                {
                    sb.Append(propertyNames[i]);
                    sb.Append(":");
                    sb.Append(GetStateString(state, types, i));
                    sb.Append(" | ");
                }
                log.Message = sb.ToString().Substring(0, Math.Min(sb.Length, 4000));
                m_logRecords.Add(log);
            }

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
                    AuditLogRecord log = CreateNew(null, null, AuditLogType.TransactionBegin);
                    m_session.Save(log);

                    for(int i=0; i<m_logRecords.Count; ++i)
                    {
                        m_session.Save(m_logRecords[i]);
                    }

                    log = CreateNew(null, null, AuditLogType.TransactionCompletion);
                    m_session.Save(log);
                    
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