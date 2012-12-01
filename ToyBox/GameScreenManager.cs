using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ToyBox
{
    public class GameScreenManager : DrawableGameComponent, IGameScreenService, IDisposable
    {
        private bool disposeDroppedScreens;
        private List<KeyValuePair<IGameScreen, GameScreenModality>> gameScreens;
        private List<IUpdateable> updateableScreens;
        private List<IDrawable> drawableScreens;

        public GameScreenManager(Game game) :
            base(game)
        {
            this.gameScreens = new List<KeyValuePair<IGameScreen, GameScreenModality>>();
            this.updateableScreens = new List<IUpdateable>();
            this.drawableScreens = new List<IDrawable>();

            if (game.Services != null)
                game.Services.AddService(typeof(IGameScreenService), this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                LeaveAllActiveScreens();

                if (this.Game.Services != null)
                {
                    this.Game.Services.RemoveService(typeof(IGameScreenService));
                }
            }

            base.Dispose(disposing);
        }

        public bool DisposeDroppedScreens
        {
            get { return this.disposeDroppedScreens; }
            set { this.disposeDroppedScreens = value; }
        }

        public void Pause()
        {
            if (this.gameScreens.Count > 0)
            {
                this.gameScreens[this.gameScreens.Count - 1].Key.Pause();
            }
        }

        public void Resume()
        {
            if (this.gameScreens.Count > 0)
            {
                this.gameScreens[this.gameScreens.Count - 1].Key.Resume();
            }
        }

        public void Push(IGameScreen screen)
        {
            Push(screen, GameScreenModality.Exclusive);
        }

        public void Push(IGameScreen screen, GameScreenModality modality)
        {
            Pause();

            // If this game screen is modal, take all game screens that came before it
            // from the draw and update lists
            if (modality == GameScreenModality.Exclusive)
            {
                this.drawableScreens.Clear();
                this.updateableScreens.Clear();
            }

            // Add the new screen to the update and draw lists if it implements
            // the required interfaces
            this.gameScreens.Add(new KeyValuePair<IGameScreen, GameScreenModality>(screen, modality));
            AppendToUpdateableAndDrawableList(screen);

            // Screen is set, now try to enter it
#if DEBUG
            screen.Enter();
#else
            try
            {
                screen.Enter();
            }
            catch (Exception)
            {
                Pop();
                throw;
            }
#endif
        }

        public IGameScreen Pop()
        {
            int lastScreenIndex = this.gameScreens.Count - 1;
            
            if (lastScreenIndex < 0)
            {
                throw new InvalidOperationException("No game screens are on the stack");
            }

            KeyValuePair<IGameScreen, GameScreenModality> old = this.gameScreens[lastScreenIndex];

            // Notify the currently active screen that it's being left and take it
            // from the stack of active screens
            old.Key.Leave();
            this.gameScreens.RemoveAt(lastScreenIndex);

            // Now we need to remove the popped screen from our update and draw lists.
            // If the popped screen was exclusive, our lists are empty and we need to
            // rebuild them. Otherwise, we can simply remove the lastmost entry.
            if (old.Value == GameScreenModality.Exclusive)
            {
                this.updateableScreens.Clear();
                this.drawableScreens.Clear();
                RebuildUpdateableAndDrawableListRecursively(lastScreenIndex - 1);
            }
            else
            {
                RemoveFromUpdateableAndDrawableList(old.Key);
            }

            // If the user desires so, dispose the dropped screen
            DisposeIfSupportedAndDesired(old.Key);

            // Resume the screen that has now become the top of the stack
            Resume();

            return old.Key;
        }

        public IGameScreen Switch(IGameScreen screen)
        {
            return Switch(screen, GameScreenModality.Exclusive);
        }

        public IGameScreen Switch(IGameScreen screen, GameScreenModality modality)
        {
            int screenCount = this.gameScreens.Count;
            
            if (screenCount == 0)
            {
                Push(screen, modality);
                return null;
            }

            int lastScreenIndex = screenCount - 1;
            KeyValuePair<IGameScreen, GameScreenModality> old = this.gameScreens[lastScreenIndex];
            IGameScreen previousScreen = old.Key;

            // Notify the previous screen that it's being left and kill it if desired
            previousScreen.Leave();
            DisposeIfSupportedAndDesired(previousScreen);

            // If the switched-to screen is exclusive, we need to clear the update
            // and draw lists. If not, depending on whether the previous screen was
            // a popup screen, we might have to 
            if (old.Value == GameScreenModality.Popup)
            {
                RemoveFromUpdateableAndDrawableList(previousScreen);
            }
            else
            {
                this.updateableScreens.Clear();
                this.drawableScreens.Clear();
            }

            // Now swap out the screen and put it in the update and draw lists. If we're
            // switching from an exclusive to a pop-up screen, the draw and update lists need
            // to be rebuilt.
            var newScreen = new KeyValuePair<IGameScreen, GameScreenModality>(screen, modality);
            this.gameScreens[lastScreenIndex] = newScreen;
            if (old.Value == GameScreenModality.Exclusive && modality == GameScreenModality.Popup)
            {
                RebuildUpdateableAndDrawableListRecursively(lastScreenIndex);
            }
            else
            {
                AppendToUpdateableAndDrawableList(screen);
            }

            // Let the screen know that it has been entered
            screen.Enter();

            return previousScreen;
        }

        public IGameScreen ActiveScreen
        {
            get
            {
                int count = this.gameScreens.Count;
                if (count == 0)
                {
                    return null;
                }
                else
                {
                    return this.gameScreens[count - 1].Key;
                }
            }
        }

        public IGameScreen First(Type type)
        {
            foreach (var gameScreen in gameScreens)
            {
                if (gameScreen.Key.GetType() == type)
                    return gameScreen.Key;
            }

            return null;
        }

        public override void Update(GameTime gameTime)
        {
            for (int index = 0; index < this.updateableScreens.Count; ++index)
            {
                var updateable = this.updateableScreens[index];
                if (updateable.Enabled)
                {
                    updateable.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            for (int index = 0; index < this.drawableScreens.Count; ++index)
            {
                var drawable = this.drawableScreens[index];
                if (drawable.Visible)
                {
                    this.drawableScreens[index].Draw(gameTime);
                }
            }
        }

        private void DisposeIfSupportedAndDesired(IGameScreen screen)
        {
            if (this.disposeDroppedScreens)
            {
                var disposable = screen as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void RebuildUpdateableAndDrawableListRecursively(int index)
        {
            if (index < 0)
            {
                return;
            }

            if (this.gameScreens[index].Value != GameScreenModality.Exclusive)
            {
                RebuildUpdateableAndDrawableListRecursively(index - 1);
            }

            AppendToUpdateableAndDrawableList(this.gameScreens[index].Key);
        }

        private void RemoveFromUpdateableAndDrawableList(IGameScreen screen)
        {
            int lastDrawableIndex = this.drawableScreens.Count - 1;
            
            if (lastDrawableIndex > -1)
            {
                if (ReferenceEquals(this.drawableScreens[lastDrawableIndex], screen))
                {
                    this.drawableScreens.RemoveAt(lastDrawableIndex);
                }
            }

            int lastUpdateableIndex = this.updateableScreens.Count - 1;
            if (lastUpdateableIndex > -1)
            {
                if (ReferenceEquals(this.updateableScreens[lastUpdateableIndex], screen))
                {
                    this.updateableScreens.RemoveAt(lastUpdateableIndex);
                }
            }
        }

        private void LeaveAllActiveScreens()
        {
            for (int index = this.gameScreens.Count - 1; index >= 0; --index)
            {
                IGameScreen screen = this.gameScreens[index].Key;
                screen.Leave();
                DisposeIfSupportedAndDesired(screen);
                this.gameScreens.RemoveAt(index);
            }

            this.drawableScreens.Clear();
            this.updateableScreens.Clear();
        }

        private void AppendToUpdateableAndDrawableList(IGameScreen screen)
        {
            IUpdateable updateable = screen as IUpdateable;
            if (updateable != null)
            {
                this.updateableScreens.Add(updateable);
            }

            IDrawable drawable = screen as IDrawable;
            if (drawable != null)
            {
                this.drawableScreens.Add(drawable);
            }
        }
    }
} 
