using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

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
