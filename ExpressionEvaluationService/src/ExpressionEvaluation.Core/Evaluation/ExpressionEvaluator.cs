using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExpressionEvaluation.Core.ExpressionNodes;

namespace ExpressionEvaluation.Core.Evaluation
{
    internal class ExpressionEvaluator : IExpressionEvaluator
    {
        public decimal Evaluate(BinaryNode input)
        {
            return Evaluate(input, PriorityLevel.One);
        }

        private decimal Evaluate(BinaryNode input, PriorityLevel level)
        {
            var rightItem = input.Rights.FirstOrDefault();
            if (rightItem == null)
            {
                // evaluate just left side as a result
                return Evaluate(input.Left.Item);
            }

            // evaluate whole level
            var newNode = EvaluateBinaryLevel(input, level);

            // continue with next level if available
            if (++level < PriorityLevel.Result)
            {
                return Evaluate(newNode, level);
            }

            // result is in left side (no right items should be present)
            Debug.Assert(newNode.Rights.Count == 0);
            return Evaluate(newNode.Left.Item);
        }

        private BinaryNode EvaluateBinaryLevel(BinaryNode input, PriorityLevel level)
        {
            if (!input.Rights.Any() || input.Rights.All(x => GetPriorityLevel(x.Operator) != level))
            {
                // nothing to evaluate
                return input;
            }

            // create chain of expressions from the binary node for simpler functionality
            var wholeExpression = new List<BinaryNodeItem>(input.Rights);
            wholeExpression.Insert(0, input.Left);

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
                    ? EvaluationBinaryLevelInLeftOrder(levelItems)
                    : EvaluationBinaryLevelInRightOrder(levelItems);

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

        private BinaryNodeItem EvaluationBinaryLevelInLeftOrder(IReadOnlyCollection<BinaryNodeItem> input)
        {
            if (input.Count <= 1)
            {
                return input.FirstOrDefault();
            }

            var left = input.First();
            var right = input.Skip(1).ToArray();
            foreach (var item in right)
            {
                var value = EvaluateBinaryOperation(left, item);
                left = new BinaryNodeItem(left.Operator, new UnaryValueNode(value));
            }

            return left;
        }

        private BinaryNodeItem EvaluationBinaryLevelInRightOrder(IReadOnlyCollection<BinaryNodeItem> input)
        {
            if (input.Count <= 1)
            {
                return input.FirstOrDefault();
            }

            var left = input.First();
            var right = input.Skip(1).ToArray();
            var value = EvaluateBinaryOperation(left, EvaluationBinaryLevelInRightOrder(right));
            return new BinaryNodeItem(left.Operator, new UnaryValueNode(value));
        }

        private decimal EvaluateBinaryOperation(BinaryNodeItem left, BinaryNodeItem right)
        {
            return right.Operator switch
            {
                BinaryOperatorType.Add      => Evaluate(left.Item) + Evaluate(right.Item),
                BinaryOperatorType.Subtract => Evaluate(left.Item) - Evaluate(right.Item),
                BinaryOperatorType.Multiply => Evaluate(left.Item) * Evaluate(right.Item),
                BinaryOperatorType.Divide   => Evaluate(left.Item) / Evaluate(right.Item),
                BinaryOperatorType.Power    => (decimal)Math.Pow((double)Evaluate(left.Item), (double)Evaluate(right.Item)),
                _ => throw new InvalidOperationException("Unknown operator type!")
            };
        }

        private bool TryEvaluatePower(UnaryNode left, ref List<BinaryNodeItem> rights, out UnaryNode result)
        {
            if (rights.Any() && rights[0].Operator == BinaryOperatorType.Power)
            {
                // power is right associative, try evaluate power in right expression first
                var childList = new List<BinaryNodeItem>(rights.Skip(1));
                if (!TryEvaluatePower(rights[0].Item, ref childList, out var right))
                {
                    right = new UnaryValueNode(Evaluate(rights[0].Item));
                    rights = childList;
                }

                result = new UnaryValueNode((decimal)Math.Pow((double)Evaluate(left), (double)Evaluate(right)));
                return true;
            }

            result = null;
            return false;
        }

        private decimal Evaluate(UnaryNode input)
        {
            switch (input)
            {
                case UnaryValueNode valueNode:
                    return valueNode.Value;
                case UnaryPrefixNode prefixNode:
                    return Evaluate(prefixNode);
                case UnaryExpressionNode expressionNode:
                    return Evaluate(expressionNode.Expression);
                default:
                    throw new InvalidCastException("Unknown type of input object!");
            }
        }

        private decimal Evaluate(UnaryPrefixNode input)
        {
            var multiply = 1.0m;
            if (input.Operator == UnaryOperatorType.Minus)
            {
                multiply = -1.0m;
            }

            return multiply * Evaluate(input.Value);
        }

        private enum PriorityLevel
        {
            One,
            Two,
            Three,
            Four,
            Result
        }

        private PriorityLevel GetPriorityLevel(BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Add => PriorityLevel.Four,
                BinaryOperatorType.Subtract => PriorityLevel.Four,
                BinaryOperatorType.Multiply => PriorityLevel.Two,
                BinaryOperatorType.Divide => PriorityLevel.Two,
                BinaryOperatorType.Power => PriorityLevel.One,
                _ => throw new InvalidOperationException("Unknown binary operator type!")
            };
        }

        private PriorityLevel GetPriorityLevel(UnaryOperatorType op)
        {
            return PriorityLevel.Three;
        }

        private enum BinaryOperatorEvaluationOrder
        {
            Left,
            Right
        }

        private BinaryOperatorEvaluationOrder GetEvaluationOrder(BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Power => BinaryOperatorEvaluationOrder.Right,
                _ => BinaryOperatorEvaluationOrder.Left
            };
        }
    }
}
