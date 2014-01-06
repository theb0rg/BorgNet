using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib.Packages
{
    public class PongBallMessage : Message
    {
        public  PongBallMessage()
        {
            base.PackageType = Packages.PackageType.BallUpdate;
        }

        public  PongBallMessage(User user) : base(user)
        {
            base.PackageType = Packages.PackageType.BallUpdate;
        }


    }
}
