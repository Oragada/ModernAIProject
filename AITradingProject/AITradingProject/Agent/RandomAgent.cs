using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProject.Model;

namespace AITradingProject.Agent
{
    public class RandomAgent : Agent
    {
        private Random rand;

        public RandomAgent()
        {
            rand = new Random();
        }

        public override List<Offer> GetTrades(City city)
        {
            //TODO
            throw new NotImplementedException();
        }

        public override bool EvaluateTrade(Offer offer)
        {
            return rand.Next(2) != 0;
        }

        public override void TradeCompleted(Offer offer, CompletionCriteria cc)
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
