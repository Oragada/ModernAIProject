using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProject.Model
{
    public class City
    {

        internal int ID;
        private Dictionary<Resource, int> resources;
        private Resource nativeResource;
        private int baseScale;
        private int health = 10; //TODO: Decide on a start health - Later
        private int points = 0;
        private Edge[] edges;
        private bool alive; //New parameter
        

        internal City(Dictionary<Resource, int> startResources, Resource nativeResource, int ID)
        {
            resources = startResources; 
            this.nativeResource = nativeResource; 
            //baseScale = startBaseScale;  //TODO: randomize it between 1-3 - Daniel
            this.ID = ID;

        }
        /// <summary>
        /// TODO: make the thingemagin - Daniel
        /// </summary>
        /// <param name="e"></param>
        internal void AddEdge(Edge e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// consumes the amount of resources designated in the Game state
        /// TODO: create the consume method - Daniel
        /// </summary>
        internal void Consume()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Produces its native resource based on its baseScale
        /// TODO: create the produce method - Daniel
        /// </summary>
        internal void Produce()
        {
            throw new NotImplementedException();
        }

        //TODO change resources method - Daniel
        internal void ChangeResource(Resource r, int change)
        {
            throw new NotImplementedException();
        }

        internal bool HaveResource(Resource r, int amount)
        {
            throw new NotImplementedException();
        }

        public bool Alive { get { return alive; }}
    }
}
