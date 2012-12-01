using System;

namespace ToyBox
{
	public interface IGraphicsService
	{
		Color BackgroundColor
		{
			get;
			set;
		}

		void Present();
	}
}

