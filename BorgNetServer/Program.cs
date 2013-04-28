using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BorgNetServer
{
	class MainClass
	{
		private static List<TcpClient> connectedClients = new List<TcpClient>();

		public static void Main (string[] args)
		{
			TcpListener serverSocket = new TcpListener(IPAddress.Any,1234);
			try{
			serverSocket.Start();

			while(true){
				TcpClient client =  (serverSocket.AcceptTcpClient());
				Thread ctThread = new Thread(doChat);
				ctThread.Start(client);
				
				Console.WriteLine ("Client {0} connected! ", client.ToString());
				connectedClients.Add(client);
			}

			}
			catch(Exception ex)
			{
				Console.WriteLine (ex);
			}
			finally{
				foreach(TcpClient client in connectedClients){
					client.Close();
				}
				serverSocket.Stop();
			}

		}

		private static void doChat(object client)
		{
			if(!(client is TcpClient))
				return;

			TcpClient clientSocket = (TcpClient)client;

			int requestCount = 0;
			byte[] bytesFrom = new byte[10025];
			string dataFromClient = null;
			Byte[] sendBytes = null;
			string serverResponse = null;
			string rCount = null;
			requestCount = 0;
			
			while ((true))
			{
				try
				{
					requestCount = requestCount + 1;
					NetworkStream networkStream = clientSocket.GetStream();
					networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
					dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom).Trim();
					//dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
					Console.WriteLine(" >> " + "From client- "+ dataFromClient);
					
					rCount = Convert.ToString(requestCount);
					serverResponse = "Server to clinet" + rCount;
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
	}
}
