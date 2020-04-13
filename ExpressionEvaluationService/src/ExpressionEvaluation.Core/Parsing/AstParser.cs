using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;
using ExpressionEvaluation.Core.Parsing.Grammar;

namespace ExpressionEvaluation.Core.Parsing
{
    internal class AstParser : IAstParser
    {
        public BinaryNode Parse(string input)
        {
            input = SanitizeInput(input);
            ValidateInput(input);

            if (TryGetE(input, out var e))
            {
                return e;
            }

            return null;
        }

        private bool TryGetE(string input, out BinaryNode e)
        {
            var innerInput = input;

            if (TryGetP(ref innerInput, out var p))
            {
                e = new BinaryNode(p);

                while (true)
                {
                    if (!TryGetBinaryOperator(ref innerInput, out var b))
                    {
                        break;
                    }

                    if (!TryGetP(ref innerInput, out var nextP))
                    {
                        break;
                    }

                    e.BP.Add((b, nextP));
                }

                // there have to be nothing left to parse
                if (!string.IsNullOrEmpty(innerInput))
                {
                    throw new AstParserException("Unable to parse: " + innerInput);
                }

                return true;
            }

            e = null;
            return false;
        }

        private bool TryGetP(ref string input, out UnaryNode p)
        {
            var innerInput = input;

            if (TryGetPExpression(ref innerInput, out var pExpr))
            {
                input = innerInput;
                p = pExpr;
                return true;
            }

            if (TryGetPNegative(ref innerInput, out var pNegative))
            {
                input = innerInput;
                p = pNegative;
                return true;
            }

            if (TryGetPValue(ref innerInput, out var pValue))
            {
                input = innerInput;
                p = pValue;
                return true;
            }

            p = null;
            return false;
        }

        private bool TryGetPExpression(ref string input, out UnaryExpressionNode p)
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
                            if (TryGetE(innerInput, out var e))
                            {
                                p = new UnaryExpressionNode(e);
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

            p = null;
            return false;
        }

        private bool TryGetPNegative(ref string input, out UnaryNegativeNode p)
        {
            if (input.FirstOrDefault() == '-')
            {
                var innerInput = input.Substring(1);
                if (TryGetP(ref innerInput, out var innerP))
                {
                    input = innerInput;
                    p = new UnaryNegativeNode(innerP);
                    return true;
                }
            }

            p = null;
            return false;
        }

        private bool TryGetPValue(ref string input, out UnaryValueNode p)
        {
            var match = Regex.Match(input, @"^([0-9\.]+)(%?)(.*)");
            if (match.Success && decimal.TryParse(match.Groups[1].Value, out var inputValue))
            {
                // convert from percentage to decimal
                var decimalValue = match.Groups[2].Value == "%"
                    ? inputValue / 100
                    : inputValue;
                
                p = new UnaryValueNode(decimalValue);
                input = match.Groups[3].Value;
                return true;
            }

            p = null;
            return false;
        }

        private bool TryGetBinaryOperator(ref string input, out BinaryOperatorType b)
        {
            if (input.Length > 0)
            {
                b = BinaryOperator.CharToOperator(input[0]);
                if (b != BinaryOperatorType.Unknown)
                {
                    input = input.Substring(1);
                    return true;
                }
            }

            b = BinaryOperatorType.Unknown;
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
