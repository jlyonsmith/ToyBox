using System;
using System.IO;

namespace ToyBox
{
	public class Texture : IDisposable
	{
        #region Fields
        private IGraphicsService graphicsService;
        private int textureName;
        #endregion

        #region Properties
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ToyBox.Size Size
        {
            get { return new Size(Width, Height); }
        }
        #endregion

        #region Construction
        public Texture(IGraphicsService graphicsService, int width, int height, Color color)
        {
            this.graphicsService = graphicsService;
        }

        public Texture(IGraphicsService graphicsService, int width, int height, byte[] data)
        {
            this.graphicsService = graphicsService;
        }
        #endregion

        #region IDisposable

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion

#warning Put this into the ContentService
		public static Texture FromPdfFile(string pdfFileName, int width, int height)
		{
#if DONT_COMPILE
            string pngFileName = 
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
                    "/.imagecache/" + assetName.Substring(assetName.LastIndexOf('/') + 1) + ".png";
            Texture texture = null;
            
            if (File.Exists(pngFileName))
            {
                texture = Texture.FromPngFile(pngFileName);
            }
            else
            {
                string assetFileName = base.GetAssetFileName(assetName, typeof(List<String>));
                List<String> pdfInfo = base.ReadAsset<List<String>>(assetName);
                string pinboardName = pdfInfo [0];
                string rectangleName = pdfInfo [1];
                int numRows = Int32.Parse(pdfInfo [2]);
                int numCols = Int32.Parse(pdfInfo [3]);
                
                PropertyInfo propInfo = typeof(Rectangles).GetProperty(pinboardName, BindingFlags.Public | BindingFlags.Static);
                object obj = propInfo.GetValue(null, null);
                
                propInfo = obj.GetType().GetProperty(rectangleName, BindingFlags.Public | BindingFlags.Instance);
                obj = propInfo.GetValue(obj, null);
                
                Rectangle rect = (Rectangle)obj;
                
                string pdfFileName = Path.ChangeExtension(assetFileName, ".pdf");
                int width = rect.Width * numCols;
                int height = rect.Height * numRows;
                
                texture = Texture.FromFile(
                    this.graphicsDeviceService.GraphicsDevice, 
                    pdfFileName, width, height);
                
                string pngDirectory = Path.GetDirectoryName(pngFileName);
                
                if (!Directory.Exists(pngDirectory))
                {
                    Directory.CreateDirectory(pngDirectory);
                }
                
                texture.SaveAsPng(pngFileName, width, height);
            }
#endif
            
            return null;
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

