using System;
using System.Collections.Generic;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace AITradingProject.Agent.MM_Subsystems
{
    public class TradeGameEvaluator : IPhenomeEvaluator<IBlackBox>
    {
        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            if(phenome.GetType() != typeof (IEvalTrack))
            {
                return FitnessInfo.Zero;
            }

            IEvalTrack eval = (IEvalTrack) phenome;

            double val = CalcFitness(eval.TurnEval);

            eval.ResetTurnEval();

            return new FitnessInfo(val, 0.0);
        }

        private double CalcFitness(List<int> turnEval)
        {
            int i = 0;

            turnEval.ForEach(a => i+=a);

            return ((double)i/turnEval.Count);
        }

        public void Reset()
        {
            //None
        }

        public ulong EvaluationCount { get; private set; }
        public bool StopConditionSatisfied { get; private set; }
    }
}