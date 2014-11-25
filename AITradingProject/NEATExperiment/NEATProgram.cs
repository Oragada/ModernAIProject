using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using log4net.Config;
using NEATExperiment.NEAT;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;

namespace NEATExperiment
{
    internal class NEATProgram
    {
        private static NeatEvolutionAlgorithm<NeatGenome> _ea;
        private const string CHAMPION_FILE = "tradegame_champion.xml";

        private static void Main(string[] args)
        {
            // Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo("log4net.properties"));

            // Experiment classes encapsulate much of the nuts
            // and bolts of setting up a NEAT search.
            NEATTradeGameExperiment experiment = new NEATTradeGameExperiment();

            // Load config XML.
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load("tradegame.config.xml");
            experiment.Initialize("TradeGame", xmlConfig.DocumentElement);

            // Create evolution algorithm and attach update event.
            _ea = experiment.CreateEvolutionAlgorithm();
            _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);

            // Start algorithm (it will run on a background thread).
            _ea.StartContinue();

            // Hit return to quit.
            Console.ReadLine();
        }

        private static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("gen={0:N0} bestFitness={1:N6}",
                _ea.CurrentGeneration, _ea.Statistics._maxFitness));

            // Save the best genome to file
            var doc = NeatGenomeXmlIO.SaveComplete(
                new List<NeatGenome>() {_ea.CurrentChampGenome},
                false);
            doc.Save(CHAMPION_FILE);
        }
    }
}