using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{
    /// <summary>
    /// FlxGlobals stores a bunch of constants
    /// </summary>
    public class FlxGlobal
    {
        /// <summary>
        /// Allows playstates to see what the current cheat to run is.
        /// </summary>
        public static string cheatString;

        /// <summary>
        /// Resolution of the PS Vita.
        /// </summary>
        public static Vector2 RESOLUTION_PSVITA = new Vector2(960, 544);
        
        /// <summary>
        /// Resolution of the OUYA
        /// </summary>
        public static Vector2 RESOLUTION_OUYA = new Vector2(1920, 1080);

        /// <summary>
        /// Resolution of the iPhone. Includes iPhone, iPhone 3G, iPhone 3GS
        /// </summary>
        public static Vector2 RESOLUTION_IPHONE3GS = new Vector2(640, 480);

        /// <summary>
        /// Includes iPhone 4, 4s
        /// </summary>
        public static Vector2 RESOLUTION_IPHONE4 = new Vector2(960, 640);

        /// <summary>
        /// Includes iPhone 5, 5c and 5s
        /// </summary>
        public static Vector2 RESOLUTION_IPHONE5 = new Vector2(1136, 640);

        public static Vector2 RESOLUTION_IPHONE6 = new Vector2(1334, 750);

        public static Vector2 RESOLUTION_IPHONE6PLUS = new Vector2(1920, 1080);

        /// <summary>
        /// Allows the FlxConsole to run commands.
        /// </summary>
        /// <param name="Cheat">Name of the cheat you want to run.</param>
        public static void runCheat(string Cheat)
        {
            if (Cheat.StartsWith("whatisgame")) FlxG.log("Four Chambers");
            else if (Cheat.StartsWith("bigmoney")) FlxG.score += 20000;
            else if (Cheat.StartsWith("nobugs")) FlxG.debug = false;
            else if (Cheat == "bounds") FlxG.showBounds = true;
            else if (Cheat == "nobounds") FlxG.showBounds = false;
            
            cheatString = Cheat;

        }

    }
}
