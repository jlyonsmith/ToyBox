using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MsGamePad = Microsoft.Xna.Framework.Input.GamePad;

namespace ToyBox
{
    internal class XBoxGamePad : IGamePad
    {
        private string name;
        private PlayerIndex playerIndex;
        private Queue<GamePadState> states;
        private GamePadState current;

        public event GamePadButtonDelegate ButtonPressed;
        public event GamePadButtonDelegate ButtonReleased;

        public XBoxGamePad(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
            this.states = new Queue<GamePadState>();
            this.current = new GamePadState();
        }

        /// <summary>Retrieves the current state of the game pad</summary>
        /// <returns>The current state of the game pad</returns>
        public GamePadState GetState()
        {
            return this.current;
        }

        /// <summary>Whether the input device is connected to the system</summary>
        public bool IsAttached
        {
            get { return this.current.IsConnected; }
        }

        /// <summary>Human-readable name of the input device</summary>
        public string Name
        {
            get
            {
                if (name == null)
                    name = "XBoxGamePad" + ((int)this.playerIndex + 1).ToString();

                return name;
            }
        }

        public void Update()
        {
            GamePadState previous = this.current;

            if (this.states.Count == 0)
            {
                this.current = MsGamePad.GetState(this.playerIndex);
            }
            else
            {
                this.current = this.states.Dequeue();
            }

            GenerateEvents(ref previous, ref this.current);
        }

        /// <summary>Checks for state changes and triggers the corresponding events</summary>
        /// <param name="previous">Previous state of the game pad</param>
        /// <param name="current">Current state of the game pad</param>
        protected void GenerateEvents(ref GamePadState previous, ref GamePadState current)
        {
            if (!(ButtonPressed != null) || (ButtonReleased != null))
            {
                return;
            }

            Buttons pressedButtons = 0;
            Buttons releasedButtons = 0;
            ulong pressedExtendedButtons = 0;
            ulong releasedExtendedButtons = 0;

            // See if the state of the 'A' button has changed between two polls
            if (current.Buttons.A != previous.Buttons.A)
            {
                if (current.Buttons.A == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.A;
                    pressedExtendedButtons |= (1 << 0);
                }
                else
                {
                    releasedButtons |= Buttons.A;
                    releasedExtendedButtons |= (1 << 0);
                }
            }

            // See if the state of the 'B' button has changed between two polls
            if (current.Buttons.B != previous.Buttons.B)
            {
                if (current.Buttons.B == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.B;
                    pressedExtendedButtons |= (1 << 1);
                }
                else
                {
                    releasedButtons |= Buttons.B;
                    releasedExtendedButtons |= (1 << 1);
                }
            }

            // See if the state of the 'X' button has changed between two polls
            if (current.Buttons.X != previous.Buttons.X)
            {
                if (current.Buttons.X == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.X;
                    pressedExtendedButtons |= (1 << 2);
                }
                else
                {
                    releasedButtons |= Buttons.X;
                    releasedExtendedButtons |= (1 << 2);
                }
            }

            // See if the state of the 'A' button has changed between two polls
            if (current.Buttons.Y != previous.Buttons.Y)
            {
                if (current.Buttons.Y == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.Y;
                    pressedExtendedButtons |= (1 << 3);
                }
                else
                {
                    releasedButtons |= Buttons.Y;
                    releasedExtendedButtons |= (1 << 3);
                }
            }

            // See if the state of the left shoulder button has changed between two polls
            if (current.Buttons.LeftShoulder != previous.Buttons.LeftShoulder)
            {
                if (current.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.LeftShoulder;
                    pressedExtendedButtons |= (1 << 4);
                }
                else
                {
                    releasedButtons |= Buttons.LeftShoulder;
                    releasedExtendedButtons |= (1 << 4);
                }
            }

            // See if the state of the right shoulder button has changed between two polls
            if (current.Buttons.RightShoulder != previous.Buttons.RightShoulder)
            {
                if (current.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.RightShoulder;
                    pressedExtendedButtons |= (1 << 5);
                }
                else
                {
                    releasedButtons |= Buttons.RightShoulder;
                    releasedExtendedButtons |= (1 << 5);
                }
            }

            // See if the state of the back button has changed between two polls
            if (current.Buttons.Back != previous.Buttons.Back)
            {
                if (current.Buttons.Back == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.Back;
                    pressedExtendedButtons |= (1 << 6);
                }
                else
                {
                    releasedButtons |= Buttons.Back;
                    releasedExtendedButtons |= (1 << 6);
                }
            }

            // See if the state of the start button has changed between two polls
            if (current.Buttons.Start != previous.Buttons.Start)
            {
                if (current.Buttons.Start == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.Start;
                    pressedExtendedButtons |= (1 << 7);
                }
                else
                {
                    releasedButtons |= Buttons.Start;
                    releasedExtendedButtons |= (1 << 7);
                }
            }

            // See if the state of the left stick button has changed between two polls
            if (current.Buttons.LeftStick != previous.Buttons.LeftStick)
            {
                if (current.Buttons.LeftStick == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.LeftStick;
                    pressedExtendedButtons |= (1 << 8);
                }
                else
                {
                    releasedButtons |= Buttons.LeftStick;
                    releasedExtendedButtons |= (1 << 8);
                }
            }


            // See if the state of the right stick button has changed between two polls
            if (current.Buttons.RightStick != previous.Buttons.RightStick)
            {
                if (current.Buttons.RightStick == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.RightStick;
                    pressedExtendedButtons |= (1 << 9);
                }
                else
                {
                    releasedButtons |= Buttons.RightStick;
                    releasedExtendedButtons |= (1 << 9);
                }
            }

            // See if the state of the back button has changed between two polls
            if (current.Buttons.BigButton != previous.Buttons.BigButton)
            {
                if (current.Buttons.BigButton == ButtonState.Pressed)
                {
                    pressedButtons |= Buttons.BigButton;
                    pressedExtendedButtons |= (1 << 10);
                }
                else
                {
                    releasedButtons |= Buttons.BigButton;
                    releasedExtendedButtons |= (1 << 10);
                }
            }

            if (releasedButtons != 0)
            {
                RaiseButtonReleased(releasedButtons);
            }
            if (pressedButtons != 0)
            {
                RaiseButtonPressed(pressedButtons);
            }
        }

        protected void RaiseButtonPressed(Buttons buttons)
        {
            if (ButtonPressed != null)
            {
                ButtonPressed(buttons);
            }
        }

        /// <summary>Fires the ButtonReleased event</summary>
        /// <param name="buttons">Buttons that have been released</param>
        protected void RaiseButtonReleased(Buttons buttons)
        {
            if (ButtonReleased != null)
            {
                ButtonReleased(buttons);
            }
        }
    }
}
