using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SlowProcess
{
    public class LongRunningProcess:IEnumerable<int>
    {
        private int _currentPosition;
        private readonly int _totalIterations;

        public LongRunningProcess(int iterations)
        {
            _totalIterations = iterations;
        }

        public IEnumerator<int> GetEnumerator()
        {
            _currentPosition = 0;
            while (_currentPosition<_totalIterations)
            {
                Thread.Sleep(100);
                _currentPosition++;
                yield return _currentPosition;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
