using System;
using System.Xml.Serialization;
using System.IO;

namespace BorgNetLib
{
	public static class Extensions
	{
		public static string SerializeObject<T>(this T toSerialize)
		{
			XmlSerializer serializer = new XmlSerializer(toSerialize.GetType());
			using (StringWriter writer = new StringWriter())
			{
				serializer.Serialize(writer, toSerialize);				
				return writer.ToString();
			}
		}

	}
}

