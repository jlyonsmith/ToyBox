using System;
using System.Collections.Generic;

namespace ToyBox
{
	public interface IGraphicsService
	{
		Color BackgroundColor
		{
			get;
			set;
		}
        int CreateTexture2D(int width, int height, IList<SpriteTextureAndPosition> textureAndPositions);
        int CreateTexture2D(int width, int height, Color color);
        void Render();
	}
}

