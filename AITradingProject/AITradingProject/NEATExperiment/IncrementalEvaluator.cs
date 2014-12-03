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
        IPhenomeEvaluator<IBlackBox> fsEv = new FixedSituationEvaluator();
        IPhenomeEvaluator<IBlackBox> simEv = new SimulationEvaluator();
        IPhenomeEvaluator<IBlackBox> presEv = new SimulationSelfPresevationEvaluator();       

        private readonly int crossoverGeneration = 10;
        private readonly double crossoverFitness = 0.9;
        IPhenomeEvaluator<IBlackBox> current;
        private static bool done = false;
        private bool extended = false;
        public static volatile int genCount = 0;
        public static volatile float avgFitness=0;
        public static object lockO = new object();

        public IncrementalEvaluator(bool extended) 
        {
            this.extended = extended;
            
            

        }

        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            //double fitness = 0.0;
            FitnessInfo fsEv = this.fsEv.Evaluate(phenome);
            double secfitness = fsEv._auxFitnessArr.First()._value;
            double fitness = fsEv._fitness;
            if (avgFitness >= 0.9)
            {
                FitnessInfo simEvFit = simEv.Evaluate(phenome);
                fitness += simEvFit._fitness;
                secfitness += simEvFit._auxFitnessArr.First()._value;
                if (avgFitness >= (1.2))
                {
                    FitnessInfo presFit = presEv.Evaluate(phenome);
                    fitness += presFit._fitness;

                    secfitness += presFit._auxFitnessArr.First()._value;
                }
            }

            return new FitnessInfo(fitness, secfitness);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public ulong EvaluationCount { get; private set; }
        public bool StopConditionSatisfied { get; private set; }
    }
}
