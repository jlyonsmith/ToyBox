using System;
using System.Collections.Generic;

namespace ToyBox
{
    internal class NoGamePad : IGamePad
    {
        public event GamePadButtonDelegate ButtonPressed { add { } remove { } }
        public event GamePadButtonDelegate ButtonReleased { add { } remove { } }

        /// <summary>Initializes a new game pad dummy</summary>
        public NoGamePad()
        {
        }

        public GamePadState GetState()
        {
            return new GamePadState();
        }

        public bool IsAttached
        {
            get { return false; }
        }

        public string Name
        {
            get { return "NoGamePad"; }
        }

        public void Update() { }
    }
}
