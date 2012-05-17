using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace System
{
    public static class MathExtensions
    {
        public static Size Abs(Size size)
        {
            return new Size(Math.Abs(size.Width), Math.Abs(size.Height));
        }
    }
}
