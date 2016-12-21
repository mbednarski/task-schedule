using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using TaskSchedule.Algo;
using TaskSchedule.Algo.Schedulers;

namespace Demo
{
    class Program
    {
        private static int machinesCount;
        private static int[] jobs;
        private static List<IScheduler> algorithms;

        static void Main(string[] args)
        {
            var parsedArgs = new CommandLineArgs();
            Parser.Default.ParseArguments(args, parsedArgs);

            ReadFile(parsedArgs.InputFile);

            PrintTestCaseSummary();

            CreateAlgorithms(parsedArgs.Algorithms, parsedArgs.PopulationSize, parsedArgs.EvolutionCount, parsedArgs.CrossoverProbability, parsedArgs.MutationProbability, parsedArgs.DfsExecutionTime);

            foreach (var algorithm in algorithms)
            {

                ProcessSingleAlgo(algorithm);

            }

            if (!parsedArgs.BatchMode)
            {
                Console.WriteLine("\n[ENTER] to exit");
                Console.ReadLine();
            }

        }

        private static void ProcessSingleAlgo(IScheduler algorithm)
        {
            WriteColorLine(ConsoleColor.Green, "===================");
            Console.WriteLine(algorithm.GetDescription(machinesCount, jobs.Length));

            SchedulingResult result = null;
            var executionTime = Benchmark.MeasureExecutionTime(() =>
            {
                result = algorithm.Schedule(jobs,
                    Enumerable.Range(0, machinesCount).Select(x => new Processor(x.ToString(CultureInfo.InvariantCulture))).ToList());
            });

            PrintResult(result, executionTime);
        }

        private static void PrintResult(SchedulingResult result, TimeSpan executionTime)
        {
            PrintSchedule(result, jobs);
            Console.Write("Algo execution time ");
            WriteColorLine(ConsoleColor.Blue, "{0}", executionTime);
        }

        private static void PrintSchedule(SchedulingResult sr, int[] jobCosts)
        {
            if (jobCosts.Length > 25)
            {
                Console.Write("Total processing time: ");
                 WriteColorLine(ConsoleColor.Yellow, "{0}", sr.ProcessingTime);
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
            else
            {
                Console.Write("Total processing time: ");
                WriteColorLine(ConsoleColor.Yellow, "{0}", sr.ProcessingTime);
                char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                int c = 0;
                foreach (var cpu in sr.Schedule.Keys)
                {
                    Console.Write("{0}: ", cpu.Name.PadLeft(10));
                    int counter = 0;
                    foreach (var job in sr.Schedule[cpu].Jobs)
                    {
                        Console.Write(new string(chars[job], jobCosts[job]));
                    }
                    Console.WriteLine();
                }
            }
        }

        private static void CreateAlgorithms(string[] algoNames, int populationSize, int evolutionCount, double crossoverProbability, double mutationProbability, int dfsExecutionTime)
        {
            algorithms = new List<IScheduler>();
            if (algoNames.Contains("brute"))
            {
                algorithms.Add(new BruteForceScheduler());
            }
            if (algoNames.Contains("greedy"))
            {
                algorithms.Add(new ListScheduler());
            }
            if (algoNames.Contains("genetic"))
            {
                algorithms.Add(new GeneticScheduler(evolutionCount, populationSize, crossoverProbability, mutationProbability));
            }
            if (algoNames.Contains("dfs"))
            {
                algorithms.Add(new DfsScheduler(TimeSpan.FromSeconds(dfsExecutionTime)));
            }
        }

        private static void PrintTestCaseSummary()
        {
            Console.WriteLine("Machines: {0}\nJobs: {1}\nSum of all task execution times {2}\nTheoretical best schedule result {3}", machinesCount, jobs.Length, jobs.Sum(), Math.Ceiling(1.0 * jobs.Sum() / machinesCount));
        }

        private static void ReadFile(string filename)
        {
            var file =
                  File.ReadAllText(filename)
                      .Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(int.Parse).ToList();

            machinesCount = file.First();
            var jobsCount = file[1];
            jobs = file.Skip(2).Take(jobsCount).ToArray();
        }

        public static void WriteColorLine(ConsoleColor color, string format, params object[] args)
        {
            var dd = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ForegroundColor = dd;
        }
    }
}
