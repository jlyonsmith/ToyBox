using System;
using System.Collections.Generic;

namespace ToyBox
{
    public interface IInputDevice
    {
        bool IsAttached { get; }
        string Name { get; }
        void Update();
    }
}
