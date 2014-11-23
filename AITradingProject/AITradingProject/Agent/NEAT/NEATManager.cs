using AITradingProject.Agent.NEAT;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace AITradingProject.Agent.MM_Subsystems
{
    public class NEATTradeGameExperiment : NEATExperiment
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
}