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
        //public MasterMind mind;
        public EvalTrade tradeEval;
        public WtoT whoToTrade;
        public TradeGenerator tradeBuild;

        public LeMasterMindAgent()
        {
            tradeBuild = new TradeGenerator("tradegame.champion.xml");
            tradeEval = new EvalTrade(10);
            whoToTrade = new SimpleWtoT();
        }

        public override List<KeyValuePair<int, Dictionary<Resource, int>>> GetOfferProposals(City city)
        {
            List<KeyValuePair<int, Dictionary<Resource, int>>> ops = new List<KeyValuePair<int, Dictionary<Resource, int>>>();
            while (PointsRemaining())
            {
                City tradePartner = whoToTrade.GetTradingPartner(city);

                Dictionary<Resource, int> t = tradeBuild.CreateTrade(city,tradePartner);
                
                ops.Add(new KeyValuePair<int, Dictionary<Resource, int>>(tradePartner.ID,t));
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
