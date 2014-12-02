using System;
using System.Collections.Generic;
using System.Linq;
using AITradingProject.Agent.MM_Subsystems;
using AITradingProjectModel;
using AITradingProjectModel.Model;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace AITradingProject.NEATExperiment
{
    public class TradeGameEvaluator : IPhenomeEvaluator<IBlackBox>
    {
        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            //Load parameters for evaluation
            TradeGenerator tg = new TradeGenerator(phenome);
            //Create Sample game states
            GameState testState = new GameState(StatusUpdateTest);
            double totalFitness = 0;
            double goodTrades = 0;
            double unreasonableTrades = 0;
            double badTrades = 0;
            for (int i = 0; i < 20; i++)
            {
                City us = testState.getCity(Utility.RAND.Next(20));
                City them = testState.getCity(Utility.RAND.Next(20));

                var rs = tg.CreateTrade(us, them);
                Offer o = new Offer(us,testState.GetEdge(us, them),rs);

                if (testState.IsOfferPossible(o)) totalFitness++;
                

            }
            
            

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
}