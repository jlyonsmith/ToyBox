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
using System.Collections;
using System.Collections.ObjectModel;

namespace ToyBox
{
    public class SpriteTexture
    {
        public Texture2D Texture { get; set; }
        public Rectangle Rectangle { get; set; }

        public SpriteTexture(Texture2D texture, Rectangle? rect)
        {
            Texture = texture;
            Rectangle = rect.HasValue ? rect.Value : new Rectangle(0, 0, texture.Width, texture.Height);
        }
    }

}
