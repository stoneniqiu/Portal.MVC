using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Niqiu.Core.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlHelper
    {
        private static void XmlSerializeInternal(Stream stream, object obj, Encoding encoding)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var serializer = new XmlSerializer(obj.GetType());

            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = Environment.NewLine,
                Encoding = encoding,
                IndentChars = "    "
            };

            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, obj);
                writer.Close();
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object obj, Encoding encoding)
        {
            using (var stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, obj, encoding);

                stream.Position = 0;
                using (var reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(object obj, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, obj, encoding);
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="key">密钥</param>
        public static void XmlSerializeToFile(object obj, string path, Encoding encoding, string key)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            var content = XmlSerialize(obj, encoding);
            //加密
            content = Encrypt.EncryptString(content);
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="content">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string content, Encoding encoding)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException("content");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var mySerializer = new XmlSerializer(typeof (T));
            using (var ms = new MemoryStream(encoding.GetBytes(content)))
            {
                using (var sr = new StreamReader(ms, encoding))
                {
                    return (T) mySerializer.Deserialize(sr);
                }
            }
        }


        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var xml = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="key">密钥</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding, string key)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var xml = File.ReadAllText(path, encoding);
            //解密
            xml = Encrypt.DecryptString(xml, key);
            return XmlDeserialize<T>(xml, encoding);
        }


        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <param name="content">包含对象的XML字符串</param>
        /// <param name="type">反序列化对象类型</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static object XmlDeserialize(string content, Type type, Encoding encoding)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException("content");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var mySerializer = new XmlSerializer(type);
            using (var ms = new MemoryStream(encoding.GetBytes(content)))
            {
                using (var sr = new StreamReader(ms, encoding))
                {
                    return mySerializer.Deserialize(sr);
                }
            }
        }


        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="type">反序列化对象类型</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static object XmlDeserializeFromFile(string path, Type type, Encoding encoding)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException("path");
                if (encoding == null)
                    throw new ArgumentNullException("encoding");

                var xml = File.ReadAllText(path, encoding);
                return XmlDeserialize(xml, type, encoding);
            }
            catch (Exception ex)
            {
                var err = "加载 \"" + path + "\" 配置文件错误，具体位置为：" + ex.Message;
                throw new Exception(err);
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="type">反序列化对象类型</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="key">密钥</param>
        /// <returns>反序列化得到的对象</returns>
        public static object XmlDeserializeFromFile(string path, Type type, Encoding encoding, string key)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var xml = File.ReadAllText(path, encoding);
            //解密
            xml = Encrypt.DecryptString(xml, key);
            return XmlDeserialize(xml, type, encoding);
        }
    }
}