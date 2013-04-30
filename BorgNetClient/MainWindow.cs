using System;
using Gtk;
using System.Net.Sockets;
using BorgNetLib;
using BorgNetClient;

public partial class MainWindow: Gtk.Window
{	
	NetService service = new NetService("127.0.0.1",1234);

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
		if(service.Connect())
		{
		//lblConnected.Text = "Connected";
			lblConnected.LabelProp = "<b>Connected</b>";
		lblConnected.RedrawOnAllocate = true;
		lblConnected.ModifyBg(StateType.Active,GdkColor.Green);
        //frame1.
		}
		else
		{
			lblConnected.Text = "Disconnected";
			lblConnected.RedrawOnAllocate = true;
			lblConnected.ModifyBg(StateType.Active,GdkColor.Red);
		}
	}

	protected void btnMessage_Click (object sender, EventArgs e)
	{
			
			txtResponse.Text = service.SendMessage(txtMessage.Text); 
	}
}
