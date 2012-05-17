using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ToyBox
{
    public interface IMousePointerService
    {
        Point Position { get; }
    }
}
