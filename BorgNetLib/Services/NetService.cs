using System;
using System.Net.Sockets;
using System.Web.Caching;
using System.Linq;
using BorgNetLib.Packages;
using System.Threading;
using System.IO;
using System.Net;
using System.Diagnostics;

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

                String dataFromClient = ""; //Recieve();

                return true;
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
                SendExitMessage();
                socket.Close();
            }
        }

        private void SendExitMessage()
        {
            Send("EXIT");
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

            int length = stream.Read(bytesFrom, 0, client.ReceiveBufferSize);
            dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom, 0, length).Trim();
            dataFromClient = dataFromClient.UTF8RemoveInvalidCharacters();

            return dataFromClient;
        }

        public static void StartListening(SocketAsyncEventArgs e, Socket client)
        {
            e.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            ResetBuffer(e);
           // e.
            //e.Completed += SocketReceive;
            e.AcceptSocket = client;
            client.ReceiveAsync(e);
        }

        public static void ResetBuffer(SocketAsyncEventArgs e)
        {
            var buffer = new Byte[4096];

            e.SetBuffer(buffer, 0, 4096);
        }





        public string Recieve()
        {
            try
            {
                NetworkStream serverStream = Socket.GetStream();
               // {
                    while (!serverStream.DataAvailable)
                    {
                        Thread.Sleep(1);
                    }

                    serverStream.ReadTimeout = 2;
                   // String dataFromClient = RecieveData(serverStream, Socket);
                   // return dataFromClient;

                    using (var stream = new MemoryStream())
                    {
                     //   byte[] buffer = new byte[2048]; // read in chunks of 2KB
                     //   int bytesRead;
                    //    while ((bytesRead = serverStream.Read(buffer, 0, buffer.Length)) > 0)
                     //   {
                    //        stream.Write(buffer, 0, bytesRead);
                    //    }

                        //byte[] result = stream.ToArray();
                        serverStream.CopyTo(stream);
                        return System.Text.Encoding.ASCII.GetString(stream.ToArray()).Trim();
                        //dataFromClient = dataFromClient.UTF8RemoveInvalidCharacters();
                        // TODO: do something with the result
                    }
                //}
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
                   // NetworkStream serverStream = socket.GetStream();
                   // serverStream.WriteTimeout = 2;

                   // byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Package);
                   // serverStream.Write(outStream, 0, outStream.Length);
                   // serverStream.Flush();
                    byte[] bytedata = System.Text.Encoding.ASCII.GetBytes(Package);

            using (MemoryStream ms = new MemoryStream())
            {
               // BinaryFormatter bf = new BinaryFormatter();

               // try { bf.Serialize(ms, data); }
               // catch { return; }

                //bytedata = ms.ToArray();
            }

            try
            {
                lock (Socket)
                {
                   // Socket.Client.BeginSend(BitConverter.GetBytes(bytedata.Length), 0, IntSize, SocketFlags.None, EndSend, null);
                    Socket.Client.BeginSend(bytedata, 0, bytedata.Length, SocketFlags.None, EndSend, null);
                }
            }catch(Exception exxx){
                Debug.Print(exxx.Message);
            }

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
        private void EndSend(IAsyncResult ar)
        {
            try { Socket.Client.EndSend(ar); }
            catch (Exception exas) { Debug.Print("EndSend:"+exas.Message); }
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

