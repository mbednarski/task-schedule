using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Darwin;

namespace TaskSchedule.Algo.Schedulers
{
    public class GeneticScheduler : IScheduler
    {
        private GeneticAlgorithm darwin;
        private readonly int populationSize, evolutionSteps;
        private readonly double crossoverProbability;
        private readonly double mutationProbability;

        public GeneticScheduler(int evolutionSteps, int populationSize, double crossoverProbability, double mutationProbability)
        {
            this.evolutionSteps = evolutionSteps;
            this.populationSize = populationSize;
            this.crossoverProbability = crossoverProbability;
            this.mutationProbability = mutationProbability;
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

        public SchedulingResult Schedule(int[] jobs, List<Processor> processors)
        {
            darwin = new GeneticAlgorithm(jobs, processors.Count, evolutionSteps, populationSize, crossoverProbability, mutationProbability);

            var bestIdyvidual = darwin.Evolve();
            var bestValue = darwin.ComputeSchedulingTime(bestIdyvidual);
            var result = new SchedulingResult
            {
                ProcessingTime = bestValue,
                Schedule = GetSchedule(bestIdyvidual.Chromosome, bestValue, processors)
            };
            //TODO: REALLY??
            return result;
        }


        public string GetDescription(int machinesCount, int taskCount)
        {
            return string.Format("Genetic algorithm. Population size: {0} evelotion steps: {1} Crossover rate {2:p}. Mutation rate: {3:p}", populationSize,
                evolutionSteps, crossoverProbability, mutationProbability);
        }
    }
}
