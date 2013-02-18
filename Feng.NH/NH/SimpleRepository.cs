using System;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Impl;
using NHibernate.Engine;
using NHibernate.Cfg;
using NHibernate.Type;
using NHibernate.Criterion;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;


namespace Feng.NH
{
    /// <summary>
    /// This class provides methods for retrieving and managing data through NHibernate
    /// sessions and transactions.
    /// Session Close 和 Disconnect 区别： Close释放资源，不能Reconnect，这个Session不能再重用。
    /// </summary>
    public class SimpleRepository : INHibernateRepository, IDisposable
    {
        #region Public Properties
        private ISession _session;
        private ITransaction _transaction;
        private IInterceptor _interceptor;
        private bool _isSessionCreator;

        /// <summary>
        /// Gets the NHibernate session being used for this instance of the Repository.
        /// </summary>
        public ISession Session
        {
            get { return _session; }
        }

        ///// <summary>
        ///// Gets the NHibernate Transaction being used for this instance of the Repository.
        ///// </summary>
        //public ITransaction Transaction
        //{
        //    get { return _transaction; }
        //}

        /// <summary>
        /// Gets a value indicating whether or not this instance of the Repository is the
        /// creator of the NHibernate session.
        /// </summary>
        public bool IsSessionCreator
        {
            get { return _isSessionCreator; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the NHibernate session is open.
        /// </summary>
        public bool IsOpen
        {
            get { return GetIsOpen(_session); }
        }

        /// <summary>
        /// Tag
        /// </summary>
        public object Tag 
        { 
            get; 
            set; 
        }
        #endregion

        #region Constructors
        private static IInterceptor CreateInterceptor()
        {
            var i = ServiceProvider.GetService<IInterceptor>();
            if (i == null)
                return i;
            return Utils.ReflectionHelper.CreateInstanceFromType(i.GetType()) as IInterceptor;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleRepository()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <remarks>This automatically opens an NHibernate session.</remarks>
        public SimpleRepository(string sessionFactoryConfigName)
            : this(sessionFactoryConfigName, CreateInterceptor())
        {
        }

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <remarks>This automatically opens an NHibernate session.</remarks>
        public SimpleRepository(string sessionFactoryConfigName, IInterceptor interceptor)
        {
            _isSessionCreator = true;
            Open(sessionFactoryConfigName, interceptor);
        }

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <param name="session">An existing NHibernate session that will be used
        /// by this instance of the Repository.</param>
        public SimpleRepository(ISession session)
        {
            if (!GetIsOpen(session))
            {
                throw new InvalidOperationException("A new Repository can only be created with an open session.");
            }

            _session = session;
        }

        #endregion

        #region "IReporitory"
        /// <summary>
        /// 
        /// </summary>
        public bool IsSupportTransaction
        {
            get { return true; }
        }
        /// <summary>
        /// Opens the gateway to the data store.
        /// </summary>
        /// <param name="sessionFactoryConfigName"></param>
        /// <param name="interceptor"></param>
        /// <returns>True if gateway was already open.</returns>
        /// <remarks>Gateway is open when session is open and connection is open.</remarks>
        private bool Open(string sessionFactoryConfigName, IInterceptor interceptor)
        {
            ISessionFactory sessionFactory = null;
            //if (string.IsNullOrEmpty(sessionFactoryConfigName))
            //{
            //    //sessionFactory = NHibernateSessionFactoryManager.Instance.GetDefaultSessionFactory();
            //    throw new ArgumentNullException("sessionFactoryConfigName");
            //}
            //else
            {
                sessionFactory = NHibernateHelper.GetSessionFactoryManager().GetSessionFactory(sessionFactoryConfigName);
            }

            _interceptor = interceptor;

            if (!_isSessionCreator)
            {
                throw new InvalidOperationException(
                    "A gateway that is sharing the session of another gateway cannot be opened directly.");
            }

            bool wasOpen = true;

            if (_session == null || !_session.IsOpen)
            {
                wasOpen = false;

                if (_interceptor != null)
                {
                    _session = sessionFactory.OpenSession(_interceptor);
                }
                else
                {
                    _session = sessionFactory.OpenSession();
                }
            }

            // Session is open, connection could be either open or closed. If connection
            // is closed, open it. Indicate that session was open.
            if (!_session.IsConnected)
            {
                wasOpen = false;
                _session.Reconnect();
            }

            // call in OpenSession(_intercept) auto
            //if (_interceptor != null)
            //{
            //    _interceptor.SetSession(_session);
            //}

            return wasOpen;
        }

        /// <summary>
        /// Closes the gateway to the data store.
        /// </summary>
        /// <remarks>This method disconnects the session and closes it.</remarks>
        private void Close()
        {
            if (!_isSessionCreator)
            {
                throw new InvalidOperationException(
                    "A gateway that is sharing the session of another gateway cannot be closed directly.");
            }

            if (_session == null)
            {
                return;
            }
            if (_transaction != null)
            {
                RollbackTransaction();
            }

            if (_session.IsConnected)
            {
                _session.Disconnect();
            }

            if (_session.IsOpen)
            {
                _session.Close();
            }
        }

        /// <summary>
        /// Lock
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lockMode"></param>
        public void Lock(object entity, NHibernate.LockMode lockMode)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before an entity can be saved or updated.");
            }

            _session.Lock(entity, lockMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void Refresh(object obj)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before an entity can be saved or updated.");
            }
            _session.Refresh(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void Attach(object obj)
        {
            if (obj == null)
                return;
            try
            {
                if (_session.HasProxy(obj))
                {
                    this.Lock(obj, LockMode.None);

                    //EntityEntry oldEntry;
                    //IEntityPersister persister = _session.GetEntityPersister(obj, out oldEntry);
                    //Object[] currentState = persister.GetPropertyValues(obj, _session.GetSessionImplementation().EntityMode);
                    //foreach (object i in currentState)
                    //{
                    //    Attach(i);
                    //}
                }
            }
            catch (NHibernate.HibernateException ex)
            {
                // reassociated object has dirty collection reference (or an array)
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        /// <summary>
        /// Saves or updates the given entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to save or update.</param>
        /// <remarks>The entity is not actually saved or updated until the transaction is committed.</remarks>
        public virtual void SaveOrUpdate(object entity)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before an entity can be saved or updated.");
            }

            if (_transaction == null)
            {
                throw new InvalidOperationException("Saves or updates must be done within a transaction.");
            }

            _session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Saves the given entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// /// <remarks>The entity is not actually saved until the transaction is committed.</remarks>
        public virtual void Save(object entity)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before an entity can be saved.");
            }

            if (_transaction == null)
            {
                throw new InvalidOperationException("Saves must be done within a transaction.");
            }

            _session.Save(entity);
        }

        /// <summary>
        /// Updates the given entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// /// <remarks>The entity is not actually updated until the transaction is committed.</remarks>
        public virtual void Update(object entity)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before an entity can be updated.");
            }

            if (_transaction == null)
            {
                throw new InvalidOperationException("Updates must be done within a transaction.");
            }

            _session.Update(entity);
        }

