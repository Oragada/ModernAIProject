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
            //Production
            game.AllCitiesProduce();

            //trades
            Dictionary<int, List<Offer>> offers = new Dictionary<int, List<Offer>>();
            //GetTrades
            foreach(int agentI in agents.Keys)
            {
                City a  = game.getCity(agentI);
                offers.Add(agentI, agents[agentI].GetTrades(a));
                //todo check offers for not exceeding DP points
            }
            //do trades (what order!?)
            //doing trades in any order:
            foreach(int agentI in offers.Keys)
            {
                foreach(Offer o in offers[agentI])
                {
                    City to = o.E.Other(o.From);
                    if(game.IsOfferPossible(o))
                    {
                        //how to get the other agent??
                        Agent.Agent toAgent = agents[to.ID];
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

        private string CreateTradesText(Dictionary<Offer, TradeStatus> offers)
        {
            StringBuilder strB = new StringBuilder();
            foreach (KeyValuePair<Offer, TradeStatus> ts in offers)
            {
                strB.Append(ts.Key.From.ID + "->" + ts.Key.E.Other(ts.Key.From).ID + ":");

                foreach (KeyValuePair<Resource, int> rs in ts.Key.ResourcesOffered)
                {
                    strB.Append(rs.Key + "(" + rs.Value + ");");
                }
                strB.Append("->");
                foreach (KeyValuePair<Resource, int> rs in ts.Key.ResourcesRequired)
                {
                    strB.Append(rs.Key + "(" + rs.Value + ");");
                }
                strB.Append(" - ");
                strB.Append(ts.Value);
                strB.Append("\n");

            }

            return strB.ToString();
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
