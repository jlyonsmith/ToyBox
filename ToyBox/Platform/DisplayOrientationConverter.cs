using System;
using MonoTouch.UIKit;

namespace ToyBox
{
    public static class OrientationConverter
    {
        public static DisplayOrientation ToDisplayOrientation(UIInterfaceOrientation orientation)
        {
            switch (orientation)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    return DisplayOrientation.LandscapeLeft;
                case UIInterfaceOrientation.LandscapeRight:
                    return DisplayOrientation.LandscapeRight;
                default:
                case UIInterfaceOrientation.Portrait:
                    return DisplayOrientation.Portrait;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return DisplayOrientation.PortraitUpsideDown;
            }
        }
    }
}

