using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;

namespace Feng.NH
{
//	/// <summary>
//	/// Handles creation and management of sessions and transactions.  It is a singleton because 
//	/// building the initial session factory is very expensive. Inspiration for this class came 
//	/// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
//	/// you can use TypeMock (http://www.typemock.com) for more flexible testing.
//	/// </summary>
//	public sealed class NHibernateSessionManager
//	{
//		#region Thread-safe, lazy Singleton
//
//		/// <summary>
//		/// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
//		/// for more details about its implementation.
//		/// </summary>
//		public static NHibernateSessionManager Instance
//		{
//			get
//			{
//				return Nested.NHibernateSessionManager;
//			}
//		}
//
//		/// <summary>
//		/// Initializes the NHibernate session factory upon instantiation.
//		/// </summary>
//		private NHibernateSessionManager()
//		{
//		}
//
//		/// <summary>
//		/// Assists with ensuring thread-safe, lazy singleton
//		/// </summary>
//		private class Nested
//		{
//			private Nested()
//			{
//			}
//			internal static readonly NHibernateSessionManager NHibernateSessionManager =
//				new NHibernateSessionManager();
//		}
//
//		#endregion
//
//		/// <summary>
//		/// Allows you to register an interceptor on a new session.  This may not be called if there is already
//		/// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
//		/// the HttpModule to call this before calling BeginTransaction().
//		/// </summary>
//		public void RegisterInterceptor(IInterceptor interceptor)
//		{
//			ISession session = ContextSession;
//
//			if (session != null && session.IsOpen)
//			{
//				throw new CacheException("You cannot register an interceptor once a session has already been opened");
//			}
//
//			GetSession(interceptor);
//		}
//
//		/// <summary>
//		/// GetSession
//		/// </summary>
//		/// <returns></returns>
//		public ISession GetSession()
//		{
//			return GetSession(null);
//		}
//
//		/// <summary>
//		/// Gets a session with or without an interceptor.  This method is not called directly; instead,
//		/// it gets invoked from other public methods.
//		/// </summary>
//		private ISession GetSession(IInterceptor interceptor)
//		{
//			ISession session = ContextSession;
//
//			if (session == null)
//			{
//				Repository rep = new Repository(interceptor);
//				ContextSession = rep.Session;
//			}
//
//			//Check.Ensure(session != null, "session was null");
//
//			return session;
//		}
//
//		/// <summary>
//		/// Flushes anything left in the session and closes the connection.
//		/// </summary>
//		public void CloseSession()
//		{
//			ISession session = ContextSession;
//
//			if (session != null && session.IsOpen)
//			{
//				session.Flush();
//				session.Close();
//			}
//
//			ContextSession = null;
//		}
//
//		/// <summary>
//		/// BeginTransaction
//		/// </summary>
//		public void BeginTransaction()
//		{
//			ITransaction transaction = ContextTransaction;
//
//			if (transaction == null)
//			{
//				transaction = GetSession().BeginTransaction();
//				ContextTransaction = transaction;
//			}
//		}
//
//		/// <summary>
//		/// CommitTransaction
//		/// </summary>
//		public void CommitTransaction()
//		{
//			ITransaction transaction = ContextTransaction;
//
//			try
//			{
//				if (HasOpenTransaction())
//				{
//					transaction.Commit();
//					ContextTransaction = null;
//				}
//			}
//			catch (HibernateException)
//			{
//				RollbackTransaction();
//				throw;
//			}
//		}
//
//		/// <summary>
//		/// HasOpenTransaction
//		/// </summary>
//		/// <returns></returns>
//		public bool HasOpenTransaction()
//		{
//			ITransaction transaction = ContextTransaction;
//
//			return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
//		}
//
//		/// <summary>
//		/// RollbackTransaction
//		/// </summary>
//		public void RollbackTransaction()
//		{
//			ITransaction transaction = ContextTransaction;
//
//			try
//			{
//				if (HasOpenTransaction())
//				{
//					transaction.Rollback();
//				}
//
//				ContextTransaction = null;
//			}
//			finally
//			{
//				CloseSession();
//			}
//		}
//
//		/// <summary>
//		/// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
//		/// specific <see cref="CallContext" />.  Discussion concerning this found at 
//		/// http://forum.springframework.net/showthread.php?t=572.
//		/// </summary>
//		private ITransaction ContextTransaction
//		{
//			get
//			{
//				if (IsInWebContext())
//				{
//					return (ITransaction)HttpContext.Current.Items[TRANSACTION_KEY];
//				}
//				else
//				{
//					return (ITransaction)CallContext.GetData(TRANSACTION_KEY);
//				}
//			}
//			set
//			{
//				if (IsInWebContext())
//				{
//					HttpContext.Current.Items[TRANSACTION_KEY] = value;
//				}
//				else
//				{
//					CallContext.SetData(TRANSACTION_KEY, value);
//				}
//			}
//		}
//
//		/// <summary>
//		/// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
//		/// specific <see cref="CallContext" />.  Discussion concerning this found at 
//		/// http://forum.springframework.net/showthread.php?t=572.
//		/// </summary>
//		private ISession ContextSession
//		{
//			get
//			{
//				if (IsInWebContext())
//				{
//					return (ISession)HttpContext.Current.Items[SESSION_KEY];
//				}
//				else
//				{
//					return (ISession)CallContext.GetData(SESSION_KEY);
//				}
//			}
//			set
//			{
//				if (IsInWebContext())
//				{
//					HttpContext.Current.Items[SESSION_KEY] = value;
//				}
//				else
//				{
//					CallContext.SetData(SESSION_KEY, value);
//				}
//			}
//		}
//
//		private static bool IsInWebContext()
//		{
//			return HttpContext.Current != null;
//		}
//
//		private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
//		private const string SESSION_KEY = "CONTEXT_SESSION";
//	}
}