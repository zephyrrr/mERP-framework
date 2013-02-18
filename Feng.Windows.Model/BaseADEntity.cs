using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 基本实体类（可Clone）
    /// </summary>
    //[AttributeIdentifier("Id.Generator", Value = "assigned")]
    //[AttributeIdentifier("Id.Column", Value = "Name")]
    [Serializable]
    public abstract class BaseADEntity : BaseEntity<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override string Identity
        {
            get { return this.Name; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Id(0, Name = "Name", Column = "Name", Length = 255)]
        [Generator(1, Class = "assigned")]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 简单帮助
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string Help
        {
            get;
            set;
        }

        /// <summary>
        /// 详细帮助
        /// </summary>
        [Property(Length = 2000, NotNull = false)]
        public virtual string Description
        {
            get;
            set;
        }

        
    }
}
