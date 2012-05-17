using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ToyBox
{
    public class InputManager : GameComponent, IInputService, IUpdateable, IDisposable 
    {
        private static Keys[] allValidKeys;

        public static Keys[] AllValidKeys
        {
            get
            {
                if (allValidKeys == null)
                    allValidKeys = GetAllValidKeys();

                return allValidKeys;
            }
        }

        private ReadOnlyCollection<IGamePad> gamePads;
        private ReadOnlyCollection<IMouse> mice;
        private ReadOnlyCollection<IKeyboard> keyboards;
        private ReadOnlyCollection<ITouchPanel> touchPanels;
        private Keys[] keysWanted;

        public InputManager(Game game) :
            this(game, AllValidKeys)
        {
        }

        public InputManager(Game game, Keys[] keysWanted) :
            base(game)
        {
            this.keysWanted = keysWanted;

            if (game.Services != null)
            {
                game.Services.AddService(typeof(IInputService), this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Game.Services != null)
                {
                    this.Game.Services.RemoveService(typeof(IInputService));
                }

                if (this.touchPanels != null)
                {
                    CollectionHelper.DisposeItems(this.touchPanels);
                    this.touchPanels = null;
                }
                if (this.keyboards != null)
                {
                    CollectionHelper.DisposeItems(this.keyboards);
                    this.keyboards = null;
                }
                if (this.mice != null)
                {
                    CollectionHelper.DisposeItems(this.mice);
                    this.mice = null;
                }
                if (this.gamePads != null)
                {
                    CollectionHelper.DisposeItems(this.gamePads);
                    this.gamePads = null;
                }
            }
        }

        public ReadOnlyCollection<IKeyboard> Keyboards
        {
            get 
            {
                if (this.keyboards == null)
                    SetupKeyboards();

                return this.keyboards; 
            }
        }

        public ReadOnlyCollection<IMouse> Mice
        {
            get { return this.mice; }
        }

        public ReadOnlyCollection<IGamePad> GamePads
        {
            get 
            {
                if (this.gamePads == null)
                    SetupGamePads();

                return this.gamePads; 
            }
        }

        public ReadOnlyCollection<ITouchPanel> TouchPanels
        {
            get 
            {
                if (this.touchPanels == null)
                    SetupTouchPanels();

                return this.touchPanels; 
            }
        }

        public IMouse GetMouse()
        {
            if (this.mice == null)
                SetupMouse();

            return CollectionHelper.GetIfExists(this.mice, 0);
        }

        public IKeyboard GetKeyboard()
        {
            if (this.keyboards == null)
                SetupKeyboards();

            return CollectionHelper.GetIfExists(this.keyboards, 4);
        }

        public IKeyboard GetKeyboard(PlayerIndex playerIndex)
        {
            return this.keyboards[(int)playerIndex];
        }

        public IGamePad GetGamePad(PlayerIndex playerIndex)
        {
            return this.gamePads[(int)playerIndex];
        }

        public ITouchPanel GetTouchPanel()
        {
            if (this.touchPanels == null)
                SetupTouchPanels();

            return this.touchPanels[0];
        }

        public override void Update(GameTime time)
        {
            if (gamePads != null)
            {
                for (int index = 0; index < this.gamePads.Count; ++index)
                {
                    this.gamePads[index].Update();
                }
            }
            if (mice != null)
            {
                for (int index = 0; index < this.mice.Count; ++index)
                {
                    this.mice[index].Update();
                }
            }
            if (keyboards != null)
            {
                for (int index = 0; index < this.keyboards.Count; ++index)
                {
                    this.keyboards[index].Update();
                }
            }
            if (touchPanels != null)
            {
                for (int index = 0; index < this.touchPanels.Count; ++index)
                {
                    this.touchPanels[index].Update();
                }
            }
        }

        private void SetupGamePads()
        {
            var gamePads = new List<IGamePad>();

            // Add default XNA game pads
            for (PlayerIndex player = PlayerIndex.One; player <= PlayerIndex.Four; ++player)
            {
                gamePads.Add(new XBoxGamePad(player));
            }

            // Add place holders for all unattached game pads
            while (gamePads.Count < 8)
            {
                gamePads.Add(new NoGamePad());
            }

            this.gamePads = new ReadOnlyCollection<IGamePad>(gamePads);
        }

        private void SetupMouse()
        {
            var mice = new List<IMouse>();
#if XBOX360
            // Add a dummy mouse
            mice.Add(new NoMouse());
#else
            // Add main PC mouse
            mice.Add(new StandardMouse());
#endif

            this.mice = new ReadOnlyCollection<IMouse>(mice);
        }

        private void SetupKeyboards()
        {
            SetupGamePads();

            var keyboards = new List<IKeyboard>();

            for (PlayerIndex player = PlayerIndex.One; player <= PlayerIndex.Four; ++player)
            {
                keyboards.Add(new GamePadKeyboard(player, this.gamePads[(int)player], this.keysWanted));
            }
#if XBOX360 || WINDOWS_PHONE || MONOTOUCH
            // Add a dummy keyboard
            keyboards.Add(new NoKeyboard(null));
#else 
            keyboards.Add(new StandardKeyboard(this.keysWanted));
#endif
            this.keyboards = new ReadOnlyCollection<IKeyboard>(keyboards);
        }

        private void SetupTouchPanels()
        {
            var touchPanels = new List<ITouchPanel>();

#if WINDOWS_PHONE
            // TODO-john-2012: Add the Windows Phone 7 touch panel
            // touchPanels.Add(new XnaTouchPanel());
#else
            touchPanels.Add(new NoTouchPanel());
#endif

            this.touchPanels = new ReadOnlyCollection<ITouchPanel>(touchPanels);
        }

        public static Keys[] GetAllValidKeys()
        {
            FieldInfo[] fieldInfos = typeof(Keys).GetFields(BindingFlags.Public | BindingFlags.Static);

            // Create an array to hold the enumeration values and copy them over from
            // the fields we just retrieved
            var values = new Keys[fieldInfos.Length];

            for (int index = 0; index < fieldInfos.Length; ++index)
            {
                values[index] = (Keys)fieldInfos[index].GetValue(null);
            }

            return values;
        }
    }
}
