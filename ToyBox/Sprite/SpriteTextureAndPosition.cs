using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections;

namespace ToyBox
{
	public class SpriteTextureAndPosition
	{
		public SpriteTextureAndPosition(SpriteTexture texture, Point point)
			: this(texture, point.X, point.Y)
		{
		}
		
		public SpriteTextureAndPosition(SpriteTexture spriteTexture, int x, int y)
		{
			this.SpriteTexture = spriteTexture;
			this.Position = new Vector2((float)x, (float)y);
		}
		
		public SpriteTexture SpriteTexture { get; private set; }
		public Vector2 Position { get; private set; } 
	}
}

