﻿using System.Collections.Generic;
using System.Diagnostics;

namespace AITradingProject.Agent.MM_Subsystems
{
    public class DecisionTree<T>
    {
        private Node<T> tree;

        public T GetValue()
        {
            return tree.getValue();
        }
    }

    public abstract class Node<T>
    {
        public abstract T getValue();
        public abstract bool validate();
    }

    public class LeafNode<T> : Node<T>
    {
        private readonly T value;

        public LeafNode(T val)
        {
            value = val;
        }

        public override T getValue()
        {
            return value;
        }

        public override bool validate()
        {
            return true;
        }
    }

    /*public class BranchNode<T> : Node<T>
    {
        private Node<T>[] branches;
        private Condition[] condition;

        public BranchNode()
        {
            
        } 

        public override T getValue()
        {
            for (int i = 0; i < condition.Length; i++)
            {
                if (condition[i]()) return branches[i].getValue();
            }
        }
    }*/

    public class SplitNode<T> : Node<T>
    {
        private readonly Node<T> trueNode;
        private readonly Node<T> falseNode;
        private readonly Condition condition;

        public SplitNode(Node<T> trueN, Node<T> falseN, Condition cond)
        {
            trueNode = trueN;
            falseNode = falseN;
            condition = cond;
        } 

        public override T getValue()
        {
            if (condition())
            {
                return trueNode.getValue();
            }
            return falseNode.getValue();
        }

        public override bool validate()
        {
            var vali = true;

            if (falseNode == null || trueNode == null)
            {
                return false;
            }
            vali &= trueNode.validate();
            vali &= falseNode.validate();
            return vali;
        }
    }

    public delegate bool Condition();
}