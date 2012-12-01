using System;

namespace ToyBox
{
	public struct GamePadThumbSticks
	{
		public enum GateType
		{
			None,
			Round,
			Square
		};
		public static GateType Gate = GateType.Round;
		
		Vector2 left;
		Vector2 right;
		
		public Vector2 Left
		{
			get
			{
				return left;
			}
			internal set
			{
				switch (Gate)
				{
				case GateType.None:
					left = value;
					break;
				case GateType.Round:
					if (value.LengthSquared () > 1f)
						left = Vector2.Normalize (value);
					else
						left = value;
					break;
				case GateType.Square:
					left = new Vector2(MathHelper.Clamp(value.X, -1f, 1f), MathHelper.Clamp(value.Y, -1f, 1f));
					break;
				default:
					left = Vector2.Zero;
					break;
				}
			}
		}
		public Vector2 Right
		{
			get
			{
				return right;
			}
			internal set
			{
				switch (Gate)
				{
				case GateType.None:
					right = value;
					break;
				case GateType.Round:
					if (value.LengthSquared () > 1f)
						right = Vector2.Normalize (value);
					else
						right = value;
					break;
				case GateType.Square:
					right = new Vector2(MathHelper.Clamp(value.X, -1f, 1f), MathHelper.Clamp(value.Y, -1f, 1f));
					break;
				default:
					right = Vector2.Zero;
					break;
				}
			}
		}
		
		public GamePadThumbSticks(Vector2 leftPosition, Vector2 rightPosition):this()
		{
			Left = leftPosition;
			Right = rightPosition;
		}
		
		internal void ApplyDeadZone(GamePadDeadZone dz, float size)
		{
			switch (dz)
			{
			case GamePadDeadZone.None:
				break;
			case GamePadDeadZone.IndependentAxes:
				if (Math.Abs(left.X) < size)
					left.X = 0f;
				if (Math.Abs(left.Y) < size)
					left.Y = 0f;
				if (Math.Abs(right.X) < size)
					right.X = 0f;
				if (Math.Abs(right.Y) < size)
					right.Y = 0f;
				break;
			case GamePadDeadZone.Circular:
				if (left.LengthSquared() < size * size)
					left = Vector2.Zero;
				if (right.LengthSquared() < size * size)
					right = Vector2.Zero;
				break;
			}
		}
	}
}