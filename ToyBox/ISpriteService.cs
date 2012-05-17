using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using System.Collections;

namespace ToyBox
{
    public interface ISpriteService
    {
        ReadOnlyCollection<Sprite> Sprites { get; }
        ReadOnlyCollection<Animation> Animations { get; }
		void AttachSprite(Sprite sprite, params Set<Sprite>[] spriteSets);
        void DetachSprite(Sprite sprite);
        void DetachAllSprites();
        void AttachAnimation(Sprite sprite, Animation animation, params ActiveAnimationSetUpdater[] animationUpdaters);
        void Draw();
        int HitTest(Point point);
        void FastForwardAnimations();
        RenderTarget2D CreateRenderTarget(int width, int height, IList<TextureAndPosition> textureAndPositions);
        void DrawRenderTarget(RenderTarget2D renderTarget, IList<TextureAndPosition> textureAndPositions);
    }
}
