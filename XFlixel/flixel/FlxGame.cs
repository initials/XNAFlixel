using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

// ***
using BloomPostprocess;


namespace org.flixel
{
    /// <summary>
    /// FlxGame, the heart of the Flixel Game.
    /// </summary>
    public partial class FlxGame : DrawableGameComponent
    {
        // ***
        public BloomComponent bloom;

        private FlxState _firstScreen;

        private SoundEffect _sndBeep;

        /// <summary>
        /// basic display stuff
        /// </summary>
        internal FlxState _state;

        /// <summary>
        /// The main backRender renderTarget.
        /// </summary>
        private RenderTarget2D backRender;


        internal int targetWidth = 0;
        internal int targetLeft = 0;

		/// <summary>
        /// basic update stuff
		/// </summary>
		private bool _paused;

        /// <summary>
        /// Pause screen, sound tray, support panel, dev console, and special effects objects
        /// </summary>
        internal FlxPause _pausePanel;
        internal bool _soundTrayVisible;
        internal Rectangle _soundTrayRect;
        internal float _soundTrayTimer;
        internal FlxSprite[] _soundTrayBars;
        internal FlxText _soundCaption;
        internal FlxConsole _console;

        /// <summary>
        /// A hud for having non scaled text. Important for nice clean text if you're working at 2x or 3x
        /// </summary>
        public FlxHud hud;


		/// <summary>
        /// pause stuff
		/// </summary>
        private string[] _helpStrings;

        /// <summary>
        /// effect stuff
        /// </summary>
        private Point _quakeOffset = Point.Zero;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="game">Game</param>
        public FlxGame(Game game) : base(game) { }


        /// <summary>
        /// Tint the entire game.
        /// </summary>
        protected Color _color = Color.White;

