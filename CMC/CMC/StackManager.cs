using System.Collections.Generic;
using CMC.AST;
using CMC.TAM;

namespace CMC
{
    class StackManager
    {
        private int _currentFrameLevel = 0; // from Jan
        private List<int> offsets = new List<int>(); // from Jan

        public int CurrentOffset => offsets[_currentFrameLevel];

        public StackManager()
        {
            offsets.Add(0);
        }

        public void IncrementFrameLevel()
        {
            _currentFrameLevel++;
            offsets.Add(Machine.linkDataSize);
        }

        public void DecrementFrameLevel()
        {
            _currentFrameLevel--;
            offsets.Remove(offsets.Count - 1);
        }

        public void IncrementOffset(int delta = 1)
        {
            offsets[_currentFrameLevel] += delta;
        }

        public void DecrementOffset(int delta = 1)
        {
            offsets[_currentFrameLevel] -= delta;
        }
    }
}