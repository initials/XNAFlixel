using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace org.flixel
{
    public class FlxSound : FlxObject
    {
		/// <summary>
        /// Whether or not this sound should be automatically destroyed when you switch states.
		/// </summary>
		public bool survive;
		/// <summary>
        /// Whether the sound is currently playing or not.
		/// </summary>
		public bool playing;

		protected bool _init;
        protected SoundEffectInstance _sound;
		protected float _position;
        protected float _volume;
		protected float _volumeAdjust;
        protected bool _looped
        {
            get { if (_sound == null) return false; return _sound.IsLooped; }
            set { if (_sound != null) _sound.IsLooped = value; }
        }
		protected FlxObject _core;
		protected float _radius;
		protected bool _pan;
		protected float _fadeOutTimer;
		protected float _fadeOutTotal;
		protected bool _pauseOnFadeOut;
		protected float _fadeInTimer;
		protected float _fadeInTotal;
		protected Vector2 _point2;

        public FlxSound()
        {
            @fixed = true; //no movement usually
        }

		/**
		 * An internal function for clearing all the variables used by sounds.
		 */
        protected void init()
		{
			_sound = null;
			_position = 0;
			_volume = 1.0f;
			_volumeAdjust = 1.0f;
			_looped = false;
			_core = null;
			_radius = 0;
			_pan = false;
			_fadeOutTimer = 0;
			_fadeOutTotal = 0;
			_pauseOnFadeOut = false;
			_fadeInTimer = 0;
			_fadeInTotal = 0;
			active = false;
			visible = false;
			solid = false;
			playing = false;
		}

        /// <summary>
        /// One of two main setup functions for sounds, this function loads a sound from an embedded MP3.
        /// </summary>
        /// <param name="EmbeddedSound">An embedded Class object representing an MP3 file.</param>
        /// <param name="Looped">Whether or not this sound should loop endlessly.</param>
        /// <returns>This <code>FlxSound</code> instance (nice for chaining stuff together, if you're into that).</returns>
		public FlxSound loadEmbedded(string EmbeddedSound, bool Looped)
		{
			stop();
			init();
            _sound = FlxSoundManager.getSound(EmbeddedSound);
			//NOTE: can't pull ID3 info from embedded sound currently
			_looped = Looped;
			updateTransform();
			active = true;
			return this;
		}

        /// <summary>
        /// Call this function if you want this sound's volume to change
        /// based on distance from a particular FlxCore object.
        /// </summary>
        /// <param name="X">The X position of the sound.</param>
        /// <param name="Y">The Y position of the sound.</param>
        /// <param name="Core">The object you want to track.</param>
        /// <param name="Radius">The maximum distance this sound can travel.</param>
        /// <returns>This FlxSound instance (nice for chaining stuff together, if you're into that).</returns>
        public FlxSound proximity(float X, float Y, FlxObject Core, float Radius)
        {
            return proximity(X,Y,Core,Radius,true);
        }
        /// <summary>
        /// Call this function if you want this sound's volume to change
        /// based on distance from a particular FlxCore object.
        /// </summary>
        /// <param name="X">The X position of the sound.</param>
        /// <param name="Y">The Y position of the sound.</param>
        /// <param name="Core">The object you want to track.</param>
        /// <param name="Radius">The maximum distance this sound can travel.</param>
        /// <param name="Pan">Pan as a bool ???</param>
        /// <returns>This FlxSound instance (nice for chaining stuff together, if you're into that).</returns>
        public FlxSound proximity(float X, float Y, FlxObject Core, float Radius, bool Pan)
        {
            x = X;
			y = Y;
			_core = Core;
			_radius = Radius;
			_pan = Pan;
			return this;
		}

		/// <summary>
        /// Call this function to play the sound.
		/// </summary>
        public void play()
        {
            if (_sound == null) return;
            _sound.Play();
            playing = true;
            active = true;
            _position = 0;
        }

		/// <summary>
        /// Call this function to pause this sound.
		/// </summary>
        public void pause()
		{
            if (_sound == null) return;
            _sound.Pause();
            playing = false;
		}

		/// <summary>
        /// Call this function to stop this sound.
		/// </summary>
        public void stop()
		{
			_position = 0;
            playing = false;
            active = false;
            if (_sound == null) return;
            _sound.Stop();
		}

        /// <summary>
        /// Call this function to make this sound fade out over a certain time interval.
        /// </summary>
        /// <param name="Seconds">The amount of time the fade out operation should take.</param>
        public void fadeOut(float Seconds)
        {
            fadeOut(Seconds, false);
        }

        /// <summary>
        /// Call this function to make this sound fade out over a certain time interval.
        /// </summary>
        /// <param name="Seconds">The amount of time the fade out operation should take.</param>
        /// <param name="PauseInstead">Tells the sound to pause on fadeout, instead of stopping.</param>
		public void fadeOut(float Seconds, bool PauseInstead)
		{
			_pauseOnFadeOut = PauseInstead;
			_fadeInTimer = 0;
			_fadeOutTimer = Seconds;
			_fadeOutTotal = _fadeOutTimer;
		}

        /// <summary>
        /// Call this function to make a sound fade in over a certain
        /// time interval (calls <code>play()</code> automatically).
        /// </summary>
        /// <param name="Seconds">The amount of time the fade-in operation should take.</param>
		public void fadeIn(float Seconds)
		{
			_fadeOutTimer = 0;
			_fadeInTimer = Seconds;
			_fadeInTotal = _fadeInTimer;
			play();
		}

        /// <summary>
        /// Set <code>volume</code> to a value between 0 and 1 to change how loud this sound is.
        /// </summary>
        public float volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                if (_volume < 0)
                    _volume = 0;
                else if (_volume > 1)
                    _volume = 1;
                updateTransform();
            }
        }

        /// <summary>
        /// Internal function that performs the actual logical updates to the sound object.
        /// Doesn't do much except optional proximity and fade calculations.
        /// </summary>
		protected void updateSound()
		{
			if(_position != 0)
				return;
			
			float radial = 1.0f;
			float fade = 1.0f;

            if (_sound.State != SoundState.Playing)
            {
                active = false;
            }

			//Distance-based volume control
			if(_core != null)
			{
				Vector2 _point = new Vector2();
				Vector2 _point2 = new Vector2();
                _point = _core.getScreenXY();
                _point2 = getScreenXY();
				float dx = _point.X - _point2.X;
				float dy = _point.Y - _point2.Y;
				radial = (float)(_radius - Math.Sqrt(dx*dx + dy*dy))/_radius;
				if(radial < 0) radial = 0;
				if(radial > 1) radial = 1;
				
				if(_pan)
				{
					float d = -dx/_radius;
					if(d < -1) d = -1;
					else if(d > 1) d = 1;
                    if (_sound != null)
                    {
                        _sound.Pan = d;
                    }
				}
			}
			
			//Cross-fading volume control
			if(_fadeOutTimer > 0)
			{
				_fadeOutTimer -= FlxG.elapsed;
				if(_fadeOutTimer <= 0)
				{
					if(_pauseOnFadeOut)
						pause();
					else
						stop();
				}
				fade = _fadeOutTimer/_fadeOutTotal;
				if(fade < 0) fade = 0;
			}
			else if(_fadeInTimer > 0)
			{
				_fadeInTimer -= FlxG.elapsed;
				fade = _fadeInTimer/_fadeInTotal;
				if(fade < 0) fade = 0;
				fade = 1 - fade;
			}
			
			_volumeAdjust = radial*fade;
			updateTransform();
		}

		/// <summary>
        /// The basic game loop update function.  Just calls <code>updateSound()</code>.
		/// </summary>
        override public void update()
		{
			base.update();
			updateSound();
		}

		/// <summary>
        /// The basic class destructor, stops the music and removes any leftover events.
		/// </summary>
        override public void destroy()
		{
			if(active)
				stop();
		}

		/// <summary>
        /// An internal function used to help organize and change the volume of the sound.
		/// </summary>
        internal void updateTransform()
		{
            if (_sound == null)
                return;
            _sound.Volume = FlxG.getMuteValue() * FlxG.volume * _volume * _volumeAdjust;
		}
    }

    /// <summary>
    /// Flixels Sound Manager.
    /// </summary>
    public static class FlxSoundManager
    {
        /// <summary>
        /// Sound List.
        /// </summary>
        private static List<SoundEffect> _sounds = new List<SoundEffect>();

        /// <summary>
        /// Sound Effect Instance.
        /// </summary>
        /// <param name="EmbeddedSound"></param>
        /// <returns></returns>
        public static SoundEffectInstance getSound(string EmbeddedSound)
        {
            //if (!System.IO.File.Exists(EmbeddedSound)) return;

            for (int i = 0; i < _sounds.Count; i++)
            {
                if (_sounds[i].Name.CompareTo(EmbeddedSound) == 0)
                {
                    return _sounds[i].CreateInstance();
                }
            }
            _sounds.Add(FlxG.Content.Load<SoundEffect>(EmbeddedSound));
            _sounds[_sounds.Count - 1].Name = EmbeddedSound;
            return _sounds[_sounds.Count - 1].CreateInstance();
        }
    }
}