        /// <summary>
        /// Used for color tinting.
        /// </summary>
        public Color color
        {
            get { return _color; }
            set { _color = new Color(value.R, value.G, value.B); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="GameSizeX">The width of your game in pixels (e.g. 320)</param>
        /// <param name="GameSizeY">The height of your game in pixels (e.g. 240)</param>
        /// <param name="InitialState">The class name of the state you want to create and switch to first (e.g. MenuState)</param>
        /// <param name="BGColor">The color of the app's background</param>
        /// <param name="showFlixelLogo">Whether or not to go to the FlxSplash screen first.</param>
        /// <param name="logoColor">The color of the great big 'f' in the flixel logo</param>
        public void initGame(int GameSizeX, int GameSizeY,
            FlxState InitialState, Color BGColor,
            bool showFlixelLogo, Color logoColor)
        {
            FlxG.backColor = BGColor;
            FlxG.setGameData(this, GameSizeX, GameSizeY);

            

            _color = Color.White;

            _paused = false;

            //activate the first screen.
            if (showFlixelLogo == false)
            {
                _firstScreen = InitialState;
            }
            else
            {
                FlxSplash.setSplashInfo(logoColor, InitialState);
                _firstScreen = new FlxSplash();
            }
        }

        /// <summary>
        /// Sets up the strings that are displayed on the left side of the pause game popup
        /// </summary>
        /// <param name="X">What to display next to the X button</param>
        /// <param name="C">What to display next to the C button</param>
        /// <param name="Mouse">What to display next to the mouse icon</param>
        /// <param name="Arrows">What to display next to the arrows icon</param>
		protected void help(string X, string C, string Mouse, string Arrows)
		{
            _helpStrings = new string[4];

			if(X != null)
				_helpStrings[0] = X;
			if(C != null)
				_helpStrings[1] = C;
			if(Mouse != null)
				_helpStrings[2] = Mouse;
			if(Arrows != null)
				_helpStrings[3] = Arrows;

            if (_pausePanel != null)
            {
                _pausePanel.helpX = _helpStrings[0];
                _pausePanel.helpC = _helpStrings[1];
                _pausePanel.helpMouse = _helpStrings[2];
                _pausePanel.helpArrows = _helpStrings[3];
            }
		}
        /// <summary>
        /// Initializes the backRender RenderTarget2D
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            backRender = new RenderTarget2D(GraphicsDevice, FlxG.width, FlxG.height, false, SurfaceFormat.Color,
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            
        }

        /// <summary>
        /// @benbaird initializes the console, the pause overlay, and the soundbar
        /// </summary>
        private void initConsole()
        {

            hud = new FlxHud(targetLeft, targetWidth);


            //initialize the debug console
            _console = new FlxConsole(targetLeft, targetWidth);

            _console.log(FlxG.LIBRARY_NAME +
                " v" + FlxG.LIBRARY_MAJOR_VERSION.ToString() + "." + FlxG.LIBRARY_MINOR_VERSION.ToString());
            _console.log("---------------------------------------");

            //Pause screen popup
            _pausePanel = new FlxPause();
            if (_helpStrings != null)
            {
                _pausePanel.helpX = _helpStrings[0];
                _pausePanel.helpC = _helpStrings[1];
                _pausePanel.helpMouse = _helpStrings[2];
                _pausePanel.helpArrows = _helpStrings[3];
            }

			//Sound Tray popup
			_soundTrayRect = new Rectangle((FlxG.width - 80) / 2, -30, 80, 30);
			_soundTrayVisible = false;
			
            _soundCaption = new FlxText((FlxG.width - 80) / 2, -10, 80, "VOLUME");
            _soundCaption.setFormat(null, 1, Color.White, FlxJustification.Center, Color.White).height = 10;

			int bx = 10;
			int by = 14;
            _soundTrayBars = new FlxSprite[10];
			for(int i = 0; i < 10; i++)
			{
                _soundTrayBars[i] = new FlxSprite(_soundTrayRect.X + (bx * 1), -i, null);
                _soundTrayBars[i].width = 4;
                _soundTrayBars[i].height = i + 1;
                _soundTrayBars[i].scrollFactor = Vector2.Zero;
				bx += 6;
				by--;
			}
        }

        /// <summary>
        /// Load Content 
        /// </summary>
        protected override void LoadContent()
        {
            //load up graphical content used for the flixel engine
            targetWidth = (int)(GraphicsDevice.Viewport.Height * ((float)FlxG.width / (float)FlxG.height));
            targetLeft = (GraphicsDevice.Viewport.Width - targetWidth) / 2;

            FlxG.LoadContent(GraphicsDevice);
			_sndBeep = FlxG.Content.Load<SoundEffect>("flixel/beep");

            initConsole();

            if (_firstScreen != null)
            {
                FlxG.state = _firstScreen;
                _firstScreen = null;
            }
        }

        /// <summary>
        /// Unload content.
        /// </summary>
        protected override void UnloadContent()
        {
            _sndBeep.Dispose();

            if (FlxG.state != null)
            {
                FlxG.state.destroy();
            }
        }
	
        /// <summary>
        /// Switch from one FlxState to another
        /// </summary>
        /// <param name="newscreen">The class name of the state you want (e.g. PlayState)</param>
        public void switchState(FlxState newscreen)
        {
            FlxG.unfollow();
            FlxG.keys.reset();
            FlxG.gamepads.reset();
            FlxG.mouse.reset();

            FlxG.flash.stop();
            FlxG.fade.stop();
            FlxG.quake.stop();

            if (_state != null)
            {
                _state.destroy();
            }
            _state = newscreen;
            _state.create();
        }

		/// <summary>
        /// Internal function to help with basic pause game functionality.
		/// </summary>
        internal void unpauseGame()
		{

            Console.WriteLine("Unpausing game from FlxGame");

			//if(!FlxG.panel.visible) flash.ui.Mouse.hide();
			FlxG.resetInput();
			_paused = false;
			//stage.frameRate = _framerate;
		}

		/// <summary>
        /// Internal function to help with basic pause game functionality.
		/// </summary>
        internal void pauseGame()
		{
            Console.WriteLine("Pausing game from FlxGame");

            //if((x != 0) || (y != 0))
            //{
            //    x = 0;
            //    y = 0;
            //}
            //flash.ui.Mouse.show();
			_paused = true;
			//stage.frameRate = _frameratePaused;
		}

        /// <summary>
        /// This is the main game loop
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public override void Update(GameTime gameTime)
        {
            PlayerIndex pi;

            //Frame timing
            FlxG.getTimer = (uint)gameTime.TotalGameTime.TotalMilliseconds;
            FlxG.elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FlxG.elapsedTotal += (float)gameTime.ElapsedGameTime.TotalSeconds;
            FlxG.elapsedAsGameTime = gameTime;
            if (FlxG.elapsed > FlxG.maxElapsed)
                FlxG.elapsed = FlxG.maxElapsed;
            FlxG.elapsed *= FlxG.timeScale;

            //Animate flixel HUD elements
            _console.update();

            hud.update();

			if(_soundTrayTimer > 0)
				_soundTrayTimer -= FlxG.elapsed;
			else if(_soundTrayRect.Y > -_soundTrayRect.Height)
			{
				_soundTrayRect.Y -= (int)(FlxG.elapsed * FlxG.height * 2);
                _soundCaption.y = (_soundTrayRect.Y + 4);
                for (int i = 0; i < _soundTrayBars.Length; i++)
                {
                    _soundTrayBars[i].y = (_soundTrayRect.Y + _soundTrayRect.Height - _soundTrayBars[i].height - 2);
                }
				if(_soundTrayRect.Y < -_soundTrayRect.Height)
					_soundTrayVisible = false;
			}

            //State updating
            FlxG.keys.update();
            FlxG.gamepads.update();
            FlxG.mouse.update();

            //set a helper for last control type used.
            updateLastUsedControl();


            FlxG.updateSounds();
            if (FlxG.keys.isNewKeyPress(Keys.OemPipe, null, out pi))
            {
                FlxG.mute = !FlxG.mute;
                showSoundTray();
            }
            else if (FlxG.keys.isNewKeyPress(Keys.OemOpenBrackets, null, out pi))
            {
                FlxG.mute = false;
                FlxG.volume -= 0.1f;
                showSoundTray();
            }
            else if (FlxG.keys.isNewKeyPress(Keys.OemCloseBrackets, null, out pi))
            {
                FlxG.mute = false;
                FlxG.volume += 0.1f;
                showSoundTray();
            }
            else if (FlxG.keys.isNewKeyPress(Keys.OemTilde, null, out pi) || FlxG.keys.isNewKeyPress(Keys.D0, null, out pi) ) //
            {
                //FlxG.keys.isNewKeyPress(Keys.D1, null, out pi) ||
                if (FlxG.debug == true)
                {
                    Console.WriteLine("TOGGLING THE CONSOLE");

                    _console.toggle();
                }
                
            }
            else if (FlxG.autoHandlePause && (FlxG.keys.isPauseGame(FlxG.controllingPlayer) || FlxG.gamepads.isPauseGame(FlxG.controllingPlayer)))
            {
                //Previously, causing a double up of pause.
                //FlxG.pause = !FlxG.pause;

                _paused = !_paused;

                if (_paused == true)
                {
                    FlxG.pauseSounds();
                }
                else if (_paused == false)
                {
                    FlxG.playSounds();
                }
            }




            if (_paused)
            {

                if (FlxG.gamepads.isNewButtonPress(Buttons.Y))
                {
                    _paused = !_paused;
                    FlxG.pauseAction = "Exit";
                }
                else
                {
                    FlxG.pauseAction = "";
                }

                return;
            }

            if (FlxG.state != null)
            {
                //Update the camera and game state
                FlxG.doFollow();
                FlxG.state.update();

                //Update the various special effects
                if (FlxG.flash.exists)
                    FlxG.flash.update();
                if (FlxG.fade.exists)
                    FlxG.fade.update();
                if (FlxG.transition.exists)
                    FlxG.transition.update();

                FlxG.quake.update();
                _quakeOffset.X = FlxG.quake.x;
                _quakeOffset.Y = FlxG.quake.y;
            }

            FlxGlobal.cheatString = "";
            FlxG.pauseAction = "";
        }


        /// <summary>
        /// The Main Rendering part.
        /// Uses two different SpriteBatches,
        /// To Do: Have Bloom effect both spritebatches.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Draw(GameTime gameTime)
        {
            //Render the screen to our internal game-sized back buffer.
            GraphicsDevice.SetRenderTarget(backRender);

            if (FlxG.state != null)
            {
                FlxG.state.preProcess(FlxG.spriteBatch);
                
                FlxG.state.render(FlxG.spriteBatch);
                
                FlxG.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                
                if (FlxG.flash.exists)
                    FlxG.flash.render(FlxG.spriteBatch);
                
                if (FlxG.fade.exists)
                    FlxG.fade.render(FlxG.spriteBatch);
                
                if (FlxG.mouse.cursor.visible)
                    FlxG.mouse.cursor.render(FlxG.spriteBatch);

                FlxG.spriteBatch.End();

                FlxG.state.postProcess(FlxG.spriteBatch);

            }
            //Render sound tray if necessary
            if (_soundTrayVisible || _paused)
            {
                FlxG.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                //GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
                //GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
                //GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;
                if (_soundTrayVisible)
                {
                    FlxG.spriteBatch.Draw(FlxG.XnaSheet, _soundTrayRect,
                        new Rectangle(1,1,1,1), _console.color);
                    _soundCaption.render(FlxG.spriteBatch);
                    for (int i = 0; i < _soundTrayBars.Length; i++)
                    {
                        _soundTrayBars[i].render(FlxG.spriteBatch);
                    }
                }
                if (_paused)
                {
                    _pausePanel.render(FlxG.spriteBatch);
                }
                FlxG.spriteBatch.End();
            }
            
            //FlxG.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            //var new_pos = new Vector2(50,50);
            //FlxG.spriteBatch.Draw(lightmask, new_pos, Color.White);
            //FlxG.spriteBatch.Draw(lightmask, new Vector2(10,100), Color.White);
            //FlxG.spriteBatch.End();
            
            GraphicsDevice.SetRenderTarget(null);

            //Copy the result to the screen, scaled to fit
            GraphicsDevice.Clear(FlxG.backColor);

            FlxG.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            //GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
            //GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
            //GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;

            // This was the original spritebatch.Draw
            //FlxG.spriteBatch.Draw(backRender,
            //    new Rectangle(targetLeft + _quakeOffset.X, _quakeOffset.Y, targetWidth, GraphicsDevice.Viewport.Height),
            //    Color.White);

            // If there are no cameras in the array, render the normal spriteBatch.
            if (FlxG.cameras.Count == 0)
            {
                // This is the new SpriteBatch.Draw the draws with the FlxG.angle
                FlxG.spriteBatch.Draw(backRender,
                    new Rectangle(targetLeft + _quakeOffset.X + FlxG.width, _quakeOffset.Y + FlxG.height, targetWidth, GraphicsDevice.Viewport.Height),
                    null,
                    _color,
                    FlxG.angle,
                    new Vector2(FlxG.width / FlxG.zoom, FlxG.height / FlxG.zoom),
                    SpriteEffects.None,
                    0f);
            }
            // if there are cameras in the FlxG.cameras array, render them here.
            else
            {
                foreach (FlxCamera cam in FlxG.cameras)
                {
                    FlxG.spriteBatch.Draw(backRender,
                    new Rectangle(
                        (int)cam.x + (targetLeft + _quakeOffset.X + cam.width), 
                        (int)cam.y + (_quakeOffset.Y + cam.height), 
                        targetWidth, 
                        GraphicsDevice.Viewport.Height),
                    null,
                    cam.color,
                    cam.angle,
                    new Vector2(FlxG.width / FlxG.zoom, FlxG.height / FlxG.zoom),
                    SpriteEffects.None,
                    0f);
                }

                
            }

            // Add cameras here.
            /*
            FlxG.spriteBatch.Draw(backRender,
                new Rectangle((targetLeft + _quakeOffset.X + FlxG.width)/2, (_quakeOffset.Y + FlxG.height)/2, (targetWidth)/2, (GraphicsDevice.Viewport.Height)/2),
                null,
                _color,
                FlxG.angle,
                new Vector2(FlxG.width / FlxG.zoom, FlxG.height / FlxG.zoom),
                SpriteEffects.None,
                0f);
            */



            //Render console if necessary
            if (_console.visible)
            {
                _console.render(FlxG.spriteBatch);
            }

            // Render hud if neccessary.
            if (hud.visible)
            {
                hud.render(FlxG.spriteBatch);
            }
            // Render transition if neccessary, and render on this layer above everything.
            if (FlxG.transition.exists)
                FlxG.transition.render(FlxG.spriteBatch);
            

            FlxG.spriteBatch.End();
        }


//        public void takeScreenshot()
//        {
//
//            int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
//            int h = GraphicsDevice.PresentationParameters.BackBufferHeight;
//
//            //pull the picture from the buffer 
//            int[] backBuffer = new int[w * h];
//            GraphicsDevice.GetBackBufferData(backBuffer);
//
//            //copy into a texture 
//            Texture2D texture = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
//            texture.SetData(backBuffer);
//
//            //save to disk 
//            Stream stream = File.OpenWrite("screenie" + ".jpg");
//
//            texture.SaveAsJpeg(stream, w, h);
//            stream.Dispose();
//
//            texture.Dispose();
//        }

		/// <summary>
		/// This function is only used by the FlxGame class to do important internal management stuff
		/// </summary>
		private void showSoundTray()
		{
            if (!FlxG.mute)
            {
                _sndBeep.Play(FlxG.volume, 0f, 0f);
            }
			_soundTrayTimer = 1;
			_soundTrayRect.Y = 0;
			_soundTrayVisible = true;

            _soundCaption.y = (_soundTrayRect.Y + 4);

			int gv = (int)Math.Round(FlxG.volume * 10);
			if(FlxG.mute)
				gv = 0;
			for (int i = 0; i < _soundTrayBars.Length; i++)
			{
                _soundTrayBars[i].y = (_soundTrayRect.Y + _soundTrayRect.Height - _soundTrayBars[i].height - 2);
				if(i < gv) _soundTrayBars[i].alpha = 1;
				else _soundTrayBars[i].alpha = 0.5f;
			}
		}

        private void updateLastUsedControl()
        {
            if (FlxG.keys.A||
                FlxG.keys.B||
                FlxG.keys.C||
                FlxG.keys.D||
                FlxG.keys.E||
                FlxG.keys.F||
                FlxG.keys.G||
                FlxG.keys.H||
                FlxG.keys.I||
                FlxG.keys.J||
                FlxG.keys.K||
                FlxG.keys.L||
                FlxG.keys.M||
                FlxG.keys.N||
                FlxG.keys.O||
                FlxG.keys.P||
                FlxG.keys.Q||
                FlxG.keys.R||
                FlxG.keys.S||
                FlxG.keys.T||
                FlxG.keys.U||
                FlxG.keys.V||
                FlxG.keys.W||
                FlxG.keys.X||
                FlxG.keys.Y||
                FlxG.keys.Z || FlxG.keys.SPACE || FlxG.keys.ENTER
                || FlxG.keys.LEFT || FlxG.keys.RIGHT || FlxG.keys.UP || FlxG.keys.DOWN)  
            {
                FlxG.lastControlTypeUsed = FlxG.CONTROL_TYPE_KEYBOARD;
            }
            else if (
                FlxG.gamepads.isButtonDown(Buttons.A) || 
                FlxG.gamepads.isButtonDown(Buttons.B) || 
                FlxG.gamepads.isButtonDown(Buttons.X) || 
                FlxG.gamepads.isButtonDown(Buttons.Y) || 
                FlxG.gamepads.isButtonDown(Buttons.LeftShoulder) ||
                FlxG.gamepads.isButtonDown(Buttons.RightShoulder) ||
                FlxG.gamepads.isButtonDown(Buttons.LeftTrigger) ||
                FlxG.gamepads.isButtonDown(Buttons.RightTrigger) ||
                FlxG.gamepads.isButtonDown(Buttons.DPadDown) ||
                FlxG.gamepads.isButtonDown(Buttons.DPadUp) ||
                FlxG.gamepads.isButtonDown(Buttons.DPadLeft) ||
                FlxG.gamepads.isButtonDown(Buttons.DPadRight) ||
                FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickDown) ||
                FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickUp) ||
                FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickLeft) ||
                FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickRight)
                )
            {
                FlxG.lastControlTypeUsed = FlxG.CONTROL_TYPE_GAMEPAD;
            }

        }
    }
}
