using System;
using System.Collections.Generic;
using System.Linq;
using CMC.AST;

namespace CMC
{
    public class IDTable
    {
        private List<(UserCreatableID structName, Struct @struct)> definedStructs;
        private List<(int scopeLevel, UserCreatableID ID, Declaration declaration, DeclarationType type)> enviornment;
        private List<bool> isLoopScopeList = new List<bool>();
        private int _currentScopeLevel;
        private VariableType.ValueTypeEnum? CurrentExpectedReturnType;
        public DeclarationFunctionDeclaration StartFunction { get; private set; }
        public bool IsGlobalScope => _currentScopeLevel == 0;

        public VariableType.ValueTypeEnum ExpectedReturnType
        {
            get
            {
                if(_currentScopeLevel != 1)
                {
                    return VariableType.ValueTypeEnum.NOTHING;
                }
                if (CurrentExpectedReturnType.HasValue)
                {
                    return CurrentExpectedReturnType.Value;
                }
                else
                {
                    throw new Exception("Someone was an idiot, trying to get expected return type when it is not set");
                }
            }
        }

        public enum DeclarationType
        {
            STRUCT,
            FUNCTION,
            VARIABLE
        }

        public IDTable()
        {
            StartFunction = null;
            _currentScopeLevel = 0;
            CurrentExpectedReturnType = null;
            enviornment = new List<(int scopeLevel, UserCreatableID ID, Declaration declaration, DeclarationType type)>();
        }

        public void Add(UserCreatableID ID, Declaration declaration, DeclarationType type)
        {
            if (enviornment.Any(env => env.scopeLevel == _currentScopeLevel && env.ID.Spelling == ID.Spelling && env.type == type))
            {
                throw new Exception("ID already defined for current scope level");
            }

            //checks for "start" function
            if (type == DeclarationType.FUNCTION && ID.Spelling == "start")
            {
                var funcDec = (DeclarationFunctionDeclaration) declaration;

                if (funcDec.FunctionDeclaration.ReturnType.ValueType == VariableType.ValueTypeEnum.INTY &&
                    funcDec.FunctionDeclaration.ParameterList.Parameters.Count == 0)
                {
                    StartFunction = funcDec;
                }
                else
                {
                    throw new Exception("start function must take nothing and giveback inty");
                }
            }

            // TODO For struct
            if (declaration is VariableDeclarationSimple variableDeclaration)
            {
                variableDeclaration.Address = new Address(_currentScopeLevel == 0);
            }

            if (declaration is DeclarationFunctionDeclaration functionDeclaration)
            {
                functionDeclaration.FunctionDeclaration.Address = new Address(true);
            }

            enviornment.Add((_currentScopeLevel, ID, declaration, type));
        }

        public bool IsInLoopScope()
        {
            return isLoopScopeList.Any(x => x == true);
        }

        public Declaration Lookup(UserCreatableID ID, DeclarationType type)
        {
            var lst = enviornment.FindAll(item => item.ID.Spelling == ID.Spelling && item.type == type);

            if (lst.Count == 0)
            {
                throw new Exception("Nothing found in lookup exception");
            }
            else
            {
                var max = lst.Max(item => item.scopeLevel);
                var result = lst.Find(item => item.scopeLevel == max);
                return result.declaration;
            }
        }

        /// <summary>
        /// returns VariableDeclarationSimple or VariableDeclarationStructVariableDeclaration if type == VARIABLE
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public VariableDeclarationSimple Lookup(Identifier ID)
        {
            VariableDeclaration variableDeclaration = ((VariableDeclaration) Lookup(ID.RootID, DeclarationType.VARIABLE));

            for (var a = 0; a < ID.NestedIDs.Count; a++)
            {
                UserCreatableID structName = ((VariableDeclarationStructVariableDeclaration) variableDeclaration).StructVariableDeclaration.StructName;
                VariableDeclaration variableDeclarationInsideStruct = ((VariableDeclarationStructVariableDeclaration) variableDeclaration).VariableDeclarations.Find(x => x.Name.Spelling == ID.NestedIDs[a].Spelling);
                
                string fullCurrentName = ID.GetFullName(a + 1);
                if (variableDeclarationInsideStruct == null)
                {
                    throw new Exception($"No matching declaration found for {fullCurrentName} in struct {structName}");
                }

                if (a < ID.NestedIDs.Count - 1 && variableDeclarationInsideStruct is VariableDeclarationSimple)
                {
                    throw new Exception($"Expected nested struct for {fullCurrentName}; but it was declared as simple variable type in struct '{structName}'");
                }

                if (a == ID.NestedIDs.Count - 1 && variableDeclarationInsideStruct is VariableDeclarationStructVariableDeclaration)
                {
                    throw new Exception($"Expected simple variable type for {fullCurrentName}; but it was declared as struct in struct '{structName}'");
                }

                variableDeclaration = variableDeclarationInsideStruct;
            }
            
            return (VariableDeclarationSimple) variableDeclaration;
        }


        public void EnterNestedScopeLevel(VariableType.ValueTypeEnum? expectedReturnType = null, bool isLoopScope = false)
        {
            if (_currentScopeLevel == 0)
            {
                if (expectedReturnType.HasValue)
                {
                    this.CurrentExpectedReturnType = expectedReturnType.Value;
                }
                else
                {
                    throw new Exception("Expected return type must be specified when entering a function defenition");
                }
            }
            else
            {
                if (expectedReturnType.HasValue)
                {
                    throw new Exception("The expected return type can only be set when entering function definition");
                }
            }

            _currentScopeLevel++;
            isLoopScopeList.Add(isLoopScope);
        }

        public void ExitNestedScopeLevel()
        {
            _currentScopeLevel--;
            if (_currentScopeLevel == 0)
            {
                CurrentExpectedReturnType = null;
            }

            isLoopScopeList.RemoveAt(isLoopScopeList.Count - 1);
            enviornment.RemoveAll(item => item.scopeLevel > _currentScopeLevel);
        }
    }
}