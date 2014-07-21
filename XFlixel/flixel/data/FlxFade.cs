using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace org.flixel
{
    /// <summary>
    /// This is a special effects utility class to help FlxGame do the 'fade' effect.
    /// </summary>
    public class FlxFade : FlxSprite
    {
		/// <summary>
        /// How long the effect should last.
		/// </summary>
		protected float _delay;
		/// <summary>
        /// Callback for when the effect is finished.
		/// </summary>
		protected EventHandler<FlxEffectCompletedEvent> _complete;
		
		/// <summary>
        /// Constructor initializes the fade object
		/// </summary>
		public FlxFade()
            : base(0, 0)
		{
            createGraphic(FlxG.width, FlxG.height, Color.Black);
			scrollFactor.X = 0;
			scrollFactor.Y = 0;
			exists = false;
			solid = false;
			@fixed = true;
		}

        /// <summary>
        /// Reset and trigger this special effect
        /// </summary>
        /// <param name="Color">The color you want to use</param>
        public void start(Color Color)
        {
            start(Color, 1f, null, false);
        }
        /// <summary>
        /// Reset and trigger this special effect
        /// </summary>
        /// <param name="Color">The color you want to use</param>
        /// <param name="Duration">How long it should take to fade the screen out</param>
        public void start(Color Color, float Duration)
        {
            start(Color, Duration, null, false);
        }
        /// <summary>
        /// Reset and trigger this special effect
        /// </summary>
        /// <param name="Color">The color you want to use</param>
        /// <param name="Duration">How long it should take to fade the screen out</param>
        /// <param name="FadeComplete">A function you want to run when the fade finishes</param>
        /// <param name="Force">Force the effect to reset</param>
        public void start(Color Color, float Duration, EventHandler<FlxEffectCompletedEvent> FadeComplete, bool Force)
		{
			if(!Force && exists) return;
            color = Color;
			_delay = Duration;
			_complete = FadeComplete;
			alpha = 0;
			exists = true;
		}

		/// <summary>
        /// Stops and hides this screen effect.
		/// </summary>
        public void stop()
		{
			exists = false;
		}

		/// <summary>
        /// Updates and/or animates this special effect
		/// </summary>
		override public void update()
		{
			alpha += FlxG.elapsed/_delay;
			if(alpha >= 1)
			{
				alpha = 1;
				if(_complete != null)
					_complete(this, new FlxEffectCompletedEvent(EffectType.FadeOut));
			}
		}

    }
}
