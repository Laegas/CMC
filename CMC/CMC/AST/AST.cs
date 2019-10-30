namespace CMC.AST
{
    public abstract class AST
    {
        public abstract object Visit(IASTVisitor visitor, object arg = null);
    }
}