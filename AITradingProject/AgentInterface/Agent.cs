using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AgentInterface
{
    public abstract class Agent
    {
        public abstract List<Offer> GetTrades(City city);
        public abstract bool EvaluateTrade(Offer offer);
        public abstract void TradeCompleted(Offer offer, CompletionCriteria cc);
        public abstract void PointGained();
        public abstract void HealthLost();
        public abstract void HealthGained();
    }

    public enum CompletionCriteria
    {
        Rejected, Impossible, Successful
    }
}
