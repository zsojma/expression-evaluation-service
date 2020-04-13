using System.Linq;
using System.Text.RegularExpressions;
using ExpressionEvaluation.Core.Expression;
using ExpressionEvaluation.Core.Expression.Abstract;

namespace ExpressionEvaluation.Core.Parsing
{
    internal class AstParser : IAstParser
    {
        private readonly ExpressionNodeFactory _nodeFactory;

        public AstParser(ExpressionNodeFactory nodeFactory)
        {
            _nodeFactory = nodeFactory;
        }

        public ExpressionNode Parse(string input)
        {
            input = SanitizeInput(input);
            ValidateInput(input);

            var root = new ParenthesesBlockOld(input);
            if (!root.IsValid())
            {
                throw new AstParserException("Cannot parse input to blocks based on parenthesis!");
            }
            
            return null;
        }

        //internal ParenthesesBlockOld SplitByParenthesis(string input)
        //{

        //}

        internal ExpressionNode SplitByExpressions(string input)
        {
            if (TryParseConstant(input, out var node))
            {
                return node;
            }

            //if (TryParseAdd(input, out node))
            //{
            //    return node;
            //}

            //if (TryParseSubtract(input, out node))
            //{
            //    return node;
            //}

            throw new AstParserException("Cannot find matching expression: " + input);
        }

        internal bool TryParseConstant(string input, out ExpressionNode node)
        {
            var match = Regex.Match(input, @"^\s*([0-9\.]+)\s*$");
            if (match.Success && decimal.TryParse(match.Groups[1].Value, out var value))
            {
                node = _nodeFactory.CreateConstant(value);
                return true;
            }

            node = null;
            return false;
        }

        //internal bool TryParseAdd(string input, out ExpressionNode node)
        //{
        //    var match = Regex.Match(input, $@"^\s*({VARIABLE_CHAR})\s*\+\s*({VARIABLE_CHAR})\s*$");
        //    if (match.Success)
        //    {
        //        node = _nodeFactory.CreateAdd(null, null);
        //        return true;
        //    }

        //    node = null;
        //    return false;
        //}

        //internal bool TryParseSubtract(string input, out ExpressionNode node)
        //{
        //    var match = Regex.Match(input, $@"^\s*({VARIABLE_CHAR})\s*\-\s*({VARIABLE_CHAR})\s*$");
        //    if (match.Success)
        //    {
        //        node = _nodeFactory.CreateSubtract(null, null);
        //        return true;
        //    }

        //    node = null;
        //    return false;
        //}

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
            var regex = new Regex(@"^[0-9\.+\-\*\/%\^\(\)]+$");

            if (!regex.Match(input).Success)
            {
                var invalidCharacters = regex.Matches(input).Select(x => $"'{x.Value}'");
                var invalidCharactersStr = string.Join(", ", invalidCharacters);
                throw new AstParserException("Input contains invalid characters: " + invalidCharactersStr);
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
