//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace ExpressionEvaluation.Core.Parsing
//{
//    internal class ParenthesesBlock
//    {
//        private readonly List<ParenthesesBlock> _innerBlocks;
//        private string _innerBlocksPattern;
//        private bool _valid;

//        public ParenthesesBlockOld()
//        {
//        }

//        public IReadOnlyList<ParenthesesBlockOld> InnerBlocks => _innerBlocks;

//        public bool IsValid() => _valid && _innerBlocks.All(x => x.IsValid());

//        public override string ToString()
//        {
//            var output = _innerBlocksPattern ?? Input;
//            var matches = Regex.Matches(output, @"\$(\d+)");
//            foreach (Match match in matches)
//            {
//                if (int.TryParse(match.Groups[1].Value, out var index) && index < _innerBlocks.Count)
//                {
//                    var innerValue = $"({_innerBlocks[index]})";
//                    output = Regex.Replace(output, $@"\${index}", innerValue);
//                }
//            }

//            return output;
//        }

//        private void ParseInnerBlocks(string input)
//        {
//            // check if inner block exists
//            var leftParenthesisIndex = input.IndexOf('(', StringComparison.Ordinal);
//            if (leftParenthesisIndex < 0)
//            {
//                // we don't have inner blocks
//                _innerBlocksPattern = null;
//                _valid = IsBlockWithNoParenthesesValid(input);
//                return;
//            }

//            // inner block exists, find end of it
//            var extraInnerBlocksCount = 0;
//            var rightParenthesisIndex = leftParenthesisIndex + 1;
//            _innerBlocksPattern = string.Empty;

//            while (rightParenthesisIndex < input.Length)
//            {
//                if (input[rightParenthesisIndex] == '(')
//                {
//                    // extra inner block exists
//                    ++extraInnerBlocksCount;
//                }
//                else if (input[rightParenthesisIndex] == ')')
//                {
//                    // if inner block is directly within this block
//                    if (extraInnerBlocksCount == 0)
//                    {
//                        // block found
//                        var innerInput = input.Substring(leftParenthesisIndex + 1, rightParenthesisIndex - leftParenthesisIndex - 1);
//                        var innerBlock = new ParenthesesBlockOld(innerInput);

//                        // update pattern so whe know how inner blocks are formatted together
//                        _innerBlocksPattern += input.Substring(0, leftParenthesisIndex) + "$" + _innerBlocks.Count;
//                        _innerBlocks.Add(innerBlock);

//                        // check if another inner block exist
//                        input = input.Substring(rightParenthesisIndex + 1);
//                        leftParenthesisIndex = input.IndexOf('(', StringComparison.Ordinal);
//                        if (leftParenthesisIndex < 0)
//                        {
//                            break;
//                        }

//                        // another inner block exists, continue to find end of it
//                        rightParenthesisIndex = leftParenthesisIndex;
//                    }
//                    else
//                    {
//                        // extra inner block ends
//                        --extraInnerBlocksCount;
//                    }
//                }

//                ++rightParenthesisIndex;
//            }

//            // add rest of input to the pattern, this ensures that tail is present, ex: (1 + 1) + 1
//            _innerBlocksPattern += input;

//            // this block is valid if all inner block were ended
//            _valid = extraInnerBlocksCount == 0;
//        }

//        private bool IsBlockWithNoParenthesesValid(string input)
//        {
//            // this block is valid if there is something inside but no right parenthesis
//            return input.Length > 0 && !string.IsNullOrWhiteSpace(input) && input.IndexOf(')', StringComparison.Ordinal) < 0;
//        }
//    }
//}
