using System;
using MonoTouch.UIKit;

namespace ToyBox
{
    public class GameApplicationDelegate : UIApplicationDelegate
    {
        protected UIWindow Window { get; set; }
        protected GameViewController GameViewController { get; set; }

        public EventHandler<EventArgs> Activated;
        public EventHandler<EventArgs> Deactivated;

        public override void OnActivated(UIApplication application)
        {
            RaiseActivated();
        }
        
        public override void OnResignActivation(UIApplication application)
        {
            RaiseDeactivated();
        }
     
        public override void FinishedLaunching(UIApplication application)
        {
            this.Window = new UIWindow(UIScreen.MainScreen.ApplicationFrame);
            this.Window.RootViewController = this.GameViewController;
            this.Window.MakeKeyAndVisible();
        }

        private void RaiseActivated()
        {
            EventHandler<EventArgs> handler = Activated;
            
            if (handler != null)
                handler(this, new EventArgs());
        }

        private void RaiseDeactivated()
        {
            EventHandler<EventArgs> handler = Deactivated;
            
            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}

