using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ToyBox
{
    public class GameScreenService : IGameScreenService, IDisposable
    {
        private bool disposeDroppedScreens;
        private List<IGameScreen> gameScreens;

        public GameScreenService(IServiceProvider services)
        {
            this.gameScreens = new List<IGameScreen>();
        }

        public void Dispose()
        {
            LeaveAllActiveScreens();
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
                this.gameScreens[this.gameScreens.Count - 1].Pause();
            }
        }

        public void Resume()
        {
            if (this.gameScreens.Count > 0)
            {
                this.gameScreens[this.gameScreens.Count - 1].Resume();
            }
        }

        public void Push(IGameScreen screen)
        {
            Pause();

            // Add the new screen to the update and draw lists if it implements
            // the required interfaces
            this.gameScreens.Add(screen);

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

            IGameScreen old = this.gameScreens[lastScreenIndex];

            // Notify the currently active screen that it's being left and take it
            // from the stack of active screens
            old.Leave();
            this.gameScreens.RemoveAt(lastScreenIndex);

            // If the user desires so, dispose the dropped screen
            DisposeIfSupportedAndDesired(old);

            // Resume the screen that has now become the top of the stack
            Resume();

            return old;
        }

        public IGameScreen Switch(IGameScreen screen)
        {
            int screenCount = this.gameScreens.Count;
            
            if (screenCount == 0)
            {
                Push(screen);
                return null;
            }

            int lastScreenIndex = screenCount - 1;
            IGameScreen old = this.gameScreens[lastScreenIndex];
            IGameScreen previousScreen = old;

            // Notify the previous screen that it's being left and kill it if desired
            previousScreen.Leave();
            DisposeIfSupportedAndDesired(previousScreen);

            // Now swap out the screen and put it in the update and draw lists. If we're
            // switching from an exclusive to a pop-up screen, the draw and update lists need
            // to be rebuilt.
            this.gameScreens[lastScreenIndex] = screen;

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
                    return this.gameScreens[count - 1];
                }
            }
        }

        public IGameScreen First(Type type)
        {
            foreach (var gameScreen in gameScreens)
            {
                if (gameScreen.GetType() == type)
                    return gameScreen;
            }

            return null;
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

        private void LeaveAllActiveScreens()
        {
            for (int index = this.gameScreens.Count - 1; index >= 0; --index)
            {
                IGameScreen screen = this.gameScreens[index];
                screen.Leave();
                DisposeIfSupportedAndDesired(screen);
                this.gameScreens.RemoveAt(index);
            }
        }
    }
} 
