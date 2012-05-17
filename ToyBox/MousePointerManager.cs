using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ToyBox
{
    public class MousePointerManager : GameComponent, IMousePointerService
    {
        private IInputService inputService;
#if WINDOWS
        private ISpriteService spriteService;
        private Texture2D mousePointerTexture;
        private TextureSprite mouseSprite;
#endif
        private Point mousePosition;

        public MousePointerManager(Game game)
            : base(game)
        {
            if (this.Game.Services != null)
            {
                this.Game.Services.AddService(typeof(IMousePointerService), this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
#if WINDOWS
                this.spriteService.DetachSprite(mouseSprite);
#endif

                if (this.Game.Services != null)
                {
                    this.Game.Services.RemoveService(typeof(IMousePointerService));
                }
            }

            base.Dispose(disposing);
        }

        public override void Initialize()
        {
            base.Initialize();

            inputService = (IInputService)this.Game.Services.GetService(typeof(IInputService));

#if WINDOWS
            spriteService = (ISpriteService)this.Game.Services.GetService(typeof(ISpriteService));

            this.mousePointerTexture = this.Game.Content.Load<Texture2D>("Windows/Textures/Arrow");
            this.mouseSprite = new TextureSprite(
                    new SpriteTexture(this.mousePointerTexture, null),
                    new Point(-this.mousePointerTexture.Width, -this.mousePointerTexture.Height),
                    0, true, null);
            
            spriteService.AttachSprite(this.mouseSprite);
#endif

            IMouse mouse = this.inputService.GetMouse();
            MouseState mouseState = mouse.GetState();

            mousePosition = new Point (mouseState.X, mouseState.Y);

            mouse.Moved += new MouseMovedDelegate(Mouse_Moved);
        }

        private void Mouse_Moved(Point point)
        {
            mousePosition = point;
#if WINDOWS
            this.mouseSprite.Position = mousePosition;
#endif
        }

        #region IMouseArrowService Members

        public Point Position
        {
            get { return mousePosition; }
        }

        #endregion
    }
}
