using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Feng.Net.Utils
{
    public static class TypeHelper
    {
        private static bool GetJsonValue(Newtonsoft.Json.Linq.JObject jo, string name, out Newtonsoft.Json.Linq.JToken value)
        {
            if (jo.Property(name) == null)
            {
                value = null;
                return false;
            }

            value = jo[name];
            return true;
        }
        private static bool GetTypeValue(object jo, string name, out object value)
        {
            Type type = jo.GetType();
            if (type.GetProperty(name) == null)
            {
                value = null;
                return false;
            }

            value = type.InvokeMember(name, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                        null, jo, null, null, null, null);
            return true;
        }

        public static string SerializeTypeFromRealToWS(object i)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.ContractResolver = RealToWSJsonConverterContractResolver.Instance;
            settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            string postJson = Newtonsoft.Json.JsonConvert.SerializeObject(i, Newtonsoft.Json.Formatting.None, settings);
            return postJson;
        }

        public static T ConvertTypeFromRealToWS<T>(object i)
            where T : class, new()
        {
            T item = Feng.Utils.ReflectionHelper.CreateInstanceFromType(typeof(T)) as T;
            foreach (var p in typeof(T).GetProperties())
            {
                object value = EntityScript.GetPropertyValue(i, p.Name);
                if (value != null)
                {
                    Type propertyType = value.GetType();
                    if (propertyType.IsValueType || propertyType == typeof(string) || propertyType.IsEnum)
                    {
                    }
                    else if (propertyType.GetInterface("IEnumerable") != null)  // Collections
                    {
                        continue;
                    }
                    else
                    {
                        value = value.ToString();
                    }
                }
                EntityScript.SetPropertyValue(item, p.Name, value);
            }
            return item;
        }
        public static IDictionary<string, object> ConvertTypeFromWSToDictionary(object i)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();

            Newtonsoft.Json.Linq.JObject jo = i as Newtonsoft.Json.Linq.JObject;
            if (jo != null)
            {
                foreach (var j in jo)
                {
                    switch (j.Value.Type)
                    {
                        case Newtonsoft.Json.Linq.JTokenType.Boolean:
                            dict[j.Key] = (bool?)j.Value;
                            break;
                        case Newtonsoft.Json.Linq.JTokenType.Date:
                            dict[j.Key] = (DateTime?)j.Value;
                            break;
                        case Newtonsoft.Json.Linq.JTokenType.Float:
                            dict[j.Key] = (double?)j.Value;
                            break;
                        case Newtonsoft.Json.Linq.JTokenType.Integer:
                            dict[j.Key] = (int?)j.Value;
                            break;
                        case Newtonsoft.Json.Linq.JTokenType.None:
                        case Newtonsoft.Json.Linq.JTokenType.Null:
                            dict[j.Key] = null;
                            break;
                        default:
                            string v = j.Value.ToString();
                            v = v.Trim('"');
                            if (string.IsNullOrEmpty(v) || v.ToUpper() == "NULL")
                            {
                                dict[j.Key] = null;
                            }
                            else
                            {
                                dict[j.Key] = v;
                            }
                            break;
                    }
                }
            }
            return dict;
        }
        public static T ConvertTypeFromWSToReal<T>(object i)
            where T : class, new()
        {
            bool isJsonObject = false;
            Newtonsoft.Json.Linq.JObject jo = i as Newtonsoft.Json.Linq.JObject;
            object obj = i;
            if (jo != null)
            {
                isJsonObject = true;
            }

            var type = typeof(T);
            T item = new T();
            foreach (var p in typeof(T).GetProperties())
            {
                if (!p.CanWrite)
                    continue;
                if (!p.GetSetMethod().IsVirtual)    // Like ID in BaseADEntity, not data property, only a wrapper
                    continue;

                object objValue = null;
                Newtonsoft.Json.Linq.JToken joValue = null;
                if (isJsonObject)
                {
                    if (!GetJsonValue(jo, p.Name, out joValue))
                        continue;
                }
                else
                {
                    if (!GetTypeValue(obj, p.Name, out objValue))
                        continue;
                }
                if (p.PropertyType.IsValueType || p.PropertyType.IsEnum
                    || p.PropertyType == typeof(string))
                {
                    object v2 = null;
                    if (isJsonObject)
                    {
                        if (joValue == null)
                        {
                        }
                        else
                        {
                            v2 = joValue.ToObject(p.PropertyType);
                            if (p.PropertyType.IsEnum)
                            {
                                v2 = Enum.Parse(p.PropertyType, v2.ToString());
                            }
                        }
                    }
                    else
                    {
                        if (objValue == null)
                        {
                        }
                        else
                        {
                            v2 = Feng.Utils.ConvertHelper.ChangeType(objValue, p.PropertyType);
                            if (p.PropertyType.IsEnum)
                            {
                                v2 = Enum.Parse(p.PropertyType, v2.ToString());
                            }
                        }
                    }
                    type.InvokeMember(p.Name, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                        null, item, new object[] { v2 }, null, null, null);
                }
                else
                {
                    if (p.PropertyType.GetInterface("IEnumerable") != null)
                    {
                        continue;
                    }
                    object v2 = Feng.Utils.ReflectionHelper.CreateInstanceFromType(p.PropertyType);
                    string[] sIdNames = new string[] { "ID" };
                    foreach (var sIdName in sIdNames)
                    {
                        if (v2.GetType().GetProperty(sIdName) != null)
                        {
                            object v = null;
                            if (isJsonObject)
                            {
                                v = joValue.ToObject(v2.GetType().GetProperty(sIdName).PropertyType);
                            }
                            else
                            {
                                v2 = Feng.Utils.ConvertHelper.ChangeType(objValue, v2.GetType().GetProperty(sIdName).PropertyType);
                            }
                            if (v == null)
                            {
                                type.InvokeMember(p.Name, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                                    null, item, new object[] { null }, null, null, null);
                            }
                            else
                            {
                                Feng.Utils.ReflectionHelper.SetObjectValue(v2, sIdName, v);
                                type.InvokeMember(p.Name, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                                    null, item, new object[] { v2 }, null, null, null);
                            }
                            break;
                        }
                    }
                }
            }
            return item;
        }

    }
}
