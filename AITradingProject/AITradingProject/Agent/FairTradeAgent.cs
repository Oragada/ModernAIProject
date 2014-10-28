using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProjectModel.Model;

namespace AITradingProject.Agent
{
    public class FairTradeAgent : Agent
    {
        
        


        public FairTradeAgent()
        {
            
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
            Dictionary<Resource, int> consumption = GameState.BasicConsume;
            Dictionary<Resource, int> need = new Dictionary<Resource,int>();
            Dictionary<Resource, int> has = new Dictionary<Resource,int>();
            foreach (Resource r in consumption.Keys)
            {
                if (!city.HaveResource(r, consumption[r])) //need a function to see how much of a given resource there is
                {
                    need.Add(r, consumption[r] - city.ResourceAmount(r));
                }
                else
                {
                    has.Add(r, city.ResourceAmount(r)-consumption[r]);
                }
            }
            List<Offer> offer = new List<Offer>();
            if (need.Count > 1)
                foreach (Edge c in city.getEdges())
                {
                    if (c.other(city).HaveResource(need))
                    {
                        //create offer   
                        Dictionary<Resource, int> offers = new Dictionary<Resource, int>();
                        foreach (Resource r in need.Keys)
                        {
                            int amount = need[r];
                            int totalOffering = 0;
                            while (amount != totalOffering)
                            {
                                int hasR = has.First().Value;
                                if (totalOffering + amount > amount)
                                {
                                    offers.Add(r, amount - totalOffering);
                                    has[r] -= amount - totalOffering;
                                    break;
                                }
                                else
                                {
                                    totalOffering += hasR;
                                    offers.Add(r, hasR);
                                    has.Remove(r);
                                    if (!need.ContainsKey(r))
                                    {
                                        need.Add(r, 1);
                                    }
                                    else
                                        need[r]++;
                                }
                            }
                        }
                        Offer anOfferYouCannotRefuse = new Offer(city, c, offers, need);
                        offer.Add(anOfferYouCannotRefuse);
                    }
                }
            else
            {
                //we want luxury goods!!!


            }
               //if no offer was made for resource
                //make smaller offers to all of them.
            
            return offer;


        }

        public override bool EvaluateTrade(Offer offer)
        {
            return true;
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


