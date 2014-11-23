using System.Collections.Generic;
using System.Linq;
using AITradingProject.Agent.MM_Subsystems;
using AITradingProjectModel.Model;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace NEATExperiment.NEAT
{
    public class TradeGameEvaluator : IPhenomeEvaluator<IBlackBox>
    {
        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            //Load parameters for evaluation
            TradeGenerator tg = new TradeGenerator(phenome);
            //Create Sample game states
            GameState testState = new GameState(StatusUpdateTest);

            double totalFitness = 0.0;
            
            //Dictionary<Resource, int> t2c2resour = new Dictionary<Resource, int> {{Resource.Food, 10}};
            //Dictionary<Resource, int> t2c1resour = new Dictionary<Resource, int> {{Resource.Water, 10}};
            //For each game state:
            int testFitness = 0;
            //test 1 - each city have 10 of one basic resource
            Dictionary<Resource, int> trade = tg.CreateTrade(testState.getCity(0), testState.getCity(1));
                //Test legality of suggested trade
            if(testState.IsOfferPossible(new Offer(testState.getCity(0),
                testState.getCity(0).getEdges().First(e => e.Other(testState.getCity(0)) == testState.getCity(1)), trade)))
            {
                testFitness += -100;
            }
            //Test survivability for 5 rounds
            
            //Test points for 5 rounds

            totalFitness += testFitness;

            return new FitnessInfo(totalFitness,0.0);
        }

        private void StatusUpdateTest(int cityid, StatusUpdateType type)
        {
            //Do nothing
        }

        public void Reset()
        {
            //None
        }

        public ulong EvaluationCount { get; private set; }
        public bool StopConditionSatisfied { get; private set; }
    }

    public class TestGameState
    {
        public City Us { get; set; }
        public City Other { get; set; }

        public TestGameState()
        {
            
        }
    }
}