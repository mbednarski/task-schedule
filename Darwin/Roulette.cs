using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darwin
{
    public class Roulette
    {
        private readonly Population population;
        private readonly FitnessFunc fitnessFunc;

        public Roulette(FitnessFunc fitnessFunc, Population population)
        {
            this.population = population;
            this.fitnessFunc = fitnessFunc;
        }

        public List<Individual> SpinTheWheel(int howMany)
        {
            var selected = new List<Individual>(howMany);
            for (int i = 0; i < howMany; i++)
            {
                int selectedIndex = SelectIndex(population.GetPopulationFitnessSum(fitnessFunc));   
                selected.Add(population.Individuals[selectedIndex]);
            }
            return selected;
        }

        private int SelectIndex(double maxValue)
        {
            int index = 0;
            double pick = Individual.RANDOM.NextDouble() * maxValue;
            while (pick > 0)
            {
                index++;
                pick -= population.Individuals[index-1].GetFitnessLevel(fitnessFunc);
            }
            return Math.Max(index - 1, 0);    
        }

        public static string ProbabilityToString(Population p, FitnessFunc fitnessFunc)
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (var individual in p.Individuals)
            {
                sb.AppendFormat("\t{0:00}: {1} Fitness: {2} Chances: {3:P}\n", i++, individual,fitnessFunc(individual),
                    fitnessFunc(individual) / p.GetPopulationFitnessSum(fitnessFunc));
            }

            return sb.ToString();
        }

    }
}
