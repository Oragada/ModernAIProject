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
    public class SimulationSelfPresevationEvaluator : IPhenomeEvaluator<IBlackBox>
    {
        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            //Load parameters for evaluation
            TradeGenerator tg = new TradeGenerator(phenome);
            //Create Sample game states
            GameMaster gm = new GameMaster(tg, false);


            double totalFitness = 0;            
            for (int i = 0; i < 70; i++)
            { 
               gm.RunTurn();               
            }
          
            City city = gm.getCities()[0];

            double aliveCities = ((double)gm.getCities().Count(c => c.Alive) / gm.getCities().Count);

            if (!city.Alive)
            {
                //Console.WriteLine("City not alive");
                return new FitnessInfo(0.0, aliveCities);
            }
            
            


            totalFitness += city.Health/10.0;

            totalFitness += city.Points/70.0;
            
            return new FitnessInfo(totalFitness/2, aliveCities);
            
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