using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace ToyBox
{
    public delegate void KeyboardButtonDelegate(Keys key);
    public delegate void CharacterDelegate(char character);

    public interface IKeyboard : IInputDevice
    {
        event KeyboardButtonDelegate ButtonPressed;
        event KeyboardButtonDelegate ButtonReleased;
        KeyboardState GetState();
    }
}
