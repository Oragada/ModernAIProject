using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using AITradingProject;
using AITradingProject.Model;

namespace AgentInterface
{
    public abstract class Agent
    {
        //TODO method documentation

        public abstract List<Offer> GetTrades(City city); 
        public abstract bool EvaluateTrade(Offer offer);
        public abstract void TradeCompleted(Offer offer, TradeStatus cc);

        public virtual void PointGained(){ } //TODO consider implementation of producer/consumer
        public virtual void HealthLost(){ }
        public virtual void HealthGained(){ }
    }

    public enum TradeStatus //TODO move to enum file
    {
        Rejected, Successful, Unable
    }
}
