using BorgNetLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib
{
    public class Userstate
    {
       public ConnectionState ConnectionState = ConnectionState.Disconnected;
       public MessageState    MessageState    = MessageState.Blank;
        //TODO: More states..
    }
}
