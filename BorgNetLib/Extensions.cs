using System;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace BorgNetLib
{
    public static class EnumerableWithIndexExtension
    {
        public static IEnumerable<ValueWithIndex<T>> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select((value, index) => new ValueWithIndex<T>(value, index));
        }

        public static IEnumerable<ValueWithIndex<T>> WithIndex<T>(this IEnumerable<T> enumerable, int startAt, int step)
        {
            return enumerable.Select((value, index) => new ValueWithIndex<T>(value, startAt + index * step));
        }

        public class ValueWithIndex<T>
        {
            public int Index { get; private set; }
            public T Value { get; private set; }

            public ValueWithIndex(T value, int index)
            {
                Value = value;
                Index = index;
            }
        }
    }
	public static class Extensions
	{

        public static bool Between(this int value, int left, int right)
        {
            return value > left && value < right;
        }

        public static bool IsDifference(this int value,int othervalue, int difference)
        {
            return othervalue.Between(value - difference, value + difference);
        }

        public static int Difference(this int value, int otherValue)
        {
            return -(value - otherValue);
        }

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
                message = message.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");
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
            objectData = objectData.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");
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

