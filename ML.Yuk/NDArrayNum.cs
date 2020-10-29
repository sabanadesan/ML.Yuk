using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Yuk
{
    public class NDArrayNum : NDArray
    {
        public NDArrayNum() : base()
        {

        }

        public NDArrayNum(params dynamic[] array) : base(array)
        {

        }

        public double Sum()
        {
            return 0;
        }
    }
}
