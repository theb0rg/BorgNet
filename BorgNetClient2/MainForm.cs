using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BorgNetLib;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;

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

		public MainForm(String Username)
		{
			InitializeComponent();
            CreateGrid();
			txtMessage.Text = defaultTxtMessage;
            ConnectionSetting connection = new ConnectionSetting(ServerIpAdress, ServerPortAdress.ToString());
			
			DisableChatGui();

            if (user.Login(Username,"",connection))
            {
                EnableChatGui();
                SetBarConnected();
                clockConnection.Enabled = true;
                clockConnection.Start();
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
            messageQueue.Add(new BorgNetLib.Message(txtMessage.Text,user));
                        
		    txtMessage.Clear();
		}
        static bool CanBeDeserialized(string message)
        {
            try
            {
                BorgNetLib.Message msg = (BorgNetLib.Message)message.XmlDeserialize(typeof(BorgNetLib.Message));

                //TODO: Check validity of XML here. 
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        internal void SyncThread()
        {

            while (true)
            {
                try
                {
                    if (user.Net.Connected)
                    {
                        //Message message = new Message(txt, user);
                        NetworkStream serverStream = user.Net.Socket.GetStream();

                        String dataFromClient = RecieveData(serverStream, user.Net.Socket);
                        if (CanBeDeserialized(dataFromClient))
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

        private static String RecieveData(NetworkStream stream, TcpClient client)
        {

            byte[] bytesFrom = new byte[client.ReceiveBufferSize];
            string dataFromClient = null;

            stream.Read(bytesFrom, 0, (int)client.ReceiveBufferSize);
            dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom).Trim();
            dataFromClient = RemoveTroublesomeCharacters(dataFromClient);

            return dataFromClient;
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
            String text = user.SendMessage("Has exited the program..").Trim();
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                    Application.OpenForms[i].Close();
            }
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            String text = user.SendMessage("Has exited the program..").Trim();
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                    Application.OpenForms[i].Close();
            }
            Application.Exit();
        }

	}
}
