using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
#if !WINDOWS_PHONE
//using Microsoft.Xna.Framework.GamerServices;
#endif
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace org.flixel
{
    /// <summary>
    /// Starts the game
    /// </summary>
    public class FlxFactory : Microsoft.Xna.Framework.Game
    {


        //primary display buffer constants
#if !WINDOWS_PHONE

        /// <summary>
        /// DO NOT CHANGE THESE VALUES!!
        /// your game should only be concerned with the
        /// resolution parameters used when you call
        /// initGame() in your FlxGame class.
        /// </summary>
        private int resX = 1280;
        
#if __ANDROID__

        resX = 1024;

#endif


        /// <summary>
        /// DO NOT CHANGE THESE VALUES!!
        /// your game should only be concerned with the
        /// resolution parameters used when you call
        /// initGame() in your FlxGame class.
        /// </summary>
        private int resY = 720;  
#else
        private int resX = 480; //DO NOT CHANGE THESE VALUES!!
        private int resY = 800;  //your game should only be concerned with the
                                 //resolution parameters used when you call
                                 //initGame() in your FlxGame class.
#endif

#if XBOX360
        private bool _fullScreen = true;
#else
        public bool _fullScreen = false;
#endif
        //graphics management
        public GraphicsDeviceManager _graphics;
        //other variables
        private FlxGame _flixelgame;

        //nothing much to see here, typical XNA initialization code
        public FlxFactory()
        {
            
            //set up the graphics device and the content manager
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";



            //we don't need no new-fangled pixel processing
            //in our retro engine!
            _graphics.PreferMultiSampling = false;
            //set preferred screen resolution. This is NOT
            //the same thing as the game's actual resolution.
            _graphics.PreferredBackBufferWidth = resX;
            _graphics.PreferredBackBufferHeight = resY;
            //make sure we're actually running fullscreen if
            //fullscreen preference is set.
            if (_fullScreen && _graphics.IsFullScreen == false)
            {
                _graphics.ToggleFullScreen();
            }
            _graphics.ApplyChanges();
			FlxG.resolutionWidth = 1024;
			FlxG.resolutionHeight = 720;
			FlxG.fullscreen = false;
			FlxG.zoom = 4;
			Console.WriteLine("!!!! ---- Running Game at Settings: {0}x{1} Fullscreen?:{2} \n Preferrred {3} {4}\nZoom:{5}\n\n", 
				FlxG.resolutionWidth, 
				FlxG.resolutionHeight, FlxG.fullscreen, 
				GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, 
				GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, 
				FlxG.zoom);

            FlxG.Game = this;
#if !WINDOWS_PHONE
            //Components.Add(new GamerServicesComponent(this));
#endif
        }
        /// <summary>
        /// load up the master class, and away we go!
        /// </summary>
        protected override void Initialize()
        {
            //load up the master class, and away we go!
            
            //_flixelgame = new FlxGame();
			#if __ANDROID__
			_flixelgame = new Loader_SuperLemonadeFactory.FlixelEntryPoint2(this);
			#endif

			#if __IOS__
			_flixelgame = new InsideKitty.FlixelEntryPoint(this);
			#endif

			#if WINDOWS
			_flixelgame = new XNAMode.FlixelEntryPoint(this);
			#endif

			#if OSX
			_flixelgame = new XNAMode.FlixelEntryPoint(this);
			#endif

            FlxG.bloom = new BloomPostprocess.BloomComponent(this);
            FlxG.pixel = new PixelPostprocess.PixelComponent(this);

            Components.Add(_flixelgame);
            Components.Add(FlxG.bloom);
            Components.Add(FlxG.pixel);

            base.Initialize();
        }

    }

    //#region Application entry point

    // Don't run on OSX.
	#if OSX

	#elif __IOS__

	#else
    static class Program
    {
        //application entry point
        static void Main(string[] args)
        {
            using (FlxFactory game = new FlxFactory())
            {
                game.Run();
            }
        }
    }
	#endif

    //#endregion
}
