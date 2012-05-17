using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
using System.Collections.ObjectModel;

namespace ToyBox
{
    public abstract class Sprite
    {
        public static readonly int MaxDepth = UInt16.MaxValue;

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

        public abstract void Draw(SpriteBatch spriteBatch);
    }

    public class TextureSprite : Sprite
    {
        private int activeTextureIndex;
        private Rectangle rect;

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
                rect = this.SpriteTextures[this.ActiveTextureIndex].Rectangle;
            }
        }
        public override Size Size
        {
            get
            {
                return new Size(rect.Width, rect.Height);
            }
        }
        public override int Width
        {
            get { return rect.Width; }
        }
        public override int Height
        {
            get { return rect.Height; }
        }
        public override Rectangle Rectangle
        {
            get
            {
                Rectangle textureRect = this.SpriteTextures[this.ActiveTextureIndex].Rectangle;

                return new Rectangle(this.Position.X, this.Position.Y, textureRect.Width, textureRect.Height);
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteTexture texture = this.SpriteTextures[ActiveTextureIndex];

            if (this.Rotation == 0.0f)
            {
                spriteBatch.Draw(
                    texture.Texture,
                    new Vector2(Position.X, Position.Y),
                    texture.Rectangle,
                    TintColor,
                    0.0f,
                    Vector2.Zero,
                    this.Scale,
                    SpriteEffects.None,
                    xnaDepth);
            }
            else
            {
                Size size = this.Size;
                Vector2 origin = new Vector2(RotationOrigin.X * size.Width, RotationOrigin.Y * size.Height);

                spriteBatch.Draw(
                    texture.Texture,
                    new Vector2(Position.X + origin.X, Position.Y + origin.Y),
                    texture.Rectangle,
                    TintColor,
                    this.Rotation,
                    origin,
                    this.Scale,
                    SpriteEffects.None,
                    xnaDepth);
            }
        }
    }

    public class StringSprite : Sprite
    {
        private Size size;
        private string text;
        
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
                size = new Size(Font.MeasureString(text));
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Rotation == 0.0f)
            {
                spriteBatch.DrawString(
                    this.Font,
                    this.Text,
                    new Vector2(position.X, position.Y),
                    this.TintColor,
                    this.Rotation,
                    Vector2.Zero,
                    this.Scale,
                    SpriteEffects.None,
                    xnaDepth);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
