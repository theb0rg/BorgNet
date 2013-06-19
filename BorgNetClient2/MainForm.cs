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
	
        List<BorgNetLib.Message> messageQueue = new List<BorgNetLib.Message>();
        BindingSource source = new BindingSource();

        private User user = new User();
		private String ServerIpAdress = "127.0.0.1";
		private int ServerPortAdress = 1234;

		public MainForm()
		{
			InitializeComponent();
            CreateGrid();
			txtMessage.Text = defaultTxtMessage;
            ConnectionSetting connection = new ConnectionSetting(ServerIpAdress, ServerPortAdress.ToString());
			
			DisableChatGui();

            if (user.Login("UnknownUser","",connection))
            {
                EnableChatGui();
                SetBarConnected();
                clockConnection.Enabled = true;
                clockConnection.Start();
            }
		}

        
    public void CreateGrid()
    {
      source.DataSource = messageQueue;

      dataGridView1.AutoGenerateColumns = false;
      dataGridView1.AllowUserToAddRows = false;
      dataGridView1.DataSource = source;

      DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
      column1.Name = "Text";
      column1.HeaderText = "Text";
      column1.DataPropertyName = "Text";
      dataGridView1.Columns.Add(column1);

     // DataGridViewTextBoxolumn column2 = new DataGridViewTextBoxColumn();
     // column2.Name = "AccountId";
     // column2.HeaderText = "Account id";
     // column2.DataPropertyName = "AccountId";
     // dataGridView1.Columns.Add(column2);

     // DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
    //  column3.Name = "PatName";
    //  column3.HeaderText = "Pat name";
    //  column3.DataPropertyName = "PatName";
    //  dataGridView1.Columns.Add(column3);
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
			if(user.Login(user.Name,"TODO: FIX RENTER PASSWORD")){
				EnableChatGui();
				SetBarConnected();
			    clockConnection.Enabled = true;
			    clockConnection.Start();
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
			
			if(user.IsConnected)
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
			//txtView.ReadOnly = true;

            if(txtMessage.Text == defaultTxtMessage)
		     txtMessage.Text = String.Empty;			
		}
		
		public void EnableChatGui(){
			txtMessage.ReadOnly = false;
			btnSend.Enabled = true;	
            //txtView.ReadOnly = false;
            
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

            String text = user.SendMessage(txtMessage.Text).Trim();
            messageQueue.Add(new BorgNetLib.Message(txtMessage.Text,user));

            source.ResetBindings(true);
		    txtMessage.Clear();
		}
		
		void ClockConnectionTick(object sender, EventArgs e)
		{
            if (!user.IsConnected)
			{
				if(user.Login(user.Name,"")){
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            String text = user.SendMessage("EXIT ME").Trim();
        }

	}
}
