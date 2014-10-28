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

        /// <summary>
        /// random stuff spewed out.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public override List<Offer> GetTrades(City city)
        {
            //TODO - Daniel
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
                    if (rand == 1 && e.getWeight()<=pointToGo) 
                    {
                        Dictionary<Resource, int> offer = new Dictionary<Resource, int>();
                        Dictionary<Resource, int> required = new Dictionary<Resource, int>();

                        //we do trade!                        
                        //int amount = Utility.RAND.Next(GameState.availableresources.Count - 1);
                        //what to trade!?                        
                        Resource w = GameState.availableresources[Utility.RAND.Next(GameState.availableresources.Count)];
                        //for which resource!!?
                        offer.Add(w, Utility.RAND.Next(maxTradableResource));

                        Resource t = GameState.availableresources[Utility.RAND.Next(GameState.availableresources.Count)];
                        required.Add(t, Utility.RAND.Next(maxTradableResource));
                        Offer o = new Offer(city, e, offer, required);
                        offers.Add(o);
                        pointToGo -= e.getWeight();

                    }
                }

            }
            return offers;            
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
