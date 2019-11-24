using System.Collections.Generic;

namespace CMC.AST
{
    public abstract class IDBase : AST { }

    public class Identifier : IDBase
    {
        public UserCreatableID RootID { get; set; }
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