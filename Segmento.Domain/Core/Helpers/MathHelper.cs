using System;
using System.Collections.Generic;
using System.Linq;

namespace Segmento.Domain.Core.Helpers
{
    public static class MathHelper
    {
        /// <summary>
        /// Calculate median 
        /// </summary>
        public static double GetMedian(IEnumerable<int> Values)
        {
            int numberCount = Values.Count();
            if (numberCount == 0) throw new InvalidOperationException("Empty collection");
            int halfIndex = Values.Count() / 2;
            var sortedNumbers = Values.OrderBy(n => n);
            if (numberCount % 2 == 0)
                return (sortedNumbers.ElementAt(halfIndex) +
                           sortedNumbers.ElementAt(halfIndex - 1)) / 2d;
            else
                return sortedNumbers.ElementAt(halfIndex);
        }        
    }
}
