using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// IRepositoryConsumer类型的AbstractSearchManagerControls
    /// </summary>
    public abstract class AbstractSearchManager<T> : Feng.AbstractSearchManager, IRepositoryConsumer
        where T : IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repCfgName"></param>
        public AbstractSearchManager(string repCfgName)
            : base()
        {
            m_repCfgName = repCfgName;
        }

        private string m_repCfgName;
        /// <summary>
        /// Repository配置名。可为空，空时采用默认名。
        /// </summary>
        public string RepositoryCfgName
        {
            get
            {
                if (string.IsNullOrEmpty(m_repCfgName))
                {
                    return Feng.Utils.RepositoryHelper.GetConfigNameFromType(typeof(T));
                }
                else
                {
                    return m_repCfgName;
                }
            }
            set
            {
                m_repCfgName = value;
            }
        }

        /// <summary>
        /// 读入Entity Schema，使Grid不读入数据时就能显示框架
        /// </summary>
        public override IEnumerable GetSchema()
        {
            return new List<T>();
        }
    }
}
