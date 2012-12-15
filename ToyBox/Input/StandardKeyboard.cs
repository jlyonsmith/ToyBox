using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyBox
{
    internal class StandardKeyboard : IKeyboard
    {
        private readonly Keys[] validKeys;
        private KeyboardState current;

        public event KeyboardButtonDelegate ButtonPressed;
        public event KeyboardButtonDelegate ButtonReleased;
        public event CharacterDelegate CharacterEntered;
        
        public StandardKeyboard(Keys[] keysWanted)
        {
            this.current = new KeyboardState();
            this.validKeys = keysWanted;
        }

        public KeyboardState GetState()
        {
            return this.current;
        }

        public bool IsAttached
        {
            get { return true; }
        }

        public string Name
        {
            get 
            {
                return "StandardKeyboard"; 
            }
        }

        public void Update()
        {
            KeyboardState previous = this.current;

            this.current = Keyboard.GetState();

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

        private void GenerateEvents(ref KeyboardState previous, ref KeyboardState current)
        {
            if ((ButtonPressed == null) && (ButtonReleased == null) && (CharacterEntered == null))
            {
                return;
            }

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
                    }
                    else
                    {
                        RaiseKeyReleased(key);
                    }
                }
            }
        }
    }
}
