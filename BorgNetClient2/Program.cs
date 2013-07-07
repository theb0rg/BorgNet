/*
 * Created by SharpDevelop.
 * User: BORG
 * Date: 2013-04-30
 * Time: 19:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace BorgNetClient2
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
        public static bool Shutdown = false;
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            LoginSplash instance = new LoginSplash();
			Application.Run(instance);

		}
		
	}
}
