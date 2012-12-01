using System;

namespace ToyBox
{
	public class GraphicsService : IGraphicsService
	{
		public GraphicsService (Game game)
		{
			throw new NotImplementedException();
		}

		#region IGraphicsService implementation

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

