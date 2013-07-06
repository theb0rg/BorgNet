﻿using BorgNetLib;
using BorgNetLib.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Data.Entity;

namespace BorgNetServer
{
    public class Server
    {
        public List<TextMessage> messageQueue = new List<TextMessage>();

        public void AcceptConnections()
        {     

            TcpListener serverSocket = new TcpListener(IPAddress.Any,1234);
			try{
			serverSocket.Start();
    
			while(true){
                TcpClient client = serverSocket.AcceptTcpClient();
				Thread ctThread = new Thread(new ParameterizedThreadStart(this.Accept));
				ctThread.Start(client);

                Console.WriteLine("Incoming hail");
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

        public void Broadcast(TextMessage message)
        {
            foreach (User client in MainClass.connectedClients)
            {
                if (client.Name != message.SenderUser.Name)
                {
                    if (client.Net.Connected)
                    {
                        Byte[] sendBytes = null;
                        String broadcast = message.SerializeObject();
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
                ConsoleHelper.WriteSuccessLine("The XML is Valid!");
            else
                ConsoleHelper.WriteWarningLine("Bad XML.");

            if (txt.IsSerializable<Message>())
                ConsoleHelper.WriteSuccessLine("The XML can be deserialized! Message");
            else
            if (txt.IsSerializable<TextMessage>())
                ConsoleHelper.WriteSuccessLine("The XML can be deserialized! TextMessage");
            else
            if (txt.IsSerializable<LoginMessage>())
                ConsoleHelper.WriteSuccessLine("The XML can be deserialized! LoginMessage");
            else
                ConsoleHelper.WriteWarningLine("Cannot be deserialized : (");

            return true;
        }

        private void Accept(object client)
        {
            if (!(client is TcpClient))
                return;

            TcpClient clientSocket = (TcpClient)client;
            NetworkStream networkStream = clientSocket.GetStream();
            int requestCount = 0;
            Byte[] sendBytes = null;
            string serverResponse = null;
            User user = null;
            try
            {
            while (true)
            {
                String dataFromClient = NetService.RecieveData(networkStream, clientSocket);
                if (dataFromClient.IsSerializable<LoginMessage>())
                {
                    LoginMessage loginMessage = (LoginMessage)dataFromClient.XmlDeserialize(typeof(LoginMessage));
                    user = AuthenticateUser(loginMessage);

                    ValidateXml(dataFromClient);

                    if (user == null)
                    {
                        sendBytes = Encoding.ASCII.GetBytes(new LoginMessage(false).SerializeObject());
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                        ConsoleHelper.WriteErrorLine("Login failed.");
                        continue;
                    }

                    ConsoleHelper.WriteSuccessLine("Hailer identified as " + user.Name + ". Sending welcomeparty.." );

                    loginMessage.Successful = true;
                    sendBytes = Encoding.ASCII.GetBytes(loginMessage.SerializeObject());
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();

                        user.Net.Socket = clientSocket;
                        MainClass.connectedClients.Add(user);
                    break;
                }
            }

            while (true)
            {
                try
                {
                    TextMessage message = null;
                    requestCount = requestCount + 1;
                    String dataFromClient = NetService.RecieveData(networkStream, clientSocket);

                    if (dataFromClient.IsSerializable<TextMessage>())
                    {
                        message = (TextMessage)dataFromClient.XmlDeserialize(typeof(TextMessage));
                        messageQueue.Add(message);
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
                catch (System.InvalidOperationException ioex)
                {
                    ConsoleHelper.WriteErrorLine("Recieved message is invalid.");
                }
            }
                }
            catch (SocketException exception)
            {
                ConsoleHelper.WriteErrorLine("A client disconnected! ... : (");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine("Disconnecting client due to: " + ex.ToString());
            }

            if (user.Net.Connected)
            {
                user.Net.Disconnect();
            }
            MainClass.connectedClients.Remove(user);
        }

        private User AuthenticateUser(LoginMessage loginMessage)
        {
            if (loginMessage == null)
            {
                return null;
            }

            foreach (User user in MainClass.connectedClients)
            {
                if (loginMessage.Username == user.Name)
                {
                    return null;
                }
            }
            

            //if(Password and stuff is correct ( Also check socket ))
            loginMessage.Successful = true;
           // else
            //loginMessage.Successful = false;
            return loginMessage.SenderUser;
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
