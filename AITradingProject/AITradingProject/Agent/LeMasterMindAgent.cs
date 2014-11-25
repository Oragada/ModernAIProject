using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProject.Agent.MM_Subsystems;
using AITradingProjectModel.Model;

namespace AITradingProject.Agent
{
    public class LeMasterMindAgent : Agent
    {
        public MasterMind mind;
        public EvalTrade tradeEval;
        public WtoT whoToTrade;
        public TradeGenerator tradeBuild;
        public int pointsRemaining;

        public LeMasterMindAgent()
        {
            tradeEval = new EvalTrade(10);
        }

        public override List<KeyValuePair<int, Dictionary<Resource, int>>> GetOfferProposals(City city)
        {
            pointsRemaining = GameState.DiplomaticPoints;
            whoToTrade = new SimpleWtoT(city.getEdges());
            List<KeyValuePair<int, Dictionary<Resource, int>>> ops = new List<KeyValuePair<int, Dictionary<Resource, int>>>();
            while (pointsRemaining>0)
            {

                Edge edg= whoToTrade.GetTradingPartner(city);
                pointsRemaining -= edg.Weight;
                if(pointsRemaining<0)
                {
                    break;
                }
                

                Dictionary<Resource, int> t = tradeBuild.CreateTrade(city,edg.Other(city));
                
                ops.Add(new KeyValuePair<int, Dictionary<Resource, int>>(edg.Other(city).ID,t));
            }

            return ops;
        }


        public override bool EvaluateTrade(Offer offer)
        {
            return tradeEval.Evaluate(offer);
        }

        public override void TradeCompleted(Offer offer, TradeStatus cc)
        {
            throw new NotImplementedException();
        }
    }
}
