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
        public bool IsGlobalScope { get; }
        public int Offset { get; set; }

        public Address(bool isGlobalScope)
        {
            IsGlobalScope = isGlobalScope;
        }
    }
}
