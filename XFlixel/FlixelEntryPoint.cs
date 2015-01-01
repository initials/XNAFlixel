#region File Description
//-----------------------------------------------------------------------------
// Flixel for XNA.
// Original repo : https://github.com/StAidan/X-flixel
// Extended and edited repo : https://github.com/initials/XNAMode
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;

namespace XNAMode
{
    /// <summary>
    /// Flixel enters here.
    /// <code>FlxFactory</code> refers to it as the "masterclass".
    /// </summary>
    /// 
    /*
     * AUTO ZIP CODE.
     * 
        cd C:\Users\Shane\Documents\Visual Studio 2010\Projects\XNAMode\Mode\Mode\bin\x86
        C:\_Files\programs\7-Zip\7z a -tzip Mode.zip Release\ -r
     * 
     * Resolutions:
     * PS VITA 960 × 544
    */

    public class FlixelEntryPoint : FlxGame
    {
        public FlixelEntryPoint(Game game)
            : base(game)
        {
			Console.WriteLine ("Flixel entry points .cs ");
            
            int w = 640/2;
            int h = 360/2;
            FlxG.zoom = 4;
            FlxG.debug = true;

#if DEBUG
            FlxG.debug = true;
#else
            FlxG.debug=false;
#endif

            initGame(w, h, new org.flixel.examples.TestState(), new Color(15, 15, 15), true, new Color(5, 5, 5));
            
        }
    }
}
