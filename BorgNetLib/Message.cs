using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BorgNetLib
{

	public class Message
	{
		private string text;
		public Message()
		{
		}
		public Message (String Text)
		{
			this.text = Text;
		}

		//[XmlElement("Text")]
		public String Text
		{
			get	{ return text;}
			set { text = value;}
		}
		
	}
}

