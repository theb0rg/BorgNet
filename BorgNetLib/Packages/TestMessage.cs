using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib.Packages
{
    public class TestMessage : Message
    {
        public String myString { get; set; }
        public TestMessage(User user) : base(user)
        {
            myString = "SUCCESS";
            base.PackageType = Packages.PackageType.TestMessage;
        }
    }
}
