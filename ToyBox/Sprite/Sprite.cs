using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace ToyBox
{
    public abstract class Sprite
    {
        public static readonly int MaxDepth = UInt16.MaxValue;

#warning Remove xnaDepth

        protected float xnaDepth;
        protected int depth;
        protected Point position;

        public Point Position 
        { 
            get { return position; }
            set { this.position = value; }
        }
        public int X 
        {
            get { return this.Position.X; }
            set { this.position.X = value; }
        }
        public int Y 
        { 
            get { return this.Position.Y; } 
            set { this.position.Y = value; }
        }
        public float Rotation { get; set; }
        public Vector2 RotationOrigin { get; set; }
        public abstract Rectangle Rectangle { get; }
        public abstract Size Size { get; }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public bool Visible { get; set; }
        public Color TintColor { get; set; }
        public int Depth 
        {
            get
            {
                return depth;
            }
            set
            {
                depth = Math.Max(0, Math.Min(value, MaxDepth));
                xnaDepth = (float)depth / (float)MaxDepth;
            }
        }
        public object GameObject { get; set; }
        public bool HitTestable { get { return this.GameObject != null; } }
        public Vector2 Scale { get; set; }
        public Animation ActiveAnimation { get; set; }

        public Sprite(Point position, int depth, bool visible, object gameObject)
        {
            this.Position = position;
            this.Visible = visible;
            this.Depth = depth;
            this.Scale = Vector2.One;
            this.ActiveAnimation = null;
            this.GameObject = gameObject;
            this.TintColor = Color.White;
        }

        public abstract void Draw();
    }

    public class TextureSprite : Sprite
    {
        private int activeTextureIndex;
        
		public SpriteTexture ActiveSpriteTexture 
		{
			get
			{
				return this.SpriteTextures[this.ActiveTextureIndex];
			}
		}

        public ReadOnlyCollection<SpriteTexture> SpriteTextures { get; private set; }
        public int ActiveTextureIndex 
        {
            get
            {
                return activeTextureIndex;
            }
            set
            {
                activeTextureIndex = value;
            }
        }
        public override Size Size
        {
            get
            {
                return ActiveSpriteTexture.DestinationSize;
            }
        }
        public override int Width
        {
			get { return ActiveSpriteTexture.DestinationSize.Width; }
        }
        public override int Height
        {
            get { return ActiveSpriteTexture.DestinationSize.Height; }
        }
        public override Rectangle Rectangle
        {
            get
            {
				// TODO: Add a constructor to Rectangle that takes a Point and Size
                return new Rectangle(this.Position.X, this.Position.Y, this.Width, this.Height);
            }
        }

        public TextureSprite(
            SpriteTexture spriteTexture,
            Point position,
            int depth,
            bool visible,
            object gameObject)
            : this(new SpriteTexture[1] { spriteTexture }, 0, position, depth, visible, gameObject)
        {
        }

        public TextureSprite(
            SpriteTexture[] spriteTextures, 
            int activeTextureIndex, 
            Point position, 
            int depth, 
            bool visible, 
            object gameObject)
            : base(position, depth, visible, gameObject)
        {
            this.SpriteTextures = Array.AsReadOnly<SpriteTexture>(spriteTextures);
            this.ActiveTextureIndex = activeTextureIndex;
        }

        public override void Draw()
        {
            SpriteTexture spriteTexture = this.ActiveSpriteTexture;

            if (this.Rotation == 0.0f)
            {
                /*
                spriteBatch.Add(
                    spriteTexture.Texture,
                    new Vector2(Position.X, Position.Y),
                    spriteTexture.SourceRectangle,
                    TintColor,
                    0.0f,
                    Vector2.Zero,
                    spriteTexture.Scale * this.Scale,
                    xnaDepth);
                */
            }
            else
            {
                Size size = this.Size;
                Vector2 origin = new Vector2(RotationOrigin.X * size.Width, RotationOrigin.Y * size.Height);

                /*
                spriteBatch.Add(
                    spriteTexture.Texture,
                    new Vector2(Position.X + origin.X, Position.Y + origin.Y),
                    spriteTexture.SourceRectangle,
                    TintColor,
                    this.Rotation,
                    origin,
                    spriteTexture.Scale * this.Scale,
                    xnaDepth);
                */
            }
        }
    }

    public class StringSprite : Sprite
    {
        private Size size;
        private string text;
		private Texture texture;
        public SpriteFont Font { get; private set; }
        public String Text 
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                texture = Font.CreateTexture(text);
				size = texture.Size;
            }
        }
        public override Size Size
        {
            get
            {
                return size;
            }
        }
        public override int Width
        {
            get { return size.Width; }
        }
        public override int Height
        {
            get { return size.Height; }
        }
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle(X, Y, size.Width, size.Height);
            }
        }

        public StringSprite(SpriteFont font, string text, Point position, int depth, bool visible, object gameObject)
            : base(position, depth, visible, gameObject)
        {
            this.Font = font;
            this.Text = text;  // Use the property, not the field for Size to work
        }

        public StringSprite(SpriteFont font, string text, Rectangle rect, TextAlignment alignment, int depth, bool visible, object gameObject)
            : base(Point.Zero, depth, visible, gameObject)
        {
            this.Font = font;
            this.Text = text;  // Use the property, not the field for Size to work

            AlignText(rect, alignment);
        }

        public void AlignText(Rectangle rect, TextAlignment alignment)
        {
            switch (alignment)
            {
                default:
                case TextAlignment.Left:
                    this.Position = rect.Location;
                    break;

                case TextAlignment.Center:
                    this.Position = new Rectangle().Resize(this.Font.MeasureString(text)).CenteredOn(rect).Location;
                    break;
            }
        }

        public override void Draw()
        {
            if (this.Rotation == 0.0f)
            {
                /*
                spriteBatch.Add(
                    texture,
                    position,
                    this.TintColor,
                    this.Rotation,
                    Vector2.Zero,
                    this.Scale,
                    xnaDepth);
                */
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

	public class BlankSprite : Sprite
	{
		#region Fields
		private Size size;
		#endregion

		#region Construction
		public BlankSprite (
			Size size,
	        Point position,
	        int depth,
	        bool visible,
	        object gameObject)
			: base(position, depth, visible, gameObject)
		{
			this.size = size;
		}
		
		#endregion

		#region Sprite Implementation
		public override void Draw()
		{
			// Do nothing
		}

		public override Rectangle Rectangle
		{
			get
			{
				// TODO: Add a constructor to Rectangle that takes a Point and Size
				return new Rectangle(this.Position.X, this.Position.Y, this.Width, this.Height);
			}
		}

		public override Size Size
		{
			get
			{
				return this.size;
			}
		}

		public override int Width
		{
			get
			{
				return this.size.Width;
			}
		}

		public override int Height
		{
			get
			{
				return this.size.Height;
			}
		}
		#endregion
	}
}
