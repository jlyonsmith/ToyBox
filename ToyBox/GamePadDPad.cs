using System;

namespace ToyBox
{
	public struct GamePadDPad
	{
		public ButtonState Down
		{
			get;
			internal set;
		}
		public ButtonState Left
		{
			get;
			internal set;
		}
		public ButtonState Right
		{
			get;
			internal set;
		}
		public ButtonState Up
		{
			get;
			internal set;
		}
		
		public GamePadDPad(ButtonState upValue, ButtonState downValue, ButtonState leftValue, ButtonState rightValue)
		: this()
		{
			Up = upValue;
			Down = downValue;
			Left = leftValue;
			Right = rightValue;
		}
		internal GamePadDPad(Buttons b)
		: this()
		{
			if ((b & Buttons.DPadDown) == Buttons.DPadDown)
				Down = ButtonState.Pressed;
			if ((b & Buttons.DPadLeft) == Buttons.DPadLeft)
				Left = ButtonState.Pressed;
			if ((b & Buttons.DPadRight) == Buttons.DPadRight)
				Right = ButtonState.Pressed;
			if ((b & Buttons.DPadUp) == Buttons.DPadUp)
				Up = ButtonState.Pressed;
		}
	}
}