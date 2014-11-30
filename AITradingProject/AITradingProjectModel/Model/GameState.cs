using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProjectModel.Model
{
    public class GameState
    {
        private const int FoodStart = 5;
        private const int WaterStart = 5;
        private const int DollStart = 5;
        public const int DiplomaticPoints = 20;
        private readonly List<City> cities;
        private List<Edge> traderoutes;
        private int turnCount;

        public static readonly Dictionary<Resource, int> BasicConsume = new Dictionary<Resource, int>()
        {
            {Resource.Food, 1},
            {Resource.Water, 1},
            {Resource.Dolls, 0}
        };
        public static readonly Dictionary<Resource, int> LuxuryConsume = new Dictionary<Resource, int>() { { Resource.Dolls, 1 } };
        public static readonly List<Resource> availableresources = ((Resource[])Enum.GetValues(typeof(Resource))).ToList();

        public GameState(int cityNum, StatusUpdate statusUpdate)
        {
            CreateStackOfResources();
            cities = new List<City>();
            for (int i = 0; i < cityNum; i++)
            {
                cities.Add(new City(
                    new Dictionary<Resource, int>() { { Resource.Food, FoodStart }, { Resource.Water, WaterStart }, { Resource.Dolls, DollStart } }, 
                    GetStartingResource(), i, statusUpdate)
                );

            }
            CreateEdges();
        }
        /// <summary>
        /// This GameState is hardcoded for the TradeGameEvaluator. DO NOT USE IN GAME
        /// </summary>
        /// <param name="statusUpdate"></param>
        public GameState(StatusUpdate statusUpdate)
        {
            cities = new List<City>();
            Dictionary<Resource, int> c0resour = new Dictionary<Resource, int> { { Resource.Water, 0 }, { Resource.Food, 10 }, { Resource.Dolls, 0 } };
            cities.Add(new City(c0resour,Resource.Food, 0, statusUpdate));
            Dictionary<Resource, int> c1resour = new Dictionary<Resource, int> { { Resource.Water, 10 }, { Resource.Food, 0 }, { Resource.Dolls, 0 } };
            cities.Add(new City(c1resour, Resource.Food, 1, statusUpdate));


            CreateEdges();
        }

        private void CreateEdges()
        {
            traderoutes = new List<Edge>();
            for (int i = 0; i < cities.Count; i++)
            {
                for (int j = i; j < cities.Count; j++)
                {
                    if (j == i) continue;
                    Edge e = new Edge(cities[i], cities[j]);
                    traderoutes.Add(e);
                    cities[i].AddEdge(e);
                    cities[j].AddEdge(e);
                }
            }
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
            return traderoutes.First(e => e.Other(from) == to);
        }

        /// <summary>
        /// This method checks an offer for whether both cities are currently alive and have the resources needed for the trade.
        /// </summary>
        /// <param name="o">An Offer to be checked</param>
        /// <returns>Whether the cities have the necessary resources</returns>
        public bool IsOfferPossible(Offer o)
        {
            City sender = o.From;
            City reciever = o.E.Other(sender);
            if(sender.Alive & reciever.Alive == false) return false;
            //To help with NEAT
            if (o.ResourcesOffered.All(e => e.Value == 0)) return false;
            //change resources
            
            return o.ResourcesOffered.Where(ro => ro.Value > 0).All(r => sender.HaveResource(r.Key, r.Value)) &
                   o.ResourcesOffered.Where(ro => ro.Value < 0).All(r => reciever.HaveResource(r.Key, -r.Value));
        }

        /// <summary>
        /// This method executes a valid and possible Offer on the current gamestate
        /// </summary>
        /// <param name="o">An valid and possible Offer</param>
        public void ExecuteOffer(Offer o)
        {
            City sender = o.From;
            City reciever = o.E.Other(sender);
            //change resources
            foreach (KeyValuePair<Resource, int> r in o.ResourcesOffered)
            {
                sender.ChangeResource(r.Key, -r.Value);
                reciever.ChangeResource(r.Key, r.Value);
            }
            /*foreach (KeyValuePair<Resource, int> r in o.ResourcesRequired)
            {
                reciever.ChangeResource(r.Key, -r.Value);
                sender.ChangeResource(r.Key, r.Value);
            }*/
        }
        // Save Game State - Troy
        


        public string GetGameStateData()
        {
            StringBuilder strB = new StringBuilder();
            foreach (City c in cities)
            {
                strB.Append(c.ID+" - " + "|");
                foreach (Resource r in (Resource[])Enum.GetValues(typeof(Resource)))
                {
                    strB.Append(r + ":" + c.ResourceAmount(r) + "|");
                }
                strB.Append(string.Format(" - H:{0},P:{1}", c.Health, c.Points));
                strB.Append("\n");
            }

            return strB.ToString();
        }

    Stack<Resource> resourcesToDivide = new Stack<Resource>();
        private Resource GetStartingResource()
        {
            if (resourcesToDivide.Count == 0)
                CreateStackOfResources();


            return resourcesToDivide.Pop();
        }


        private void CreateStackOfResources()
        {
            
            resourcesToDivide.Push(Resource.Dolls);
            resourcesToDivide.Push(Resource.Food);
            resourcesToDivide.Push(Resource.Water);
        }

        public List<City> getCities()
        {
            return cities;
        }
    }
}
