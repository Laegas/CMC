using System.Collections.Generic;

namespace CMC.AST
{
    public abstract class ParameterList
    {
    }

    public class ParameterListSimple : ParameterList
    {
        public ParameterListSimple((VariableType, UserCreatableID) parameter,
            List<(VariableType, UserCreatableID)> otherParameters)
        {
            Parameter = parameter;
            OtherParameters = otherParameters;
        }

        public (VariableType parameterType, UserCreatableID parameterName) Parameter { get; }
        public List<(VariableType parameterType, UserCreatableID parameterName)> OtherParameters { get; }
    }

    public class ParameterListNothing : ParameterList
    {
    }
}