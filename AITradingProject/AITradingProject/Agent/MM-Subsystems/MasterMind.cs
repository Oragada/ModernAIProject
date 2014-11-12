using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AITradingProjectModel.Model;
using SharpNeat;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.DistanceMetrics;
using SharpNeat.Domains;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using SharpNeat.SpeciationStrategies;
using System.Threading.Tasks;

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

    public class NEATManager : INeatExperiment 
    {

        NeatEvolutionAlgorithmParameters _eaParams;
        NeatGenomeParameters _neatGenomeParams;
        public int SpiciesCount { get; private set; }
        public NetworkActivationScheme activationScheme;
        string _complexityRegulationStr;
        int? _complexityThreshold;
        ParallelOptions _parallelOptions;

        public IPhenomeEvaluator<IBlackBox> PhenomeEvaluator {
            get {return new TradeGameEvaluator();}
        }
        public int InputCount { get { return 9; } }
        public int OutputCount { get { return 9; } }
        public bool EvaluateParents { get { return true; } }

        public void Initialize(string name, XmlElement xmlConfig)
        {
            this.Name = name;
            this.DefaultPopulationSize = XmlUtils.GetValueAsInt(xmlConfig, "PopulationSize");
            this.SpiciesCount = XmlUtils.GetValueAsInt(xmlConfig, "SpecieCount");
            this.activationScheme = ExperimentUtils.CreateActivationScheme(xmlConfig, "Activation");
            _complexityRegulationStr = XmlUtils.TryGetValueAsString(xmlConfig, "ComplexityRegulationStrategy");
            _complexityThreshold = XmlUtils.TryGetValueAsInt(xmlConfig, "ComplexityThreshold");
            this.Description = XmlUtils.TryGetValueAsString(xmlConfig, "Description");
            _parallelOptions = ExperimentUtils.ReadParallelOptions(xmlConfig);

            _eaParams = new NeatEvolutionAlgorithmParameters();
            _eaParams.SpecieCount = this.SpiciesCount;
            _neatGenomeParams = new NeatGenomeParameters();
        }

        public List<NeatGenome> LoadPopulation(XmlReader xr)
        {
            NeatGenomeFactory genomeFactory = (NeatGenomeFactory)CreateGenomeFactory();
            return NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, genomeFactory);
        }

        public void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList)
        {
            // Writing node IDs is not necessary for NEAT.
            NeatGenomeXmlIO.WriteComplete(xw, genomeList, false);
        }

        public IGenomeDecoder<NeatGenome, SharpNeat.Phenomes.IBlackBox> CreateGenomeDecoder()
        {
            return new NeatGenomeDecoder(activationScheme);
        }

        public IGenomeFactory<NeatGenome> CreateGenomeFactory()
        {
            return new NeatGenomeFactory(InputCount, OutputCount, _neatGenomeParams);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm()
        {
            return CreateEvolutionAlgorithm(DefaultPopulationSize);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize)
        {
            // Create a genome2 factory with our neat genome2 parameters object and the appropriate number of input and output neuron genes.
            IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();

            // Create an initial population of randomly generated genomes.
            List<NeatGenome> genomeList = genomeFactory.CreateGenomeList(populationSize, 0);

            // Create evolution algorithm.
            return CreateEvolutionAlgorithm(genomeFactory, genomeList);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList)
        {
            // Create distance metric. Mismatched genes have a fixed distance of 10; for matched genes the distance is their weigth difference.
            IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
            ISpeciationStrategy<NeatGenome> speciationStrategy = new ParallelKMeansClusteringStrategy<NeatGenome>(distanceMetric, _parallelOptions);

            // Create complexity regulation strategy.
            IComplexityRegulationStrategy complexityRegulationStrategy = ExperimentUtils.CreateComplexityRegulationStrategy(_complexityRegulationStr, _complexityThreshold);

            // Create the evolution algorithm.
            NeatEvolutionAlgorithm<NeatGenome> ea = new NeatEvolutionAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);

            // Create genome2 decoder.
            IGenomeDecoder<NeatGenome, SharpNeat.Phenomes.IBlackBox> genomeDecoder = CreateGenomeDecoder();

            // Create a genome2 list evaluator. This packages up the genome2 decoder with the genome2 evaluator.
            IGenomeListEvaluator<NeatGenome> genomeListEvaluator = new ParallelGenomeListEvaluator<NeatGenome, SharpNeat.Phenomes.IBlackBox>(genomeDecoder, PhenomeEvaluator, _parallelOptions);

            // Wrap the list evaluator in a 'selective' evaulator that will only evaluate new genomes. That is, we skip re-evaluating any genomes
            // that were in the population in previous generations (elite genomes). This is determiend by examining each genome2's evaluation info object.
            if (!EvaluateParents)
                genomeListEvaluator = new SelectiveGenomeListEvaluator<NeatGenome>(genomeListEvaluator,
                                         SelectiveGenomeListEvaluator<NeatGenome>.CreatePredicate_OnceOnly());

            // Initialize the evolution algorithm.
            ea.Initialize(genomeListEvaluator, genomeFactory, genomeList);

            // Finished. Return the evolution algorithm
            return ea;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int DefaultPopulationSize { get; private set; }
        public NeatEvolutionAlgorithmParameters NeatEvolutionAlgorithmParameters { get; private set; }
        public NeatGenomeParameters NeatGenomeParameters { get; private set; }
    }

    public class TradeGameEvaluator : IPhenomeEvaluator<SharpNeat.Phenomes.IBlackBox>
    {
        public FitnessInfo Evaluate(SharpNeat.Phenomes.IBlackBox phenome)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public ulong EvaluationCount { get; private set; }
        public bool StopConditionSatisfied { get; private set; }
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
