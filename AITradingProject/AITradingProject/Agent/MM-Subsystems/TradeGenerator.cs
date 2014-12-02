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

            double maxVal = 20.0;

            var resources = new List<double>()
            {
                us.ResourceAmount(Resource.Water)> maxVal ? 1.0 : us.ResourceAmount(Resource.Water)/maxVal,
                us.ResourceAmount(Resource.Food)> maxVal ? 1.0 :us.ResourceAmount(Resource.Food)/maxVal,
                us.ResourceAmount(Resource.Dolls)> maxVal ? 1.0 :us.ResourceAmount(Resource.Dolls)/maxVal,
                tradePartner.ResourceAmount(Resource.Water)> maxVal ? 1.0 :tradePartner.ResourceAmount(Resource.Water)/maxVal,
                tradePartner.ResourceAmount(Resource.Water)> maxVal ? 1.0 :tradePartner.ResourceAmount(Resource.Food)/maxVal,
                tradePartner.ResourceAmount(Resource.Water)> maxVal ? 1.0 :tradePartner.ResourceAmount(Resource.Dolls)/maxVal,
                us.NativeResource == Resource.Water ? 1.0 : 0.0,
                us.NativeResource == Resource.Food ? 1.0 : 0.0,
                us.NativeResource == Resource.Dolls ? 1.0 : 0.0
 
            };

            for (int i = 0; i < resources.Count; i++)
            {
                brain.InputSignalArray[i] = resources[i];

            }

            brain.Activate();

            List<int> output = new List<int>();

            for (int i = 0; i < brain.OutputCount; i++)
            {
                output.Add((int)Math.Round(brain.OutputSignalArray[i]*maxVal));
            }

            Dictionary<Resource, int> trade = new Dictionary<Resource, int>()
            {
                {Resource.Water, output[0]-output[3]},
                {Resource.Food, output[1]-output[4]},
                {Resource.Dolls, output[2]-output[5]}
            };

            return trade;
        }
    }
}