using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TaskSchedule.Algo
{
    [Equals]
    public class Processor
    {
        public string Name { get; set; }

        [DebuggerStepThrough]
        public Processor(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<Processor> CreateFromNames(params string[] names)
        {
            return names.Select(name => new Processor(name));
        }
    }
}
