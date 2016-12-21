using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darwin
{
    public delegate double FitnessFunc(Individual individual);
    public class Individual
    {
        public static readonly Random RANDOM = new Random();

        public int[] Chromosome { get; set; }

        public double GetFitnessLevel(FitnessFunc fitnessFunc)
        {
            return fitnessFunc(this);
        }

        public Individual(int chromosomeLength)
        {
            Chromosome = new int[chromosomeLength];
        }

        public static Individual GetRandomIndividual(int chromosomeLength, int machineCount)
        {
            var ind = new Individual(chromosomeLength);
            for (int i = 0; i < chromosomeLength; i++)
            {
                //ind.Chromosome.Set(i, RANDOM.NextDouble() < 0.5);
                ind.Chromosome[i] = RANDOM.Next(0, machineCount);
            }
            return ind;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Chromosome.Length; i++)
            {
                sb.Append(Chromosome[i]);
            }
            return sb.ToString();
        }

        public Individual Crossover(Individual partner, int pivot)
        {
            /*var geneA = new BitArray(this.Chromosome);
            var geneB = new BitArray(partner.Chromosome);
            var ones = Enumerable.Repeat(true, pivot).ToList();
            var zeros = Enumerable.Repeat(false, BITS_PER_CHROMOSOME - pivot).ToList();
            ones.AddRange(zeros);
            var maskA = new BitArray(ones.ToArray());
            var maskB = new BitArray(maskA).Not();

            var finalChromo = new BitArray(BITS_PER_CHROMOSOME);
            finalChromo.Or(geneA.And(maskA));
            finalChromo.Or(geneB.And(maskB));
            return new Individual { Chromosome = finalChromo };*/

            var child = new Individual(this.Chromosome.Length);
            for (int i = 0; i < child.Chromosome.Length; i++)
            {
                child.Chromosome[i] = i < pivot ? this.Chromosome[i] : partner.Chromosome[i];
            }
            return child;
        }
        public Individual Crossover(Individual partner)
        {
            return Crossover(partner, RANDOM.Next(1, Chromosome.Length));
        }

        public Individual[] CrossoverPair(Individual partner)
        {
            var pivot = RANDOM.Next(1, Chromosome.Length);

            //var dd = Console.ForegroundColor;
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("CROSSING OVER {0} with {1} in {2}", this, partner, pivot);
            //Console.ForegroundColor = dd;

            return new[] { Crossover(partner, pivot), partner.Crossover(this, pivot) };
        }
    }
}
