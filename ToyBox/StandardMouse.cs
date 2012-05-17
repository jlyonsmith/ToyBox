using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ToyBox
{
    internal class StandardMouse : IMouse
    {
        private MouseState current;

        public event MouseMovedDelegate Moved;
        public event MouseButtonDelegate ButtonPressed;
        public event MouseButtonDelegate ButtonReleased;
        public event MouseWheelDelegate WheelRotated;

        public MouseState GetState()
        {
            return current;
        }

        public void MoveTo(Point point)
        {
            throw new NotImplementedException();
        }

        public bool IsAttached
        {
            get { return true; }
        }

        public string Name
        {
            get { return "Mouse"; }
        }

        public void Update()
        {
            MouseState previous = this.current;

            this.current = Mouse.GetState();

            GenerateEvents(ref previous, ref current);
        }

        private void GenerateEvents(ref MouseState previous, ref MouseState current)
        {
            if (Moved == null && ButtonPressed == null && ButtonReleased == null && WheelRotated == null)
                return;

            if (previous.X != current.X || previous.Y != current.Y)
            {
                RaiseMouseMoved(new Point(current.X, current.Y));
            }

            MouseButtons buttonsReleased = 0;
            MouseButtons buttonsPressed = 0;

            if (previous.LeftButton != current.LeftButton)
            {
                if (current.LeftButton == ButtonState.Pressed)
                    buttonsPressed |= MouseButtons.Left;
                else
                    buttonsReleased |= MouseButtons.Left;
            }

            if (previous.RightButton != current.RightButton)
            {
                if (current.RightButton == ButtonState.Pressed)
                    buttonsPressed |= MouseButtons.Right;
                else
                    buttonsReleased |= MouseButtons.Right;
            }

            if (previous.MiddleButton != current.MiddleButton)
            {
                if (current.MiddleButton == ButtonState.Pressed)
                    buttonsPressed |= MouseButtons.Middle;
                else
                    buttonsReleased |= MouseButtons.Middle;
            }

            if (previous.XButton1 != current.XButton1)
            {
                if (current.XButton1 == ButtonState.Pressed)
                    buttonsPressed |= MouseButtons.X1;
                else
                    buttonsReleased |= MouseButtons.X1;
            }

            if (previous.XButton2 != current.XButton2)
            {
                if (current.LeftButton == ButtonState.Pressed)
                    buttonsPressed |= MouseButtons.X2;
                else
                    buttonsReleased |= MouseButtons.X2;
            }

            if (buttonsPressed != 0)
                RaiseMouseButtonPressed(buttonsPressed);

            if (buttonsReleased != 0)
                RaiseMouseButtonReleased(buttonsReleased);
        }

        private void RaiseMouseButtonPressed(MouseButtons buttons)
        {
            if (ButtonPressed != null)
            {
                ButtonPressed(buttons);
            }
        }

        private void RaiseMouseButtonReleased(MouseButtons buttons)
        {
            if (ButtonReleased != null)
            {
                ButtonReleased(buttons);
            }
        }

        private void RaiseMouseMoved(Point point)
        {
            if (Moved != null)
            {
                this.Moved(point);
            }
        }
    }
}
