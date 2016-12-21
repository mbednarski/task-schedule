using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Demo
{
    class CommandLineArgs
    {
        [Option('f', "file", Required = true)]
        public string InputFile { get; set; }

        [OptionArray('a', "algos", DefaultValue = new[] {"brute", "greedy", "genetic", "dfs"})]
        public string[] Algorithms { get; set; }

        [Option('b', "batch",DefaultValue = false )]
        public bool BatchMode { get; set; }

        [Option('p', "population", DefaultValue = 100)]
        public int PopulationSize { get; set; }

        [Option('e', "evolutions", DefaultValue = 100)]
        public int EvolutionCount { get; set; }

        [Option('c', "crossover", DefaultValue = 0.75)]
        public double CrossoverProbability { get; set; }

        [Option('m', "mutation", DefaultValue = 0.01)]
        public double MutationProbability { get; set; }

        [ParserState]
        public ParserState State { get; set; }

        [Option('t', "dfstime", DefaultValue = 20)]
        public int DfsExecutionTime { get; set; }
    }
}
