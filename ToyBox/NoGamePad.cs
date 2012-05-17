﻿#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

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
