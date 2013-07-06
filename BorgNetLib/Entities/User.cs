using BorgNetLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Data.Entity;

namespace BorgNetLib
{
	public class User
	{
        private Userstate userstate;
        private ConnectionSetting setting;
        private Dictionary<int,Property> properties = new Dictionary<int,Property>();
        private NetService netService;

		public User ()
		{
            userstate = new Userstate();
            setting = new ConnectionSetting("127.0.0.1","1234");
		}

        public Userstate State
        {
            get { return userstate; }
            set { userstate = value; }
        }

        public List<Property> Properties
        {
            get { return properties.Values.ToList(); }
        }

        public String Name
        {
            get { return (String)GetProperty(PropertyId.Name); }
            set
            {
                if (!PropertySet(PropertyId.Name))
                {
                    AddProperty(PropertyId.Name, value);
                }
                else
                {
                    ReplaceProperty(PropertyId.Name, value);
                }
            }
        }


        private bool AddProperty(PropertyId id, Object Value)
        {
            try
            {
                Type type = Value.GetType();
                String name = Enum.GetName(typeof(PropertyId), id);
                Property property = new Property((int)id, name, Value, type);
                properties.Add((int)id, property);
            }
            catch
            {
                //TODO: LOG
                return false;
            }
            return true;
        }
        private bool AddProperty(PropertyId id, String Value)
        {
            try
            {
                Type type = typeof(String);
                String name = Enum.GetName(typeof(PropertyId), id);
                Property property = new Property((int)id, name, Value, type);
                properties.Add((int)id, property);
            }
            catch
            {
                //TODO: LOG
                return false;
            }
            return true;
        }

        private bool ReplaceProperty(PropertyId id, String Value)
        {
            try
            {
                Type type = typeof(String);
                String name = Enum.GetName(typeof(PropertyId), id);
                Property property = new Property((int)id, name, Value, type);
                properties.Remove((int)id);
                properties.Add((int)id, property);
            }
            catch
            {
                //TODO: LOG
                return false;
            }
            return true;
        }
        private bool PropertySet(PropertyId id)
        {
            return properties.ContainsKey((int)id);
        }

        private object GetProperty(PropertyId id)
        {
            if (properties.ContainsKey((int)id))
            {
                return properties[(int)id].Value;
            }
            else return null;
        }

        [XmlIgnore]
        public NetService Net
        {
            get {
                if (netService == null)
                {
                    netService = new NetService(setting);
                }
                return netService; }
            set { netService = value; }
        }

        public String SendMessage(String message)
        {
                return Net.SendMessage(message,this);
        }

        public bool Login(String Username, String Password, ConnectionSetting connection)
        {
            if (this.State.ConnectionState == ConnectionState.Disconnected)
            {
                setting = connection;
                if (!Net.Connect())
                {
                    return false;
                }
            }
            else if(this.State.ConnectionState == ConnectionState.Connected && (connection.IpAdress != setting.IpAdress && connection.Port != setting.Port))
            {
                //Settings changed at logon, reconnect with new settings.
                if (netService.Reconnect())
                {

                }
                else
                {
                    return false;
                }
            }

            if (Net.Login(Username, Password, this))
            {

            //Do loginstuff here
            Name = Username;


            //Sync();
            return true;
            }
            return false;
        }

        public bool Login(String Username, String Password)
        {
            return Login(Username, Password, setting);
        }

        public bool Sync()
        {

            return true;
        }

        [XmlIgnore]
        public bool IsConnected { get { return Net.Connected; } }
    }
}

