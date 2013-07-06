using BorgNetLib.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib
{
    public class Broadcast
    {
        private List<TextMessage> queue = new List<TextMessage>();

        public List<TextMessage> Queue
        {
            get { return queue;  }
            set { queue = value;  }
        }

    }
}
