using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 审计日志属性
    /// 用于实体类IEntity接口，可对IEntity的Save、Update、Delete保存操作记录
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AuditableAttribute : Attribute
    {
    }
}
