using System;
using System.Collections.Generic;
using System.Text;

namespace ToyBox
{
    public interface IGameScreenService
    {
        /// <summary>The currently active game screen. Can be null.</summary>
        IGameScreen ActiveScreen { get; }

        /// <summary>Pauses the currently active screen</summary>
        void Pause();

        /// <summary>Resumes the currently active screen</summary>
        void Resume();

        /// <summary>Pushes the specified screen onto the screen stack</summary>
        /// <param name="state">State that will be pushed onto the stack</param>
        void Push(IGameScreen screen);

        /// <summary>Takes the currently active game screen from the stack</summary>
        /// <returns>The game screen that has been popped from the stack</returns>
        IGameScreen Pop();

        /// <summary>Switches the game to the specified screen</summary>
        /// <param name="state">State the game will be switched to</param>
        /// <returns>The game screen that was replaced on the stack</returns>
        /// <remarks>
        ///   This replaces the running game screen in the stack with the specified screen.
        /// </remarks>
        IGameScreen Switch(IGameScreen screen);

        bool DisposeDroppedScreens { get; set; }
    }
}
