using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using AITradingProject;
using AITradingProjectModel.Model;

namespace AITradingProject.Agent
{
    public abstract class Agent
    {
        
        /// <summary>
        /// This function is called at each turn, and expects the trades the agent wishes to perform on it's provided city        
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        //public abstract List<Offer> GetTrades(City city);

        public abstract List<KeyValuePair<int, Dictionary<Resource, int>>> GetOfferProposals(City city); 
        /// <summary>
        /// This function is called at the end of each turn where all offers are being evaluated one at a time in order.
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        public abstract bool EvaluateTrade(Offer offer);

        /// <summary>
        /// All offers sent by this agent will be returned here with a status describing whether it was able to be performed, was accepted or rejected
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="cc"></param>
        public abstract void TradeCompleted(Offer offer, TradeStatus cc);


        /// <summary>
        /// whenever the agent's city has gained a point this function is called
        /// </summary>
        public virtual void PointGained(){ } //TODO consider implementation of producer/consumer - Troy for later

        /// <summary>
        /// whenever the city loses a healthpoint this function is called
        /// </summary>
        public virtual void HealthLost(){ }

        /// <summary>
        /// Whenever a health point is gained this function is called.
        /// </summary>
        public virtual void HealthGained(){ }

        /// <summary>
        /// This helper method takes two lists of resources, one being offered and the other requested in trade 
        /// and converts them into the combined Dictionarys that GetOfferProposals rewuire
        /// </summary>
        /// <param name="offering">The list of offered resources, that the agent wants to give away in trade</param>
        /// <param name="requiring">The list of requested resources, that the agent wants to get out of the trade</param>
        /// <returns>The combined MF dictionary of resources</returns>
        public Dictionary<Resource, int> CreateMfDictionary(Dictionary<Resource, int> offering,
            Dictionary<Resource, int> requiring)
        {
            Dictionary<Resource, int> mfDict = new Dictionary<Resource, int>(offering);
            foreach (KeyValuePair<Resource, int> pair in requiring)
            {
                if (mfDict.ContainsKey(pair.Key))
                {
                    mfDict[pair.Key] -= pair.Value;
                    continue;
                }
                mfDict.Add(pair.Key, -pair.Value);

            }

            return mfDict;
        }
    }
}
