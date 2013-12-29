using System;
using System.Net.Sockets;
using System.Web.Caching;
using System.Linq;
using BorgNetLib.Packages;

namespace BorgNetLib
{
	public class NetService
	{
		private TcpClient socket = new TcpClient();
		private String serverIp;
		private Int32 portNumber;
		
		private static String _connectedKey = "connected";

		public NetService (ConnectionSetting setting)
		{
			serverIp = setting.IpAdress;
			portNumber = Int32.Parse(new String(setting.Port.Where(c => Char.IsDigit(c)).ToArray()));
		}
		internal NetService (String ServerIp, Int32 PortNumber)
		{
			this.serverIp = ServerIp;
			this.portNumber = PortNumber;
		}

		public TcpClient Socket{
			get{ return socket; }
			set{ socket = value; }
		}
		
		public bool Connected{
			get{  
				object _connected = CacheService.Get(_connectedKey);
				if(_connected == null)
				{
					_connected = !((socket.Client.Poll(200, SelectMode.SelectRead) && (socket.Client.Available == 0)) || !socket.Client.Connected);
					CacheService.Add(_connectedKey,(bool)_connected);
					return (bool)_connected;
				}
				else return (bool)_connected;
			}
		}

		public bool Login(String Username, String Password, User user)
		{
            if (Connected)
            {
                user.Name = Username;
                Send(new LoginMessage(user));

                String dataFromClient = Recieve();

                //Identify packets here
                if (dataFromClient.IsSerializable<LoginMessage>())
                {
                    LoginMessage loginMessage = (LoginMessage)dataFromClient.XmlDeserialize(typeof(LoginMessage));

                    //verify loginMessage here
                    if(loginMessage != null)
                    return loginMessage.Successful;
                }
            }
            return false;
		}

		public bool Connect(){
			//if(Connected) return true;

			try
			{
			socket.Connect(serverIp, portNumber);
			}
			catch(Exception ex){
				//log.Error(ex);
			}
			
			CacheService.Remove(_connectedKey);
			return Connected;
		}

        public void Disconnect()
        {
            if (Connected)
            {
                //TODO: Send disconnect message..
                socket.Close();
            }
        }

        public bool Reconnect()
        {
            Disconnect();
            return Connect();
        }

		public String ServerIp
		{
			get{ return serverIp;}
		}
		public string PortNumber
		{
			get{ return portNumber.ToString();}
		}

        public static String RecieveData(NetworkStream stream, TcpClient client)
        {
            byte[] bytesFrom = new byte[client.ReceiveBufferSize];
            string dataFromClient = null;

            stream.Read(bytesFrom, 0, (int)client.ReceiveBufferSize);
            dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom).Trim();
            dataFromClient = dataFromClient.UTF8RemoveInvalidCharacters();

            return dataFromClient;
        }

        public string Recieve()
        {
            try
            {
                NetworkStream serverStream = Socket.GetStream();
                String dataFromClient = RecieveData(serverStream, Socket);
                return dataFromClient;
            }
            catch (Exception networkException)
                {
                    return String.Empty;
            }
        }

        public bool Send(Object Package)
        {
           try
           {
               if (Connected)
               {
                   NetworkStream serverStream = socket.GetStream();

                   byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Package.SerializeObject().Trim());
                   serverStream.Write(outStream, 0, outStream.Length);
                   serverStream.Flush();
               }
               else
                   return false;
            }
            catch (Exception e)
            {
                //Log.Error(e);
                return false;
            }
           return true;
        }

        public bool Send(String Package)
        {
            try
            {
                if (Connected)
                {
                    NetworkStream serverStream = socket.GetStream();

                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Package);
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                //Log.Error(e);
                return false;
            }
            return true;
        }

        internal string SendMessage(string txt, User user)
        {

                    TextMessage message = new TextMessage(txt,user);
                    Send(message);
                   // byte[] inStream = new byte[socket.ReceiveBufferSize];
                   // serverStream.Read(inStream, 0, (int)socket.ReceiveBufferSize);
                   // string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    return "";
               // }

          
            //return "Not connected, cant send a message!";
        }
    }
}

