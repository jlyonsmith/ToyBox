using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace ToyBox
{
    public class SpriteTexture
    {
		/// <summary>
		/// The texture containing within it actual sprite texture
		/// </summary>
        public Texture Texture { get; private set; }

		/// <summary>
		/// The rectangle in the texture to use for the sprite texture
		/// </summary>
        public Rectangle SourceRectangle { get; private set; }

		/// <summary>
		/// The actual size of the sprite texture after scaling
		/// </summary>
		public Size DestinationSize { get; private set; }

		public Vector2 Scale { get; private set; }

        public SpriteTexture(Texture texture)
        {
            this.Texture = texture;
            this.SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
			this.DestinationSize = new Size(texture.Width, texture.Height);
			this.Scale = new Vector2(1.0f, 1.0f);
        }

		public SpriteTexture(Texture texture, Rectangle sourceRect)
		{
			this.Texture = texture;
			this.SourceRectangle = sourceRect;
			this.DestinationSize = new Size(sourceRect.Width, sourceRect.Height);
			this.Scale = new Vector2(1.0f, 1.0f);
		}
		
		public SpriteTexture(Texture texture, Size destSize)
		{
			this.Texture = texture;
			this.SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
			this.DestinationSize = destSize;
			this.Scale = new Vector2(
				(float)destSize.Width / texture.Width, 
				(float)destSize.Height / texture.Height);
		}
		
		public SpriteTexture(Texture texture, Rectangle sourceRect, Size destSize)
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
