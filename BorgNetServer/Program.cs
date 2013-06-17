using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using BorgNetLib;

namespace BorgNetServer
{
	class MainClass
	{
		private static List<User> connectedClients = new List<User>();

		public static void Main (string[] args)
		{
			TcpListener serverSocket = new TcpListener(IPAddress.Any,1234);
			try{
			serverSocket.Start();

			while(true){
				TcpClient client =  (serverSocket.AcceptTcpClient());
				Thread ctThread = new Thread(Accept);
				ctThread.Start(client);

                Console.WriteLine("A client connected!");

				//connectedClients.Add(client);
			}

			}
			catch(Exception ex)
			{
				Console.WriteLine (ex);
			}
			finally{
				foreach(User client in connectedClients){
                    client.Net.Disconnect();
				}
				serverSocket.Stop();
			}

		}

        private static String RecieveData(NetworkStream stream, TcpClient client)
        {

            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;

            stream.Read(bytesFrom, 0, (int)client.ReceiveBufferSize);
            dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom).Trim();
            dataFromClient = RemoveTroublesomeCharacters(dataFromClient);

            return dataFromClient;
        }
        private static bool ValidateXml(String txt)
        {
            if (ValidXml(txt))
                Console.WriteLine("The XML is Valid!");
            else
                Console.WriteLine("Bad XML.");

            if (CanBeDeserialized(txt))
                Console.WriteLine("The XML can be deserialized!");
            else
                Console.WriteLine("Cannot be deserialized : (");

            return true;
        }

		private static void Accept(object client)
		{
			if(!(client is TcpClient))
				return;

            TcpClient clientSocket = (TcpClient)client;
            NetworkStream networkStream = clientSocket.GetStream();

            bool InitialRequest = true;

			int requestCount = 0;
            Byte[] sendBytes = null;
            string serverResponse = null;
			
			while ((true))
			{
				try
				{
                    Message message = null;
					requestCount = requestCount + 1;
                    String dataFromClient = RecieveData(networkStream, clientSocket);

                    if (CanBeDeserialized(dataFromClient))
                    {
                        message = (Message)dataFromClient.XmlDeserialize(typeof(Message));
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
                        InitialRequest = false;
                    }

					Console.WriteLine(dataFromClient);

                    ValidateXml(dataFromClient);
					
                    serverResponse = "Message recieved. Count: " + requestCount;
					sendBytes = Encoding.ASCII.GetBytes(serverResponse);
					networkStream.Write(sendBytes, 0, sendBytes.Length);
					networkStream.Flush();
					Console.WriteLine(" >> " + serverResponse);
				}
				catch(SocketException exception)
				{
					Console.WriteLine("A client disconnected! ... : (");
				}
				catch (Exception ex)
				{
					Console.WriteLine("Disconnecting client due to: " + ex.ToString());
					break;
				}
			}
		}

		public static string RemoveTroublesomeCharacters(string inString)
{
    if (inString == null) return null;

    StringBuilder newString = new StringBuilder();
    char ch;

    for (int i = 0; i < inString.Length; i++)
    {

        ch = inString[i];
        // remove any characters outside the valid UTF-8 range as well as all control characters
        // except tabs and new lines
        if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
        {
            newString.Append(ch);
        }
    }
    return newString.ToString();

}
		static bool ValidXml (string xml)
		{

			try
			{
				XDocument doc = XDocument.Parse(xml);

				//TODO: Check validity of XML here. 
				return true;
			}
			catch(Exception e){
				Console.WriteLine(e);
				return false;
			}
		}

		static bool CanBeDeserialized(string message)
		{
			try
			{
				Message msg = (Message)message.XmlDeserialize(typeof(Message));
				
				//TODO: Check validity of XML here. 
				return true;
			}
			catch(Exception e){
				return false;
			}
		}
	}
}
