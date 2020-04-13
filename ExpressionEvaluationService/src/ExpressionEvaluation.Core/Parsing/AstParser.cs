using System.Linq;
using System.Text.RegularExpressions;
using ExpressionEvaluation.Core.ExpressionNodes;

namespace ExpressionEvaluation.Core.Parsing
{
    internal class AstParser : IAstParser
    {
        public BinaryNode Parse(string input)
        {
            input = SanitizeInput(input);
            ValidateInput(input);

            return TryGetBinaryNode(input, out var root)
                ? root
                : throw new AstParserException("Unable to parse input: " + input);
        }

        private bool TryGetBinaryNode(string input, out BinaryNode output)
        {
            var innerInput = input;

            if (TryGetUnaryNode(ref innerInput, out var left))
            {
                output = new BinaryNode(left);

                while (true)
                {
                    if (!TryGetBinaryOperatorType(ref innerInput, out var op))
                    {
                        break;
                    }

                    if (!TryGetUnaryNode(ref innerInput, out var right))
                    {
                        break;
                    }

                    output.Rights.Add((op, right));
                }

                // there have to be nothing left to parse
                if (!string.IsNullOrEmpty(innerInput))
                {
                    throw new AstParserException("Unable to parse: " + innerInput);
                }

                return true;
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryNode(ref string input, out UnaryNode output)
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

            if (TryGetUnaryValueNode(ref innerInput, out var valueNode))
            {
                input = innerInput;
                output = valueNode;
                return true;
            }

            output = null;
            return false;
        }

        private bool TryGetUnaryExpressionNode(ref string input, out UnaryExpressionNode output)
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
                            if (TryGetBinaryNode(innerInput, out var expression))
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

        private bool TryGetUnaryPrefixNode(ref string input, out UnaryPrefixNode output)
        {
            if (input.Length > 0)
            {
                var op = UnaryOperator.CharToOperator(input[0]);
                if (op != UnaryOperatorType.Unknown)
                {
                    var innerInput = input.Substring(1);
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

        private bool TryGetUnaryValueNode(ref string input, out UnaryValueNode output)
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
                output = BinaryOperator.CharToOperator(input[0]);
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
                throw new AstParserException("Input contains invalid strings: " + string.Join(", ", invalidStrings));
            }
        }

        private void ValidateInputParenthesis(string input)
        {
            // check is number of left & right parenthesis match
            var leftCount = input.Count(x => x == '(');
            var rightCount = input.Count(x => x == ')');
            if (leftCount != rightCount)
            {
                throw new AstParserException("Number of left and right parenthesis does not match!");
            }
        }
    }
}
