using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 包含事件出发的Dao。事件包含<see cref="TransactionBeginning"/>、<see cref="EntityOperating"/>、
    /// <see cref="EntityOperated"/>、<see cref="TransactionCommited"/>、<see cref="TransactionRollbacked"/>、
    /// </summary>
    public interface IEventDao : IRelationalDao
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs> TransactionBeginning;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs> TransactionBegun;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs> EntityOperating;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs> EntityOperated;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs> TransactionCommitting;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs> TransactionCommited;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs> TransactionRollbacking;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs> TransactionRollbacked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnEntityOperating(OperateArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnEntityOperated(OperateArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionBeginning(OperateArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionBegun(OperateArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionCommitting(OperateArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionCommited(OperateArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionRollbacking(OperateArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionRollbacked(OperateArgs e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventDao<T> : IRelationalDao
         where T : class, IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionBeginning(OperateArgs<T> e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionBegun(OperateArgs<T> e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnEntityOperating(OperateArgs<T> e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnEntityOperated(OperateArgs<T> e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionCommitting(OperateArgs<T> e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionCommited(OperateArgs<T> e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionRollbacking(OperateArgs<T> e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTransactionRollbacked(OperateArgs<T> e);


        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs<T>> TransactionBeginning;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs<T>> TransactionBegun;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs<T>> EntityOperating;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs<T>> EntityOperated;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs<T>> TransactionCommitting;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs<T>> TransactionCommited;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs<T>> TransactionRollbacking;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<OperateArgs<T>> TransactionRollbacked;
    }
}
