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
		/// <summary>
		/// The texture containing within it actual sprite texture
		/// </summary>
        public Texture2D Texture { get; private set; }

		/// <summary>
		/// The rectangle in the texture to use for the sprite texture
		/// </summary>
        public Rectangle SourceRectangle { get; private set; }

		/// <summary>
		/// The actual size of the sprite texture after scaling
		/// </summary>
		public Size DestinationSize { get; private set; }

		public Vector2 Scale { get; private set; }

        public SpriteTexture(Texture2D texture)
        {
            this.Texture = texture;
            this.SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
			this.DestinationSize = new Size(texture.Width, texture.Height);
			this.Scale = new Vector2(1.0f, 1.0f);
        }

		public SpriteTexture(Texture2D texture, Rectangle sourceRect)
		{
			this.Texture = texture;
			this.SourceRectangle = sourceRect;
			this.DestinationSize = new Size(sourceRect.Width, sourceRect.Height);
			this.Scale = new Vector2(1.0f, 1.0f);
		}
		
		public SpriteTexture(Texture2D texture, Size destSize)
		{
			this.Texture = texture;
			this.SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
			this.DestinationSize = destSize;
			this.Scale = new Vector2(
				(float)destSize.Width / texture.Width, 
				(float)destSize.Height / texture.Height);
		}
		
		public SpriteTexture(Texture2D texture, Rectangle sourceRect, Size destSize)
		{
			this.Texture = texture;
			this.SourceRectangle = sourceRect;
			this.DestinationSize = destSize;
			this.Scale = new Vector2(
				(float)destSize.Width / sourceRect.Width, 
				(float)destSize.Height / sourceRect.Height);
		}
	}

}
