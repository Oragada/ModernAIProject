using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProjectModel.Model;

namespace AITradingProject.Agent.MM_Subsystems
{
    public class MasterMind
    {
        private DecisionTree dt;
    }

    public class EvalTrade
    {
        private ANN neural;

        public bool Evaluate(Offer offer)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class WtoT
    {
        public abstract int GetTradingPartner();
    }

    public class TradeGenerator
    {
        private ANN neural;

        public KeyValuePair<int, Dictionary<Resource, int>> CreateTrade(int tradePartner)
        {
            throw new NotImplementedException();
        }
    }

    public class NEATManager
    {
        public ANN AdjustNetwork(ANN network, List<TurnLog> results)
        {
            throw new NotImplementedException();
        }
    }

    public class TurnLog
    {
        private Dictionary<KeyValuePair<int, Dictionary<Resource, int>>, TradeStatus> data;

    }

    internal class DecisionTree
    {
    }

    public class ANN
    {
    }
    

}
