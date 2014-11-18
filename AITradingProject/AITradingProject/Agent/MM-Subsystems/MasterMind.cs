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
using SharpNeat.Network;
using SharpNeat.Phenomes;
using SharpNeat.Phenomes.NeuralNets;
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
        private ANNHandler neural;

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
        private FastAcyclicNetwork neural;

        TradeGenerator()
        {
            neural = new CyclicNetwork();
            List<Neuron> neus = new List<Neuron>(){new Neuron()};
            List<Connection> conns;
            int inputNeu = 6;
            int outputNeu = 3;
            int tsPerActiv;
        }

        public KeyValuePair<int, Dictionary<Resource, int>> CreateTrade(City us, City tradePartner)
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
                sigIn[i] = resources[i]/maxVal;
            }

            SignalArray isig = new SignalArray(sigIn,0,6);
            //neural.InputSignalArray = isig;
            neural.Activate();
        }
    }

    public class NEATManager : NEATStructure
    {
        public override IPhenomeEvaluator<IBlackBox> PhenomeEvaluator
        {
            get { return new TradeGameEvaluator(); }
        }

        public override int InputCount
        {
            get { return 6; }
        }

        public override int OutputCount
        {
            get { return 3; }
        }

        public override bool EvaluateParents
        {
            get { return true; }
        }
    }

    public class TurnLog
    {
        private Dictionary<KeyValuePair<int, Dictionary<Resource, int>>, TradeStatus> data;

    }

    public class ANNHandler : IBlackBox, IEvalTrack
    {
        private ANN neural;

        public void Activate()
        {
            throw new NotImplementedException();
        }

        public void ResetState()
        {
            throw new NotImplementedException();
        }

        public void ResetTurnEval()
        {
            throw new NotImplementedException();
        }

        public int InputCount { get; private set; }
        public int OutputCount { get; private set; }
        public ISignalArray InputSignalArray { get; private set; }
        public ISignalArray OutputSignalArray { get; private set; }
        public bool IsStateValid { get; private set; }
        public List<int> TurnEval { get; private set; }
        
    }

    internal class ANN
    {
    }

    public interface IEvalTrack
    {
        List<int> TurnEval { get; }
        void ResetTurnEval();
    }
}
