using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace ToyBox
{
	public class Game : IDisposable, IServiceProvider
	{
		private const float defaultTargetFramesPerSecond = 60.0f;
		private DateTime _now;
		private DateTime _lastUpdate = DateTime.UtcNow;
		private readonly GameTime _gameTime = new GameTime();
		private readonly GameTime _fixedTimeStepTime = new GameTime();
		private TimeSpan _totalTime = TimeSpan.Zero;
		private GameComponentCollection _components;
		private GameServiceContainer _services;
		private IGraphicsService graphicsService;
		private IPlatformService platformService;

		private SortingFilteringCollection<IDrawable> _drawables =
			new SortingFilteringCollection<IDrawable>(
				d => d.Visible,
				(d, handler) => d.VisibleChanged += handler,
				(d, handler) => d.VisibleChanged -= handler,
				(d1 ,d2) => Comparer<int>.Default.Compare(d1.DrawOrder, d2.DrawOrder),
				(d, handler) => d.DrawOrderChanged += handler,
				(d, handler) => d.DrawOrderChanged -= handler);
		
		private SortingFilteringCollection<IUpdateable> _updateables =
			new SortingFilteringCollection<IUpdateable>(
				u => u.Enabled,
				(u, handler) => u.EnabledChanged += handler,
				(u, handler) => u.EnabledChanged -= handler,
				(u1, u2) => Comparer<int>.Default.Compare(u1.UpdateOrder, u2.UpdateOrder),
				(u, handler) => u.UpdateOrderChanged += handler,
				(u, handler) => u.UpdateOrderChanged -= handler);
		
		private bool _initialized = false;
		private bool _isFixedTimeStep = true;
		private TimeSpan _targetElapsedTime = TimeSpan.FromSeconds(1 / defaultTargetFramesPerSecond);
		private int previousDisplayWidth;
		private int previousDisplayHeight;
		
		public Game()
		{
			_services = new GameServiceContainer();
			_components = new GameComponentCollection();

			this.platformService = new PlatformManager(this);

#warning Fix activated/deactivated
			//Platform.Activated += Platform_Activated;
			//Platform.Deactivated += Platform_Deactivated;

			_services.AddService(typeof(IPlatformService), platformService);
		}

		#region IDisposable Implementation
		
		public void Dispose()
		{
			Dispose(true);
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
#warning Need to clean-up all services

			}
		}

		#endregion IDisposable Implementation

		#region IServiceProvider implementation

		public object GetService(Type serviceType)
		{
			throw new NotImplementedException();
		}

		#endregion
		
		#region Properties
		
		public GameComponentCollection Components
		{
			get { return _components; }
		}
		
		public TimeSpan TargetElapsedTime
		{
			get { return _targetElapsedTime; }
			set
			{
				if (value <= TimeSpan.Zero)
					throw new ArgumentOutOfRangeException("Value must be positive and non-zero.");
				
				_targetElapsedTime = value;
			}
		}
		
		public bool IsFixedTimeStep
		{
			get { return _isFixedTimeStep; }
			set { _isFixedTimeStep = value; }
		}
		
		public GameServiceContainer Services 
		{
			get 
			{ 
				return _services; 
			}
		}
		
		public IContentManager Content { get; set; }
		
		#endregion Properties
		
		#region Events
		public event EventHandler<EventArgs> Activated;
		public event EventHandler<EventArgs> Deactivated;
		public event EventHandler<EventArgs> Disposed;
		public event EventHandler<EventArgs> Exiting;
		
		#endregion
		
		#region Public Methods
		
		public void Exit()
		{
			platformService.Exit();
		}
		
		public void ResetElapsedTime()
		{
#warning What is this used for?
			//platformService.ResetElapsedTime();
			_gameTime.ResetElapsedTime();
		}

		public void Tick()
		{
			bool doDraw = false;
			
			_now = DateTime.UtcNow;
			
			_gameTime.Update(_now - _lastUpdate);
			_lastUpdate = _now;
			
			if (IsFixedTimeStep)
			{
				_totalTime += _gameTime.ElapsedGameTime;
				int max = (500/TargetElapsedTime.Milliseconds);    //Only do updates for half a second worth of updates
				int iterations = 0;
				
				max = max <= 0 ? 1 : max;   //Make sure at least 1 update is called
				
				while (_totalTime >= TargetElapsedTime)
				{
					_fixedTimeStepTime.Update(TargetElapsedTime);
					_totalTime -= TargetElapsedTime;
					DoUpdate(_fixedTimeStepTime);
					doDraw = true;
					
					iterations++;
					if (iterations >= max)  //Reset catchup if to many updates have been called
					{
						_totalTime = TimeSpan.Zero;
					}
				}
			}
			else
			{
				DoUpdate(_gameTime);
				doDraw = true;
			}
			
			if (doDraw)
			{
				DoDraw(_gameTime);
				this.graphicsService.Present();
			}
			
			if (IsFixedTimeStep)
			{
				var currentTime = (DateTime.UtcNow - _lastUpdate) + _totalTime;
				
				if (currentTime < TargetElapsedTime)
				{
					System.Threading.Thread.Sleep((TargetElapsedTime - currentTime).Milliseconds);
				}
			}
		}
		
		#endregion
		
		#region Protected Methods
		
		protected virtual bool BeginDraw() { return true; }
		protected virtual void EndDraw() { }
		protected virtual void BeginRun() { }
		protected virtual void EndRun() { }
		protected virtual void LoadContent() { }
		protected virtual void UnloadContent() { }
		
		protected virtual void Initialize()
		{
			CategorizeComponents();

			_components.ComponentAdded += Components_ComponentAdded;
			_components.ComponentRemoved += Components_ComponentRemoved;

			InitializeExistingComponents();
			
			graphicsService = (IGraphicsService)Services.GetService(typeof(IGraphicsService));
			
			if (graphicsService != null)
			{
				LoadContent();
			}
		}
		
		private static readonly Action<IDrawable, GameTime> DrawAction =
			(drawable, gameTime) => drawable.Draw(gameTime);
		
		protected virtual void Draw(GameTime gameTime)
		{
			_drawables.ForEachFilteredItem(DrawAction, gameTime);
		}
		
		private static readonly Action<IUpdateable, GameTime> UpdateAction =
			(updateable, gameTime) => updateable.Update(gameTime);
		
		protected virtual void Update(GameTime gameTime)
		{
			_updateables.ForEachFilteredItem(UpdateAction, gameTime);
		}
		
		protected virtual void OnExiting(object sender, EventArgs args)
		{
			Raise(Exiting, args);
		}
		
		#endregion Protected Methods
		
		#region Event Handlers
		
		private void Components_ComponentAdded(
			object sender, GameComponentCollectionEventArgs e)
		{
			// Since we only subscribe to ComponentAdded after the graphics
			// devices are set up, it is safe to just blindly call Initialize.
			e.GameComponent.Initialize();
			CategorizeComponent(e.GameComponent);
		}
		
		private void Components_ComponentRemoved(
			object sender, GameComponentCollectionEventArgs e)
		{
			DecategorizeComponent(e.GameComponent);
		}
		
		private void Platform_Activated(object sender, EventArgs e)
		{
			Raise(Activated, e);
		}
		
		private void Platform_Deactivated(object sender, EventArgs e)
		{
			Raise(Deactivated, e);
		}
		
		#endregion Event Handlers
		
		#region Internal Methods
		
		internal void DoUpdate(GameTime gameTime)
		{
			Update(gameTime);
		}

		internal void DoDraw(GameTime gameTime)
		{
			Draw(gameTime);
			EndDraw();
		}
		
		internal void DoInitialize()
		{
			Initialize();
		}
		
		internal void DoExiting()
		{
			OnExiting(this, EventArgs.Empty);
		}
		
		#endregion Internal Methods

		public void Run()
		{
			throw new NotImplementedException();
		}

		private void InitializeExistingComponents()
		{
			var copy = new IGameComponent[Components.Count];
			Components.CopyTo(copy, 0);
			foreach (var component in copy)
				component.Initialize();
		}
		
		private void CategorizeComponents()
		{
			DecategorizeComponents();
			for (int i = 0; i < Components.Count; ++i)
				CategorizeComponent(Components[i]);
		}
		
		private void DecategorizeComponents()
		{
			_updateables.Clear();
			_drawables.Clear();
		}
		
		private void CategorizeComponent(IGameComponent component)
		{
			if (component is IUpdateable)
				_updateables.Add((IUpdateable)component);
			if (component is IDrawable)
				_drawables.Add((IDrawable)component);
		}
		
		private void DecategorizeComponent(IGameComponent component)
		{
			if (component is IUpdateable)
				_updateables.Remove((IUpdateable)component);
			if (component is IDrawable)
				_drawables.Remove((IDrawable)component);
		}
		
		private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e)
			where TEventArgs : EventArgs
		{
			if (handler != null)
				handler(this, e);
		}
	}		
}
