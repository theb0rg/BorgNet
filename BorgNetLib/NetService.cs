using System;
using System.Net.Sockets;

namespace BorgNetLib
{
	public class NetService
	{
		private TcpClient socket = new TcpClient();
		private String serverIp;
		private Int32 portNumber;

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
			if(socket.Connected)
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

			return "Not connected, cant send a message!";
		}

		public TcpClient Socket{
			get{ return socket; }
			set{ socket = value; }
		}

		public bool Connected{
			get{
				if( socket.Client.Poll( 0, SelectMode.SelectRead ) )
				{
					byte[] buff = new byte[1];
					if( socket.Client.Receive( buff, SocketFlags.Peek ) == 0 )
					{
						// Client disconnected
						return false;
					}
				}
				return socket.Connected; 
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

