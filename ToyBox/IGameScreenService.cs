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

        /// <summary>Pushes the specified screen onto the screen stack</summary>
        /// <param name="state">State that will be pushed onto the stack</param>
        /// <param name="modality">
        ///   Behavior of the game screen in relation to the screen(s) below it on the stack
        /// </param>
        void Push(IGameScreen screen, GameScreenModality modality);

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

        /// <summary>Switches the game to the specified screen</summary>
        /// <param name="state">State the game will be switched to</param>
        /// <param name="modality">
        ///   Behavior of the game screen in relation to the screen(s) below it on the stack
        /// </param>
        /// <returns>The game screen that was replaced on the stack</returns>
        /// <remarks>
        ///   This replaces the running game screen in the stack with the specified screen.
        /// </remarks>
        IGameScreen Switch(IGameScreen screen, GameScreenModality modality);
    }

    public enum GameScreenModality
    {
        /// <summary>
        /// The game screen takes exclusive of the screen does not require the screen
        /// below it in the stack to be updated as long as it's active.
        /// </summary>
        Exclusive,

        /// <summary>
        /// The game screen sits on top of the screen below it in the stack, but does
        /// not completely obscure it or requires it to continue being updated.
        /// </summary>
        Popup
    }
}
