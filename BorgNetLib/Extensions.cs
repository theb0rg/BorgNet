using System;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace BorgNetLib
{
	public static class Extensions
	{

        public static string UTF8RemoveInvalidCharacters(this string str)
        {
            if (str == null) return null;

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < str.Length; i++)
            {

                ch = str[i];
                // remove any characters outside the valid UTF-8 range as well as all control characters
                // except tabs and new lines
                if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();

        }
        public static bool IsSerializable<T>(this String message)
        {
            try
            {                
                object msg = message.XmlDeserialize(typeof(T));

                //TODO: Check validity of XML here. 
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
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

