using System.Collections.Generic;

namespace CMC.AST
{
    public class ParameterList : AST
    {
        public List<Parameter> Parameters { get; }

        public ParameterList( List<Parameter> parameters )
        {
            Parameters = parameters;
        }

        public override object Visit( IASTVisitor visitor, object arg = null )
        {
            return visitor.VisitParameterList( this, arg );
        }
    }

    public class Parameter : AST
    {
        public VariableType ParameterType { get; }
        public UserCreatableID ParameterName { get; }

        public Parameter( VariableType parameterType, UserCreatableID parameterName )
        {
            this.ParameterType = parameterType;
            this.ParameterName = parameterName;
        }

        public override object Visit( IASTVisitor visitor, object arg = null )
        {
            return visitor.VisitParameter(this, arg);
        }
    }
}