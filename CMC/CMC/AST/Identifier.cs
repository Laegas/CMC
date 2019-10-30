using System.Collections.Generic;

namespace CMC.AST
{
    public class Identifier
    {
        public UserCreatableID RootID { get; }
        public List<UserCreatableID> NestedIDs { get; }

        public Identifier(UserCreatableID rootId, List<UserCreatableID> nestedIDs)
        {
            RootID = rootId;
            NestedIDs = nestedIDs;
        }
    }
}