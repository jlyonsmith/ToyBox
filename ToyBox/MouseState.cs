using System;

namespace ToyBox
{    
	public struct MouseState
	{
		int _x,_y;
		ButtonState _leftButton;
		ButtonState _middleButton;
		ButtonState _rightButton;
		
		internal MouseState(int x, int y)
		{
			_x = x;
			_y = y;
			_leftButton = ButtonState.Released;
			_middleButton = ButtonState.Released;
			_rightButton = ButtonState.Released;
		}
		
		public int X
		{
			get
			{
				return _x;
			}
			
			internal set
			{
				_x = value;
			}	           
		}
		public int Y
		{
			get
			{
				return _y;
			}
			
			internal set
			{
				_y = value;
			}
		}
		
		public ButtonState LeftButton 
		{ 
			get
			{
				return _leftButton;	
			}
			
			internal set
			{
				_leftButton = value;
			}
		}
		
		public ButtonState MiddleButton 
		{ 
			get
			{
				return _middleButton;	
			}
			
			internal set
			{
				_middleButton = value;
			}
		}
		
		public ButtonState RightButton 
		{ 
			get
			{
				return _rightButton;	
			}
			
			internal set
			{
				_rightButton = value;
			}
		}
		
		public int ScrollWheelValue 
		{ 
			get
			{
				return 0;
			}
		}
		
		public ButtonState XButton1
		{ 
			get
			{
				return ButtonState.Released;
			}
		}
		
		public ButtonState XButton2
		{ 
			get
			{
				return ButtonState.Released;
			}
		}
	}
}

