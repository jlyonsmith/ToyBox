using System;

namespace ToyBox
{
	public static class Mouse
	{
		internal static MouseState State;
		
		public static IntPtr WindowHandle { get; set; }
		
		public static MouseState GetState()
		{
			return State;
		}
	}
}

