using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /// <summary>
    /// @benbaird justification uses this enum in X-flixel, rather than a string
    /// 
    /// Left = 0,
    /// Right = 1,
    /// Center = 2
    /// </summary>
    public enum FlxJustification
    {
        Left = 0,
        Right = 1,
        Center = 2
    }

    /// <summary>
    /// Extends <code>FlxSprite</code> to support rendering text.
    /// Can tint, fade, rotate and scale just like a sprite.
    /// Doesn't really animate though, as far as I know.
    /// Also does nice pixel-perfect centering on pixel fonts
    /// as long as they are only one liners.
    /// 
    /// FlxText's internal implementation in X-flixel hasn't
    /// changed drastically from its v1.25 debut. The primary
    /// modifications are to its public interface in order
    /// to align it with AS3 flixel.
    /// </summary>
    public class FlxText : FlxSprite
    {

        private string _text;
        private SpriteFont _font;
        private Vector2 _fontmeasure = Vector2.Zero;
        private float _scale = 1f;
        //private float _angle = 0f;
        //private float _radians = 0f;

        public int textWidth
        {
            get { return (int)_fontmeasure.X; }
        }
        public int textHeight
        {
            get { return (int)_fontmeasure.Y; }
        }


        public override Vector2 origin
        {
            get
            {
                return _origin * _scale;
            }
            set
            {
                _origin = value / _scale;
            }
        }

        //public float angle
        //{
        //    get { return _angle; }
        //    set { _angle = value; _radians = MathHelper.ToRadians(_angle); }
        //}

        /// <summary>
        /// @benbaird X-flixel only
        /// </summary>
        public void autoSize()
        {
            width = textWidth;
            height = textHeight;
        }


        /// <summary>
        /// Creates a new <code>FlxText</code> object at the specified position.
        /// </summary>
        /// <param name="X">The X position of the text.</param>
        /// <param name="Y">The Y position of the text.</param>
        /// <param name="Width">The width of the text object (height is determined automatically).</param>
        public FlxText(float X, float Y, float Width)
            : base(X, Y)
        {
            constructor(X, Y, Width, 10, "", Color.White, FlxG.Font, 1, FlxJustification.Center, 0);
        }
        /// <summary>
        /// Creates a new <code>FlxText</code> object at the specified position.
        /// </summary>
        /// <param name="X">The X position of the text.</param>
        /// <param name="Y">The Y position of the text.</param>
        /// <param name="Width">The width of the text object (height is determined automatically).</param>
        /// <param name="Text">The actual text you would like to display initially.</param>
        public FlxText(float X, float Y, float Width, string Text)
            : base(X, Y)
		{
			if(Text == null)
				Text = "";

            constructor(X, Y, Width, 10, Text, Color.White, FlxG.Font, 1, FlxJustification.Center, 0);
		}

        /// <summary>
        /// Creates a new <code>FlxText</code> object at the specified position.
        /// </summary>
        /// <param name="X">The X position of the text.</param>
        /// <param name="Y">The Y position of the text.</param>
        /// <param name="Width">The width of the text object (height is determined automatically).</param>
        /// <param name="Height"></param>
        /// <param name="sText"></param>
        /// <param name="cColor"></param>
        /// <param name="fFont"></param>
        /// <param name="fScale"></param>
        /// <param name="fJustification"></param>
        /// <param name="fAngle"></param>
        public void constructor(float X, float Y, float Width, float Height, string sText, Color cColor, SpriteFont fFont, float fScale, FlxJustification fJustification, float fAngle)
        {
            _text = sText;
            color = cColor;
            shadow = Color.Black;

            backColor = new Color(0xFF, 0xFF, 0xFF, 0x0);
            if (fFont == null)
                fFont = FlxG.Font;
            _font = fFont;
            angle = fAngle;
            _scale = fScale;

            alignment = fJustification;

            x = X;
            y = Y;
            width = Width;
            height = Height;

            scrollFactor = Vector2.Zero;

            solid = false;
            moves = false;
            recalcMeasurements();
        }

        /// <summary>
        /// You can use this if you have a lot of text parameters
        /// to set instead of the individual properties.
        /// </summary>
        /// <param name="Font">The name of the font face for the text display. -- FlxG.Content.Load|SpriteFont|("initials/Munro") -- </param>
        /// <param name="Scale">The scale of the font (in AS3 flixel, this is Size)</param>
        /// <param name="Color">The color of the text in traditional flash 0xRRGGBB format.</param>
        /// <param name="Alignment">A string representing the desired alignment ("left,"right" or "center").</param>
        /// <param name="ShadowColor">A uint representing the desired text shadow color in flash 0xRRGGBB format.</param>
        /// <returns>This FlxText instance (nice for chaining stuff together, if you're into that).</returns>
		public FlxText setFormat(SpriteFont Font, float Scale, Color Color, FlxJustification Alignment, Color ShadowColor)
		{
			if(Font == null)
				Font = FlxG.Font;
			_font = Font;
			_scale = Scale;
			color = Color;
			alignment = Alignment;
			shadow = ShadowColor;
            recalcMeasurements();
			return this;
		}

        /// <summary>
        /// The text being displayed.
        /// </summary>
        public string text
        {
            get { return _text; }
            set { _text = value; recalcMeasurements(); }
        }

        /// <summary>
        /// The size of the text being displayed.
        /// </summary>
        public new float scale
        {
            get { return _scale; }
            set
            {
                // Preserve origin proportions
                _origin *= _scale;
                _scale = value;
                // Restore origin proportions
                _origin /= _scale;
                recalcMeasurements();
            }
        }

        /// <summary>
        /// The font used for this text.
        /// </summary>
        public SpriteFont font
        {
            get { return _font; }
            set { _font = value; if (_font == null) _font = FlxG.Font; recalcMeasurements(); }
        }

        /// <summary>
        /// The alignment of the font ("left", "right", or "center").
        /// </summary>
        public FlxJustification alignment = FlxJustification.Left;

        /// <summary>
        /// The color of the text's shadow.
        /// </summary>
        public Color shadow;

        /// <summary>
        /// The color of the background behind the text (X-flixel only).
        /// </summary>
        public Color backColor;

        /// <summary>
        /// @benbaird X-flixel only. Used to ensure the textWidth and textHeight properties
        /// are always up to date.
        /// </summary>
        private void recalcMeasurements()
        {
            try
            {
                _fontmeasure = _font.MeasureString(_text) * _scale;
                origin = new Vector2(_fontmeasure.X / 2, _fontmeasure.Y / 2);
            }
            catch
            {
                _fontmeasure = Vector2.Zero;
            }
        }

        /// <summary>
        /// Called by the game loop automatically, blits the text object to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
		
        public override void render(SpriteBatch spriteBatch)
        {
            if (visible == false || exists == false)
            {
                return;
            }

            Vector2 pos = new Vector2(x, y) + origin;
            pos += (FlxG.scroll * scrollFactor);

            if (backColor.A > 0)
            {
                //Has a background color
                spriteBatch.Draw(FlxG.XnaSheet, new Rectangle((int)x, (int)y, (int)width, (int)height),
                    new Rectangle(1, 1, 1, 1), backColor);
            }

            if (shadow != color)
            {
                pos += new Vector2(1, 1);
                if (alignment == FlxJustification.Left)
                {
                    spriteBatch.DrawString(_font, _text,
                        pos, shadow,
                        _radians, _origin, _scale, SpriteEffects.None, 0f);
                }
                else if (alignment == FlxJustification.Right)
                {
                    spriteBatch.DrawString(_font, _text,
                        new Vector2(pos.X + width - textWidth, pos.Y), shadow,
                        _radians, _origin, _scale, SpriteEffects.None, 0f);
                }
                else if (alignment == FlxJustification.Center)
                {
                    spriteBatch.DrawString(_font, _text,
                        new Vector2(pos.X + ((width - textWidth) / 2), pos.Y), shadow,
                        _radians, _origin, _scale, SpriteEffects.None, 0f);
                }
                pos += new Vector2(-1, -1);
            }

            if (alignment == FlxJustification.Left)
            {
                spriteBatch.DrawString(_font, _text,
                    pos, color,
                    _radians, _origin, _scale, SpriteEffects.None, 0f);
            }
            else if (alignment == FlxJustification.Right)
            {
                spriteBatch.DrawString(_font, _text,
                    new Vector2(pos.X + width - textWidth, pos.Y), color,
                    _radians, _origin, _scale, SpriteEffects.None, 0f);
            }
            else if (alignment == FlxJustification.Center)
            {
                spriteBatch.DrawString(_font, _text,
                    new Vector2(pos.X + ((width - textWidth) / 2), pos.Y), color,
                    _radians, _origin, _scale, SpriteEffects.None, 0f);
            }
        }
    }
}
