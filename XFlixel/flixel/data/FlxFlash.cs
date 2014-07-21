using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace org.flixel
{
    /// <summary>
    /// Special effects class.
    /// </summary>
    public class FlxFlash : FlxSprite
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
        public FlxFlash()
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
        /// <param name="Duration">How long it takes for the flash to fade</param>
        public void start(Color Color, float Duration)
        {
            start(Color, Duration, null, false);
        }

        /// <summary>
        /// Reset and trigger this special effect
        /// </summary>
        /// <param name="Color">The color you want to use</param>
        /// <param name="Duration">How long it takes for the flash to fade</param>
        /// <param name="FlashComplete">A function you want to run when the flash finishes</param>
        /// <param name="Force">Force the effect to reset</param>
        public void start(Color Color, float Duration, EventHandler<FlxEffectCompletedEvent> FlashComplete, bool Force)
		{
			if(!Force && exists) return;
            color = Color;
			_delay = Duration;
			_complete = FlashComplete;
			alpha = 1;
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
			alpha -= FlxG.elapsed/_delay;
			if(alpha <= 0)
			{
				exists = false;
				if(_complete != null)
					_complete(this, new FlxEffectCompletedEvent(EffectType.Flash));
			}
		}

    }
}
