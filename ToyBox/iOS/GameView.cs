using System;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.CoreAnimation;
using MonoTouch.OpenGLES;
using MonoTouch.UIKit;
using OpenTK;
using OpenTK.Platform.iPhoneOS;
using GL11 = OpenTK.Graphics.ES11.GL;
using GL20 = OpenTK.Graphics.ES20.GL;
using ALL11 = OpenTK.Graphics.ES11.All;
using ALL20 = OpenTK.Graphics.ES20.All;
using System.Drawing;
using MonoTouch.iAd;
using MonoTouch.MessageUI;
using System.Collections.Generic;

namespace ToyBox
{
    public class GameView : UIView
    {
        #region Fields
        private CAEAGLLayer eaglLayer;
        private EAGLContext context;
        private CADisplayLink displayLink;
        private Size size;
        #endregion

        #region Events
        public event UpdateDelegate Update;
        public event RenderDelegate Render;
        #endregion

        #region Construction
        [Export("layerClass")]
        static Class LayerClass()
        {
            return iPhoneOSGameView.GetLayerClass();
        }

        public GameView(RectangleF frame) : base(frame)
        {
        }
        #endregion

        #region Methods
        private void RaiseRenderEvent()
        {
            RenderDelegate handler = this.Render;
            
            if (handler != null)
                handler();
        }

        private void RaiseUpdateEvent(double elapsed)
        {
            UpdateDelegate handler = this.Update;
            
            if (handler != null)
                // TODO-johnls: Fill in the GameTime values correctly
                handler(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(elapsed), false));
        }
        #endregion
    }
}

