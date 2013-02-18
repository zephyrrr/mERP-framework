using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Feng.Utils
{
    /// <summary>
    /// 反射的帮助类
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool ObjectEquals(object c1, object c2)
        {
            if (c1 == null && c2 == null)
            {
                return true;
            }

            if ((c1 == null && c2 != null) || (c1 != null && c2 == null))
            {
                return false;
            }

            Type type = c1.GetType();
            if (type != c2.GetType())
            {
                throw new ArgumentException("Two object's type should be same! now one is " + type + " and the other is " + c2.GetType() + ".");
            }
            MethodInfo m = type.GetMethod("Equals", new Type[] { typeof(object) });

            bool ret = (bool)m.Invoke(c1, new object[] { c2 });

            return ret;
        }

        ///// <summary>
        ///// Clone(现在只复制Property和Field，而且只是
        ///// </summary>
        ///// <param name="src"></param>
        ///// <returns></returns>
        //private static object Clone(object src)
        //{
        //    //First we create an instance of this specific type.
        //    object newObject = Activator.CreateInstance(src.GetType());

        //    // 对C#3.0 get; set; 需要Properties，但。。。。
        //    PropertyInfo[] properties = newObject.GetType().GetProperties();

        //    //We get the array of fields for the new type instance.
        //    FieldInfo[] fields = newObject.GetType().GetFields();
        //    for (int i = 0; i < fields.Length; ++i)
        //    {
        //        //We query if the fiels support the ICloneable interface.
        //        Type ICloneType = fields[i].FieldType.GetInterface("ICloneable", true);

        //        if (ICloneType != null)
        //        {
        //            //Getting the ICloneable interface from the object.
        //            ICloneable IClone = (ICloneable)fields[i].GetValue(src);
        //            //We use the clone method to set the new value to the field.
        //            fields[i].SetValue(newObject, IClone.Clone());
        //        }
        //        else
        //        {
        //            // If the field doesn't support the ICloneable interface then just set it.
        //            fields[i].SetValue(newObject, fields[i].GetValue(src));
        //        }

        //        //Now we check if the object support the IEnumerable interface, so if it does
        //        //we need to enumerate all its items and check if they support the ICloneable interface.
        //        Type IEnumerableType = fields[i].FieldType.GetInterface("IEnumerable", true);
        //        if (IEnumerableType != null)
        //        {
        //            //Get the IEnumerable interface from the field.
        //            IEnumerable IEnum = (IEnumerable)fields[i].GetValue(src);

        //            //This version support the IList and the IDictionary interfaces to iterate on collections.
        //            Type IListType = fields[i].FieldType.GetInterface("IList", true);
        //            Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);

        //            int j = 0;
        //            if (IListType != null)
        //            {
        //                //Getting the IList interface.
        //                IList list = (IList)fields[i].GetValue(newObject);
        //                foreach (object obj in IEnum)
        //                {
        //                    //Checking to see if the current item support the ICloneable interface.
        //                    ICloneType = obj.GetType().GetInterface("ICloneable", true);

        //                    if (ICloneType != null)
        //                    {
        //                        //If it does support the ICloneable interface, we use it to set the clone of the object in the list.
        //                        ICloneable clone = (ICloneable)obj;
        //                        list[j] = clone.Clone();
        //                    }

        //                    //NOTE: If the item in the list is not support the ICloneable interface then in the cloned list this item will be the same item as in the original list(as long as this type is a reference type).
        //                    j++;
        //                }
        //            }
        //            else if (IDicType != null)
        //            {
        //                //Getting the dictionary interface.
        //                IDictionary dic = (IDictionary)fields[i].GetValue(newObject);
        //                j = 0;
        //                foreach (DictionaryEntry de in IEnum)
        //                {
        //                    //Checking to see if the item support the ICloneable interface.
        //                    ICloneType = de.Value.GetType().GetInterface("ICloneable", true);
        //                    if (ICloneType != null)
        //                    {
        //                        ICloneable clone = (ICloneable)de.Value;
        //                        dic[de.Key] = clone.Clone();
        //                    }
        //                    j++;
        //                }
        //            }
        //        }
        //    }
        //    return newObject;
        //}


        private static Dictionary<string, Type> s_typesBuffer = new Dictionary<string, Type>();
        /// <summary>
        /// 根据名字获得类型
        /// </summary>
        /// <param name="classPath"></param>
        /// <returns></returns>
        public static Type GetTypeFromName(string classPath)
        {
            if (string.IsNullOrEmpty(classPath))
            {
                throw new ArgumentNullException("classPath");
            }
            if (!s_typesBuffer.ContainsKey(classPath))
            {
                Type type = Type.GetType(classPath);
                if (type == null)
                {
                    throw new ArgumentException("Type " + classPath + " can't be resolved!");
                }
                s_typesBuffer[classPath] = type;
            }

            return s_typesBuffer[classPath];
        }

        /// <summary>
        /// 根据名字获得类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="classPath"></param>
        /// <returns></returns>
        public static Type GetTypeFromName(string assemblyName, string classPath)
        {
            return GetTypeFromName(System.Reflection.Assembly.Load(assemblyName), classPath);
        }
        
        /// <summary>
        /// 根据名字获得类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="classPath"></param>
        /// <returns></returns>
        public static Type GetTypeFromName(Assembly assembly, string classPath)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(classPath))
            {
                throw new ArgumentNullException("classPath");
            }
            string fullName = classPath + ", " + assembly;
            if (!s_typesBuffer.ContainsKey(fullName))
            {
                Type type = assembly.GetType(classPath, false);
                if (type == null)
                {
                    throw new ArgumentException("Type " + classPath + " can't be resolved!");
                }
                s_typesBuffer[fullName] = type;
            }
            return s_typesBuffer[fullName];
        }

        /// <summary>
        /// 动态加载类实例
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="classPath"></param>
        /// <returns></returns>
        public static object CreateInstanceFromName(string assemblyName, string classPath)
        {
            Type type = GetTypeFromName(assemblyName, classPath);

            //Type type = Type.GetType(classPath + "," + assemblyPath);

            return CreateInstanceFromType(type);
        }

        /// <summary>
        /// 动态加载类实例
        /// </summary>
        /// <param name="classPath"></param>
        /// <returns></returns>
        public static object CreateInstanceFromName(string classPath)
        {
            Type type = GetTypeFromName(classPath);

            return CreateInstanceFromType(type);
        }

        /// <summary>
        /// 从类型实例化具体变量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateInstanceFromType(Type type, params object[] args)
        {
            try
            {
                object obj = Activator.CreateInstance(type, args);
                return obj;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach(object i in args)
                {
                    if (i == null)
                        sb.Append("[null]");
                    else
                        sb.Append("[" + i.ToString() + "]");
                    sb.Append(",");
                }
                throw new ArgumentException("Type of " + type.ToString() + " with params " + sb.ToString() + " Create Instance Failed!", ex);
            }
        }

        /// <summary>
        /// 从类型实例化具体变量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstanceFromType(Type type)
        {
            // 1.通过 InvokeMember（Constructor）
            //Object obj = type.InvokeMember(classPath, System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public
            //                                            | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            //                                            | System.Reflection.BindingFlags.CreateInstance, null, null, null);

            // 2.通过得到 ConstructorInfo 再构造
            //System.Reflection.ConstructorInfo conn = type.GetConstructor(new Type[0] { });
            //object obj = conn.Invoke(new object[0]);

            // 3. 直接
            try
            {
                object obj = Activator.CreateInstance(type);
                return obj;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(type.ToString() + " Create Instance Failed!", ex);
            }

            // 如何调用方法
            //MethodInfo method = t.GetMethod("func", new Type[] { typeof(int) });
            //method.Invoke(a1, new object[] { 1 });
        }

        /// <summary>
        /// 创建Generic类型
        /// </summary>
        /// <param name="containerType"></param>
        /// <param name="genericTypes"></param>
        /// <returns></returns>
        public static Type CreateGenericType(Type containerType, Type[] genericTypes)
        {
            if (containerType == null)
            {
                throw new ArgumentNullException("containerType");
            }
            if (genericTypes == null || genericTypes.Length == 0)
            {
                throw new ArgumentNullException("genericTypes");
            }

            // 1.通过字符串
            //Type type = Type.GetType("System.Collections.Generic.List`1[[ " + m_entityType.AssemblyQualifiedName + "]]");

            //Type generic = typeof(IList<>);
            //Type[] typeArgs = { m_entityType };
            //Type constructed = generic.MakeGenericType(typeArgs);

            return containerType.MakeGenericType(genericTypes);

            // 如何调用范型方法
            //MethodInfo mi = typeof(BaseDao).GetMethod("GetEntities", new Type[0] { });
            //MethodInfo mi_bound = mi.MakeGenericMethod(m_entityType);
            //object o = mi_bound.Invoke(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="aobjParams"></param>
        /// <returns></returns>
        public static object RunStaticMethod(Type type, string methodName, object[] aobjParams)
        {
            BindingFlags eFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return RunMethod(type, methodName, null, aobjParams, eFlags);
        }

        /// <summary>
        /// 运行静态方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="aobjParams"></param>
        /// <returns></returns>
        public static object RunStaticMethod(string assemblyName, string typeName, string methodName, object[] aobjParams)
        {
            BindingFlags eFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return RunMethod(assemblyName, typeName, methodName, null, aobjParams, eFlags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyTypeName"></param>
        /// <param name="methodName"></param>
        /// <param name="aobjParams"></param>
        /// <returns></returns>
        public static object RunStaticMethod(string assemblyTypeName, string methodName, object[] aobjParams)
        {
            string[] s = assemblyTypeName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2)
            {
                throw new ArgumentException("assemblyTypeName of " + assemblyTypeName + " is Invalid!");
            }
            return RunStaticMethod(s[1].Trim(), s[0].Trim(), methodName, aobjParams);
        }


        /// <summary>
        /// 运行实例方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="instance"></param>
        /// <param name="aobjParams"></param>
        /// <returns></returns>
        public static object RunInstanceMethod(string assemblyName, string typeName, string methodName, object instance, object[] aobjParams)
        {
            BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return RunMethod(assemblyName, typeName, methodName, instance, aobjParams, eFlags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyTypeName"></param>
        /// <param name="methodName"></param>
        /// <param name="instance"></param>
        /// <param name="aobjParams"></param>
        /// <returns></returns>
        public static object RunInstanceMethod(string assemblyTypeName, string methodName, object instance, object[] aobjParams)
        {
            BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return RunMethod(assemblyTypeName, methodName, instance, aobjParams, eFlags);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="assemblyTypeName"></param>
        ///// <param name="methodName"></param>
        ///// <param name="instance"></param>
        ///// <param name="aobjParams"></param>
        ///// <returns></returns>
        //public static object RunInstanceMethod(string assemblyTypeName, string methodName, object instance, object[] aobjParams)
        //{
        //    //string[] s = assemblyTypeName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    //if (s.Length < 2)
        //    //{
        //    //    throw new ArgumentException("assemblyTypeName of " + assemblyTypeName + " is Invalid!");
        //    //}
        //    return RunInstanceMethod(assemblyTypeName, methodName, instance, aobjParams);
        //}

        /// <summary>
        /// 运行自定义方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="objInstance"></param>
        /// <param name="aobjParams"></param>
        /// <param name="eFlags"></param>
        /// <returns></returns>
        private static object RunMethod(string assemblyName, string typeName, string methodName, object objInstance, object[] aobjParams,
                                        BindingFlags eFlags)
        {
            System.Type type = GetTypeFromName(assemblyName, typeName);
            return RunMethod(type, methodName, objInstance, aobjParams, eFlags);
        }

        private static object RunMethod(string assemblyTypeName, string methodName, object objInstance, object[] aobjParams,
                                        BindingFlags eFlags)
        {
            System.Type type = GetTypeFromName(assemblyTypeName);
            return RunMethod(type, methodName, objInstance, aobjParams, eFlags);
        }

        private static object RunMethod(Type type, string methodName, object objInstance, object[] aobjParams,
                                        BindingFlags eFlags)
        {
            try
            {
                MethodInfo m = type.GetMethod(methodName, eFlags);
                if (m == null)
                {
                    throw new ArgumentException("There is no method '" + methodName + "' for type '" + type.ToString() + "'.");
                }

                return m.Invoke(objInstance, aobjParams);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 返回obj的内部属性或者字段值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetObjectValue(object obj, string propertyName)
        {
            if (obj == null)
                return null;
            if (obj is IDictionary)
            {
                return (obj as IDictionary)[propertyName];
            }

            System.Type type = obj.GetType();

            return GetObjectValue(type, obj, propertyName);
        }

        /// <summary>
        /// 返回obj的内部属性或者字段值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetObjectValue(Type type, object obj, string propertyName)
        {
            var p = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (p != null)
            {
                return p.GetValue(obj, null);
            }
            else
            {
                var f = type.GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                {
                    return f.GetValue(obj);
                }
            }
            return null;
        }

        /// <summary>
        /// 设置obj的内部属性或者字段值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetObjectValue(object obj, string propertyName, object value)
        {
            if (obj == null)
                return;
            if (obj is IDictionary)
            {
                (obj as IDictionary)[propertyName] = value;
            }

            System.Type type = obj.GetType();

            SetObjectValue(type, obj, propertyName, value);
        }

        /// <summary>
        /// 设置obj的内部属性或者字段值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetObjectValue(Type type, object obj, string propertyName, object value)
        {
            var p = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (p != null)
            {
                p.SetValue(obj, value, null);
            }
            else
            {
                var f = type.GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                {
                    f.SetValue(obj, value);
                }
            }
            return;
        }

        /// <summary>
        /// 得到泛型类型的内置类型
        /// </summary>
        /// <param name="srcType"></param>
        /// <returns></returns>
        public static Type GetGenericUnderlyingType(Type srcType)
        {
            if (!srcType.IsGenericType)
                return null;
            string collectionClassName = srcType.FullName;
            int idx = collectionClassName.IndexOf("[[", StringComparison.InvariantCulture);
            int idx2 = collectionClassName.IndexOf("]]", StringComparison.InvariantCulture);
            string innerClassName = collectionClassName.Substring(idx + 2, idx2 - idx - 2);

            Type retType = Feng.Utils.ReflectionHelper.GetTypeFromName(innerClassName);
            return retType;
        }
    }
}