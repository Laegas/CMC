using System;
using System.Collections.Generic;
using System.Linq;
using CMC.AST;

namespace CMC
{
    public class IDTable
    {
        private List<(UserCreatableID structName, Struct @struct)> definedStructs;
        private List<(int scopeLevel, UserCreatableID ID, DeclarationType type)> enviornment;
        private List<bool> isLoopScopeList = new List<bool>();
        private int _currentScopeLevel;
        private VariableType.ValueTypeEnum? CurrentExpectedReturnType;
        public bool HasStartFunction { get; private set; }
        public VariableType.ValueTypeEnum ExpectedReturnType
        {
            get
            {
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

        public enum DeclarationType { STRUCT, FUNCTION, VARIABLE }
        public IDTable()
        {
            HasStartFunction = false;
            _currentScopeLevel = 0;
            CurrentExpectedReturnType = null;
            enviornment = new List<(int scopeLevel, UserCreatableID ID, DeclarationType type)>();
        }

        public void Add(UserCreatableID ID, Declaration subTreePointer, DeclarationType type)
        {
            if (enviornment.Any(env => env.scopeLevel == _currentScopeLevel && env.ID.Spelling == ID.Spelling && env.type == type))
            {
                throw new Exception("ID already defined for current scope level");
            }
            //checks for "start" function
            if (type == DeclarationType.FUNCTION && ID.Spelling == "start")
            {
                ID.decl = subTreePointer;

                var funcDec = (DeclarationFunctionDeclaration) subTreePointer;

                if (funcDec.FunctionDeclaration.ReturnType.ValueType == VariableType.ValueTypeEnum.INTY &&
                    funcDec.FunctionDeclaration.ParameterList.Parameters.Count == 0)
                {
                    HasStartFunction = true;
                }
                else
                {
                    throw new Exception("start function must take nothing and giveback inty");
                }
            }
            enviornment.Add((_currentScopeLevel, ID, type));
        }

        public bool IsInLoopScope()
        {
            return isLoopScopeList.Any(x => x == true);
        }

        public UserCreatableID Lookup(UserCreatableID ID, DeclarationType type)
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
                return result.ID;
            }
        }

        /// <summary>
        /// returns VariableDeclarationSimple or VariableDeclarationStructVariableDeclaration if type == VARIABLE
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public UserCreatableID Lookup(Identifier ID, DeclarationType type)
        {
            if (false == ID.NestedIDs.Any())
            {
                return Lookup(ID.RootID, type); 
            }

            var first = (UserCreatableID) Lookup(ID.RootID, type);
            var second = (DeclarationStruct)first.decl;
            var @struct = second.Struct;

            var loopingStruct = @struct;
            for (int i = 0; i < ID.NestedIDs.Count; i++)
            {
                var bla = loopingStruct.VariableDeclarationList.VariableDeclarations;

                var searchedList = bla.Where(item =>
                {
                    var name = "";
                    switch (item)
                    {
                        case VariableDeclarationSimple _simple:
                            name = _simple.VariableName.Spelling;
                            break;
                        case VariableDeclarationStructVariableDeclaration _structVar:
                            name = _structVar.StructVariableDeclaration.VariableName.Spelling;
                            break;
                    }
                    return (name == ID.NestedIDs[i].Spelling);
                }
                );

                if (searchedList.Count() != 1)
                {
                    throw new Exception("ID not defined");
                }

                if ((i + 1) == ID.NestedIDs.Count)
                {

                    var simple = searchedList.First();
                    if (simple is VariableDeclarationSimple _simple)
                    {
                        return ID.NestedIDs[i];
                    }
                    else
                    {
                        throw new Exception("Identifier did not evaluate to a variable with type inty or booly");
                    }
                }
                var strucDeclare = (VariableDeclarationStructVariableDeclaration)searchedList.First();
                var one = Lookup(strucDeclare.StructVariableDeclaration.StructName, DeclarationType.STRUCT);
                var two = (DeclarationStruct) one.decl;
                loopingStruct = two.Struct;
            }

            throw new Exception("Something went wrong in the checker");
        }

        public void EnterNestedScopeLevel(VariableType.ValueTypeEnum? expectedReturnType = null, bool isLoopScope = false)
        {
            if (_currentScopeLevel == 0 )
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