using System.Collections.Generic;
using System.Linq;
using AITradingProject.Agent.MM_Subsystems;
using AITradingProjectModel.Model;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace AITradingProject.NEATExperiment
{
    public class SimulationEvaluator : IPhenomeEvaluator<IBlackBox>
    {
        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            //Load parameters for evaluation
            TradeGenerator tg = new TradeGenerator(phenome);
            //Create Sample game states
            GameMaster gm = new GameMaster(10, tg);
            
            
            double totalFitness = 0;
            double goodTrades = 0;
            double unreasonableTrades = 0;
            double badTrades = 0;
            for (int i = 0; i < 20; i++)
            {
                Dictionary<Offer,TradeStatus> offers = gm.RunTurn();
                foreach(KeyValuePair<Offer, TradeStatus> kv in offers)
                {
                    //Console.WriteLine("Evaluating {0}: {1}", kv.Key, kv.Value);
                    if (TradeStatus.Unable == kv.Value) badTrades++;
                    else if (TradeStatus.Rejected == kv.Value) unreasonableTrades++;
                    else goodTrades++;
                    /*
                    TradeStatus t = kv.Value;
                    
                    if (t == TradeStatus.Successful)
                    {
                        totalFitness += 5;
                    }
                    else if (t == TradeStatus.Rejected)
                    {
                        totalFitness+=2;
                    }*/
                }

            }
            double workingTrades = goodTrades + unreasonableTrades;
            double totalTrades = workingTrades + badTrades;

            totalFitness += ((goodTrades * 2 + unreasonableTrades) / (totalTrades * 2));

            City city = gm.getCities()[0];

            double aliveCities = ((double) gm.getCities().Count(c => c.Alive)/gm.getCities().Count);

            if (!city.Alive)
            {
                //Console.WriteLine("City not alive");
                return new FitnessInfo(0.0, aliveCities);
            }
            
            

            //totalFitness += city.Health/10.0;
            
            //totalFitness += city.Points/20.0;
            //Console.WriteLine("City Health {0}, City Points {1}", city.Health, city.Points);

            return new FitnessInfo(totalFitness,aliveCities);
            /*GameState testState = new GameState(StatusUpdateTest);

            
            
            //Dictionary<Resource, int> t2c2resour = new Dictionary<Resource, int> {{Resource.Food, 10}};
            //Dictionary<Resource, int> t2c1resour = new Dictionary<Resource, int> {{Resource.Water, 10}};
            //For each game state:
            int testFitness = 0;
            //test 1 - two cities have 10 of one basic resource
            Dictionary<Resource, int> trade = tg.CreateTrade(testState.getCity(0), testState.getCity(1));
            //Test legality of suggested trade 
            Offer o = new Offer(testState.getCity(0), testState.GetEdge(testState.getCity(0), testState.getCity(1)), trade);
            if(trade.Values.All(i => i == 0))
            {
                testFitness += 1;
                
            }
            else if(testState.IsOfferPossible(o))
            {
                testFitness += 100;
                testState.ExecuteOffer(o);
                //Test how many rounds the city would survive on these resources
                Dictionary<Resource, int> surviveTurns = new Dictionary<Resource, int>();

                foreach (KeyValuePair<Resource, int> pair in GameState.BasicConsume)
                {
                    surviveTurns.Add(pair.Key,testState.getCity(0).ResourceAmount(pair.Key)/pair.Value);
                }

                testFitness += (surviveTurns.Values.Min()*10);

            }
            
            //Test points for 5 rounds

            totalFitness += testFitness;

            return new FitnessInfo(totalFitness,0.0);*/
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