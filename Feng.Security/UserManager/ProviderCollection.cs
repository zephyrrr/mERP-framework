using System;
using System.Configuration.Provider;

namespace Feng.UserManager
{
    /// <summary>
    /// ProviderCollection
    /// </summary>
    public class ProviderCollection : System.Configuration.Provider.ProviderCollection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public override void Add(System.Configuration.Provider.ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (!(provider is UserManager.ProviderBase))
            {
                throw new ArgumentException("The provider parameter must be of type UserManager.ProviderBase.", "provider");
            }

            base.Add(provider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new ProviderBase this[string name]
        {
            get { return (ProviderBase) base[name]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(ProviderBase[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}