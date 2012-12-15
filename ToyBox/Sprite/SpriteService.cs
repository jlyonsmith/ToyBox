using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections;

namespace ToyBox
{
    public class SpriteService : ISpriteService
    {
        private bool visible;
        private IGraphicsService graphicsService;
        private IPlatformService platformService;
        private List<Animation> animations;
        private ReadOnlyCollection<Animation> readOnlyAnimations;
        private List<Sprite> sprites;
        private ReadOnlyCollection<Sprite> readOnlySprites;
        public ReadOnlyCollection<Sprite> Sprites { get { return readOnlySprites; } }
        public ReadOnlyCollection<Animation> Animations { get { return readOnlyAnimations; } }
        public SpriteSortMode SortMode { get; set; }
        public SpriteBlendMode BlendMode { get; set; }
        public bool Visible { get; set; }

        public SpriteService(IServiceProvider services)
        {
			sprites = new List<Sprite>();
            readOnlySprites = sprites.AsReadOnly();
            animations = new List<Animation>();
            readOnlyAnimations = animations.AsReadOnly();
            platformService = services.GetService<IPlatformService>();
            graphicsService = services.GetService<IGraphicsService>();

            platformService.Update += OnUpdate;
            platformService.Draw += OnDraw;
        }

        public void OnUpdate(GameTime gameTime)
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
        }

        public void OnDraw() 
        { 
            if (!Visible)
                return;
            
            foreach (var sprite in sprites)
            {
                if (sprite.Visible)
                    sprite.Draw();
            }
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

        public Texture CreateRenderTarget(int width, int height, IList<SpriteTextureAndPosition> textureAndPositions)
        {
            Texture renderTarget = new Texture(width, height, false, SurfaceFormat.Color, DepthFormat.None);

            if (textureAndPositions != null)
                DrawRenderTarget(renderTarget, textureAndPositions);

            return renderTarget;
        }

        public void DrawRenderTarget(Texture renderTarget, IList<SpriteTextureAndPosition> textureAndPositions)
        {
			throw new NotImplementedException();

#if DONT_COMPILE
            this.Game.GraphicsDevice.SetRenderTarget(renderTarget);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var textureAndPosition in textureAndPositions)
            {
                spriteBatch.Add(
					textureAndPosition.SpriteTexture.Texture, 
					textureAndPosition.Position,
					textureAndPosition.SpriteTexture.SourceRectangle,
                    Color.White,
					0f,
					Vector2.Zero,
					textureAndPosition.SpriteTexture.Scale,
					0f);
            }

            spriteBatch.End();

            this.Game.GraphicsDevice.SetRenderTarget(null);
#endif
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
                    Size lineSize = font.MeasureString(line + ' ' + wordArray[i]);

                    if (lineSize.Width > width)
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