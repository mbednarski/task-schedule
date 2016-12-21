using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskSchedule.Algo.Schedulers
{
    public class BruteForceScheduler : IScheduler
    {
        public SchedulingResult Schedule(int[] jobs, List<Processor> processors)
        {
            int processorCount = processors.Count();
            var ntuples = new NTupleGenerator().GenerateNTuples(jobs.Length, processorCount);
            int[] bestSchedule = new int[0];
            int bestValue = int.MaxValue;

            foreach (var tuple in ntuples)
            {
                var value = ComputeSchedule(tuple, jobs, processorCount);
                if (value < bestValue)
                {
                    bestValue = value;
                    bestSchedule = (int[])tuple.Clone();
                }
            }

            return new SchedulingResult
            {

                ProcessingTime = bestValue,
                Schedule = GetSchedule(bestSchedule, bestValue, processors)
            };
        }
        
        public string GetDescription(int machinesCount, int taskCount)
        {
            return string.Format("Brute force algorithm. N-tuples to check: {0}", Math.Pow(machinesCount, taskCount));
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
        
        private int ComputeSchedule(int[] tuple, int[] jobCosts, int processorCount)
        {
            var costs = new int[processorCount];

            for (int i = 0; i < tuple.Length; i++)
            {
                var cost = jobCosts[i];
                costs[tuple[i]] += cost;
            }

            return costs.Max();
        }
    }
}
