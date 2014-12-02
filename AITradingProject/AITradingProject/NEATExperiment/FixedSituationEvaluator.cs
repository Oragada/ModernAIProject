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
    public class FixedSituationEvaluator : IPhenomeEvaluator<IBlackBox>
    {


        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            Random rand = new Random();

            //Load parameters for evaluation
            TradeGenerator tg = new TradeGenerator(phenome);
            //Create Sample game states
            GameState testState = new GameState(StatusUpdateTest);
            int cCount = testState.getCities().Count;
            double totalFitness = 0.0;
            for (int i = 0; i < cCount; i++)
            {
                City us = testState.getCity(rand.Next(cCount));
                int otherID = us.ID;
                while (otherID == us.ID)
                {
                    otherID = rand.Next(cCount);
                }
                City them = testState.getCity(otherID);

                Dictionary<Resource, int> rs = tg.CreateTrade(us, them);
                Offer o = new Offer(us,testState.GetEdge(us, them),rs);

                if (testState.IsOfferPossible(o)) totalFitness+=1.0;
            }
            
            return new FitnessInfo(totalFitness/cCount,0.0);
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