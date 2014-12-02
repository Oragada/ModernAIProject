using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProjectModel.Model
{
    public class City
    {
        private Dictionary<Resource, int> resources;
        private Resource nativeResource;
        private int baseScale;
        private int health = 10; //TODO: Decide on a start health - Later
        private int points = 0;
        private List<Edge> edges = new List<Edge>();        
        private bool alive = true;
        private StatusUpdate status;
        

        internal City(Dictionary<Resource, int> startResources, Resource nativeResource, int ID, StatusUpdate statusUpdate)
        {
            resources = startResources; 
            this.nativeResource = nativeResource;
            baseScale = Utility.RAND.Next(3,7);
            this.ID = ID;
            status = statusUpdate;

        }

        public int ID { get; private set; }

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
            bool basicNeeds = true;
            foreach (Resource r in GameState.BasicConsume.Keys)
            {
                try
                {
                    ChangeResource(r, -GameState.BasicConsume[r]);
                }
                catch (NegativeResourcesException)
                {
                    basicNeeds = false;

                }
                
            }
            if (!basicNeeds)
            {
                health = Health - 1;
                status(ID, StatusUpdateType.HealthLost);
                if (health <= 0) alive = false;
            }

            foreach (Resource r in GameState.LuxuryConsume.Keys)
            {
                bool luxuryNeeds = true;
                try
                {
                    ChangeResource(r, -GameState.LuxuryConsume[r]);
                }
                catch (NegativeResourcesException)
                {
                    luxuryNeeds = false;

                }
                if (luxuryNeeds)
                {
                    points = Points + 1;
                    status(ID, StatusUpdateType.PointGained);
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
        /// Changes the city resource by the amount, negative or positive. If this would reduce the resource amount to a negative value, 
        /// it is instead reduced to 0, and a NegativeResourcesException is thrown
        /// </summary>
        /// <param name="r">The Resource to be changed</param>
        /// <param name="change">How much, negative or positive, the resource stockpile should be changed by</param>
        internal void ChangeResource(Resource r, int change) 
        {
            resources[r] += change;
            if (resources[r] >= 0) return;
            resources[r] = 0;
            throw new NegativeResourcesException();
        }

        public bool HaveResource(Resource r, int amount)
        {
            if (resources[r] >= amount)
                return true;
            return false;
        }

        public int ResourceAmount(Resource r)
        {
            if (!resources.ContainsKey(r))
            {
                resources.Add(r,0);
            }
            return resources[r];
        }

        public bool HaveResource(Dictionary<Resource, int> resources)
        {
            if (resources.Count == 0) return true;
            return resources.Keys.All(r => this.resources[r] >= resources[r]);
        }

        public bool Alive { get { return alive; }}

        public int Health
        {
            get { return health; }
        }

        public int Points
        {
            get { return points; }
        }

        public Resource NativeResource
        {
            get { return nativeResource; }
        }
    }

    public class NegativeResourcesException : Exception{}
}
