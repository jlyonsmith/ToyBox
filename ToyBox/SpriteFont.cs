using System;

namespace ToyBox
{
	public class SpriteFont
	{
		public SpriteFont ()
		{
		}

		public Size MeasureString(string text)
		{
			throw new NotImplementedException();
		}

		public Texture CreateTexture(string text)
		{
			throw new NotImplementedException();
		}

		public int LineSpacing
		{
			get;
			set;
		}
	}
}

