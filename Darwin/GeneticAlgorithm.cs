using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Darwin
{
    public class GeneticAlgorithm
    {
        private readonly int[] jobs;
        private readonly int jobsSum;
        public readonly int MachinesCount;
        public readonly int JobsCount;
        private readonly int stepsToPerform;

        private int step = 1;
        private readonly FitnessFunc fitness;
        private readonly int populationSize;
        private StreamWriter file;

        private Individual bestSoFar;

        public GeneticAlgorithm(int[] jobs, int machinesCount, int stepsToPerform, int populationSize, double crossoverProbaility, double mutationProbability)
        {
            this.jobs = jobs;
            jobsSum = jobs.Sum();
            JobsCount = jobs.Length;
            MachinesCount = machinesCount;
            this.stepsToPerform = stepsToPerform;
            this.populationSize = populationSize;
            fitness = ComputeFitness;
            Harem.PARENTING_PROBABILITY = crossoverProbaility;
            Mutator.MUTATION_RATE = mutationProbability;

#if LOG_PLOT
            file = new StreamWriter("result");
#endif
        }

        public double ComputeFitness(Individual ind)
        {
            int[] biases = new int[MachinesCount];
            for (int i = 0; i < JobsCount; i++)
            {
                biases[ind.Chromosome[i]] += jobs[i];
            }
            return jobsSum - biases.Max() + 1; //force positive value
        }

        public int ComputeSchedulingTime(Individual ind)
        {
            int[] biases = new int[MachinesCount];
            for (int i = 0; i < JobsCount; i++)
            {
                biases[ind.Chromosome[i]] += jobs[i];
            }
            return biases.Max();
        }

        public Individual Evolve()
        {
            var generation = Population.GetRandomPopulation(populationSize, JobsCount, MachinesCount);
            bestSoFar = generation.Individuals[0];
            for (step = 0; step <= stepsToPerform; step++)
            {
                generation = Step(generation);
            }
            generation.Individuals.Add(bestSoFar);
            var old = generation.Individuals.Min(x => ComputeSchedulingTime(x));
            var best = generation.Individuals.OrderBy(ComputeSchedulingTime).First();
            if(old != ComputeSchedulingTime(best))
                throw new Exception();
            return best;

            //Console.WriteLine(generation.ToString(fitness));
        }

        Population Step(Population generation)
        {
            //Extensions.WriteColorLine(ConsoleColor.Green, "======={0} Step======", step++);

            //Console.WriteLine(generation.ToString(fitness));
            //Console.WriteLine("Population fitness sum: {0}",
            //   generation.GetPopulationFitnessSum(fitness));
            var np = new Population { Individuals = new Roulette(fitness, generation).SpinTheWheel(populationSize) };

            //Console.WriteLine(Roulette.ProbabilityToString(generation, fitness));

            //Console.WriteLine("Selected for reproducing");
            //Console.WriteLine(np.ToString(fitness));

            var h = new Harem(fitness, np);
            var newGeneration = h.Reproduce();
            
            //Console.WriteLine("After reproduction");
            //Console.WriteLine(newGeneration.ToString());

            //Console.WriteLine("After mutation");
            new Mutator(MachinesCount).Mutate(ref newGeneration);
            //Console.WriteLine(newGeneration.ToString());


#if STORE_BEST_SO_FAR
            var sorted = newGeneration.Individuals.OrderBy(ComputeSchedulingTime);
            var currentBest = sorted.First();
            if (ComputeSchedulingTime(currentBest) < ComputeSchedulingTime(bestSoFar))
            {
                bestSoFar = currentBest;
            }
#endif

#if LOG_PLOT
            file.WriteLine("{2} {0} {1}", newGeneration.Individuals.Average(x => ComputeSchedulingTime(x)),
                newGeneration.Individuals.Min(x => ComputeSchedulingTime(x)), step);
            file.Flush();
#endif
            return newGeneration;
        }

    }
}
