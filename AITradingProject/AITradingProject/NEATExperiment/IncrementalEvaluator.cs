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
        IPhenomeEvaluator<IBlackBox> simSucEv = new SimulationOnlySuccesFullEvaluator();
        IPhenomeEvaluator<IBlackBox> simSaEv = new SimulationStayingAliveEvaluator();
        Stack<IPhenomeEvaluator<IBlackBox>> evaluators = new Stack<IPhenomeEvaluator<IBlackBox>>();
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
           double fitness=fsEv._fitness;
            if (fitness >= crossoverFitness)
            {
                FitnessInfo simEvFit = simEv.Evaluate(phenome);
                fitness += simEvFit._fitness;
                secfitness+=simEvFit._auxFitnessArr.First()._value;
                if (extended && simEvFit._fitness >= crossoverFitness)
                {
                    
                    
                        FitnessInfo simSucFit = simSucEv.Evaluate(phenome);
                        fitness += simSucFit._fitness;
                        secfitness+=simSucFit._auxFitnessArr.First()._value;
                        if (simSucFit._fitness >= crossoverFitness)
                        {
                            FitnessInfo simSaFit = simSaEv.Evaluate(phenome);
                            secfitness =simSaFit._auxFitnessArr.First()._value;
                            fitness += simSaFit._fitness;
                        }                    
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
