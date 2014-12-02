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

        Stack<IPhenomeEvaluator<IBlackBox>> evaluators = new Stack<IPhenomeEvaluator<IBlackBox>>();
        private readonly int crossoverGeneration = 10;
        private readonly double crossoverFitness = 0.9;
        IPhenomeEvaluator<IBlackBox> current;

        
        public static volatile int genCount = 0;
        public static volatile float avgFitness=0;


        public IncrementalEvaluator(bool extended) 
        {
            current = (new FixedSituationEvaluator());
            evaluators.Push(new SimulationEvaluator());
            if(extended)
                evaluators.Push(new SimulationOnlySuccesFullEvaluator());            
            
            if(extended)
                evaluators.Push(new SimulationStayingAliveEvaluator());

        }

        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            if (avgFitness>=crossoverFitness)
            {
                if (evaluators.Count > 0)
                    current = evaluators.Pop();
                else
                {
                    this.StopConditionSatisfied = true;
                }
            }
            return current.Evaluate(phenome);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public ulong EvaluationCount { get; private set; }
        public bool StopConditionSatisfied { get; private set; }
    }
}
