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
    }
}
