using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSchedule.Algo;
using TaskSchedule.Algo.Schedulers;
using Profiler = TaskSchedule.Algo.Benchmark;

namespace Benchmark
{
    class Program
    {
        private static void ReadFile(string filename, out int machinesCount, out int[] jobs)
        {
            var file =
                  File.ReadAllText(filename)
                      .Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(int.Parse).ToList();

            machinesCount = file.First();
            var jobsCount = file[1];
            jobs = file.Skip(2).Take(jobsCount).ToArray();
        }

        static void Main(string[] args)
        {
            using (var results = new StreamWriter("results.csv"))
            using (var times = new StreamWriter("times.csv"))
            {
                times.WriteLine("n\tBrute-force\tGreedy\tGenetic\tDFS");

                for (int i = 1; i <= 12; i++)
                {
                    var brute = (new BruteForceScheduler());
                    var greedy = (new ListScheduler());
                    var genetic = (new GeneticScheduler(500, 100, 0.75, 0.01));
                    var dfs = (new DfsScheduler());

                    Console.WriteLine(i);
                    string filename = @"c:\code\put\OK\task-schedule\src\Demo\bin\Release\test\r" + i.ToString("00");

                    int machinesCount;
                    int[] jobs;
                    ReadFile(filename, out machinesCount, out jobs);
                    var processors = Enumerable.Range(0, machinesCount).Select(x => new Processor(x.ToString(CultureInfo.InvariantCulture))).ToList();

                    int bruteResult = -1, greedyResult = -1, geneticResult = -1, dfsResult = -1;
                    TimeSpan bruteTime, greedyTime, geneticTime, dfsTime;

                    bruteTime = Profiler.MeasureExecutionTime(() =>
                        bruteResult = brute.Schedule(jobs, processors).ProcessingTime
                        );
                    greedyTime = Profiler.MeasureExecutionTime(() =>
                        greedyResult = greedy.Schedule(jobs, processors).ProcessingTime
                        );
                    geneticTime = Profiler.MeasureExecutionTime(() =>
                        geneticResult = genetic.Schedule(jobs, processors).ProcessingTime
                        );
                    dfsTime = Profiler.MeasureExecutionTime(() =>
                        dfsResult = dfs.Schedule(jobs, processors).ProcessingTime
                        );

                    results.WriteLine(string.Join("\t", new[] {i, bruteResult, greedyResult, geneticResult, dfsResult }));
                    times.WriteLine(string.Join("\t", new[] {i, bruteTime.TotalMilliseconds, greedyTime.TotalMilliseconds, geneticTime.TotalMilliseconds, dfsTime.TotalMilliseconds }));

                }
            }
        }
    }
}
