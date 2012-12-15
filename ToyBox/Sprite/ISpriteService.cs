using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        int HitTest(Point point);
        void FastForwardAnimations();
        SpriteSortMode SortMode { get; set; }
        SpriteBlendMode BlendMode { get; set; }
        bool Visible { get; set; }
    }
}
