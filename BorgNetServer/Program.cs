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
        public static List<User> connectedClients = new List<User>();

		public static void Main (string[] args)
		{
            Server serverInstance = new Server();
            Thread AcceptThread = new Thread(new ThreadStart(serverInstance.AcceptConnections));
            AcceptThread.Start();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.D1:
                            Console.WriteLine("\n" + connectedClients.Count + " Connected clients: ");
                            for(int i = 0; i < connectedClients.Count; i++)
                            {
                                User user = connectedClients[i];
                                Console.WriteLine(String.Format("{0} {1}",i+1,user.Name));
                            }
                            break;
                        case ConsoleKey.D2:
                            Console.WriteLine(Server.NumberOfTicks);
                            break;
                        default:
                            break;
                    }
                }
                Thread.Sleep(10);
            }
		}
       
	}
}
