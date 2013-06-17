using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BorgNetLib
{
    public static class ConsoleHelper
    {
        public static void WriteErrorLine(Object text)
        {
            WriteLine(text, ConsoleColor.Red);
        }

        public static void WriteSuccessLine(Object text)
        {
            WriteLine(text, ConsoleColor.Green);
        }

        public static void WriteWarningLine(Object text)
        {
            WriteLine(text, ConsoleColor.Yellow);
        }

        public static void WriteLine(Object text)
        {
            WriteLine(text, Console.ForegroundColor);
        }
        public static void WriteLine(Object text, ConsoleColor color)
        {
            ConsoleColor PreviousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = PreviousColor;
        }
    }
}
