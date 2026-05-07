using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifaeTools
{
    public class SafeTaskExecutor
    {
        public static Action<string>? ShowError;
        public static void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ShowError is not null)
                {
                    ShowError(ex.Message);
                }
                else {
                    var fc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.ToString());
                    Console.ForegroundColor = fc;
                }
            }
        }
    }
}
