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
        public override List<KeyValuePair<int, Dictionary<Resource, int>>> GetOfferProposals(City city)
        {
            //TODO - Daniel
            int pointToGo=GameState.DiplomaticPoints;
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
            foreach(Resource r in GameState.LuxuryConsume.Keys)
            {
                int amount = city.ResourceAmount(r);
                if(amount>0)
                    has.Add(r, amount);
            }
            List<KeyValuePair<int, Dictionary<Resource, int>>> offerProposals = new List<KeyValuePair<int, Dictionary<Resource, int>>>();
            if (need.Count > 0)
                foreach (Edge e in city.getEdges())
                {
                    Dictionary<Resource, int> newNeeds = new Dictionary<Resource, int>();
                    if(e.Other(city).HaveResource(need))
                    {
                        //create offer   
                        Dictionary<Resource, int> offers = new Dictionary<Resource, int>();
                        foreach (Resource r in need.Keys)
                        {
                            
                            int amount = need[r];
                            int totalOffering = 0;
                            while (amount != totalOffering)
                            {
                                if (has.Count < 1)
                                    break;
                                int hasR = has.First().Value;
                                Resource hasResource = has.First().Key;
                                if (hasR+totalOffering > amount)
                                {
                                    if (offers.ContainsKey(hasResource))
                                    {
                                        offers[hasResource]+=amount-totalOffering;
                                    }
                                    else
                                        offers.Add(hasResource, amount - totalOffering);
                                    has[hasResource] -= amount - totalOffering;
                                    break;
                                }
                                else
                                {
                                    totalOffering += hasR;
                                    if (offers.ContainsKey(hasResource))
                                    {
                                        offers[hasResource] += hasR;
                                    }
                                    else
                                        offers.Add(hasResource, hasR);
                                    //offers.Add(hasResource, hasR);
                                    has.Remove(hasResource);
                                    if (!need.ContainsKey(hasResource))
                                    {
                                        newNeeds.Add(hasResource, 1);
                                    }
                                    else
                                        newNeeds[hasResource]++;
                                }
                            }
                        }
                        Dictionary<Resource, int> mfDict = CreateMfDictionary(offers, need);
                        KeyValuePair<int, Dictionary<Resource, int>> offerYouCannotRefuse = new KeyValuePair<int, Dictionary<Resource, int>>(e.Other(city).ID,mfDict);
                        //Offer anOfferYouCannotRefuse = new Offer(city, e, offers, need);
                        offerProposals.Add(offerYouCannotRefuse);
                    }
                    need = need.Concat(newNeeds).ToDictionary(k =>k.Key,v=> v.Value );
                }
            else
            {
                //we want luxury goods!!!


            }
               //if no offer was made for resource
                //make smaller offers to all of them.
            
            return offerProposals;


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
            //Console.WriteLine("Agent {0} gained a point!");
        }

        public override void HealthLost()
        {
            //Console.WriteLine("Agent {0} lost a point of health!");
        }

        public override void HealthGained()
        {
            //Console.WriteLine("Agent {0} gained a point of health!");
        }
    }
}


