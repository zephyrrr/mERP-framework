using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Reflection;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection.Emit;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlEntityHelper
    {
        private static Type GetTypeFromXsdType(string xsdType)
        {
            switch (xsdType)
            {
                case "xs:boolean":
                    return typeof(bool);
                case "xs:string":
                    return typeof(string);
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static Type GenerateTypeFromXml(XmlDocument xmlDoc, string attributeName)
        {
            XmlNode typeNode = XmlHelper.FindXmlNode(xmlDoc, new Predicate<XmlNode>(delegate(XmlNode node)
            {
                return node.Name == "xs:complexType" && node.Attributes["name"] != null && node.Attributes["name"].Value == attributeName;
            }));

            if (typeNode.ChildNodes[0].Name != "xs:sequence")
            {
                throw new NotSupportedException();
            }

            // create a dynamic assembly and module
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "tmpAssembly";
            AssemblyBuilder assemblyBuilder = System.Threading.Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assemblyBuilder.DefineDynamicModule("tmpModule");

            // create a new type builder
            TypeBuilder typeBuilder = module.DefineType(attributeName, TypeAttributes.Public | TypeAttributes.Class
                | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout,
                null, new Type[] { typeof(IEntity) });

            int idx = 0;
            // Loop over the attributes that will be used as the properties names in out new type
            foreach (XmlNode propertyNode in typeNode.ChildNodes[0].ChildNodes)
            {
                string propertyName = propertyNode.Attributes["name"].Value;
                Type propertyType = GetTypeFromXsdType(propertyNode.Attributes["type"].Value);

                if (propertyType.IsEnum || propertyType.IsClass)
                {
                    propertyType = typeof(string);
                }

                // Generate a private field
                FieldBuilder field = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
                // Generate a public property
                PropertyBuilder property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault,
                    propertyType, null);

                // The property set and property get methods require a special set of attributes:
                MethodAttributes GetSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                // Define the "get" accessor method for current private field.
                MethodBuilder currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, GetSetAttr,
                                               propertyType, Type.EmptyTypes);

                // Intermediate Language stuff...
                ILGenerator currGetIL = currGetPropMthdBldr.GetILGenerator();
                currGetIL.Emit(OpCodes.Ldarg_0);
                currGetIL.Emit(OpCodes.Ldfld, field);
                currGetIL.Emit(OpCodes.Ret);

                // Define the "set" accessor method for current private field.
                MethodBuilder currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName, GetSetAttr,
                                               null, new Type[] { propertyType });

                // Again some Intermediate Language stuff...
                ILGenerator currSetIL = currSetPropMthdBldr.GetILGenerator();
                currSetIL.Emit(OpCodes.Ldarg_0);
                currSetIL.Emit(OpCodes.Ldarg_1);
                currSetIL.Emit(OpCodes.Stfld, field);
                currSetIL.Emit(OpCodes.Ret);

                // Last, we must map the two methods created above to our PropertyBuilder to
                // their corresponding behaviors, "get" and "set" respectively.
                property.SetGetMethod(currGetPropMthdBldr);
                property.SetSetMethod(currSetPropMthdBldr);

                idx++;
            }

            // Generate our type
            Type generetedType = typeBuilder.CreateType();

            return generetedType;
        }
    }
}
