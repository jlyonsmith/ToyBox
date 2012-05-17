using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Create a new rectangle from this one with with a location of (0, 0)
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Rectangle ZeroLocation(this Rectangle rect)
        {
            rect.X = 0;
            rect.Y = 0;
            return rect;
        }

        /// <summary>
        /// Create a new rectangle from this one that is the same same and centered on another rectangle.
        /// </summary>
        /// <returns></returns>
        public static Rectangle CenteredOn(this Rectangle rect, Rectangle otherRect)
        {
            return new Rectangle(
                otherRect.X + (otherRect.Width - rect.Width) / 2,
                otherRect.Y + (otherRect.Height - rect.Height) / 2, 
                rect.Width, rect.Height);
        }

        /// <summary>
        /// Set the size of a rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Rectangle Resize(this Rectangle rect, Vector2 vector)
        {
            rect.Width = (int)vector.X;
            rect.Height = (int)vector.Y;
            return rect;
        }

        /// <summary>
        /// Offset the rectangle by a multiple of its Width and Height
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Rectangle UnitOffset(this Rectangle rect, float X, float Y)
        {
            return new Rectangle((int)(rect.X + X * rect.Width), (int)(rect.Y + Y * rect.Height), rect.Width, rect.Height);
        }
    }
}
