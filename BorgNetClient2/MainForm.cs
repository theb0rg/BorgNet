using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BorgNetLib;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using BorgNetLib.Packages;

namespace BorgNetClient2
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private String defaultTxtMessage = "Enter text here.";

        DeepBindingList<BorgNetLib.Message> messageQueue = new DeepBindingList<BorgNetLib.Message>();


        private User user = new User();
		private String ServerIpAdress = "85.230.218.187";
		private int ServerPortAdress = 1234;

		public MainForm(User user)
		{
			InitializeComponent();
            CreateGrid();
			txtMessage.Text = defaultTxtMessage;
			
			DisableChatGui();

            if (user != null)
            {
                EnableChatGui();
                SetBarConnected();
                clockConnection.Enabled = true;
                clockConnection.Start();
                this.user = user;
            }

            Thread ctThread = new Thread(new ThreadStart(this.SyncThread));
            ctThread.Start();
		}

        
    public void CreateGrid()
    {
      dataGridView1.AutoGenerateColumns = false;
      dataGridView1.AllowUserToAddRows = false;
      dataGridView1.RowHeadersVisible = false;
      dataGridView1.DataSource = messageQueue;

      DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
      column1.Name = "Name";
      column1.HeaderText = "Name";
      column1.DataPropertyName = "SenderUser.Name";
      dataGridView1.Columns.Add(column1);

      DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
      column2.Name = "Text";
      column2.HeaderText = "Text";
      column2.DataPropertyName = "Text";
      dataGridView1.Columns.Add(column2);

      DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
      column3.Name = "Time";
      column3.HeaderText = "Time";
      column3.DataPropertyName = "Time";
      dataGridView1.Columns.Add(column3);
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
		
		public static void DisplayError(String Message, String Caption)
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
            messageQueue.Add(new TextMessage(txtMessage.Text, user));
                        
		    txtMessage.Clear();
		}

        internal void SyncThread()
        {

            while (true)
            {
                try
                {
                    if (user.Net.Connected)
                    {
                        String dataFromClient = user.Net.Recieve();

                        //Identify packets here
                        if (dataFromClient.IsSerializable<BorgNetLib.Message>())
                        {
                            BorgNetLib.Message message = (BorgNetLib.Message)dataFromClient.XmlDeserialize(typeof(BorgNetLib.Message));
                            messageQueue.Add(message);
                        }
                    }

                }
                catch (Exception e)
                {
                    //Log.Error(e);
                    //break;
                }
            }
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
           // String text = user.SendMessage("Has exited the program..").Trim();
           // for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
           // {
           //         Application.OpenForms[i].Close();
           // }
           // this.Close();
            //Application.Exit();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            String text = user.SendMessage("Has exited the program..").Trim();
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "LoginSplash")
                {
                    form.Close();
                }
            }
            
        }

	}
}
