using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darwin
{
    public class Mutator
    {
        public static double MUTATION_RATE = 0.01;
        private readonly int machinesCount;

        public Mutator(int machinesCount)
        {
            this.machinesCount = machinesCount;
        }

        public void Mutate(ref Population population)
        {
#if PARALLEL_MUTATE
            population.Individuals.AsParallel().ForAll( individual => {
#else
            foreach (var individual in population.Individuals)
            {
#endif
                for (int i = 0; i < individual.Chromosome.Length; i++)
                {
                    if (Individual.RANDOM.NextDouble() < MUTATION_RATE)
                    {
                        individual.Chromosome[i] = Individual.RANDOM.Next(machinesCount);
                    }
                }
#if PARALLEL_MUTATE
            });
#else
            }
#endif

        }
    }
}
