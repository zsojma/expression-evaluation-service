using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NumericExpressionEvaluation.Core.Nodes.Binary;
using NumericExpressionEvaluation.Core.Nodes.Unary;

namespace NumericExpressionEvaluation.Core.Evaluation
{
    internal class AstEvaluator : IAstEvaluator
    {
        public double Evaluate(BinaryNode root)
        {
            return Evaluate(root, PriorityLevel.One);
        }

        private double Evaluate(BinaryNode node, PriorityLevel level)
        {
            if (!node.Rights.Any())
            {
                // evaluate just left side as a result
                return Evaluate(node.Left.Item);
            }

            // evaluate whole level
            var newNode = EvaluateBinaryNodeLevel(node, level);

            // continue with next level if available
            if (++level < PriorityLevel.Result)
            {
                return Evaluate(newNode, level);
            }

            // result is in left side (no right items should be present)
            Debug.Assert(newNode.Rights.Count == 0);
            return Evaluate(newNode.Left.Item);
        }

        private BinaryNode EvaluateBinaryNodeLevel(BinaryNode node, PriorityLevel level)
        {
            if (!node.Rights.Any() || node.Rights.All(x => GetPriorityLevel(x.Operator) != level))
            {
                // nothing to evaluate
                return node;
            }

            // create chain of expressions from the binary node for simpler functionality
            var wholeExpression = new List<BinaryNodeItem>(node.Rights);
            wholeExpression.Insert(0, node.Left);

            for (var i = 0; i < wholeExpression.Count - 1; ++i)
            {
                var op = wholeExpression[i + 1].Operator;
                if (GetPriorityLevel(op) != level)
                {
                    continue;
                }

                // get all items with same level operator
                var levelItems = new List<BinaryNodeItem> { wholeExpression[i], wholeExpression[i + 1] };
                for (var j = i + 2; j < wholeExpression.Count; ++j)
                {
                    var item = wholeExpression[j];

                    // when next operator differs, stop
                    if (GetPriorityLevel(item.Operator) != level)
                    {
                        break;
                    }

                    levelItems.Add(item);
                }

                var resultItem = GetEvaluationOrder(op) == BinaryOperatorEvaluationOrder.Left
                    ? EvaluationBinaryNodeLevelInLeftOrder(levelItems)
                    : EvaluationBinaryNodeLevelInRightOrder(levelItems);

                // replace value in the expression chain
                wholeExpression[i] = resultItem;

                // remove evaluated items from expression
                // skip first which we replaced earlier
                var countToRemove = levelItems.Count - 1;
                for (var j = 0; j < countToRemove; ++j)
                {
                    wholeExpression.RemoveAt(i + 1);
                }
            }

            return new BinaryNode(wholeExpression[0].Item, wholeExpression.Skip(1));
        }

        private BinaryNodeItem EvaluationBinaryNodeLevelInLeftOrder(IReadOnlyCollection<BinaryNodeItem> node)
        {
            if (node.Count <= 1)
            {
                return node.FirstOrDefault();
            }

            var left = node.First();
            foreach (var right in node.Skip(1))
            {
                var value = EvaluateBinaryOperation(left, right);
                left = new BinaryNodeItem(left.Operator, new UnaryValueNode(value));
            }

            return left;
        }

        private BinaryNodeItem EvaluationBinaryNodeLevelInRightOrder(IReadOnlyCollection<BinaryNodeItem> node)
        {
            if (node.Count <= 1)
            {
                return node.FirstOrDefault();
            }

            var left = node.First();
            var right = node.Skip(1).ToArray();
            var value = EvaluateBinaryOperation(left, EvaluationBinaryNodeLevelInRightOrder(right));
            return new BinaryNodeItem(left.Operator, new UnaryValueNode(value));
        }

        private double EvaluateBinaryOperation(BinaryNodeItem left, BinaryNodeItem right)
        {
            return right.Operator switch
            {
                BinaryOperatorType.Add      => Evaluate(left.Item) + Evaluate(right.Item),
                BinaryOperatorType.Subtract => Evaluate(left.Item) - Evaluate(right.Item),
                BinaryOperatorType.Multiply => Evaluate(left.Item) * Evaluate(right.Item),
                BinaryOperatorType.Divide   => Evaluate(left.Item) / Evaluate(right.Item),
                BinaryOperatorType.Power    => Math.Pow(Evaluate(left.Item), Evaluate(right.Item)),
                _ => throw new InvalidOperationException("Unknown operator type!")
            };
        }

        private double Evaluate(IUnaryNode node)
        {
            switch (node)
            {
                case UnaryValueNode valueNode:
                    return valueNode.Value;
                case UnaryPrefixNode prefixNode:
                    return Evaluate(prefixNode);
                case UnaryExpressionNode expressionNode:
                    return Evaluate(expressionNode.Expression);
                default:
                    throw new InvalidOperationException("Unknown type of node object!");
            }
        }

        private double Evaluate(UnaryPrefixNode node)
        {
            var multiply = 1.0;
            if (node.Operator == UnaryOperatorType.Minus)
            {
                multiply = -1.0;
            }

            return multiply * Evaluate(node.Value);
        }

        private PriorityLevel GetPriorityLevel(BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Add => PriorityLevel.Three,
                BinaryOperatorType.Subtract => PriorityLevel.Three,
                BinaryOperatorType.Multiply => PriorityLevel.Two,
                BinaryOperatorType.Divide => PriorityLevel.Two,
                BinaryOperatorType.Power => PriorityLevel.One,
                _ => throw new InvalidOperationException("Unknown binary operator type!")
            };
        }

        private BinaryOperatorEvaluationOrder GetEvaluationOrder(BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Power => BinaryOperatorEvaluationOrder.Right,
                _ => BinaryOperatorEvaluationOrder.Left
            };
        }

        private enum PriorityLevel
        {
            One,
            Two,
            Three,
            Result
        }

        private enum BinaryOperatorEvaluationOrder
        {
            Left,
            Right
        }
    }
}
