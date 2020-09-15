using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FtB_Common.Utils
{
    public class SerializeUtil
    {
        public static T DeserializeFromString<T>(string objectData)
        {
            return (T)DeserializeFromString(objectData, typeof(T));
        }

        private static object DeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            TextReader reader = null;
            try
            {
                reader = new StringReader(objectData);

                result = serializer.Deserialize(reader);
            }
            finally
            {
                reader?.Close();
            }

            return result;
        }


        public static string Serialize(object form)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(form.GetType());
            var stringWriter = new Utf8StringWriter();
            serializer.Serialize(stringWriter, form);
            return stringWriter.ToString();
        }
    }
}
