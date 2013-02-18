using System;
using System.Reflection;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Type;
using NHibernate.Criterion;
using System.Configuration;
using System.Collections.Generic;


namespace Feng.NH
{
    public class StatelessRepository : INHibernateStatelessRepository, IDisposable
    {
        //public IStatelessSession OpenStatelessSession(string sessionFactoryConfigName)
        //{
        //    ISessionFactory sessionFactory = NHibernateSessionFactoryManager.Instance.GetSessionFactory(sessionFactoryConfigName);

        //    IStatelessSession statelesssession = sessionFactory.OpenStatelessSession();

        //    return statelesssession;
        //}

        #region Public Properties
        private IStatelessSession _session;
        //private ITransaction _transaction;
        private bool _isSessionCreator;

        /// <summary>
        /// Gets the NHibernate session being used for this instance of the Repository.
        /// </summary>
        public IStatelessSession Session
        {
            get { return _session; }
        }

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
        /// <summary>
        /// Constructor
        /// </summary>
        public StatelessRepository()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <remarks>This automatically opens an NHibernate session.</remarks>
        public StatelessRepository(string sessionFactoryConfigName)
        {
            _isSessionCreator = true;
            Open(sessionFactoryConfigName);
        }


        #endregion

        #region "IReporitory"
        /// <summary>
        /// Opens the gateway to the data store.
        /// </summary>
        /// <param name="sessionFactoryConfigName"></param>
        /// <returns>True if gateway was already open.</returns>
        /// <remarks>Gateway is open when session is open and connection is open.</remarks>
        private bool Open(string sessionFactoryConfigName)
        {
            ISessionFactory sessionFactory = NHibernateHelper.GetSessionFactoryManager().GetSessionFactory(sessionFactoryConfigName);

            if (!_isSessionCreator)
            {
                throw new InvalidOperationException(
                    "A gateway that is sharing the session of another gateway cannot be opened directly.");
            }

            bool wasOpen = true;

            if (_session == null)
            {
                _session = sessionFactory.OpenStatelessSession();
            }

            return wasOpen;
        }

        ///// <summary>
        ///// Closes the gateway to the data store.
        ///// </summary>
        ///// <remarks>This method disconnects the session and closes it.</remarks>
        //private void Close()
        //{
        //    if (!_isSessionCreator)
        //    {
        //        throw new InvalidOperationException(
        //            "A gateway that is sharing the session of another gateway cannot be closed directly.");
        //    }

        //    if (_session == null)
        //    {
        //        return;
        //    }
        //    //if (_transaction != null)
        //    //{
        //    //    RollbackTransaction();
        //    //}

        //    //if (_session.IsConnected)
        //    //{
        //    //    _session.Disconnect();
        //    //}

        //    //if (_session.IsOpen)
        //    //{
        //        _session.Close();
        //    //}
        //}

        ///// <summary>
        ///// Saves the given entity in the data store.
        ///// </summary>
        ///// <param name="entity">The entity to save.</param>
        ///// /// <remarks>The entity is not actually saved until the transaction is committed.</remarks>
        //public void Save(object entity)
        //{
        //    if (!this.IsOpen)
        //    {
        //        throw new InvalidOperationException("Repository must be open before an entity can be saved.");
        //    }

        //    if (_transaction == null)
        //    {
        //        throw new InvalidOperationException("Saves must be done within a transaction.");
        //    }

        //    _session.Save(entity);
        //}

        ///// <summary>
        ///// Updates the given entity in the data store.
        ///// </summary>
        ///// <param name="entity">The entity to update.</param>
        ///// /// <remarks>The entity is not actually updated until the transaction is committed.</remarks>
        //public void Update(object entity)
        //{
        //    if (!this.IsOpen)
        //    {
        //        throw new InvalidOperationException("Repository must be open before an entity can be updated.");
        //    }

        //    if (_transaction == null)
        //    {
        //        throw new InvalidOperationException("Updates must be done within a transaction.");
        //    }

        //    _session.Update(entity);
        //}

