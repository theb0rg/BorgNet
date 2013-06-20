using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BorgNetClient2
{
    public partial class LoginSplash : Form
    {
        public LoginSplash()
        {
            InitializeComponent();
            lblLogo.Font = new Font(lblLogo.Font.FontFamily, 1, lblLogo.Font.Style);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() != String.Empty)
            {
                Form form = new MainForm(txtUsername.Text);
                //TODO: Add userthings to form and do validation on user credentials..
                this.Hide();
                form.Show();
                //this.Close();
            }
            else
            {
                MainForm.DisplayError("You need to enter an username.","Login");
            }
        }

        private int MaxLimit = 40;
        private int MinLimit = 0;

        private bool ScaleUp = false;

        private void test_Tick(object sender, EventArgs e)
        {
            Size size = TextRenderer.MeasureText(lblLogo.Text, new Font(lblLogo.Font.FontFamily, lblLogo.Font.Size, lblLogo.Font.Style));

            if (lblLogo.Font.Size >= MaxLimit)
            {
                ScaleUp = false;
            }
            else if ((lblLogo.Font.Size - 0.1f) <= MinLimit)
            {
                ScaleUp = true;
            }

            if (ScaleUp)
            {
                lblLogo.Font = new Font(lblLogo.Font.FontFamily,lblLogo.Font.Size + 0.1f,lblLogo.Font.Style);
            }
            else
            {
                lblLogo.Font = new Font(lblLogo.Font.FontFamily,lblLogo.Font.Size - 0.1f,lblLogo.Font.Style);
            }      
        }
    }
}
