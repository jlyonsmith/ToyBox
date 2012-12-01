using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace ToyBox
{
    public abstract class Animation
    {
        private TimeSpan time = TimeSpan.Zero;
        private SpriteManager spriteManager;

        public bool HasStarted { get; private set; }
        public bool HasFinished { get; private set; }
        public Sprite Sprite { get; private set; }
        public TimeSpan StartDelay { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Animation NextAnimation { get; set; }

        public event EventHandler<EventArgs> Finished;
        public event EventHandler<EventArgs> Started;

        public Animation(TimeSpan startDelay, TimeSpan duration)
        {
            this.StartDelay = startDelay;
            this.Duration = duration;
        }

        public virtual void Initialize(SpriteManager spriteManager, Sprite sprite)
        {
            this.spriteManager = spriteManager;
            this.Sprite = sprite;
            this.HasStarted = false;
            this.HasFinished = false;
        }

        public void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime;

            if (time < StartDelay)
            {
                return;
            }
            else if (time >= StartDelay + Duration)
            {
                DoFinishActions();
            }
            else 
            {
                if (!HasStarted)
                    DoStartActions();

                OnAnimate((float)(time.Ticks - StartDelay.Ticks) / (float)Duration.Ticks);
            }
        }

        internal void DoStartActions()
        {
            OnStart();
            this.HasStarted = true;

            EventHandler<EventArgs> handler = this.Started;

            if (handler != null)
                handler(this, new EventArgs());
        }

        internal void DoFinishActions()
        {
            OnFinish();

            this.Sprite.ActiveAnimation = null;
            this.HasFinished = true;

            EventHandler<EventArgs> handler = this.Finished;

            if (handler != null)
                handler(this, new EventArgs());

            if (this.NextAnimation != null)
            {
                this.spriteManager.ActivateNextAnimation(this, this.NextAnimation);
            }
        }

        protected abstract void OnStart();
        protected abstract void OnAnimate(float t);
        protected abstract void OnFinish();
    }
}