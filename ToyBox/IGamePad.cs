﻿using System;
using System.Collections.Generic;

namespace ToyBox
{
    public delegate void GamePadButtonDelegate(Buttons buttons);
    public delegate void ExtendedGamePadButtonDelegate(ulong buttons1, ulong buttons2);

    public interface IGamePad : IInputDevice
    {
        event GamePadButtonDelegate ButtonPressed;
        event GamePadButtonDelegate ButtonReleased;
        GamePadState GetState();
    }
}
