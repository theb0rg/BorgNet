using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib
{
    public class Broadcast
    {
        private List<Message> queue = new List<Message>();

        public List<Message> Queue
        {
            get { return queue;  }
            set { queue = value;  }
        }

    }
}
