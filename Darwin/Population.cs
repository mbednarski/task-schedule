using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darwin
{
    public class Population
    {
        public List<Individual> Individuals { get; set; }

        public Population()
        {
            Individuals = new List<Individual>();
        }

        public double GetPopulationFitnessSum(FitnessFunc fitnessFunc)
        {
            return Individuals.Sum(x => x.GetFitnessLevel(fitnessFunc));
        }

        public static Population GetRandomPopulation(int count, int chromosomeLength, int machinesCount)
        {
            var p = new Population();
            for (int i = 0; i < count; i++)
            {
                p.Individuals.Add(Individual.GetRandomIndividual(chromosomeLength, machinesCount));
            }
            return p;
        }

        //public string ToString(FitnessFunc fitnessFunc)
        //{
        //    StringBuilder sb = new StringBuilder("Population: (LITTLE ENDIAN!)");
        //    sb.AppendFormat("Mean fitness: {0}\n", GetMeanFitness(fitnessFunc));
        //    int i = 0;
        //    foreach (var individual in Individuals)
        //    {
        //        int[] biases = new int[Individual.MACHINES_COUNT];
        //        for (int j = 0; j < Individual.BITS_PER_CHROMOSOME; j++)
        //        {
        //            biases[individual.Chromosome[j]] += Individual.JOBS[j];
        //        }
        //        var result = biases.Max();

        //        sb.AppendFormat("\t{0:00}: {1} Time: {2}\n", i++, individual,
        //            result);
        //    }
        //    return sb.ToString();
        //}
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Population (LITTLE ENDIAN!):\n");
            int i = 0;
            foreach (var individual in Individuals)
            {
                sb.AppendFormat("\t{0:00}: {1}\n", i++, individual);
            }
            return sb.ToString();
        }

        private double GetMeanFitness(FitnessFunc fitnessFunc)
        {
            return this.Individuals.Sum(x => x.GetFitnessLevel(fitnessFunc)) /
                Individuals.Count;
        }
    }
}
