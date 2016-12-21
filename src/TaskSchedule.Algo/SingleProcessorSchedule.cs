using System.Collections.Generic;

namespace TaskSchedule.Algo
{
    [Equals]
    public class SingleProcessorSchedule
    {
        public int ProcessingTime { get; set; }
        public List<int> Jobs { get; set; }

        public SingleProcessorSchedule()
        {
            Jobs = new List<int>();
            ProcessingTime = 0;
        }

        public void Append(int job, int jobExecutionTime)
        {
            Jobs.Add(job);
            ProcessingTime += jobExecutionTime;
        }
    }
}
