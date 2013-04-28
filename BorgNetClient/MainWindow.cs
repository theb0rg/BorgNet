using System;
using Gtk;
using System.Net.Sockets;

public partial class MainWindow: Gtk.Window
{	
	TcpClient clientSocket = new TcpClient();

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void btnConnect_Click (object sender, EventArgs e)
	{
		if(clientSocket.Connected)
		{
		NetworkStream serverStream = clientSocket.GetStream();
		byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Message from Client$");
		serverStream.Write(outStream, 0, outStream.Length);
		serverStream.Flush();
		
		byte[] inStream = new byte[10025];
		serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
		string returndata = System.Text.Encoding.ASCII.GetString(inStream);

		txtResponse.Text = returndata; 
		//msg("Data from Server : " + returndata);
		}
	}

	protected void btnMessage_Click (object sender, EventArgs e)
	{
		if(!clientSocket.Connected)
		clientSocket.Connect("127.0.0.1", 8888);
	}
}
