using System;
using System.Collections.Generic;
using System.Numerics;

namespace TaskSchedule.Algo
{
    public class NTupleGenerator
    {
        public IEnumerable<int[]> GenerateNTuples(int digits, int @base)
        {
            //var iterations = (ulong)Math.Pow(@base, digits);
            var iterations = BigInteger.Pow(@base, digits);
            
            var tuple = new int[digits];
            yield return tuple;
            
            for (ulong i = 1; i < iterations; i++)
            {
                tuple = Next(tuple, 0, @base);
                yield return tuple;
            }
        }

        private int[] Next(int[] prev, int offset, int maxVal)
        {
            if (offset >= prev.Length)
            {
                throw new OverflowException();
            }

            //dodaj na ostatniej pozycji
            prev[offset]++;

            //przeniesienie
            if (prev[offset] >= maxVal)
            {
                prev[offset] = 0;
                return Next(prev, offset + 1, maxVal);
            }

            return prev;
        }
    }
}
