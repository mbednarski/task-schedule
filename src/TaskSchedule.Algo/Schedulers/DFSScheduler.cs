#define DFS_PARALLEL_COMPUTE_SCHEDULE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Node = System.Collections.Generic.List<int>;

namespace TaskSchedule.Algo.Schedulers
{
    public class DfsScheduler : IScheduler
    {
        private int upperBoundValue = int.MaxValue;
        private int bestSoFarValue = int.MaxValue;
        private int[] bestSoFarTuple = null;
        private int[] jobs = null;
        private int machinesCount = -1;
        object sync = new object();
        private readonly TimeSpan? executionTime;
        private Stopwatch stopwatch;

        private List<Tuple<int, int>> sortAssign = new List<Tuple<int, int>>();

        public SchedulingResult Schedule(int[] jobs, List<Processor> processors)
        {
            for (int i = 0; i < jobs.Length; i++)
            {
                sortAssign.Add(new Tuple<int, int>(i, jobs[i]));
            }
            sortAssign = sortAssign.OrderByDescending(x => x.Item2).ToList();
            this.jobs = jobs.OrderByDescending(x => x).ToArray();

            //Console.WriteLine(string.Join(" ", this.jobs));

            machinesCount = processors.Count;
            //Seed();

            for (int i = 0; i < jobs.Length; i++)
            {
                buffer.Add((int)Math.Ceiling(this.jobs.Skip(i).Sum() * 1.0 / machinesCount * 0.3));
            }
            stopwatch = new Stopwatch();
            stopwatch.Start();

            var root = new Node { 1 };
            Traverse(root);
            stopwatch.Stop();

            var result = new int[bestSoFarTuple.Length];
            for (int i = 0; i < bestSoFarTuple.Length; i++)
            {
                result[sortAssign[i].Item1] = bestSoFarTuple[i];
            }

            return new SchedulingResult
            {
                ProcessingTime = bestSoFarValue,
                Schedule = GetSchedule(result, bestSoFarValue, processors)
            };
        }

        private void Traverse(Node root)
        {

            //if (root.Count == 2)
            //{
            //    Console.WriteLine("Visitng " + string.Join(" ", root));
            //}



            if (root.Count == jobs.Length)
            {
                // mamy liść
                // Console.WriteLine("Got leaf: " + string.Join(" ", root));
                var leafValue = ComputeSchedule(root);
                {
                    if (leafValue < bestSoFarValue)
                    {
                        bestSoFarValue = leafValue;
                        bestSoFarTuple = root.ToArray();
                        //Extensions.WriteColorLine(ConsoleColor.Green,
                        //    string.Format("{0} New bound at {1}", bestSoFarValue, string.Join(" ", root)));
                    }
                }

                return;
            }
            var estimate = ComputeEstimation(root.Count);
            // odwiedź
            //Console.WriteLine("Visiting " + string.Join(" ", root));
            // jeśli gorzej niż to co mamy to nie ma sensu grzebać dalej
            var nodeValue = ComputeSchedule(root);
            if (nodeValue + estimate > bestSoFarValue)
            {
                return;
            }

            if (executionTime != null && stopwatch.Elapsed > executionTime)
                return;

            foreach (var d in GetDescendatns(root))
            {
                Traverse(d);
            }
        }

        List<int> buffer = new Node();

        public DfsScheduler(TimeSpan? executionTime = null)
        {
            this.executionTime = executionTime;
        }

        private int ComputeEstimation(int used)
        {
            return buffer[used];
            //return (int)Math.Ceiling(jobs.Skip(used).Sum() * 1.0 / machinesCount);
            //return jobs.Skip(used).Max();
        }
        
        [DebuggerStepThrough]
        public IEnumerable<Node> GetDescendatns(Node node)
        {
            for (int i = 0; i < machinesCount; i++)
            {
                var desc = new Node(node) { i };
                yield return desc;
            }
        }

        public string GetDescription(int machinesCount, int taskCount)
        {
            return "DFS algo";
        }
        private Dictionary<Processor, SingleProcessorSchedule> GetSchedule(int[] tuple, int cost, List<Processor> processors)
        {
            var res = processors.ToDictionary(cpu => cpu, cpu => new SingleProcessorSchedule { ProcessingTime = cost, Jobs = new List<int>() });

            for (var i = 0; i < tuple.Length; i++)
            {
                var t = tuple[i];
                res[processors[t]].Jobs.Add(i);
            }
            return res;
        }

        // [DebuggerStepThrough]
        private int ComputeSchedule(List<int> tuple)
        {
            var costs = new int[machinesCount];

            for (int i = 0; i < tuple.Count; i++)
            {
                var cost = jobs[i];
                costs[tuple[i]] += cost;
            }
            return costs.Max();
        }
    }
}
