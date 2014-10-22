using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgentInterface
{
    public class GameMaster
    {
        private Dictionary<int, Agent> agents;
        private TradeGame game;

        public GameMaster(int cityNum)
        {
            game = new TradeGame(cityNum);

            agents = new Dictionary<int, Agent>();

            for (int i = 0; i < cityNum; i++)
            {
                agents.Add(i, new RandomAgent());
            }
        }

        private void RunTurn()
        {
            //TODO
            //Production

            //Trade

            //Consumption
        }
    }

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
