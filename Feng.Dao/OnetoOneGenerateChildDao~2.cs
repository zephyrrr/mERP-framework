using System;
using System.Collections.Generic;
using System.Text;
using Feng.Utils;

namespace Feng
{
    /// <summary>
    /// OnetoOne关系的Dao
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class OnetoOneGenerateChildDao<T, S>: AbstractRelationalDao<T, S>
        where T : class, IOnetoOneParentGenerateChildEntity<T, S>
        where S : class, IOnetoOneChildEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="childDao"></param>
        public OnetoOneGenerateChildDao(IRepositoryDao<S> childDao)
            : base(childDao)
        {
        }

        /// <summary>
        /// 在Entity操作前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperating(OperateArgs<T> e)
        {
            base.OnEntityOperating(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    break;
                case OperateType.Update:
                    break;
                case OperateType.Delete:
                    throw new ArgumentException("u should not call delete in OnetoOneGenerateChildDao!");
                ////if (e.Entity.Submitted)
                //{
                //    if (e.Entity.ChildEntity != null)
                //    {
                //        m_childDao.Delete(e.Repository, e.Entity.ChildEntity);
                //    }
                //}
                //break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }

        private void GenerateNewChild(T parent)
        {
            parent.ChildEntity = ReflectionHelper.CreateInstanceFromType(parent.ChildType) as S;
            parent.ChildEntity.ParentEntity = parent;

            EntityScript.SetPropertyValue(parent.ChildEntity, ServiceProvider.GetService<IEntityMetadataGenerator>().GenerateEntityMetadata(typeof(S)).IdName,
                EntityScript.GetPropertyValue(parent, ServiceProvider.GetService<IEntityMetadataGenerator>().GenerateEntityMetadata(typeof(T)).IdName));
        }

        /// <summary>
        /// 在Entity操作后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperated(OperateArgs<T> e)
        {
            base.OnEntityOperated(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    throw new ArgumentException("u should not call Save in OnetoOneGenerateChildDao!");
                    ////if (e.Entity.Submitted)
                    //{
                    //    if (e.Entity.ChildType != null)
                    //    {
                    //        GenerateNewChild(e.Entity);
                    //        m_childDao.Save(e.Repository, e.Entity.ChildEntity);
                    //    }
                    //}
                    // break;
                case OperateType.Update:
                    //if (e.Entity.Submitted)
                    {
                        if (e.Entity.ChildType == null && e.Entity.ChildEntity == null)
                        {
                        }
                        else if (e.Entity.ChildType != null && e.Entity.ChildEntity == null)
                        {
                            GenerateNewChild(e.Entity);
                            this.DetailDao.Save(e.Repository, e.Entity.ChildEntity);
                        }
                        else if (e.Entity.ChildType == null && e.Entity.ChildEntity != null)
                        {
                            this.DetailDao.Delete(e.Repository, e.Entity.ChildEntity);
                            e.Entity.ChildEntity = null;
                        }
                        else if (e.Entity.ChildType != e.Entity.ChildEntity.GetType()
                            && e.Entity.ChildType != e.Entity.ChildEntity.GetType().BaseType)   // maybe Proxy
                        {
                            this.DetailDao.Delete(e.Repository, e.Entity.ChildEntity);

                            GenerateNewChild(e.Entity);
                            this.DetailDao.Save(e.Repository, e.Entity.ChildEntity);
                        }
                        else
                        {
                            this.DetailDao.Update(e.Repository, e.Entity.ChildEntity);
                        }
                    }
                    break;
                case OperateType.Delete:
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }
    }
}
