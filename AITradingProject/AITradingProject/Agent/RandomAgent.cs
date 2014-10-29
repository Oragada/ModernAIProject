using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProjectModel;
using AITradingProjectModel.Model;


namespace AITradingProject.Agent
{
    public class RandomAgent : Agent
    {
        private Random rand;
        private int maxTradableResource;


        public RandomAgent(int MaxTradableResource)
        {

            rand = new Random();
            this.maxTradableResource = MaxTradableResource;
        }
        /*
        /// <summary>
        /// random stuff spewed out.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public override List<Offer> GetTrades(City city)
        {
            int pointToGo=GameState.diplomaticPoints;
            List<Offer> offers = new List<Offer>();
            bool throug = false;
            
            {
                foreach(Edge e in city.getEdges())
                {
                    if (pointToGo >= 0 )
                    {
                        break;
                    }
                    int rand=Utility.RAND.Next(2);
                    if (rand == 1 && e.Weight<=pointToGo) 
                    {
                        Dictionary<Resource, int> offer = new Dictionary<Resource, int>();
                        //Dictionary<Resource, int> required = new Dictionary<Resource, int>();

                        //we do trade!                        
                        //int amount = Utility.RAND.Next(GameState.availableresources.Count - 1);
                        //what to trade!?                        
                        Resource w = GameState.availableresources[Utility.RAND.Next(GameState.availableresources.Count)];
                        //for which resource!!?
                        offer.Add(w, Utility.RAND.Next(maxTradableResource));

                        Resource t = GameState.availableresources[Utility.RAND.Next(GameState.availableresources.Count)];
                        offer.Add(t, -Utility.RAND.Next(maxTradableResource));
                        Offer o = new Offer(city, e, offer);
                        offers.Add(o);
                        pointToGo -= e.Weight;

                    }
                }

            }
            return offers;            
        }*/

        public override List<KeyValuePair<int, Dictionary<Resource, int>>> GetOfferProposals(City city)
        {
            int pointToGo = GameState.DiplomaticPoints;
            List<KeyValuePair<int, Dictionary<Resource, int>>> offers = new List<KeyValuePair<int, Dictionary<Resource, int>>>();
            bool throug = false;

            {
                foreach (Edge e in city.getEdges())
                {
                    if (pointToGo >= 0)
                    {
                        break;
                    }
                    int rand = Utility.RAND.Next(2);
                    if (rand == 1 && e.Weight <= pointToGo)
                    {
                        Dictionary<Resource, int> offer = new Dictionary<Resource, int>();

                        //we do trade!                        
                        //int amount = Utility.RAND.Next(GameState.availableresources.Count - 1);
                        //what to trade!?
                        //for which resource!!?
                        offer.Add(GetRandomResource(), Utility.RAND.Next(maxTradableResource));
                        offer.Add(GetRandomResource(), -Utility.RAND.Next(maxTradableResource));
                        //Offer o = new Offer(city, e, offer);
                        offers.Add(new KeyValuePair<int, Dictionary<Resource, int>>(e.Other(city).ID,offer));
                        pointToGo -= e.Weight;

                    }
                }

            }
            return offers;
        }

        private Resource GetRandomResource()
        {
            return GameState.availableresources[Utility.RAND.Next(GameState.availableresources.Count)];
        }

        public override bool EvaluateTrade(Offer offer)
        {
            return rand.Next(2) != 0;
        }

        public override void TradeCompleted(Offer offer,  TradeStatus cc)
        {
        }

        public override void PointGained()
        {
        }

        public override void HealthLost()
        {
        }

        public override void HealthGained()
        {
        }
    }
}
