using System;
using Gtk;
using System.Net.Sockets;
using BorgNetLib;

public partial class MainWindow: Gtk.Window
{	
	NetService service = new NetService();

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
			service.Connect();
	}

	protected void btnMessage_Click (object sender, EventArgs e)
	{
			
			txtResponse.Text = service.SendMessage(txtMessage.Text); 
	}
}
