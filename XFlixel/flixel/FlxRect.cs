using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.flixel
{
    /// <summary>
    /// A simple rectangle helper class.
    /// </summary>
    public class FlxRect
    {
        /// <summary>
        /// X value.
        /// </summary>
        public float x;

        /// <summary>
        /// Y value.
        /// </summary>
        public float y;
        
        /// <summary>
        /// Width.
        /// </summary>
        public float width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }
        /// <summary>
        /// Height.
        /// </summary>
        public float height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        private float _width;
        private float _height;

        /// <summary>
        /// Creates a FlxRect.
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="Width">Width</param>
        /// <param name="Height">Height</param>
        public FlxRect(float X, float Y, float Width, float Height)
        {
            x = X;
            y = Y;
            _width = Width;
            _height = Height;
        }

        /// <summary>
        /// Creates a FlxRect of dimensions 0,0,0,0
        /// </summary>
        static public FlxRect Empty
        {
            get { return new FlxRect(0, 0, 0, 0); }
        }
    }
}
