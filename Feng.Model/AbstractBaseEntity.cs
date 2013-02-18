using System;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class AbstractBaseEntity<T> : IEntity, IIdEntity<T>, IConvertible, IComparable, ICloneable
        where T : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract T Identity
        {
            get;
        }

        #region "ICloneable"
        /// <summary>
        /// Clone
        /// 目前只是MemberwiseClone，对于引用类型，只是简单的拷贝引用
        /// 对于Nullable，也是拷贝值。
        /// </summary>
        /// <returns>克隆的新Entity</returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();

            //MemoryStream memoryStream = new MemoryStream();
            //BinaryFormatter binaryFormatter = new BinaryFormatter();

            //binaryFormatter.Serialize(memoryStream, this);
            //memoryStream.Position = 0;
            //return (T)binaryFormatter.Deserialize(memoryStream);
        }
        #endregion

        #region IGenericEntity<TIdentity> Members

        /// <summary>
        /// Compare equality trough Id
        /// </summary>
        /// <param name="other">Entity to compare.</param>
        /// <returns>true is are equals</returns>
        /// <remarks>
        /// Two entities are equals if they are of the same hierarcy tree/sub-tree
        /// and has same id.
        /// </remarks>
        public virtual bool Equals(IIdEntity<T> other)
        {
            if (null == other || !GetType().IsInstanceOfType(other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            bool otherIsTransient = Equals(other.Identity, default(T));
            bool thisIsTransient = IsTransient();
            if (otherIsTransient && thisIsTransient)
            {
                return ReferenceEquals(other, this);
            }

            return other.Identity.Equals(Identity);
        }

        /// <summary>
        /// IsTransient
        /// </summary>
        /// <returns>如是Transient Entity（刚创建为保存入数据库），返回true，否则返回false</returns>
        protected bool IsTransient()
        {
            return Equals(Identity, default(T));
        }

        /// <summary>
        /// Equals
        /// sealed??
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>如相等，返回true，否则返回false</returns>
        public sealed override bool Equals(object obj)
        {
            var that = obj as IIdEntity<T>;
            return this.Equals(that);
        }

        /// <summary>
        /// GetHashCode
        /// sealed??
        /// </summary>
        /// <returns>当前Entity的哈希代码</returns>
        public sealed override int GetHashCode()
        {
            return this.Identity.GetHashCode();
        }

        /// <summary>
        /// sealed??
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString()
        {
            if (this.Identity != null)
                return this.Identity.ToString();
            else
                return "null";// typeof(T).ToString();
        }
        #endregion

        #region "IConvertible"
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            try
            {
                IEntityBuffer eb = EntityBufferCollection.Instance[this.GetType()];
                if (eb != null)
                {
                    return eb.Get(this.Identity);
                }
                else
                {
                    using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository(this.GetType()))
                    {
                        rep.BeginTransaction();
                        var r = rep.Get(this.GetType(), this.Identity);
                        rep.CommitTransaction();
                        return r;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            try
            {
                return Convert.ToBoolean(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            try
            {
                return Convert.ToByte(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            try
            {
                return Convert.ToChar(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            try
            {
                return Convert.ToDateTime(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            try
            {
                return Convert.ToDecimal(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            try
            {
                return Convert.ToDouble(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            try
            {
                return Convert.ToInt16(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            try
            {
                return Convert.ToInt32(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            try
            {
                return Convert.ToInt64(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            try
            {
                return Convert.ToSByte(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            try
            {
                return Convert.ToSingle(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        string IConvertible.ToString(IFormatProvider provider)
        {
            try
            {
                return Convert.ToString(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            try
            {
                return Convert.ToUInt16(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            try
            {
                return Convert.ToUInt32(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            try
            {
                return Convert.ToUInt64(this.Identity);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Convert is invalid!", ex);
            }
        }
        #endregion

        #region "IComparable"
        int IComparable.CompareTo(object obj)
        {
            if (this == null && obj == null)
            {
                return 0;
            }
            if (this == null)
            {
                return -1;
            }
            if (obj == null)
            {
                return 1;
            }
            AbstractBaseEntity<T> that = obj as AbstractBaseEntity<T>;
            if (that == null)
            {
                throw new ArgumentException("obj must be BaseAbstractEntity<T>!");
            }
            if (this.Identity == null && that.Identity == null)
            {
                return 0;
            }
            if (this.Identity == null)
            {
                return -1;
            }
            return this.Identity.CompareTo(that.Identity);
        }
        #endregion
    }
}
