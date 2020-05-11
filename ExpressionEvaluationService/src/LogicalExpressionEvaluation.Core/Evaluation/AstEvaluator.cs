using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LogicalExpressionEvaluation.Core.Nodes.Binary;
using LogicalExpressionEvaluation.Core.Nodes.Unary;

namespace LogicalExpressionEvaluation.Core.Evaluation
{
    internal class AstEvaluator : IAstEvaluator
    {
        public bool Evaluate(BinaryNode root, string input)
        {
            return Evaluate(root, input, PriorityLevel.One);
        }

        private bool Evaluate(BinaryNode node, string input, PriorityLevel level)
        {
            if (!node.Rights.Any())
            {
                // evaluate just left side as a result
                return Evaluate(node.Left.Item, input);
            }

            // evaluate whole level
            var newNode = EvaluateBinaryNodeLevel(node, input, level);

            // continue with next level if available
            if (++level < PriorityLevel.Result)
            {
                return Evaluate(newNode, input, level);
            }

            // result is in left side (no right items should be present)
            Debug.Assert(newNode.Rights.Count == 0);
            return Evaluate(newNode.Left.Item, input);
        }

        private BinaryNode EvaluateBinaryNodeLevel(BinaryNode node, string input, PriorityLevel level)
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

                var resultItem = EvaluationBinaryNodeLevelInLeftOrder(levelItems, input);
                
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

        private BinaryNodeItem EvaluationBinaryNodeLevelInLeftOrder(IReadOnlyCollection<BinaryNodeItem> node, string input)
        {
            if (node.Count <= 1)
            {
                return node.FirstOrDefault();
            }

            var left = node.First();
            foreach (var right in node.Skip(1))
            {
                var result = EvaluateBinaryOperation(left, right, input);
                left = new BinaryNodeItem(left.Operator, new UnaryLeafNode(result));
            }

            return left;
        }

        private bool EvaluateBinaryOperation(BinaryNodeItem left, BinaryNodeItem right, string input)
        {
            return right.Operator switch
            {
                BinaryOperatorType.And => Evaluate(left.Item, input) && Evaluate(right.Item, input),
                BinaryOperatorType.Or  => Evaluate(left.Item, input) || Evaluate(right.Item, input),
                _ => throw new InvalidOperationException("Unknown operator type!")
            };
        }

        private bool Evaluate(IUnaryNode node, string input)
        {
            switch (node)
            {
                case UnaryLeafNode leafNode:
                    return Evaluate(leafNode);
                case UnaryValueNode valueNode:
                    return Evaluate(input, valueNode.Value);
                case UnaryPrefixNode prefixNode:
                    return Evaluate(prefixNode, input);
                case UnaryExpressionNode expressionNode:
                    return Evaluate(expressionNode.Expression, input);
                default:
                    throw new InvalidOperationException("Unknown type of input object!");
            }
        }

        private bool Evaluate(UnaryPrefixNode node, string input)
        {
            if (node.Operator == UnaryOperatorType.Not)
            {
                return !Evaluate(node.Value, input);
            }

            return Evaluate(node.Value, input);
        }

        private bool Evaluate(UnaryLeafNode node)
        {
            return node.Value;
        }

        private bool Evaluate(string input, string value)
        {
            return input.Contains(value, StringComparison.InvariantCulture);
        }

        private PriorityLevel GetPriorityLevel(BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Or => PriorityLevel.Two,
                BinaryOperatorType.And => PriorityLevel.One,
                _ => throw new InvalidOperationException("Unknown binary operator type!")
            };
        }

        private enum PriorityLevel
        {
            One,
            Two,
            Result
        }
    }
}