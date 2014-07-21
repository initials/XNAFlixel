using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{
    public class FlxCamera
    {

        /// <summary>
        /// Camera "follow" style preset: camera has no deadzone, just tracks the focus object directly.
        /// </summary>
		public const uint STYLE_LOCKON = 0;
		/// <summary>
		/// /Camera "follow" style preset: camera deadzone is narrow but tall.
		/// </summary>
		public const uint STYLE_PLATFORMER = 1;
		/// <summary>
		/// Camera "follow" style preset: camera deadzone is a medium-size square around the focus object.
		/// </summary>
		public const uint STYLE_TOPDOWN = 2;
		/// <summary>
		/// Camera "follow" style preset: camera deadzone is a small square around the focus object.
		/// </summary>
		public const uint STYLE_TOPDOWN_TIGHT = 3;
		
		/// <summary>
		/// Camera "shake" effect preset: shake camera on both the X and Y axes.
		/// </summary>
		public const uint SHAKE_BOTH_AXES = 0;
		/// <summary>
		/// Camera "shake" effect preset: shake camera on the X axis only.
		/// </summary>
		public const uint SHAKE_HORIZONTAL_ONLY = 1;
		/// <summary>
		/// Camera "shake" effect preset: shake camera on the Y axis only.
		/// </summary>
		public const uint SHAKE_VERTICAL_ONLY = 2;
		
        /// <summary>
        /// While you can alter the zoom of each camera after the fact, this variable determines what value the camera will start at when created.
        /// </summary>
		public float defaultZoom;
		
		/// <summary>
		/// The X position of this camera's display.  Zoom does NOT affect this number. Measured in pixels from the left side of the flash window.
		/// </summary>
		public float x;
		/// <summary>
		/// The Y position of this camera's display.  Zoom does NOT affect this number. Measured in pixels from the top of the flash window.
		/// </summary>
		public float y;
		/// <summary>
		/// How wide the camera display is, in game pixels.
		/// </summary>
		public int width;
		/// <summary>
		/// How tall the camera display is, in game pixels.
		/// </summary>
		public int height;
		/// <summary>
        /// Tells the camera to follow this FlxObject object around.
		/// </summary>
		public FlxObject target;

        public Color color;

        /// <summary>
        /// The edges of the camera's range, i.e. where to stop scrolling.
        /// Measured in game pixels and world coordinates.
        /// </summary>
		public Rectangle bounds;
		
		/// <summary>
        /// Stores the basic parallax scrolling values.
		/// </summary>
		public Vector2 scroll;

        /// <summary>
        /// Tells the camera to follow this <code>FlxCore</code> object around.
        /// </summary>
        static public FlxObject followTarget;
        /// <summary>
        /// Used to force the camera to look ahead of the <code>followTarget</code>.
        /// </summary>
        static public Vector2 followLead;
        /// <summary>
        /// Used to smoothly track the camera as it follows.
        /// </summary>
        static public float followLerp;
        /// <summary>
        /// Stores the top and left edges of the camera area.
        /// </summary>
        static public Point followMin;
        /// <summary>
        /// Stores the bottom and right edges of the camera area.
        /// </summary>
        static public Point followMax;
        /// <summary>
        /// Internal, used to assist camera and scrolling.
        /// </summary>
        static protected Vector2 _scrollTarget;

        /// <summary>
        /// The natural background color of the camera. Defaults to FlxG.bgColor.
        /// NOTE: can be transparent for crazy FX!
        /// </summary>
		public Color bgColor;

        /// <summary>
        /// Will rotate the screen.
        /// </summary>
        public float angle;

        private Point _quakeOffset = Point.Zero;
        
        /// <summary>
        /// Sometimes it's easier to just work with a <code>FlxSprite</code> than it is to work
        /// directly with the <code>BitmapData</code> buffer.  This sprite reference will
        /// allow you to do exactly that.
        /// </summary>
        public FlxSprite screen;

        /// <summary>
        /// Instantiates a new camera at the specified location, with the specified size and zoom level.
        /// </summary>
        /// <param name="X">X location of the camera's display in pixels. Uses native, 1:1 resolution, ignores zoom.</param>
        /// <param name="Y">Y location of the camera's display in pixels. Uses native, 1:1 resolution, ignores zoom.</param>
        /// <param name="Width">The width of the camera display in pixels.</param>
        /// <param name="Height">The height of the camera display in pixels.</param>
        /// <param name="Zoom">The initial zoom level of the camera.  A zoom level of 2 will make all pixels display at 2x resolution.</param>
        public FlxCamera(int X, int Y, int Width, int Height, float Zoom)
        {
            x = X;
            y = Y;
            width = Width;
            height = Height;
            target = null;
            scroll = new Vector2();
            bounds = new Rectangle();
            screen = new FlxSprite();
            screen.createGraphic(0, 0, Color.Black);
            bgColor = FlxG.backColor;
            angle = 0;
            color = Color.White;
        }


    }
}
