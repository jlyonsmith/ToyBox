using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ToyBox
{
    public interface IInputService
    {
        ReadOnlyCollection<IKeyboard> Keyboards { get; }
        ReadOnlyCollection<IMouse> Mice { get; }
        ReadOnlyCollection<IGamePad> GamePads { get; }
        ReadOnlyCollection<ITouchPanel> TouchPanels { get; }

        IMouse GetMouse();
        IKeyboard GetKeyboard();
        IKeyboard GetKeyboard(PlayerIndex playerIndex);
        IGamePad GetGamePad(PlayerIndex playerIndex);
        ITouchPanel GetTouchPanel();
    }

}
