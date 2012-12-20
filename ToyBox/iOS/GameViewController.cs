using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.iAd;
using System.Collections.Generic;

namespace ToyBox
{
    public class GameViewController : UIViewController, IPlatformService, IServiceProvider
    {
        #region Fields
        public DisplayOrientation SupportedOrientations { get; set; }

        public DisplayOrientation DefaultSupportedOrientations { get; private set; }

        public event EventHandler<EventArgs> DisplayOrientationChanged;

        private PlatformType platformType;
        private Size adBannerSize;
        private Dictionary<Type, object> services;
        private GameView gameView;
        private int targetFramesPerSecond = 60;
        #endregion

        public int TargetFramesPerSecond
        {
            get { return targetFramesPerSecond; }
            set
            {
                throw new NotImplementedException();
            }
        }

        #region Construction
        public GameViewController()
        {
            SetDefaultSupportedOrientations();
            SupportedOrientations = DefaultSupportedOrientations;

            platformType = PlatformType.iPhone4;
            adBannerSize = new Size(0, 0);

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

            this.services = new Dictionary<Type, object>();
            
            // Non-platform specific services
            AddService<IPlatformService>(this);
            AddService<IGraphicsService>(new GraphicsService(this));
            AddService<IGameScreenService>(new GameScreenService(this));
            AddService<ISpriteService>(new SpriteService(this));
            AddService<IInputService>(new InputService(this));
            AddService<IStorageService>(new StorageService(this));
            AddService<IPropertyListService>(new PropertyListService(this));
            
            // Platform specific services
            AddService<IMousePointerService>(new MousePointerService(this));
        }
        #endregion

        #region Overrides
        public override void LoadView()
        {
            RectangleF frame;

            if (ParentViewController != null && ParentViewController.View != null)
            {
                frame = new RectangleF(PointF.Empty, ParentViewController.View.Frame.Size);
            }
            else
            {
                UIScreen screen = UIScreen.MainScreen;

                if (InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft ||
                    InterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
                {
                    frame = new RectangleF(0, 0, screen.Bounds.Height, screen.Bounds.Width);
                }
                else
                {
                    frame = new RectangleF(0, 0, screen.Bounds.Width, screen.Bounds.Height);
                }
            }
            
            gameView = new GameView(frame);

            this.Add(gameView);
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return (OrientationConverter.ToDisplayOrientation(toInterfaceOrientation) & SupportedOrientations) != 0;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
        }

        public override void ViewWillUnload()
        {
#warning Dispose of all the Services correctly
        }
        #endregion

        #region IServiceProvider
        public object GetService(Type serviceType)
        {
            return services [serviceType];
        }
        
        #endregion
        
        #region IPlatformService
        
        public Size AdBannerSize
        {
            get { return adBannerSize; }
        }
        
        public PlatformType Platform
        {
            get
            {
                return platformType;
            }
        }
        
        public void Exit()
        {
            throw new NotImplementedException();
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
            
            this.PresentModalViewController(mailController, true);
        }
        
        public void AddService<T>(object service)
        {
            if (services.ContainsKey(typeof(T).GetType()))
                throw new ArgumentException("Service type already exists");
            
            services.Add(typeof(T).GetType(), service);
        }
        
        public event UpdateDelegate Update
        {
            add
            {
                if (gameView != null)
                    gameView.Update += value;
            }
            remove
            {
                if (gameView != null)
                    gameView.Update -= value;
            }
        }

        public event RenderDelegate Draw
        {
            add
            {
                if (gameView != null)
                    gameView.Render += value;
            }
            remove
            {
                if (gameView != null)
                    gameView.Render -= value;
            }
        }
        
        #endregion

        #region Methods
        private void SetDefaultSupportedOrientations()
        {
            DefaultSupportedOrientations = DisplayOrientation.Portrait;

            var key = new NSString("UISupportedInterfaceOrientations");
            NSObject arrayObj;

            if (!NSBundle.MainBundle.InfoDictionary.TryGetValue(key, out arrayObj))
                return;

            DisplayOrientation orientations = DisplayOrientation.Unknown;
            var supportedOrientationStrings = NSArray.ArrayFromHandle<NSString>(arrayObj.Handle);
            
            foreach (var orientationString in supportedOrientationStrings)
            {
                var s = (string)orientationString;

                if (!s.StartsWith("UIInterfaceOrientation"))
                    continue;

                s = s.Substring("UIInterfaceOrientation".Length);
                
                try
                {
                    var supportedOrientation = (UIInterfaceOrientation)Enum.Parse(
                        typeof(UIInterfaceOrientation), s);

                    orientations |= OrientationConverter.ToDisplayOrientation(supportedOrientation);
                }
                catch
                {
                }
            }
            
            if (orientations == DisplayOrientation.Unknown)
                return;
            
            DefaultSupportedOrientations = orientations;
        }

        #endregion
    }
}
