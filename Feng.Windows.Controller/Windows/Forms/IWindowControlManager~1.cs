using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWindowControlManager<T> : IWindowControlManager, IControlManager<T>
        where T : class, IEntity, new()
    {
    }
}