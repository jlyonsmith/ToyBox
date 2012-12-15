using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyBox
{
    [Flags]
    public enum MouseButtons
    {
        Left = 1 << 0,
        Middle = 1 << 1,
        Right = 1 << 2,
        X1 = 1 << 3,
        X2 = 1 << 4
    }
}
