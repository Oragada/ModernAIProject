using SharpNeat.Core;
using SharpNeat.Phenomes;

//The code in this class is heavily based on the code from
// http://www.nashcoding.com/2010/07/17/tutorial-evolving-neural-networks-with-sharpneat-2-part-1/
// and any similar elements are to be credited to them  

namespace AITradingProject.NEATExperiment
{
    public class NEATTradeGameExperiment : NEATFrame
    {
        public override IPhenomeEvaluator<IBlackBox> PhenomeEvaluator
        {
            get { return new IncrementalEvaluator(); }
        }

        public override int InputCount
        {
            get { return 9; }
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