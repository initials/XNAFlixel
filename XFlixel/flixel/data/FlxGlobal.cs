using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
