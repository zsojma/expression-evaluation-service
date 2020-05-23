using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LogicalExpressionEvaluation.Core.Nodes;
using LogicalExpressionEvaluation.Core.Nodes.Binary;
using LogicalExpressionEvaluation.Core.Nodes.Unary;

namespace LogicalExpressionEvaluation.Core.Parsing
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
            if (TryGetBinaryNode(ref innerInput, out var root))
            {
                return root;
            }

            throw new ExpressionParserException("Unable to parse input: " + input);
        }

        private bool TryGetBinaryNode(
            ref string input,
            [NotNullWhen(true)] out BinaryNode? output,
            BinaryOperatorType requiredOp = BinaryOperatorType.Unknown)
        {
            var innerInput = input;

            if (TryGetUnaryNode(ref innerInput, out var left))
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

                    if (!TryGetUnaryNode(ref innerInput, out var right))
                    {
                        break;
                    }

                    rights.Add(new BinaryNodeItem(op, right));
                    input = innerInput;
                }

                // there have to be nothing left to parse if we are not restricted by operator
                if (requiredOp == BinaryOperatorType.Unknown && !string.IsNullOrEmpty(input))
                {
                    throw new ExpressionParserException("Unable to parse: " + input);
                }

                output = new BinaryNode(left, rights);
                return true;
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryNode(ref string input, [NotNullWhen(true)] out IUnaryNode? output)
        {
            var innerInput = input;

            if (TryGetUnaryExpressionNode(ref innerInput, out var expressionNode))
            {
                input = innerInput;
                output = expressionNode;
                return true;
            }

            if (TryGetUnaryPrefixNode(ref innerInput, out var prefixNode))
            {
                input = innerInput;
                output = prefixNode;
                return true;
            }

            if (TryGetComplexUnaryValueNode(ref innerInput, out var complexValueNode))
            {
                input = innerInput;
                output = complexValueNode;
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

        private bool TryGetUnaryExpressionNode(ref string input, [NotNullWhen(true)] out UnaryExpressionNode? output)
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
                            if (TryGetBinaryNode(ref innerInput, out var expression))
                            {
                                output = new UnaryExpressionNode(expression);
                                input = input.Substring(rightParenthesisIndex + 1);
                                return true;
                            }

                            break;
                        }

                        // extra inner block ends
                        --extraInnerBlocksCount;
                    }

                    ++rightParenthesisIndex;
                }
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryPrefixNode(ref string input, [NotNullWhen(true)] out UnaryPrefixNode? output)
        {
            var innerInput = input;
            if (TryParseUnaryValue(ref innerInput, out var value))
            {
                var op = OperatorsFormatter.StringToUnaryOperator(value);
                if (op != UnaryOperatorType.Unknown)
                {
                    if (TryGetUnaryNode(ref innerInput, out var innerUnaryNode))
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

        private bool TryGetComplexUnaryValueNode(ref string input, [NotNullWhen(true)] out UnaryValueNode? output)
        {
            if (input.StartsWith("\""))
            {
                var endIndex = input.Substring(1).IndexOf("\"", StringComparison.InvariantCulture);
                if (endIndex >= 0)
                {
                    var value = input.Substring(0, endIndex + 2);
                    output = new UnaryValueNode(value);
                    input = input.Substring(endIndex + 2).TrimStart();
                    return true;
                }
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryValueNode(ref string input, [NotNullWhen(true)] out UnaryValueNode? output)
        {
            var innerInput = input;
            if (TryParseUnaryValue(ref innerInput, out var value)
             && ValidateValueIsNotOperator(value))
            {
                input = innerInput;
                output = new UnaryValueNode(value);
                return true;
            }

            output = null;
            return false;
        }

        private bool TryParseUnaryValue(ref string input, [NotNullWhen(true)] out string? output)
        {
            var match = Regex.Match(input, @"^([^\s\(\)]+)\s(.*)");
            if (match.Success)
            {
                output = match.Groups[1].Value;
                input = match.Groups[2].Value;
                return true;
            }
            
            match = Regex.Match(input, @"^([^\s\(\)]+)$");
            if (match.Success)
            {
                output = input;
                input = string.Empty;
                return true;
            }

            output = null;
            return false;
        }

        private bool TryGetBinaryOperatorType(ref string input, [NotNullWhen(true)] out BinaryOperatorType output)
        {
            var innerInput = input;
            if (TryParseUnaryValue(ref innerInput, out var value))
            {
                output = OperatorsFormatter.StringToBinaryOperator(value);
                if (output != BinaryOperatorType.Unknown)
                {
                    input = innerInput;
                    return true;
                }
            }

            output = BinaryOperatorType.Unknown;
            return false;
        }

        private string SanitizeInput(string input)
        {
            input = input.Trim();
            var parsed = new StringBuilder();

            // remove multiple spaces
            var withinParenthesis = false;
            var previousValueIsSpace = true;
            for (var i = 0; i < input.Length; ++i)
            {
                var value = input[i];

                // skip escaped characters
                if (value == '\\')
                {
                    ++i;
                    continue;
                }

                if (value == '\"')
                {
                    parsed.Append(value);
                    withinParenthesis = !withinParenthesis;
                    previousValueIsSpace = false;
                    continue;
                }

                if (withinParenthesis)
                {
                    parsed.Append(value);
                    continue;
                }

                if (value == ' ')
                {
                    if (previousValueIsSpace)
                    {
                        continue;
                    }

                    previousValueIsSpace = true;
                }
                else
                {
                    if (value == '(' || value == ')')
                    {
                        if (!previousValueIsSpace && value == '(')
                        {
                            // add space before opening parenthesis
                            // not(3) => not (3)
                            parsed.Append(" ");
                        }

                        // consider parenthesis as spaces too, but do not skip them
                        previousValueIsSpace = true;
                    }
                    else
                    {
                        previousValueIsSpace = false;
                    }
                }


                parsed.Append(value);
            }
            
            var output = parsed.ToString();

            // also remove space before closing parenthesis
            return output.Replace(" )", ")");
        }

        private void ValidateInput(string input)
        {
            ValidateInputParenthesis(input);
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

        private bool ValidateValueIsNotOperator(string value)
        {
            return ValidateValueIsNotUnaryOperator(value)
                && ValidateValueIsNotBinaryOperator(value);
        }

        private bool ValidateValueIsNotBinaryOperator(string value)
        {
            foreach (BinaryOperatorType? type in Enum.GetValues(typeof(BinaryOperatorType)))
            {
                if (string.Equals(type?.ToDisplayString(), value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidateValueIsNotUnaryOperator(string value)
        {
            foreach (UnaryOperatorType? type in Enum.GetValues(typeof(UnaryOperatorType)))
            {
                if (string.Equals(type?.ToDisplayString(), value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }
}