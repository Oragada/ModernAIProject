using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AITradingProjectModel.Model;
using AITradingProject.NEATExperiment;
using SharpNeat.Core;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;

namespace AITradingProject.Agent.MM_Subsystems
{
    public class TradeGenerator
    {
        private IBlackBox brain;

        public TradeGenerator(string fileName)
        {
            LoadBrain(fileName);
        }

        public TradeGenerator(IBlackBox brain)
        {
            this.brain = brain;
        }

        private void LoadBrain(string fileName)
        {
            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            NEATTradeGameExperiment _experiment = new NEATTradeGameExperiment();

            // Load config XML.
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load("tradegame.config.xml");
            _experiment.Initialize("TradeGenerator", xmlConfig.DocumentElement);

            NeatGenome genome = null;

            // Try to load the genome from the XML document.
            try
            {
                using (XmlReader xr = XmlReader.Create(fileName))
                    genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)_experiment.CreateGenomeFactory())[0];
            }
            catch (Exception e1)
            {
                Console.WriteLine("Error loading genome from file!\nLoading aborted.\n"
                                  + e1.Message);
                return;
            }

            // Get a genome decoder that can convert genomes to phenomes.
            IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = _experiment.CreateGenomeDecoder();

            // Decode the genome into a phenome (neural network).
            IBlackBox phenome = genomeDecoder.Decode(genome);

            // Set the NEAT player's brain to the newly loaded neural network.
            brain = phenome;
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
                {Resource.Water, output[3]-output[0]},
                {Resource.Food, output[4]-output[1]},
                {Resource.Dolls, output[5]-output[2]}
            };

            return trade;
        }
    }
}