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

using org.flixel;
using System.IO;

namespace Loader_SuperLemonadeFactory
{
    /// <summary>
    /// Starts the game
    /// </summary>
    public class FlxFactory : Microsoft.Xna.Framework.Game
    {
        //graphics management
        public GraphicsDeviceManager _graphics;
        //other variables
        private FlxGame _flixelgame;

        //nothing much to see here, typical XNA initialization code
        public FlxFactory()
        {
            //Read the GAMESETTINGS.txt file

            string gameSettings = File.ReadAllText("GAMESETTINGS.txt");
            string[] splitter = gameSettings.Split('\n');
            //Console.WriteLine(splitter[0]);

            FlxG.resolutionWidth = Convert.ToInt32(splitter[0].Substring(2));
            FlxG.resolutionHeight = Convert.ToInt32(splitter[1].Substring(2));
            if (splitter[2].Substring(11).StartsWith("1"))
                FlxG.fullscreen = true;
            FlxG.zoom = Convert.ToInt32(splitter[3].Substring(5));


            //set up the graphics device and the content manager
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (FlxG.fullscreen)
            {
                //resX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                //resY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
//                if (GraphicsAdapter.DefaultAdapter.IsWideScreen)
//                {
//                    //if user has it set to widescreen, let's make sure this
//                    //is ACTUALLY a widescreen resolution.
//                    if (((FlxG.resolutionWidth / 16) * 9) != FlxG.resolutionHeight)
//                    {
//                        FlxG.resolutionWidth = (FlxG.resolutionHeight / 9) * 16;
//                    }
//                }
            }

            //we don't need no new-fangled pixel processing
            //in our retro engine!
            _graphics.PreferMultiSampling = false;
            //set preferred screen resolution. This is NOT
            //the same thing as the game's actual resolution.
            _graphics.PreferredBackBufferWidth = FlxG.resolutionWidth;
            _graphics.PreferredBackBufferHeight = FlxG.resolutionHeight;
            //make sure we're actually running fullscreen if
            //fullscreen preference is set.
            if (FlxG.fullscreen && _graphics.IsFullScreen == false)
            {
                _graphics.ToggleFullScreen();
            }
            _graphics.ApplyChanges();

            Console.WriteLine("Running Game at Settings: {0}x{1} Fullscreen?:{2} // Preferrred {3} {4}", FlxG.resolutionWidth, FlxG.resolutionHeight, FlxG.fullscreen, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

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
            _flixelgame = new FlixelEntryPoint2(this);

            FlxG.bloom = new BloomPostprocess.BloomComponent(this);

            Components.Add(_flixelgame);
            Components.Add(FlxG.bloom);

            base.Initialize();
        }

    }

    #region Application entry point

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

    #endregion
}
