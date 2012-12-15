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

        Texture CreateTexture2D(int width, int height, IList<SpriteTextureAndPosition> textureAndPositions);
        void DrawRenderTarget(Texture texture, IList<SpriteTextureAndPosition> textureAndPositions);
        void Present();
	}
}

