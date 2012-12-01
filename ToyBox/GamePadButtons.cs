using System;

namespace ToyBox
{
	public struct GamePadButtons
	{
		internal Buttons buttons;
		
		public ButtonState A
		{
			get
			{
				return ((buttons & Buttons.A) == Buttons.A) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState B
		{
			get
			{
				return ((buttons & Buttons.B) == Buttons.B) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState Back
		{
			get
			{
				return ((buttons & Buttons.Back) == Buttons.Back) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState X
		{
			get
			{
				return ((buttons & Buttons.X) == Buttons.X) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState Y
		{
			get
			{
				return ((buttons & Buttons.Y) == Buttons.Y) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState Start
		{
			get
			{
				return ((buttons & Buttons.Start) == Buttons.Start) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState LeftShoulder
		{
			get
			{
				return ((buttons & Buttons.LeftShoulder) == Buttons.LeftShoulder) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState LeftStick
		{
			get
			{
				return ((buttons & Buttons.LeftStick) == Buttons.LeftStick) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState RightShoulder
		{
			get
			{
				return ((buttons & Buttons.RightShoulder) == Buttons.RightShoulder) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState RightStick
		{
			get
			{
				return ((buttons & Buttons.RightStick) == Buttons.RightStick) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		public ButtonState BigButton
		{
			get
			{
				return ((buttons & Buttons.BigButton) == Buttons.BigButton) ? ButtonState.Pressed : ButtonState.Released;
			}
		}
		
		public GamePadButtons(Buttons buttons)
		{
			this.buttons = buttons;
		}
		internal GamePadButtons(params Buttons[] buttons)
		: this()
		{
			foreach (Buttons b in buttons)
				this.buttons |= b;
		}
	}
}

