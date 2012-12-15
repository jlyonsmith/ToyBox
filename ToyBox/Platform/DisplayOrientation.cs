using System;

namespace ToyBox
{
    [Flags]
    public enum DisplayOrientation
    {
        Unknown = 0,
        Portrait = 1<<0,
        PortraitUpsideDown = 1<<1,
        LandscapeLeft = 1<<2,
        LandscapeRight = 1<<3,
    }
}