        ///// <summary>
        ///// Deletes an entity from the data store.
        ///// </summary>
        ///// <param name="entity">The entity to delete.</param>
        //public void Delete(object entity)
        //{
        //    if (!this.IsOpen)
        //    {
        //        throw new InvalidOperationException("Repository must be open before an entity can be deleted.");
        //    }

        //    if (_transaction == null)
        //    {
        //        throw new InvalidOperationException("Deletes must be done within a transaction.");
        //    }

        //    _session.Delete(entity);
        //}

        ///// <summary>
        ///// Begins an NHibernate transaction.
        ///// </summary>
        ///// <param name="isolationLevel"></param>
        ///// <returns></returns>
        //public void BeginTransaction(System.Data.IsolationLevel isolationLevel)
        //{
        //    if (!this.IsOpen)
        //    {
        //        throw new InvalidOperationException(
        //            "Repository must be open before a transaction can be started on the gateway.");
        //    }

        //    if (_transaction == null)
        //    {
        //        _transaction = _session.BeginTransaction(isolationLevel);
        //    }

        //    //return _transaction;
        //}

        ///// <summary>
        ///// Begins an NHibernate transaction.
        ///// </summary>
        //public void BeginTransaction()
        //{
        //    //if (!_isSessionCreator)
        //    //{
        //    //    throw new InvalidOperationException(
        //    //        "A gateway that is sharing the session of another gateway cannot start a transaction on the shared session.");
        //    //}

        //    if (!this.IsOpen)
        //    {
        //        throw new InvalidOperationException(
        //            "Repository must be open before a transaction can be started on the gateway.");
        //    }

        //    if (_transaction == null)
        //    {
        //        _transaction = _session.BeginTransaction();
        //    }

        //    //return _transaction;
        //}

        ///// <summary>
        ///// Commits an NHibernate transaction.
        ///// </summary>
        //public void CommitTransaction()
        //{
        //    //if (!_isSessionCreator)
        //    //{
        //    //    throw new InvalidOperationException(
        //    //        "A gateway that is sharing the session of another gateway cannot commit a transaction on the shared session.");
        //    //}

        //    if (!this.IsOpen)
        //    {
        //        throw new InvalidOperationException("Repository must be open before a transaction can be committed.");
        //    }

        //    if (_transaction == null)
        //    {
        //        throw new InvalidOperationException("Repository must have an open transaction in order to commit.");
        //    }

        //    // Do the commit then kill the transaction
        //    _transaction.Commit();
        //    _transaction = null;
        //}

        ///// <summary>
        ///// Rolls back an NHibernate transaction.
        ///// </summary>
        //public void RollbackTransaction()
        //{
        //    //if (!_isSessionCreator)
        //    //{
        //    //    throw new InvalidOperationException(
        //    //        "A gateway that is sharing the session of another gateway cannot roll back a transaction on the shared session.");
        //    //}

        //    if (!this.IsOpen)
        //    {
        //        throw new InvalidOperationException("Repository must be open before a transaction can be rolled back.");
        //    }

        //    if (_transaction != null)
        //    {
        //        // Do the rollback then kill the transaction
        //        try
        //        {
        //            _transaction.Rollback();
        //        }
        //        catch (Exception ex)
        //        {
        //            // no more method.
        //            ExceptionProcess.ProcessWithResume(ex);
        //        }
        //        _transaction = null;
        //    }
        //}

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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IList<T> List<T>(string queryString, Dictionary<string, object> parameters = null)
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
                foreach(var kvp in parameters)
                {
                    if (SimpleRepository.ProcessQueryParameters(kvp, query))
                        continue;

                    System.Collections.IEnumerable b = kvp.Value as System.Collections.IEnumerable;
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

        #endregion

        #region Private Methods

        private static bool GetIsOpen(IStatelessSession session)
        {
            if (session == null || session.Connection.State == System.Data.ConnectionState.Closed)
            {
                return false;
            }

            // Otherwise
            return true;
        }

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor for the Repository class.
        /// </summary>
        ~StatelessRepository()
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
                    //this.Close();

                    //if (_transaction != null)
                    //{
                    //    _transaction.Dispose();
                    //    _transaction = null;
                    //}

                    if (_session != null)
                    {
                        _session.Dispose();
                        _session = null;
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

        #endregion
    }
}
