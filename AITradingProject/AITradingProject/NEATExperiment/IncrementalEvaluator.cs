using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace AITradingProject.NEATExperiment
{
    class IncrementalEvaluator : IPhenomeEvaluator<IBlackBox>
    {
        private FixedSituationEvaluator fsEval;
        private SimulationEvaluator simEval;
        private readonly int crossoverGeneration = 10;
        private readonly double crossoverFitness = 0.9;
            
        public static volatile int genCount;
        public static volatile float avgFitness;


        public IncrementalEvaluator() 
        {
            fsEval = new FixedSituationEvaluator();
            simEval = new SimulationEvaluator();
        }

        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            if (genCount > crossoverGeneration)
            {
                return simEval.Evaluate(phenome);
            }
            return fsEval.Evaluate(phenome);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public ulong EvaluationCount { get; private set; }
        public bool StopConditionSatisfied { get; private set; }
    }
}
