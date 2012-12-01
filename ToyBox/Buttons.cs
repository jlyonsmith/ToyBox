using System;

namespace ToyBox
{
	// Summary:
	//     Enumerates input device buttons.
	[Flags]
	public enum Buttons
	{
		// Summary:
		//     Directional pad down
		DPadUp = 1,
		//
		// Summary:
		//     Directional pad up
		DPadDown = 2,
		//
		// Summary:
		//     Directional pad left
		DPadLeft = 4,
		//
		// Summary:
		//     Directional pad right
		DPadRight = 8,
		//
		// Summary:
		//     START button
		Start = 16,
		//
		// Summary:
		//     BACK button
		Back = 32,
		//
		// Summary:
		//     Left stick button (pressing the left stick)
		LeftStick = 64,
		//
		// Summary:
		//     Right stick button (pressing the right stick)
		RightStick = 128,
		//
		// Summary:
		//     Left bumper (shoulder) button
		LeftShoulder = 256,
		//
		// Summary:
		//     Right bumper (shoulder) button
		RightShoulder = 512,
		//
		// Summary:
		//     Big button
		BigButton = 2048,
		//
		// Summary:
		//     A button
		A = 4096,
		//
		// Summary:
		//     B button
		B = 8192,
		//
		// Summary:
		//     X button
		X = 16384,
		//
		// Summary:
		//     Y button
		Y = 32768,
		//
		// Summary:
		//     Left stick is towards the left
		LeftThumbstickLeft = 2097152,
		//
		// Summary:
		//     Right trigger
		RightTrigger = 4194304,
		//
		// Summary:
		//     Left trigger
		LeftTrigger = 8388608,
		//
		// Summary:
		//     Right stick is towards up
		RightThumbstickUp = 16777216,
		//
		// Summary:
		//     Right stick is towards down
		RightThumbstickDown = 33554432,
		//
		// Summary:
		//     Right stick is towards the right
		RightThumbstickRight = 67108864,
		//
		// Summary:
		//     Right stick is towards the left
		RightThumbstickLeft = 134217728,
		//
		// Summary:
		//     Left stick is towards up
		LeftThumbstickUp = 268435456,
		//
		// Summary:
		//     Left stick is towards down
		LeftThumbstickDown = 536870912,
		//
		// Summary:
		//     Left stick is towards the right
		LeftThumbstickRight = 1073741824,
	}
	
}
