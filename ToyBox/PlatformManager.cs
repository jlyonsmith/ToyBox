using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#elif MONOTOUCH
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.iAd;
#endif

namespace ToyBox
{
	public class PlatformManager : GameComponent, IPlatformService, IDisposable
	{
		#region Fields
		private PlatformType platform;
		private Size adBannerSize;
		#endregion
		
		#region Construction
		public PlatformManager(Game game) : base(game)
		{
            if (this.Game.Services != null)
            {
                this.Game.Services.AddService(typeof(IPlatformService), this);
            }
		}
		#endregion

		#region IDisposable Implementation
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Game.Services != null)
                {
                    this.Game.Services.RemoveService(typeof(IPlatformService));
                }
            }

            base.Dispose(disposing);
        }
		#endregion

        public override void Initialize()
        {
            base.Initialize();

#if MONOTOUCH
			// Use for debugging new iOS screen resolutions
			// Debug.WriteLine(((int)UIScreen.MainScreen.CurrentMode.Size.Width).ToString());
			
			switch ((int)UIScreen.MainScreen.CurrentMode.Size.Width)
			{
			case 480:
			case 320:
				platform = PlatformType.iPhone3;
				break;
			case 960:
			case 640:
				platform = PlatformType.iPhone4;
				break;
			case 1024:
			case 768:
				platform = PlatformType.iPad2;
				break;
			case 2048:
			case 1536:
				platform = PlatformType.iPad3;
				break;
			default:
				platform = PlatformType.Unknown;
				break;
			}
			
			System.Drawing.SizeF size = ADBannerView.SizeFromContentSizeIdentifier(ADBannerView.SizeIdentifierLandscape);
			
			if (platform == PlatformType.iPhone4 || platform == PlatformType.iPad3)
				adBannerSize = new Size((int)size.Width * 2, (int)size.Height * 2);
			else
				adBannerSize = new Size((int)size.Width, (int)size.Height);
			
#elif WINDOWS
			platform = PlatformType.Windows;
            adBannerSize = new Size();
#elif WINDOWS_PHONE
			platform = PlatformType.WinPhone7;
            adBannerSize = new Size();
#endif
        }


		#region IPlatformService Implementation
		public PlatformType Platform { get { return this.platform; } }
		
		public Size AdBannerSize { get { return this.adBannerSize; } }
		#endregion
	}
}

