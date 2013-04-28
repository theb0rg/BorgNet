using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BorgNetLib
{
	public class XmlService
	{
		public XmlService ()
		{
		}
		/*public static string SerializeObject<T>(this T toSerialize)
		{
			XmlSerializer serializer = new XmlSerializer(toSerialize.GetType());
			using (StringWriter writer = new StringWriter())
			{
				serializer.Serialize(writer, toSerialize);				
				return writer.ToString();
			}
		}*/
		

		public static void ToXml(object Object, Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(Object.GetType());

			serializer.Serialize(stream, Object);				
		}




	}
}

