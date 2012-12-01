using System;
using System.Collections.Generic;

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