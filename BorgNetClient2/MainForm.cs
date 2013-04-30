using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BorgNetLib;

namespace BorgNetClient2
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private String defaultTxtMessage = "Enter text here.";
		private NetService service;
		private String ServerIpAdress = "127.0.0.1";
		private int ServerPortAdress = 1234;
		
		public MainForm()
		{
			InitializeComponent();
			txtMessage.Text = defaultTxtMessage;
			service = new NetService(ServerIpAdress,ServerPortAdress);
			
			DisableChatGui();
		}
		
		void TxtMessageEnter(object sender, EventArgs e)
		{
			if(txtMessage.Text == defaultTxtMessage)
		     txtMessage.Text = String.Empty;
		}
		
		void TxtMessageLeave(object sender, EventArgs e)
		{
			if(txtMessage.Text == String.Empty)
		     txtMessage.Text = defaultTxtMessage;
		}
		
		void NewToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(service.Connect()){
				EnableChatGui();
				SetBarConnected();
			    clockConnection.Enabled = true;
			}
			else{
				DisplayError(String.Format("Cant connect to Server({0}:{1}). Is it on?",ServerIpAdress,ServerPortAdress),"Connection error");
			}
		}
		
		public void DisplayError(String Message, String Caption)
		{			
			MessageBox.Show(Message,Caption,MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly,false);
		}
		
		public void HandleGuiState(){
			
			if(service.Connected)
			{
				EnableChatGui();				
			}
			else{				
				DisableChatGui();
			}			
		}
		
		public void DisableChatGui(){
			txtMessage.ReadOnly = true;
			btnSend.Enabled = false;	
			txtView.ReadOnly = true;

            if(txtMessage.Text == defaultTxtMessage)
		     txtMessage.Text = String.Empty;			
		}
		
		public void EnableChatGui(){
			txtMessage.ReadOnly = false;
			btnSend.Enabled = true;	
            txtView.ReadOnly = false;
            
            if(txtMessage.Text == String.Empty)
		     txtMessage.Text = defaultTxtMessage;            
		}
		
		public void SetBarConnected(){
			barConnection.Step = 2;
			barConnection.BackColor = Color.Green;
			barConnection.ForeColor = Color.Green;
		}
		
		public void SetBarDisconnected(){
			barConnection.Step = 2;
			barConnection.BackColor = Color.Red;
			barConnection.ForeColor = Color.Red;
		}
		
		void BtnSendClick(object sender, EventArgs e)
		{
				txtView.Text += Environment.NewLine + service.SendMessage(txtMessage.Text);
				txtMessage.Clear();
		}
		
		void ClockConnectionTick(object sender, EventArgs e)
		{
			if(!service.Connected)
			{
				if(service.Connect()){
					SetBarConnected();
					EnableChatGui();
				}
				else{
					DisableChatGui();
				    SetBarDisconnected();
				}
			}
		}
		
		void TxtMessageMouseHover(object sender, EventArgs e)
		{
			  if(txtMessage.Text == defaultTxtMessage)
		     txtMessage.Text = String.Empty;	
		}
		
		void TxtMessageKeyPress(object sender, KeyPressEventArgs e)
		{
			  if(txtMessage.Text == defaultTxtMessage)
		     txtMessage.Text = String.Empty;	
		}
	}
}
