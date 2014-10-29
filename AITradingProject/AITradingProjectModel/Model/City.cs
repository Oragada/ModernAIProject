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
            if (!basicNeeds) health = Health - 1;
            status(ID,StatusUpdateType.HealthLost);

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
        /// Changes the city resource by the amount.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="change"></param>
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
            return resources[r];
        }

        public bool HaveResource(Dictionary<Resource, int> resources)
        {
            if (resources.Count == 0) return true;
            return resources.Keys.All(r => this.resources[r] >= resources[r]);
        }

        public bool Alive { get { return alive; }}

        internal int Health
        {
            get { return health; }
        }

        internal int Points
        {
            get { return points; }
        }
    }

    public class NegativeResourcesException : Exception{}
}
