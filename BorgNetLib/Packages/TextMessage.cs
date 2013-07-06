﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib.Packages
{
    public class TextMessage : Message
    {
        private string text;

        public TextMessage() : base()
        {
            text = "";
        }
        public TextMessage(String text, User user) : base(user)
        {
            this.text = text;
        }

        public String Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}
