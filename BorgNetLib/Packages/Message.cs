using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BorgNetLib
{

	public class Message
	{
        private long timestamp;
        private User user;
		public Message()
		{
            this.user = new User();
            timestamp = DateTime.Now.ToBinary();
		}

		public Message (User user)
		{
            this.user = user;
            timestamp = DateTime.Now.ToBinary();
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

