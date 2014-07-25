using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

namespace org.flixel
{
    /// <summary>
    /// This state will show how to use the Debug menu and the Cheat system.
    /// </summary>
    public class CheatState : FlxState
    {
        override public void create()
        {
            base.create();

            FlxG.showHud();
            FlxG.setHudText(1, "Press [`] to show the debug console.\nYou must build with the Debug Solution Configuration.\n\nIn the console press TAB to bring up the cheat menu.\n\nTry typing in the cheat code \"cheatcode\"");
            FlxG.setHudGamepadButton(FlxHud.TYPE_KEYBOARD, FlxHud.Keyboard_Tilda, (FlxG.width*2) - 100, 10);
            FlxG.setHudGamepadButton(FlxHud.TYPE_KEYBOARD_DIRECTION, FlxHud.Keyboard_Tab, (FlxG.width * 2) - 100, 100);

        }

        override public void update()
        {

            if (FlxGlobal.cheatString == "cheatcode")
            {
                FlxG._game._console.visible = false;
                FlxG.showHud();

                FlxG.setHudText(1, "Awesome Cheat Activated!");
            }

            base.update();
        
        }
    }
}
