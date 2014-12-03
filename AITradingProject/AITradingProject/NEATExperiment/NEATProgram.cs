using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using log4net.Config;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace AITradingProject.NEATExperiment
{
    public class NEATProgram
    {
        private static NeatEvolutionAlgorithm<NeatGenome> _ea;
        private const string CHAMPION_FILE = "tradegame_champion.xml";
        private const string CHAMPIONBEST_FILE = "tradegame_champion_best.xml";
        private static double fitness = 0;
        private static StringBuilder sb = new StringBuilder();
        public static volatile XmlDocument[] lastGeneration;
        public static bool saveSpecie = true;


        public static void Run()
        {


            sb.AppendLine("New Experiment: FixedSituation, SimulationGoodTrades, SimulationSelfPreservation, Incremental - GEN 250; SPEC:10; ACYC;CROSS - 0.9 - 0.3:");
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

            //logs
            
           // Hit return to quit.
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();

            //write 
            string file = System.IO.Directory.GetCurrentDirectory() + "\\log.txt";

            
            System.IO.File.AppendAllText(file, sb.ToString());
        }

        private static void ea_UpdateEvent(object sender, EventArgs e)
        {
            
            Console.WriteLine("gen={0:N0} bestFitness={1:N3}, avgFitness={2:N3}", _ea.CurrentGeneration, _ea.Statistics._maxFitness, _ea.Statistics._meanFitness);                       
           IncrementalEvaluator.avgFitness = (float)_ea.Statistics._meanFitness;
           IncrementalEvaluator.genCount = (int)_ea.CurrentGeneration;
           
            // Save the best genome to file
            sb.Append(_ea.CurrentGeneration + ";" + _ea.Statistics._maxFitness + ";" +_ea.Statistics._meanFitness+";");
            if (fitness < _ea.Statistics._maxFitness)
            {
                var doc = NeatGenomeXmlIO.SaveComplete(
                    new List<NeatGenome> { _ea.CurrentChampGenome },
                    false);
                doc.Save(CHAMPIONBEST_FILE);
                fitness = _ea.Statistics._maxFitness;
            }
            else
            {
                var doc = NeatGenomeXmlIO.SaveComplete(
                    new List<NeatGenome> { _ea.CurrentChampGenome },
                    false);
                doc.Save(CHAMPION_FILE);
            }


            if (saveSpecie)
            {
                lastGeneration = new XmlDocument[_ea.SpecieList.Count()];
                int i = 0;
                foreach (SharpNeat.Core.Specie<NeatGenome> ng in _ea.SpecieList)
                {

                    NeatGenome a = ng.GenomeList.First(x => x.EvaluationInfo.Fitness >= ng.GenomeList.Max(p => p.EvaluationInfo.Fitness)); //not pretty but gets the best element
                    lastGeneration[i] = NeatGenomeXmlIO.SaveComplete(
                       new List<NeatGenome> { a },
                       false);
                    lastGeneration[i].Save(CHAMPION_FILE.Replace(".xml", i + ".xml"));
                    i++;
                }
            }

            if (_ea.Statistics._maxFitness >= 3)
            {
                string file = System.IO.Directory.GetCurrentDirectory() + "\\autolog.txt";
                System.IO.File.AppendAllText(file, sb.ToString());
                _ea.Stop();
            }

           
        }
    }
}