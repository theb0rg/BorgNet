using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib.Packages
{
    public class PongUpdateMessage : Message
    {
        public PongUpdateMessage()
        {
        }

        public PongUpdateMessage(int SessionID, int Player, int Y, TimeSpan gameTime,User user) : base (user)
        {
            this.SessionID = SessionID;
            this.Player = Player;
            this.Y = Y;
            this.ElapsedGameTime = gameTime;
        }
        public TimeSpan ElapsedGameTime
        {
            get;
            set;
        }
        public int SessionID
        {
            get;
            set;
        }
        public Int32 Player
        {
            get;
            set;
        }
       /* public Int32 X
        {
            get;
            set;
        }*/

        public int[] LastMovements
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }
    }
}
