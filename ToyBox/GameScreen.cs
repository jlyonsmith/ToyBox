using System;
using System.Collections.Generic;

namespace ToyBox
{
    public abstract class GameScreen : IGameScreen, IUpdateable
    {
        protected Game Game { get; set; }
        
        public GameScreen(Game game)
        {
            this.Game = game;
        }

        public event EventHandler<EventArgs> EnabledChanged { add { } remove { } }
        public event EventHandler<EventArgs> UpdateOrderChanged { add { } remove { } }

		public void Pause()
        {
            if (!this.paused)
            {
                OnPause();
                this.paused = true;
            }
        }

        public void Resume()
        {
            if (this.paused)
            {
                OnResume();
                this.paused = false;
            }
        }

        public abstract void Update(GameTime gameTime);

		protected virtual void OnEntered() { }
        protected virtual void OnLeaving() { }
        protected virtual void OnPause() { }
        protected virtual void OnResume() { }

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

        public bool Enabled
        {
            get { return true; }
        }

        public int UpdateOrder
        {
            get { return 0; }
        }

        private bool paused;
    }
}
