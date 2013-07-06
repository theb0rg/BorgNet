using BorgNetLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BorgNetLib
{
    [XmlRoot("Property")]
   public class Property
    {
       [XmlElement("Id")]
       public int id;
       [XmlElement("Name")]
       public String name;
       [XmlIgnore]
       public object value;
       [XmlIgnore]
       public Type type;

       public Property()
       {
           this.id = (int)PropertyId.None;
           this.name = String.Empty;
           this.value = null;
           this.type = null;
       }

       public Property(int id, String Name, object Value, Type type) : this()
       {
           this.id = id;
           this.name = Name;
           this.value = Value;
           this.type = type;
       }

       [XmlElement("Type")]
       public String Type
       {
           get
           {
               if (type == null) return String.Empty;
               return type.ToString();
           }
           set { String XmlFix = value; }
       }

        [XmlElement("Value")]
        public String Value 
        {
            get
            {
                if (value == null) return String.Empty;
                return value.ToString();
            }
            set { String XmlFix = value; }
        }

    }
}
