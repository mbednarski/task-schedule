using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace TaskSchedule.Algo
{
    public interface IScheduler
    {
        SchedulingResult Schedule(int[] jobs, List<Processor> processors);
        string GetDescription(int machinesCount, int taskCount);

    }
}