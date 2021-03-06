﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib.Packages
{
    public class PongUpdateMessage : Message
    {
        public PongUpdateMessage()
        {
            base.PackageType = Packages.PackageType.PaddleUpdate;
        }

        public PongUpdateMessage(int SessionID, int Player, int Y, TimeSpan gameTime,User user) : base (user)
        {
            this.SessionID = SessionID;
            this.Player = Player;
            this.Y = Y;
            this.ElapsedGameTime = gameTime;
            base.PackageType = Packages.PackageType.PaddleUpdate;
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

      /*  public List<int> LastMovements
        {
            get;
            set;
        }*/
        public int Y
        {
            get;
            set;
        }

        public String MinimalPositionPacket
        {
            get { return "$"+Y; }
        }
    }
}
