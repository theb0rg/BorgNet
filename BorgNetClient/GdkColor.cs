using System;

namespace BorgNetClient
{
	public static class GdkColor
	{
		public static Gdk.Color Red{ get{ return new Gdk.Color(255,0,0); }	}
		public static Gdk.Color Green{ get{ return new Gdk.Color(0,255,0); }	}
		public static Gdk.Color Blue{ get{ return new Gdk.Color(0,0,255); }	}
	}
}

