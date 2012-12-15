using System;

namespace ToyBox
{
	public class SpriteBatch
	{
		public SpriteBatch ()
		{
		}

		public void Add(Texture texture, Vector2 position, Rectangle sourceRectangle, Color tintColor, float rotation, Vector2 RotationOrigin, Vector2 scale, float xnaDepth)
		{
			throw new NotImplementedException();
		}
		
		public void Add(Texture texture, Point position, Color tintColor, float rotation, Vector2 rotationOrigin, Vector2 scale, float xnaDepth)
		{
			throw new NotImplementedException();
		}
		
		public void Begin(SpriteSortMode backToFront, SpriteBlendMode alphaBlend)
		{
		}

		public void End()
		{
		}
	}
}

