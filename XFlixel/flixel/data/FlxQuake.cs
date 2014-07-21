using System;
using System.Collections.Generic;

namespace org.flixel
{
    public class FlxQuake
    {
		/// <summary>
        /// The game's level of zoom.
		/// </summary>
		protected int _zoom;
		/// <summary>
        /// The intensity of the quake effect: a percentage of the screen's size.
		/// </summary>
		protected float _intensity;
		/// <summary>
        /// Set to countdown the quake time.
		/// </summary>
		protected float _timer;

		/// <summary>
        /// The amount of X distortion to apply to the screen.
		/// </summary>
		public int x = 0;
		/// <summary>
        /// The amount of Y distortion to apply to the screen.
		/// </summary>
		public int y = 0;

		/// <summary>
        /// Constructor.
		/// </summary>
		/// <param name="Zoom"></param>
		public FlxQuake(int Zoom)
		{
			_zoom = Zoom;
			start(0, 0);
		}

        /// <summary>
        /// Reset and trigger this special effect.
        /// </summary>
        /// <param name="Intensity">Percentage of screen size representing the maximum distance that the screen can move during the 'quake'.</param>
        /// <param name="Duration">The length in seconds that the "quake" should last.</param>
		public void start(float Intensity, float Duration)
		{
			stop();
			_intensity = Intensity;
			_timer = Duration;
		}

		/// <summary>
        /// Stops this screen effect.
		/// </summary>
        public void stop()
		{
			x = 0;
			y = 0;
			_intensity = 0;
			_timer = 0;
		}

		/// <summary>
        /// Updates and/or animates this special effect.
		/// </summary>
        public void update()
		{
			if(_timer > 0)
			{
				_timer -= FlxG.elapsed;
				if(_timer <= 0)
				{
					_timer = 0;
					x = 0;
					y = 0;
				}
				else
				{
					x = (int)(FlxU.random()*_intensity*FlxG.width*2-_intensity*FlxG.width)*_zoom;
                    y = (int)(FlxU.random() * _intensity * FlxG.height * 2 - _intensity * FlxG.height) * _zoom;
				}
			}
		}

    }
}
