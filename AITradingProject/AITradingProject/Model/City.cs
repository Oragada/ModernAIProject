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
        private List<Edge> edges = new List<Edge>();        
        private bool alive =true; //New parameter - 
        

        internal City(Dictionary<Resource, int> startResources, Resource nativeResource, int ID)
        {
            resources = startResources; 
            this.nativeResource = nativeResource;
            baseScale = Utility.RAND.Next(3,7);
            this.ID = ID;

        }


        public List<Edge> getEdges()
        {
            Edge[] edgesReturned = new Edge[edges.Count];
            edges.CopyTo(edgesReturned);
            return edgesReturned.ToList();
                
        }


        /// <summary>
        /// Adds an edge to the city.
        /// </summary>
        /// <param name="e"></param>
        internal void AddEdge(Edge e)
        {
            edges.Add(e);

        }

        /// <summary>
        /// consumes the amount of resources designated in the Game state
        /// 
        /// </summary>
        internal void Consume()
        {
            foreach (Resource r in GameState.consumptions.Keys)
            {
                if (resources.ContainsKey(r))
                    resources[r] -= GameState.consumptions[r];
                else
                {
                    health--; //lost healt. should be probagated to top level
                    if (health < 1)
                    {
                        alive = false; //no more health =dead
                        return;
                    }
                }
            }

        }

        /// <summary>
        /// Produces its native resource based on its baseScale        
        /// </summary>
        internal void Produce()
        {
            int produce = Utility.RAND.Next(-3, 3);
            if((baseScale+produce)<1)
            {
                resources[nativeResource] += 1;
            }

            resources[nativeResource] += baseScale + produce;
        }

        /// <summary>
        /// Changes the city resource by the amount.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="change"></param>
        internal void ChangeResource(Resource r, int change)
        {
            resources[r] += change;
        }

        internal bool HaveResource(Resource r, int amount)
        {
            if (resources[r] >= amount)
                return true;
            return false;
        }

        internal bool HaveResource(Dictionary<Resource, int> resources)
        {
            foreach (Resource r in resources.Keys)
            {
                if (resources[r] <resources[r])
                    return false;
            }
            return true;
        }

        public bool Alive { get { return alive; }}
    }
}
