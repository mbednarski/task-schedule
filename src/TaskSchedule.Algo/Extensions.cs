using System;

namespace TaskSchedule.Algo
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
