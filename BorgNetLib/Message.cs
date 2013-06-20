using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BorgNetLib
{

	public class Message
	{
		private string text;
        private long timestamp;
        private User user;
		public Message()
		{
            this.text = "";
            this.user = new User();
            timestamp = DateTime.Now.ToBinary();
		}

		public Message (String Text,User user)
		{
			this.text = Text;
            this.user = user;
            timestamp = DateTime.Now.ToBinary();
		}

		//[XmlElement("Text")]
		public String Text
		{
			get	{ return text;}
			set { text = value;}
		}

        public String Timestamp
        {
            get { return timestamp.ToString(); }
            set
            {
                try
                {
                    timestamp = Convert.ToInt64(value);
                }
                catch { }
            }
        }

        public String Time
        {
            get { return DateTime.FromBinary(timestamp).ToShortTimeString(); }
            set
            {
 
            }
        }

        public User SenderUser
        {
            get { return user; }
            set { user = value; }
        }
		
	}
}

