using System.Collections.Generic;

namespace TaskSchedule.Algo
{
    public class SchedulingResult
    {
        public int ProcessingTime { get; set; }
        public Dictionary<Processor, SingleProcessorSchedule> Schedule { get; set; }
    }
}
