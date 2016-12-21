using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darwin
{
    public class Harem
    {
        public static double PARENTING_PROBABILITY = 0.75;
        private readonly FitnessFunc fitnessFunc;
        private readonly Population parents;

        public Harem(FitnessFunc fitnessFunc, Population parents)
        {
            this.fitnessFunc = fitnessFunc;
            this.parents = parents;
        }

        public Population Reproduce()
        {
            Shuffle(parents.Individuals);
            List<Individual> children = new List<Individual>(parents.Individuals.Count);

            for (int i = 0; i < parents.Individuals.Count / 2; i++)
            {
                children.AddRange(Crossover(parents.Individuals[i * 2],
                    parents.Individuals[i * 2 + 1]));
            }

            return new Population { Individuals = children };
        }

        private Individual[] Crossover(Individual a, Individual b)
        {
            if (Individual.RANDOM.NextDouble() < PARENTING_PROBABILITY)
            {
                return a.CrossoverPair(b);
            }
            return new[] { a, b };
        }


        public static void Shuffle<T>(IList<T> list)
        {
            Random rng = Individual.RANDOM;
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
