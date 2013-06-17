using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BorgNetLib
{

	public class Message
	{
		private string text;
        private User user;
		public Message()
		{
            this.text = "";
            this.user = new User();
		}

		public Message (String Text,User user)
		{
			this.text = Text;
            this.user = user;
		}

		//[XmlElement("Text")]
		public String Text
		{
			get	{ return text;}
			set { text = value;}
		}

        public User SenderUser
        {
            get { return user; }
            set { user = value; }
        }
		
	}
}

