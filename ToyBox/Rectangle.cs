using System;
using System.Globalization;
using System.ComponentModel;

namespace ToyBox
{
	public struct Rectangle : IEquatable<Rectangle>
	{
		#region Private Fields
		
		private static Rectangle emptyRectangle = new Rectangle();
		
		#endregion Private Fields
		
		#region Public Fields
		
		public int X;
		public int Y;
		public int Width;
		public int Height;
		
		#endregion Public Fields
		
		#region Public Properties
		
		public static Rectangle Empty
		{
			get { return emptyRectangle; }
		}
		
		public int Left
		{
			get { return this.X; }
		}
		
		public int Right
		{
			get { return (this.X + this.Width); }
		}
		
		public int Top
		{
			get { return this.Y; }
		}
		
		public int Bottom
		{
			get { return (this.Y + this.Height); }
		}
		
		#endregion Public Properties
		
		#region Constructors
		public Rectangle(int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}
		
		#endregion Constructors
		
		#region Public Methods
		
		// Create a new rectangle from this one with with a location of (0, 0)
		public Rectangle ZeroLocation()
		{
			return new Rectangle(0, 0, Width, Height);
		}
		
		// Create a new rectangle from this one that is the same same and centered on another rectangle.
		public Rectangle CenteredOn(Rectangle otherRect)
		{
			return new Rectangle(
				otherRect.X + (otherRect.Width - this.Width) / 2,
				otherRect.Y + (otherRect.Height - this.Height) / 2, 
				this.Width, this.Height);
		}
		
		// Set the size of a rectangle
		public Rectangle Resize(Size size)
		{
			return new Rectangle(X, Y, size.Width, size.Height);
		}
		
		// Offset the rectangle by a multiple of its Width and Height
		public Rectangle UnitOffset(float X, float Y)
		{
			return new Rectangle((int)(this.X + X * this.Width), (int)(this.Y + Y * this.Height), this.Width, this.Height);
		}
		
		// Get the size of the Rectangle
		public Size Size()
		{
			return new Size(this.Width, this.Height);
		}

		public static bool operator ==(Rectangle a, Rectangle b)
		{
			return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
		}
		
		public bool Contains(int x, int y)
		{
			return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
		}
		
		public bool Contains(Vector2 value)
		{
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}
		
		public bool Contains(Point value)
		{
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}
		
		public bool Contains(Rectangle value)
		{
			return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
		}
		
		public static bool operator !=(Rectangle a, Rectangle b)
		{
			return !(a == b);
		}
		
		public void Offset(Point offset)
		{
			X += offset.X;
			Y += offset.Y;
		}
		
		public void Offset(int offsetX, int offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}
		
		public Point Location
		{
			get 
			{
				return new Point(this.X, this.Y);
			}
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}
		
		public Point Center
		{
			get 
			{
				// This is incorrect
				//return new Point( (this.X + this.Width) / 2,(this.Y + this.Height) / 2 );
				// What we want is the Center of the rectangle from the X and Y Origins
				return new Point(this.X + (this.Width / 2), this.Y + (this.Height / 2));
			}
		}
		
		
		
		
		public void Inflate(int horizontalValue, int verticalValue)
		{
			X -= horizontalValue;
			Y -= verticalValue;
			Width += horizontalValue * 2;
			Height += verticalValue * 2;
		}
		
		public bool IsEmpty
		{
			get
			{
				return ((((this.Width == 0) && (this.Height == 0)) && (this.X == 0)) && (this.Y == 0));
			}
		}
		
		public bool Equals(Rectangle other)
		{
			return this == other;
		}
		
		public override bool Equals(object obj)
		{
			return (obj is Rectangle) ? this == ((Rectangle)obj) : false;
		}
		
		public override string ToString()
		{
			return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height);
		}
		
		public override int GetHashCode()
		{
			return (this.X ^ this.Y ^ this.Width ^ this.Height);
		}
		
		public bool Intersects(Rectangle value)
		{
			return value.Left < Right       && 
				Left       < value.Right && 
					value.Top  < Bottom      &&
					Top        < value.Bottom;            
		}
		
		
		public void Intersects(ref Rectangle value, out bool result)
		{
			result = value.Left < Right       && 
				Left       < value.Right && 
					value.Top  < Bottom      &&
					Top        < value.Bottom;
		}
		
		public static Rectangle Intersect(Rectangle value1, Rectangle value2)
		{
			Rectangle rectangle;
			Intersect(ref value1, ref value2, out rectangle);
			return rectangle;
		}
		
		
		public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			if (value1.Intersects(value2))
			{
				int right_side = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
				int left_side = Math.Max(value1.X, value2.X);
				int top_side = Math.Max(value1.Y, value2.Y);
				int bottom_side = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
				result = new Rectangle(left_side, top_side, right_side - left_side, bottom_side - top_side);
			}
			else
			{
				result = new Rectangle(0, 0, 0, 0);
			}
		}
		
		#endregion Public Methods
	}
}
