using System;
using System.Collections.Generic;

namespace ToyBox
{
    public abstract class GameScreen : IGameScreen, IDisposable
    {
        protected const int DefaultScreenLayer = 1000;

        protected IPlatformService PlatformService { get; private set; }
        private bool visible;
        private bool paused;
        private int layer = DefaultScreenLayer;

        protected IServiceProvider Services { get; private set; }

        public GameScreen(IServiceProvider services)
        {
            PlatformService = services.GetService<IPlatformService>();

            PlatformService.Update += OnUpdate;
            PlatformService.Draw += OnDraw;
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            PlatformService.Update -= OnUpdate;
            PlatformService.Draw -= OnDraw;
        }

        #endregion

		public void Pause()
        {
            if (!this.paused)
            {
                OnPause();
                PlatformService.Update -= OnUpdate;
                this.paused = true;
            }
        }

        public void Resume()
        {
            if (this.paused)
            {
                OnResume();
                PlatformService.Update -= OnUpdate;
                this.paused = false;
            }
        }

        public bool Visible
        { 
            get { return visible; }
            set
            {
                visible = value;

                if (visible)
                {
                    PlatformService.Draw += OnDraw;
                }
                else
                {
                    PlatformService.Draw -= OnDraw;
                }
            }
        }

		protected virtual void OnEntered() { }
        protected virtual void OnLeaving() { }
        protected virtual void OnPause() { }
        protected virtual void OnResume() { }
        protected virtual void OnDraw() { }
        protected virtual void OnUpdate(GameTime gameTime) { }

        protected int ScreenLayer
        { 
            get { return layer; }
            set
            {
                PlatformService.Draw -= OnDraw;
                layer = value;
                PlatformService.Draw += OnDraw;
            }
        } 

        protected bool Paused
        {
            get { return this.paused; }
        }

        public void Enter()
        {
            OnEntered();
        }

        public void Leave()
        {
            OnLeaving();
        }
    }
}
