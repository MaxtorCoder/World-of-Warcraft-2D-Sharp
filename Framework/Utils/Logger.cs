using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils
{
    /// <summary>
    /// Logs specified output.
    /// </summary>
    public class Logger
    {
        public enum LogType
        {
            Server,
            Error,
        }

        public static IReadOnlyDictionary<LogType, ConsoleColor> LogColor = new Dictionary<LogType, ConsoleColor>()
        {
            {LogType.Server,    ConsoleColor.White},
            {LogType.Error,     ConsoleColor.Red} 
        };

        public static void Write(LogType type, string message)
        {
            Console.Write("<");
            Console.ForegroundColor = LogColor[type];
            switch (type)
            {
                case LogType.Server:
                    Console.Write("SERVER");
                    break;
                case LogType.Error:
                    Console.Write("ERROR");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> ");
            Console.WriteLine(message);
        }

        public static void WriteDebug(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
