using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProjectModel.Model
{
    public class Edge
    {
        public const int MIN_WEIGHT = 1;
        public const int MAX_WEIGHT = 10;

        private readonly City city1;
        private readonly City city2;

        public int Weight { get; private set; }

        internal Edge(City first, City second)
        {
            city1 = first;
            city2 = second;
            Weight = Utility.RAND.Next(MIN_WEIGHT, MAX_WEIGHT+1);
        }

        /// <summary>
        /// Returns the city at other end of this edge from the given city.
        /// </summary>
        /// <param name="c">´The city to start from</param>
        /// <returns>The other city of this edge, or null if the city is not part of this edge</returns>
        public City Other(City c)
        {
            if (c.ID == city1.ID) return city2;
            if (c.ID == city2.ID) return city1;
            return null;

        }


    }
}
