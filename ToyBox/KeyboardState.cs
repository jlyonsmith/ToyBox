using System;
using System.Linq;

namespace ToyBox
{
	public struct KeyboardState
	{
		private Keys[] keys;
		
		#region Methods
		public KeyboardState (Keys[] keys)
		{
			this.keys = keys;
			// Never used on iPhone or Zune
		}
		
		public override bool Equals(Object obj)
		{
			return (this.GetHashCode() == obj.GetHashCode());
		}
		
		public override int GetHashCode()
		{
			// use the hash code of the _keys array
			if (keys != null)
				return keys.GetHashCode();
			else
				return -1;
		}
		
		public Keys[] GetPressedKeys()
		{
			if (keys == null)
				keys = new Keys[] {};
			return keys;
		}
		
		public bool IsKeyDown(Keys key)
		{
			if (keys != null)
			{
				return keys.Contains(key);
			}
			return false;
		}
		
		public bool IsKeyUp(Keys key)
		{
			if (keys != null)
			{
				return !keys.Contains(key);
			}
			return true;
		}
		
#endregion
		
		#region Properties
		public KeyState this [Keys key]
		{ 
			get
			{
				return (IsKeyDown(key) ? KeyState.Down : KeyState.Up);
			}			
		}
		
#endregion
		
		#region Operator overloads
		public static bool operator ==(KeyboardState first, KeyboardState second)
		{
			return first.GetHashCode() == second.GetHashCode();
		}
		
		public static bool operator !=(KeyboardState first, KeyboardState second)
		{
			return first.GetHashCode() != second.GetHashCode();
		}
#endregion
	}
}
