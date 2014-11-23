using System;
using System.Collections.Generic;
using System.Linq;
using AITradingProjectModel.Model;
using SharpNeat.Phenomes;

namespace AITradingProject.Agent.MM_Subsystems
{
    public class TradeGenerator
    {
        private IBlackBox brain;

        public TradeGenerator(IBlackBox b)
        {
            brain = b;
        }

        public Dictionary<Resource, int> CreateTrade(City us, City tradePartner)
        {
            var resources = new List<double>()
            {
                us.ResourceAmount(Resource.Water),
                us.ResourceAmount(Resource.Food),
                us.ResourceAmount(Resource.Dolls),
                tradePartner.ResourceAmount(Resource.Water),
                tradePartner.ResourceAmount(Resource.Food),
                tradePartner.ResourceAmount(Resource.Dolls)
            };

            double maxVal = resources.Max();

            double[] sigIn = new double[resources.Count];

            for (int i = 0; i < sigIn.Length; i++)
            {
                brain.InputSignalArray[i] = resources[i]/maxVal;
            }

            brain.Activate();

            List<int> output = new List<int>();

            for (int i = 0; i < brain.OutputCount; i++)
            {
                output.Add((int)Math.Round(brain.OutputSignalArray[i]*resources.Max()));
            }

            Dictionary<Resource, int> trade = new Dictionary<Resource, int>()
            {
                {Resource.Water, output[0]},
                {Resource.Food, output[1]},
                {Resource.Dolls, output[2]}
            };

            return trade;
        }
    }
}