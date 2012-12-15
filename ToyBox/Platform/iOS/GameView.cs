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
    public class GameView : iPhoneOSGameView
    {
        #region Fields
        public event DrawDelegate Draw;
        #endregion

        #region Construction
        [Export("layerClass")]
        static Class LayerClass()
        {
            return iPhoneOSGameView.GetLayerClass();
        }

        public GameView(RectangleF frame) : base(frame)
        {
            LayerRetainsBacking = false;
            LayerColorFormat = EAGLColorFormat.RGBA8;
            ContextRenderingApi = EAGLRenderingAPI.OpenGLES2;
        }
        #endregion

        #region Overrides
        protected override void ConfigureLayer(CAEAGLLayer eaglLayer)
        {
            eaglLayer.Opaque = true;
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            RaiseDrawEvent();

            float[] squareVertices = {
                -0.5f, -0.5f,
                0.5f, -0.5f,
                -0.5f, 0.5f, 
                0.5f, 0.5f,
            };
            
            byte[] squareColors = {
                255, 255,   0, 255,
                0,   255, 255, 255,
                0,     0,    0,  0,
                255,   0,  255, 255,
            };
            
            MakeCurrent();

            GL11.Viewport(0, 0, Size.Width, Size.Height);
            
            GL11.MatrixMode(ALL11.Projection);
            GL11.LoadIdentity();
            GL11.Ortho(-1.0f, 1.0f, -1.5f, 1.5f, -1.0f, 1.0f);
            GL11.MatrixMode(ALL11.Modelview);
            GL11.Rotate(3.0f, 0.0f, 0.0f, 1.0f);
            
            GL11.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL11.Clear((uint)ALL11.ColorBufferBit);
            
            GL11.VertexPointer(2, ALL11.Float, 0, squareVertices);
            GL11.EnableClientState(ALL11.VertexArray);
            GL11.ColorPointer(4, ALL11.UnsignedByte, 0, squareColors);
            GL11.EnableClientState(ALL11.ColorArray);
            
            GL11.DrawArrays(ALL11.TriangleStrip, 0, 4);
            
            SwapBuffers();
        }
        #endregion

        #region Methods
        private void RaiseDrawEvent()
        {
            DrawDelegate handler = this.Draw;

            if (handler != null)
                handler();
        }
        #endregion
    }
}

