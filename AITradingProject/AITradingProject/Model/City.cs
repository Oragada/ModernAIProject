using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProject.Model
{

    public enum Resource {Food, Water, Dolls};//TODO: move to own file
    public class City
    {

        internal int ID;
        private Dictionary<Resource, int> resources;
        private Resource nativeResource;
        private int baseScale;
        private int health = 10; //TODO: Decide on a start healt
        private int points = 0;
        private Edge[] edges;
        

        internal City(Dictionary<Resource, int> startResources, Resource nativeResource, int ID)
        {
            resources = startResources; 
            this.nativeResource = nativeResource; 
            //baseScale = startBaseScale;  //TODO: randomize it between 1-3
            this.ID = ID;

        }
        /// <summary>
        /// TODO: make the thingemagin
        /// </summary>
        /// <param name="e"></param>
        internal void addEdge(Edge e)
        {
        }

        /// <summary>
        /// consumes the amount of resources designated in the Game state
        /// TODO: create the consume method
        /// </summary>
        internal void consume()
        {
        }

        /// <summary>
        /// Produces its native resource based on its baseScale
        /// TODO: create the produce method
        /// </summary>
        internal void produce()
        {
        }

    }
}
