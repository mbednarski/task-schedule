using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Humanizer;
using TaskSchedule.Algo;
using TaskSchedule.Algo.Schedulers;

namespace ManualTest
{
    class CommandLineArgs
    {
        [Option('m', "machines", DefaultValue = 2, Required = false)]
        public int ProcessorCount { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = 1)]
        public IList<string> InputFiles { get; set; } 

        [ParserState]
        public ParserState State { get; set; }
    }

    public class Program
    {
        private static readonly IEnumerable<Processor> ProcessorSource = Processor.CreateFromNames("Aries", "Taurus",
            "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio",
            "Sagittarius", "Capricorn", "Aquarius", "Pisces", "Ophiuchus");


        static void Main(string[] args)
        {
            var parsedArgs = new CommandLineArgs();
            Parser.Default.ParseArguments(args, parsedArgs);

            if (parsedArgs.InputFiles.Count == 0)
            {
                InteractiveMode();
            }
            else
            {
                BatchMode(parsedArgs.ProcessorCount, parsedArgs.InputFiles.Single());
            }
        }

        private static void BatchMode(int processorCount, string filename)
        {
            Console.WriteLine("Batch mode with {0} machines. Using both algorithms", processorCount);
            int[] jobs = File.ReadAllText(filename).Split(new[] {' ', '\t', '\r', '\n'})
                .Select(int.Parse).ToArray();

            Console.WriteLine();
            Console.WriteLine("Brute force. N-tuples to check: {0}",
                (ulong)Math.Pow(processorCount, jobs.Length));
            DoScheduling(jobs, processorCount, new BruteForceScheduler());
            Console.WriteLine("\nList scheduler.");
            DoScheduling(jobs, processorCount, new ListScheduler());
        }

        private static void InteractiveMode()
        {
            Console.Write("How many machines? ");
            int processorCount = int.Parse(Console.ReadLine());
            Console.WriteLine("Interactive mode with {0} machines. Using both algorithms.", processorCount);
            Console.WriteLine("Plase type lenghts of tasks. [ENTER] to accept.");
            string times = Console.ReadLine();
            int[] jobs =
                times.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();

            Console.WriteLine();
            Console.WriteLine("Brute force. N-tuples to check: {0}",
                (ulong) Math.Pow(processorCount, jobs.Length));
            DoScheduling(jobs, processorCount, new BruteForceScheduler());
            Console.WriteLine("\nList scheduler.");
            DoScheduling(jobs, processorCount, new ListScheduler());
            Console.WriteLine("\nDFS scheduler.");
            DoScheduling(jobs, processorCount, new DfsScheduler());

            Console.WriteLine("[ENTER] to exit.");
            Console.ReadLine();
        }

        private static void DoScheduling(int[] jobs, int processorCount, IScheduler scheduler)
        {
            SchedulingResult result = null;
            var time = Benchmark.MeasureExecutionTime(() =>
               result = scheduler.Schedule(jobs, ProcessorSource.Take(processorCount).ToList()));
            PrintSchedule(result, jobs);
            Console.WriteLine("Scheduling time: {0}", time.Humanize());
        }

        public static void PrintArray(int[] arr)
        {
            Console.WriteLine(string.Join(" ", arr));
        }

        static void PrintSchedule(SchedulingResult sr, int[] jobCosts)
        {
            Console.WriteLine("Total processing time: {0}", sr.ProcessingTime);

            foreach (var cpu in sr.Schedule.Keys)
            {
                Console.Write("{0}: ", cpu.Name.PadLeft(10));
                int counter = 0;
                foreach (var job in sr.Schedule[cpu].Jobs)
                {
                    if (counter++ % 2 == 0)
                        Console.Write(new string('#', jobCosts[job]));
                    else
                        Console.Write(new string('@', jobCosts[job]));
                }

                Console.WriteLine();
            }
        }
    }
}

