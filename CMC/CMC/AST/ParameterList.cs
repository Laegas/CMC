using System.Collections.Generic;

namespace CMC.AST
{
    public class ParameterList : AST
    {
        public List<(VariableType parameterType, UserCreatableID parameterName)> Parameters { get; }

        public ParameterList(List<(VariableType parameterType, UserCreatableID parameterName)> parameters)
        {
            Parameters = parameters;
        }
    }

}