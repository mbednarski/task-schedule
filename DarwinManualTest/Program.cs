using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Darwin;
using TaskSchedule.Algo;
using TaskSchedule.Algo.Schedulers;

namespace DarwinManualTest
{
    class Program
    {
        private static readonly string m10 = @"c:\code\put\OK\task-schedule\instances\m10.txt";
        private static readonly string m20 = @"c:\code\put\OK\task-schedule\instances\m10.txt";
        private static readonly string d5 = @"c:\code\put\OK\task-schedule\instances\d5.txt";

        static void Main(string[] args)
        {
            int machinesCount;
            const int STEPS_TO_PERFORM = 200;
            const int POPULATION_SIZE = 100;
            int[] jobs = ReadFile(d5, out machinesCount);

            PrintTestCaseSummary(jobs.Length, machinesCount, STEPS_TO_PERFORM, POPULATION_SIZE, jobs);

            var darwin = new GeneticAlgorithm(jobs, machinesCount, STEPS_TO_PERFORM, POPULATION_SIZE,0,0);
            var greedy = new ListScheduler();
            var brute = new BruteForceScheduler();
            
            int darwinResult = -1;
            var darwinTime = Benchmark.MeasureExecutionTime(() =>
            {
               // darwinResult = darwin.Evolve().;
            });

            var greedyResult = new SchedulingResult();
            var greedyTime = Benchmark.MeasureExecutionTime(() =>
            {
                greedyResult = greedy.Schedule(jobs, Enumerable.Range(0, machinesCount).Select(x => new Processor(x.ToString())).ToList());
            });

            var bruteResult = new SchedulingResult();
            var bruteTime = Benchmark.MeasureExecutionTime(() =>
            {
                bruteResult = brute.Schedule(jobs, Enumerable.Range(0, machinesCount).Select(x => new Processor(x.ToString())).ToList());
            });

            Console.WriteLine("Greedy: {0} in {1}", greedyResult.ProcessingTime, greedyTime);
            Console.WriteLine("Darwin: {0} in {1}", darwinResult, darwinTime);
            Console.WriteLine("Brute: {0} in {1}", bruteResult.ProcessingTime, bruteTime);

            Console.ReadLine();
        }

        private static void PrintTestCaseSummary(int jobs, int machines, int stepsToPerform, int populationSize, int[] jobCosts)
        {
            Console.WriteLine("{0} jobs on {1} machines, {2} genetic steps on population of {3}\nSum of all tasks {4}   ", jobs, machines, stepsToPerform, populationSize, jobCosts.Sum());
        }

        private static int[] ReadFile(string filename, out int machines)
        {
            var file =
                File.ReadAllText(filename)
                    .Split(new[] {' ', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToList();

            machines = file.First();
            var jobs = file[1];
            return file.Skip(2).Take(jobs).ToArray();
        }
    }
}
