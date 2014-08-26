using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{
    /// <summary>
    /// Helper class to check for an input across keyboard and gamepad.
    /// </summary>
    public class FlxControl
    {
        /// <summary>
        /// Is left pressed? (A, Left Arrow, D-Pad Left, Left Thumbstick Left);
        /// </summary>
        public static bool LEFT
        {
            get { return FlxG.keys.A || FlxG.keys.LEFT || FlxG.gamepads.isButtonDown(Buttons.DPadLeft) || FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickLeft); }
        }

        /// <summary>
        /// Is left just pressed? (A, Left Arrow, D-Pad Left, Left Thumbstick Left);
        /// </summary>
        public static bool LEFTJUSTPRESSED
        {
            get { return FlxG.keys.justPressed(Keys.A) || FlxG.keys.justPressed(Keys.Left) || FlxG.gamepads.isNewButtonPress(Buttons.DPadLeft) || FlxG.gamepads.isNewButtonPress(Buttons.LeftThumbstickLeft); }
        }

        /// <summary>
        /// Is Right pressed? (D, Right Arrow, D-Pad Right, Left Thumbstick Right);
        /// </summary>
        public static bool RIGHT
        {
            get { return FlxG.keys.D || FlxG.keys.RIGHT || FlxG.gamepads.isButtonDown(Buttons.DPadRight) || FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickRight); }
        }

        /// <summary>
        /// Is Right just pressed? (D, Right Arrow, D-Pad Right, Left Thumbstick Right);
        /// </summary>
        public static bool RIGHTJUSTPRESSED
        {
            get { return FlxG.keys.justPressed(Keys.D) || FlxG.keys.justPressed(Keys.Right) || FlxG.gamepads.isNewButtonPress(Buttons.DPadRight) || FlxG.gamepads.isNewButtonPress(Buttons.LeftThumbstickRight); }
        }

        /// <summary>
        /// Is Up pressed? (W, Up Arrow, D-Pad Up, Left Thumbstick Up);
        /// </summary>
        public static bool UP
        {
            get { return FlxG.keys.W || FlxG.keys.UP || FlxG.gamepads.isButtonDown(Buttons.DPadUp) || FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickUp); }
        }

        /// <summary>
        /// Is Up just pressed? (W, Up Arrow, D-Pad Up, Left Thumbstick Up);
        /// </summary>
        public static bool UPJUSTPRESSED
        {
            get { return FlxG.keys.justPressed(Keys.W) || FlxG.keys.justPressed(Keys.Up) || FlxG.gamepads.isNewButtonPress(Buttons.DPadUp) || FlxG.gamepads.isNewButtonPress(Buttons.LeftThumbstickUp); }
        }

        /// <summary>
        /// Is Down pressed? (S, Down Arrow, D-Pad Down, Left Thumbstick Down);
        /// </summary>
        public static bool DOWN
        {
            get { return FlxG.keys.S || FlxG.keys.DOWN || FlxG.gamepads.isButtonDown(Buttons.DPadDown) || FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickDown); }
        }
        /// <summary>
        /// Is Down just pressed? (S, Down Arrow, D-Pad Down, Left Thumbstick Down);
        /// </summary>
        public static bool DOWNJUSTPRESSED
        {
            get { return FlxG.keys.justPressed(Keys.S) || FlxG.keys.justPressed(Keys.Down) || FlxG.gamepads.isNewButtonPress(Buttons.DPadDown) || FlxG.gamepads.isNewButtonPress(Buttons.LeftThumbstickDown); }
        }

        /// <summary>
        /// Action is defined as the following buttons:
        /// Keyboard - X, Enter, SPACE
        /// Joystick - A, Start
        /// </summary>
        public static bool ACTION
        {
            get { return FlxG.keys.N || 
                FlxG.keys.X || 
                FlxG.keys.ENTER || 
                FlxG.keys.SPACE || 
                FlxG.gamepads.isButtonDown(Buttons.A) || 
                FlxG.gamepads.isButtonDown(Buttons.Start); }
        }

        /// <summary>
        /// Action just pressed, is defined as the following buttons:
        /// Keyboard - X, Enter, SPACE
        /// Joystick - A, Start
        /// </summary>
        public static bool ACTIONJUSTPRESSED
        {
            get { return FlxG.keys.justPressed(Keys.N) ||
                FlxG.keys.justPressed(Keys.X) || 
                FlxG.keys.justPressed(Keys.Enter) ||
                FlxG.keys.justPressed(Keys.Space) || 
                FlxG.gamepads.isNewButtonPress(Buttons.A) || 
                FlxG.gamepads.isNewButtonPress(Buttons.Start); }
        }

        /// <summary>
        /// Cancel is defined as:
        /// Keyboard: Escape
        /// GamePad: Back, B
        /// </summary>
        public static bool CANCEL
        {
            get { return FlxG.keys.ESCAPE || 
                FlxG.gamepads.isButtonDown(Buttons.Back) || 
                FlxG.gamepads.isButtonDown(Buttons.B); }
        }

        /// <summary>
        /// Cancel just pressed, is defined as:
        /// Keyboard: Escape
        /// GamePad: Back, B
        /// </summary>
        public static bool CANCELJUSTPRESSED
        {
            get { return FlxG.keys.justPressed(Keys.Escape) || 
                FlxG.gamepads.isNewButtonPress(Buttons.Back) || 
                FlxG.gamepads.isNewButtonPress(Buttons.B); }
        }


    }
}
