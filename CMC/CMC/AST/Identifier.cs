using System.Collections.Generic;

namespace CMC.AST
{
    public class Identifier : AST
    {
        public UserCreatableID RootID { get; }
        public List<UserCreatableID> NestedIDs { get; }

        public Identifier(UserCreatableID rootId, List<UserCreatableID> nestedIDs)
        {
            RootID = rootId;
            NestedIDs = nestedIDs;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitIdentifier(this, arg);
        }
    }
}