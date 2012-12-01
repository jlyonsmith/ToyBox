using System;

namespace ToyBox
{   

#warning Should be UpdatableGameComponent

	public class GameComponent : IGameComponent, IUpdateable, IComparable<GameComponent>, IDisposable
	{
		Game _game;
		int _updateOrder;
		bool _enabled;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> EnabledChanged;
		public GameComponent(Game game)
		{
			_game = game;
			Enabled = true;
		}
		
		public Game Game 
		{
			get 
			{
				return _game;
			}
		}
		
		public virtual void Initialize()
		{
		}
		
		public virtual void Update(GameTime gameTime)
		{
		}
		
		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				Raise(EnabledChanged, EventArgs.Empty);
				OnEnabledChanged(this, null);
			}
		}
		
		public int UpdateOrder
		{
			get { return _updateOrder; }
			set
			{
				_updateOrder = value;
				Raise(UpdateOrderChanged, EventArgs.Empty);
				OnUpdateOrderChanged(this, null);
			}
		}
		
		private void Raise(EventHandler<EventArgs> handler, EventArgs e)
		{
			if (handler != null)
				handler(this, e);
		}
		
		protected virtual void OnUpdateOrderChanged(object sender, EventArgs args)
		{
		}
		
		protected virtual void OnEnabledChanged(object sender, EventArgs args)
		{
		}
		
		protected virtual void Dispose(bool disposing)
		{
		}
		
		public virtual void Dispose()
		{
			Dispose(true);
		}
		
		#region IComparable<> Members
		
		public int CompareTo(GameComponent other)
		{
			return other.UpdateOrder - this.UpdateOrder;
		}
		
		#endregion
	}
}
