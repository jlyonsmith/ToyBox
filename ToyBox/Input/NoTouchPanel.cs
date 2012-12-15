using System;
using System.Collections.Generic;

namespace ToyBox
{
    internal class NoTouchPanel : ITouchPanel
    {
        public event TouchDelegate Pressed { add { } remove { } }
        public event TouchDelegate Released { add { } remove { } }
        public event TouchDelegate Moved { add { } remove { } }
        
        public NoTouchPanel() { }

        public int MaximumTouchCount { get { return 0; } }
        public TouchState GetState() { return new TouchState(); }
        public bool IsAttached { get { return false; } }

        public string Name
        {
            get { return "NoTouchPanel"; }
        }

        public void Update() { }
    }
}
