using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SharpNeat;
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
        public TradeGenerator tg;

        public MasterMind(IBlackBox tgBrain)
        {
            tg = new TradeGenerator(tgBrain);
        }

        private DecisionTree<int> dt;
    }

    /*public class EvalTrade
    {
        private ANNHandler neural;

        public bool Evaluate(Offer offer)
        {
            throw new NotImplementedException();
        }
    }*/

    public abstract class WtoT
    {
        public abstract int GetTradingPartner();
    }
}
