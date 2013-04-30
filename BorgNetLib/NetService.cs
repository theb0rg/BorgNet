using System;
using System.Net.Sockets;
using System.Web.Caching;

namespace BorgNetLib
{
	public class NetService
	{
		private TcpClient socket = new TcpClient();
		private String serverIp;
		private Int32 portNumber;
		
		private static String _connectedKey = "connected";

		public NetService ()
		{
			serverIp = "127.0.0.1";
			portNumber = 1234;
		}
		public NetService (String ServerIp, Int32 PortNumber)
		{
			this.serverIp = ServerIp;
			this.portNumber = PortNumber;
		}

		public string SendMessage(String Message)
		{
			try{
			if(Connected)
			{
				Message message = new Message(Message);
				NetworkStream serverStream = socket.GetStream();

			byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message.SerializeObject().Trim());
			serverStream.Write(outStream, 0, outStream.Length);
			serverStream.Flush();
			
			byte[] inStream = new byte[10025];
			serverStream.Read(inStream, 0, (int)socket.ReceiveBufferSize);
			string returndata = System.Text.Encoding.ASCII.GetString(inStream);
			return returndata;
			}

			}
			catch(Exception e){
				//Log.Error(e);
			}
			return "Not connected, cant send a message!";
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
					_connected = !((socket.Client.Poll(1000, SelectMode.SelectRead) && (socket.Client.Available == 0)) || !socket.Client.Connected);
					CacheService.Add(_connectedKey,(bool)_connected);
					return (bool)_connected;
				}
				else return (bool)_connected;
			}
		}

		public bool Login(String Username, String Password)
		{
			return false;
		}

		public bool Connect(){
			if(Connected) return true;

			try
			{
			socket.Connect(serverIp, portNumber);
			}
			catch(Exception ex){
				//log.Error(ex);
			}

			return Connected;
		}

		public String ServerIp
		{
			get{ return serverIp;}
		}
		public string PortNumber
		{
			get{ return portNumber.ToString();}
		}

	}
}

