using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace ToyBox
{
    public class TextureAndPosition
    {
        public TextureAndPosition(Texture2D texture, Point point, Rectangle? sourceRectangle)
            : this(texture, point.X, point.Y, sourceRectangle)
        {
        }

        public TextureAndPosition(Texture2D texture, int x, int y, Rectangle? sourceRectangle)
        {
            this.Texture = texture;
            this.SourceRectangle = sourceRectangle.HasValue ?
                sourceRectangle.Value : new Rectangle(0, 0, texture.Width, texture.Height);
            this.Position = new Vector2((float)x, (float)y);
        }

        public Texture2D Texture { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public Vector2 Position { get; private set; } 
    }

    public class SpriteEventArgs : EventArgs
    {
        public SpriteEventArgs(Sprite sprite)
        {
            this.Sprite = sprite;
        }

        public Sprite Sprite { get; private set; }
    }

    public class SpriteManager : GameComponent, ISpriteService
    {
        private SpriteBatch spriteBatch;
        private List<Animation> animations;
        private ReadOnlyCollection<Animation> readOnlyAnimations;
        private List<Sprite> sprites;
        private ReadOnlyCollection<Sprite> readOnlySprites;
        
        public ReadOnlyCollection<Sprite> Sprites { get { return readOnlySprites; } }
        public ReadOnlyCollection<Animation> Animations { get { return readOnlyAnimations; } }

        public SpriteManager(Game game)
            : base(game)
        {
            Enabled = true;
			sprites = new List<Sprite>();
            readOnlySprites = sprites.AsReadOnly();
            animations = new List<Animation>();
            readOnlyAnimations = animations.AsReadOnly();

            if (this.Game.Services != null)
            {
                this.Game.Services.AddService(typeof(ISpriteService), this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Game.Services != null)
                {
                    this.Game.Services.RemoveService(typeof(ISpriteService));
                }
            }

            base.Dispose(disposing);
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                Animation animation = this.animations[i];

                if (animation != null)
                {
                    animation.Update(gameTime);

                    if (animation.HasFinished)
                        animations[i] = null;
                }
            }

            // ONLY delete entries from the animations array in 
            // one place because the Started/Finished callbacks
            // might cause animations to finish
            for (int i = 0; i < animations.Count; )
            {
                if (animations[i] == null)
                    animations.RemoveAt(i);
                else
                    i++;
            }

            base.Update(gameTime);
        }
		
        public void AttachSprite(Sprite sprite, params Set<Sprite>[] spriteSets)
        {
#if DEBUG
            if (sprites.Contains(sprite))
                throw new ArgumentException("Sprite attached multiple times");
#endif

            this.sprites.Add(sprite);

            foreach (var spriteSet in spriteSets)
            {
                spriteSet.Add(sprite);
            }
        }

        public void AttachAnimation(Sprite sprite, Animation animation, params ActiveAnimationSetUpdater[] animationUpdaters)
        {
            animation.Initialize(this, sprite);

            foreach (var animationUpdater in animationUpdaters)
            {
                animationUpdater.Add(animation);
            }

            Animation activeAnimation = sprite.ActiveAnimation;

            if (activeAnimation == null)
            {
                sprite.ActiveAnimation = animation;
                this.animations.Add(animation);
            }
            else
            {
                while (activeAnimation.NextAnimation != null)
                {
                    activeAnimation = activeAnimation.NextAnimation;

                    if (activeAnimation == animation)
                        throw new ArgumentException("Same animation attached more than once");
                }

                activeAnimation.NextAnimation = animation;
            }
        }

        internal void ActivateNextAnimation(Animation animation, Animation nextAnimation)
        {
            this.animations.Add(nextAnimation);
        }

        public void FastForwardAnimations()
        {
            while (animations.Count(a => a != null) > 0)
            {
                for (int i = 0; i < animations.Count; i++)
                {
                    Animation animation = this.animations[i];

                    if (animation != null)
                    {
                        if (!animation.HasStarted)
                        {
                            animation.DoStartActions();
                        }
                        else if (!animation.HasFinished)
                        {
                            animation.DoFinishActions();
                        }
                        else
                        {
                            animations[i] = null;
                        }
                    }
                }
            }
        }

        public void DetachSprite(Sprite sprite)
        {
            this.sprites.Remove(sprite);
        }

        public void DetachAllSprites()
        {
            this.sprites.Clear();
        }

        public void Draw()
        {
            if (!Enabled)
				return;
			
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (var sprite in sprites)
            {
                if (sprite.Visible)
                    sprite.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public int HitTest(Point point)
        {
            int minDepthFound = Sprite.MaxDepth;
            int foundIndex = -1;

            for (int i = 0; i < sprites.Count; i++)
            {
                Sprite sprite = sprites[i];

                if (sprite.Visible && 
                    sprite.HitTestable &&
                    sprite.Rectangle.Contains(point) && 
                    sprite.Depth < minDepthFound)
                {
                    foundIndex = i;
                    minDepthFound = sprite.Depth;
                }
            }

            return foundIndex;
        }

        public RenderTarget2D CreateRenderTarget(int width, int height, IList<TextureAndPosition> textureAndPositions)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(
                this.Game.GraphicsDevice, width, height,
                false, SurfaceFormat.Color, DepthFormat.None);

            if (textureAndPositions != null)
                DrawRenderTarget(renderTarget, textureAndPositions);

            return renderTarget;
        }

        public void DrawRenderTarget(RenderTarget2D renderTarget, IList<TextureAndPosition> textureAndPositions)
        {
            this.Game.GraphicsDevice.SetRenderTarget(renderTarget);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var textureAndPosition in textureAndPositions)
            {
                spriteBatch.Draw(textureAndPosition.Texture, textureAndPosition.Position,
                    textureAndPosition.SourceRectangle, Color.White);
            }

            spriteBatch.End();

            this.Game.GraphicsDevice.SetRenderTarget(null);
        }

        private string InsertNewLines(string text, SpriteFont font, int width, ref int height)
        {
            String line = String.Empty;
            String newText = String.Empty;
            String[] wordArray = text.Split(' ');
            int i = 0;

            height = 0;

            while (i < wordArray.Length)
            {
                // Start of the line; add the first word
                line += wordArray[i++];

                while (i < wordArray.Length)
                {
                    // More words to add
                    Vector2 lineSize = font.MeasureString(line + ' ' + wordArray[i]);

                    if (lineSize.X > width)
                    {
                        // This word pushes us to the next line
                        newText += line + '\n';
                        line = String.Empty;
                        height += font.LineSpacing;
                        break;
                    }
                    else
                    {
                        line += ' ' + wordArray[i++];
                    }
                }
            }

            newText += line;

            return newText;
        }
    }
}
