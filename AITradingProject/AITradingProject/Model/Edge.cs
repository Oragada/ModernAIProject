using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProject.Model
{
    public class Edge
    {
        public const int MIN_WEIGHT = 1;
        public const int MAX_WEIGHT = 10;

        private City city1;
        private City city2;
        private int weight;


        public Edge(City first, City second)
        {
            city1 = first;
            city2 = second;
            weight = Utility.RAND.Next(MIN_WEIGHT, MAX_WEIGHT+1);
        }


       public int getWeight()
       {
           return weight;
       }


        /// <summary>
        /// Returns the city that is not the city given.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public City other(City city)
        {
            //@TODO: should implement an other function with structs - Later
            return null;
            
        }


    }
}
