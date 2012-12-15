using System;
using System.Collections.Generic;
using System.Reflection;

namespace ToyBox
{
    internal partial class GamePadKeyboard : IKeyboard
    {
        private string name;
        private PlayerIndex playerIndex;
        private IGamePad gamePad;
        private KeyboardState current;

        public event KeyboardButtonDelegate ButtonPressed;
        public event KeyboardButtonDelegate ButtonReleased;
        public event CharacterDelegate CharacterEntered;
        
        public GamePadKeyboard(PlayerIndex playerIndex, IGamePad gamePad)
        {
            this.playerIndex = playerIndex;
            this.gamePad = gamePad;
            this.current = new KeyboardState();
        }

        public KeyboardState GetState()
        {
            return this.current;
        }

        public bool IsAttached
        {
            get { return this.gamePad.IsAttached; }
        }

        public string Name
        {
            get 
            {
                if (name == null)
                    name = "GamePadKeyboard" + playerIndex.ToString();
                
                return name; 
            }
        }

        public void Update()
        {
            KeyboardState previous = this.current;

            this.current = QueryKeyboardState();

            GenerateEvents(ref previous, ref this.current);
        }

        protected void RaiseKeyPressed(Keys key)
        {
            if (ButtonPressed != null)
            {
                ButtonPressed(key);
            }
        }

        protected void RaiseKeyReleased(Keys key)
        {
            if (ButtonReleased != null)
            {
                ButtonReleased(key);
            }
        }

        protected void RaiseCharacterEntered(char character)
        {
            if (CharacterEntered != null)
            {
                CharacterEntered(character);
            }
        }

        private KeyboardState QueryKeyboardState()
        {
            if (this.gamePad.IsAttached)
            {
                return Keyboard.GetState(this.playerIndex);
            }
            else
            {
                return new KeyboardState();
            }
        }

        private void GenerateEvents(ref KeyboardState previous, ref KeyboardState current)
        {
            if ((ButtonPressed == null) && (ButtonReleased == null) && (CharacterEntered == null))
            {
                return;
            }

#if DONT_COMPILE
            // Check all keys for changes between the two provided states
            for (int keyIndex = 0; keyIndex < validKeys.Length; ++keyIndex)
            {
                Keys key = validKeys[keyIndex];

                KeyState previousState = previous[key];
                KeyState currentState = current[key];

                // If this key changed state, report it
                if (previousState != currentState)
                {
                    if (currentState == KeyState.Down)
                    {
                        RaiseKeyPressed(key);
                        GenerateCharacterEvent(key);
                    }
                    else
                    {
                        RaiseKeyReleased(key);
                    }
                }
            }
#endif
        }

        private void GenerateCharacterEvent(Keys key)
        {
            throw new NotImplementedException();
#if DONT_COMPILE
            char character = characterMap[(int)key];
            if (character == '\0')
            {
                return;
            }

            bool isShiftPressed =
              this.current.IsKeyDown(Keys.LeftShift) ||
              this.current.IsKeyDown(Keys.RightShift);

            if (isShiftPressed)
            {
                OnCharacterEntered(char.ToUpper(character));
            }
            else
            {
                OnCharacterEntered(character);
            }
#endif // DONT_COMPILE
        }
    }
}
