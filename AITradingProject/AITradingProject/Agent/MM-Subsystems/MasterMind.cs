using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AITradingProjectModel.Model;
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
using System.Collections.Generic;
using System.Linq;

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

    public class EvalTrade
    {
        private int greedLevel = 3; //the larger the greed level, the less greedy it is.
        private Offer theOffer;
        private DecisionTree<bool> dt;
              

        public EvalTrade(int greedLevel)
        {

            if (greedLevel > 0)
            {
                this.greedLevel = greedLevel;
            }

            //1.can we even do the offer. 
            //2. will we loose life if we end with this offer?
            //3. offer fair?
            //4. should we do the offer 
            
                //4.1 we get nessesary items. 
                //4.2 we get luxury goods. 
                //4.3 we get more items to trade with.
            


            Node<bool> falseNode = new LeafNode<bool>(false);
            Node<bool> trueNode = new LeafNode<bool>(true);
            
            SplitNode<bool> part43= new SplitNode<bool>(trueNode, falseNode, new Condition(weGetItemsToTrade));
            SplitNode<bool> part42 = new SplitNode<bool>(trueNode, part43, new Condition(weGetLuxuries));
            SplitNode<bool> part41 = new SplitNode<bool>(trueNode, part42, new Condition(weGetNessesities));
            SplitNode<bool> part411 = new SplitNode<bool>(part42, falseNode, new Condition(weGetNessesities));
            SplitNode<bool> part3 = new SplitNode<bool>(part41, part411, new Condition(fairOffer));
            SplitNode<bool> part2 = new SplitNode<bool>(part3, falseNode, new Condition(willWeLoseLife));

            SplitNode<bool> part1 = new SplitNode<bool>(part2, falseNode, new Condition(canWeDoTheOffer));

            DecisionTree<bool> dt = new DecisionTree<bool>(part1);
            //root - selector
            //typeof offer? -
            //or what do we need.
            //two types. evaluate the type of offer and see if we want such a type of offer.
            //or evaluate what we need and see if the offer does this for us?- first is better, second is easier.

            this.dt = dt;
            


        }


        public bool Evaluate(Offer offer)
        {
            theOffer = offer;
            return dt.GetValue();
        }




        public bool canWeDoTheOffer()
        {
            foreach (Resource wanted in theOffer.ResourcesOffered.Keys)
            {
                int amount = theOffer.ResourcesOffered[wanted];
                if (amount < 0)
                {
                    //losses.Add(wanted, amount);
                    if (theOffer.E.Other(theOffer.From).ResourceAmount(wanted) < (amount * (-1)))
                        return false; //we dont have what you want
                }
                //else
                //    gets.Add(wanted, amount);
            }
            return true;
        }
        public bool willWeLoseLife()
        {//linq ftw.
            return (theOffer.ResourcesOffered.Where(
                        x => x.Value < 0)//all resources we loose
                        .Select(
                            x => x.Key) //which resources is that?
                                .All(
                                    wanted => theOffer.E.Other(theOffer.From).ResourceAmount(wanted) - theOffer.ResourcesOffered[wanted] < GameState.BasicConsume[wanted]));    //will we go below the basic comsume if we accept?
        }


        public bool fairOffer()
        {

            int theOffset = theOffer.ResourcesOffered.Sum(x => x.Value);
            if (theOffset > 0 || theOffset == 0)
                return true;

            return false;
        }

        public bool weGetItemsToTrade()
        {

            int sum = theOffer.ResourcesOffered.Sum(x => x.Value);
            return sum > (sum / greedLevel);

        }

        public bool weGetLuxuries()
        {

            return theOffer.ResourcesOffered.Where( //the resources we get.
                    x => x.Value > 0)
                        .Any(   //are any of these resources needed for luxury consumption and do we actually need it to score points?
                            x => GameState.LuxuryConsume.ContainsKey(x.Key) && GameState.LuxuryConsume[x.Key] > theOffer.E.Other(theOffer.From).ResourceAmount(x.Key));


        }

        public bool weGetNessesities()
        {
            return theOffer.ResourcesOffered.Where( //the resources we get.
                    x => x.Value > 0)
                        .Any(   //are any of these resources needed for consumption and do we actually need those resources to survive next round?
                            x => GameState.BasicConsume.ContainsKey(x.Key) && GameState.BasicConsume[x.Key] > theOffer.E.Other(theOffer.From).ResourceAmount(x.Key));


        }
    }

    public abstract class WtoT
    {
        public abstract City GetTradingPartner(City city);
    }
}
