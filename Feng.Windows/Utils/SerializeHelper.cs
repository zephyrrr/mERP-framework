using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class SerializeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="graph"></param>
        public static void Serialize(string fileName, object graph)
        {
            try
            {
                using (var file = System.IO.File.Open(fileName, System.IO.FileMode.Create))
                {
                    var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bf.Serialize(file, graph);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string fileName)
        {
            try
            {
                using (var file = System.IO.File.Open(fileName, System.IO.FileMode.Open))
                {
                    var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)bf.Deserialize(file);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="graph"></param>
        public static byte[] Serialize(object graph)
        {
            try
            {
                using (var file = new System.IO.MemoryStream())
                {
                    var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bf.Serialize(file, graph);
                    return file.ToArray();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
        {
            try
            {
                using (var file = new System.IO.MemoryStream(data))
                {
                    var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)bf.Deserialize(file);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="graph"></param>
        public static byte[] SerializeBson(object graph)
        {
            try
            {
                using (var file = new System.IO.MemoryStream())
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    Newtonsoft.Json.Bson.BsonWriter writer = new Newtonsoft.Json.Bson.BsonWriter(file);
                    serializer.Serialize(writer, graph);
                    return file.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeBson<T>(byte[] data)
        {
            try
            {
                using (var file = new System.IO.MemoryStream(data))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    Newtonsoft.Json.Bson.BsonReader reader = new Newtonsoft.Json.Bson.BsonReader(file);
                    return serializer.Deserialize<T>(reader);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
