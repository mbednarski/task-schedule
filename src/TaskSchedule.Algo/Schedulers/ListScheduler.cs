using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskSchedule.Algo.Schedulers
{
    public class ListScheduler : IScheduler
    {
        public SchedulingResult Schedule(int[] jobs, List<Processor> processors)
        {
            var schedules = processors.ToDictionary(key => key, x => new SingleProcessorSchedule());

            for (int i = 0; i < jobs.Length; i++)
            {
                var localMinimum = new Tuple<int, Processor>(schedules[processors.First()].ProcessingTime, processors.First());
                foreach (var cpu in processors)
                {
                    if (schedules[cpu].ProcessingTime < localMinimum.Item1)
                    {
                        localMinimum = new Tuple<int, Processor>(schedules[cpu].ProcessingTime, cpu);
                    }
                }
                schedules[localMinimum.Item2].Append(i, jobs[i]);
            }
            return new SchedulingResult
            {
                Schedule = schedules,
                ProcessingTime = schedules.Values.Max(x => x.ProcessingTime)
            };
        }


        public string GetDescription(int machinesCount, int taskCount)
        {
            return "List scheduling algorithm";
        }
    }
}