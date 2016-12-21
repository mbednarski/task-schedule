using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Darwin
{
    public static class Extensions
    {
        public static void WriteColorLine(ConsoleColor color, string format, params object[] args)
        {
            var dd = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ForegroundColor = dd;
        }
    }
}
