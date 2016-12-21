using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace TaskSchedule.Algo.Test
{
    public class NTupleGeneratorTests
    {
        private NTupleGenerator gen = new NTupleGenerator();

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 10, 100)]
        [InlineData(1, 10, 10)]
        [InlineData(1, 16, 16)]
        [InlineData(2, 16, 256)]
        [InlineData(3, 10, 1000)]
        [InlineData(4, 10, 10000)]
        public void generates_proper_count_of_ntuples(int digits, int @base, int expected)
        {
            var tuples = gen.GenerateNTuples(digits, @base);

            tuples.Should().HaveCount(expected, "all of ntuples must be present");
        }
    }
}
