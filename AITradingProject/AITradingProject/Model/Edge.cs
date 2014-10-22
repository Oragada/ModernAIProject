using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProject.Model
{
    public class Edge
    {

        private City city1;
        private City city2;
        private int weight;


        public Edge(City first, City second, int weight)
        {
            city1 = first;
            city2 = second;
            this.weight = weight;//TODO: Randomize it from static reference - remove weight from contructor
        }


       


        /// <summary>
        /// Returns the city that is not the city given.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public City other(City city)
        {
            //@TODO: should implement an other function with structs
            return null;
            
        }


    }
}
