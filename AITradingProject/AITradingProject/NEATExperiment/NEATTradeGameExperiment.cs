using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace AITradingProject.NEATExperiment
{
    public class NEATTradeGameExperiment : NEATFrame
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
            get { return 6; }
        }

        public override bool EvaluateParents
        {
            get { return true; }
        }
    }
}