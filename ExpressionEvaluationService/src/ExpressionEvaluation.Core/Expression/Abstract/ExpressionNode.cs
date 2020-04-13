namespace ExpressionEvaluation.Core.Expression.Abstract
{
    public abstract class ExpressionNode
    {
        protected ExpressionNode(ExpressionNodeType nodeType)
        {
            NodeType = nodeType;
        }

        public ExpressionNodeType NodeType { get; }
    }
}
