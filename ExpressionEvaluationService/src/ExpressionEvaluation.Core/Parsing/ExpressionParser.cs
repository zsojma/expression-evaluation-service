using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExpressionEvaluation.Core.Nodes;
using ExpressionEvaluation.Core.Nodes.Binary;
using ExpressionEvaluation.Core.Nodes.Unary;

namespace ExpressionEvaluation.Core.Parsing
{
    internal class ExpressionParser : IExpressionParser
    {
        /// <summary>
        /// Parses given input string to AST.
        /// Uses Recursive-descent recognition. See: http://www.engr.mun.ca/~theo/Misc/exp_parsing.htm
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Root of AST</returns>
        public BinaryNode Parse(string input)
        {
            input = SanitizeInput(input);
            ValidateInput(input);

            var innerInput = input;
            if (TryGetBinaryNode(ref innerInput, out var root) && root != null)
            {
                return root;
            }

            throw new ExpressionParserException("Unable to parse input: " + input);
        }

        private bool TryGetBinaryNode(ref string input, out BinaryNode? output, BinaryOperatorType requiredOp = BinaryOperatorType.Unknown)
        {
            var innerInput = input;

            if (TryGetUnaryNode(ref innerInput, out var left) && left != null)
            {
                var rights = new List<BinaryNodeItem>();
                input = innerInput;

                while (true)
                {
                    if (!TryGetBinaryOperatorType(ref innerInput, out var op))
                    {
                        break;
                    }

                    if (requiredOp != BinaryOperatorType.Unknown && op != requiredOp)
                    {
                        break;
                    }

                    if (!TryGetUnaryNode(ref innerInput, out var right) || right == null)
                    {
                        break;
                    }

                    rights.Add(new BinaryNodeItem(op, right));
                    input = innerInput;
                }

                // there have to be nothing left to parse if we are not restricted by operator
                if (requiredOp == BinaryOperatorType.Unknown && !string.IsNullOrEmpty(innerInput))
                {
                    throw new ExpressionParserException("Unable to parse: " + innerInput);
                }

                output = new BinaryNode(left, rights);
                return true;
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryNode(ref string input, out IUnaryNode? output)
        {
            var innerInput = input;

            if (TryGetUnaryExpressionNode(ref innerInput, out var expressionNode))
            {
                input = innerInput;
                output = expressionNode;
                return true;
            }

            if (TryGetUnaryPrefixWithPowerNode(ref innerInput, out var prefixWithPowerNode))
            {
                input = innerInput;
                output = prefixWithPowerNode;
                return true;
            }

            if (TryGetUnaryPrefixNode(ref innerInput, out var prefixNode))
            {
                input = innerInput;
                output = prefixNode;
                return true;
            }

            if (TryGetUnaryValueNode(ref innerInput, out var valueNode))
            {
                input = innerInput;
                output = valueNode;
                return true;
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryExpressionNode(ref string input, out UnaryExpressionNode? output)
        {
            if (input.StartsWith("("))
            {
                var extraInnerBlocksCount = 0;
                var rightParenthesisIndex = 1;

                while (rightParenthesisIndex < input.Length)
                {
                    if (input[rightParenthesisIndex] == '(')
                    {
                        // extra inner block exists
                        ++extraInnerBlocksCount;
                    }
                    else if (input[rightParenthesisIndex] == ')')
                    {
                        // if inner block is directly within this block
                        if (extraInnerBlocksCount == 0)
                        {
                            // block found
                            var innerInput = input.Substring(1, rightParenthesisIndex - 1);
                            if (TryGetBinaryNode(ref innerInput, out var expression) && expression != null)
                            {
                                output = new UnaryExpressionNode(expression);
                                input = input.Substring(rightParenthesisIndex + 1);
                                return true;
                            }

                            break;
                        }
                        else
                        {
                            // extra inner block ends
                            --extraInnerBlocksCount;
                        }
                    }

                    ++rightParenthesisIndex;
                }
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryPrefixWithPowerNode(ref string input, out UnaryPrefixNode? output)
        {
            if (input.Length > 0)
            {
                var op = OperatorsFormatter.CharToUnaryOperator(input[0]);
                if (op != UnaryOperatorType.Unknown)
                {
                    var innerInput = input.Substring(1);
                    if (TryGetBinaryNode(ref innerInput, out var innerBinaryNode, BinaryOperatorType.Power) && innerBinaryNode != null)
                    {
                        if (innerBinaryNode.Rights.Any())
                        {
                            input = innerInput;
                            output = new UnaryPrefixNode(op, new UnaryExpressionNode(innerBinaryNode));
                            return true;
                        }
                    }
                }
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryPrefixNode(ref string input, out UnaryPrefixNode? output)
        {
            if (input.Length > 0)
            {
                var op = OperatorsFormatter.CharToUnaryOperator(input[0]);
                if (op != UnaryOperatorType.Unknown)
                {
                    var innerInput = input.Substring(1);
                    if (TryGetUnaryNode(ref innerInput, out var innerUnaryNode) && innerUnaryNode != null)
                    {
                        input = innerInput;
                        output = new UnaryPrefixNode(op, innerUnaryNode);
                        return true;
                    }
                }
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryValueNode(ref string input, out UnaryValueNode? output)
        {
            var match = Regex.Match(input, @"^([0-9\.]+)(%?)(.*)");
            if (match.Success && decimal.TryParse(match.Groups[1].Value, out var inputValue))
            {
                // convert from percentage to decimal
                var decimalValue = match.Groups[2].Value == "%"
                    ? inputValue / 100
                    : inputValue;
                
                output = new UnaryValueNode(decimalValue);
                input = match.Groups[3].Value;
                return true;
            }

            output = null;
            return false;
        }

        private bool TryGetBinaryOperatorType(ref string input, out BinaryOperatorType output)
        {
            if (input.Length > 0)
            {
                output = OperatorsFormatter.CharToBinaryOperator(input[0]);
                if (output != BinaryOperatorType.Unknown)
                {
                    input = input.Substring(1);
                    return true;
                }
            }

            output = BinaryOperatorType.Unknown;
            return false;
        }

        private string SanitizeInput(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }

        private void ValidateInput(string input)
        {
            ValidateInputCharacters(input);
            ValidateInputParenthesis(input);
        }

        private void ValidateInputCharacters(string input)
        {
            // check if all characters are allowed in input string
            var match = Regex.Match(input, @"^[0-9\.+\-\*\/%\^\(\)]+$");
            if (!match.Success)
            {
                var invalidStrings = Regex.Matches(input, @"[^0-9\.+\-\*\/%\^\(\)]+").Select(x => $"'{x.Value}'");
                throw new ExpressionParserException("Input contains invalid strings: " + string.Join(", ", invalidStrings));
            }
        }

        private void ValidateInputParenthesis(string input)
        {
            // check is number of left & right parenthesis match
            var leftCount = input.Count(x => x == '(');
            var rightCount = input.Count(x => x == ')');
            if (leftCount != rightCount)
            {
                throw new ExpressionParserException("Number of left and right parenthesis does not match!");
            }
        }
    }
}
