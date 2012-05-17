using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework
{
    public static class PointExtensions
    {
        public static Point Translate(this Point point, Point other)
        {
            return new Point(point.X + other.X, point.Y + other.Y);
        }

        public static Point Translate(this Point point, int x, int y)
        {
            return new Point(point.X + x, point.Y + y);
        }

        /// <summary>
        /// Gets the spacing defined by the rectangle and another, defined as the absolute horizontal and vertical
        /// distance between the two rectangles top-left corners.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Point Subtract(this Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        public static Point Add(this Point point1, Point point2)
        {
            return Add(point1, point2.X, point2.Y);
        }

        public static Point Add(this Point point1, int x, int y)
        {
            return new Point(point1.X + x, point1.Y + y);
        }
    }
}
