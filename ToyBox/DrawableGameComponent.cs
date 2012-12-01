using System;

namespace ToyBox
{
	public class DrawableGameComponent : GameComponent, IDrawable
	{
		private bool _isInitialized;
		private bool _isVisible;
		private int _drawOrder;
		
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;
		
		public DrawableGameComponent(Game game)
			: base(game)
		{
			Visible = true;
		}
		
		public override void Initialize()
		{
			if (!_isInitialized)
			{
				_isInitialized = true;
				LoadContent();
			}
		}
		
		protected virtual void LoadContent()
		{
		}
		
		protected virtual void UnloadContent()
		{
		}
		
		#region IDrawable Members
		
		public int DrawOrder
		{
			get { return _drawOrder; }
			set
			{
				_drawOrder = value;
				if(DrawOrderChanged != null)
					DrawOrderChanged(this, null);
				
				OnDrawOrderChanged(this, null);
			}
		}
		
		public bool Visible
		{
			get { return _isVisible; }
			set
			{
				if (_isVisible != value)
				{
					_isVisible = value;
					
					var handler = VisibleChanged;
					if (handler != null)
						handler(this, EventArgs.Empty);
					
					OnVisibleChanged(this, EventArgs.Empty);
				}
			}
		}
		
		public virtual void Draw(GameTime gameTime) { }
		protected virtual void OnVisibleChanged(object sender, EventArgs args) { }
		protected virtual void OnDrawOrderChanged(object sender, EventArgs args) { }
		
		#endregion
	}
}
