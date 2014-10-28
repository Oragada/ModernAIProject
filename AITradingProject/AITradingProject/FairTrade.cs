using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProject.Model;

namespace AITradingProject.Agent
{
    public class FairTradeAgent : Agent
    {
        private Random rand;
        


        public FairTradeAgent()
        {
            rand = new Random();         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public override List<Offer> GetTrades(City city)
        {
            //TODO - Daniel
            int pointToGo=GameState.diplomaticPoints;
            Dictionary<Resource, int> consumption = GameState.consumptions;
            Dictionary<Resource, int> need = new Dictionary<Resource,int>();
            Dictionary<Resource, int> has = new Dictionary<Resource,int>();
            foreach (Resource r in consumption.Keys)
            {
                if (!city.HaveResource(r, consumption[r])) //need a function to see how much of a given resource there is
                {
                    need.Add(r, consumption[r] - city.getResource(r));
                }
                else
                {
                    has.Add(r, city.getResource(r)-consumption[r]);
                }
            }
            foreach (Resource r in need.Keys)
            {
                foreach (Edge c in city.getEdges())
                {
                    if (c.other(city).HaveResource(need))
                    {
                        //create offer
                    }                   
                }
               //if no offer was made for resource
                //make smaller offers to all of them.
            }
            return null;


        }

        public override bool EvaluateTrade(Offer offer)
        {
            return false;
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
