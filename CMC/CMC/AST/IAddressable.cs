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
        public int FrameLevel { get; private set; }
        public int Offset { get; private set; }

        public Address(int frameLevel, int offset )
        {
            FrameLevel = frameLevel;
            Offset = offset;
        }
    }
}
