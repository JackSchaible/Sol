using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Assets.Data
{
    public class XmlDataManager<T> : IDataManager<T>
    {
        private readonly XmlSerializer _serializer;
        private readonly string _filename = Path.Combine(Directory.GetCurrentDirectory(), typeof(T).Name + ".xml");

        public XmlDataManager()
        {
                _serializer = new XmlSerializer(typeof(T));
        }

        public T Load()
        {
            try
            {
                using (var stream = new FileStream(_filename, FileMode.Open))
                    return (T) _serializer.Deserialize(stream);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public void Save(T item)
        {
            using (var stream = new FileStream(_filename, FileMode.Open))
                _serializer.Serialize(stream, item);
        }
    }
}
