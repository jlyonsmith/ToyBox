using System;

namespace ToyBox
{
	public static class Keyboard
	{		
		public static KeyboardState GetState()
		{
			return new KeyboardState(new Keys[4]); // TODO Not used on iPhone or Zune
		}
		
		public static KeyboardState GetState(PlayerIndex playerIndex)
		{
			return new KeyboardState(new Keys[4]);  // TODO Not used on iPhone or Zune
		}
	}
}
