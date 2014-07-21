using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /// <summary>
    /// <code>FlxEmitter</code> is a lightweight particle emitter.
    /// It can be used for one-time explosions or for
    /// continuous fx like rain and fire.  <code>FlxEmitter</code>
    /// is not optimized or anything; all it does is launch
    /// <code>FlxSprite</code> objects out at set intervals
    /// by setting their positions and velocities accordingly.
    /// It is easy to use and relatively efficient, since it
    /// automatically redelays its sprites and/or kills
    /// them once they've been launched.
    /// </summary>
    public class FlxEmitter : FlxGroup
    {
        /// <summary>
        /// The minimum possible velocity of a particle.
        /// The default value is (-100,-100).
        /// </summary>
		public Vector2 minParticleSpeed;
		/// <summary>
        /// The maximum possible velocity of a particle.
        /// The default value is (100,100).
		/// </summary>
		public Vector2 maxParticleSpeed;
		/// <summary>
        /// The X and Y drag component of particles launched from the emitter.
		/// </summary>
		public Vector2 particleDrag;
		/// <summary>
        /// The minimum possible angular velocity of a particle.  The default value is -360.
        /// NOTE: rotating particles are more expensive to draw than non-rotating ones!
		/// </summary>
		public float minRotation;
		/// <summary>
        /// The maximum possible angular velocity of a particle.  The default value is 360.
        /// NOTE: rotating particles are more expensive to draw than non-rotating ones!
		/// </summary>
		public float maxRotation;
        /// <summary>
        /// Sets the <code>acceleration.y</code> member of each particle to this value on launch.
        /// </summary>
		public float gravity;
		/// <summary>
        /// Determines whether the emitter is currently emitting particles.
		/// </summary>
		public bool on;
		/// <summary>
        /// This variable has different effects depending on what kind of emission it is.
        /// During an explosion, delay controls the lifespan of the particles.
        /// During normal emission, delay controls the time between particle launches.
        /// NOTE: In older builds, polarity (negative numbers) was used to define emitter behavior.
        /// THIS IS NO LONGER THE CASE!  FlxEmitter.start() controls that now!
		/// </summary>
		public float delay;
		/// <summary>
        /// The number of particles to launch at a time.
		/// </summary>
		public int quantity;
		/// <summary>
        /// Checks whether you already fired a particle this frame.
		/// </summary>
		public bool justEmitted;
		/// <summary>
        /// The style of particle emission (all at once, or one at a time).
		/// </summary>
		protected bool _explode;
		/// <summary>
        /// Internal helper for deciding when to launch particles or kill them.
		/// </summary>
		protected float _timer;
		/// <summary>
        /// Internal marker for where we are in <code>_sprites</code>.
		/// </summary>
		protected int _particle;
		/// <summary>
        /// Internal counter for figuring out how many particles to launch.
		/// </summary>
		protected int _counter;

        /// <summary>
        /// Creates a new <code>FlxEmitter</code> object at a specific position.
        /// Does not automatically generate or attach particles!
        /// </summary>
        public FlxEmitter()
        {
            constructor(0, 0);
        }
        /// <summary>
        /// Creates a new <code>FlxEmitter</code> object at a specific position.
        /// Does not automatically generate or attach particles!
        /// </summary>
        /// <param name="X">The X position of the emitter.</param>
        /// <param name="Y">The Y position of the emitter.</param>
        public FlxEmitter(int X, int Y)
        {
            constructor(X, Y);
        }
        /// <summary>
        /// Creates a new <code>FlxEmitter</code> object at a specific position.
        /// Does not automatically generate or attach particles!
        /// </summary>
        /// <param name="X">The X position of the emitter.</param>
        /// <param name="Y">The Y position of the emitter.</param>
        private void constructor(int X, int Y)
        {
            x = X;
            y = Y;
            width = 0;
            height = 0;

            minParticleSpeed = new Vector2(-100, -100);
            maxParticleSpeed = new Vector2(100, 100);
            minRotation = -360;
            maxRotation = 360;
            gravity = 400;
            particleDrag = new Vector2();
            delay = 0;
            quantity = 0;
            _counter = 0;
            _explode = true;
            exists = false;
            on = false;
            justEmitted = false;
        }

        /// <summary>
        /// This function generates a new array of sprites to attach to the emitter.
        /// </summary>
        /// <param name="Graphics"><para>Use FlxG.Content.Load&lt;Texture2D&gt;("Lemonade/ccc")</para><para>If you opted to not pre-configure an array of FlxSprite objects, you can simply pass in a particle image or sprite sheet.</para></param>
        /// <param name="Quantity">The number of particles to generate when using the "create from image" option.</param>
        /// <returns>This FlxEmitter instance (nice for chaining stuff together, if you're into that).</returns>
        public FlxEmitter createSprites(Texture2D Graphics, int Quantity)
        {
            return createSprites(Graphics, Quantity, true, 0, 0);
        }
        /// <summary>
        /// This function generates a new array of sprites to attach to the emitter.
        /// </summary>
        /// <param name="Graphics"><para>Use FlxG.Content.Load&lt;Texture2D&gt;("Lemonade/ccc")</para><para>If you opted to not pre-configure an array of FlxSprite objects, you can simply pass in a particle image or sprite sheet.</para></param>
        /// <param name="Quantity">The number of particles to generate when using the "create from image" option.</param>
        /// <param name="Multiple">Whether the image in the Graphics param is a single particle or a bunch of particles (if it's a bunch, they need to be square!).</param>
        /// <returns>This FlxEmitter instance (nice for chaining stuff together, if you're into that).</returns>
        public FlxEmitter createSprites(Texture2D Graphics, int Quantity, bool Multiple)
        {
            return createSprites(Graphics, Quantity, Multiple, 0, 0);
        }
        /// <summary>
        /// This function generates a new array of sprites to attach to the emitter.
        /// </summary>
        /// <param name="Graphics"><para>Use FlxG.Content.Load&lt;Texture2D&gt;("Lemonade/ccc")</para><para>If you opted to not pre-configure an array of FlxSprite objects, you can simply pass in a particle image or sprite sheet.</para></param>
        /// <param name="Quantity">The number of particles to generate when using the "create from image" option.</param>
        /// <param name="Multiple">Whether the image in the Graphics param is a single particle or a bunch of particles (if it's a bunch, they need to be square!).</param>
        /// <param name="Collide">Whether the particles should be flagged as not 'dead' (non-colliding particles are higher performance).  0 means no collisions, 0-1 controls scale of particle's bounding box.</param>
        /// <param name="Bounce">Whether the particles should bounce after colliding with things.  0 means no bounce, 1 means full reflection.</param>
        /// <returns>This FlxEmitter instance (nice for chaining stuff together, if you're into that).</returns>
        public FlxEmitter createSprites(Texture2D Graphics, int Quantity, bool Multiple, float Collide, float Bounce)
		{
			members = new List<FlxObject>();
			int  r;
			FlxSprite s;
			int tf = 1;
			float sw;
			float sh;
			if(Multiple)
			{
				s = new FlxSprite();
				s.loadGraphic(Graphics,true);
				tf = s.frames;
			}
			int i = 0;
			while(i < Quantity)
			{
				if((Collide > 0) && (Bounce > 0))
					s = new FlxParticle(Bounce) as FlxSprite;
				else
					s = new FlxSprite();
				if(Multiple)
				{
					r = (int)(FlxU.random()*tf);
                    //if(BakedRotations > 0)
                    //    s.loadRotatedGraphic(Graphics,BakedRotations,r);
                    //else
                    //{
						s.loadGraphic(Graphics,true);
						s.frame = r;
					//}
				}
				else
				{
                    //if(BakedRotations > 0)
                    //    s.loadRotatedGraphic(Graphics,BakedRotations);
                    //else
						s.loadGraphic(Graphics);
				}
				if(Collide > 0)
				{
					sw = s.width;
					sh = s.height;
					s.width = (int)(s.width * Collide);
                    s.height = (int)(s.height * Collide);
					s.offset.X = (int)(sw-s.width)/2;
					s.offset.Y = (int)(sh-s.height)/2;
					s.solid = true;
				}
				else
					s.solid = false;
				s.exists = false;
				s.scrollFactor = scrollFactor;
				add(s);
				i++;
			}
			return this;
		}
		
        /// <summary>
        /// A more compact way of setting the width and height of the emitter.
        /// </summary>
        /// <param name="Width">The desired width of the emitter (particles are spawned randomly within these dimensions).</param>
        /// <param name="Height">The desired height of the emitter.</param>
		public void setSize(int Width, int Height)
		{
			width = Width;
			height = Height;
		}

        /// <summary>
        /// A more compact way of setting the X velocity range of the emitter.
        /// </summary>
        public void setXSpeed()
        {
            setXSpeed(0, 0);
        }
        /// <summary>
        /// A more compact way of setting the X velocity range of the emitter.
        /// </summary>
        /// <param name="Min">The minimum value for this range.</param>
        /// <param name="Max">The maximum value for this range.</param>
		public void setXSpeed(float Min, float Max)
		{
			minParticleSpeed.X = Min;
			maxParticleSpeed.X = Max;
		}

		/// <summary>
        /// A more compact way of setting the Y velocity range of the emitter.
		/// </summary>
        public void setYSpeed()
        {
            setYSpeed(0, 0);
        }
        /// <summary>
        /// A more compact way of setting the Y velocity range of the emitter.
        /// </summary>
        /// <param name="Min">The minimum value for this range.</param>
        /// <param name="Max">The maximum value for this range.</param>
        public void setYSpeed(float Min, float Max)
		{
			minParticleSpeed.Y = Min;
			maxParticleSpeed.Y = Max;
		}

		/// <summary>
        /// A more compact way of setting the angular velocity constraints of the emitter.
		/// </summary>
        public void setRotation()
        {
            setRotation(0, 0);
        }
        /// <summary>
        /// A more compact way of setting the angular velocity constraints of the emitter.
        /// </summary>
        /// <param name="Min">The minimum value for this range.</param>
        /// <param name="Max">The maximum value for this range.</param>
		public void setRotation(float Min, float Max)
		{
			minRotation = Min;
			maxRotation = Max;
		}

		/// <summary>
        /// Internal function that actually performs the emitter update (called by update()).
		/// </summary>
		protected void updateEmitter()
		{
			if(_explode)
			{
				_timer += FlxG.elapsed;
				if((delay > 0) && (_timer > delay))
				{
					kill();
					return;
				}
				if(on)
				{
					on = false;
					int i = _particle;
					int l = members.Count;
					if(quantity > 0)
						l = quantity;
					l += _particle;
					while(i < l)
					{
						emitParticle();
						i++;
					}
				}
				return;
			}
			if(!on)
				return;
			_timer += FlxG.elapsed;
			while((_timer > delay) && ((quantity <= 0) || (_counter < quantity)))
			{
				_timer -= delay;
				emitParticle();
			}
		}

        /// <summary>
        /// Internal function that actually goes through and updates all the group members.
        /// Overridden here to remove the position update code normally used by a FlxGroup.
        /// </summary>
		override protected void updateMembers()
		{
			FlxObject o;
			int i = 0;
			int l = members.Count;
			while(i < l)
			{
				o = members[i++] as FlxObject;
				if((o != null) && o.exists && o.active)
					o.update();
			}
		}

		/// <summary>
        /// Called automatically by the game loop, decides when to launch particles and when to "die".
		/// </summary>
        override public void update()
		{
			justEmitted = false;
			base.update();
			updateEmitter();
		}

        /// <summary>
        /// Call this function to start emitting particles.
        /// </summary>
        public void start()
        {
            start(true, 0, 0);
        }
        /// <summary>
        /// Call this function to start emitting particles.
        /// </summary>
        /// <param name="Explode">Whether the particles should all burst out at once.</param>
        /// <param name="Delay">You can set the delay (or lifespan) here if you want.</param>
        public void start(bool Explode, float Delay)
        {
            start(Explode, Delay, 0);
        }
        /// <summary>
        /// Call this function to start emitting particles.
        /// </summary>
        /// <param name="Explode">Whether the particles should all burst out at once.</param>
        /// <param name="Delay">You can set the delay (or lifespan) here if you want.</param>
        /// <param name="Quantity">How many particles to launch.  Default value is 0, or "all the particles".</param>
        public void start(bool Explode, float Delay, int Quantity)
		{
			if(members.Count <= 0)
			{
				FlxG.log("WARNING: there are no sprites loaded in your emitter.\nAdd some to FlxEmitter.members or use FlxEmitter.createSprites().");
				return;
			}
			_explode = Explode;
			if(!_explode)
				_counter = 0;
			if(!exists)
				_particle = 0;
			exists = true;
			visible = true;
			active = true;
			dead = false;
			on = true;
			_timer = 0;
			if(quantity == 0)
				quantity = Quantity;
			else if(Quantity != 0)
				quantity = Quantity;
			if(Delay != 0)
				delay = Delay;
			if(delay < 0)
				delay = -delay;
			if(delay == 0)
			{
				if(Explode)
					delay = 3;	//default value for particle explosions
				else
					delay = 0.1f;//default value for particle streams
			}
		}


		/// <summary>
        /// This function can be used both internally and externally to emit the next particle.
		/// </summary>
		public void emitParticle()
		{
			_counter++;
            FlxSprite s = members[_particle] as FlxSprite;
			s.visible = true;
			s.exists = true;
			s.active = true;
            s.x = x - ((int)s.width >> 1) + FlxU.random() * width;
            s.y = y - ((int)s.height >> 1) + FlxU.random() * height;
			s.velocity.X = minParticleSpeed.X;
			if(minParticleSpeed.X != maxParticleSpeed.X) s.velocity.X += FlxU.random()*(maxParticleSpeed.X-minParticleSpeed.X);
			s.velocity.Y = minParticleSpeed.Y;
			if(minParticleSpeed.Y != maxParticleSpeed.Y) s.velocity.Y += FlxU.random()*(maxParticleSpeed.Y-minParticleSpeed.Y);
			s.acceleration.Y = gravity;
			s.angularVelocity = minRotation;
			if(minRotation != maxRotation) s.angularVelocity += FlxU.random()*(maxRotation-minRotation);
			if(s.angularVelocity != 0) s.angle = FlxU.random()*360-180;
			s.drag.X = particleDrag.X;
			s.drag.Y = particleDrag.Y;
			_particle++;
			if(_particle >= members.Count)
				_particle = 0;
			s.onEmit();
			justEmitted = true;
		}

		/// <summary>
        /// Call this function to stop the emitter without killing it.
		/// </summary>
        public void stop()
        {
            stop(3f);
        }
        /// <summary>
        /// Call this function to stop the emitter without killing it.
        /// </summary>
        /// <param name="Delay">How long to wait before killing all the particles.  Set to 'zero' to never kill them.</param>
		public void stop(float Delay)
		{
			_explode = true;
			delay = Delay;
			if(delay < 0)
				delay = -Delay;
			on = false;
		}

        /// <summary>
        /// Change the emitter's position to the origin of a <code>FlxObject</code>.
        /// </summary>
        /// <param name="Object">The <code>FlxObject</code> that needs to emit particles.</param>
		public void at(FlxObject Object)
		{
            //x = Object.x + Object.origin.X;
            //y = Object.y + Object.origin.Y;

            //x = Object.x + ((FlxSprite)(Object)).offset.X;
            //y = Object.y + ((FlxSprite)(Object)).offset.Y;

            x = Object.x + ((FlxSprite)(Object)).offset.X/2;
            y = Object.y + ((FlxSprite)(Object)).offset.Y / 2;
		}
		
		/// <summary>
        /// Call this function to turn off all the particles and the emitter.
		/// </summary>
        override public void kill()
		{
			base.kill();
			on = false;
		}

        /// <summary>
        /// Internal function that actually goes through and updates all the group members.
        /// Overridden here to remove the position update code normally used by a FlxGroup.
        /// </summary>
        public void setScale(float Scale)
        {
            FlxSprite o;
            int i = 0;
            int l = members.Count;
            while (i < l)
            {
                o = members[i++] as FlxSprite;
                //if ((o != null) && o.exists && o.active)
                o.scale = Scale; 
            }
        }


    }
}
