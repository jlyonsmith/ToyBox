using System;

namespace ToyBox
{
	public struct TouchLocation : IEquatable<TouchLocation>
	{
		/// <summary>
		///Attributes 
		/// </summary>
		private int id;
		private Vector2 position;
		private Vector2 previousPosition;
		private TouchLocationState state;
		private TouchLocationState previousState;
		
		// Only used in Android, for now
		private float pressure;
		private float previousPressure;
		
		#region Properties
		public int Id 
		{ 
			get
			{
				return id;
			}
		}
		
		public Vector2 Position 
		{ 
			get
			{
				return position;
			} 
			set {
				previousPosition = position;
				position = value;
			}
		}
		
		public float Pressure 
		{ 
			get
			{
				return pressure;
			}
		}
		
		public float PrevPressure 
		{ 
			get
			{
				return previousPressure;
			}
		}
		
		public Vector2 PrevPosition
		{
			get{
				return previousPosition;
			}
			set{
				previousPosition = value;
			}
		}
		
		public TouchLocationState State 
		{ 
			get
			{
				return state;
			} 
			set
			{
				previousState = state;
				state = value;
			}
		}
		public TouchLocationState PrevState
		{
			get
			{
				return previousState;
			}
			set{
				previousState = value;
			}
		}
#endregion
		
		#region Constructors
		public TouchLocation(int aId, TouchLocationState aState, Vector2 aPosition,
		                     TouchLocationState aPreviousState, Vector2 aPreviousPosition)
		{
			id = aId;
			position = aPosition;
			previousPosition = aPreviousPosition;
			state = aState;
			previousState = aPreviousState;	
			pressure = 0.0f;
			previousPressure = 0.0f;
		}
		
		public TouchLocation(int aId, TouchLocationState aState, Vector2 aPosition)
		{
			id = aId;
			position = aPosition;
			previousPosition = Vector2.Zero;		
			state = aState;
			previousState = TouchLocationState.Invalid;	
			pressure = 0.0f;
			previousPressure = 0.0f;
		}
		
		// Only for Android
		public TouchLocation(int aId, TouchLocationState aState, Vector2 aPosition, float aPressure,
		                     TouchLocationState aPreviousState, Vector2 aPreviousPosition, float aPreviousPressure)
		{
			id = aId;
			position = aPosition;
			previousPosition = aPreviousPosition;
			state = aState;
			previousState = aPreviousState;	
			pressure = aPressure;
			previousPressure = aPreviousPressure;
		}
		
		public TouchLocation(int aId, TouchLocationState aState, Vector2 aPosition, float aPressure)
		{
			id = aId;
			position = aPosition;
			previousPosition = Vector2.Zero;		
			state = aState;
			previousState = TouchLocationState.Invalid;
			pressure = aPressure;
			previousPressure = 0.0f;
		}
#endregion
		
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is TouchLocation)
			{
				result = Equals((TouchLocation) obj);
			}
			return result;
		}
		
		public bool Equals(TouchLocation other)
		{
			return ( ( this.Id.Equals( other.Id ) ) && 
			        ( this.Position.Equals( other.Position ) ) && 
			        ( this.previousPosition.Equals ( other.previousPosition)));
		}
		
		public override int GetHashCode()
		{
			return id;
		}
		
		public override string ToString()
		{
			return "Touch id:"+id+" state:"+state + " position:" + position + " pressure:" + pressure +" prevState:"+previousState+" prevPosition:"+ previousPosition + " previousPressure:" + previousPressure;
		}
		
		public bool TryGetPreviousLocation(out TouchLocation aPreviousLocation)
		{
			if ( previousState == TouchLocationState.Invalid )
			{
				aPreviousLocation.id = -1;
				aPreviousLocation.state = TouchLocationState.Invalid;
				aPreviousLocation.position = Vector2.Zero;
				aPreviousLocation.previousState = TouchLocationState.Invalid;
				aPreviousLocation.previousPosition = Vector2.Zero; 
				aPreviousLocation.pressure = 0.0f;
				aPreviousLocation.previousPressure = 0.0f;
				return false;
			}
			else
			{
				aPreviousLocation.id = this.id;
				aPreviousLocation.state = this.previousState;
				aPreviousLocation.position = this.previousPosition;
				aPreviousLocation.previousState = TouchLocationState.Invalid;
				aPreviousLocation.previousPosition = Vector2.Zero;
				aPreviousLocation.pressure = this.previousPressure;
				aPreviousLocation.previousPressure = 0.0f;
				return true;
			}
		}
		
		public static bool operator !=(TouchLocation value1, TouchLocation value2)
		{
			return ! (((value1.id == value2.id) && 
			           (value1.state == value2.state) &&
			           (value1.position == value2.position) &&
			           (value1.previousState == value2.previousState) &&
			           (value1.previousPosition == value2.previousPosition)));
		}
		
		public static bool operator ==(TouchLocation value1, TouchLocation value2)
		{
			return ((value1.id == value2.id) && 
			        (value1.state == value2.state) &&
			        (value1.position == value2.position) &&
			        (value1.previousState == value2.previousState) &&
			        (value1.previousPosition == value2.previousPosition));
		}
	}
}