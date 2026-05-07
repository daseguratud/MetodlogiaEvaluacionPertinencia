using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimento
{
    public class ConsoleUI
    {
        public static ConsoleKeyInfo WaitKey() { 
            return Console.ReadKey();
        }
        public static void ShowMessage(string message)
        {
            Show(message, ConsoleColor.Yellow);
        }
        public static void ShowError(Exception ex)
        {
            Show(ex.Message, ConsoleColor.Red);
        }
        public static void ShowStageTitle(string message)
        {
            Show(message, ConsoleColor.Cyan);
        }
        public static void ShowTitle(string message)
        {
            Show(message, ConsoleColor.Yellow);
        }
        private static void Show(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
