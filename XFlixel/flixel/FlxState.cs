using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /// <summary>
    /// This is the basic game "state" object - e.g. in a simple game
    /// you might have a menu state and a play state.
    /// It acts as a kind of container for all your game objects.
    /// You can also access the game's background color
    /// and screen buffer through this object.
    /// FlxState is kind of a funny class from the technical side,
    /// it is just a regular Flash Sprite display object,
    /// with one member variable: a flixel <code>FlxGroup</code>.
    /// This means you can load it up with regular Flash stuff
    /// or with flixel elements, whatever works!
    /// </summary>
    public abstract class FlxState
    {
		
        /// <summary>
        /// This static variable holds the screen buffer,
        /// so you can draw to it directly if you want.
        /// </summary>
        static public FlxSprite screen;

        /// <summary>
        /// This static variable indicates the "clear color"
        /// or default background color of the game.
        /// Change it at ANY time using <code>FlxState.bgColor</code>.
        /// </summary>
		static public Color bgColor;

		/// <summary>
        /// Internal group used to organize and display objects you add to this state.
		/// </summary>
        public FlxGroup defaultGroup;

        /// <summary>
        /// Keeps track of how much time is elapsed in this state only.
        /// </summary>
        public float elapsedInState;

        /// <summary>
        /// Creates a new <code>FlxState</code> object,
        /// instantiating <code>screen</code> if necessary.
        /// </summary>
        public FlxState()
        {
            defaultGroup = new FlxGroup();
        }

        /// <summary>
        /// Override this function to set up your game state.
        /// This is where you create your groups and game objects and all that good stuff.
        /// </summary>
        virtual public void create()
		{
            bgColor = FlxG.backColor;
            //nothing to create initially

            elapsedInState = 0.0f;

            // Clear the array first, for each state you need to set cameras.
            FlxG.cameras.Clear();

		}

        /// <summary>
        /// Adds a new FlxCore subclass (FlxSprite, FlxBlock, etc) to the game loop
        /// </summary>
        /// <param name="Core">The object you want to add to the game loop</param>
        /// <returns></returns>
        virtual public FlxObject add(FlxObject Core)
		{
			return defaultGroup.add(Core);
		}

        /// <summary>
        /// Override this function to do special pre-processing FX like motion blur.
        /// You can use scaling or blending modes or whatever you want against
        /// <code>FlxState.screen</code> to achieve all sorts of cool FX.
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        virtual public void preProcess(SpriteBatch spriteBatch)
		{
            spriteBatch.GraphicsDevice.Clear(bgColor); //Default behavior - just overwrite buffer with background color

            // Bloom effect
            FlxG.bloom.BeginDraw();
		}

        /// <summary>
        /// Automatically goes through and calls update on everything you added to the game loop,
        /// override this function to handle custom input and perform collisions/
        /// </summary>
        virtual public void update()
        {
            // Update all time-related stuff.
            defaultGroup.update();

            elapsedInState += FlxG.elapsed;
        }

        /// <summary>
        /// This function collides <code>defaultGroup</code> against <code>defaultGroup</code>
        /// (basically everything you added to this state).
        /// </summary>
        virtual public void collide()
		{
			FlxU.collide(defaultGroup,defaultGroup);
		}

        /// <summary>
        /// Automatically goes through and calls render on everything you added to the game loop,
        /// override this loop to manually control the rendering process.
        /// </summary>
        /// <param name="spriteBatch"></param>
        virtual public void render(SpriteBatch spriteBatch)
        {
            // Render everything that should display on the screen.

            //spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            //spriteBatch.GraphicsDevice.SamplerStates[0].Filter = TextureFilter.Point;
            defaultGroup.render(spriteBatch);
            spriteBatch.End();

        }

        /// <summary>
        /// Override this function to do special pre-processing FX like light bloom.
        /// You can use scaling or blending modes or whatever you want against
        /// <code>FlxState.screen</code> to achieve all sorts of cool FX.
        /// </summary>
        /// <param name="spriteBatch"></param>
        virtual public void postProcess(SpriteBatch spriteBatch)
		{
			//no fx by default
            
            
            
            
            

		}

        /// <summary>
        /// Override this function to handle any deleting or "shutdown" type operations you
        /// might need (such as removing traditional Flash children like Sprite objects).
        /// </summary>
        virtual public void destroy()
        {
            defaultGroup.destroy();
        }

    }
}
