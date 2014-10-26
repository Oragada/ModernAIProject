using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProject.Model
{
    public class GameState
    {
        private const int FoodStart = 5;
        private const int WaterStart = 5;
        private const int DollStart = 5;
        public const int diplomaticPoints = 20;
        private List<City> cities;
        private List<Edge> traderoutes;
        private int turnCount;
        public static Dictionary<Resource, int> consumptions = new Dictionary<Resource, int>();//TODO add consumption in constructor - Troi
        public static List<Resource> availableresources;

        public GameState(int cityNum)
        {
            cities = new List<City>();
            for (int i = 0; i < cityNum; i++)
            {
                cities.Add(new City(
                    new Dictionary<Resource, int>() { { Resource.Food, FoodStart }, { Resource.Water, WaterStart }, { Resource.Dolls, DollStart } }, 
                    GetStartingResource(), 
                    i)
                );

            }
            traderoutes = new List<Edge>();
            for (int i = 0; i < cities.Count; i++)
            {
                for (int j = 0; j < cities.Count; j++)
                {
                    if(j == i) continue;
                    Edge e = new Edge(cities[i], cities[j]);
                    traderoutes.Add(e);
                    cities[i].AddEdge(e);
                    cities[j].AddEdge(e);
                }
            }

            availableresources = new List<Resource>{Resource.Food, Resource.Dolls, Resource.Water}; //added. needs to be more dynamic.

        }
   

        public  City getCity(int cityIndex)//newly added for gameMaster
        {
            return cities[cityIndex];
        }

        /// <summary>
        /// Causes all cities to consume resources, lose health, gain points and advance the turn count by 1
        /// </summary>
        public void AllCitiesConsume()
        {
            foreach (City c in cities.Where(c => c.Alive))
            {
                c.Consume();
            }
            turnCount++;
        }

        public void AllCitiesProduce()
        {
            foreach (City c in cities.Where(c => c.Alive))
            {
                c.Produce();
            }
        }
        //TOD offer an offer to a city

        /// <summary>
        /// This method returns the connecting Edge between two cities
        /// </summary>
        /// <param name="from">The first City</param>
        /// <param name="to">The second City</param>
        /// <returns>The Edge connecting those two cities</returns>
        public Edge GetEdge(City from, City to)
        {
            return traderoutes.First(e => e.other(from) == to);
        }

        /// <summary>
        /// This method checks an offer for whether both cities are currently alive and have the resources needed for the trade.
        /// </summary>
        /// <param name="o">An Offer to be checked</param>
        /// <returns>Whether the cities have the necessary resources</returns>
        public bool IsOfferPossible(Offer o)
        {
            City sender = o.From;
            City reciever = o.E.other(sender);
            if(sender.Alive & reciever.Alive == false) return false;
            //change resources
            
            return o.ResourcesOffered.All(r => sender.HaveResource(r.Key, r.Value)) & 
                   o.ResourcesRequired.All(r => reciever.HaveResource(r.Key, r.Value));
            //Such LINQ, much awesome ;)
        }

        /// <summary>
        /// This method executes a valid and possible Offer on the current gamestate
        /// </summary>
        /// <param name="o">An valid and possible Offer</param>
        public void ExecuteOffer(Offer o)
        {
            City sender = o.From;
            City reciever = o.E.other(sender);
            //change resources
            foreach (KeyValuePair<Resource, int> r in o.ResourcesOffered)
            {
                sender.ChangeResource(r.Key, -r.Value);
                reciever.ChangeResource(r.Key, r.Value);
            }
            foreach (KeyValuePair<Resource, int> r in o.ResourcesRequired)
            {
                reciever.ChangeResource(r.Key, -r.Value);
                sender.ChangeResource(r.Key, r.Value);
            }
        }

        // Save Game State - Troy

        private Resource GetStartingResource()
        {
            int randVal = Utility.RAND.Next(20); //HACK: hardcoded for initial build

            if(randVal < 8) return Resource.Water;
            if(randVal < 16) return Resource.Food;
            return Resource.Dolls;
        }
    }
}
