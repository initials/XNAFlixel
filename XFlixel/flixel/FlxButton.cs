using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{
    public delegate void FlxButtonClick();

    /// <summary>
    /// A simple button class that calls a function when clicked by the mouse.
    /// Supports labels, highlight states, and parallax scrolling.
    /// 
    /// Supports controller buttons A B X Y
    /// </summary>
    public class FlxButton : FlxGroup
    {

        public const int ControlPadA = 0;
        public const int ControlPadB = 1;
        public const int ControlPadX = 2;
        public const int ControlPadY = 3;
        public const int ControlPadRStick = 4;
        public const int ControlPadLStick = 5;
        public const int ControlPadDPad = 6;
        public const int ControlPadRB = 7;
        public const int ControlPadLB = 8;
        public const int ControlPadRT = 9;
        public const int ControlPadLT = 10;
        public const int ControlPadBack = 11;
        public const int ControlPadStart = 12;



        /// <summary>
        /// Set this to true if you want this button to function even while the game is paused.
        /// </summary>
        public bool pauseProof;

        /// <summary>
        /// Used for checkbox-style behavior.
        /// </summary>
        protected bool _onToggle;

        /// <summary>
        /// Stores the 'off' or normal button state graphic.
        /// </summary>
        protected FlxSprite _off;
        /// <summary>
        /// Stores the 'on' or highlighted button state graphic.
        /// </summary>
        protected FlxSprite _on;
        /// <summary>
        /// Stores the 'off' or normal button state label.
        /// </summary>
        protected FlxText _offT;
        /// <summary>
        /// Stores the 'on' or highlighted button state label.
        /// </summary>
        protected FlxText _onT;

        /// <summary>
        /// holds the controller button graphic
        /// </summary>
        protected FlxSprite _controllerButton;

        /// <summary>
        /// Holds the controller button that can activate the button.
        /// </summary>
        protected int _controllerButtonIndex;
        /// <summary>
        /// This function is called when the button is clicked.
        /// </summary>
        protected FlxButtonClick _callback;
        /// <summary>
        /// Tracks whether or not the button is currently pressed.
        /// </summary>
        protected bool _pressed;
        /// <summary>
        /// Whether or not the button has initialized itself yet.
        /// </summary>
        protected bool _initialized;
        /// <summary>
        /// Helper variable for correcting its members' <code>scrollFactor</code> objects.
        /// </summary>
        protected Vector2 _sf;

        /// <summary>
        /// Counts the time the button has been alive for. Callbacks will only work after button has been alive for specified time, to avoid accidental clicks in new states.
        /// </summary>
        public float _counter;

        

        /// <summary>
        /// Creates a new <code>FlxButton</code> object with a gray background
        /// and a callback function on the UI thread.
        /// </summary>
        /// <param name="X">The X position of the button.</param>
        /// <param name="Y">The Y position of the button.</param>
        /// <param name="Callback">The function to call whenever the button is clicked.</param>
        public FlxButton(int X, int Y, FlxButtonClick Callback)
            : base()
        {
            x = X;
            y = Y;
            width = 100;
            height = 20;
            _off = new FlxSprite().createGraphic((int)width, (int)height, new Color(0x7f, 0x7f, 0x7f));
            _off.solid = false;
            add(_off, true);
            _on = new FlxSprite().createGraphic((int)width, (int)height, Color.White);
            _on.solid = false;
            add(_on, true);
            _offT = null;
            _onT = null;
            _callback = Callback;
            _onToggle = false;
            _pressed = false;
            _initialized = false;
            _sf = Vector2.Zero;
            pauseProof = false;

            _controllerButtonIndex = -1;
            //_controllerButton = new FlxSprite((int)width+5,0);
            //_controllerButton.loadGraphic(FlxG.Content.Load<Texture2D>("buttons/BP3_SSTRIP_32"),true,false,32,32);
            //_controllerButton.solid = false;
            ////_controllerButton.scale
            //add(_controllerButton, true);



        }

        /// <summary>
        /// Creates a new <code>FlxButton</code> object with a gray background
        /// and a callback function on the UI thread.
        /// Also appends a GamePad symbol to allow for using a gamepad to select the menu.
        /// </summary>
        /// <param name="X">The X position of the button.</param>
        /// <param name="Y">The Y position of the button.</param>
        /// <param name="Callback">The function to call whenever the button is clicked.</param>
        /// <param name="Button">Button number. Uses FlxButton.ControlPad** to select a button</param>
        public FlxButton(int X, int Y, FlxButtonClick Callback, int Button)
            : base()
        {
            x = X;
            y = Y;
            width = 100;
            height = 20;
            _off = new FlxSprite().createGraphic((int)width, (int)height, new Color(0x7f, 0x7f, 0x7f));
            _off.solid = false;
            add(_off, true);
            _on = new FlxSprite().createGraphic((int)width, (int)height, Color.White);
            _on.solid = false;
            add(_on, true);
            _offT = null;
            _onT = null;
            _callback = Callback;
            _onToggle = false;
            _pressed = false;
            _initialized = false;
            _sf = Vector2.Zero;
            pauseProof = false;
            _controllerButtonIndex = Button;

            //_controllerButton = new FlxSprite((int)width + 5, 0);
            //_controllerButton.loadGraphic(FlxG.Content.Load<Texture2D>("buttons/BP3_SSTRIP_32"), true, false, 31, 32);
            //_controllerButton.width = 29;
            //_controllerButton.height = 30;
            //_controllerButton.offset.X = 1;
            //_controllerButton.offset.Y = 1;
            //_controllerButton.addAnimation("frame", new int[] {Button});
            //_controllerButton.play("frame");
            //_controllerButton.solid = false;
            //add(_controllerButton, true);
        }

        /// <summary>
        /// Set your own image as the button background.
        /// </summary>
        /// <param name="Image">A FlxSprite object to use for the button background.</param>
        /// <param name="ImageHighlight">A FlxSprite object to use for the button background when highlighted (optional).</param>
        /// <returns>This FlxButton instance (nice for chaining stuff together, if you're into that).</returns>
        public FlxButton loadGraphic(FlxSprite Image, FlxSprite ImageHighlight)
        {
            _off = replace(_off, Image) as FlxSprite;
            if (ImageHighlight == null)
            {
                if (_on != _off)
                    remove(_on);
                _on = _off;
            }
            else
                _on = replace(_on, ImageHighlight) as FlxSprite;
            _on.solid = _off.solid = false;
            _off.scrollFactor = scrollFactor;
            _on.scrollFactor = scrollFactor;
            width = _off.width;
            height = _off.height;
            refreshHulls();
            return this;
        }

        /// <summary>
        /// Add a text label to the button.
        /// </summary>
        /// <param name="Text">A FlxText object to use to display text on this button (optional).</param>
        /// <param name="TextHighlight">A FlxText object that is used when the button is highlighted (optional).</param>
        /// <returns>This FlxButton instance (nice for chaining stuff together, if you're into that).</returns>
        public FlxButton loadText(FlxText Text, FlxText TextHighlight)
        {
            if (Text != null)
            {
                if (_offT == null)
                {
                    _offT = Text;
                    add(_offT);
                }
                else
                    _offT = replace(_offT, Text) as FlxText;
            }
            if (TextHighlight == null)
                _onT = _offT;
            else
            {
                if (_onT == null)
                {
                    _onT = TextHighlight;
                    add(_onT);
                }
                else
                    _onT = replace(_onT, TextHighlight) as FlxText;
            }
            _offT.scrollFactor = scrollFactor;
            _onT.scrollFactor = scrollFactor;
            return this;
        }


        /// <summary>
        /// Called by the game loop automatically, handles mouseover and click detection.
        /// </summary>
        override public void update()
        {
            _counter += FlxG.elapsed;

            if (!_initialized)
            {
                if (FlxG.state == null) return;
                FlxG.mouse.addMouseListener(onMouseUp);
                _initialized = true;
            }



            base.update();

            //if (_controllerButtonIndex != -1)
            //{
            //    PlayerIndex pi;

            //    if (FlxG.gamepads.isNewButtonRelease(Buttons.A, FlxG.controllingPlayer, out pi) && _controllerButtonIndex == 0 && on && _counter > 0.5f)
            //    {
            //        if (!exists || !visible || !active || !FlxG.gamepads.isNewButtonRelease(Buttons.A, FlxG.controllingPlayer, out pi) || (FlxG.pause && !pauseProof) || (_callback == null)) return;
            //        Console.WriteLine("calling back from controller press ");

            //        _callback();
            //        destroy();
            //        return;
            //    }
            //    if (FlxG.gamepads.isNewButtonRelease(Buttons.B, FlxG.controllingPlayer, out pi) && _controllerButtonIndex == 1 && on && _counter > 0.5f)
            //    {
            //        if (!exists || !visible || !active || (FlxG.pause && !pauseProof) || (_callback == null)) return;
            //        _callback();
            //    }
            //    if (FlxG.gamepads.isNewButtonRelease(Buttons.X, FlxG.controllingPlayer, out pi) && _controllerButtonIndex == 2 && on && _counter > 0.5f)
            //    {
            //        if (!exists || !visible || !active || (FlxG.pause && !pauseProof) || (_callback == null)) return;
            //        _callback();
            //    }
            //    if (FlxG.gamepads.isNewButtonRelease(Buttons.Y, FlxG.controllingPlayer, out pi) && _controllerButtonIndex == 3 && on && _counter > 0.5f)
            //    {
            //        if (!exists || !visible || !active || (FlxG.pause && !pauseProof) || (_callback == null)) return;
            //        _callback();
            //    }
            //}



            visibility(false);
            if (overlapsPoint(FlxG.mouse.x, FlxG.mouse.y))
            {
                if (!FlxG.mouse.pressed())
                    _pressed = false;
                else if (!_pressed)
                    _pressed = true;
                visibility(!_pressed);
            }
            if (_onToggle) visibility(_off.visible);
        }

        /// <summary>
        /// Use this to toggle checkbox-style behavior.
        /// </summary>
        public bool on
        {
            get
            {
                return _onToggle;
            }
            set
            {
                _onToggle = value;
            }
        }

        /// <summary>
        /// Called by the game state when state is changed (if this object belongs to the state)
        /// </summary>
        override public void destroy()
        {
            if (FlxG.mouse != null)
                FlxG.mouse.removeMouseListener(onMouseUp);
        }

        /// <summary>
        /// Render
        /// </summary>
        /// <param name="spriteBatch"></param>
        override public void render(SpriteBatch spriteBatch)
        {
            base.render(spriteBatch);
            if ((_off != null) && _off.exists && _off.visible) _off.render(spriteBatch);
            if ((_on != null) && _on.exists && _on.visible) _on.render(spriteBatch);
            if (_offT != null)
            {
                if ((_offT != null) && _offT.exists && _offT.visible) _offT.render(spriteBatch);
                if ((_onT != null) && _onT.exists && _onT.visible) _onT.render(spriteBatch);
            }
        }

        /// <summary>
        /// Internal function for handling the visibility of the off and on graphics.
        /// </summary>
        /// <param name="On">Whether the button should be on or off.</param>
        protected void visibility(bool On)
        {
            if (On)
            {
                _off.visible = false;
                if (_offT != null) _offT.visible = false;
                _on.visible = true;
                if (_onT != null) _onT.visible = true;
            }
            else
            {
                _on.visible = false;
                if (_onT != null) _onT.visible = false;
                _off.visible = true;
                if (_offT != null) _offT.visible = true;
            }
        }

        /// <summary>
        /// Internal function for handling the actual callback call (for UI thread dependent calls like <code>FlxU.openURL()</code>).
        /// </summary>
        /// <param name="Sender">Sender</param>
        /// <param name="MouseEvent">Mouse Event</param>
        private void onMouseUp(object Sender, FlxMouseEvent MouseEvent)
        {

            if (!exists || !visible || !active || !FlxG.mouse.justReleased() || (FlxG.pause && !pauseProof) || (_callback == null)) return;
            if (overlapsPoint(FlxG.mouse.x, FlxG.mouse.y) && _counter > 0.5f)
            {
                Console.WriteLine("calling back from mouse press");
                on = true;
                _callback();
            }

            


        }
        /// <summary>
        /// returns the position of the game pad position.
        /// </summary>
        /// <returns></returns>
        public Vector2 getGamepadButtonPosition()
        {
            return new Vector2(_controllerButton.x, _controllerButton.y);
        }

    }
}
