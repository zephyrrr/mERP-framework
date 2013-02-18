using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// IMasterDao
    /// </summary>
    public interface IRelationalDao<T, S> : IRepositoryDao<T>, IEventDao<T>
        where T : class, IEntity
        where S : class, IEntity
    {
        /// <summary>
        /// DetailDao
        /// </summary>
        IRepositoryDao<S> DetailDao
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IMemoriedRelationalDao
    {
        /// <summary>
        /// 
        /// </summary>
        IMemoryDao DetailMemoryDao
        {
            get;
        }

        /// <summary>
        /// 在MemoryDao中加入主从关系
        /// </summary>
        /// <param name="cm"></param>
        void AddRelationToMemoryDao(IEntityList cm);
    }

    /// <summary>
    /// IMasterDao with DetailMemoryDao
    /// </summary>
    public interface IMemoriedRelationalDao<T, S> : IRelationalDao<T, S>
        where T : class, IEntity
        where S : class, IEntity
    {
        /// <summary>
        /// DetailMemoryDao
        /// </summary>
        IMemoryDao<S> DetailMemoryDao
        {
            get;
        }

        /// <summary>
        /// 在MemoryDao中加入主从关系
        /// </summary>
        /// <param name="cm"></param>
        void AddRelationToMemoryDao(IEntityList cm);
    }
}
