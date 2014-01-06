using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib.Packages
{
    public class LoginMessage : Message
    {
        private bool successful;
        private String password;

        public LoginMessage() : base()
        {
            this.successful = false;
            base.PackageType = Packages.PackageType.LoginMessage;

        }
        public LoginMessage(bool successful) : this()
        {
            this.successful = successful;
        }

        public LoginMessage(User user) : base(user)
        {
            this.successful = false;
            base.PackageType = Packages.PackageType.LoginMessage;
        }

        public bool Successful
        {
            get { return successful; }
            set { successful = value; }
        }

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        public String Username
        {
            get { return base.SenderUser; }
            set { base.SenderUser = value; }
        }


    }
}
