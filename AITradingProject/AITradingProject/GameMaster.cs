using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using AITradingProject.Agent.MM_Subsystems;
using AITradingProjectModel.Model;
using AITradingProject.Agent;
using System.IO;


namespace AITradingProject
{
    public class GameMaster
    {
        private Dictionary<int, Agent.Agent> agents;
        private GameState game;

        public GameMaster(int cityNum)
        {
            game = new GameState(cityNum, CityStatusUpdate);

            agents = new Dictionary<int, Agent.Agent>();

            for (int i = 0; i < cityNum; i++)
            {
                agents.Add(i, new FairTradeAgent());                
            }
        }

        public GameMaster(int cityNum, TradeGenerator tg)
        {
            game = new GameState(cityNum, CityStatusUpdate);

            agents = new Dictionary<int, Agent.Agent>();

            for (int i = 0; i < cityNum; i++)
            {
                agents.Add(i, new LeMasterMindAgent(tg));
            }
        }

        public List<City> getCities()
        {
            return game.getCities();
        }

        public Dictionary<Offer,TradeStatus> RunTurn()
        {
            //Production
            game.AllCitiesProduce();

            //trades
            Dictionary<int, List<Offer>> offers = new Dictionary<int, List<Offer>>();
            //GetTrades
            foreach(int agentI in agents.Keys)
            {
                City a  = game.getCity(agentI);
                ////List<Offer> agentOffers = agents[agentI].GetTrades(a);
                List<Offer> agentOffers = ConvertAgentProposals(agentI, agents[agentI].GetOfferProposals(a));
                ////agentOffers = agentOffers.Where(o => o.From)
                offers.Add(agentI, agentOffers);
                //todo check offers for not exceeding DP points
            }
            //do trades (what order!?)
            //doing trades in any order:
            Dictionary<Offer, TradeStatus> tradeTrackingList = new Dictionary<Offer, TradeStatus>();
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
                            tradeTrackingList.Add(o, TradeStatus.Successful);
                        }
                        else
                        {
                            agents[agentI].TradeCompleted(o, TradeStatus.Rejected);
                            tradeTrackingList.Add(o, TradeStatus.Rejected);
                        }
                    }
                    else
                    {
                        agents[agentI].TradeCompleted(o, TradeStatus.Unable);
                        tradeTrackingList.Add(o, TradeStatus.Unable);
                    }

                }

            }
            //Console.WriteLine(tradeTrackingList.Count);

            //Print City Status to file
            string gameState = game.GetGameStateData();
            string trades = CreateTradesText(tradeTrackingList);
            WriteToFile(gameState + trades + "\n\n", "filedump.txt");

            //Consumption
            game.AllCitiesConsume();
            return tradeTrackingList;
        }

        private void WriteToFile(string s, string filedumpPath)
        {
            //File.AppendAllText(filedumpPath,s);
        }

        private List<Offer> ConvertAgentProposals(int agentI, List<KeyValuePair<int, Dictionary<Resource, int>>> getOfferProposals)
        {
            if(getOfferProposals == null) return new List<Offer>();
            City thisAgentCity = game.getCity(agentI);

            List<Offer> list = new List<Offer>();
            foreach (KeyValuePair<int, Dictionary<Resource, int>> offerProposal in getOfferProposals)
            {
                if(offerProposal.Value == null) continue; //KeyValuePair is not nullable, since it is a struct
                if (!agents.ContainsKey(offerProposal.Key) || agentI == offerProposal.Key) continue;
                Edge tradeEdge = game.GetEdge(thisAgentCity, game.getCity(offerProposal.Key));
                //Dictionary<Resource, int> offeredResources = offerProposal.Value.Where(ra => ra.Value > 0).ToDictionary(v => v.Key, v => v.Value);
                //Dictionary<Resource, int> requestedResources = offerProposal.Value.Where(ra => ra.Value < 0).ToDictionary(v => v.Key, v => v.Value);
                list.Add(new Offer(thisAgentCity, tradeEdge, offerProposal.Value));
            }
            return list;
        }

        private string CreateTradesText(Dictionary<Offer, TradeStatus> offers)
        {
            StringBuilder strB = new StringBuilder();
            foreach (KeyValuePair<Offer, TradeStatus> ts in offers)
            {
                strB.Append(ts.Key.From.ID + "->" + ts.Key.E.Other(ts.Key.From).ID + ":");

                foreach (KeyValuePair<Resource, int> rs in ts.Key.ResourcesOffered.Where(ro => ro.Value > 0))
                {
                    strB.Append(rs.Key + "(" + rs.Value + ");");
                }
                strB.Append("->");
                foreach (KeyValuePair<Resource, int> rs in ts.Key.ResourcesOffered.Where(ro => ro.Value < 0))
                {
                    strB.Append(rs.Key + "(" + -rs.Value + ");");
                }
                strB.Append(" - ");
                strB.Append(ts.Value);
                strB.Append("\n");

            }

            return strB.ToString();
        }

        public void startGame()
        {
            //TODO Daniel--- what more? - Troy
            int i =0;
            int turns=100;

            while (i < turns)
            {
                i++;
                RunTurn();

            }
            //go through turns until something is achieved.

            //function is called from program.
        }

        private void CityStatusUpdate(int id, StatusUpdateType type)
        {
            switch (type)
            {
                case StatusUpdateType.HealthGained:
                    agents[id].HealthGained();
                    break;
                case StatusUpdateType.HealthLost:
                    agents[id].HealthLost();
                    break;
                case StatusUpdateType.PointGained:
                    agents[id].PointGained();
                    break;
                default:
                    throw new Exception("Unknown StatusUpdateType in GameMaster.CityStatusUpdate");
            }
        }
    }


}
