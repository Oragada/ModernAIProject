using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProjectModel.Model;
using AITradingProject.Agent;


namespace AITradingProject
{
    public class GameMaster
    {
        private Dictionary<int, Agent.Agent> agents;
        private GameState game;

        public GameMaster(int cityNum)
        {
            game = new GameState(cityNum);

            agents = new Dictionary<int, Agent.Agent>();

            for (int i = 0; i < cityNum; i++)
            {
                agents.Add(i, new RandomAgent(10));
            }
        }

        private void RunTurn()
        {
            //TODO
            //Production
            game.AllCitiesProduce();

           //trades
            Dictionary<int, List<Offer>> offers = new Dictionary<int, List<Offer>>();
            ///GetTrades
            foreach(int agentI in agents.Keys)
            {
                City a  = game.getCity(agentI);
                offers.Add(agentI, agents[agentI].GetTrades(a));
                //todo check offers for not exceeding DP points
            }
            ///do trades (what order!?)
            ///doing trades in any order:
            foreach(int agentI in offers.Keys)
            {
                foreach(Offer o in offers[agentI])
                {
                    City to = o.E.other(o.From);
                    if(game.IsOfferPossible(o))
                    {
                        //how to get the other agent??
                        Agent.Agent toAgent = agents[to.getID()];//TODO do we really want this? -troy
                        if(toAgent.EvaluateTrade(o))
                        {
                            game.ExecuteOffer(o);
                            agents[agentI].TradeCompleted(o, TradeStatus.Successful);
                        }
                        else
                        {
                            agents[agentI].TradeCompleted(o, TradeStatus.Rejected);
                        }
                    }
                    else
                    {
                        agents[agentI].TradeCompleted(o, TradeStatus.Unable);
                    }

                }

            }
            //Consumption
            game.AllCitiesConsume();
        }

        public void startGame()
        {
            //TODO Daniel--- what more? - Troi
            int i =0;
            int turns=100;

            while (i < turns)
            {
                i++;
                this.RunTurn();

            }
            //go through turns until something is achieved.

            //function is called from program.
        }
    }

    
}
