using System.Collections.Generic;
using System.Linq;

namespace CMC.AST
{
    public class Identifier : AST
    {
        public UserCreatableID RootID { get; set; }
        public List<UserCreatableID> NestedIDs { get; }

        public string GetFullName(int? nestedIDsCount = null)
        {
            string fullSpelling = RootID.Spelling;
            if (nestedIDsCount != null)
            {
                fullSpelling += "." + string.Join(".", NestedIDs.Select(x => x.Spelling).ToArray(), 0, nestedIDsCount);
            }

            return fullSpelling;
        }

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