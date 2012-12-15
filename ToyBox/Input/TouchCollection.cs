using System;
using System.Collections;
using System.Collections.Generic;

namespace ToyBox
{	
	public class TouchCollection : List<TouchLocation>
	{
		private bool isConnected;
		
		#region Properties
		public bool IsConnected
		{
			get
			{
				return this.isConnected;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}
		#endregion
		
		public TouchCollection ()
		{
		}
		
		internal TouchCollection (IEnumerable<TouchLocation> locations)	: base (locations)
		{
			
		}
		
		internal void Update()
		{
			//Console.WriteLine(">>> Touches: {0}", Count);
			for (int i = this.Count - 1; i >= 0; --i)
			{
				TouchLocation t = this [i];
				switch (t.State)
				{
				case TouchLocationState.Pressed:
					t.State = TouchLocationState.Moved;
					t.PrevPosition = t.Position;
					this [i] = t;
					break;
				case TouchLocationState.Moved:
					t.PrevState = TouchLocationState.Moved;
					this [i] = t;
					break;
				case TouchLocationState.Released:
				case TouchLocationState.Invalid:
					RemoveAt(i);
					break;
				}
			}
			//Console.WriteLine("<<< Touches: {0}", Count);
		}
		
		public bool FindById(int id, out TouchLocation touchLocation)
		{
			int index = this.FindIndex((t) => {
				return t.Id == id; });
			if (index >= 0)
			{
				touchLocation = this [index];
				return true;
			}
			touchLocation = default(TouchLocation);
			return false;
		}
		
		internal int FindIndexById(int id, out TouchLocation touchLocation)
		{
			for (int i = 0; i < this.Count; i++)
			{
				TouchLocation location = this [i];
				if (location.Id == id)
				{
					touchLocation = this [i];
					return i;
				}
			}
			touchLocation = default(TouchLocation);
			return -1;
		}
		
		internal void Add(int id, Vector2 position)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this [i].Id == id)
				{
#if DEBUG
					Console.WriteLine("Error: Attempted to re-add the same touch as a press.");
#endif
					Clear();
				}
			}

			Add(new TouchLocation (id, TouchLocationState.Pressed, position));
		}
		
		internal void Update(int id, TouchLocationState state, Vector2 position)
		{
			if (state == TouchLocationState.Pressed)
				throw new ArgumentException ("Argument 'state' cannot be TouchLocationState.Pressed.");
			
			for (int i = 0; i < Count; i++)
			{
				if (this [i].Id == id)
				{
					var touchLocation = this [i];
					touchLocation.Position = position;
					touchLocation.State = state;
					this [i] = touchLocation;
					return;
				}
			}
#if DEBUG			
			Console.WriteLine("Error: Attempted to mark a non-existent touch {0} as {1}.", id, state);
#endif
			Clear();
		}
	}
}