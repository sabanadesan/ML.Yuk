using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Yuk
{
    public class Slice
    {

        private Index _start;
        private Index _end;
        private int _step;

        public Slice(Index start, Index end, int step = 1)
        {
            _start = start;
            _end = end;
            _step = step;
        }

        public Index Start
        {
            get => _start;
            set => _start = value;
        }

        public Index End
        {
            get => _end;
            set => _end = value;
        }

        public int Step
        {
            get => _step;
            set => _step = value;
        }
    }
}
