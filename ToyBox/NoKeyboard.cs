using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ToyBox
{
    internal class NoKeyboard : IKeyboard
    {
        public event KeyboardButtonDelegate ButtonPressed { add { } remove { } }
        public event KeyboardButtonDelegate ButtonReleased { add { } remove { } }
        public event CharacterDelegate CharacterEntered { add { } remove { } }

        public NoKeyboard(Keys[] keysWanted)
        {
        }

        public KeyboardState GetState()
        {
            return new KeyboardState();
        }

        public bool IsAttached
        {
            get { return false; }
        }

        public string Name
        {
            get
            {
                return "NoKeyboard";
            }
        }

        public void Update()
        {
        }
    }
}
