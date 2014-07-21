using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /// <summary>
    /// X-flixel only; Replaces the "facing" boolean member
    /// </summary>
    public enum Flx2DFacing
    {
        NotUsed = -1,
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }

    /// <summary>
    /// The main "game object" class, handles basic physics and animation.
    /// </summary>
    public class FlxSprite : FlxObject
    {
        //@benbaird X-flixel only stuff
        protected Texture2D _tex;
        private bool _stretchToFit = false;

        /// <summary>
        /// If you changed the size of your sprite object to shrink the bounding box,
        /// you might need to offset the new bounding box from the top-left corner of the sprite.
        /// </summary>
        public Point offset = Point.Zero;

        /// <summary>
        /// Change the size of your sprite's graphic.
        /// NOTE: Scale doesn't currently affect collisions automatically,
        /// you will need to adjust the width, height and offset manually.
        /// </summary>
        public float scale = 1f;

        /// <summary>
        /// Blending modes, just like Photoshop!
        /// E.g. "multiply", "screen", etc.
        /// @default null
        /// Currently not working.!!!
        /// </summary>
		public string blend;

        /// <summary>
        /// Controls whether the object is smoothed when rotated, affects performance.
        /// @default false
        /// </summary>
		public bool antialiasing;

        /// <summary>
        /// Whether the current animation has finished its first (or only) loop.
        /// </summary>
        public bool finished;

        /// <summary>
        /// The width of the actual graphic or image being displayed (not necessarily the game object/bounding box).
        /// NOTE: Edit at your own risk!!  This is intended to be read-only.
        /// </summary>
		public int frameWidth;

        /// <summary>
        /// The height of the actual graphic or image being displayed (not necessarily the game object/bounding box).
        /// NOTE: Edit at your own risk!!  This is intended to be read-only.
        /// </summary>
		public int frameHeight;
		/// <summary>
        /// The total number of frames in this image (assumes each row is full).
		/// </summary>
		public int frames;
        
        //Animation helpers
        private List<FlxAnim> _animations;
		private uint _flipped;

        /// <summary>
        /// Internal helper used for retro-style flickering. colorFlicker uses color and alpha, rather than on/off.
        /// </summary>
        protected bool _colorFlicker;
        /// <summary>
        /// Internal helper used for retro-style flickering. colorFlicker uses color and alpha, rather than on/off.
        /// </summary>
        protected float _colorFlickerTimer;

        protected float _colorFlickerRMin = 0.0f;
        protected float _colorFlickerGMin = 0.0f;
        protected float _colorFlickerBMin = 0.0f;

        protected float _colorFlickerRMax = 1.0f;
        protected float _colorFlickerGMax = 1.0f;
        protected float _colorFlickerBMax = 1.0f;


        /// <summary>
        /// _curAnim is the current playing animation.
        /// <para>Use _curAnim.name to get the name of the currently playing.</para>
        /// </summary>
		protected FlxAnim _curAnim;

        /// <summary>
        /// _curFrame is the sequential frame.
        /// Eg. Regardless of what frame order you are using, _curFrame will always return 0,1,2,3,4,5,6,7...
        /// <para>Known as Frame in the animation callback.</para>
        /// </summary>
		protected int _curFrame;
        
        /// <summary>
        /// _caf returns the frame number on the spritesheet.
        /// <para>Also known as Frame Index.</para>
        /// </summary>
        protected int _caf;

        private float _frameTimer;
		
        /// <summary>
        /// The function to call if you set an Animation Callback.
        /// </summary>
        private FlxAnimationCallback _callback;
        private Flx2DFacing _facing2d = Flx2DFacing.NotUsed;

        //Various rendering helpers
		protected Rectangle _flashRect;
		protected Rectangle _flashRect2;
        protected Point _flashPointZero;
        protected float _alpha;
        /// <summary>
        /// internal color used for tinting.
        /// </summary>
        protected Color _color = Color.White;
        protected Color _lastColor = Color.White;
        private byte _bytealpha = 0xff;

        /// <summary>
        /// Used for color tinting.
        /// </summary>
        public Color color
        {
            get { return _color; }
            set { _color = new Color(value.R, value.G, value.B, _bytealpha); }
        }

        /// <summary>
        /// Alpha of the sprite.
        /// </summary>
        public float alpha
        {
            get { return _alpha; }
            set { _alpha = value; _bytealpha = (byte)(255f * _alpha); _color = new Color(_color.R, _color.G, _color.B, _bytealpha); }
        }

        /// <summary>
        /// Returns a list of FlxAnims.
        /// </summary>
        public List<FlxAnim> animations
        {
            get { return _animations; }
        }

        /// <summary>
        /// Creates a white 8x8 square <code>FlxSprite</code> at the specified position.
        /// Optionally can load a simple, one-frame graphic instead.
        /// </summary>
        public FlxSprite()
        {
            // call our constructor
            constructor(0, 0, null);
        }
        /// <summary>
        /// Creates a white 8x8 square <code>FlxSprite</code> at the specified position.
        /// Optionally can load a simple, one-frame graphic instead.
        /// </summary>
        /// <param name="X">The initial X position of the sprite.</param>
        /// <param name="Y">The initial Y position of the sprite.</param>
        public FlxSprite(float X, float Y)
        {
            // call our constructor
            constructor(X, Y, null);
        }
        /// <summary>
        /// Creates a white 8x8 square <code>FlxSprite</code> at the specified position.
        /// Optionally can load a simple, one-frame graphic instead.
        /// </summary>
        /// <param name="X">The initial X position of the sprite.</param>
        /// <param name="Y">The initial Y position of the sprite.</param>
        /// <param name="SimpleGraphic">The graphic you want to display (OPTIONAL - for simple stuff only, do NOT use for animated images!). 
        /// <para>FlxG.Content.Load&lt;Texture2D&gt;("flixel/exampleImage")</para></param>
        public FlxSprite(float X, float Y, Texture2D SimpleGraphic)
        {
            // call our constructor
            constructor(X, Y, SimpleGraphic);
        }

        /// <summary>
        /// FlxSprite private constructor
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="SimpleGraphic"></param>
        private void constructor(float X, float Y, Texture2D SimpleGraphic)
        {
            x = X;
            y = Y;

            originalPosition.X = X;
            originalPosition.Y = Y;

            _flashRect = new Rectangle();
            _flashRect2 = new Rectangle();
            _flashPointZero = new Point();
            offset = new Point();

            scale = 1f;
            _alpha = 1;
            _color = Color.White;
            _lastColor = Color.White;
            blend = null;
            antialiasing = false;

            finished = false;
            facing = Flx2DFacing.NotUsed;
            _animations = new List<FlxAnim>();
            _flipped = 0;
            _curAnim = null;
            _curFrame = 0;
            _caf = 0;
            _frameTimer = 0;
            boundingBoxOverride = true;

            _colorFlicker = false;
            _colorFlickerTimer = -1;

            //_mtx = new Matrix();
            _callback = null;
            //if (_gfxSprite == null)
            //{
            //    _gfxSprite = new Sprite();
            //    _gfx = _gfxSprite.graphics;
            //}

            if (SimpleGraphic == null)
                createGraphic(8, 8, color);
            else
                loadGraphic(SimpleGraphic);
        }

        /// <summary>
        /// Load an image from an embedded graphic file.
        /// </summary>
        /// <param name="Graphic">The image you want to use. FlxG.Content.Load&lt;Texture2D&gt;("Revvolvver/bgLarge")</param>
        /// <returns>This FlxSprite instance (nice for chaining stuff together, if you're into that).</returns>
        public virtual FlxSprite loadGraphic(Texture2D Graphic)
        {
            return loadGraphic(Graphic, false, false, 0, 0);
        }
        /// <summary>
        /// Load an image from an embedded graphic file.
        /// </summary>
        /// <param name="Graphic">The image you want to use. FlxG.Content.Load&lt;Texture2D&gt;("Revvolvver/bgLarge")</param>
        /// <param name="Animated">Whether the Graphic parameter is a single sprite or a row of sprites.</param>
        /// <returns>This FlxSprite instance (nice for chaining stuff together, if you're into that).</returns>
        public virtual FlxSprite loadGraphic(Texture2D Graphic, bool Animated)
        {
            return loadGraphic(Graphic, Animated, false, 0, 0);
        }
        /// <summary>
        /// Load an image from an embedded graphic file.
        /// </summary>
        /// <param name="Graphic">The image you want to use. FlxG.Content.Load&lt;Texture2D&gt;("Revvolvver/bgLarge")</param>
        /// <param name="Animated">Whether the Graphic parameter is a single sprite or a row of sprites.</param>
        /// <param name="Reverse">Whether you need this class to generate horizontally flipped versions of the animation frames.</param>
        /// <param name="Width">OPTIONAL - Specify the width of your sprite (helps FlxSprite figure out what to do with non-square sprites or sprite sheets).</param>
        /// <returns>This FlxSprite instance (nice for chaining stuff together, if you're into that).</returns>
        public virtual FlxSprite loadGraphic(Texture2D Graphic, bool Animated, bool Reverse, int Width)
        {
            return loadGraphic(Graphic, Animated, Reverse, Width, 0);
        }
        /// <summary>
        /// Load an image from an embedded graphic file.
        /// <para>Load in this fashion : FlxG.Content.Load&lt;Texture2D&gt;("initials/autotiles_16x16")</para>
        /// </summary>
        /// <param name="Graphic">The image you want to use. FlxG.Content.Load&lt;Texture2D&gt;("Revvolvver/bgLarge")</param>
        /// <param name="Animated">Whether the Graphic parameter is a single sprite or a row of sprites.</param>
        /// <param name="Reverse">Whether you need this class to generate horizontally flipped versions of the animation frames.</param>
        /// <param name="Width">OPTIONAL - Specify the width of your sprite (helps FlxSprite figure out what to do with non-square sprites or sprite sheets).</param>
        /// <param name="Height">OPTIONAL - Specify the height of your sprite (helps FlxSprite figure out what to do with non-square sprites or sprite sheets).</param>
        /// <returns>This FlxSprite instance (nice for chaining stuff together, if you're into that).</returns>
        public virtual FlxSprite loadGraphic(Texture2D Graphic, bool Animated, bool Reverse, int Width, int Height)
		{
            _stretchToFit = false;
            _tex = Graphic;
			if(Reverse)
				_flipped = (uint)Graphic.Width>>1;
			else
				_flipped = 0;
			if(Width == 0)
			{
				if(Animated)
					Width = Graphic.Height;
				else if(_flipped > 0)
                    Width = (int)((float)Graphic.Width * 0.5f);
				else
                    Width = Graphic.Width;
			}
			width = frameWidth = Width;
			if(Height == 0)
			{
				if(Animated)
                    Height = (int)width;
				else
                    Height = Graphic.Height;
			}
			height = frameHeight = Height;
			resetHelpers();
			return this;
		}

        /// <summary>
        /// This function creates a flat colored square image dynamically.
        /// </summary>
        /// <param name="Width">The width of the sprite you want to generate.</param>
        /// <param name="Height">The height of the sprite you want to generate.</param>
        /// <param name="Color">Specifies the color of the generated block.</param>
        /// <returns>This FlxSprite instance (nice for chaining stuff together, if you're into that).</returns>
		public FlxSprite createGraphic(int Width, int Height, Color Color)
		{
            _tex = FlxG.XnaSheet;
            _stretchToFit = true;
            _facing2d = Flx2DFacing.NotUsed;

            frameWidth = 1;
            frameHeight = 1;
			width = Width;
			height = Height;
            _color = Color;
			resetHelpers();
			return this;
		}


        /// <summary>
        /// Places this object at the specified object.
        /// </summary>
        /// <param name="obj"></param>
        public void at(FlxObject obj)
        {
            x = obj.x;
            y = obj.y;
        }


		/// <summary>
        /// Resets some important variables for sprite optimization and rendering.
		/// </summary>
        protected void resetHelpers()
		{
			_flashRect2.X = 0;
			_flashRect2.Y = 0;
            if (_stretchToFit == false)
            {
                _flashRect.X = 0;
                _flashRect.Y = 0;
                _flashRect.Width = frameWidth;
                _flashRect.Height = frameHeight;

                _flashRect2.Width = _tex.Width;
                _flashRect2.Height = _tex.Height;
            }
            else
            {
                _flashRect.X = 1;
                _flashRect.Y = 1;
                _flashRect.Width = frameWidth;
                _flashRect.Height = frameHeight;

                _flashRect2.Width = (int)width;
                _flashRect2.Height = (int)height;
            }

			_origin.X = frameWidth*0.5f;
			_origin.Y = frameHeight*0.5f;

            frames = (_flashRect2.Width / _flashRect.Width) * (_flashRect2.Height / _flashRect.Height);

            _caf = 0;
			refreshHulls();
		}

        public void setOffset(int X, int Y)
        {
            offset.X = X;
            offset.Y = Y;

            adjustOrigin();
        }

        public void adjustOrigin()
        {
            _origin.X = (width) * 0.5f;
            _origin.Y = (height) * 0.5f;
        }

        /// <summary>
        /// Internal function for updating the sprite's animation.
        /// Useful for cases when you need to update this but are buried down in too many supers.
        /// This function is called automatically by <code>FlxSprite.update()</code>.
        /// </summary>
		protected void updateAnimation()
		{
			if((_curAnim != null) && (_curAnim.delay > 0) && (_curAnim.looped || !finished))
			{
				_frameTimer += FlxG.elapsed;
				while(_frameTimer > _curAnim.delay)
				{
					_frameTimer = _frameTimer - _curAnim.delay;
					if(_curFrame == _curAnim.frames.Length-1)
					{
						if(_curAnim.looped) _curFrame = 0;
						finished = true;
					}
					else
						_curFrame++;
					_caf = _curAnim.frames[_curFrame];
					calcFrame();
				}
			}
		}

        /// <summary>
        /// Main game loop update function.  Override this to create your own sprite logic!
        /// Just don't forget to call super.update() or any of the helper functions.
        /// </summary>
        public override void update()
        {
            preUpdate();
            updateMotion();
            updateAnimation();
            updateFlickering();
            
            if (FlxG.colorFlickeringEnabled) updatecolorFlickering();

        }

        /// <summary>
        /// Just updates the render-style flickering.
        /// </summary>
        public virtual void updatecolorFlickering()
        {
            if (colorFlickering())
            {
                if (_colorFlickerTimer > 0)
                {
                    _colorFlickerTimer -= FlxG.elapsed;
                    if (_colorFlickerTimer == 0)
                    {
                        _colorFlickerTimer = -1;
                    }
                }
                if (_colorFlickerTimer < 0) colorFlicker(-1);
                else
                {
                    _colorFlicker = !_colorFlicker;

                    if (_colorFlicker) color = new Color(0.99f, 0.99f, 0.99f);
                    else if (!_colorFlicker)
                    {
                        color = new Color(
                            FlxU.random(_colorFlickerRMin, _colorFlickerRMax),
                            FlxU.random(_colorFlickerGMin, _colorFlickerGMax),
                            FlxU.random(_colorFlickerBMin, _colorFlickerBMax));
                    }
                }
            }
            else
            {
                color = _lastColor;
            }
        }

        public void setColorFlickerValues(float rMin, float gMin, float bMin, float rMax, float gMax, float bMax)
        {
            _colorFlickerRMin = rMin;
            _colorFlickerGMin = gMin;
            _colorFlickerBMin = bMin;

            _colorFlickerRMax = rMax;
            _colorFlickerGMax = gMax;
            _colorFlickerBMax = bMax;
        }

        /// <summary>
        /// Tells this object to flicker, retro-style.
        /// </summary>
        /// <param name="Duration">How many seconds to flicker for.</param>
        public void colorFlicker(float Duration)
        {
            _colorFlickerTimer = Duration;

            if (_colorFlickerTimer < 0)
            {
                _colorFlicker = false;
                visible = true;
                color = _lastColor;
            }
            else
            {
                _lastColor = color;
            }
        }

        /// <summary>
        /// Check to see if the object is still flickering.
        /// </summary>
        /// <returns>Whether the object is flickering or not.</returns>
        public bool colorFlickering() { return _colorFlickerTimer >= 0; }

        /// <summary>
        /// Called by game loop, updates then blits or renders current frame of animation to the screen
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        public override void render(SpriteBatch spriteBatch)
        {
            if (visible == false || exists == false)
            {
                return;
            }

            //X-flixel only: adjust for origin (by default we match flixel's behavior by using a center origin).
            Vector2 pos = Vector2.Zero;
            Vector2 vc = Vector2.Zero;

            pos = getScreenXY() + origin;
            pos += (new Vector2(_flashRect.Width - width, _flashRect.Height - height)
                * (origin / new Vector2(width, height)));

            //the origin must be recalculated based on the difference between the
            //object's actual (collision) dimensions and its art (animation) dimensions.

            // BROKEN WHEN USING OFFSETS!
            vc = new Vector2(_flashRect.Width, _flashRect.Height);
            if (!_stretchToFit)
            {
                vc *= (origin / new Vector2(width, height));
            }
            else
            {
                vc *= (origin / new Vector2(width + 1, height + 1));
                
            }

            if (_facing2d == Flx2DFacing.NotUsed)
            {
                if (_stretchToFit)
                {
                    //if (width == 7)
                    //    vc = vc;
                    spriteBatch.Draw(_tex, new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height),
                                    _flashRect, _color,
                                    _radians, vc, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(_tex, pos,
                                    _flashRect, _color,
                                    _radians, vc, scale, SpriteEffects.None, 0);
                    //spriteBatch.GraphicsDevice.BlendState.
                }
            }
            else if (_facing2d == Flx2DFacing.Right)
            {
                spriteBatch.Draw(_tex, pos,
                                _flashRect, _color,
                                _radians, vc, scale, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(_tex, pos,
                                _flashRect, _color,
                                _radians, vc, scale, SpriteEffects.FlipHorizontally, 0);
            }

            if (FlxG.showBounds && boundingBoxOverride==true)
            {
                pos = getScreenXY();
                pos.X += offset.X;
                pos.Y += offset.Y;
                drawBounds(spriteBatch, (int)pos.X, (int)pos.Y);
                drawPivot(spriteBatch, (int)(origin.X), (int)(origin.Y), Color.Yellow);
                drawPivot(spriteBatch, (int)(pos.X + origin.X), (int)(pos.Y + origin.Y), Color.Purple);
            }
        }

        /// <summary>
        /// Checks to see if a point in 2D space overlaps this FlxCore object.
        /// </summary>
        /// <param name="X">The X coordinate of the point.</param>
        /// <param name="Y">The Y coordinate of the point.</param>
        /// <param name="PerPixel">Whether or not to use per pixel collision checking.</param>
        /// <returns>Whether or not the point overlaps this object.</returns>
        override public bool overlapsPoint(float X, float Y, bool PerPixel)
		{
			X = X + FlxU.floor(FlxG.scroll.X);
			Y = Y + FlxU.floor(FlxG.scroll.Y);
			_point = getScreenXY();
            //if(PerPixel)
            //    return _framePixels.hitTest(new Point(0,0),0xFF,new Point(X-_point.x,Y-_point.y));
			//else
            if (_stretchToFit == false)
            {
                if ((X <= _point.X) || (X >= _point.X + frameWidth) || (Y <= _point.Y) || (Y >= _point.Y + frameHeight))
                    return false;
            }
            else
            {
                if ((X <= _point.X) || (X >= _point.X + width) || (Y <= _point.Y) || (Y >= _point.Y + height))
                    return false;
            }
			return true;
		}
		
		/// <summary>
        /// Triggered whenever this sprite is launched by a <code>FlxEmitter</code>.
		/// </summary>
		virtual public void onEmit() { }

        /// <summary>
        /// Adds a new animation to the sprite.
        /// </summary>
        /// <param name="Name">What this animation should be called (e.g. "run").</param>
        /// <param name="Frames">An array of numbers indicating what frames to play in what order <code>new int [] {1, 2, 3, 0}</code>.</param>
        public void addAnimation(string Name, int[] Frames)
        {
            _animations.Add(new FlxAnim(Name, Frames, 0, true));
        }
        /// <summary>
        /// Adds a new animation to the sprite.
        /// </summary>
        /// <param name="Name">What this animation should be called (e.g. "run").</param>
        /// <param name="Frames">An array of numbers indicating what frames to play in what order <code>new int [] {1, 2, 3, 0}</code>.</param>
        /// <param name="FrameRate">The speed in frames per second that the animation should play at (e.g. 40 fps).</param>
        public void addAnimation(string Name, int[] Frames, int FrameRate)
        {
            _animations.Add(new FlxAnim(Name, Frames, FrameRate, true));
        }
        /// <summary>
        /// Adds a new animation to the sprite.
        /// </summary>
        /// <param name="Name">What this animation should be called (e.g. "run").</param>
        /// <param name="Frames">An array of numbers indicating what frames to play in what order <code>new int [] {1, 2, 3, 0}</code>.</param>
        /// <param name="FrameRate">The speed in frames per second that the animation should play at (e.g. 40 fps).</param>
        /// <param name="Looped">Whether or not the animation is looped or just plays once.</param>
        public void addAnimation(string Name, int[] Frames, int FrameRate, bool Looped)
		{
			_animations.Add(new FlxAnim(Name, Frames, FrameRate, Looped));
		}


        /// <summary>
        /// Pass in a function to be called on each frame change or animation change.
        /// </summary>
        /// <param name="ac">A function that has 3 parameters: a string name, a uint frame number, and a uint frame index.
        /// <para>Should look like this.</para>
        /// <para>public void pr(string Name, uint Frame, int FrameIndex)</para></param>
        public void addAnimationCallback(FlxAnimationCallback ac)
        {
            _callback = ac;
        }

        /// <summary>
        /// Plays an existing animation (e.g. "run").
        /// If you call an animation that is already playing it will be ignored.
        /// </summary>
        /// <param name="AnimName">The string name of the animation you want to play.</param>
        /// <param name="Force">Whether to force the animation to restart.</param>
        public void play(string AnimName, bool Force)
		{
			if(!Force && (_curAnim != null) && (AnimName == _curAnim.name)) return;
			_curFrame = 0;
            _caf = 0;
            _frameTimer = 0;
			for(int i = 0; i < _animations.Count; i++)
			{
				if(_animations[i].name == AnimName)
				{
                    _curAnim = _animations[i];
                    if (_curAnim.delay <= 0)
                        finished = true;
                    else
                        finished = false;
                    _caf = _curAnim.frames[_curFrame];
                    calcFrame();
                    return;
                }
			}
		}

        /// <summary>
        /// Plays an existing animation (e.g. "run").
        /// If you call an animation that is already playing it will be ignored.
        /// </summary>
        /// <param name="AnimName">The string name of the animation you want to play.</param>
        public void play(string AnimName)
        {
            play(AnimName, false);
        }

        /// <summary>
        /// Tell the sprite which way to face (you can just set 'facing' but this function also updates the animation instantly)
        /// In "normal" flixel, this is an int.
        /// </summary>
		public Flx2DFacing facing
		{
            get
            {
                return _facing2d;
            }
            set
            {
			    bool c = _facing2d != value;
			    _facing2d = value;
			    if(c) calcFrame();
            }
		}

        /// <summary>
        /// Tell the sprite to change to a random frame of animation
        /// Useful for instantiating particles or other weird things.
        /// </summary>
        public void randomFrame()
        {
			_curAnim = null;
			_caf = (int)(FlxU.random()*(_tex.Width/frameWidth));
			calcFrame();
        }

        /// <summary>
        /// Tell the sprite to change to a specific frame of animation.
        /// Can also be used to get the frame which is being displayed.
        /// @param	Frame	The frame you want to display.
        /// </summary>
        public int frame
        {
            get
            {
                return _caf;
            }
            set
            {
                _curAnim = null;
                _caf = value;
                calcFrame();
            }
        }

        /// <summary>
        /// Call this function to figure out the on-screen position of the object.
        /// 
        /// Takes a <code>Point</code> object and assigns the post-scrolled X and Y values of this object to it.
        /// </summary>
        /// <returns>The <code>Point</code> you passed in, or a new <code>Point</code> if you didn't pass one, containing the screen X and Y position of this object.</returns>
        override public Vector2 getScreenXY()
        {
            Vector2 Point = Vector2.Zero;
            Point.X = FlxU.floor(x + FlxU.roundingError) + FlxU.floor(FlxG.scroll.X * scrollFactor.X) - offset.X;
            Point.Y = FlxU.floor(y + FlxU.roundingError) + FlxU.floor(FlxG.scroll.Y * scrollFactor.Y) - offset.Y;
            return Point;
        }

        /// <summary>
        /// Internal function to update the current animation frame.
        /// </summary>
        protected void calcFrame()
		{
            if (_stretchToFit)
            {
                _flashRect = new Rectangle(0, 0, frameWidth, frameHeight);
                return;
            }
			else
			{
                uint rx = (uint)(_caf * frameWidth);
                uint ry = 0;

                //Handle sprite sheets
                uint w = (uint)_tex.Width;
                if (rx >= w)
                {
                    ry = (uint)(rx / w) * (uint)frameHeight;
                    rx %= w;
                }

                //handle reversed sprites

                // Commented out for breaking multiple Y framed pics.

                //if ((_facing2d == Flx2DFacing.Left) && (_flipped > 0))
                //    rx = (_flipped << 1) - rx - (uint)frameWidth;

                _flashRect = new Rectangle((int)rx, (int)ry, frameWidth, frameHeight);
			}
            if (_callback != null && _curAnim != null) _callback(_curAnim.name, (uint)_curFrame, _caf);
		}

        protected void drawPivot(SpriteBatch spriteBatch, int X, int Y, Color col)
        {
            spriteBatch.Draw(FlxG.XnaSheet,
                new Rectangle((int)(FlxU.floor(X + FlxU.roundingError) + FlxU.floor(FlxG.scroll.X * scrollFactor.X)),
                    (int)(FlxU.floor(Y + FlxU.roundingError) + FlxU.floor(FlxG.scroll.Y * scrollFactor.Y)),
                    1,
                    1),
                new Rectangle(1, 1, 1, 1),
                col);
        }

        /// <summary>
        /// Draws bounds
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        protected void drawBounds(SpriteBatch spriteBatch, int X, int Y)
        {
            spriteBatch.Draw(FlxG.XnaSheet,
                new Rectangle((int)(FlxU.floor(x + FlxU.roundingError) + FlxU.floor(FlxG.scroll.X * scrollFactor.X)),
                    (int)(FlxU.floor(y + FlxU.roundingError) + FlxU.floor(FlxG.scroll.Y * scrollFactor.Y)), 
                    (int)width, 
                    (int)height),
                new Rectangle(1, 1, 1, 1), 
                getBoundingColor());

            if (path != null)
            {
                //int count = 0;
                foreach (var item in path.nodes)
                {
                    spriteBatch.Draw(FlxG.XnaSheet,
                        new Rectangle((int)item.X - 2 + (int)(FlxU.floor(FlxG.scroll.X * scrollFactor.X)),
                            (int)item.Y - 2 + (int)(FlxU.floor(FlxG.scroll.Y * scrollFactor.Y)), 
                            4, 
                            4),
                        new Rectangle(1, 1, 1, 1), 
                        Color.DarkOrange);

                    //if (count < path.nodes.Count - 1)
                    //{
                    //    float xangle = (float)Math.Atan2(path.nodes[count + 1].Y - path.nodes[count].Y, path.nodes[count + 1].X - path.nodes[count].X);
                    //    float length = Vector2.Distance(path.nodes[count], path.nodes[count + 1]);

                    //    spriteBatch.Draw(FlxG.XnaSheet, path.nodes[count], null, color,
                    //               xangle, Vector2.Zero, new Vector2(length, 1.0f),
                    //               SpriteEffects.None, 0);
                    //}

                    //count++;
                }
            }
        }

 



    }

}
