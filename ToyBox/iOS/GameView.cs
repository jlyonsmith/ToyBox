using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.OpenGLES;
using MonoTouch.ObjCRuntime;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ToyBox
{
    public class GameView : UIView, IGraphicsService
    {
        #region Fields
        private CAEAGLLayer eaglLayer;
        private EAGLContext context;
        private CADisplayLink displayLink;
        private Size viewportSize;
        private uint colorRenderBuffer;
        private uint depthRenderBuffer;
        private uint frameBuffer;
        private int tintColorSlot;
        private int positionSlot;
        private int projectionSlot;
        private int modelViewSlot;
        private int texCoordSlot;
        private int textureSlot;
        private int targetFrameInterval = 60;
        #endregion

        #region Construction
        [Export("layerClass")]
        static Class LayerClass()
        {
            return new Class(typeof(CAEAGLLayer));
        }

        [Export("initWithFrame")]
        public GameView(RectangleF frame) : base(frame)
        {
            CreateFrameBuffer();
            CompileShaders();
            
            // Create CADisplayLink for animation loop
            
            displayLink = CADisplayLink.Create(this, new Selector("renderFrame"));
            // TODO-johnls: Set this based on target frames interval
            displayLink.FrameInterval = 1;
            displayLink.AddToRunLoop(NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
        }
        #endregion

        #region IDispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
            
            if (displayLink != null)
            {
                displayLink.Dispose();
                displayLink = null;
            }
        }
        #endregion

        #region Methods
        private void CreateFrameBuffer()
        {
            // Setup layer
            
            eaglLayer = (CAEAGLLayer)this.Layer;
            eaglLayer.Opaque = false;
            
            bool layerRetainsBacking = false;
            NSString layerColorFormat = EAGLColorFormat.RGBA8;
            
            eaglLayer.DrawableProperties = NSDictionary.FromObjectsAndKeys(new NSObject[]
                                                                           {
                NSNumber.FromBoolean(layerRetainsBacking),
                layerColorFormat
            }, new NSObject[]
            {
                EAGLDrawableProperty.RetainedBacking,
                EAGLDrawableProperty.ColorFormat
            });
            
            // Create OpenGL drawing context
            
            EAGLRenderingAPI api = EAGLRenderingAPI.OpenGLES2;
            
            context = new EAGLContext(api);
            
            if (context == null)
            {
                throw new InvalidOperationException("Failed to initialize OpenGLES 2.0 context");
            }
            
            if (!EAGLContext.SetCurrentContext(context))
            {
                throw new InvalidOperationException("Failed to set current OpenGL context");
            }
            
            // Create size for use later
            
            viewportSize = new Size((int)Math.Round((double)eaglLayer.Bounds.Size.Width), (int)Math.Round((double)eaglLayer.Bounds.Size.Height));
            
            // Create depth buffer (you have to do this BEFORE creating the color buffer)
            GL.GenRenderbuffers(1, ref depthRenderBuffer);
            GL.BindRenderbuffer(All.Renderbuffer, depthRenderBuffer);
            GL.RenderbufferStorage(All.Renderbuffer, All.DepthComponent16, viewportSize.Width, viewportSize.Height);
            
            // Create color buffer and allocate storage through the context (as a special case for this particular buffer)

            int oldColorRenderBuffer = 0;

            GL.GetInteger(All.Renderbuffer, ref oldColorRenderBuffer);
            GL.GenRenderbuffers(1, ref colorRenderBuffer);
            GL.BindRenderbuffer(All.Renderbuffer, colorRenderBuffer);

            if (!context.RenderBufferStorage((uint)All.Renderbuffer, eaglLayer))
            {
                GL.DeleteRenderbuffers(1, ref colorRenderBuffer);
                colorRenderBuffer = 0;
                GL.BindRenderbuffer(All.RenderbufferBinding, oldColorRenderBuffer);
                throw new InvalidOperationException("Unable to allocate color render buffer storage");
            }

            // Create frame buffer
            
            GL.GenFramebuffers(1, ref frameBuffer);
            GL.BindFramebuffer(All.Framebuffer, frameBuffer);
            GL.FramebufferRenderbuffer(All.Framebuffer, All.ColorAttachment0, All.Renderbuffer, colorRenderBuffer);
            GL.FramebufferRenderbuffer(All.Framebuffer, All.DepthAttachment, All.Renderbuffer, depthRenderBuffer);
            
            // Set viewport 
            GL.Viewport(0, 0, viewportSize.Width, viewportSize.Height);
        }

        private void DestroyFrameBuffer()
        {
            throw new NotImplementedException();
        }

        private int CompileShader(string shaderName, All shaderType)
        {
            string shaderProgram;

            using (StreamReader reader = new StreamReader(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("ToyBox.Shaders" + shaderName + ".glsl")))
            {
                shaderProgram = reader.ReadToEnd();
            }
            
            int shader = GL.CreateShader(shaderType);
            int length = shaderProgram.Length;
            
            GL.ShaderSource(shader, 1, new string[] { shaderProgram }, ref length);
            GL.CompileShader(shader);
            
            int compileStatus = 0;
            
            GL.GetShader(shader, All.CompileStatus, ref compileStatus);
            
            if (compileStatus == (int)All.False)
            {
                StringBuilder sb = new StringBuilder(256);
                length = 0;
                GL.GetShaderInfoLog(shader, sb.Capacity, ref length, sb);
                Console.WriteLine(sb.ToString());
                throw new InvalidOperationException();
            }
            
            return shader;
        }
        
        private void CompileShaders()
        {
            int vertexShader = CompileShader("SpriteVertex", All.VertexShader);
            int fragmentShader = CompileShader("SpriteFragment", All.FragmentShader);
            
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
            
            int linkStatus = 0;
            
            GL.GetProgram(program, All.LinkStatus, ref linkStatus);
            
            if (linkStatus == (int)All.False)
            {
                StringBuilder sb = new StringBuilder(256);
                int length = 0;
                GL.GetProgramInfoLog(program, sb.Capacity, ref length, sb);
                Console.WriteLine(sb.ToString());
                throw new InvalidOperationException();
            }
            
            GL.UseProgram(program);
            
            positionSlot = GL.GetAttribLocation(program, "Position");
            GL.EnableVertexAttribArray(positionSlot);
            
            texCoordSlot = GL.GetAttribLocation(program, "TexCoordIn");
            GL.EnableVertexAttribArray(texCoordSlot);
            
            projectionSlot = GL.GetUniformLocation(program, "Projection");
            modelViewSlot = GL.GetUniformLocation(program, "ModelView");
            textureSlot = GL.GetUniformLocation(program, "Texture");
            tintColorSlot = GL.GetUniformLocation(program, "TintColor");
        }

        [Export("renderFrame")]
        private void RenderFrame()
        {
            // TODO: This needs to call the Update methods then ...

            // Render
            GL.ClearColor((float)BackgroundColor.R/255f, (float)BackgroundColor.G/255f, (float)BackgroundColor.B/255f, (float)1f);
            GL.Clear((int)All.ColorBufferBit | (int)All.DepthBufferBit);

            // TODO: This code should be in the SpriteService...

            Matrix4 projectionMatrix;
            Matrix4.CreateOrthographic(viewportSize.Width, viewportSize.Height, Sprite.MinDepth, Sprite.MaxDepth, out projectionMatrix);
            
            GL.Enable(All.DepthTest);

            // TODO: This matrix is changed per sprite...
            Matrix4 modelViewMatrix;
            Matrix4.CreateTranslation((float)Math.Sin(CABasicAnimation.CurrentMediaTime()), 0f, -7f, out modelViewMatrix);

            GL.VertexAttribPointer(positionSlot, 3, All.Float, false, Marshal.SizeOf(typeof(TexturedVertex)), (IntPtr)0);
            GL.VertexAttribPointer(texCoordSlot, 2, All.Float, false, Marshal.SizeOf(typeof(TexturedVertex)), (IntPtr)(sizeof(float) * 3));
            
            GL.UniformMatrix4(projectionSlot, 1, false, ref projectionMatrix.Row0.X);
            GL.UniformMatrix4(modelViewSlot, 1, false, ref modelViewMatrix.Row0.X);
            // TODO-johnls: Get this from the Sprite data
            GL.Uniform4(tintColorSlot, 1f, 1f, 1f, 1f);

            context.PresentRenderBuffer((uint)All.Renderbuffer);
        }

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

        #region IGraphicsService

        public int CreateTexture2D(int width, int height, IList<SpriteTextureAndPosition> textureAndPositions)
        {
            throw new NotImplementedException();
        }

        public int CreateTexture2D(int width, int height, Color color)
        {
            throw new NotImplementedException();
        }

        public Color BackgroundColor { get; set; }

        public Size ViewportSize
        { 
            get { return viewportSize; }
            private set
            {
                throw new NotImplementedException();
            }
        }

        public int TargetFrameInterval
        { 
            get { return targetFrameInterval; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event UpdateDelegate Update;
        public event RenderDelegate Render;

        #endregion
    }
}

