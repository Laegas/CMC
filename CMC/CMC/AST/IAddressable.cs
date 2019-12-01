using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.AST
{
    public interface IAddressable
    {
        Address Address { get; set; }
    }


    public class Address
    {
        public int ScopeLevel { get; private set; }
        public int Address_ { get; private set; }

        public Address(int scopeLevel, int address )
        {
            this.ScopeLevel = scopeLevel;
            this.Address_ = address;
        }
    }
}
