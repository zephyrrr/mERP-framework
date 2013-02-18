using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Feng.Net.Utils
{
    public class RealToWSJsonConverterContractResolver : DefaultContractResolver
    {
        public static readonly RealToWSJsonConverterContractResolver Instance = new RealToWSJsonConverterContractResolver();

        //protected override JsonContract CreateContract(Type objectType)
        //{
        //    JsonContract contract = base.CreateContract(objectType);

        //    // this will only be called once and then cached
        //    if (objectType.IsValueType || objectType.IsEnum || objectType == typeof(string))
        //        return contract;
        //    if (objectType.GetInterface("IEnumerable") != null)
        //        throw new ArgumentException("IEnumerable should not serialize!");

        //    contract.Converter = new EntityIdConverter();

        //    return contract;
        //}
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            var propertyType = property.PropertyType;
            if (propertyType.IsValueType || propertyType == typeof(string))
            {
            }
            else if (propertyType.IsEnum)
            {
                //property.Converter = new Newtonsoft.Json.Converters.StringEnumConverter();
            }
            else if (propertyType.GetInterface("IEnumerable") != null)
            {
                property.ShouldSerialize =
                  instance =>
                  {
                      return false;
                  };
            }
            else
            {
                property.Converter = new EntityIdConverter();
            }
            return property;
        }
        //protected override IList<JsonProperty> CreateProperties(Type type, Newtonsoft.Json.MemberSerialization memberSerialization)
        //{
        //    IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

        //    // only serializer properties that start with the specified character
        //    properties =
        //       properties.Where(p => p.PropertyName.StartsWith(_startingWithChar.ToString())).ToList();

        //    return properties;
        //}
    }
}
