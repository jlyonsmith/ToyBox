using System;

namespace ToyBox
{
	public class Texture : IDisposable
	{
		public int Width;
		public int Height;

		public Texture ()
		{
		}

		public Texture (int width, int height, bool b, SurfaceFormat color, DepthFormat none)
		{
			throw new NotImplementedException();
		}

		#region IDisposable implementation

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion

		public ToyBox.Size Size
		{
			get;
			set;
		}

		public static Texture FromPdfFile(string pdfFileName, int width, int height)
		{
			throw new NotImplementedException();
		}

        public static Texture FromPngFile(string pngFileName)
        {
            throw new NotImplementedException();
        }

		public void SaveAsPng(string pngFileName, int width, int height)
		{
			throw new NotImplementedException();
		}
	}
}