        /// <summary>
        /// Deletes an entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(object entity)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before an entity can be deleted.");
            }

            if (_transaction == null)
            {
                throw new InvalidOperationException("Deletes must be done within a transaction.");
            }

            _session.Delete(entity);
        }

        

        /// <summary>
        /// Begins an NHibernate transaction.
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public virtual void BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException(
                    "Repository must be open before a transaction can be started on the gateway.");
            }

            if (_transaction == null)
            {
                _transaction = _session.BeginTransaction(isolationLevel);
            }
            //return _transaction;
        }

        /// <summary>
        /// Begins an NHibernate transaction.
        /// </summary>
        public virtual void BeginTransaction()
        {
            //if (!_isSessionCreator)
            //{
            //    throw new InvalidOperationException(
            //        "A gateway that is sharing the session of another gateway cannot start a transaction on the shared session.");
            //}

            if (!this.IsOpen)
            {
                throw new InvalidOperationException(
                    "Repository must be open before a transaction can be started on the gateway.");
            }

            if (_transaction == null)
            {
                _transaction = _session.BeginTransaction();
            }

            //return _transaction;
        }

        /// <summary>
        /// Commits an NHibernate transaction.
        /// </summary>
        public virtual void CommitTransaction()
        {
            //if (!_isSessionCreator)
            //{
            //    throw new InvalidOperationException(
            //        "A gateway that is sharing the session of another gateway cannot commit a transaction on the shared session.");
            //}

            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before a transaction can be committed.");
            }

            if (_transaction == null)
            {
                throw new InvalidOperationException("Repository must have an open transaction in order to commit.");
            }

            // Do the commit then kill the transaction
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// Rolls back an NHibernate transaction.
        /// </summary>
        public virtual void RollbackTransaction()
        {
            //if (!_isSessionCreator)
            //{
            //    throw new InvalidOperationException(
            //        "A gateway that is sharing the session of another gateway cannot roll back a transaction on the shared session.");
            //}

            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before a transaction can be rolled back.");
            }

            if (_transaction != null)
            {
                // Do the rollback then kill the transaction
                try
                {
                    _transaction.Rollback();
                }
                catch (Exception ex)
                {
                    // no more method.
                    ExceptionProcess.ProcessWithResume(ex);
                }
                _transaction = null;
            }
        }

        /// <summary>
        /// 根据Id查找实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(object id)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before retrive data.");
            }
            return _session.Get<T>(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(Type type, object id)
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before retrive data.");
            }
            return _session.Get(type, id);
        }

        /// <summary>
        /// 得到全部列列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> List<T>()
            where T : class, new()
        {
            return _session.CreateCriteria<T>().List<T>();
        }

        /// <summary>
        /// 得到全部列表
        /// </summary>
        /// <param name="persistantClass"></param>
        /// <returns></returns>
        public System.Collections.IList List(Type persistantClass)
        {
            return _session.CreateCriteria(persistantClass).List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="detachedCriteria"></param>
        /// <returns></returns>
        public IList<T> List<T>(NHibernate.Criterion.DetachedCriteria detachedCriteria)
            where T : class, new()
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before retrive data.");
            }
            if (detachedCriteria == null)
            {
                throw new ArgumentNullException("detachedCriteria");
            }
            return detachedCriteria.GetExecutableCriteria(_session).List<T>();
        }

        internal static bool ProcessQueryParameters(KeyValuePair<string, object> kvp, IQuery query)
        {
            switch(kvp.Key)
            {
                case "MaxResults":
                    query.SetMaxResults((int)kvp.Value);
                    return true;
                case "FirstResult":
                    query.SetFirstResult((int)kvp.Value);
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IList<T> List<T>(string queryString = null, Dictionary<string, object> parameters = null)
            where T : class, new()
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before retrive data.");
            }
            if (string.IsNullOrEmpty(queryString))
            {
                return List<T>();
            }
            
            IQuery query = _session.CreateQuery(queryString);
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    if (ProcessQueryParameters(kvp, query))
                        continue;
                    
                    System.Collections.ICollection b = kvp.Value as System.Collections.ICollection;
                    if (b != null)
                    {
                        query.SetParameterList(kvp.Key, b);
                    }
                    else
                    {
                        query.SetParameter(kvp.Key, kvp.Value);
                    }
                }
            }
            return query.List<T>();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="queryString"></param>
        ///// <param name="parameters"></param>
        ///// <returns></returns>
        //public IList<T> List<T>(string queryString, params object[] parameters)
        //    where T : class, new()
        //{
        //    if (!this.IsOpen)
        //    {
        //        throw new InvalidOperationException("Repository must be open before retrive data.");
        //    }
        //    if (string.IsNullOrEmpty(queryString))
        //    {
        //        return List<T>();
        //    }
        //    IQuery query = _session.CreateQuery(queryString);
        //    for (int i = 0; i < parameters.Length; ++i)
        //    {
        //        query.SetParameter(i + 1, parameters[i]);
        //    }
        //    return query.List<T>();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="detachedCriteria"></param>
        /// <returns></returns>
        public T UniqueResult<T>(NHibernate.Criterion.DetachedCriteria detachedCriteria)
            where T : class, new()
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Repository must be open before retrive data.");
            }
            if (detachedCriteria == null)
            {
                throw new ArgumentNullException("detachedCriteria");
            }
            return detachedCriteria.GetExecutableCriteria(_session).UniqueResult<T>();
        }

        ///// <summary>
        ///// 根据属性查找实体
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="property"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public T GetByProperty<T>(string property, object value)
        //{
        //    using (var tx = this.BeginTransaction())
        //    {
        //        StringBuilder hql = new StringBuilder();
        //        hql.Append(string.Format("FROM {0} a ", typeof(T).FullName));
        //        hql.Append(string.Format("WHERE a.{0} = ?", property));

        //        object obj = this.Session.CreateQuery(hql.ToString())
        //            .SetParameter(0, value)
        //            .UniqueResult();

        //        this.CommitTransaction();
        //        return (T)obj;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public bool IsProxy(object proxy)
        {
            return NHibernateHelper.IsProxy(proxy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="owner"></param>
        public void Initialize(object proxy, object owner)
        {
            NHibernateHelper.Initialize(proxy, owner, this.Session);
        }
        #endregion

        #region Private Methods

        private static bool GetIsOpen(ISession session)
        {
            if (session == null || !session.IsOpen)
            {
                return false;
            }

            // Session is open, connection could be either open or closed.
            // If connection is closed, signal that session is closed.
            if (!session.IsConnected)
            {
                return false;
            }

            // Otherwise
            return true;
        }

        //private static Collection<T> ToGenericCollection<T>(IList<T> list)
        //{
        //    Collection<T> coll = new Collection<T>();

        //    if (list != null && list.Count > 0)
        //    {
        //        foreach (T t in list)
        //        {
        //            coll.Add(t);
        //        }
        //    }

        //    return coll;
        //}

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor for the Repository class.
        /// </summary>
        ~SimpleRepository()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Disposes of this instance of the Repository class.
        /// </summary>
        /// <param name="isDisposing">Flag that tells the .NET runtime if this class is disposing.</param>
        /// <remarks>This method kills the existing NHibernate session, transaction, and interceptor.</remarks>
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_isSessionCreator)
                {
                    this.Close();

                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_session != null)
                    {
                        _session.Dispose();
                        _session = null;
                    }

                    if (_interceptor != null)
                    {
                        _interceptor.SetSession(null);
                        _interceptor = null;
                    }
                }
            }
        }

        /// <summary>
        /// Disposes of this instance of the Repository class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        public void Disconnect()
        {
            if (_isSessionCreator && _session.IsConnected)
            {
                _session.Disconnect();
            }
        }

        #endregion
    }
}