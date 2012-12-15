using System;

namespace ToyBox
{
	public enum TouchLocationState
	{
		Invalid,    // This touch location position is invalid. 
		// Typically, you will encounter this state when a new touch location attempts to get the previous state of itself.
		
		Moved,      // This touch location position was updated or pressed at same position. 
		Pressed,    // This touch location position is new. 
		Released,   // This touch location position was released. 
	}
}