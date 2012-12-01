using System;
using MonoTouch.UIKit;
using MonoTouch.iAd;
using MonoTouch.MessageUI;

namespace ToyBox
{
#if MONOTOUCH
	public partial class PlatformManager : GameComponent, IPlatformService
	{
		PlatformType platformType;
#warning Size portrait or landscape?
		Size adBannerSize;

		public PlatformManager(Game game) : base(game)
		{
			platformType = PlatformType.iPhone4;
			adBannerSize = new Size(0, 0);
		}

		public override void Initialize()
		{
			// Use for debugging new iOS screen resolutions
			//Debug.WriteLine(((int)UIScreen.MainScreen.CurrentMode.Size.Width).ToString());
			
			switch ((int)UIScreen.MainScreen.CurrentMode.Size.Width)
			{
			case 480:
			case 320:
				platformType = PlatformType.iPhone3;
				break;
			case 960:
			case 640:
				platformType = PlatformType.iPhone4;
				break;
			case 1024:
			case 768:
				platformType = PlatformType.iPad2;
				break;
			case 2048:
			case 1536:
				platformType = PlatformType.iPad3;
				break;
			default:
				platformType = PlatformType.Unknown;
				break;
			}
			
			System.Drawing.SizeF size = ADBannerView.SizeFromContentSizeIdentifier(ADBannerView.SizeIdentifierLandscape);
			
			if (platformType == PlatformType.iPhone4 || platformType == PlatformType.iPad3)
				adBannerSize = new Size((int)size.Width * 2, (int)size.Height * 2);
			else
				adBannerSize = new Size((int)size.Width, (int)size.Height);
		}

		public PlatformType PlatformType
		{
			get { return platformType; }
		}

#warning Should be part of an advertising service
		public Size AdBannerSize
		{
			get { return adBannerSize; }
		}
		
		public ToyBox.PlatformType Platform
		{
			get
			{
				return platformType;
			}
		}

		public void Exit()
		{
		}

		public void SendMail(string to, string subject, string body, Action whenDismissed)
		{
			MFMailComposeViewController mailController = new MFMailComposeViewController(); 
			
			mailController.SetToRecipients(new string[]{to});
			mailController.SetSubject(subject);
			mailController.SetMessageBody(body, false);
			
			mailController.Finished += (object obj, MFComposeResultEventArgs args) =>
			{
				args.Controller.DismissModalViewControllerAnimated(true); 
				whenDismissed();
			};
			
			UIApplication.SharedApplication.Windows[0].RootViewController.PresentModalViewController(mailController, true);
		}
	}
#endif
}

