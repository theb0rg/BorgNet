using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib
{
   public class ConnectionSetting
    {
       public String IpAdress = String.Empty;
       public String Port = String.Empty;
       public ConnectionSetting()
       {

       }

       public ConnectionSetting(String IpAdress, String Port) : this()
       {
           this.IpAdress = IpAdress;
           this.Port = Port;
       }
    }
}
