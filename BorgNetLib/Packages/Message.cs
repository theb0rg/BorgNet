using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using BorgNetLib.Packages;

namespace BorgNetLib
{

	public class Message
	{
        private PackageType type = PackageType.Message;
        private long timestamp;
        private String username;
		public Message()
		{
            this.username = "";
            timestamp = DateTime.Now.ToBinary();
		}

		public Message (User user)
		{
            this.username = user.Name;
            timestamp = DateTime.Now.ToBinary();
		}

        public PackageType PackageType
        {
            get { return type; }
            set { type = value; }

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

        public String SenderUser
        {
            get { return username; }
            set { username = value; }
        }
		
	}
}

