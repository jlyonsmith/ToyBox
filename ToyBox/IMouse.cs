using System;
using System.Collections.Generic;

namespace ToyBox
{
    public delegate void MouseMovedDelegate(Point point);
    public delegate void MouseButtonDelegate(MouseButtons buttons);
    public delegate void MouseWheelDelegate(float ticks);

    public interface IMouse : IInputDevice
    {
        event MouseMovedDelegate Moved;
        event MouseButtonDelegate ButtonPressed;
        event MouseButtonDelegate ButtonReleased;
        event MouseWheelDelegate WheelRotated;
        MouseState GetState();
        void MoveTo(Point point);
    }
}
