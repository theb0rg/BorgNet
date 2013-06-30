using BorgNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace BorgNetServer
{
    public class Server
    {
        public List<Message> messageQueue = new List<Message>();

        public void AcceptConnections()
        {     

            TcpListener serverSocket = new TcpListener(IPAddress.Any,1234);
			try{
			serverSocket.Start();
    
			while(true){
                TcpClient client = serverSocket.AcceptTcpClient();
				Thread ctThread = new Thread(new ParameterizedThreadStart(this.Accept));
				ctThread.Start(client);

                Console.WriteLine("A client connected!");
			}

			}
			catch(Exception ex)
			{
				Console.WriteLine (ex);
			}
			finally{
                foreach (User client in MainClass.connectedClients)
                {
                   client.Net.Disconnect();
				}
				serverSocket.Stop();
			}
        }

        public void Broadcast(Message message)
        {
            foreach (User client in MainClass.connectedClients)
            {
                if (client.Name != message.SenderUser.Name)
                {
                    if (client.Net.Connected)
                    {
                        Byte[] sendBytes = null;
                        String broadcast = message.SerializeObject<Message>();
                        sendBytes = Encoding.ASCII.GetBytes(broadcast);
                        NetworkStream stream = client.Net.Socket.GetStream();
                        stream.Write(sendBytes, 0, sendBytes.Length);
                        stream.Flush();
                    }
                }
            }
        }

        private static bool ValidateXml(String txt)
        {
            if (ValidXml(txt))
                Console.WriteLine("The XML is Valid!");
            else
                Console.WriteLine("Bad XML.");

            if (txt.IsSerializable<Message>())
                Console.WriteLine("The XML can be deserialized!");
            else
                Console.WriteLine("Cannot be deserialized : (");

            return true;
        }

        private void Accept(object client)
        {
            if (!(client is TcpClient))
                return;

            TcpClient clientSocket = (TcpClient)client;
            NetworkStream networkStream = clientSocket.GetStream();

            bool InitialRequest = true;

            int requestCount = 0;
            Byte[] sendBytes = null;
            string serverResponse = null;
            User user = null;

            while (true)
            {
                try
                {
                    Message message = null;
                    requestCount = requestCount + 1;
                    String dataFromClient = NetService.RecieveData(networkStream, clientSocket);

                    if (dataFromClient.IsSerializable<Message>())
                    {
                        message = (Message)dataFromClient.XmlDeserialize(typeof(Message));
                        messageQueue.Add(message);
                    }

                    if (InitialRequest)
                    {
                        if (message == null)
                        {
                            ConsoleHelper.WriteErrorLine("An unknown user logged in. Could not parse message.");
                        }
                        else
                        {
                            ConsoleHelper.WriteSuccessLine(String.Format("User {0} is logged in.", message.SenderUser.Name));
                        }

                        user = message.SenderUser;
                        user.Net.Socket = clientSocket;
                        MainClass.connectedClients.Add(user);
                        InitialRequest = false;
                    }
                    Broadcast(message);
                    Console.WriteLine(dataFromClient);

                    ValidateXml(dataFromClient);

                    serverResponse = "Message recieved. Count: " + requestCount;
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                }
                catch (SocketException exception)
                {
                    ConsoleHelper.WriteErrorLine("A client disconnected! ... : (");
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteErrorLine("Disconnecting client due to: " + ex.ToString());
                    break;
                }
            }

            MainClass.connectedClients.Remove(user);
        }
        static bool ValidXml(string xml)
        {

            try
            {
                XDocument doc = XDocument.Parse(xml);

                //TODO: Check validity of XML here. 
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public static int NumberOfTicks { get; set; }
    }
}
