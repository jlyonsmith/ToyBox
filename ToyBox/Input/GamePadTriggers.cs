using System;

namespace ToyBox
{
	public struct GamePadTriggers
	{
		float left, right;
		
		public float Left
		{
			get { return left; }
			internal set { left = MathHelper.Clamp(value, 0f, 1f); }
		}
		public float Right
		{
			get { return right; }
			internal set { right = MathHelper.Clamp(value, 0f, 1f); }
		}
		
		public GamePadTriggers(float leftTrigger, float rightTrigger):this()
		{
			Left = leftTrigger;
			Right = rightTrigger;
		}
	}
}
