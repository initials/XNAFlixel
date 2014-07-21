using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{
    /// <summary>
    /// Contains all the logic for the developer console
    /// </summary>
	
    public class FlxConsole
    {
        private int MAX_CONSOLE_LINES = 256; // Not a const in X-flixel... we'll just base it on max lines visible on the console

        public bool visible = false;

        private Rectangle _srcRect = new Rectangle(1, 1, 1, 1);
        private Rectangle _consoleRect;
        private Rectangle _titleSafeRect;
        private Color _consoleColor;
        private FlxText _consoleText;
        private FlxSprite _consoleCheatActivated;
        private bool canTypeCheat = false;
        
        /// <summary>
        /// Text describing frames per second.
        /// </summary>
        private FlxText _consoleFPS;

        private int[] _FPS;
        private int _curFPS;
        private List<string> _consoleLines;
        private float _consoleY;
        private float _consoleYT;
        private bool _fpsUpdate;

        int width; // Viewport width
        int height; // Viewport height
        int dx; // 5% of width
        int dy; // 5% of height
        Color notActionSafeColor = new Color(255, 0, 0, 23); // Red, 50% opacity
        Color notTitleSafeColor = new Color(255, 255, 0, 8); // Yellow, 50% opacity

        public FlxRecord vcr;


        /// <summary>
        /// Allows a command to be typed and executed.
        /// </summary>
        private FlxText _consoleCommand;

        public Color color
        {
            get { return _consoleColor; }
        }

        public FlxConsole(int targetLeft, int targetWidth)
        {
            visible = false;

            _FPS = new int[8];

            _consoleRect = new Rectangle(FlxG.spriteBatch.GraphicsDevice.Viewport.X, 0,
                FlxG.spriteBatch.GraphicsDevice.Viewport.Width, 
                FlxG.spriteBatch.GraphicsDevice.Viewport.Height);

            width = FlxG.spriteBatch.GraphicsDevice.Viewport.Width;
            height = FlxG.spriteBatch.GraphicsDevice.Viewport.Height;
            dx = (int)(width * 0.05);
            dy = (int)(height * 0.05);

            _titleSafeRect = new Rectangle(dx * 2, dy * 2,
                width - 4 * dx,
                height - 4 * dy);



            _consoleColor = new Color(0, 0, 0, 0x7F);

            _consoleText = new FlxText(targetLeft+(dx*2), -800, targetWidth, "").setFormat(null, 1, Color.White, FlxJustification.Left, Color.White);
            _consoleText.height = FlxG.height; //FlxG.spriteBatch.GraphicsDevice.Viewport.Height;

            _consoleCommand = new FlxText(targetLeft + (dx * 2) + 50, -800, targetWidth, "").setFormat(null, 1, Color.HotPink, FlxJustification.Left, Color.White);
            _consoleCommand.text = "";

            _consoleCheatActivated = new FlxSprite(targetLeft + (dx * 2), -800, FlxG.Content.Load<Texture2D>("flixel/vcr/cheat_on"));
            _consoleCheatActivated.setScrollFactors(0, 0);

            _consoleFPS = new FlxText(targetLeft + targetWidth - (dx*3), -800, 30, "").setFormat(null, 2, Color.White, FlxJustification.Right, Color.White);

            _consoleLines = new List<string>();

            MAX_CONSOLE_LINES = (FlxG.spriteBatch.GraphicsDevice.Viewport.Height / (int)(_consoleText.font.MeasureString("Qq").Y)) - 1;

            vcr = new FlxRecord();





        }
	
        /// <summary>
        /// Log data to the developer console
        /// </summary>
        /// <param name="Data">The data (in string format) that you wanted to write to the console</param>
        public void log(string Data)
        {
            if (Data == null)
                Data = "ERROR: NULL GAME LOG MESSAGE";

            _consoleLines.Add(Data);
            if (_consoleLines.Count > MAX_CONSOLE_LINES)
            {
                _consoleLines.RemoveAt(0);
                string newText = "";
                for (int i = 0; i < _consoleLines.Count; i++)
                    newText += _consoleLines[i] + "\n";
                _consoleText.text = newText;
            }
            else
                _consoleText.text += (Data + "\n");
            //_consoleText.scrollV = _consoleText.height;
        }

        /// <summary>
        /// Shows/hides the console
        /// </summary>
        public void toggle()
        {
            if (_consoleYT == FlxG.spriteBatch.GraphicsDevice.Viewport.Height)
                _consoleYT = 0;
            else
            {
                _consoleYT = FlxG.spriteBatch.GraphicsDevice.Viewport.Height;
                visible = true;
            }
        }

        /// <summary>
        /// Updates and/or animates the dev console
        /// </summary>
	
        public void update()
        {
            _consoleCheatActivated.update();

            if (visible)
            {

                FlxG._game.Game.IsMouseVisible = true;

                if (FlxG.keys.justPressed(Keys.Tab)) canTypeCheat = !canTypeCheat;
                if (canTypeCheat) keyboardEntry();

                vcr.update();

//                _FPS[_curFPS] = (int)(1f / FlxG.elapsed);
//                if (++_curFPS >= _FPS.Length) _curFPS = 0;
//                _fpsUpdate = !_fpsUpdate;
//                if (_fpsUpdate)
//                {
//                    int fps = 0;
//                    for (int i = 0; i < _FPS.Length; i++)
//                        fps += _FPS[i];
//                    _consoleFPS.text = ((int)Math.Floor((double)(fps / _FPS.Length))).ToString() + " fps";
//                }


				_consoleFPS.text = ((int)Math.Floor(1/ FlxG.elapsed)).ToString() + " fps";


                _consoleText.y = (-FlxG.spriteBatch.GraphicsDevice.Viewport.Height + _consoleRect.Height + 70);
                _consoleFPS.y = _consoleText.y;
                _consoleCommand.y = _consoleText.y - 16;
                _consoleCheatActivated.y = _consoleText.y - 32;

            }
            else
            {
                FlxG._game.Game.IsMouseVisible = false;
            }
            if (_consoleY < _consoleYT)
                _consoleY += FlxG.height * 10 * FlxG.elapsed;
            else if (_consoleY > _consoleYT)
                _consoleY -= FlxG.height * 10 * FlxG.elapsed;
            if (_consoleY > FlxG.spriteBatch.GraphicsDevice.Viewport.Height)
                _consoleY = FlxG.spriteBatch.GraphicsDevice.Viewport.Height;
            else if (_consoleY < 0)
            {
                _consoleY = 0;
                visible = false;
            }
            _consoleRect.Height = (int)Math.Floor(_consoleY);
        }

        /// <summary>
        /// Render
        /// </summary>
        /// <param name="spriteBatch">sb</param>
        public void render(SpriteBatch spriteBatch)
        {
            _FPS[_curFPS] = (int)(1f / FlxG.elapsed);
            if (++_curFPS >= _FPS.Length) _curFPS = 0;
            _fpsUpdate = !_fpsUpdate;
            if (_fpsUpdate)
            {
                int fps = 0;
                for (int i = 0; i < _FPS.Length; i++)
                    fps += _FPS[i];
                _consoleFPS.text = ((int)Math.Floor((double)(fps / _FPS.Length))).ToString() + " fps";
            }

            spriteBatch.Draw(FlxG.XnaSheet, _consoleRect,
                _srcRect, _consoleColor);

            spriteBatch.Draw(FlxG.XnaSheet, _titleSafeRect,
                _srcRect, notTitleSafeColor);

            

            _consoleText.render(spriteBatch);
            _consoleFPS.render(spriteBatch);
            _consoleCommand.render(spriteBatch);
            vcr.render(spriteBatch);
            if (canTypeCheat) _consoleCheatActivated.render(spriteBatch);



        }

        /// <summary>
        /// Keyboard Entry allows for a command to be written and run when the console is active.
        /// </summary>
        public void keyboardEntry()
        {
            PlayerIndex pi;
            bool shift = FlxG.keys.SHIFT;

            if (FlxG.keys.isNewKeyPress(Keys.A, null, out pi)) _consoleCommand.text += shift ? "A" : "a";
            if (FlxG.keys.isNewKeyPress(Keys.B, null, out pi)) _consoleCommand.text += shift ? "B" : "b";
            if (FlxG.keys.isNewKeyPress(Keys.C, null, out pi)) _consoleCommand.text += shift ? "C" : "c";
            if (FlxG.keys.isNewKeyPress(Keys.D, null, out pi)) _consoleCommand.text += shift ? "D" : "d";
            if (FlxG.keys.isNewKeyPress(Keys.E, null, out pi)) _consoleCommand.text += shift ? "E" : "e";
            if (FlxG.keys.isNewKeyPress(Keys.F, null, out pi)) _consoleCommand.text += shift ? "F" : "f";
            if (FlxG.keys.isNewKeyPress(Keys.G, null, out pi)) _consoleCommand.text += shift ? "G" : "g";
            if (FlxG.keys.isNewKeyPress(Keys.H, null, out pi)) _consoleCommand.text += shift ? "H" : "h";
            if (FlxG.keys.isNewKeyPress(Keys.I, null, out pi)) _consoleCommand.text += shift ? "I" : "i";
            if (FlxG.keys.isNewKeyPress(Keys.J, null, out pi)) _consoleCommand.text += shift ? "J" : "j";
            if (FlxG.keys.isNewKeyPress(Keys.K, null, out pi)) _consoleCommand.text += shift ? "K" : "k";
            if (FlxG.keys.isNewKeyPress(Keys.L, null, out pi)) _consoleCommand.text += shift ? "L" : "l";
            if (FlxG.keys.isNewKeyPress(Keys.M, null, out pi)) _consoleCommand.text += shift ? "M" : "m";
            if (FlxG.keys.isNewKeyPress(Keys.N, null, out pi)) _consoleCommand.text += shift ? "N" : "n";
            if (FlxG.keys.isNewKeyPress(Keys.O, null, out pi)) _consoleCommand.text += shift ? "O" : "o";
            if (FlxG.keys.isNewKeyPress(Keys.P, null, out pi)) _consoleCommand.text += shift ? "P" : "p";
            if (FlxG.keys.isNewKeyPress(Keys.Q, null, out pi)) _consoleCommand.text += shift ? "Q" : "q";
            if (FlxG.keys.isNewKeyPress(Keys.R, null, out pi)) _consoleCommand.text += shift ? "R" : "r";
            if (FlxG.keys.isNewKeyPress(Keys.S, null, out pi)) _consoleCommand.text += shift ? "S" : "s";
            if (FlxG.keys.isNewKeyPress(Keys.T, null, out pi)) _consoleCommand.text += shift ? "T" : "t";
            if (FlxG.keys.isNewKeyPress(Keys.U, null, out pi)) _consoleCommand.text += shift ? "U" : "u";
            if (FlxG.keys.isNewKeyPress(Keys.V, null, out pi)) _consoleCommand.text += shift ? "V" : "v";
            if (FlxG.keys.isNewKeyPress(Keys.W, null, out pi)) _consoleCommand.text += shift ? "W" : "w";
            if (FlxG.keys.isNewKeyPress(Keys.X, null, out pi)) _consoleCommand.text += shift ? "X" : "x";
            if (FlxG.keys.isNewKeyPress(Keys.Y, null, out pi)) _consoleCommand.text += shift ? "Y" : "y";
            if (FlxG.keys.isNewKeyPress(Keys.Z, null, out pi)) _consoleCommand.text += shift ? "Z" : "z";

            if (FlxG.keys.isNewKeyPress(Keys.D1, null, out pi)) _consoleCommand.text += shift ? "!" : "1";
            if (FlxG.keys.isNewKeyPress(Keys.D2, null, out pi)) _consoleCommand.text += shift ? "@" : "2";
            if (FlxG.keys.isNewKeyPress(Keys.D3, null, out pi)) _consoleCommand.text += shift ? "#" : "3";
            if (FlxG.keys.isNewKeyPress(Keys.D4, null, out pi)) _consoleCommand.text += shift ? "$" : "4";
            if (FlxG.keys.isNewKeyPress(Keys.D5, null, out pi)) _consoleCommand.text += shift ? "%" : "5";
            if (FlxG.keys.isNewKeyPress(Keys.D6, null, out pi)) _consoleCommand.text += shift ? "^" : "6";
            if (FlxG.keys.isNewKeyPress(Keys.D7, null, out pi)) _consoleCommand.text += shift ? "&" : "7";
            if (FlxG.keys.isNewKeyPress(Keys.D8, null, out pi)) _consoleCommand.text += shift ? "*" : "8";
            if (FlxG.keys.isNewKeyPress(Keys.D9, null, out pi)) _consoleCommand.text += shift ? "(" : "9";
            if (FlxG.keys.isNewKeyPress(Keys.D0, null, out pi)) _consoleCommand.text += shift ? ")" : "0";
            //if (FlxG.keys.isNewKeyPress(Keys.OemMinus, null, out pi)) _consoleCommand.text += shift ? "_" : "-";

            if (FlxG.keys.isNewKeyPress(Keys.Back, null, out pi))
            {

                if (_consoleCommand.text.Length <= 0)
                {
                    return;
                }

                string backspaced = _consoleCommand.text;

                _consoleCommand.text = backspaced.Remove(backspaced.Length - 1);
            }

            if (FlxG.keys.isNewKeyPress(Keys.Enter, null, out pi))
            {

                // run command(_consoleCommand.text);
                FlxGlobal.runCheat(_consoleCommand.text);


                _consoleCommand.text = "";

                canTypeCheat = false;
            }
        }



    }
}
