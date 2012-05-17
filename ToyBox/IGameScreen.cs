using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ToyBox
{
    public interface IGameScreen
    {
        /// <summary>Called when the game state has been entered</summary>
        void Enter();

        /// <summary>Called when the game state is being left again</summary>
        void Leave();

        /// <summary>Called when the game state is being paused</summary>
        void Pause();

        /// <summary>Called when the game state is being resumed from pause mode</summary>
        void Resume();
    }
}
