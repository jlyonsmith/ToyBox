using System;
using System.Collections.Generic;

namespace ToyBox
{
    public delegate void TouchDelegate(int id, Vector2 position);

    public interface ITouchPanel : IInputDevice
    {
        event TouchDelegate Pressed;
        event TouchDelegate Moved;
        event TouchDelegate Released;
        int MaximumTouchCount { get; }
        TouchState GetState();
    }
}
