using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /// <summary>
    /// <para> is the base class for most of the display objects (<code>FlxSprite</code>, <code>FlxText</code>, etc). </para>
    /// <para> It includes some basic attributes about game objects, including retro-style flickering,</para>
    /// <para> basic state information, sizes, scrolling, and basic physics and motion.</para>
    /// </summary>
    public class FlxObject
    {

        /// <summary>
        /// Override for drawing bounding box. Used to turn off bg sprites bounding boxes
        /// <para>True = displays bounding box.</para>
        /// <para>False = never will display bounding box.</para>
        /// </summary>
        public bool boundingBoxOverride;
        /// <summary>
        /// Kind of a global on/off switch for any objects descended from <code>FlxObject</code>.
        /// </summary>
        public bool exists;
        /// <summary>
        /// If an object is not alive, the game loop will not automatically call <code>update()</code> on it.
        /// </summary>
        public bool active;
        /// <summary>
        /// If an object is not visible, the game loop will not automatically call <code>render()</code> on it.
        /// </summary>
        public bool visible;
		/// <summary>
        /// Internal tracker for whether or not the object collides (see <code>solid</code>).
		/// </summary>
		protected bool _solid;
		/// <summary>
        /// Internal tracker for whether an object will move/alter position after a collision (see <code>fixed</code>).
		/// </summary>
		protected bool _fixed;

		/// <summary>
        /// The basic speed of this object.
		/// </summary>
		public Vector2 velocity;

        /// <summary>
        /// How fast the speed of this object is changing.
        /// Useful for smooth movement and gravity.
        /// </summary>
		public Vector2 acceleration;

        /// <summary>
        /// This isn't drag exactly, more like deceleration that is only applied
        /// when acceleration is not affecting the sprite.
        /// </summary>
		public Vector2 drag;

        /// <summary>
        /// If you are using <code>acceleration</code>, you can use <code>maxVelocity</code> with it
        /// to cap the speed automatically (very useful!).
        /// </summary>
		public Vector2 maxVelocity;

        /// <summary>
        /// Set the angle of a sprite to rotate it.
        /// WARNING: rotating sprites decreases rendering
        /// performance for this sprite by a factor of 10x!
        /// </summary>
        public float angle
        {
            get { return _angle; }
            set { _angle = value; _radians = MathHelper.ToRadians(_angle); }
        }
        //@benbaird We keep some private angle-related members in X-flixel due to XNA rotation differences

        /// <summary>
        /// @benbaird We keep some private angle-related members in X-flixel due to XNA rotation differences
        /// Note, test!
        /// </summary>
        protected float _angle = 0f;
        
        /// <summary>
        /// @benbaird We keep some private angle-related members in X-flixel due to XNA rotation differences
        /// Note, test!
        /// </summary>
        protected float _radians = 0f;

        /// <summary>
        /// This is how fast you want this sprite to spin.
        /// </summary>
		public float angularVelocity;
		/// <summary>
        /// How fast the spin speed should change.
		/// </summary>
		public float angularAcceleration;
		/// <summary>
        /// Like <code>drag</code> but for spinning.
		/// </summary>
		public float angularDrag;
		/// <summary>
        /// Use in conjunction with <code>angularAcceleration</code> for fluid spin speed control.
		/// </summary>
		public float maxAngular;
 
        /// <summary>
        /// WARNING: The origin of the sprite will default to its center.
        /// If you change this, the visuals and the collisions will likely be
        /// pretty out-of-sync if you do any rotation.
        /// 
        /// modified for X-flixel
        /// </summary>
        protected Vector2 _origin = Vector2.Zero;
        virtual public Vector2 origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// If you want to do Asteroids style stuff, check out thrust,
        /// instead of directly accessing the object's velocity or acceleration.
        /// </summary>
		public float thrust;
		/// <summary>
        /// Used to cap <code>thrust</code>, helpful and easy!
		/// </summary>
		public float maxThrust;
		/// <summary>
        /// A handy "empty point" object
		/// </summary>
		static protected Vector2 _pZero = Vector2.Zero;
		
        /// <summary>
        /// A point that can store numbers from 0 to 1 (for X and Y independently)
        /// that governs how much this object is affected by the camera subsystem.
        /// 0 means it never moves, like a HUD element or far background graphic.
        /// 1 means it scrolls along a the same speed as the foreground layer.
        /// scrollFactor is initialized as (1,1) by default.
        /// </summary>
		public Vector2 scrollFactor;
		/// <summary>
        /// Internal helper used for retro-style flickering.
		/// </summary>
		protected bool _flicker;
		/// <summary>
        /// Internal helper used for retro-style flickering.
		/// </summary>
		protected float _flickerTimer;
		/// <summary>
        /// Handy for storing health percentage or armor points or whatever.
		/// </summary>
		public float health;
        /// <summary>
        /// How much to take off health in the event of collision.
        /// </summary>
        public float damage;

		/// <summary>
        /// Handy for tracking gameplay or animations.
		/// </summary>
        public bool dead;

		/// <summary>
        /// This is just a pre-allocated x-y point container to be used however you like
		/// </summary>
		protected Vector2 _point;
		/// <summary>
        /// This is just a pre-allocated rectangle container to be used however you like
		/// </summary>
		protected Rectangle _rect;
		/// <summary>
        ///  This is a pre-allocated Flash Point object, which is useful for certain Flash graphics API calls
		/// </summary>
		protected Vector2 _flashPoint;


        /// <summary>
        /// Stores the first position in the level the object was placed.
        /// </summary>
        public Vector2 originalPosition;

        /// <summary>
        /// Set this to false if you want to skip the automatic motion/movement stuff (see <code>updateMotion()</code>).
        /// FlxObject and FlxSprite default to true.
        /// FlxText, FlxTileblock, FlxTilemap and FlxSound default to false.
        /// </summary>
		public bool moves;
		/// <summary>
        /// These store a couple of useful numbers for speeding up collision resolution.
		/// </summary>
		public FlxRect colHullX;
		/// <summary>
        /// These store a couple of useful numbers for speeding up collision resolution.
		/// </summary>
        public FlxRect colHullY;
		/// <summary>
        /// These store a couple of useful numbers for speeding up collision resolution.
		/// </summary>
		public Vector2 colVector;
		/// <summary>
        /// An array of <code>FlxPoint</code> objects.  By default contains a single offset (0,0).
		/// </summary>
		public List<Vector2> colOffsets = new List<Vector2>();
		/// <summary>
        ///  Dedicated internal flag for whether or not this class is a FlxGroup.
		/// </summary>
		internal bool _group;

        /// <summary>
        /// Flag that indicates whether or not you just hit the floor.
        /// Primarily useful for platformers, this flag is reset during the <code>updateMotion()</code>.
        /// </summary>
		public bool onFloor;
		/// <summary>
        /// Flag for direction collision resolution.
		/// </summary>
		public bool collideLeft;
		/// <summary>
        /// Flag for direction collision resolution.
		/// </summary>
		public bool collideRight;
		/// <summary>
        /// Flag for direction collision resolution.
		/// </summary>
		public bool collideTop;
		/// <summary>
        /// Flag for direction collision resolution.
		/// </summary>
		public bool collideBottom;

        // X-flixel only: Positioning variables to compensate for the fact that in
        // standard flixel, FlxObject inherits from FlxRect.

        /// <summary>
        /// X Position
        /// </summary>
        public float x;

        /// <summary>
        /// Y position
        /// </summary>
        public float y;
        /// <summary>
        /// Width
        /// </summary>
        public float width;

        /// <summary>
        /// Height
        /// </summary>
        public float height;

        /// <summary>
        /// Helper for debugging. Originally used to pin down crashes in buttons.
        /// </summary>
        public string debugName;


        /// <summary>
        /// Path behavior controls, move from the start of the path to the end then stop.
        /// </summary>
        public const uint PATH_FORWARD = 0x000000;

        /// <summary>
        /// Path behavior controls: move from the end of the path to the start then stop.
        /// </summary>
        public const uint PATH_BACKWARD = 0x000001;

        /// <summary>
        /// Path behavior controls: move from the start of the path to the end then directly back to the start, and start over.
        /// </summary>
        public const uint PATH_LOOP_FORWARD = 0x000010;

        /// <summary>
        /// Path behavior controls: move from the end of the path to the start then directly back to the end, and start over.
        /// </summary>
        public const uint PATH_LOOP_BACKWARD = 0x000100;

        /// <summary>
        /// Path behavior controls: move from the start of the path to the end then turn around and go back to the start, over and over.
        /// </summary>
        public const uint PATH_YOYO = 0x001000;

        /// <summary>
        /// Path behavior controls: ignores any vertical component to the path data, only follows side to side.
        /// </summary>
        public const uint PATH_HORIZONTAL_ONLY = 0x010000;

        /// <summary>
        /// Path behavior controls: ignores any horizontal component to the path data, only follows up and down.
        /// </summary>
        public const uint PATH_VERTICAL_ONLY = 0x100000;

        /// <summary>
        /// A reference to a path object.  Null by default, assigned by <code>followPath()</code>.
        /// </summary>
        public FlxPath path;

        /// <summary>
        /// The speed at which the object is moving on the path.
        /// When an object completes a non-looping path circuit,
        /// the pathSpeed will be zeroed out, but the <code>path</code> reference
        /// will NOT be nulled out.  So <code>pathSpeed</code> is a good way
        /// to check if this object is currently following a path or not.
        /// </summary>
        public float pathSpeed;

        /// <summary>
        /// Holder to maintain a path speed.
        /// </summary>
        protected float _pathSpeed;
        
        /// <summary>
        /// The angle in degrees between this object and the next node, where 0 is directly upward, and 90 is to the right.
        /// </summary>
        public float pathAngle;

        /// <summary>
        /// Internal helper, tracks which node of the path this object is moving toward.
        /// </summary>
        protected int _pathNodeIndex;

        /// <summary>
        /// Internal tracker for path behavior flags (like looping, horizontal only, etc).
        /// </summary>
        protected uint _pathMode;

        /// <summary>
        /// Internal helper for node navigation, specifically yo-yo and backwards movement.
        /// </summary>
        protected int _pathInc;

        /// <summary>
        /// Internal flag for whether the object's angle should be adjusted to the path angle during path follow behavior.
        /// </summary>
        protected bool _pathRotate;

        /// <summary>
        /// <value>0</value> is Linear movement.
        /// <value>0.001f and above</value> Higher numbers will result in tighter curves.
        /// </summary>
        public float pathCornering;

        /// <summary>
        /// A helper string so that you can set "modes" or "flags"
        /// For examples set this to "attacking" and then check in movement for "attacking" enemies
        /// </summary>
        public string mode;
        

        /// <summary>
        /// Creates a new <code>FlxObject</code>.
        /// </summary>
        public FlxObject()
        {
            constructor1(0, 0, 0, 0);
        }
        /// <summary>
        /// Creates a new <code>FlxObject</code>.
        /// </summary>
        /// <param name="X">The X-coordinate of the point in space.</param>
        /// <param name="Y">The Y-coordinate of the point in space.</param>
        /// <param name="Width">Desired width of the rectangle.</param>
        /// <param name="Height">Desired height of the rectangle.</param>
        public FlxObject(float X, float Y, float Width, float Height)
        {
            constructor1(X, Y, Width, Height);
        }
        /// <summary>
        /// Creates a new <code>FlxObject</code>.
        /// </summary>
        /// <param name="X">The X-coordinate of the point in space.</param>
        /// <param name="Y">The Y-coordinate of the point in space.</param>
        /// <param name="Width">Desired width of the rectangle.</param>
        /// <param name="Height">Desired height of the rectangle.</param>
        private void constructor1(float X, float Y, float Width, float Height)
        {
            x = X;
            y = Y;

            originalPosition.X = X;
            originalPosition.Y = Y;

            width = Width;
            height = Height;

            exists = true;
            active = true;
            visible = true;
            _solid = true;
            _fixed = false;
            moves = true;

            collideLeft = true;
            collideRight = true;
            collideTop = true;
            collideBottom = true;

            _origin = Vector2.Zero;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            drag = Vector2.Zero;
            maxVelocity = new Vector2(10000, 10000);

            angle = 0;
            angularVelocity = 0;
            angularAcceleration = 0;
            angularDrag = 0;
            maxAngular = 10000;

            thrust = 0;

            scrollFactor = new Vector2(1, 1);
            _flicker = false;
            _flickerTimer = -1;
            health = 1;
            damage = 1;
            dead = false;
            _point = Vector2.Zero;
            _rect = Rectangle.Empty;
            _flashPoint = Vector2.Zero;

            colHullX = FlxRect.Empty;
            colHullY = FlxRect.Empty;
            colVector = Vector2.Zero;
            colOffsets.Add(Vector2.Zero);
            _group = false;

            // Initialize path
            path = null;
            pathSpeed = 0;
            pathAngle = 0;
            _pathNodeIndex = 0;
            pathCornering = 0.0f;

            mode = "";


        }

		/// <summary>
        /// Called by <code>FlxGroup</code>, commonly when game states are changed.
		/// </summary>
		virtual public void destroy()
		{
			//Nothing to destroy yet
            

            // path code.
            if (path != null)
                path.destroy();
            path = null;

		}

        /// <summary>
        /// Set <code>solid</code> to true if you want to collide this object.
        /// </summary>
        public bool solid
        {
            get { return _solid; }
            set { _solid = value; }
        }

        /// <summary>
        /// Set <code>fixed</code> to true if you want the object to stay in place during collisions.
        /// Useful for levels and other environmental objects.
        /// </summary>
        public bool @fixed
        {
            get { return _fixed; }
            set { _fixed = value; }
        }

        /// <summary>
        /// Convience method to set scrollfactors in one hit.
        /// </summary>
        /// <param name="xScroll">X value of the scroll factor.</param>
        /// <param name="yScroll">Y value of the scroll factor.</param>
        public void setScrollFactors(float xScroll, float yScroll)
        {
            scrollFactor.X = xScroll;
            scrollFactor.Y = xScroll;
        }

        /// <summary>
        /// Set position in one call.
        /// </summary>
        /// <param name="xPos">x position</param>
        /// <param name="yPos">y position</param>
        public void setPosition(float xPos, float yPos)
        {
            this.x = xPos;
            this.y = yPos;
        }

        public void setDrags(float xValue, float yValue)
        {
            drag.X = xValue;
            drag.Y = yValue;
        }






        /// <summary>
        /// Called by <code>FlxObject.updateMotion()</code> and some constructors to
        /// rebuild the basic collision data for this object.
        /// </summary>
        virtual public void refreshHulls()
		{
			colHullX.x = x;
			colHullX.y = y;
			colHullX.width = width;
			colHullX.height = height;
			colHullY.x = x;
			colHullY.y = y;
			colHullY.width = width;
			colHullY.height = height;
		}

        /// <summary>
        /// Internal function for updating the position and speed of this object.
        /// Useful for cases when you need to update this but are buried down in too many supers.
        /// </summary>
        protected void updateMotion()
        {
            if (!moves)
                return;

            if (_solid)
                refreshHulls();
            onFloor = false;

            // Motion/physics
            angularVelocity = FlxU.computeVelocity(angularVelocity, angularAcceleration, angularDrag, maxAngular);
            angle += angularVelocity * FlxG.elapsed;
            Vector2 thrustComponents;
            if (thrust != 0)
            {
                thrustComponents = FlxU.rotatePoint(-thrust, 0, 0, 0, angle);
                Vector2 maxComponents = FlxU.rotatePoint(-maxThrust, 0, 0, 0, angle);
                float max = Math.Abs(maxComponents.X);
                if (max > Math.Abs(maxComponents.Y))
                    maxComponents.Y = max;
                else
                    max = Math.Abs(maxComponents.Y);
                maxVelocity.X = Math.Abs(max);
                maxVelocity.Y = Math.Abs(max);
            }
            else
            {
                thrustComponents = Vector2.Zero;
            }
            velocity.X = FlxU.computeVelocity(velocity.X, acceleration.X + thrustComponents.X, drag.X, maxVelocity.X);
            velocity.Y = FlxU.computeVelocity(velocity.Y, acceleration.Y + thrustComponents.Y, drag.Y, maxVelocity.Y);
            x += velocity.X * FlxG.elapsed;
            y += velocity.Y * FlxG.elapsed;

            //Update collision data with new movement results
            if (!_solid)
                return;
            colVector.X = velocity.X * FlxG.elapsed;
            colVector.Y = velocity.Y * FlxG.elapsed;
            colHullX.width += ((colVector.X > 0) ? colVector.X : -colVector.X);
            if (colVector.X < 0)
                colHullX.x += colVector.X;
            colHullY.x = x;
            colHullY.height += ((colVector.Y > 0) ? colVector.Y : -colVector.Y);
            if (colVector.Y < 0)
                colHullY.y += colVector.Y;
        }

        /// <summary>
        /// Just updates the retro-style flickering.
        /// Considered update logic rather than rendering because it toggles visibility.
        /// </summary>
        public virtual void updateFlickering()
        {
            if (flickering())
            {
                if (_flickerTimer > 0)
                {
                    _flickerTimer -= FlxG.elapsed;
                    if (_flickerTimer == 0)
                    {
                        _flickerTimer = -1;
                    }
                }
                if (_flickerTimer < 0) flicker(-1);
                else
                {
                    _flicker = !_flicker;
					visible = !_flicker;
                }
            }
        }

        /// <summary>
        /// Pre-update is called right before <code>update()</code> on each object in the game loop.
        /// In <code>FlxObject</code> it controls the flicker timer,
        /// tracking the last coordinates for collision purposes,
        /// and checking if the object is moving along a path or not.
        /// </summary>
        public void preUpdate()
        {
            //Console.WriteLine("Path: {0}, {1}, {2}, {3}", path, pathSpeed, path.nodes[_pathNodeIndex], _pathNodeIndex);
            if ((path != null) && (pathSpeed != 0) && (path.nodes[_pathNodeIndex] != null))
                updatePathMotion();
        }

		/// <summary>
        /// Called by the main game loop, handles motion/physics and game logic
		/// </summary>
        virtual public void update()
		{
            //Console.WriteLine("Path: {0}, {1}, {2}, {3}", path, pathSpeed, path.nodes[_pathNodeIndex], _pathNodeIndex);

            preUpdate();
			updateMotion();
			updateFlickering();
		}

        /// <summary>
        /// Override this function to draw graphics (see <code>FlxSprite</code>).
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        virtual public void render(SpriteBatch spriteBatch)
        {
            //Objects don't have any visual logic/display of their own.
        }

        /// <summary>
        /// Checks to see if some <code>FlxObject</code> object overlaps this <code>FlxObject</code> object.
        /// </summary>
        /// <param name="Object">he object being tested.</param>
        /// <returns>Whether or not the two objects overlap.</returns>
        virtual public bool overlaps(FlxObject Object)
		{
            _point = getScreenXY();
			float tx = _point.X;
			float ty = _point.Y;
            _point = Object.getScreenXY();
			if((_point.X <= tx-Object.width) || (_point.X >= tx+width) || (_point.Y <= ty-Object.height) || (_point.Y >= ty+height))
				return false;
            
			return true;
		}

        /// <summary>
        /// Checks to see if a point in 2D space overlaps this <code>FlxObject</code> object.
        /// </summary>
        /// <param name="X">The X coordinate of the point.</param>
        /// <param name="Y">The Y coordinate of the point.</param>
        /// <returns>Whether or not the point overlaps this object.</returns>
        virtual public bool overlapsPoint(float X, float Y)
        {
            return overlapsPoint(X, Y, false);
        }
        /// <summary>
        /// Checks to see if a point in 2D space overlaps this <code>FlxObject</code> object.
        /// </summary>
        /// <param name="X">The X coordinate of the point.</param>
        /// <param name="Y">The Y coordinate of the point.</param>
        /// <param name="PerPixel">Whether or not to use per pixel collision checking (only available in <code>FlxSprite</code> subclass).</param>
        /// <returns>Whether or not the point overlaps this object.</returns>
        virtual public bool overlapsPoint(float X, float Y, bool PerPixel)
		{
			X = X + FlxU.floor(FlxG.scroll.X);
			Y = Y + FlxU.floor(FlxG.scroll.Y);
            _point = getScreenXY();
			if((X <= _point.X) || (X >= _point.X+width) || (Y <= _point.Y) || (Y >= _point.Y+height))
				return false;
			return true;
		}

        /// <summary>
        /// If you don't want to call <code>FlxU.collide()</code> you can use this instead.
        /// Just calls <code>FlxU.collide(this,Object);</code>.  Will collide against itself
        /// if Object==null.
        /// </summary>
        /// <param name="Object">The FlxObject you want to collide with.</param>
        /// <returns>Whether the object collides or not.</returns>
        virtual public bool collide(FlxObject Object)
		{
			return FlxU.collide(this,((Object==null)?this:Object));
		}

        /// <summary>
        /// <code>FlxU.collide()</code> (and thus <code>FlxObject.collide()</code>) call
        /// this function each time two objects are compared to see if they collide.
        /// It doesn't necessarily mean these objects WILL collide, however.
        /// </summary>
        /// <param name="Object">The <code>FlxObject</code> you're about to run into.</param>
        virtual public void preCollide(FlxObject Object)
		{
			//Most objects don't have to do anything here.
		}

        /// <summary>
        /// Called when this object's left side collides with another <code>FlxObject</code>'s right.
        /// NOTE: by default this function just calls <code>hitSide()</code>.
        /// </summary>
        /// <param name="Contact">The <code>FlxObject</code> you just ran into.</param>
        /// <param name="Velocity">The suggested new velocity for this object.</param>
		virtual public void hitLeft(FlxObject Contact, float Velocity)
		{
			hitSide(Contact,Velocity);
		}

        /// <summary>
        /// Called when this object's right side collides with another <code>FlxObject</code>'s left.
        /// NOTE: by default this function just calls <code>hitSide()</code>.
        /// </summary>
        /// <param name="Contact">The <code>FlxObject</code> you just ran into.</param>
        /// <param name="Velocity">The suggested new velocity for this object.</param>
        virtual public void hitRight(FlxObject Contact, float Velocity)
		{
			hitSide(Contact,Velocity);
		}

        /// <summary>
        /// Since most games have identical behavior for running into walls,
        ///  you can just override this function instead of overriding both hitLeft and hitRight. 
        /// </summary>
        /// <param name="Contact">The <code>FlxObject</code> you just ran into.</param>
        /// <param name="Velocity">The suggested new velocity for this object.</param>
        virtual public void hitSide(FlxObject Contact, float Velocity)
		{
			if(!@fixed || (Contact.@fixed && ((velocity.Y != 0) || (velocity.X != 0))))
				velocity.X = Velocity;
		}

        /// <summary>
        /// Called when this object's top collides with the bottom of another <code>FlxObject</code>.
        /// </summary>
        /// <param name="Contact">The <code>FlxObject</code> you just ran into.</param>
        /// <param name="Velocity">The suggested new velocity for this object.</param>
        virtual public void hitTop(FlxObject Contact, float Velocity)
		{
			if(!@fixed || (Contact.@fixed && ((velocity.Y != 0) || (velocity.X != 0))))
				velocity.Y = Velocity;
		}

        /// <summary>
        /// Called when this object's bottom edge collides with the top of another <code>FlxObject</code>.
        /// </summary>
        /// <param name="Contact">The <code>FlxObject</code> you just ran into.</param>
        /// <param name="Velocity">The suggested new velocity for this object.</param>
        virtual public void hitBottom(FlxObject Contact, float Velocity)
		{
			onFloor = true;
			if(!@fixed || (Contact.@fixed && ((velocity.Y != 0) || (velocity.X != 0))))
				velocity.Y = Velocity;
		}

        /// <summary>
        /// Call this function to "damage" (or give health bonus) to this sprite.
        /// </summary>
        /// <param name="Damage">How much health to take away (use a negative number to give a health bonus).</param>
		virtual public void hurt(float Damage)
		{
			health = health - Damage;
			if(health <= 0)
				kill();
		}
		
		/// <summary>
        /// Call this function to "kill" a sprite so that it no longer 'exists'.
		/// </summary>
        virtual public void kill()
		{
			exists = false;
			dead = true;
		}

        /// <summary>
        /// Tells this object to flicker, retro-style.
        /// </summary>
        /// <param name="Duration">How many seconds to flicker for.</param>
		public void flicker(float Duration) { _flickerTimer = Duration; if(_flickerTimer < 0) { _flicker = false; visible = true; } }
		
        /// <summary>
        /// Check to see if the object is still flickering.
        /// </summary>
        /// <returns>Whether the object is flickering or not.</returns>
		public bool flickering() { return _flickerTimer >= 0; }

        /// <summary>
        /// Call this function to figure out the on-screen position of the object.
        /// 
        /// @param	P	Takes a <code>Point</code> object and assigns the post-scrolled X and Y values of this object to it.
        /// </summary>
        /// <returns>The <code>Point</code> you passed in, or a new <code>Point</code> if you didn't pass one, containing the screen X and Y position of this object.</returns>
        virtual public Vector2 getScreenXY()
		{
            Vector2 Point = Vector2.Zero;
			Point.X = FlxU.floor(x + FlxU.roundingError)+FlxU.floor(FlxG.scroll.X*scrollFactor.X);
			Point.Y = FlxU.floor(y + FlxU.roundingError)+FlxU.floor(FlxG.scroll.Y*scrollFactor.Y);
			return Point;
		}
		
        /// <summary>
        /// Check and see if this object is currently on screen.
        /// </summary>
        /// <returns>Whether the object is on screen or not.</returns>
        virtual public bool onScreen()
		{
            _point = getScreenXY();
			if((_point.X + width < 0) || (_point.X > FlxG.width) || (_point.Y + height < 0) || (_point.Y > FlxG.height))
				return false;
			return true;
		}

        /// <summary>
        /// Handy function for reviving game objects.
        /// Resets their existence flags and position, including LAST position.
        /// </summary>
        /// <param name="X">The new X position of this object.</param>
        /// <param name="Y">The new Y position of this object.</param>
        virtual public void reset(float X, float Y)
		{
			x = X;
			y = Y;
			exists = true;
			dead = false;
		}

		/// <summary>
        /// Returns the appropriate color for the bounding box depending on object state.
		/// </summary>
		/// <returns>Color of the bounding box</returns>
		public Color getBoundingColor()
		{
			if(solid)
			{
				if(@fixed)
					return new Color(0x00, 0xf2, 0x25, 0x7f);
				else
					return new Color(0xff, 0x00, 0x12, 0x7f);
			}
			else
				return new Color(0x00, 0x90, 0xe9, 0x7f);
		}

        // ---------------------------- Start the path stuff here.


        public void assignPath(FlxPath Path, float Speed, uint Mode, bool AutoRotate)
        {
            if (Path.nodes.Count <= 0)
            {
                FlxG.log("WARNING: Paths need at least one node in them to be followed.");
                return;
            }

            path = Path;
            pathSpeed = 0;
            _pathSpeed = FlxU.abs(Speed);
            _pathMode = Mode;
            _pathRotate = AutoRotate;

            if ((_pathMode == PATH_BACKWARD) || (_pathMode == PATH_LOOP_BACKWARD))
            {
                _pathNodeIndex = path.nodes.Count - 1;
                _pathInc = -1;
            }
            else
            {
                _pathNodeIndex = 0;
                _pathInc = 1;
            }

        }

        public void startFollowingPath()
        {
            pathSpeed = _pathSpeed;

            if ((_pathMode == PATH_BACKWARD) || (_pathMode == PATH_LOOP_BACKWARD))
            {
                _pathNodeIndex = path.nodes.Count - 1;
                _pathInc = -1;
            }
            else
            {
                _pathNodeIndex = 0;
                _pathInc = 1;
            }
        }

        /// <summary>
        /// Call this function to give this object a path to follow.
        /// If the path does not have at least one node in it, this function
        /// will log a warning message and return.
        /// </summary>
        /// <param name="Path">The <code>FlxPath</code> you want this object to follow.</param>
        /// <param name="Speed">How fast to travel along the path in pixels per second.</param>
        /// <param name="Mode">Optional, controls the behavior of the object following the path using the path behavior constants.  Can use multiple flags at once, for example PATH_YOYO|PATH_HORIZONTAL_ONLY will make an object move back and forth along the X axis of the path only.</param>
        /// <param name="AutoRotate">Automatically point the object toward the next node.  Assumes the graphic is pointing upward.  Default behavior is false, or no automatic rotation.</param>
        public void followPath(FlxPath Path, float Speed, uint Mode, bool AutoRotate)
        {

            // Current progress : complete, needs testing.

            if (Path.nodes.Count <= 0)
            {
                FlxG.log("WARNING: Paths need at least one node in them to be followed.");
                return;
            }

            path = Path;
            pathSpeed = FlxU.abs(Speed);
            _pathSpeed = FlxU.abs(Speed);
            _pathMode = Mode;
            _pathRotate = AutoRotate;


            if((_pathMode == PATH_BACKWARD) || (_pathMode == PATH_LOOP_BACKWARD))
            {
                _pathNodeIndex = path.nodes.Count - 1;
                _pathInc = -1;
            }
            else
            {
                _pathNodeIndex = 0;
                _pathInc = 1;
            }
             
        }

        /// <summary>
        /// Tells this object to stop following the path its on.
        /// </summary>
        /// <param name="DestroyPath">Tells this function whether to call destroy on the path object.  Default value is false.</param>
        public void stopFollowingPath(bool DestroyPath=false)
        {

            // Current progress : complete, to test.

            pathSpeed = 0;
            velocity = Vector2.Zero;

            if (DestroyPath && (path != null))
            {
                path.destroy();
                path = null;
            }

        }
        
        /// <summary>
        /// Internal function that decides what node in the path to aim for next based on the behavior flags.
        /// </summary>
        /// <param name="Snap">Snap</param>
        /// <returns>The node (a <code>Vector2</code> object) we are aiming for next.</returns>
        public Vector2 advancePath(bool Snap=true)
        {
            //Console.WriteLine("advancePath " + _pathNodeIndex + " " + _pathInc);

            // Current progress : completed, test.

            if(Snap)
            {
                //Console.WriteLine("snap == YES " + _pathNodeIndex + " " + _pathInc);

                Vector2 oldNode = path.nodes[_pathNodeIndex];
                if(oldNode != null)
                {
                    if((_pathMode & PATH_VERTICAL_ONLY) == 0)
                        x = oldNode.X - width*0.5f;
                    if((_pathMode & PATH_HORIZONTAL_ONLY) == 0)
                        y = oldNode.Y - height*0.5f;
                }
            }
                        
            _pathNodeIndex += _pathInc;
                        
            if((_pathMode & PATH_BACKWARD) > 0)
            {
                //Console.WriteLine("PATH_BACKWARD " + _pathNodeIndex);

                if(_pathNodeIndex < 0)
                {
                    _pathNodeIndex = 0;
                    pathSpeed = 0;
                }
            }
            else if((_pathMode & PATH_LOOP_FORWARD) > 0)
            {
                //Console.WriteLine("PATH_LOOP_FORWARD " + _pathNodeIndex);

                if(_pathNodeIndex >= path.nodes.Count)
                    _pathNodeIndex = 0;
            }
            else if((_pathMode & PATH_LOOP_BACKWARD) > 0)
            {
                //Console.WriteLine("PATH_LOOP_BACKWARD " + _pathNodeIndex);

                if(_pathNodeIndex < 0)
                {
                    _pathNodeIndex = path.nodes.Count - 1;
                    if(_pathNodeIndex < 0)
                        _pathNodeIndex = 0;
                }
            }
            else if((_pathMode & PATH_YOYO) > 0)
            {
                //Console.WriteLine("PATH_YOYO " + _pathNodeIndex);

                if(_pathInc > 0)
                {
                    if (_pathNodeIndex >= path.nodes.Count)
                    {
                        _pathNodeIndex = path.nodes.Count - 2;
                        if(_pathNodeIndex < 0)
                            _pathNodeIndex = 0;
                       _pathInc = -_pathInc;
                    }
                }
                else if(_pathNodeIndex < 0)
                {
                    _pathNodeIndex = 1;
                    if (_pathNodeIndex >= path.nodes.Count)
                        _pathNodeIndex = path.nodes.Count - 1;
                    if(_pathNodeIndex < 0)
                        _pathNodeIndex = 0;
                    _pathInc = -_pathInc;
                }
            }
            else
            {
                //Console.WriteLine(" else " + _pathNodeIndex);

                if (_pathNodeIndex >= path.nodes.Count)
                {
                    _pathNodeIndex = path.nodes.Count - 1;
                    pathSpeed = 0;
                }
            }

            return path.nodes[_pathNodeIndex];

        }
                
        /// <summary>
        /// Internal function for moving the object along the path.
        /// Generally this function is called automatically by <code>preUpdate()</code>.
        /// The first half of the function decides if the object can advance to the next node in the path,
        /// while the second half handles actually picking a velocity toward the next node.
        /// </summary>
        public void updatePathMotion()
        {
            //first check if we need to be pointing at the next node yet
            _point.X = x + width*0.5f;
            _point.Y = y + height*0.5f;
            Vector2 node = path.nodes[_pathNodeIndex];
            float deltaX = node.X - _point.X;
            float deltaY = node.Y - _point.Y;
                        
            bool horizontalOnly = (_pathMode & PATH_HORIZONTAL_ONLY) > 0;
            bool verticalOnly = (_pathMode & PATH_VERTICAL_ONLY) > 0;
                        
            if(horizontalOnly)
            {
                if(((deltaX>0)?deltaX:-deltaX) < pathSpeed*FlxG.elapsed)
                    node = advancePath();
            }
            else if(verticalOnly)
            {
                if(((deltaY>0)?deltaY:-deltaY) < pathSpeed*FlxG.elapsed)
                    node = advancePath();
            }
            else
            {
                if(Math.Sqrt(deltaX*deltaX + deltaY*deltaY) < pathSpeed*FlxG.elapsed)
                    node = advancePath();
            }
                        
            //then just move toward the current node at the requested speed
            if(pathSpeed != 0)
            {
                //set velocity based on path mode
                _point.X = x + width*0.5f;
                _point.Y = y + height*0.5f;
                if(horizontalOnly || (_point.Y == node.Y))
                {
                    // Path cornering is 0, so move in a linear fashion.

                    if (pathCornering == 0)
                    {
                        velocity.X = (_point.X < node.X) ? pathSpeed : -pathSpeed;

                        if (velocity.X < 0)
                            pathAngle = -90;
                        else
                            pathAngle = 90;
                        if (!horizontalOnly)
                            velocity.Y = 0;
                    }
                    // Path cornering is not zero, so progress to target speed using the PathCornering value.
                    else
                    {
                        float targetVelX = (_point.X < node.X) ? pathSpeed : -pathSpeed;

                        Vector2 targetVel = new Vector2(targetVelX, 0);

                        if (velocity.X < targetVel.X) velocity.X += pathCornering;
                        if (velocity.X > targetVel.X) velocity.X -= pathCornering;
                        if (velocity.Y < targetVel.Y) velocity.Y += pathCornering;
                        if (velocity.Y > targetVel.Y) velocity.Y -= pathCornering;
                        //velocity.Y = 0;

                        if (velocity.X < 0)
                            pathAngle = -90;
                        else
                            pathAngle = 90;
                    }

                }
                else if(verticalOnly || (_point.X == node.X))
                {
                    if (pathCornering == 0)
                    {
                        //Console.WriteLine("verticalOnly 0 ");

                        velocity.Y = (_point.Y < node.Y) ? pathSpeed : -pathSpeed;

                        if (velocity.Y < 0)
                            pathAngle = 0;
                        else
                            pathAngle = 180;
                        if (!verticalOnly)
                            velocity.X = 0;
                    }
                    else
                    {
                        //Console.WriteLine("verticalOnly ");

                        float targetVelY = (_point.Y < node.Y) ? pathSpeed : -pathSpeed;
                        
                        Vector2 targetVel = new Vector2(0, targetVelY);

                        if (velocity.X < targetVel.X) velocity.X += pathCornering;
                        if (velocity.X > targetVel.X) velocity.X -= pathCornering;
                        if (velocity.Y < targetVel.Y) velocity.Y += pathCornering;
                        if (velocity.Y > targetVel.Y) velocity.Y -= pathCornering;
                        //velocity.X = 0;

                        if (velocity.Y < 0)
                            pathAngle = 0;
                        else
                            pathAngle = 180;
                    }

                }
                else
                {
                    if (pathCornering == 0)
                    {
                        //Console.WriteLine(" else 0 ");

                        pathAngle = FlxU.getAngle(_point, node);
                        velocity = (FlxU.rotatePoint(0, pathSpeed, 0, 0, pathAngle)) * -1;
                    }
                    else
                    {
                        pathAngle = FlxU.getAngle(_point, node);

                        //Console.WriteLine("else ");

                        Vector2 targetVel = (FlxU.rotatePoint(0, pathSpeed, 0, 0, pathAngle)) * -1;
                        if (velocity.X < targetVel.X) velocity.X += pathCornering;
                        if (velocity.X > targetVel.X) velocity.X -= pathCornering;
                        if (velocity.Y < targetVel.Y) velocity.Y += pathCornering;
                        if (velocity.Y > targetVel.Y) velocity.Y -= pathCornering;
                    }
                }
                                
                //then set object rotation if necessary
                if(_pathRotate)
                {
                    angularVelocity = 0;
                    angularAcceleration = 0;
                    angle = pathAngle;
                }
            }
        }

        /// <summary>
        /// Use this to trigger overlaps from your callback as defined in a FlxState.
        /// </summary>
        /// <param name="obj"></param>
        virtual public void overlapped(FlxObject obj)
        {
            //Console.WriteLine("Obj overlapped " + obj.GetType().ToString());
            //this.overlapped(obj);

        }


        // end
    }
}
