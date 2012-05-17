using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace ToyBox
{
    public struct TouchState
    {
        public TouchState(bool isAttached, TouchCollection touches)
        {
            this.isAttached = isAttached;
            this.touches = touches;
        }

        public bool IsAttached
        {
            get { return this.isAttached; }
        }

        public TouchCollection Touches
        {
            get { return this.touches; }
        }

        private bool isAttached;
        private TouchCollection touches;
    }
}