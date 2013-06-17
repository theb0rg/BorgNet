using System;
using System.Xml.Serialization;
using System.IO;

namespace BorgNetLib
{
	public static class Extensions
	{

		public static string SerializeObject<T>(this T toSerialize)
		{
            try
            {
                XmlSerializer serializer = new XmlSerializer(toSerialize.GetType());
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, toSerialize);
                    return writer.ToString();
                }
            }
            catch(Exception e)
            {
                int asd = 0;
                return "";
            }
		}

		public static T XmlDeserialize<T>(this string objectData)
		{
			return (T)XmlDeserialize(objectData, typeof(T));
		}
		
		public static object XmlDeserialize(this string objectData, Type type)
		{
			var serializer = new XmlSerializer(type);
			object result;
			
			using (TextReader reader = new StringReader(objectData))
			{
				result = serializer.Deserialize(reader);
			}
			
			return result;
		}


	}
}

