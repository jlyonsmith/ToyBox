using System;

namespace ToyBox
{
	public class GraphicsService : IGraphicsService
	{
		public GraphicsService (IServiceProvider services)
		{
		}

		#region IGraphicsService

        public Texture CreateTexture2D(int width, int height, System.Collections.Generic.IList<SpriteTextureAndPosition> textureAndPositions)
        {
            throw new NotImplementedException();
        }

        public void DrawRenderTarget(Texture texture, System.Collections.Generic.IList<SpriteTextureAndPosition> textureAndPositions)
        {
            throw new NotImplementedException();
        }

		public Color BackgroundColor
		{
			get;
			set;
		}

		public void Present()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

