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
        public NEATManager neat;

        public LeMasterMindAgent()
        {
            tradeEval = new EvalTrade();
        }

        public override List<KeyValuePair<int, Dictionary<Resource, int>>> GetOfferProposals(City city)
        {
            List<KeyValuePair<int, Dictionary<Resource, int>>> ops = new List<KeyValuePair<int, Dictionary<Resource, int>>>();
            while (PointsRemaining())
            {
                int tradePartner = whoToTrade.GetTradingPartner();

                KeyValuePair<int, Dictionary<Resource, int>> t = tradeBuild.CreateTrade(tradePartner);
                
                ops.Add(t);
            }

            return ops;


        }

        private bool PointsRemaining()
        {
            throw new NotImplementedException();
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
